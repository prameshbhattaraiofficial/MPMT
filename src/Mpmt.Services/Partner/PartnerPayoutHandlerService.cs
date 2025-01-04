using Mpmt.Core.Domain.Payout;
using Mpmt.Core.Dtos.BankLoad;
using Mpmt.Core.Dtos.Payout;
using Mpmt.Core.Dtos.WalletLoad.MyPay;
using Mpmt.Core.Dtos;
using Mpmt.Core.Models.Mail;
using System.Net;
using Mpmt.Services.Logging;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Mpmt.Services.Extensions;
using Mpmt.Services.Services.WalletLoadApi.MyPay;
using Mpmt.Data.Repositories.Payout;
using Mpmt.Services.Services.BankLoadApi;
using Mpmt.Services.Services.MailingService;
using Mpmt.Core.Domain.Admin.Reports;
using Mpmt.Core.Dtos.AgentFundTransfer;
using Mpmt.Core.Dtos.Transaction;
using System.Net.Mail;
using System.Reflection.Metadata.Ecma335;

namespace Mpmt.Services.Partner
{
    public class PartnerPayoutHandlerService : IPartnerPayoutHandlerService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMyPayWalletLoadApiService _myPayWalletLoadApiService;
        private readonly IMyPayBankLoadApiService _myPayBankLoadApiService;
        private readonly IMailService _mailService;
        private readonly IExceptionLogger _exceptionLogger;
        private readonly IVendorApiLogger _vendorApiLogger;
        private readonly IPayoutRepository _payoutRepository;
        private readonly IMyPayWalletLoadRepository _myPayWalletLoadRepository;

        public PartnerPayoutHandlerService(
            // TODO: move below services to service layer
            IHttpContextAccessor httpContextAccessor,
            IVendorApiLogger vendorApiLogger,
            IPayoutRepository payoutRepository,
            IMyPayWalletLoadRepository myPayWalletLoadRepository,
            IMyPayWalletLoadApiService myPayWalletLoadApiService,
            IMyPayBankLoadApiService myPayBankLoadApiService,
            IMailService mailService,
            IExceptionLogger exceptionLogger)
        {
            _httpContextAccessor = httpContextAccessor;
            _myPayWalletLoadApiService = myPayWalletLoadApiService;
            _myPayBankLoadApiService = myPayBankLoadApiService;
            _mailService = mailService;
            _exceptionLogger = exceptionLogger;
            _vendorApiLogger = vendorApiLogger;
            _payoutRepository = payoutRepository;
            _myPayWalletLoadRepository = myPayWalletLoadRepository;
        }

        public async Task<MpmtResult> HandlePayoutToWalletAsync(AddTransactionResultDetails details)
        {
            // set partner transactionId as a tracker Id
            _vendorApiLogger.SetLogContext(trackerId: details.TransactionId);

            var result = new MpmtResult();
            var userMetaInfo = new
            {
                AgentCode = AgentCodes.MyPay,
                PartnerCode = _httpContextAccessor.HttpContext?.User.FindFirstValue("PartnerCode"),
                UserType = _httpContextAccessor.HttpContext?.GetUserType(),
                LoggedInUser = _httpContextAccessor.HttpContext?.GetUserName() ?? _httpContextAccessor.HttpContext?.GetUserEmail(),
                IpAddress = _httpContextAccessor.HttpContext?.GetIpAddress(),
                DeviceId = _httpContextAccessor.HttpContext?.GetUserAgentDevicePlatform(),
            };

            var walletValidationReqObj = new MyPayValidateWalletUserDto
            {
                WalletNumber = details.WalletNumber,
                Reference = Guid.NewGuid().ToString("N"),
            };

            var (walletValidationStatusCode, walletValidationRes) = await _myPayWalletLoadApiService
                .ValidateWalletUserAsync(walletValidationReqObj);

            if (walletValidationRes.Message.ToUpper() == "SUCCESS")
            {
                if (!walletValidationRes.FullName.Equals(details.WalletHolderName, StringComparison.OrdinalIgnoreCase))
                {
                    walletValidationRes.ResponseMessage = "Wallet Registered Name does not match with given Wallet Number!";
                    return result;
                }
            }

            if (walletValidationRes is null)
                return result;

            var validationLogParam = new MyPayValidateWalletUserLogParam
            {
                RemitTransactionId = details.TransactionId,
                AccountStatus = walletValidationRes.AccountStatus,
                ContactNumber = walletValidationRes.ContactNumber,
                Message = walletValidationRes.Message,
                ResponseMessage = walletValidationRes.ResponseMessage,
                IsAccountValidated = walletValidationRes.IsAccountValidated,
                ResponseCode = walletValidationRes.ReponseCode.ToString(),
                Status = walletValidationRes.status,
                AgentCode = userMetaInfo.AgentCode,
                UserType = userMetaInfo.UserType,
                LoggedInUser = userMetaInfo.LoggedInUser
            };
            var validationLogStatus = await _myPayWalletLoadRepository.LogWalletUserValidationAsync(validationLogParam);

            if (walletValidationStatusCode != HttpStatusCode.OK || walletValidationRes is null)
                return result;

            if (validationLogStatus.StatusCode != 200)
                return result;

            if (!(!string.IsNullOrWhiteSpace(walletValidationRes.AccountStatus) &&
                walletValidationRes.AccountStatus.Equals("Active", StringComparison.OrdinalIgnoreCase) &&
                walletValidationRes.IsAccountValidated))
                return result;

            var paramGetPayoutReferenceInfo = new GetPayoutReferenceInfoDto
            {
                RemitTransactionId = details.TransactionId,
                PaymentType = details.PaymentType,
                DeviceId = userMetaInfo.DeviceId,
                AgentCode = userMetaInfo.AgentCode,
                UserType = userMetaInfo.UserType,
                LoggedInUser = userMetaInfo.LoggedInUser,
                IpAddress = userMetaInfo.IpAddress
            };
            var (_, payoutRefInfo) = await _payoutRepository.GetPayoutReferenceInfoAsync(paramGetPayoutReferenceInfo);

            if (payoutRefInfo is null)
                return result;

            /* set PayoutReferenceNo as TransactionId in log.
             * PayoutReferenceNo => Transaction Id (we can say sub-transaction Id) associated with parent tracker Id (Remit Transaction Id) in the database */
            _vendorApiLogger.SetLogContext(transactionId: payoutRefInfo.PayoutReferenceNo);

            // now we have reference, and we proceed to payout
            var payoutReqObj = new MyPayWalletPayoutDto
            {
                Amount = details.PayableAmount.ToString(),
                ContactNumber = details.WalletNumber,
                Remarks = $"Payout amt. {details.PayableAmount} to {details.WalletNumber}.",
                Reference = payoutRefInfo.PayoutReferenceNo
            };
            var (_, walletPayoutRes) = await _myPayWalletLoadApiService.WalletPayoutAsync(payoutReqObj);

            // if unable to get wallet payout response, check payout txn status once and update accordingly
            if (walletPayoutRes is null)
            {
                var payoutTxnCheckReqObj = new MyPayWalletPayoutCheckTransactionStatusDto
                {
                    TransactionReference = payoutRefInfo.PayoutReferenceNo,
                    Reference = Guid.NewGuid().ToString("N")
                };
                var (_, txnCheckRes) = await _myPayWalletLoadApiService.CheckTransactionStatusAsync(payoutTxnCheckReqObj);
                if (txnCheckRes is null)
                    return result;

                var payoutTxnLogStatusObj = new MyPayWalletLoadPayoutTxnStatusLogParam
                {
                    PayoutRefereneNo = payoutRefInfo.PayoutReferenceNo,
                    AgentStatusCode = txnCheckRes.StatusCode.ToString(),
                    ResponseCode = txnCheckRes.ReponseCode.ToString(),
                    Message = txnCheckRes.Message,
                    ResponseMessage = txnCheckRes.ResponseMessage,
                    Status = txnCheckRes.Status,
                    TransactionStatus = txnCheckRes.TransactionStatus,
                    Details = txnCheckRes.Details,
                    UserType = userMetaInfo.UserType,
                    LoggedInUser = userMetaInfo.LoggedInUser
                };
                var payoutTxnLogStatus = await _myPayWalletLoadRepository.LogWalletPayoutTxnStatusAsync(payoutTxnLogStatusObj);

                // TODO: add further more logics here if required

                return result;
            }

            var payoutLogParam = new MyPayWalletPayoutLogParam
            {
                PayoutRefereneNo = payoutRefInfo.PayoutReferenceNo,
                AgentTransactionId = walletPayoutRes.MerchantWallet_TransactionId,
                Message = walletPayoutRes.Message,
                ResponseMessage = walletPayoutRes.ResponseMessage,
                ResponseCode = walletPayoutRes.ReponseCode.ToString(),
                Status = walletPayoutRes.status,
                Details = walletPayoutRes.Details,
                UserType = userMetaInfo.UserType,
                LoggedInUser = userMetaInfo.LoggedInUser,
            };
            var payoutLogStatus = await _myPayWalletLoadRepository.LogRemitToWalletPayoutAsync(payoutLogParam);

            if (payoutLogStatus.StatusCode != 200)
                return result;

            if (walletPayoutRes is null || string.IsNullOrWhiteSpace(walletPayoutRes.MerchantWallet_TransactionId))
                return result;

            // TODO: add further more logics here if required

            return result;
        }

