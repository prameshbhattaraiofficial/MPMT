using AutoMapper;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Office2016.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.HttpSys;
using Mpmt.Core.Domain;
using Mpmt.Core.Domain.Admin.Reports;
using Mpmt.Core.Domain.Partners.SendTransactions;
using Mpmt.Core.Domain.Payout;
using Mpmt.Core.Dtos;
using Mpmt.Core.Dtos.AgentApi;
using Mpmt.Core.Dtos.PartnerReport;
using Mpmt.Core.Dtos.Transaction;
using Mpmt.Data.Repositories.Agents.AgentTxn;
using Mpmt.Data.Repositories.Partner;
using Mpmt.Services.Extensions;
using Mpmt.Services.Services.AgentTxn;
using Mpmt.Services.Services.Common;
using Mpmts.Core.Dtos;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Reflection;
using System.Security.Claims;

namespace Mpmt.Services.Partner
{
    /// <summary>
    /// The partner send txns service.
    /// </summary>
    public class PartnerSendTxnsService : BaseService, IPartnerSendTxnsService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPartnerSendTxnsRepository _partnerSendTxnsRepository;
        private readonly IPartnerPayoutHandlerService _partnerPayoutHandler;
        private readonly IMapper _mapper;
        private readonly IAgentTransactionRepository _repo;
        /// <summary>
        /// Initializes a new instance of the <see cref="PartnerSendTxnsService"/> class.
        /// </summary>
        /// <param name="httpContextAccessor">The http context accessor.</param>
        /// <param name="partnerSendTxnsRepository">The partner send txns repository.</param>
        /// <param name="payoutRepository">The payout repository.</param>
        /// <param name="vendorApiLogger">The vendor api logger.</param>
        /// <param name="myPayWalletLoadRepository">The my pay wallet load repository.</param>
        /// <param name="myPayWalletLoadApiService">The my pay wallet load api service.</param>
        /// <param name="mailService">The mail service.</param>
        /// <param name="myPayBankLoadApiService">The my pay bank load api service.</param>
        public PartnerSendTxnsService(
            IHttpContextAccessor httpContextAccessor,
            IPartnerSendTxnsRepository partnerSendTxnsRepository,
            IPartnerPayoutHandlerService partnerPayoutHandler,
            IMapper mapper,
            IAgentTransactionRepository repo)
        {
            _httpContextAccessor = httpContextAccessor;
            _partnerSendTxnsRepository = partnerSendTxnsRepository;
            _partnerPayoutHandler = partnerPayoutHandler;
            _mapper = mapper;
            _repo = repo;
        }

        /// <summary>
        /// Adds the transaction async.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>A Task.</returns>
        public async Task<MpmtResult> AddTransactionAsync(AddTransactionDto request)
        {
            var result = new MpmtResult();
            request.TransactionType = "PARTNERTRANSACTION";
            var PartnerCode = string.IsNullOrEmpty(request.PartnerCode) ? _httpContextAccessor.HttpContext.User.FindFirstValue("PartnerCode") : request.PartnerCode;
            request.IpAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
            request.LoggedInUser = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
            request.UserType = _httpContextAccessor.HttpContext.User.FindFirstValue("UserType");
            request.PartnerCode = PartnerCode;

            if (string.IsNullOrWhiteSpace(request.ProcessId))
            {
                result.AddError(400, "Process ID is required.");
                return result;
            }

            // TODO: Add more validation logics

            var (addTxnStatus, addTxnDetails) = await _partnerSendTxnsRepository.AddTransactionAsync(request);
            if (addTxnStatus.StatusCode != 200)
            {
                result.AddError(addTxnStatus.StatusCode, addTxnStatus.MsgText);
                return result;
            }

            if (addTxnDetails is null)
                return result;

            var txnResultDetails = MapToSendTransactionResultDetails(addTxnDetails);

            _ = _partnerPayoutHandler.HandleSenderTransactionNotificationEmailing(txnResultDetails);
            _ = _partnerPayoutHandler.HandlePartnerLowWalletBalanceNotificationEmailing(txnResultDetails);
            _ = _partnerPayoutHandler.HandlePartnerRemainingFeeBalanceNotificationEmailing(txnResultDetails);

            if (addTxnDetails.ComplianceFlag == true)
                return result;

            if (!PartnerPayoutHelper.IsPayoutProceedable(txnResultDetails))
                return result;
            if (request.PaymentType.ToUpper() == "CASH")
                return result;
            return addTxnDetails.PaymentType switch
            {
                PayoutTypes.Wallet => await _partnerPayoutHandler.HandlePayoutToWalletAsync(txnResultDetails),
                PayoutTypes.Bank => await _partnerPayoutHandler.HandlePayoutToBankAsync(txnResultDetails)
            };

        }

        public async Task<MpmtResult> AddByAdminTransactionAsync(TransactionDetailsAdmin request)
        {
            request.TransactionType = "ADMINTRANSACTION";
            var PartnerCode = string.IsNullOrEmpty(request.PartnerCode) ? _httpContextAccessor.HttpContext.User.FindFirstValue("PartnerCode") : request.PartnerCode;
            request.IpAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
            request.LoggedInUser = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
            request.UserType = _httpContextAccessor.HttpContext.User.FindFirstValue("UserType");
            request.PartnerCode = PartnerCode;

            return request.PaymentType switch
            {
                PayoutTypes.Bank => await _partnerPayoutHandler.HandleByAdminPayoutToBankAsync(request),
                PayoutTypes.Wallet => await _partnerPayoutHandler.HandleByAdminPayoutToWalletAsync(request)
            };
        }

        public async Task<MpmtResult> CheckStatusTransactionAsync(TransactionDetailsAdmin request)
        {
            request.TransactionType = "ADMINTRANSACTION";
            var PartnerCode = string.IsNullOrEmpty(request.PartnerCode) ? _httpContextAccessor.HttpContext.User.FindFirstValue("PartnerCode") : request.PartnerCode;
            request.IpAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
            request.LoggedInUser = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
            request.UserType = _httpContextAccessor.HttpContext.User.FindFirstValue("UserType");
            request.PartnerCode = PartnerCode;

            return request.PaymentType switch
            {
                PayoutTypes.Wallet => await _partnerPayoutHandler.HandleByAdminCheckStatusWalletAsync(request)
            };
        }

        private AddTransactionResultDetails MapToSendTransactionResultDetails(AddTransactionDetailsDto details)
        {
            ArgumentNullException.ThrowIfNull(details, nameof(details));

            return new AddTransactionResultDetails
            {
                TransactionId = details.TransactionId,
                ReferenceTokenNo = details.ReferenceTokenNo,
                PaymentType = details.PaymentType,
                TransactionApprovalRequired = details.TransactionApprovalRequired,
                WalletNumber = details.WalletNumber,
                WalletName = details.WalletName,
                WalletHolderName = details.WalletHolderName,
                BankCode = details.BankCode,
                BankName = details.BankName,
                BankAccountNumber = details.BankAccountNumber,
                BankAccountHolderName = details.BankAccountHolderName,
                SendingAmount = details.SendingAmount,
                ServiceCharge = details.ServiceCharge,
                NetSendingAmount = details.NetSendingAmount,
                ConversionRate = details.ConversionRate,
                PayableAmount = details.PayableAmount,
                SenderEmail = details.SenderEmail,
                PartnerEmail = details.PartnerEmail,
                SendWalletNotificationEmail = details.SendWalletNotificationEmail,
                RemainingWalletBal = details.RemainingWalletBal,
                WalletCurrency = details.WalletCurrency,
                TransactionDate = details.TransactionDate,
                SendFeeNotificationEmail = details.SendFeeNotificationEmail,
                RemainingFeeBal = details.RemainingFeeBal,
                FeeCreditLimitOverFlow = details.FeeCreditLimitOverFlow,
                ComplianceFlag = details.ComplianceFlag
            };
        }
        /// <summary>
        /// Gets the send txn charge amount details async.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>A Task.</returns>
        public async Task<(SprocMessage, SendTxnChargeAmountDetailsDto)> GetSendTxnChargeAmountDetailsAsync(GetSendTxnChargeAmountDetailsRequest request)
        {
            var partnerCode = _httpContextAccessor.HttpContext.User.FindFirstValue("PartnerCode");
            request.PartnerCode = partnerCode;
            return await _partnerSendTxnsRepository.GetSendTxnChargeAmountDetailsAsync(request);
        }