        public async Task<MpmtResult> HandlePayoutToBankAsync(AddTransactionResultDetails details)
        {
            // set partner transactionId as a tracker Id
            _vendorApiLogger.SetLogContext(trackerId: details.TransactionId);

            var result = new MpmtResult();
            var userMetaInfo = new
            {
                AgentCode = AgentCodes.MyPay, // TODO: re-check this agent code
                UserType = _httpContextAccessor.HttpContext?.GetUserType(),
                LoggedInUser = _httpContextAccessor.HttpContext?.GetUserName() ?? _httpContextAccessor.HttpContext?.GetUserEmail(),
                IpAddress = _httpContextAccessor.HttpContext?.GetIpAddress(),
                DeviceId = _httpContextAccessor.HttpContext?.GetUserAgentDevicePlatform(),
            };
            try
            {
                //var bankPayoutThread = new Thread(async () =>
                //{
                    #region Check validation
                    // Check Bank Account Validation
                    var banklValidationReqObj = new MyPayValidateBankUserDto
                    {
                        AccountName = details.BankAccountHolderName,
                        AccountNumber = details.BankAccountNumber,
                        BankCode = details.BankCode,
                        Amount = details.PayableAmount,
                        Reference = Guid.NewGuid().ToString("N"),
                    };
                    var (bankValidationStatusCode, bankValidationRes) = await _myPayBankLoadApiService
                       .ValidateBankUserAsync(banklValidationReqObj);

                    if (bankValidationRes is null)
                        return result;

                    var validationLogParam = new MyPayValidateWalletUserLogParam //MyPayValidateBankUserLogParam
                    {
                        RemitTransactionId = details.TransactionId,
                        AccountStatus = bankValidationRes.AccountStatus,
                        Message = bankValidationRes.Message,
                        ResponseMessage = bankValidationRes.responseMessage,
                        IsAccountValidated = bankValidationRes.Message.ToUpper() == "SUCCESS" ? true : bankValidationRes.IsAccountValidated,
                        ResponseCode = bankValidationRes.ReponseCode.ToString(),
                        Status = bankValidationRes.status,
                        AgentCode = userMetaInfo.AgentCode,
                        UserType = userMetaInfo.UserType,
                        LoggedInUser = userMetaInfo.LoggedInUser
                    };
                    var validationLogStatus = await _myPayWalletLoadRepository.LogWalletUserValidationAsync(validationLogParam);
                    if (bankValidationStatusCode != HttpStatusCode.Created || bankValidationRes is null)
                        return result;

                    if (bankValidationRes.ResponseCode == "000" || bankValidationRes.ResponseCode == "999")
                    {
                        if (Int32.TryParse(bankValidationRes.MatchPercentate, out var matchPercentage))
                        {
                            if (matchPercentage > 80)
                            {
                                // till here wallet account is valid           
                                if (validationLogStatus.StatusCode != 200)
                                    return result;

                                var paramGetPayoutReferenceInfo = new GetPayoutReferenceInfoDto
                                {
                                    RemitTransactionId = details.TransactionId,
                                    PaymentType = details.PaymentType,
                                    DeviceId = userMetaInfo.DeviceId,
                                    AgentCode = userMetaInfo.AgentCode,
                                    UserType = userMetaInfo.UserType,
                                    LoggedInUser = userMetaInfo.LoggedInUser,
                                    IpAddress = userMetaInfo.IpAddress
                                };
                                var (_, payoutRefInfo) = await _payoutRepository.GetPayoutReferenceInfoAsync(paramGetPayoutReferenceInfo);

                                if (payoutRefInfo is null)
                                    return result;

                                _vendorApiLogger.SetLogContext(transactionId: payoutRefInfo.PayoutReferenceNo);

                                var payoutReqObj = new MyPayBankPayoutDto
                                {
                                    Amount = details.PayableAmount.ToString(),
                                    BankId = details.BankCode,
                                    AccountHolderName = details.BankAccountHolderName,
                                    AccountNumber = details.BankAccountNumber,
                                    BankName = details.BankName,
                                    Remarks = $"MPMT-{details.SenderName}",
                                    Reference = payoutRefInfo.PayoutReferenceNo
                                };
                                var (_, bankPayoutRes) = await _myPayBankLoadApiService.BankPayoutAsync(payoutReqObj);
                                // now we have reference, and we proceed to payout

                                var payoutLogParam = new MyPayWalletPayoutLogParam
                                {
                                    PayoutRefereneNo = payoutRefInfo.PayoutReferenceNo,
                                    AgentTransactionId = bankPayoutRes.TransactionUniqueId,
                                    Message = bankPayoutRes.Message,
                                    ResponseMessage = bankPayoutRes.responseMessage,
                                    ResponseCode = bankPayoutRes.ReponseCode.ToString(),
                                    Status = bankPayoutRes.status,
                                    Details = bankPayoutRes.Details,
                                    UserType = userMetaInfo.UserType,
                                    LoggedInUser = userMetaInfo.LoggedInUser,
                                };
                                var payoutLogStatus = await _myPayWalletLoadRepository.LogRemitToWalletPayoutAsync(payoutLogParam);

                                if (payoutLogStatus.StatusCode != 200)
                                    return result;

                                if (bankPayoutRes is null || string.IsNullOrWhiteSpace(bankPayoutRes.TransactionUniqueId))
                                    return result;
                            }
                            else
                            {
                                return result;
                            }
                        }
                        else
                        {
                            return result;
                        }
                    }
                    #endregion

                    /* set PayoutReferenceNo as TransactionId in log.
                     * PayoutReferenceNo => Transaction Id (we can say sub-transaction Id) associated with parent tracker Id (Remit Transaction Id) in the database */

                    return result;
                //})
                //{
                //    Priority = ThreadPriority.AboveNormal
                //};
                //bankPayoutThread.Start();
            }
            catch (Exception ex)
            {
                await _exceptionLogger.LogAsync(ex);
                return result;
            }

            //return result;
        }

        #region Admin Payout Handler
        public async Task<MpmtResult> HandleByAdminPayoutToBankAsync(TransactionDetailsAdmin txnDetails)
        {

            #region OLD Working code
            //// set partner transactionId as a tracker Id
            _vendorApiLogger.SetLogContext(trackerId: txnDetails.TransactionId);

            var result = new MpmtResult();
            var userMetaInfo = new
            {
                AgentCode = AgentCodes.MyPay, // TODO: re-check this agent code
                UserType = _httpContextAccessor.HttpContext?.GetUserType(),
                LoggedInUser = _httpContextAccessor.HttpContext?.GetUserName() ?? _httpContextAccessor.HttpContext?.GetUserEmail(),
                IpAddress = _httpContextAccessor.HttpContext?.GetIpAddress(),
                DeviceId = _httpContextAccessor.HttpContext?.GetUserAgentDevicePlatform(),
            };

            #region Check Comliance

            #endregion

            #region Check validation
            //Check Bank Account Validation
            var banklValidationReqObj = new MyPayValidateBankUserDto
            {
                AccountName = txnDetails.AccountHolderName,
                AccountNumber = txnDetails.AccountNumber,
                BankCode = txnDetails.BankCode,
                Amount = txnDetails.SendingAmount,
                Reference = Guid.NewGuid().ToString("N"),
            };
            var (bankValidationStatusCode, bankValidationRes) = await _myPayBankLoadApiService
               .ValidateBankUserAsync(banklValidationReqObj);

            if (bankValidationRes is null)
                return result;
            if (bankValidationRes.ResponseCode == "000" || bankValidationRes.ResponseCode == "999")
            {
                //Int32.TryParse(bankValidationRes.MatchPercentate,out var matchPercentate)
                if (Int32.TryParse(bankValidationRes.MatchPercentate, out var matchPercentage))
                {
                    if (matchPercentage > 80)
                    {
                        var validationLogParam = new MyPayValidateWalletUserLogParam //MyPayValidateBankUserLogParam
                        {
                            RemitTransactionId = txnDetails.TransactionId,
                            AccountStatus = bankValidationRes.AccountStatus,
                            Message = bankValidationRes.Message,
                            ResponseMessage = bankValidationRes.responseMessage,
                            IsAccountValidated = bankValidationRes.Message.ToUpper() == "SUCCESS" ? true : bankValidationRes.IsAccountValidated,
                            ResponseCode = bankValidationRes.ReponseCode.ToString(),
                            Status = bankValidationRes.status,
                            AgentCode = userMetaInfo.AgentCode,
                            UserType = userMetaInfo.UserType,
                            LoggedInUser = userMetaInfo.LoggedInUser
                        };

                        var validationLogStatus = await _myPayWalletLoadRepository.LogWalletUserValidationAsync(validationLogParam);
                        if (bankValidationStatusCode != HttpStatusCode.Created || bankValidationRes is null)
                        {
                            result.AddError(bankValidationRes.ReponseCode, bankValidationRes.responseMessage);
                            return result;
                        }

                        if (validationLogStatus.StatusCode != 200)
                        {
                            result.AddError(bankValidationRes.ReponseCode, bankValidationRes.responseMessage);
                            return result;
                        }
                        #endregion

                        // till here wallet account is valid
                        var paramGetPayoutReferenceInfo = new GetPayoutReferenceInfoDto
                        {
                            RemitTransactionId = txnDetails.TransactionId,
                            PaymentType = txnDetails.PaymentType,
                            DeviceId = userMetaInfo.DeviceId,
                            AgentCode = userMetaInfo.AgentCode,
                            UserType = userMetaInfo.UserType,
                            LoggedInUser = userMetaInfo.LoggedInUser,
                            IpAddress = userMetaInfo.IpAddress
                        };
                        var (_, payoutRefInfo) = await _payoutRepository.GetPayoutReferenceInfoAsync(paramGetPayoutReferenceInfo);

                        if (payoutRefInfo is null)
                            return result;
                        // now we have reference, and we proceed to payout
                        var payoutReqObj = new MyPayBankPayoutDto
                        {
                            Amount = txnDetails.NetRecievingAmountNPR.ToString(),
                            BankId = txnDetails.BankCode,
                            AccountHolderName = txnDetails.AccountHolderName,
                            AccountNumber = txnDetails.AccountNumber,
                            BankName = txnDetails.BankName,
                            Remarks = $"MPMT-{txnDetails.SenderFirstName} {txnDetails.SenderLastName}",
                            Reference = payoutRefInfo.PayoutReferenceNo
                        };
                        var (_, bankPayoutRes) = await _myPayBankLoadApiService.BankPayoutAsync(payoutReqObj);

                        var payoutLogParam = new MyPayWalletPayoutLogParam
                        {
                            PayoutRefereneNo = payoutRefInfo.PayoutReferenceNo,
                            AgentTransactionId = bankPayoutRes.TransactionUniqueId,
                            Message = bankPayoutRes.Message,
                            ResponseMessage = bankPayoutRes.responseMessage,
                            ResponseCode = bankPayoutRes.ReponseCode.ToString(),
                            Status = bankPayoutRes.status,
                            Details = bankPayoutRes.Details,
                            UserType = userMetaInfo.UserType,
                            LoggedInUser = userMetaInfo.LoggedInUser,
                        };
                        var payoutLogStatus = await _myPayWalletLoadRepository.LogRemitToWalletPayoutAsync(payoutLogParam);

                        if (payoutLogStatus.StatusCode != 200)
                        {
                            result.AddErrors(bankPayoutRes.ReponseCode, bankPayoutRes.responseMessage);
                            return result;
                        }
                        if (bankPayoutRes.ReponseCode == 1 && !string.IsNullOrEmpty(bankPayoutRes.responseMessage))
                        {
                            result.AddSuccess(bankPayoutRes.ReponseCode, bankPayoutRes.responseMessage);
                            return result;
                        }
                        if (bankPayoutRes is null || string.IsNullOrWhiteSpace(bankPayoutRes.TransactionUniqueId))
                        {
                            result.AddErrors(bankPayoutRes.ReponseCode, bankPayoutRes.responseMessage);
                            return result;
                        }
                    }
                    else
                    {
                        result.AddError(bankValidationRes.ReponseCode, bankValidationRes.responseMessage);
                        return result;
                    }

                }
                else
                {
                    result.AddError(bankValidationRes.ReponseCode, bankValidationRes.responseMessage);
                    return result;
                }

            }
            return result;

            #endregion 

        }
        public async Task<MpmtResult> HandleByAdminPayoutToWalletAsync(TransactionDetailsAdmin details)
        {
            // set partner transactionId as a tracker Id
            _vendorApiLogger.SetLogContext(trackerId: details.TransactionId);
            var result = new MpmtResult();
            var userMetaInfo = new
            {
                AgentCode = AgentCodes.MyPay,
                PartnerCode = _httpContextAccessor.HttpContext?.User.FindFirstValue("PartnerCode"),
                UserType = _httpContextAccessor.HttpContext?.GetUserType(),
                LoggedInUser = _httpContextAccessor.HttpContext?.GetUserName() ?? _httpContextAccessor.HttpContext?.GetUserEmail(),
                IpAddress = _httpContextAccessor.HttpContext?.GetIpAddress(),
                DeviceId = _httpContextAccessor.HttpContext?.GetUserAgentDevicePlatform(),
            };

            var walletValidationReqObj = new MyPayValidateWalletUserDto
            {
                WalletNumber = details.WalletNumber,
                Reference = Guid.NewGuid().ToString("N"),
            };
            var (walletValidationStatusCode, walletValidationRes) = await _myPayWalletLoadApiService
                .ValidateWalletUserAsync(walletValidationReqObj);


            if (walletValidationRes.Message.ToUpper() == "SUCCESS")
            {
                if (!walletValidationRes.FullName.Equals(details.WalletHolderName, StringComparison.OrdinalIgnoreCase))
                {
                    walletValidationRes.ResponseMessage = "Wallet Registered Name does not match with given Wallet Number!";
                    result.AddError(walletValidationRes.ReponseCode, walletValidationRes.ResponseMessage);
                    return result;
                }
            }


            if (walletValidationRes is null)
                return result;

            var validationLogParam = new MyPayValidateWalletUserLogParam
            {
                RemitTransactionId = details.TransactionId,
                AccountStatus = walletValidationRes.AccountStatus,
                ContactNumber = walletValidationRes.ContactNumber,
                Message = walletValidationRes.Message,
                ResponseMessage = walletValidationRes.ResponseMessage,
                IsAccountValidated = walletValidationRes.IsAccountValidated,
                ResponseCode = walletValidationRes.ReponseCode.ToString(),
                Status = walletValidationRes.status,
                AgentCode = userMetaInfo.AgentCode,
                UserType = userMetaInfo.UserType,
                LoggedInUser = userMetaInfo.LoggedInUser
            };
            var validationLogStatus = await _myPayWalletLoadRepository.LogWalletUserValidationAsync(validationLogParam);

            if (walletValidationStatusCode != HttpStatusCode.OK || walletValidationRes is null)
            {
                result.AddError(walletValidationRes.ReponseCode, walletValidationRes.ResponseMessage);
                return result;
            }

            if (validationLogStatus.StatusCode != 200)
            {
                result.AddError(walletValidationRes.ReponseCode, walletValidationRes.ResponseMessage);
                return result;
            }

            if (!(!string.IsNullOrWhiteSpace(walletValidationRes.AccountStatus) &&
                walletValidationRes.AccountStatus.Equals("Active", StringComparison.OrdinalIgnoreCase) &&
                walletValidationRes.IsAccountValidated))
                return result;

            var paramGetPayoutReferenceInfo = new GetPayoutReferenceInfoDto
            {
                RemitTransactionId = details.TransactionId,
                PaymentType = details.PaymentType,
                DeviceId = userMetaInfo.DeviceId,
                AgentCode = userMetaInfo.AgentCode,
                UserType = userMetaInfo.UserType,
                LoggedInUser = userMetaInfo.LoggedInUser,
                IpAddress = userMetaInfo.IpAddress
            };
            var (_, payoutRefInfo) = await _payoutRepository.GetPayoutReferenceInfoAsync(paramGetPayoutReferenceInfo);

            if (payoutRefInfo is null)
                return result;

            // now we have reference, and we proceed to payout
            var payoutReqObj = new MyPayWalletPayoutDto
            {
                Amount = details.NetRecievingAmountNPR.ToString(),
                ContactNumber = details.WalletNumber,
                Remarks = $"Payout amt. {details.NetRecievingAmountNPR} to {details.WalletNumber}.",
                Reference = payoutRefInfo.PayoutReferenceNo
            };
            var (_, walletPayoutRes) = await _myPayWalletLoadApiService.WalletPayoutAsync(payoutReqObj);

            // if unable to get wallet payout response, check payout txn status once and update accordingly
            if (walletPayoutRes is null)
            {
                var payoutTxnCheckReqObj = new MyPayWalletPayoutCheckTransactionStatusDto
                {
                    TransactionReference = payoutRefInfo.PayoutReferenceNo,
                    Reference = Guid.NewGuid().ToString("N")
                };
                var (_, txnCheckRes) = await _myPayWalletLoadApiService.CheckTransactionStatusAsync(payoutTxnCheckReqObj);
                if (txnCheckRes is null)
                    return result;

                var payoutTxnLogStatusObj = new MyPayWalletLoadPayoutTxnStatusLogParam
                {
                    PayoutRefereneNo = payoutRefInfo.PayoutReferenceNo,
                    AgentStatusCode = txnCheckRes.StatusCode.ToString(),
                    ResponseCode = txnCheckRes.ReponseCode.ToString(),
                    Message = txnCheckRes.Message,
                    ResponseMessage = txnCheckRes.ResponseMessage,
                    Status = txnCheckRes.Status,
                    TransactionStatus = txnCheckRes.TransactionStatus,
                    Details = txnCheckRes.Details,
                    UserType = userMetaInfo.UserType,
                    LoggedInUser = userMetaInfo.LoggedInUser
                };
                var payoutTxnLogStatus = await _myPayWalletLoadRepository.LogWalletPayoutTxnStatusAsync(payoutTxnLogStatusObj);

                // TODO: add further more logics here if required

                return result;
            }

            var payoutLogParam = new MyPayWalletPayoutLogParam
            {
                PayoutRefereneNo = payoutRefInfo.PayoutReferenceNo,
                AgentTransactionId = walletPayoutRes.MerchantWallet_TransactionId,
                Message = walletPayoutRes.Message,
                ResponseMessage = walletPayoutRes.ResponseMessage,
                ResponseCode = walletPayoutRes.ReponseCode.ToString(),
                Status = walletPayoutRes.status,
                Details = walletPayoutRes.Details,
                UserType = userMetaInfo.UserType,
                LoggedInUser = userMetaInfo.LoggedInUser,
            };
            var payoutLogStatus = await _myPayWalletLoadRepository.LogRemitToWalletPayoutAsync(payoutLogParam);

            if (payoutLogStatus.StatusCode != 200)
            {
                result.AddError(walletPayoutRes.ReponseCode, walletPayoutRes.ResponseMessage);
                return result;
            }
            if (walletValidationRes.ReponseCode == 1 && !string.IsNullOrEmpty(walletValidationRes.ResponseMessage))
            {
                result.AddSuccess(walletPayoutRes.ReponseCode, walletPayoutRes.ResponseMessage);
                return result;
            }
            if (walletPayoutRes is null || string.IsNullOrWhiteSpace(walletPayoutRes.MerchantWallet_TransactionId))
            {
                result.AddError(walletPayoutRes.ReponseCode, walletPayoutRes.ResponseMessage);
                return result;
            }
            // TODO: add further more logics here if required

            return result;
        }
        public async Task<MpmtResult> HandleByBulkTransactionToWalletAsync(BulkTransactionDetailsModel details, BulkTxnBaseModel requestModel)
        {
            // set partner transactionId as a tracker Id
            // _vendorApiLogger.SetLogContext(trackerId: details.TransactionId);
            var result = new MpmtResult();
            var userMetaInfo = new
            {
                AgentCode = AgentCodes.MyPay,
                PartnerCode = _httpContextAccessor.HttpContext?.User.FindFirstValue("PartnerCode"),
                UserType = _httpContextAccessor.HttpContext?.GetUserType(),
                LoggedInUser = _httpContextAccessor.HttpContext?.GetUserName() ?? _httpContextAccessor.HttpContext?.GetUserEmail(),
                IpAddress = _httpContextAccessor.HttpContext?.GetIpAddress(),
                DeviceId = _httpContextAccessor.HttpContext?.GetUserAgentDevicePlatform(),
            };
            var walletValidationReqObj = new MyPayValidateWalletUserDto
            {
                WalletNumber = details.WalletID,
                Reference = Guid.NewGuid().ToString("N"),
            };
            var (walletValidationStatusCode, walletValidationRes) = await _myPayWalletLoadApiService
                .ValidateWalletUserAsync(walletValidationReqObj);


            if (walletValidationRes.Message.ToUpper() == "SUCCESS")
            {
                if (!walletValidationRes.FullName.Equals(details.BeneficiaryName, StringComparison.OrdinalIgnoreCase))
                {
                    walletValidationRes.ResponseMessage = "Wallet Registered Name does not match with given Wallet Number!";
                    result.AddError(walletValidationRes.ReponseCode, walletValidationRes.ResponseMessage);
                    return result;
                }
            }

            if (walletValidationRes is null)
                return result;

            var validationLogParam = new MyPayValidateWalletUserLogParam
            {
                //RemitTransactionId = details.TransactionId,
                AccountStatus = walletValidationRes.AccountStatus,
                ContactNumber = walletValidationRes.ContactNumber,
                Message = walletValidationRes.Message,
                ResponseMessage = walletValidationRes.ResponseMessage,
                IsAccountValidated = walletValidationRes.IsAccountValidated,
                ResponseCode = walletValidationRes.ReponseCode.ToString(),
                Status = walletValidationRes.status,
                AgentCode = userMetaInfo.AgentCode,
                UserType = userMetaInfo.UserType,
                LoggedInUser = userMetaInfo.LoggedInUser
            };
            var validationLogStatus = await _myPayWalletLoadRepository.LogWalletUserValidationAsync(validationLogParam);

            if (walletValidationStatusCode != HttpStatusCode.OK || walletValidationRes is null)
            {
                result.AddError(walletValidationRes.ReponseCode, walletValidationRes.ResponseMessage);
                return result;
            }

            if (validationLogStatus.StatusCode != 200)
            {
                result.AddError(walletValidationRes.ReponseCode, walletValidationRes.ResponseMessage);
                return result;
            }

            if (!(!string.IsNullOrWhiteSpace(walletValidationRes.AccountStatus) &&
                walletValidationRes.AccountStatus.Equals("Active", StringComparison.OrdinalIgnoreCase) &&
                walletValidationRes.IsAccountValidated))
                return result;

            var paramGetPayoutReferenceInfo = new GetPayoutReferenceInfoDto
            {
                //RemitTransactionId = details.TransactionId,
                PaymentType = details.PaymentType,
                DeviceId = userMetaInfo.DeviceId,
                AgentCode = userMetaInfo.AgentCode,
                UserType = userMetaInfo.UserType,
                LoggedInUser = userMetaInfo.LoggedInUser,
                IpAddress = userMetaInfo.IpAddress
            };
            var (_, payoutRefInfo) = await _payoutRepository.GetPayoutReferenceInfoAsync(paramGetPayoutReferenceInfo);

            if (payoutRefInfo is null)
                return result;

            // now we have reference, and we proceed to payout
            var payoutReqObj = new MyPayWalletPayoutDto
            {
                Amount = details.ReceivingAmount.ToString(),
                ContactNumber = details.WalletID,
                Remarks = $"Payout amt. {details.ReceivingAmount} to {details.WalletID}.",
                Reference = payoutRefInfo.PayoutReferenceNo
            };
            var (_, walletPayoutRes) = await _myPayWalletLoadApiService.WalletPayoutAsync(payoutReqObj);

            // if unable to get wallet payout response, check payout txn status once and update accordingly
            if (walletPayoutRes is null)
            {
                var payoutTxnCheckReqObj = new MyPayWalletPayoutCheckTransactionStatusDto
                {
                    TransactionReference = payoutRefInfo.PayoutReferenceNo,
                    Reference = Guid.NewGuid().ToString("N")
                };
                var (_, txnCheckRes) = await _myPayWalletLoadApiService.CheckTransactionStatusAsync(payoutTxnCheckReqObj);
                if (txnCheckRes is null)
                    return result;

                var payoutTxnLogStatusObj = new MyPayWalletLoadPayoutTxnStatusLogParam
                {
                    PayoutRefereneNo = payoutRefInfo.PayoutReferenceNo,
                    AgentStatusCode = txnCheckRes.StatusCode.ToString(),
                    ResponseCode = txnCheckRes.ReponseCode.ToString(),
                    Message = txnCheckRes.Message,
                    ResponseMessage = txnCheckRes.ResponseMessage,
                    Status = txnCheckRes.Status,
                    TransactionStatus = txnCheckRes.TransactionStatus,
                    Details = txnCheckRes.Details,
                    UserType = userMetaInfo.UserType,
                    LoggedInUser = userMetaInfo.LoggedInUser
                };
                var payoutTxnLogStatus = await _myPayWalletLoadRepository.LogWalletPayoutTxnStatusAsync(payoutTxnLogStatusObj);

                // TODO: add further more logics here if required

                return result;
            }

            var payoutLogParam = new MyPayWalletPayoutLogParam
            {
                PayoutRefereneNo = payoutRefInfo.PayoutReferenceNo,
                AgentTransactionId = walletPayoutRes.MerchantWallet_TransactionId,
                Message = walletPayoutRes.Message,
                ResponseMessage = walletPayoutRes.ResponseMessage,
                ResponseCode = walletPayoutRes.ReponseCode.ToString(),
                Status = walletPayoutRes.status,
                Details = walletPayoutRes.Details,
                UserType = userMetaInfo.UserType,
                LoggedInUser = userMetaInfo.LoggedInUser,
            };
            var payoutLogStatus = await _myPayWalletLoadRepository.LogRemitToWalletPayoutAsync(payoutLogParam);

            if (payoutLogStatus.StatusCode != 200)
            {
                result.AddError(walletPayoutRes.ReponseCode, walletPayoutRes.ResponseMessage);
                return result;
            }
            if (walletValidationRes.ReponseCode == 1 && !string.IsNullOrEmpty(walletValidationRes.ResponseMessage))
            {
                result.AddSuccess(walletPayoutRes.ReponseCode, walletPayoutRes.ResponseMessage);
                return result;
            }
            if (walletPayoutRes is null || string.IsNullOrWhiteSpace(walletPayoutRes.MerchantWallet_TransactionId))
            {
                result.AddError(walletPayoutRes.ReponseCode, walletPayoutRes.ResponseMessage);
                return result;
            }
            // TODO: add further more logics here if required

            return result;
        }
        public async Task<MpmtResult> HandleByBulkTransactionToBankAsync(BulkTransactionDetailsModel txnDetails, BulkTxnBaseModel requestModel)
        {
            #region OLD Working code
            //// set partner transactionId as a tracker Id
            //_vendorApiLogger.SetLogContext(trackerId: txnDetails.tra);
            var result = new MpmtResult();
            var userMetaInfo = new
            {
                AgentCode = AgentCodes.MyPay, // TODO: re-check this agent code
                UserType = _httpContextAccessor.HttpContext?.GetUserType(),
                LoggedInUser = _httpContextAccessor.HttpContext?.GetUserName() ?? _httpContextAccessor.HttpContext?.GetUserEmail(),
                IpAddress = _httpContextAccessor.HttpContext?.GetIpAddress(),
                DeviceId = _httpContextAccessor.HttpContext?.GetUserAgentDevicePlatform(),
            };
            #region Check validation
            //Check Bank Account Validation
            var banklValidationReqObj = new MyPayValidateBankUserDto
            {
                AccountName = txnDetails.BeneficiaryName,
                AccountNumber = txnDetails.BankAccountNo,
                BankCode = txnDetails.BeneficiaryBankCode,
                Amount = decimal.Parse(txnDetails.SendingAmount),
                Reference = Guid.NewGuid().ToString("N"),
            };
            var (bankValidationStatusCode, bankValidationRes) = await _myPayBankLoadApiService
               .ValidateBankUserAsync(banklValidationReqObj);

            if (bankValidationRes is null)
                return result;
            if (bankValidationRes.ReponseCode == 3)
            {
                result.AddErrors(bankValidationRes.ReponseCode, bankValidationRes.responseMessage);
                return result;
            }

            if (bankValidationRes.ResponseCode == "000" || bankValidationRes.ResponseCode == "999")
            {
                //Int32.TryParse(bankValidationRes.MatchPercentate,out var matchPercentate)
                if (Int32.TryParse(bankValidationRes.MatchPercentate, out var matchPercentage))
                {
                    if (matchPercentage > 80)
                    {
                        var validationLogParam = new MyPayValidateWalletUserLogParam //MyPayValidateBankUserLogParam
                        {
                            //RemitTransactionId = txnDetails.TransactionId,
                            AccountStatus = bankValidationRes.AccountStatus,
                            Message = bankValidationRes.Message,
                            ResponseMessage = bankValidationRes.responseMessage,
                            IsAccountValidated = bankValidationRes.Message.ToUpper() == "SUCCESS" ? true : bankValidationRes.IsAccountValidated,
                            ResponseCode = bankValidationRes.ReponseCode.ToString(),
                            Status = bankValidationRes.status,
                            AgentCode = userMetaInfo.AgentCode,
                            UserType = userMetaInfo.UserType,
                            LoggedInUser = userMetaInfo.LoggedInUser
                        };

                        var validationLogStatus = await _myPayWalletLoadRepository.LogWalletUserValidationAsync(validationLogParam);
                        if (bankValidationStatusCode != HttpStatusCode.Created || bankValidationRes is null)
                        {
                            result.AddError(bankValidationRes.ReponseCode, bankValidationRes.responseMessage);
                            return result;
                        }

                        if (validationLogStatus.StatusCode != 200)
                        {
                            result.AddError(bankValidationRes.ReponseCode, bankValidationRes.responseMessage);
                            return result;
                        }
                        #endregion
                        // till here wallet account is valid
                        var paramGetPayoutReferenceInfo = new GetPayoutReferenceInfoDto
                        {
                            //RemitTransactionId = txnDetails.TransactionId,
                            PaymentType = txnDetails.PaymentType,
                            DeviceId = userMetaInfo.DeviceId,
                            AgentCode = userMetaInfo.AgentCode,
                            UserType = userMetaInfo.UserType,
                            LoggedInUser = userMetaInfo.LoggedInUser,
                            IpAddress = userMetaInfo.IpAddress
                        };
                        var (_, payoutRefInfo) = await _payoutRepository.GetPayoutReferenceInfoAsync(paramGetPayoutReferenceInfo);

                        if (payoutRefInfo is null)
                            return result;
                        // now we have reference, and we proceed to payout
                        var payoutReqObj = new MyPayBankPayoutDto
                        {
                            Amount = txnDetails.ReceivingAmount.ToString(),
                            BankId = txnDetails.BeneficiaryBankCode,
                            AccountHolderName = txnDetails.BeneficiaryName,
                            AccountNumber = txnDetails.BankAccountNo,
                            BankName = txnDetails.BeneficiaryBankName,
                            //Remarks = $"Payout amt. {txnDetails.ReceivingAmount} to {txnDetails.BankAccountNo}.",
                            Remarks = $"MPMT-{txnDetails.SenderName}",
                            Reference = payoutRefInfo.PayoutReferenceNo
                        };
                        var (_, bankPayoutRes) = await _myPayBankLoadApiService.BankPayoutAsync(payoutReqObj);

                        var payoutLogParam = new MyPayWalletPayoutLogParam
                        {
                            PayoutRefereneNo = payoutRefInfo.PayoutReferenceNo,
                            AgentTransactionId = bankPayoutRes.TransactionUniqueId,
                            Message = bankPayoutRes.Message,
                            ResponseMessage = bankPayoutRes.responseMessage,
                            ResponseCode = bankPayoutRes.ReponseCode.ToString(),
                            Status = bankPayoutRes.status,
                            Details = bankPayoutRes.Details,
                            UserType = userMetaInfo.UserType,
                            LoggedInUser = userMetaInfo.LoggedInUser,
                        };
                        var payoutLogStatus = await _myPayWalletLoadRepository.LogRemitToWalletPayoutAsync(payoutLogParam);

                        if (payoutLogStatus.StatusCode != 200)
                        {
                            result.AddErrors(bankPayoutRes.ReponseCode, bankPayoutRes.responseMessage);
                            return result;
                        }
                        if (bankPayoutRes.ReponseCode == 1 && !string.IsNullOrEmpty(bankPayoutRes.responseMessage))
                        {
                            result.AddSuccess(bankPayoutRes.ReponseCode, bankPayoutRes.responseMessage);
                            return result;
                        }
                        if (bankPayoutRes is null || string.IsNullOrWhiteSpace(bankPayoutRes.TransactionUniqueId))
                        {
                            result.AddErrors(bankPayoutRes.ReponseCode, bankPayoutRes.responseMessage);
                            return result;
                        }
                    }
                    else
                    {
                        result.AddError(bankValidationRes.ReponseCode, bankValidationRes.responseMessage);
                        return result;
                    }

                }
                else
                {
                    result.AddError(bankValidationRes.ReponseCode, bankValidationRes.responseMessage);
                    return result;
                }

            }
            return result;

            #endregion 

        }
        public async Task<MpmtResult> HandleByAdminCheckStatusWalletAsync(TransactionDetailsAdmin details)
        {
            _vendorApiLogger.SetLogContext(trackerId: details.TransactionId);
            var result = new MpmtResult();

            var walletCheckStatusReqObj = new MyPayWalletPayoutCheckTransactionStatusDto
            {
                TransactionId = details.TransactionId,
                Reference = Guid.NewGuid().ToString("N"),
                TransactionReference = details.TransactionId,
            };
            var (walletCheckStatusCode, walletCheckStatusRes) = await _myPayWalletLoadApiService
                .CheckTransactionStatusAsync(walletCheckStatusReqObj);
            if (walletCheckStatusRes is null)
                return result;
            else if (!walletCheckStatusRes.status)
            {
                result.AddError(walletCheckStatusRes.ReponseCode, walletCheckStatusRes.ResponseMessage);
                return result;
            }
            else
            {
                result.AddSuccess(walletCheckStatusRes.ReponseCode, walletCheckStatusRes.ResponseMessage);
                return result;
            }
        }
        #endregion
        //private bool IsPayoutProceedable(AddTransactionResultDetails details)
        //{
        //    // payout is not Proceedable if Transaction Approval Required
        //    if (!details.TransactionApprovalRequired.HasValue)
        //        return false;
        //    if (details.TransactionApprovalRequired.HasValue && details.TransactionApprovalRequired.Value)
        //        return false;