        public async Task<(MpmtResult, TxnProcessIdDto)> GetProcessIdAsync(GetProcessId request)
        {
            var result = new MpmtResult();
            var partnerCode = _httpContextAccessor.HttpContext.User.FindFirstValue("PartnerCode");

            if (string.IsNullOrEmpty(partnerCode))
            {
                result.AddError(400, "Bad request");
                return (result, null);
            }

            var (sprocMessage, processId) = await _partnerSendTxnsRepository.GetProcessIdAsync(partnerCode, request);

            return (MapSprocMessageToMpmtResult(sprocMessage), processId);
        }

        public async Task<(SprocMessage, SendTxnChargeAmountDetailsDto)> GetSendNepaliToOtherChargeAmountDetailsAsync(GetSendTxnChargeAmountDetailsRequest request)
        {
            var partnerCode = _httpContextAccessor.HttpContext.User.FindFirstValue("PartnerCode");
            request.PartnerCode = partnerCode;
            return await _partnerSendTxnsRepository.GetSendNepaliToOtherChargeAmountDetailsAsync(request);
        }


        #region Bulk Transaction Validation
        public async Task<(FieldValidationResult, BulkTransactionResponse)> ValidateBulkTransactionCells(BulkTransactionDetailsModel request)
        {
            var result = new FieldValidationResult();
            //var resp = new BulkTransactionResponse();
            var resp = _mapper.Map<BulkTransactionResponse>(request);

            decimal sendingAmount;

            if (request.PaymentType.ToUpper() != "CASH" && request.PaymentType.ToUpper() != "BANK" && request.PaymentType.ToUpper() != "WALLET")
                result.AddError(new() { Field = nameof(request.PaymentType), Message = $"{nameof(request.PaymentType)} is invalid" });

            if (string.IsNullOrWhiteSpace(request.PaymentType))
                result.AddError(new() { Field = nameof(request.PaymentType), Message = $"{nameof(request.PaymentType)} is required" });

            if (string.IsNullOrWhiteSpace(request.SourceCurrency))
                result.AddError(new() { Field = nameof(request.SourceCurrency), Message = $"{nameof(request.SourceCurrency)} is required" });

            if (string.IsNullOrWhiteSpace(request.DestinationCurrency))
                result.AddError(new() { Field = nameof(request.DestinationCurrency), Message = $"{nameof(request.DestinationCurrency)} is required" });

            if (string.IsNullOrWhiteSpace(request.SenderEmail))
                result.AddError(new() { Field = nameof(request.SenderEmail), Message = $"{nameof(request.SenderEmail)} is required" });

            if (string.IsNullOrWhiteSpace(request.SendingAmount))
                result.AddError(new() { Field = nameof(request.SendingAmount), Message = $"{nameof(request.SendingAmount)} is required" });

            if (string.IsNullOrWhiteSpace(request.ReceivingAmount))
                result.AddError(new() { Field = nameof(request.ReceivingAmount), Message = $"{nameof(request.ReceivingAmount)} is required" });

            if (string.IsNullOrWhiteSpace(request.SenderName))
                result.AddError(new() { Field = nameof(request.SenderName), Message = $"{nameof(request.SenderName)} is required" });

            if (string.IsNullOrWhiteSpace(request.SendeContactNumber))
                result.AddError(new() { Field = nameof(request.SendeContactNumber), Message = $"{nameof(request.SendeContactNumber)} is required" });

            if (string.IsNullOrWhiteSpace(request.SenderCountry))
                result.AddError(new() { Field = nameof(request.SenderCountry), Message = $"{nameof(request.SenderCountry)} is required" });

            if (string.IsNullOrWhiteSpace(request.SenderCity))
                result.AddError(new() { Field = nameof(request.SenderCity), Message = $"{nameof(request.SenderCity)} is required" });

            if (string.IsNullOrWhiteSpace(request.SenderAddress))
                result.AddError(new() { Field = nameof(request.SenderAddress), Message = $"{nameof(request.SenderAddress)} is required" });

            if (string.IsNullOrWhiteSpace(request.SenderDocType))
                result.AddError(new() { Field = nameof(request.SenderDocType), Message = $"{nameof(request.SenderDocType)} is required" });

            if (string.IsNullOrWhiteSpace(request.RelationshipWithBeneficiary))
                result.AddError(new() { Field = nameof(request.RelationshipWithBeneficiary), Message = $"{nameof(request.RelationshipWithBeneficiary)} is required" });

            if (string.IsNullOrWhiteSpace(request.SenderOccupation))
                result.AddError(new() { Field = nameof(request.SenderOccupation), Message = $"{nameof(request.SenderOccupation)} is required" });

            if (string.IsNullOrWhiteSpace(request.SenderSourceOfIncome))
                result.AddError(new() { Field = nameof(request.SenderSourceOfIncome), Message = $"{nameof(request.SenderSourceOfIncome)} is required" });

            if (string.IsNullOrWhiteSpace(request.PurposeOfRemittance))
                result.AddError(new() { Field = nameof(request.PurposeOfRemittance), Message = $"{nameof(request.PurposeOfRemittance)} is required" });

            if (!Decimal.TryParse(request.SendingAmount, out sendingAmount))
                result.AddError(new() { Field = nameof(sendingAmount), Message = $"Sending amount is invalid" });

            if (string.IsNullOrWhiteSpace(request.BeneficiaryName))
                result.AddError(new() { Field = nameof(request.BeneficiaryName), Message = $"{nameof(request.BeneficiaryName)} is required" });

            if (string.IsNullOrWhiteSpace(request.BeneficiaryContactNumber))
                result.AddError(new() { Field = nameof(request.BeneficiaryContactNumber), Message = $"{nameof(request.BeneficiaryContactNumber)} is required" });

            if (string.IsNullOrWhiteSpace(request.BeneficiaryCountry))
                result.AddError(new() { Field = nameof(request.BeneficiaryCountry), Message = $"{nameof(request.BeneficiaryCountry)} is required" });

            if (string.IsNullOrWhiteSpace(request.BeneficiaryCity))
                result.AddError(new() { Field = nameof(request.BeneficiaryCity), Message = $"{nameof(request.BeneficiaryCity)} is required" });

            if (string.IsNullOrWhiteSpace(request.BeneficiaryAddress))
                result.AddError(new() { Field = nameof(request.BeneficiaryAddress), Message = $"{nameof(request.BeneficiaryAddress)} is required" });

            if (string.IsNullOrWhiteSpace(request.BeneficiaryRelationwithSender))
                result.AddError(new() { Field = nameof(request.BeneficiaryRelationwithSender), Message = $"{nameof(request.BeneficiaryRelationwithSender)} is required" });

            if (request.PaymentType.ToUpper() == "BANK")
            {
                if (string.IsNullOrWhiteSpace(request.BeneficiaryBankCode))
                    result.AddError(new() { Field = nameof(request.BeneficiaryBankCode), Message = $"{nameof(request.BeneficiaryBankCode)} is required" });

                if (string.IsNullOrWhiteSpace(request.BeneficiaryBankName))
                    result.AddError(new() { Field = nameof(request.BeneficiaryBankName), Message = $"{nameof(request.BeneficiaryBankName)} is required" });

                if (string.IsNullOrWhiteSpace(request.BeneficiaryBankBranch))
                    result.AddError(new() { Field = nameof(request.BeneficiaryBankBranch), Message = $"{nameof(request.BeneficiaryBankBranch)} is required" });

                if (string.IsNullOrWhiteSpace(request.BankAccountNo))
                    result.AddError(new() { Field = nameof(request.BankAccountNo), Message = $"{nameof(request.BankAccountNo)} is required" });
            }
            if (request.PaymentType.ToUpper() == "WALLET")
            {
                if (string.IsNullOrWhiteSpace(request.WalletCode))
                    result.AddError(new() { Field = nameof(request.WalletCode), Message = $"{nameof(request.WalletCode)} is required" });

                if (string.IsNullOrWhiteSpace(request.WalletID))
                    result.AddError(new() { Field = nameof(request.WalletID), Message = $"{nameof(request.WalletID)} is required" });
            }

            return (result, resp);
        }
        #endregion