        //    // payout is not Proceedable if FeeCreditLimitOverFlow reached
        //    if (!details.FeeCreditLimitOverFlow.HasValue)
        //        return false;
        //    if (details.FeeCreditLimitOverFlow.HasValue && details.FeeCreditLimitOverFlow.Value)
        //        return false;

        //    return true;
        //}

        //private string BankUserComplianceDetail(MyPayCheckComplianceForBankLoadDto bankUserComplianceDetailReqObj)
        //{
        //    return "1";
        //}
        public bool HandlePartnerLowWalletBalanceNotificationEmailing(AddTransactionResultDetails details)
        {
            if (!details.SendWalletNotificationEmail.HasValue ||
                (details.SendWalletNotificationEmail.HasValue && !details.SendWalletNotificationEmail.Value))
                return false;

            if (string.IsNullOrWhiteSpace(details.PartnerEmail))
                return false;

            var mailContent = GeneratePartnerLowWalletBalanceNotificationEmailBody(details);

            var mailServiceModel = new MailServiceModel
            {
                MailFor = string.Empty,
                MailTo = details.PartnerEmail,
                RecipientName = string.Empty,
                MailSubject = "MyPay Money Transfer - Low Wallet Balance Alert",
                MailBody = mailContent,
                MailBcc = string.Empty,
                MailCc = "saroj.chaudhary@mypay.com.np",
                Content = string.Empty
            };
            var emailThread = new Thread(() => _mailService.SendMail(mailServiceModel)) { IsBackground = true };
            emailThread.Start();

            return true;
        }
        public bool HandlePartnerRemainingFeeBalanceNotificationEmailing(AddTransactionResultDetails details)
        {
            if (!details.SendFeeNotificationEmail.HasValue ||
               (details.SendFeeNotificationEmail.HasValue && !details.SendFeeNotificationEmail.Value))
                return false;

            if (string.IsNullOrWhiteSpace(details.PartnerEmail))
                return false;

            var mailContent = GeneratePartnerFeeBalanceNotificationEmailBody(details);

            var mailServiceModel = new MailServiceModel
            {
                MailFor = string.Empty,
                MailTo = details.PartnerEmail,
                RecipientName = string.Empty,
                MailSubject = "MyPay Money Transfer - Low fee Balance Alert",
                MailBody = mailContent,
                MailBcc = string.Empty,
                MailCc = "saroj.chaudhary@mypay.com.np",
                Content = string.Empty
            };
            var emailThread = new Thread(() => _mailService.SendMail(mailServiceModel)) { IsBackground = true };
            emailThread.Start();

            return true;
        }
        public bool HandleSenderTransactionNotificationEmailing(AddTransactionResultDetails details)
        {
            if (string.IsNullOrWhiteSpace(details.SenderEmail))
                return false;

            var mailContent = GenerateSenderTxnSummaryEmailBody(details);

            var mailServiceModel = new MailServiceModel
            {
                MailFor = string.Empty,
                MailTo = details.SenderEmail,
                RecipientName = string.Empty,
                MailSubject = "MyPay Money Transfer - Transaction Notification",
                MailBody = mailContent,
                MailBcc = string.Empty,
                MailCc = "saroj.chaudhary@mypay.com.np",
                Content = string.Empty
            };
            var emailThread = new Thread(async () => await _mailService.SendMail(mailServiceModel)) { IsBackground = true };
            emailThread.Start();

            return true;
        }
        private string GeneratePartnerLowWalletBalanceNotificationEmailBody(AddTransactionResultDetails details)
        {
            var companyName = "MyPay Money Transfer Pvt. Ltd.";
            var companyAlias = "MyPay Money Transfer";
            var companyEmail = "support@mypaymt.com";

            string mailBody =
                $@"<p>Dear partner,</p>
                    <p>Your wallet balance is running low. Please top up soon to avoid transaction disruptions.</p>
                    <p><strong>Current Balance: </strong>{details.RemainingWalletBal?.ToString() ?? string.Empty} {details.WalletCurrency ?? string.Empty}</p>
                    <p>Thank you!</p>
                    <br />
                    <h3><u>{companyAlias} Service Contact Information:</u></h3>
                    <p>
                        {companyName}<br>
                        Contact No.: <br>
                        Email: {companyEmail}<br>
                        Website: <br>
                        Address: <br>
                    </p>
                    <p>Thank you for choosing {companyAlias} Service!</p>

                <p>CONFIDENTIALITY NOTICE: This transmittal is a confidential communication or may otherwise be privileged. If you are not the intended recipient, you are hereby notified that you have received this transmittal in error and that any review, dissemination, distribution or copying of this transmittal is strictly prohibited. If you have received this communication in error, please notify this office, and immediately delete this message and all its attachments, if any.</p>";

            return mailBody;
        }
        private string GeneratePartnerFeeBalanceNotificationEmailBody(AddTransactionResultDetails details)
        {
            var companyName = "MyPay Money Transfer Pvt. Ltd.";
            var companyAlias = "MyPay Money Transfer";
            var companyEmail = "support@mypaymt.com";

            string mailBody =
                $@"<p>Dear partner,</p>
                    <p>Your fee balance is running low. Please top up soon to avoid transaction disruptions.</p>
                    <p><strong>Current Balance: </strong>{details.RemainingFeeBal?.ToString() ?? string.Empty} {details.WalletCurrency?.ToString() ?? string.Empty}</p>
                    <p>Thank you!</p>
                    <br />
                    <h3><u>{companyAlias} Service Contact Information:</u></h3>
                    <p>
                        {companyName}<br>
                        Contact No.: <br>
                        Email: {companyEmail}<br>
                        Website: <br>
                        Address: <br>
                    </p>
                    <p>Thank you for choosing {companyAlias} Service!</p>

                <p>CONFIDENTIALITY NOTICE: This transmittal is a confidential communication or may otherwise be privileged. If you are not the intended recipient, you are hereby notified that you have received this transmittal in error and that any review, dissemination, distribution or copying of this transmittal is strictly prohibited. If you have received this communication in error, please notify this office, and immediately delete this message and all its attachments, if any.</p>";

            return mailBody;
        }
        private string GenerateSenderTxnSummaryEmailBody(AddTransactionResultDetails details)
        {
            var companyName = "MyPay Money Transfer Pvt. Ltd.";
            var companyAlias = "MyPay Money Transfer";
            var companyEmail = "support@mypaymt.com";

            var trasferMethodPart = details.PaymentType switch
            {
                PayoutTypes.Wallet => $@"<p>Transfer Type: <strong>Wallet Transfer</strong></p>
                                        <p>Payment Account: <strong>{details.WalletNumber ?? string.Empty}</strong></p>
                                        <p>Wallet Name: <strong>{details.WalletName ?? string.Empty}</strong></p>",
                PayoutTypes.Bank => $@"<p>Transfer Type: <strong>Bank Transfer</strong></p>
                                        <p>Payment Account: <strong>{details.BankAccountNumber ?? string.Empty}</strong></p>
                                        <p>Bank Name: <strong>{details.BankName ?? string.Empty}</strong></p>",
                PayoutTypes.Cash => $@"<p>Transfer Type: <strong>Cash</strong></p>
                                        <p>Payment Account: <strong>Cash</strong></p>",
                _ => $@"<p>Transfer Type: <strong></strong></p>
                        <p>Payment Account: <strong>{details.BankAccountNumber ?? details.WalletNumber ?? string.Empty}</strong></p>"
            };

            // after Email < p > Service Charge: < strong >{ details.ServiceCharge?.ToString() ?? string.Empty}
            //{ details.WalletCurrency}</ strong ></ p >

            string mailBody =
                $@"<h1>{companyAlias} Summary Receipt:</h1>
                    <p>Transaction Date: <strong>{details.TransactionDate ?? string.Empty}</strong></p>
                    <p>Transaction ID: <strong>{details.TransactionId ?? string.Empty}</strong></p>
                    <p>Reference Number: <strong>{details.ReferenceTokenNo ?? string.Empty}</strong></p>
                    <h3>Sender Information:</h3>
                    <p>Email: <strong>{details.SenderEmail ?? string.Empty}</strong></p>
                    <h3>Transfer Details:</h3>
                    <p>Sending Amount: <strong>{details.SendingAmount?.ToString() ?? string.Empty} {details.WalletCurrency}</strong></p>             
                    <p>Net Sending Amount: <strong>{details.NetSendingAmount?.ToString() ?? string.Empty} {details.WalletCurrency}</strong></p>
                    <p>Exchange Rate: <strong>{details.ConversionRate?.ToString() ?? string.Empty}</strong></p>
                    <p>Receiving Amount: <strong>{details.PayableAmount?.ToString() ?? string.Empty} NPR</strong></p>
                    <h3>Transfer Method:</h3>
                    {trasferMethodPart}
                    <br />
                    <h3><u>{companyAlias} Service Contact Information:</u></h3>
                    <p>
                        {companyName}<br>
                        Contact No.: <br>
                        Email: {companyEmail}<br>
                        Website: <br>
                        Address: <br>
                    </p>
                    <p>Thank you for choosing {companyAlias} Service!</p>


                <p>CONFIDENTIALITY NOTICE: This transmittal is a confidential communication or may otherwise be privileged. If you are not the intended recipient, you are hereby notified that you have received this transmittal in error and that any review, dissemination, distribution or copying of this transmittal is strictly prohibited. If you have received this communication in error, please notify this office, and immediately delete this message and all its attachments, if any.</p>";

            return mailBody;
        }
        #region Bulk Transaction Emailing Partner
        public bool HandleBulkTransactionEmailing(BulkTxnBaseModel details)
        {
            if (string.IsNullOrWhiteSpace(details.PartnerEmail))
                return false;

            var mailContent = GenerateBulkTransactionEmailBody();

            var attachment = new Dictionary<string, byte[]>
            {
                { details.FileName, details.SendExcelFile  }

            };

            var mailServiceModel = new MailServiceModel
            {
                MailFor = string.Empty,
                MailTo = details.PartnerEmail,
                RecipientName = string.Empty,
                MailSubject = "MyPay Money Transfer - Bulk Transaction Error List",
                MailBody = mailContent,
                MailBcc = string.Empty,
                MailCc = "saroj.chaudhary@mypay.com.np",
                Content = string.Empty,
                MailAttachements = attachment
            };


            var emailThread = new Thread(() => _mailService.SendMail(mailServiceModel)) { IsBackground = true };
            emailThread.Start();

            return true;
        }
        private string GenerateBulkTransactionEmailBody()
        {
            var companyName = "MyPay Money Transfer Pvt. Ltd.";
            var companyAlias = "MyPay Money Transfer";
            var companyEmail = "support@mypaymt.com";

            string mailBody =
                $@"<p>Dear partner's,</p>
                    <p>I am writing to bring to your attention on some issues that have arisen regarding our recent transaction. </p>
                    <p> It appears that there are certain discrepancies that require immediate attention and resolution.</p>
                    <p> Please follow the attached Document.</p>
                    <p>Thank you!</p>
                    <br />
                    <h3><u>{companyAlias} Service Contact Information:</u></h3>
                    <p>
                        {companyName}<br>
                        Contact No.: <br>
                        Email: {companyEmail}<br>
                        Website: <br>
                        Address: <br>
                    </p>
                    <p>Thank you for choosing {companyAlias} Service!</p>
                <p>CONFIDENTIALITY NOTICE: This transmittal is a confidential communication or may otherwise be privileged. If you are not the intended recipient, you are hereby notified that you have received this transmittal in error and that any review, dissemination, distribution or copying of this transmittal is strictly prohibited. If you have received this communication in error, please notify this office, and immediately delete this message and all its attachments, if any.</p>";

            return mailBody;
        }
        #endregion
    }
}