        public async Task<MpmtResult> ValidateBulkTxnApiAsync(BulkTransactionDetailsModel request)
        {
            var PartnerCode = _httpContextAccessor.HttpContext.User.FindFirstValue("PartnerCode");
            var requestModel = new BulkTxnBaseModel
            {
                TransactionType = "BULKTRANSACTION",
                IpAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString(),
                LoggedInUser = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name),
                UserType = _httpContextAccessor.HttpContext.User.FindFirstValue("UserType"),
                PartnerCode = PartnerCode
            };
            return request.PaymentType.ToUpper() switch
            {
                PayoutTypes.Bank => await _partnerPayoutHandler.HandleByBulkTransactionToBankAsync(request, requestModel),
                PayoutTypes.Wallet => await _partnerPayoutHandler.HandleByBulkTransactionToWalletAsync(request, requestModel)
            };
        }

        public async Task<(SprocMessage, AddTransactionDetailsDto)> PushBulkTransactionServiceAsync(BulkTransactionDetailsModel bulkTransactionDetailsModel)
        {
            var PartnerCode = _httpContextAccessor.HttpContext.User.FindFirstValue("PartnerCode");
            var ReferenceId = Guid.NewGuid().ToString("N");
            var processId = await _repo.GetProcessIdAsync(PartnerCode, ReferenceId);
            var requestModel = new BulkTxnBaseModel
            {
                ProcessId = processId.ToString(),
                LoggedInUser = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name),
                PartnerCode = PartnerCode,
                DeviceId = _httpContextAccessor.HttpContext?.GetUserAgentDevicePlatform(),
                IpAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString()
            };
            var (resp, data) = await _partnerSendTxnsRepository.PushBulkTransactionAsync(bulkTransactionDetailsModel, requestModel);
            
            if (resp.StatusCode == 200)
            {
                var txnResultDetails = MapToSendTransactionResultDetails(data);

                _ = _partnerPayoutHandler.HandleSenderTransactionNotificationEmailing(txnResultDetails);
                _ = _partnerPayoutHandler.HandlePartnerLowWalletBalanceNotificationEmailing(txnResultDetails);
                _ = _partnerPayoutHandler.HandlePartnerRemainingFeeBalanceNotificationEmailing(txnResultDetails);
            }
            return (resp, data);
        }

        public bool SendBulkTransactionEmailAsync(BulkTxnBaseModel modelRequest)
        {
            var emailPartner = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
            modelRequest.PartnerEmail = emailPartner;
            _ = _partnerPayoutHandler.HandleBulkTransactionEmailing(modelRequest);
            return true;
        }

        public async Task<SprocMessage> cancelledTransactionAsysnc(RemitTxnReport model)
        {
            model.CancelledBy = "admin";
            model.CancelledUserType = "admin";
            return await _partnerSendTxnsRepository.cancelledTransactionAsysnc(model);
        }
    }
}
