using Microsoft.AspNetCore.Http;
using Mpmt.Core;
using Mpmt.Core.Common;
using Mpmt.Core.Domain;
using Mpmt.Core.Domain.Agent;
using Mpmt.Core.Domain.Partners;
using Mpmt.Core.Domain.Payout;
using Mpmt.Core.Dtos.BankLoad;
using Mpmt.Core.Dtos.CashAgent;
using Mpmt.Core.Dtos.PartnerApi;
using Mpmt.Core.Dtos.WalletLoad.MyPay;
using Mpmt.Data.Repositories.PartnerApi;
using Mpmt.Services.Logging;
using Mpmt.Services.Partner;
using Mpmt.Services.Services.BankLoadApi;
using Mpmt.Services.Services.WalletLoadApi.MyPay;
using Org.BouncyCastle.Asn1;
using System.Globalization;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace Mpmt.Services.Services.PartnerApi
{
    public class PartnerApiService : IPartnerApiService
    {
        private readonly IExceptionLogger _exceptionLogger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPartnerApiRepository _partnerApiRepository;
        private readonly IPartnerPayoutHandlerService _partnerPayoutHandlerService;
        private readonly IMyPayWalletLoadApiService _myPayWalletLoadApiService;
        private readonly IMyPayBankLoadApiService _myPayBankLoadApiService;
        private readonly IVendorApiLogger _vendorApiLogger;

        public PartnerApiService(
            IExceptionLogger exceptionLogger,
            IHttpContextAccessor httpContextAccessor,
            IPartnerApiRepository partnerApiRepository,
            IPartnerPayoutHandlerService partnerPayoutHandlerService,
            IMyPayWalletLoadApiService myPayWalletLoadApiService,
            IMyPayBankLoadApiService myPayBankLoadApiService,
            IVendorApiLogger vendorApiLogger)
        {
            _exceptionLogger = exceptionLogger;
            _httpContextAccessor = httpContextAccessor;
            _partnerApiRepository = partnerApiRepository;
            _partnerPayoutHandlerService = partnerPayoutHandlerService;
            _myPayWalletLoadApiService = myPayWalletLoadApiService;
            _myPayBankLoadApiService = myPayBankLoadApiService;
            _vendorApiLogger = vendorApiLogger;
        }

        public async Task<(HttpStatusCode, object)> GetInstrumentListsAsync(GetInstrumentListsRequest request)
        {
            var partnerCode = _httpContextAccessor.HttpContext?.User.FindFirstValue(PartnerClaimTypes.PartnerCode);
            var apiUserName = _httpContextAccessor.HttpContext?.User.FindFirstValue(PartnerClaimTypes.ApiUserName);
            if (partnerCode is null)
                partnerCode = _httpContextAccessor.HttpContext?.User.FindFirstValue(AgentClaimTypes.AgentCode);
            var agentCode = _httpContextAccessor.HttpContext?.User.FindFirstValue(AgentClaimTypes.AgentCode);

            if (string.IsNullOrEmpty(partnerCode))
                return HandleBadRequestResponse<InstrumentLists>();

            if (string.IsNullOrEmpty(apiUserName))
                return HandleBadRequestResponse<InstrumentLists>();

            var fieldValidationResult = ValidateGetInstrumentListRequestFields(request);
            if (!fieldValidationResult.Success)
                return HandleBadRequestResponse<InstrumentLists>(responseDetailMessage: "One or more field validations failed",
                    fieldErrors: fieldValidationResult.Errors);

            if (!apiUserName.Equals(request.ApiUserName))
                return HandleBadRequestResponse<InstrumentLists>(responseDetailMessage: "One or more field validations failed",
                    fieldErrors: new() { new() { Field = nameof(request.Signature), Message = "Invalid ApiUserName." } });

            var signatureData = request.ApiUserName;
            var isSignatureValid = await IsSignatureValidAsync(request.Signature, signatureData);

            if (!isSignatureValid)
                return HandleInvalidSignatureTokenResponse<InstrumentLists>();

            var response = await _partnerApiRepository.GetInstrumentListsAsync(partnerCode);
            response.ResponseCode = ResponseCodes.Code200_Success;
            response.ResponseStatus = ResponseStatuses.Success;
            response.ResponseMessage = "Instrument lists fetched successfully!";

            return (HttpStatusCode.OK, response);
        }

        public async Task<(HttpStatusCode, object)> GetTxnChargeDetailsAsync(GetTxnChargeDetailsRequest request)
        {
            var partnerCode = _httpContextAccessor.HttpContext?.User.FindFirstValue(PartnerClaimTypes.PartnerCode);
            var apiUserName = _httpContextAccessor.HttpContext?.User.FindFirstValue(PartnerClaimTypes.ApiUserName);
            if (string.IsNullOrEmpty(partnerCode))
                return HandleBadRequestResponse<TxnChargeDetails>();

            if (string.IsNullOrEmpty(apiUserName))
                return HandleBadRequestResponse<TxnChargeDetails>();

            var fieldValidationResult = ValidateGetTxnChargeDetailsRequestFields(request);
            if (!fieldValidationResult.Success)
                return HandleBadRequestResponse<TxnChargeDetails>(responseDetailMessage: "One or more field validations failed",
                    fieldErrors: fieldValidationResult.Errors);

            if (!apiUserName.Equals(request.ApiUserName))
                return HandleBadRequestResponse<TxnChargeDetails>(responseDetailMessage: "One or more field validations failed",
                    fieldErrors: new() { new() { Field = nameof(request.ApiUserName), Message = "Invalid ApiUserName." } });

            var signatureData = $"{request.ApiUserName},{request.PaymentType},{request.SourceCurrency},{request.DestinationCurrency},{request.SourceAmount}";
            var isSignatureValid = await IsSignatureValidAsync(request.Signature, signatureData);

            if (!isSignatureValid)
                return HandleInvalidSignatureTokenResponse<TxnChargeDetails>();

            var getChargeDetailsParam = new GetTxnChargeDetailsParam
            {
                PartnerCode = partnerCode,
                PaymentType = request.PaymentType,
                SourceCurrency = request.SourceCurrency,
                DestinationCurrency = request.DestinationCurrency,
                SourceAmount = request.SourceAmount
            };
            var (sprocStatus, response) = await _partnerApiRepository.GetTxnChargeDetailsAsync(getChargeDetailsParam);

            if (sprocStatus.StatusCode != 200)
                return HandleBadRequestResponse<TxnChargeDetails>(responseMessage: sprocStatus.MsgText);

            if (response is null)
                return HandleBadRequestResponse<TxnChargeDetails>(responseMessage: "Unable to fetch transaction charge details.");

            response.ResponseCode = ResponseCodes.Code200_Success;
            response.ResponseStatus = ResponseStatuses.Success;
            response.ResponseMessage = "Transaction charge details fetched successfully!";

            return (HttpStatusCode.OK, response);
        }

        public async Task<(HttpStatusCode, object)> GetTxnProcessIdAsync(GetProcessIdRequest request)
        {
            var apiUserName = _httpContextAccessor.HttpContext?.User.FindFirstValue(PartnerClaimTypes.ApiUserName);
            var partnerCode = _httpContextAccessor.HttpContext?.User.FindFirstValue(PartnerClaimTypes.PartnerCode);

            if (string.IsNullOrEmpty(apiUserName))
                return HandleBadRequestResponse<TxnProcessIdResponse>();

            if (string.IsNullOrEmpty(partnerCode))
                return HandleBadRequestResponse<TxnProcessIdResponse>();

            var fieldValidationResult = ValidateGetTxnProcessIdRequestFields(request);
            if (!fieldValidationResult.Success)
                return HandleBadRequestResponse<TxnChargeDetails>(responseDetailMessage: "One or more field validations failed",
                    fieldErrors: fieldValidationResult.Errors);

            if (!apiUserName.Equals(request.ApiUserName))
                return HandleBadRequestResponse<TxnProcessIdResponse>(responseDetailMessage: "One or more field validations failed",
                    fieldErrors: new() { new() { Field = nameof(request.ApiUserName), Message = "Invalid ApiUserName." } });

            var signatureData = $"{request.ApiUserName},{request.ReferenceId}";
            var isSignatureValid = await IsSignatureValidAsync(request.Signature, signatureData);

            if (!isSignatureValid)
                return HandleInvalidSignatureTokenResponse<TxnProcessIdResponse>();

            var (sprocMessage, processId) = await _partnerApiRepository.GetTxnProcessIdAsync(partnerCode, request.ReferenceId);
            if (sprocMessage.StatusCode != 200)
                return HandleBadRequestResponse<TxnProcessIdResponse>(responseMessage: "Duplicate ReferenceId");

            if (string.IsNullOrWhiteSpace(processId?.ProcessId))
                return HandleBadRequestResponse<TxnProcessIdResponse>(responseMessage: "Unable to get process ID.");

            var response = new TxnProcessIdResponse
            {
                ProcessId = processId.ProcessId,
                ResponseCode = ResponseCodes.Code200_Success,
                ResponseStatus = ResponseStatuses.Success,
                ResponseMessage = "Process ID fetched successfully!"
            };

            return (HttpStatusCode.OK, response);
        }

        public async Task<(HttpStatusCode, object)> PushTransactionAsync(PushTransactionRequest request)
        {
            var partnerCode = _httpContextAccessor.HttpContext?.User.FindFirstValue(PartnerClaimTypes.PartnerCode);
            var apiUserName = _httpContextAccessor.HttpContext?.User.FindFirstValue(PartnerClaimTypes.ApiUserName);
            if (string.IsNullOrEmpty(partnerCode))
                return HandleBadRequestResponse<PushTransactionResponse>();

            if (string.IsNullOrEmpty(apiUserName))
                return HandleBadRequestResponse<PushTransactionResponse>();

            var fieldValidationResult = ValidatePushTransactionRequestFields(request);
            if (!fieldValidationResult.Success)
                return HandleBadRequestResponse<PushTransactionResponse>(responseDetailMessage: "One or more field validations failed",
                    fieldErrors: fieldValidationResult.Errors);

            if (!apiUserName.Equals(request.ApiUserName))
                return HandleBadRequestResponse<PushTransactionResponse>(responseDetailMessage: "One or more field validations failed",
                    fieldErrors: new() { new() { Field = nameof(request.Signature), Message = "Invalid ApiUserName." } });

            var paymentTypes = new string[] { PayoutTypes.Wallet, PayoutTypes.Bank, PayoutTypes.Cash };
            var paymentType = paymentTypes.FirstOrDefault(p => p.Equals(request.PaymentType, StringComparison.OrdinalIgnoreCase));
            if (paymentType == null)
                return HandleBadRequestResponse<PushTransactionResponse>(responseDetailMessage: "One or more field validations failed",
                    fieldErrors: new() { new() { Field = nameof(request.PaymentType), Message = "Invalid PaymentType." } });

            var signatureData = paymentType switch
            {
                PayoutTypes.Wallet => $"{request.ApiUserName},{request.ProcessId},{request.PartnerTransactionId},{request.PaymentType},{request.SourceCurrency},{request.DestinationCurrency},{request.SendingAmount},{request.SenderFirstName},{request.SenderLastName},{request.SenderCountry},{request.SenderAddress},{request.RecipientType},{request.RecipientFirstName},{request.RecipientLastName},{request.RecipientCountry},{request.WalletCode},{request.WalletNumber},{request.WalletHolderName}",
                PayoutTypes.Bank => $"{request.ApiUserName},{request.ProcessId},{request.PartnerTransactionId},{request.PaymentType},{request.SourceCurrency},{request.DestinationCurrency},{request.SendingAmount},{request.SenderFirstName},{request.SenderLastName},{request.SenderCountry},{request.SenderAddress},{request.RecipientType},{request.RecipientFirstName},{request.RecipientLastName},{request.RecipientCountry},{request.BankCode},{request.AccountNumber},{request.AccountHolderName}",
                PayoutTypes.Cash => $"{request.ApiUserName},{request.ProcessId},{request.PartnerTransactionId},{request.PaymentType},{request.SourceCurrency},{request.DestinationCurrency},{request.SendingAmount},{request.SenderFirstName},{request.SenderLastName},{request.SenderCountry},{request.SenderAddress},{request.RecipientType},{request.RecipientFirstName},{request.RecipientLastName},{request.RecipientCountry}",
                _ => throw new MpmtException("Invalid PaymentType.")
            };

            var isSignatureValid = await IsSignatureValidAsync(request.Signature, signatureData);
            if (!isSignatureValid)
                return HandleInvalidSignatureTokenResponse<PushTransactionResponse>();

            var pushTxnParam = MapToPushTransactionParam(request);
            pushTxnParam.TransactionType = "APITRANSACTION"; // constant for API transaction
            pushTxnParam.PartnerCode = partnerCode;
            pushTxnParam.IpAddress = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();
            pushTxnParam.LoggedInUser = _httpContextAccessor.HttpContext?.User.FindFirstValue(PartnerClaimTypes.UserName);
            pushTxnParam.UserType = _httpContextAccessor.HttpContext?.User.FindFirstValue(PartnerClaimTypes.UserType);

            var (pushTxnStatus, pushTxnDetails) = await _partnerApiRepository.PushTransactionAsync(pushTxnParam);

            // if StatusCode = 411, then ProcessId is invalid
            if (pushTxnStatus.StatusCode == 411)
                return (HttpStatusCode.BadRequest, new PushTransactionResponse
                {
                    ResponseCode = ResponseCodes.CodeE602_InvalidProcessId,
                    ResponseStatus = ResponseStatuses.Error,
                    ResponseMessage = ResponseMessages.MsgE602_InvalidProcessId
                });

            if (pushTxnStatus.StatusCode != 200 || pushTxnDetails is null)
                return HandleBadRequestResponse<PushTransactionResponse>(responseMessage: pushTxnStatus.MsgText);

            // TODO: Check the response here

            var txnResultDetails = MapToAddTransactionResultDetails(pushTxnDetails);

            _ = _partnerPayoutHandlerService.HandleSenderTransactionNotificationEmailing(txnResultDetails);
            _ = _partnerPayoutHandlerService.HandlePartnerLowWalletBalanceNotificationEmailing(txnResultDetails);
            _ = _partnerPayoutHandlerService.HandlePartnerRemainingFeeBalanceNotificationEmailing(txnResultDetails);

            // check if 
            if (pushTxnDetails.ComplianceFlag is true)
                return await HandleSuccessPushTxnResponse(pushTxnDetails);

            if (!PartnerPayoutHelper.IsPayoutProceedable(txnResultDetails))
                return await HandleSuccessPushTxnResponse(pushTxnDetails);

            switch (pushTxnDetails.PaymentType)
            {
                case PayoutTypes.Wallet:
                    var walletPayoutResult = await _partnerPayoutHandlerService.HandlePayoutToWalletAsync(txnResultDetails);
                    // Just return success response
                    return await HandleSuccessPushTxnResponse(pushTxnDetails);
                case PayoutTypes.Bank:
                    var bankPayoutResult = await _partnerPayoutHandlerService.HandlePayoutToBankAsync(txnResultDetails);
                    // Just return success response
                    return await HandleSuccessPushTxnResponse(pushTxnDetails);
                default:
                    // Just return success response
                    return await HandleSuccessPushTxnResponse(pushTxnDetails);
            };
        }

        public async Task<(HttpStatusCode, object)> GetTransactionStatusAsync(TransactionStatusRequest request)
        {
            var apiUserName = _httpContextAccessor.HttpContext?.User.FindFirstValue(PartnerClaimTypes.ApiUserName);
            var partnerCode = _httpContextAccessor.HttpContext?.User.FindFirstValue(PartnerClaimTypes.PartnerCode);

            if (string.IsNullOrEmpty(apiUserName))
                return HandleBadRequestResponse<TransactionStatusResponse>();

            if (string.IsNullOrEmpty(partnerCode))
                return HandleBadRequestResponse<TransactionStatusResponse>();

            var fieldValidationResult = ValidateGetTxnChargeDetailsRequestFields(request);
            if (!fieldValidationResult.Success)
                return HandleBadRequestResponse<TxnChargeDetails>(responseDetailMessage: "One or more field validations failed",
                    fieldErrors: fieldValidationResult.Errors);

            if (!apiUserName.Equals(request.ApiUserName))
                return HandleBadRequestResponse<TxnChargeDetails>(responseDetailMessage: "One or more field validations failed",
                    fieldErrors: new() { new() { Field = nameof(request.ApiUserName), Message = "Invalid ApiUserName." } });

            var signatureData = $"{request.ApiUserName},{request.TransactionId}";
            var isSignatureValid = await IsSignatureValidAsync(request.Signature, signatureData);

            if (!isSignatureValid)
                return HandleInvalidSignatureTokenResponse<TransactionStatusResponse>();

            var txnStatusDetails = await _partnerApiRepository.GetTransactionStatusAsync(partnerCode, request.TransactionId);
            if (txnStatusDetails is null)
                return HandleBadRequestResponse<TransactionStatusResponse>(responseMessage: "Transaction not found.");

            var response = new TransactionStatusResponse
            {
                ResponseCode = ResponseCodes.Code200_Success,
                ResponseStatus = ResponseStatuses.Success,
                ResponseMessage = "Transaction status fetched successfully!",
                ResponseDetailMessage = $"Transaction status with Id {txnStatusDetails.TransactionId} fetched successfully!",
                TransactionId = txnStatusDetails.TransactionId,
                PartnerTransactionId = txnStatusDetails.PartnerTransactionId,
                PaymentType = txnStatusDetails.PaymentType,
                DebitStatusCode = txnStatusDetails.DebitStatusCode,
                ComplianceStatusCode = txnStatusDetails.ComplianceStatusCode,
                PayoutStatusCode = txnStatusDetails.PayoutStatusCode,
                TransactionDate = txnStatusDetails.TransactionDate?.ToString("yyyy-MM-ddTHH:mm:ss.fff"),
                PayoutDate = txnStatusDetails.PayoutDate?.ToString("yyyy-MM-ddTHH:mm:ss.fff"),
                TransactionStateRemarks = txnStatusDetails.TransactionStateRemarks
            };

            return (HttpStatusCode.OK, response);
        }

        public async Task<(HttpStatusCode, object)> ValidateAccountAsync(ValidateAccountRequest request)
        {
            var apiUserName = _httpContextAccessor.HttpContext?.User.FindFirstValue(PartnerClaimTypes.ApiUserName);
            var partnerCode = _httpContextAccessor.HttpContext?.User.FindFirstValue(PartnerClaimTypes.PartnerCode);

            if (string.IsNullOrEmpty(apiUserName))
                return HandleBadRequestResponse<ValidateAccountResponse>();

            if (string.IsNullOrEmpty(partnerCode))
                return HandleBadRequestResponse<ValidateAccountResponse>();

            var fieldValidationResult = ValidateAccountRequestFields(request);
            if (!fieldValidationResult.Success)
                return HandleBadRequestResponse<ValidateAccountResponse>(responseDetailMessage: "One or more field validations failed",
                    fieldErrors: fieldValidationResult.Errors);

            if (!apiUserName.Equals(request.ApiUserName))
                return HandleBadRequestResponse<ValidateAccountResponse>(responseDetailMessage: "One or more field validations failed",
                    fieldErrors: new() { new() { Field = nameof(request.ApiUserName), Message = "Invalid ApiUserName." } });

            var signatureData = request.PaymentType.ToUpper() switch
            {
                PayoutTypes.Wallet => $"{request.ApiUserName},{request.PaymentType},{request.ReferenceId},{request.WalletCode},{request.AccountNumber}",
                PayoutTypes.Bank => $"{request.ApiUserName},{request.PaymentType},{request.ReferenceId},{request.BankCode},{request.AccountName},{request.AccountNumber},{request.Amount}",
                _ => throw new MpmtException("Invalid PaymentType.")
            };
            var isSignatureValid = await IsSignatureValidAsync(request.Signature, signatureData);

            if (!isSignatureValid)
                return HandleInvalidSignatureTokenResponse<ValidateAccountResponse>();
            if (request.PaymentType.ToUpper() == "WALLET" || request.PaymentType.ToUpper() == "BANK")
            {
                var status = await _partnerApiRepository.validateReferenceNumber(request, partnerCode);
                if (status.StatusCode != 200)
                    return HandleBadRequestResponse<ValidateAccountResponse>(responseDetailMessage: "One or more field validations failed",
                fieldErrors: new() { new() { Field = nameof(request.ReferenceId), Message = status.MsgText } });
            }
            return request.PaymentType.ToUpper() switch
            {
                PayoutTypes.Wallet => await HandleWalletAccountValidationAsync(request),
                PayoutTypes.Bank => await HandleBankAccountValidationAsync(request),
                _ => HandleBadRequestResponse<ValidateAccountResponse>(responseMessage: "Invalid PaymentType")
            };
        }

        private async Task<(HttpStatusCode, ValidateAccountResponse)> HandleWalletAccountValidationAsync(ValidateAccountRequest request)
        {
            switch (request.WalletCode)
            {
                case WalletTypes.MyPay:
                    var reqValidateWalletAccount = new MyPayValidateWalletUserDto
                    {
                        WalletNumber = request.AccountNumber,
                        Reference = request.ReferenceId,
                    };

                    var (httpStatus, response) = await _myPayWalletLoadApiService.ValidateWalletUserAsync(reqValidateWalletAccount);

                    if (httpStatus == HttpStatusCode.GatewayTimeout)
                        return HandleGatewayTimeoutResponse<ValidateAccountResponse>();

                    return MapToValidateAccountResponse(response);
                default:
                    return HandleBadRequestResponse<ValidateAccountResponse>(responseMessage: "Invalid WalletCode");
            }
        }

        private async Task<(HttpStatusCode, ValidateAccountResponse)> HandleBankAccountValidationAsync(ValidateAccountRequest request)
        {
            var reqValidateBankAccount = new MyPayValidateBankUserDto
            {
                AccountName = request.AccountName,
                AccountNumber = request.AccountNumber,
                Amount = decimal.Parse(request.Amount),
                BankCode = request.BankCode,
                Reference = request.ReferenceId
            };

            var (httpStatus, response) = await _myPayBankLoadApiService.ValidateBankUserAsync(reqValidateBankAccount);

            if (httpStatus == HttpStatusCode.GatewayTimeout)
                return HandleGatewayTimeoutResponse<ValidateAccountResponse>();

            return MapToValidateAccountResponse(response);
        }

        private async Task<bool> IsSignatureValidAsync(string signature, string payloadData)
        {
            if (signature is null || payloadData is null)
                return false;

            var userPublicKey = _httpContextAccessor.HttpContext?.User.FindFirstValue(PartnerClaimTypes.UserPublicKey);
            if (string.IsNullOrWhiteSpace(userPublicKey))
                return false;

            try
            {
                var rsaKeyParam = RsaCryptoUtils.ImportPublicKeyPem(userPublicKey);

                var signatureBytes = Convert.FromBase64String(signature);
                var dataBytes = Encoding.UTF8.GetBytes(payloadData);

                return RsaCryptoUtils.VerifySignature(dataBytes, signatureBytes, rsaKeyParam);
            }
            catch (Exception ex)
            {
                await _exceptionLogger.LogAsync(ex);
                return false;
            }
        }

        private FieldValidationResult ValidateGetInstrumentListRequestFields(GetInstrumentListsRequest request)
        {
            var result = new FieldValidationResult();

            if (string.IsNullOrWhiteSpace(request.ApiUserName))
                result.AddError(new() { Field = nameof(request.ApiUserName), Message = $"{nameof(request.ApiUserName)} is required" });

            if (string.IsNullOrWhiteSpace(request.Signature))
                result.AddError(new() { Field = nameof(request.Signature), Message = $"{nameof(request.Signature)} is required" });

            return result;
        }

        private FieldValidationResult ValidateGetTxnChargeDetailsRequestFields(GetTxnChargeDetailsRequest request)
        {
            var result = new FieldValidationResult();

            if (string.IsNullOrWhiteSpace(request.ApiUserName))
                result.AddError(new() { Field = nameof(request.ApiUserName), Message = $"{nameof(request.ApiUserName)} is required" });

            if (string.IsNullOrWhiteSpace(request.SourceCurrency))
                result.AddError(new() { Field = nameof(request.SourceCurrency), Message = $"{nameof(request.SourceCurrency)} is required" });

            if (string.IsNullOrWhiteSpace(request.SourceAmount))
                result.AddError(new() { Field = nameof(request.SourceAmount), Message = $"{nameof(request.SourceAmount)} is required" });

            if (!decimal.TryParse(request.SourceAmount, out var sourceAmount) || sourceAmount <= 0) // greater than 0
                result.AddError(new() { Field = nameof(request.SourceAmount), Message = $"Invalid {nameof(request.SourceAmount)}" });

            if (string.IsNullOrWhiteSpace(request.PaymentType))
                result.AddError(new() { Field = nameof(request.PaymentType), Message = $"{nameof(request.PaymentType)} is required" });

            if (string.IsNullOrWhiteSpace(request.DestinationCurrency))
                result.AddError(new() { Field = nameof(request.DestinationCurrency), Message = $"{nameof(request.DestinationCurrency)} is required" });

            if (string.IsNullOrWhiteSpace(request.Signature))
                result.AddError(new() { Field = nameof(request.Signature), Message = $"{nameof(request.Signature)} is required" });

            return result;
        }

        private FieldValidationResult ValidateGetTxnProcessIdRequestFields(GetProcessIdRequest request)
        {
            var result = new FieldValidationResult();

            if (string.IsNullOrWhiteSpace(request.ApiUserName))
                result.AddError(new() { Field = nameof(request.ApiUserName), Message = $"{nameof(request.ApiUserName)} is required" });

            if (string.IsNullOrWhiteSpace(request.ReferenceId))
                result.AddError(new() { Field = nameof(request.ReferenceId), Message = $"{nameof(request.ReferenceId)} is required" });

            if (string.IsNullOrWhiteSpace(request.Signature))
                result.AddError(new() { Field = nameof(request.Signature), Message = $"{nameof(request.Signature)} is required" });

            return result;
        }

        private FieldValidationResult ValidatePushTransactionRequestFields(PushTransactionRequest request)
        {
            var result = new FieldValidationResult();

            if (string.IsNullOrWhiteSpace(request.ApiUserName))
                result.AddError(new() { Field = nameof(request.ApiUserName), Message = $"{nameof(request.ApiUserName)} is required" });

            if (string.IsNullOrWhiteSpace(request.ProcessId))
                result.AddError(new() { Field = nameof(request.ProcessId), Message = $"{nameof(request.ProcessId)} is required" });

            if (string.IsNullOrWhiteSpace(request.PartnerTransactionId))
                result.AddError(new() { Field = nameof(request.PartnerTransactionId), Message = $"{nameof(request.PartnerTransactionId)} is required" });

            if (string.IsNullOrWhiteSpace(request.PaymentType))
                result.AddError(new() { Field = nameof(request.PaymentType), Message = $"{nameof(request.PaymentType)} is required" });

            if (string.IsNullOrWhiteSpace(request.SourceCurrency))
                result.AddError(new() { Field = nameof(request.SourceCurrency), Message = $"{nameof(request.SourceCurrency)} is required" });

            if (string.IsNullOrWhiteSpace(request.DestinationCurrency))
                result.AddError(new() { Field = nameof(request.DestinationCurrency), Message = $"{nameof(request.DestinationCurrency)} is required" });

            // SendingAmount validation
            if (string.IsNullOrWhiteSpace(request.SendingAmount))
                result.AddError(new() { Field = nameof(request.SendingAmount), Message = $"{nameof(request.SendingAmount)} is required" });

            if (!decimal.TryParse(request.SendingAmount, out var sendingAmount) || sendingAmount <= 0) // greater than 0
                result.AddError(new() { Field = nameof(request.SendingAmount), Message = $"Invalid {nameof(request.SendingAmount)}" });

            // ServiceCharge validation
            //if (string.IsNullOrWhiteSpace(request.ServiceCharge))
            //    result.AddError(new() { Field = nameof(request.ServiceCharge), Message = $"{nameof(request.ServiceCharge)} is required" });

            //if (!decimal.TryParse(request.ServiceCharge, out var serviceCharge) || serviceCharge < 0) // 0 or more
            //    result.AddError(new() { Field = nameof(request.ServiceCharge), Message = $"Invalid {nameof(request.ServiceCharge)}" });

            //// NetSendingAmount validation
            //if (string.IsNullOrWhiteSpace(request.NetSendingAmount))
            //    result.AddError(new() { Field = nameof(request.NetSendingAmount), Message = $"{nameof(request.NetSendingAmount)} is required" });

            //if (!decimal.TryParse(request.NetSendingAmount, out var netSendingAmount) || netSendingAmount <= 0) // greater than 0
            //    result.AddError(new() { Field = nameof(request.NetSendingAmount), Message = $"Invalid {nameof(request.NetSendingAmount)}" });

            //// ConversionRate validation
            //if (string.IsNullOrWhiteSpace(request.ConversionRate))
            //    result.AddError(new() { Field = nameof(request.ConversionRate), Message = $"{nameof(request.ConversionRate)} is required" });

            //if (!decimal.TryParse(request.ConversionRate, out var conversionRate) || conversionRate <= 0) // greater than 0
            //    result.AddError(new() { Field = nameof(request.ConversionRate), Message = $"Invalid {nameof(request.ConversionRate)}" });

            //// NetRecievingAmountNPR validation
            //if (string.IsNullOrWhiteSpace(request.NetRecievingAmountNPR))
            //    result.AddError(new() { Field = nameof(request.NetRecievingAmountNPR), Message = $"{nameof(request.NetRecievingAmountNPR)} is required" });

            //if (!decimal.TryParse(request.NetRecievingAmountNPR, out var netRecievingAmountNPR) || netRecievingAmountNPR <= 0) // greater than 0
            //    result.AddError(new() { Field = nameof(request.NetRecievingAmountNPR), Message = $"Invalid {nameof(request.NetRecievingAmountNPR)}" });

            //// PartnerServiceCharge validation
            //if (string.IsNullOrWhiteSpace(request.PartnerServiceCharge))
            //    result.AddError(new() { Field = nameof(request.PartnerServiceCharge), Message = $"{nameof(request.PartnerServiceCharge)} is required" });

            //if (!decimal.TryParse(request.PartnerServiceCharge, out var partnerServiceCharge) || partnerServiceCharge < 0) // 0 or more
            //    result.AddError(new() { Field = nameof(request.PartnerServiceCharge), Message = $"Invalid {nameof(request.PartnerServiceCharge)}" });

            if (string.IsNullOrWhiteSpace(request.SenderFirstName))
                result.AddError(new() { Field = nameof(request.SenderFirstName), Message = $"{nameof(request.SenderFirstName)} is required" });

            if (string.IsNullOrWhiteSpace(request.SenderLastName))
                result.AddError(new() { Field = nameof(request.SenderLastName), Message = $"{nameof(request.SenderLastName)} is required" });

            if (new string[] { PayoutTypes.Cash, PayoutTypes.Wallet }.Any(p => p.Equals(request.PaymentType, StringComparison.InvariantCultureIgnoreCase)))
            {
                if (!string.IsNullOrWhiteSpace(request.RecipientDateOfBirth))
                {
                    if (!DateTime.TryParseExact(request.RecipientDateOfBirth, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
                        result.AddError(new() { Field = nameof(request.RecipientDateOfBirth), Message = $"{nameof(request.RecipientDateOfBirth)} is required" });
                }

                if (string.IsNullOrWhiteSpace(request.SenderContactNumber))
                    result.AddError(new() { Field = nameof(request.SenderContactNumber), Message = $"{nameof(request.SenderContactNumber)} is required" });
            }

            // TODO: check if required
            //SenderEmail

            if (string.IsNullOrWhiteSpace(request.SenderCountry))
                result.AddError(new() { Field = nameof(request.SenderCountry), Message = $"{nameof(request.SenderCountry)} is required" });

            // TODO: check if required
            //SenderProvince
            //SenderCity
            //SenderZipcode

            if (string.IsNullOrWhiteSpace(request.SenderAddress))
                result.AddError(new() { Field = nameof(request.SenderAddress), Message = $"{nameof(request.SenderAddress)} is required" });

            // SenderDocumentType
            if (string.IsNullOrWhiteSpace(request.SenderDocumentType))
                result.AddError(new() { Field = nameof(request.SenderDocumentType), Message = $"{nameof(request.SenderDocumentType)} is required" });

            // SenderDocumentNumber
            if (string.IsNullOrWhiteSpace(request.SenderDocumentNumber))
                result.AddError(new() { Field = nameof(request.SenderDocumentNumber), Message = $"{nameof(request.SenderDocumentNumber)} is required" });

            // SenderRelationshipWithRecipient validation
            if (string.IsNullOrWhiteSpace(request.SenderRelationshipWithRecipient))
                result.AddError(new() { Field = nameof(request.SenderRelationshipWithRecipient), Message = $"{nameof(request.SenderRelationshipWithRecipient)} is required" });

            // SenderOccupation validation
            if (string.IsNullOrWhiteSpace(request.SenderOccupation))
                result.AddError(new() { Field = nameof(request.SenderOccupation), Message = $"{nameof(request.SenderOccupation)} is required" });

            // SenderSourceOfIncome validation
            if (string.IsNullOrWhiteSpace(request.SenderSourceOfIncome))
                result.AddError(new() { Field = nameof(request.SenderSourceOfIncome), Message = $"{nameof(request.SenderSourceOfIncome)} is required" });

            // SenderPurposeOfRemittance validation
            if (string.IsNullOrWhiteSpace(request.SenderPurposeOfRemittance))
                result.AddError(new() { Field = nameof(request.SenderPurposeOfRemittance), Message = $"{nameof(request.SenderPurposeOfRemittance)} is required" });

            // TODO: check if required
            //SenderRemarks

            if (string.IsNullOrWhiteSpace(request.RecipientType))
                result.AddError(new() { Field = nameof(request.RecipientType), Message = $"{nameof(request.RecipientType)} is required" });

            if (string.IsNullOrWhiteSpace(request.RecipientFirstName))
                result.AddError(new() { Field = nameof(request.RecipientFirstName), Message = $"{nameof(request.RecipientFirstName)} is required" });

            if (string.IsNullOrWhiteSpace(request.RecipientLastName))
                result.AddError(new() { Field = nameof(request.RecipientLastName), Message = $"{nameof(request.RecipientLastName)} is required" });

            if (request.RecipientType.Equals(RecipientTypes.Joint, StringComparison.OrdinalIgnoreCase))
            {
                if (string.IsNullOrWhiteSpace(request.JointAccountFirstName))
                    result.AddError(new() { Field = nameof(request.JointAccountFirstName), Message = $"{nameof(request.JointAccountFirstName)} is required" });

                if (string.IsNullOrWhiteSpace(request.JointAccountLastName))
                    result.AddError(new() { Field = nameof(request.JointAccountLastName), Message = $"{nameof(request.JointAccountLastName)} is required" });
            }

            if (request.RecipientType.Equals(RecipientTypes.CharityOrCorporate, StringComparison.OrdinalIgnoreCase))
            {
                if (string.IsNullOrWhiteSpace(request.BusinessName))
                    result.AddError(new() { Field = nameof(request.BusinessName), Message = $"{nameof(request.BusinessName)} is required" });
            }

            // TODO: check if required
            //RecipientContactNumber
            //RecipientEmail


            // RecipientCountry validation
            if (string.IsNullOrWhiteSpace(request.RecipientCountry))
                result.AddError(new() { Field = nameof(request.RecipientCountry), Message = $"{nameof(request.RecipientCountry)} is required" });

            // TODO: check if required
            //RecipientCity
            //RecipientZipcode
            //RecipientAddress

            // RecipientRelationshipWithSender validation
            if (string.IsNullOrWhiteSpace(request.RecipientRelationshipWithSender))
                result.AddError(new() { Field = nameof(request.RecipientRelationshipWithSender), Message = $"{nameof(request.RecipientRelationshipWithSender)} is required" });

            // Payout Type: Bank
            if (request.PaymentType.Equals(PayoutTypes.Bank, StringComparison.OrdinalIgnoreCase))
            {
                if (string.IsNullOrWhiteSpace(request.BankCode))
                    result.AddError(new() { Field = nameof(request.BankCode), Message = $"{nameof(request.BankCode)} is required" });

                if (string.IsNullOrWhiteSpace(request.Branch))
                    result.AddError(new() { Field = nameof(request.Branch), Message = $"{nameof(request.Branch)} is required" });

                if (string.IsNullOrWhiteSpace(request.AccountHolderName))
                    result.AddError(new() { Field = nameof(request.AccountHolderName), Message = $"{nameof(request.AccountHolderName)} is required" });

                if (string.IsNullOrWhiteSpace(request.AccountNumber))
                    result.AddError(new() { Field = nameof(request.AccountNumber), Message = $"{nameof(request.AccountNumber)} is required" });
            }

            // Payout Type: Wallet
            if (request.PaymentType.Equals(PayoutTypes.Wallet, StringComparison.OrdinalIgnoreCase))
            {
                if (string.IsNullOrWhiteSpace(request.WalletCode))
                    result.AddError(new() { Field = nameof(request.WalletCode), Message = $"{nameof(request.WalletCode)} is required" });

                if (string.IsNullOrWhiteSpace(request.WalletNumber))
                    result.AddError(new() { Field = nameof(request.WalletNumber), Message = $"{nameof(request.WalletNumber)} is required" });

                if (string.IsNullOrWhiteSpace(request.WalletHolderName))
                    result.AddError(new() { Field = nameof(request.WalletHolderName), Message = $"{nameof(request.WalletHolderName)} is required" });
            }

            // Signature
            if (string.IsNullOrWhiteSpace(request.Signature))
                result.AddError(new() { Field = nameof(request.Signature), Message = $"{nameof(request.Signature)} is required" });

            return result;
        }

        private FieldValidationResult ValidatePushTransactionRequestDetailsFields(PushTransactionRequestDetals request)
        {
            var result = new FieldValidationResult();

            if (string.IsNullOrWhiteSpace(request.ApiUserName))
                result.AddError(new() { Field = nameof(request.ApiUserName), Message = $"{nameof(request.ApiUserName)} is required" });

            if (string.IsNullOrWhiteSpace(request.ProcessId))
                result.AddError(new() { Field = nameof(request.ProcessId), Message = $"{nameof(request.ProcessId)} is required" });

            if (string.IsNullOrWhiteSpace(request.PartnerTransactionId))
                result.AddError(new() { Field = nameof(request.PartnerTransactionId), Message = $"{nameof(request.PartnerTransactionId)} is required" });

            if (string.IsNullOrWhiteSpace(request.PaymentType))
                result.AddError(new() { Field = nameof(request.PaymentType), Message = $"{nameof(request.PaymentType)} is required" });

            if (string.IsNullOrWhiteSpace(request.SourceCurrency))
                result.AddError(new() { Field = nameof(request.SourceCurrency), Message = $"{nameof(request.SourceCurrency)} is required" });

            if (string.IsNullOrWhiteSpace(request.DestinationCurrency))
                result.AddError(new() { Field = nameof(request.DestinationCurrency), Message = $"{nameof(request.DestinationCurrency)} is required" });

            // SendingAmount validation
            if (string.IsNullOrWhiteSpace(request.SendingAmount))
                result.AddError(new() { Field = nameof(request.SendingAmount), Message = $"{nameof(request.SendingAmount)} is required" });

            if (!decimal.TryParse(request.SendingAmount, out var sendingAmount) || sendingAmount <= 0) // greater than 0
                result.AddError(new() { Field = nameof(request.SendingAmount), Message = $"Invalid {nameof(request.SendingAmount)}" });

            //ConversionRate validation
            if (string.IsNullOrWhiteSpace(request.NetReceivingAmount))
                result.AddError(new() { Field = nameof(request.NetReceivingAmount), Message = $"{nameof(request.NetReceivingAmount)} is required" });
            else
            {
                // If ConversionRate is not null or empty, try to parse it into a numeric value.
                if (!decimal.TryParse(request.NetReceivingAmount, out decimal conversionRate))
                {
                    // If parsing fails, it means the ConversionRate is invalid.
                    result.AddError(new() { Field = nameof(request.NetReceivingAmount), Message = $"Invalid {nameof(request.NetReceivingAmount)}" });
                }
                else
                {
                    // If parsing succeeds, check if the parsed value is negative.
                    if (conversionRate < 0)
                    {
                        // If the value is negative, add an error indicating it.
                        result.AddError(new() { Field = nameof(request.NetReceivingAmount), Message = $"{nameof(request.NetReceivingAmount)} cannot be negative" });
                    }
                    else if (conversionRate == 0)
                    {
                        // If the value is negative, add an error indicating it.
                        result.AddError(new() { Field = nameof(request.NetReceivingAmount), Message = $"{nameof(request.NetReceivingAmount)} should be greater than 0" });
                    }
                }
            }

            if (string.IsNullOrWhiteSpace(request.SenderFirstName))
                result.AddError(new() { Field = nameof(request.SenderFirstName), Message = $"{nameof(request.SenderFirstName)} is required" });

            if (string.IsNullOrWhiteSpace(request.SenderLastName))
                result.AddError(new() { Field = nameof(request.SenderLastName), Message = $"{nameof(request.SenderLastName)} is required" });

            if (new string[] { PayoutTypes.Cash, PayoutTypes.Wallet }.Any(p => p.Equals(request.PaymentType, StringComparison.InvariantCultureIgnoreCase)))
            {
                if (!string.IsNullOrWhiteSpace(request.RecipientDateOfBirth))
                {
                    if (!DateTime.TryParseExact(request.RecipientDateOfBirth, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
                        result.AddError(new() { Field = nameof(request.RecipientDateOfBirth), Message = $"{nameof(request.RecipientDateOfBirth)} is required" });
                }

                if (string.IsNullOrWhiteSpace(request.SenderContactNumber))
                    result.AddError(new() { Field = nameof(request.SenderContactNumber), Message = $"{nameof(request.SenderContactNumber)} is required" });
            }
            // TODO: check if required
            //SenderEmail

            if (string.IsNullOrWhiteSpace(request.SenderCountry))
                result.AddError(new() { Field = nameof(request.SenderCountry), Message = $"{nameof(request.SenderCountry)} is required" });
            // TODO: check if required          
            if (string.IsNullOrWhiteSpace(request.SenderAddress))
                result.AddError(new() { Field = nameof(request.SenderAddress), Message = $"{nameof(request.SenderAddress)} is required" });

            // SenderDocumentType
            if (string.IsNullOrWhiteSpace(request.SenderDocumentType))
                result.AddError(new() { Field = nameof(request.SenderDocumentType), Message = $"{nameof(request.SenderDocumentType)} is required" });

            // SenderDocumentNumber
            if (string.IsNullOrWhiteSpace(request.SenderDocumentNumber))
                result.AddError(new() { Field = nameof(request.SenderDocumentNumber), Message = $"{nameof(request.SenderDocumentNumber)} is required" });

            // SenderRelationshipWithRecipient validation
            if (string.IsNullOrWhiteSpace(request.SenderRelationshipWithRecipient))
                result.AddError(new() { Field = nameof(request.SenderRelationshipWithRecipient), Message = $"{nameof(request.SenderRelationshipWithRecipient)} is required" });

            // SenderOccupation validation
            if (string.IsNullOrWhiteSpace(request.SenderOccupation))
                result.AddError(new() { Field = nameof(request.SenderOccupation), Message = $"{nameof(request.SenderOccupation)} is required" });

            // SenderSourceOfIncome validation
            if (string.IsNullOrWhiteSpace(request.SenderSourceOfIncome))
                result.AddError(new() { Field = nameof(request.SenderSourceOfIncome), Message = $"{nameof(request.SenderSourceOfIncome)} is required" });

            // SenderPurposeOfRemittance validation
            if (string.IsNullOrWhiteSpace(request.SenderPurposeOfRemittance))
                result.AddError(new() { Field = nameof(request.SenderPurposeOfRemittance), Message = $"{nameof(request.SenderPurposeOfRemittance)} is required" });

            // TODO: check if required
            //SenderRemarks

            if (string.IsNullOrWhiteSpace(request.RecipientType))
                result.AddError(new() { Field = nameof(request.RecipientType), Message = $"{nameof(request.RecipientType)} is required" });

            if (string.IsNullOrWhiteSpace(request.RecipientFirstName))
                result.AddError(new() { Field = nameof(request.RecipientFirstName), Message = $"{nameof(request.RecipientFirstName)} is required" });

            if (string.IsNullOrWhiteSpace(request.RecipientLastName))
                result.AddError(new() { Field = nameof(request.RecipientLastName), Message = $"{nameof(request.RecipientLastName)} is required" });

            if (request.RecipientType.Equals(RecipientTypes.Joint, StringComparison.OrdinalIgnoreCase))
            {
                if (string.IsNullOrWhiteSpace(request.JointAccountFirstName))
                    result.AddError(new() { Field = nameof(request.JointAccountFirstName), Message = $"{nameof(request.JointAccountFirstName)} is required" });

                if (string.IsNullOrWhiteSpace(request.JointAccountLastName))
                    result.AddError(new() { Field = nameof(request.JointAccountLastName), Message = $"{nameof(request.JointAccountLastName)} is required" });
            }

            if (request.RecipientType.Equals(RecipientTypes.CharityOrCorporate, StringComparison.OrdinalIgnoreCase))
            {
                if (string.IsNullOrWhiteSpace(request.BusinessName))
                    result.AddError(new() { Field = nameof(request.BusinessName), Message = $"{nameof(request.BusinessName)} is required" });
            }

            // TODO: check if required

            // RecipientCountry validation
            if (string.IsNullOrWhiteSpace(request.RecipientCountry))
                result.AddError(new() { Field = nameof(request.RecipientCountry), Message = $"{nameof(request.RecipientCountry)} is required" });

            // TODO: check if required

            // RecipientRelationshipWithSender validation
            if (string.IsNullOrWhiteSpace(request.RecipientRelationshipWithSender))
                result.AddError(new() { Field = nameof(request.RecipientRelationshipWithSender), Message = $"{nameof(request.RecipientRelationshipWithSender)} is required" });
            // Payout Type: Bank
            if (request.PaymentType.Equals(PayoutTypes.Bank, StringComparison.OrdinalIgnoreCase))
            {
                if (string.IsNullOrWhiteSpace(request.BankCode))
                    result.AddError(new() { Field = nameof(request.BankCode), Message = $"{nameof(request.BankCode)} is required" });

                if (string.IsNullOrWhiteSpace(request.Branch))
                    result.AddError(new() { Field = nameof(request.Branch), Message = $"{nameof(request.Branch)} is required" });

                if (string.IsNullOrWhiteSpace(request.AccountHolderName))
                    result.AddError(new() { Field = nameof(request.AccountHolderName), Message = $"{nameof(request.AccountHolderName)} is required" });

                if (string.IsNullOrWhiteSpace(request.AccountNumber))
                    result.AddError(new() { Field = nameof(request.AccountNumber), Message = $"{nameof(request.AccountNumber)} is required" });
            }
            // Payout Type: Wallet
            if (request.PaymentType.Equals(PayoutTypes.Wallet, StringComparison.OrdinalIgnoreCase))
            {
                if (string.IsNullOrWhiteSpace(request.WalletCode))
                    result.AddError(new() { Field = nameof(request.WalletCode), Message = $"{nameof(request.WalletCode)} is required" });

                if (string.IsNullOrWhiteSpace(request.WalletNumber))
                    result.AddError(new() { Field = nameof(request.WalletNumber), Message = $"{nameof(request.WalletNumber)} is required" });

                if (string.IsNullOrWhiteSpace(request.WalletHolderName))
                    result.AddError(new() { Field = nameof(request.WalletHolderName), Message = $"{nameof(request.WalletHolderName)} is required" });
            }
            // Signature
            if (string.IsNullOrWhiteSpace(request.Signature))
                result.AddError(new() { Field = nameof(request.Signature), Message = $"{nameof(request.Signature)} is required" });
            return result;
        }

        private FieldValidationResult ValidateGetTxnChargeDetailsRequestFields(TransactionStatusRequest request)
        {
            var result = new FieldValidationResult();

            if (string.IsNullOrWhiteSpace(request.ApiUserName))
                result.AddError(new() { Field = nameof(request.ApiUserName), Message = $"{nameof(request.ApiUserName)} is required" });

            if (string.IsNullOrWhiteSpace(request.TransactionId))
                result.AddError(new() { Field = nameof(request.TransactionId), Message = $"{nameof(request.TransactionId)} is required" });

            if (string.IsNullOrWhiteSpace(request.Signature))
                result.AddError(new() { Field = nameof(request.Signature), Message = $"{nameof(request.Signature)} is required" });

            return result;
        }

        private FieldValidationResult ValidateAccountRequestFields(ValidateAccountRequest request)
        {
            var result = new FieldValidationResult();

            if (string.IsNullOrWhiteSpace(request.ApiUserName))
                result.AddError(new() { Field = nameof(request.ApiUserName), Message = $"{nameof(request.ApiUserName)} is required" });

            if (string.IsNullOrWhiteSpace(request.PaymentType))
                result.AddError(new() { Field = nameof(request.PaymentType), Message = $"{nameof(request.PaymentType)} is required" });

            // Accepted Payment Types validation
            if (!string.IsNullOrWhiteSpace(request.PaymentType) &&
                !new string[] { PayoutTypes.Bank, PayoutTypes.Wallet }.Any(p => p.Equals(request.PaymentType, StringComparison.InvariantCultureIgnoreCase)))
                result.AddError(new() { Field = nameof(request.PaymentType), Message = $"Invalid {nameof(request.PaymentType)}" });

            if (string.IsNullOrWhiteSpace(request.ReferenceId))
                result.AddError(new() { Field = nameof(request.ReferenceId), Message = $"{nameof(request.ReferenceId)} is required" });

            if (string.IsNullOrWhiteSpace(request.AccountNumber))
                result.AddError(new() { Field = nameof(request.AccountNumber), Message = $"{nameof(request.AccountNumber)} is required" });

            if (request.PaymentType.Equals(PayoutTypes.Bank, StringComparison.OrdinalIgnoreCase))
            {
                if (string.IsNullOrWhiteSpace(request.BankCode))
                    result.AddError(new() { Field = nameof(request.BankCode), Message = $"{nameof(request.BankCode)} is required" });

                if (string.IsNullOrWhiteSpace(request.AccountName))
                    result.AddError(new() { Field = nameof(request.AccountName), Message = $"{nameof(request.AccountName)} is required" });

                if (string.IsNullOrWhiteSpace(request.Amount))
                    result.AddError(new() { Field = nameof(request.Amount), Message = $"{nameof(request.Amount)} is required" });

                if (!decimal.TryParse(request.Amount, out var amount) || amount <= 0) // greater than 0
                    result.AddError(new() { Field = nameof(request.Amount), Message = $"Invalid {nameof(request.Amount)}" });
            }

            // Payout Type: Wallet
            if (request.PaymentType.Equals(PayoutTypes.Wallet, StringComparison.OrdinalIgnoreCase))
            {
                if (string.IsNullOrWhiteSpace(request.WalletCode))
                    result.AddError(new() { Field = nameof(request.WalletCode), Message = $"{nameof(request.WalletCode)} is required" });

                // more validations related to Wallet here
            }

            if (string.IsNullOrWhiteSpace(request.Signature))
                result.AddError(new() { Field = nameof(request.Signature), Message = $"{nameof(request.Signature)} is required" });

            return result;
        }

        private PushTransactionParam MapToPushTransactionParam(PushTransactionRequest request)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));

            return new PushTransactionParam
            {
                ProcessId = request.ProcessId,
                PartnerTransactionId = request.PartnerTransactionId,
                PaymentType = request.PaymentType,
                SourceCurrency = request.SourceCurrency,
                DestinationCurrency = request.DestinationCurrency,
                SendingAmount = decimal.Parse(request.SendingAmount),
                //ServiceCharge = decimal.Parse(request.ServiceCharge),
                //NetSendingAmount = decimal.Parse(request.NetSendingAmount),
                //ConversionRate = decimal.Parse(request.ConversionRate),
                //NetRecievingAmountNPR = decimal.Parse(request.NetRecievingAmountNPR),
                //PartnerServiceCharge = decimal.Parse(request.PartnerServiceCharge),
                SenderFirstName = request.SenderFirstName,
                SenderLastName = request.SenderLastName,
                SenderContactNumber = request.SenderContactNumber,
                SenderEmail = request.SenderEmail,
                //SenderCountryCode = request.SenderCountryCode,
                SenderCountry = request.SenderCountry,
                SenderProvince = request.SenderProvince,
                SenderCity = request.SenderCity,
                SenderZipcode = request.SenderZipcode,
                SenderAddress = request.SenderAddress,
                //SenderDocumentTypeId = request.SenderDocumentTypeId,
                SenderDocumentType = request.SenderDocumentType,
                SenderDocumentNumber = request.SenderDocumentNumber,
                //SenderRelationshipId = int.Parse(request.SenderRelationshipId),
                //SenderPurposeId = int.Parse(request.SenderPurposeId),
                //SenderOccupationId = request.SenderOccupationId,
                SenderRelationshipWithRecipient = request.SenderRelationshipWithRecipient,
                SenderOccupation = request.SenderOccupation,
                SenderSourceOfIncome = request.SenderSourceOfIncome,
                SenderPurposeOfRemittance = request.SenderPurposeOfRemittance,
                SenderRemarks = request.SenderRemarks,
                RecipientType = request.RecipientType,
                RecipientFirstName = request.RecipientFirstName,
                RecipientLastName = request.RecipientLastName,
                JointAccountFirstName = request.JointAccountFirstName,
                JointAccountLastName = request.JointAccountLastName,
                BusinessName = request.BusinessName,
                RecipientContactNumber = request.RecipientContactNumber,
                RecipientEmail = request.RecipientEmail,
                RecipientDateOfBirth = DateTime.TryParse(request.RecipientDateOfBirth, out var recipientDateOfBirth) ? recipientDateOfBirth : null,
                //RecipientCountryCode = request.RecipientCountryCode,
                RecipientCountry = request.RecipientCountry,
                RecipientCity = request.RecipientCity,
                RecipientZipcode = request.RecipientZipcode,
                RecipientAddress = request.RecipientAddress,
                //RecipientRelationshipId = int.Parse(request.RecipientRelationshipId),
                RecipientRelationshipWithSender = request.RecipientRelationshipWithSender,
                BankCode = request.BankCode,
                Branch = request.Branch,
                AccountHolderName = request.AccountHolderName,
                AccountNumber = request.AccountNumber,
                WalletCode = request.WalletCode,
                WalletNumber = request.WalletNumber,
                WalletHolderName = request.WalletHolderName
            };
        }

        private PushTransactionRequestDetailsParam MapToPushTransactionRequestDetailsParam(PushTransactionRequestDetals request)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));

            return new PushTransactionRequestDetailsParam
            {
                ProcessId = request.ProcessId,
                PartnerTransactionId = request.PartnerTransactionId,
                PaymentType = request.PaymentType,
                SourceCurrency = request.SourceCurrency,
                DestinationCurrency = request.DestinationCurrency,
                SendingAmount = decimal.Parse(request.SendingAmount),
                NetReceivingAmount = decimal.Parse(request.NetReceivingAmount),
                //NetRecievingAmountNPR = decimal.Parse(request.NetRecievingAmountNPR),
                SenderFirstName = request.SenderFirstName,
                SenderLastName = request.SenderLastName,
                SenderContactNumber = request.SenderContactNumber,
                SenderEmail = request.SenderEmail,
                SenderCountry = request.SenderCountry,
                SenderProvince = request.SenderProvince,
                SenderCity = request.SenderCity,
                SenderZipcode = request.SenderZipcode,
                SenderAddress = request.SenderAddress,
                SenderDocumentType = request.SenderDocumentType,
                SenderDocumentNumber = request.SenderDocumentNumber,
                SenderRelationshipWithRecipient = request.SenderRelationshipWithRecipient,
                SenderOccupation = request.SenderOccupation,
                SenderSourceOfIncome = request.SenderSourceOfIncome,
                SenderPurposeOfRemittance = request.SenderPurposeOfRemittance,
                SenderRemarks = request.SenderRemarks,
                RecipientType = request.RecipientType,
                RecipientFirstName = request.RecipientFirstName,
                RecipientLastName = request.RecipientLastName,
                JointAccountFirstName = request.JointAccountFirstName,
                JointAccountLastName = request.JointAccountLastName,
                BusinessName = request.BusinessName,
                RecipientContactNumber = request.RecipientContactNumber,
                RecipientEmail = request.RecipientEmail,
                RecipientDateOfBirth = DateTime.TryParse(request.RecipientDateOfBirth, out var recipientDateOfBirth) ? recipientDateOfBirth : null,
                RecipientCountry = request.RecipientCountry,
                RecipientCity = request.RecipientCity,
                RecipientZipcode = request.RecipientZipcode,
                RecipientAddress = request.RecipientAddress,
                RecipientRelationshipWithSender = request.RecipientRelationshipWithSender,
                BankCode = request.BankCode,
                Branch = request.Branch,
                AccountHolderName = request.AccountHolderName,
                AccountNumber = request.AccountNumber,
                WalletCode = request.WalletCode,
                WalletNumber = request.WalletNumber,
                WalletHolderName = request.WalletHolderName
            };
        }

        private AddTransactionResultDetails MapToAddTransactionResultDetails(PushTransactionDetails details)
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
                SourceCurrency = details.SourceCurrency,
                DestinationCurrency = details.DestinationCurrency,
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
                ComplianceFlag = details.ComplianceFlag,
                PartnerServiceCharge = details.PartnerServiceCharge,
                SenderName = details.SenderName,
                SenderContactNumber = details.SenderContactNumber,
                SenderCountry = details.SenderCountry,
                RecipientCountry = details.RecipientCountry,
                RecipientName = details.RecipientName,
                RecipientContactNumber = details.RecipientContactNumber,
                Commission = details.Commission,
                TransactionDateTime = details.TransactionDateTime,
                flag = "0"
            };
        }

        // for MyPay Wallet Account Validation
        private (HttpStatusCode, ValidateAccountResponse) MapToValidateAccountResponse(MyPayValidateWalletUserApiResponse source)
        {
            ArgumentNullException.ThrowIfNull(source, nameof(source));

            var response = new ValidateAccountResponse();

            // Account is valid
            if (source.status &&
                source.ReponseCode == 1 &&
                source.IsAccountValidated &&
                !string.IsNullOrWhiteSpace(source.ContactNumber) &&
                string.Equals("ACTIVE", source.AccountStatus, StringComparison.InvariantCultureIgnoreCase))
            {
                response.ResponseStatus = ResponseStatuses.Success;
                response.ResponseCode = "000";
                response.ResponseMessage = source.ResponseMessage;
                response.ResponseDetailMessage = source.Details;

                response.WalletAccountStatus = source.AccountStatus;
                response.WalletAccountName = source.FullName;
                response.WalletAccountNumber = source.ContactNumber;
                response.IsWalletKycVerified = source.kycstatus;

                return (HttpStatusCode.OK, response);
            }

            // Account not active => 951
            if (!string.IsNullOrWhiteSpace(source.AccountStatus) &&
                !string.Equals("ACTIVE", source.AccountStatus, StringComparison.InvariantCultureIgnoreCase) &&
                !string.IsNullOrWhiteSpace(source.ContactNumber))
            {
                response.ResponseStatus = ResponseStatuses.Success;
                response.ResponseCode = "951";
                response.ResponseMessage = source.ResponseMessage;
                response.ResponseDetailMessage = source.Details;

                response.WalletAccountStatus = source.AccountStatus;
                response.WalletAccountName = source.FullName;
                response.WalletAccountNumber = source.ContactNumber;
                response.IsWalletKycVerified = source.kycstatus;

                return (HttpStatusCode.OK, response);
            }

            // return default Bad Response
            response.ResponseStatus = ResponseStatuses.Error;
            response.ResponseCode = ResponseCodes.Code400_BadRequest;
            response.ResponseMessage = source.ResponseMessage;
            response.ResponseDetailMessage = source.Details;

            return (HttpStatusCode.BadRequest, response);
        }

        // for Bank Account Validation
        private (HttpStatusCode, ValidateAccountResponse) MapToValidateAccountResponse(MyPayValidateBankUserApiResponse source)
        {
            ArgumentNullException.ThrowIfNull(source, nameof(source));

            if (string.IsNullOrWhiteSpace(source.ResponseCode) || string.IsNullOrWhiteSpace(source.MatchPercentate))
                return HandleBadGatewayResponse<ValidateAccountResponse>();

            var response = new ValidateAccountResponse();

            /* map "000" => "000" */
            /* Account successfully validated */
            if (source.ResponseCode.Equals("000"))
            {
                response.ResponseStatus = ResponseStatuses.Success;
                response.ResponseCode = "000";
                response.ResponseMessage = source.responseMessage;
                response.ResponseDetailMessage = source.Details;
                response.MatchPercentage = source.MatchPercentate;
                response.BranchId = source.BranchId;
                return (HttpStatusCode.OK, response);
            }

            /* map "999" => "971" */
            /* Some difference in beneficiary account name observed. Transaction once sent is irreversible, please reconfirm the beneficiary account number.
             * Check match percentage, if greater than 80%, proceed for payment */
            if (source.ResponseCode.Equals("999"))
            {
                response.ResponseStatus = ResponseStatuses.Success;
                response.ResponseCode = "971";
                response.ResponseMessage = source.responseMessage;
                response.ResponseDetailMessage = source.Details;
                response.MatchPercentage = source.MatchPercentate;
                response.BranchId = source.BranchId;
                return (HttpStatusCode.OK, response);
            }

            /* map "001" => "972" */
            /* Beneficiary account validation not enabled for this bank. Transaction once sent is irreversible, please reconfirm. */
            if (source.ResponseCode.Equals("001"))
            {
                response.ResponseStatus = ResponseStatuses.Success;
                response.ResponseCode = "972";
                response.ResponseMessage = source.responseMessage;
                response.ResponseDetailMessage = source.Details;
                response.MatchPercentage = source.MatchPercentate;
                response.BranchId = source.BranchId;
                return (HttpStatusCode.OK, response);
            }

            /* map "1000" => "973" */
            /* Bank not reachable */
            if (source.ResponseCode.Equals("1000"))
            {
                response.ResponseStatus = ResponseStatuses.Success;
                response.ResponseCode = "973";
                response.ResponseMessage = source.responseMessage;
                response.ResponseDetailMessage = source.Details;
                response.MatchPercentage = source.MatchPercentate;
                response.BranchId = source.BranchId;
                return (HttpStatusCode.OK, response);
            }

            /* Code: 974, 975 are reserverd for future use (for manuallly decidable cases) */

            /* map "523" => "976" */
            /* Beneficiary account name mismatch. */
            if (source.ResponseCode.Equals("523"))
            {
                response.ResponseStatus = ResponseStatuses.Success;
                response.ResponseCode = "976";
                response.ResponseMessage = source.responseMessage;
                response.ResponseDetailMessage = source.Details;
                response.MatchPercentage = source.MatchPercentate;
                response.BranchId = source.BranchId;
                return (HttpStatusCode.OK, response);
            }

            /* map "502" => "977" */
            /* Account not exist */
            if (source.ResponseCode.Equals("502"))
            {
                response.ResponseStatus = ResponseStatuses.Success;
                response.ResponseCode = "977";
                response.ResponseMessage = source.responseMessage;
                response.ResponseDetailMessage = source.Details;
                response.MatchPercentage = source.MatchPercentate;
                response.BranchId = source.BranchId;
                return (HttpStatusCode.OK, response);
            }

            return HandleBadGatewayResponse<ValidateAccountResponse>();
        }

        private (HttpStatusCode, T) HandleBadRequestResponse<T>(string responseMessage = null, string responseDetailMessage = null, List<FieldError> fieldErrors = null) where T : ApiResponse
        {
            var instance = Activator.CreateInstance<T>();
            instance.ResponseCode = ResponseCodes.Code400_BadRequest;
            instance.ResponseStatus = ResponseStatuses.Error;
            instance.ResponseMessage = ResponseMessages.Msg400_BadRequest;

            if (responseMessage is not null)
                instance.ResponseMessage = responseMessage;

            if (responseDetailMessage is not null)
                instance.ResponseDetailMessage = responseDetailMessage;

            if (fieldErrors is not null)
                instance.FieldErrors = fieldErrors;

            return (HttpStatusCode.BadRequest, instance);
        }

        private (HttpStatusCode, T) HandleBadGatewayResponse<T>(string responseMessage = null, string responseDetailMessage = null) where T : ApiResponse
        {
            var instance = Activator.CreateInstance<T>();
            instance.ResponseCode = ResponseCodes.Code502_BadGateway;
            instance.ResponseStatus = ResponseStatuses.Error;
            instance.ResponseMessage = ResponseMessages.Msg502_BadGateway;

            if (responseMessage is not null)
                instance.ResponseMessage = responseMessage;

            if (responseDetailMessage is not null)
                instance.ResponseDetailMessage = responseDetailMessage;

            return (HttpStatusCode.BadGateway, instance);
        }

        private (HttpStatusCode, T) HandleGatewayTimeoutResponse<T>(string responseMessage = null, string responseDetailMessage = null) where T : ApiResponse
        {
            var instance = Activator.CreateInstance<T>();
            instance.ResponseCode = ResponseCodes.Code504_GatewayTimeout;
            instance.ResponseStatus = ResponseStatuses.Error;
            instance.ResponseMessage = ResponseMessages.Msg504_GatewayTimeout;

            if (responseMessage is not null)
                instance.ResponseMessage = responseMessage;

            if (responseDetailMessage is not null)
                instance.ResponseDetailMessage = responseDetailMessage;

            return (HttpStatusCode.GatewayTimeout, instance);
        }

        private (HttpStatusCode, object) HandleInvalidSignatureTokenResponse<T>() where T : ApiResponse
        {
            var instance = Activator.CreateInstance<T>();
            instance.ResponseCode = ResponseCodes.CodeE601_InvalidSignatureToken;
            instance.ResponseStatus = ResponseStatuses.Error;
            instance.ResponseMessage = ResponseMessages.MsgE601_InvalidSignatureToken;

            return (HttpStatusCode.BadRequest, instance);
        }

        private async Task<(HttpStatusCode, object)> HandleSuccessPushTxnResponse(PushTransactionDetails details = null, string responseMessage = null, string responseDetailMessage = null)
        {
            var response = new PushTransactionResponse
            {
                ResponseCode = ResponseCodes.Code200_Success,
                ResponseStatus = ResponseStatuses.Success,
                ResponseMessage = ResponseMessages.Msg200_Success,
                ResponseDetailMessage = "Transaction successful!",
            };

            if (responseMessage is not null)
                response.ResponseMessage = responseMessage;

            if (responseDetailMessage is not null)
                response.ResponseDetailMessage = responseDetailMessage;

            if (details is null)
                return (HttpStatusCode.OK, response);

            response.TransactionId = details.TransactionId;

            var systemPublicKey = _httpContextAccessor.HttpContext?.User.FindFirstValue(PartnerClaimTypes.SystemPublicKey);
            if (!string.IsNullOrWhiteSpace(details.ReferenceTokenNo) && systemPublicKey is not null)
            {
                try
                {
                    var rsaKeyParam = RsaCryptoUtils.ImportPublicKeyPem(systemPublicKey);
                    var mtchDataBytes = Encoding.UTF8.GetBytes(details.ReferenceTokenNo);

                    var mtcnCipheredBytes = RsaCryptoUtils.EncryptData(mtchDataBytes, rsaKeyParam, RsaCryptoUtils.AlgorithmRsaNoneOaepWithSha256AndMgf1Padding);

                    var mtcnBase64 = Convert.ToBase64String(mtcnCipheredBytes);
                    response.MtcnNumber = mtcnBase64;
                }
                catch (Exception ex)
                {
                    await _exceptionLogger.LogAsync(ex);
                }
            }

            response.PaymentType = details.PaymentType;
            response.SourceCurrency = details.SourceCurrency;
            response.DestinationCurrency = details.DestinationCurrency;
            response.SendingAmount = details.SendingAmount?.ToString();
            response.NetSendingAmount = details.NetSendingAmount?.ToString();
            response.DestCreditAmount = details.PayableAmount?.ToString();
            response.ConversionRate = details.ConversionRate?.ToString();
            response.ServiceCharge = details.ServiceCharge?.ToString();
            response.PartnerServiceCharge = details.PartnerServiceCharge?.ToString();
            response.Commission = details.Commission;
            response.TransactionDate = details.TransactionDateTime?.ToString("yyyy-MM-ddTHH:mm:ss.fff");
            response.SenderName = details.SenderName;
            response.SenderEmail = details.SenderEmail;
            response.SenderContactNumber = details.SenderContactNumber;
            response.SenderCountryCode = details.SenderCountry;
            response.RecipientCountryCode = details.RecipientCountry;
            response.RecipientName = details.RecipientName;
            response.RecipientContactNumber = details.RecipientContactNumber;
            response.PayableAmount = details.PayableAmount?.ToString();

            return (HttpStatusCode.OK, response);
        }

        public async Task<(HttpStatusCode, object)> PushTransactionDetailsAsync(PushTransactionRequestDetals request)
        {
            var partnerCode = _httpContextAccessor.HttpContext?.User.FindFirstValue(PartnerClaimTypes.PartnerCode);
            var apiUserName = _httpContextAccessor.HttpContext?.User.FindFirstValue(PartnerClaimTypes.ApiUserName);
            if (string.IsNullOrEmpty(partnerCode))
                return HandleBadRequestResponse<PushTransactionResponse>();

            if (string.IsNullOrEmpty(apiUserName))
                return HandleBadRequestResponse<PushTransactionResponse>();
            //var fieldValidationResult = ValidatePushTransactionRequestFields(request);
            var fieldValidationResult = ValidatePushTransactionRequestDetailsFields(request);
            if (!fieldValidationResult.Success)
                return HandleBadRequestResponse<PushTransactionResponse>(responseDetailMessage: "One or more field validations failed",
                    fieldErrors: fieldValidationResult.Errors);

            if (!apiUserName.Equals(request.ApiUserName))
                return HandleBadRequestResponse<PushTransactionResponse>(responseDetailMessage: "One or more field validations failed",
                    fieldErrors: new() { new() { Field = nameof(request.Signature), Message = "Invalid ApiUserName." } });

            var paymentTypes = new string[] { PayoutTypes.Wallet, PayoutTypes.Bank, PayoutTypes.Cash };
            var paymentType = paymentTypes.FirstOrDefault(p => p.Equals(request.PaymentType, StringComparison.OrdinalIgnoreCase));
            if (paymentType == null)
                return HandleBadRequestResponse<PushTransactionResponse>(responseDetailMessage: "One or more field validations failed",
                    fieldErrors: new() { new() { Field = nameof(request.PaymentType), Message = "Invalid PaymentType." } });

            var signatureData = paymentType switch
            {
                PayoutTypes.Wallet => $"{request.ApiUserName},{request.ProcessId},{request.PartnerTransactionId},{request.PaymentType},{request.SourceCurrency},{request.DestinationCurrency},{request.SendingAmount},{request.NetReceivingAmount},{request.SenderFirstName},{request.SenderLastName},{request.SenderCountry},{request.SenderAddress},{request.RecipientType},{request.RecipientFirstName},{request.RecipientLastName},{request.RecipientCountry},{request.WalletCode},{request.WalletNumber},{request.WalletHolderName}",
                PayoutTypes.Bank => $"{request.ApiUserName},{request.ProcessId},{request.PartnerTransactionId},{request.PaymentType},{request.SourceCurrency},{request.DestinationCurrency},{request.SendingAmount},{request.NetReceivingAmount},{request.SenderFirstName},{request.SenderLastName},{request.SenderCountry},{request.SenderAddress},{request.RecipientType},{request.RecipientFirstName},{request.RecipientLastName},{request.RecipientCountry},{request.BankCode},{request.AccountNumber},{request.AccountHolderName}",
                PayoutTypes.Cash => $"{request.ApiUserName},{request.ProcessId},{request.PartnerTransactionId},{request.PaymentType},{request.SourceCurrency},{request.DestinationCurrency},{request.SendingAmount},{request.NetReceivingAmount},{request.SenderFirstName},{request.SenderLastName},{request.SenderCountry},{request.SenderAddress},{request.RecipientType},{request.RecipientFirstName},{request.RecipientLastName},{request.RecipientCountry}",
                _ => throw new MpmtException("Invalid PaymentType.")
            };

            var isSignatureValid = await IsSignatureValidAsync(request.Signature, signatureData);
            if (!isSignatureValid)
                return HandleInvalidSignatureTokenResponse<PushTransactionResponse>();

            var pushTxnParam = MapToPushTransactionRequestDetailsParam(request);
            pushTxnParam.TransactionType = "APITRANSACTION"; // constant for API transaction
            pushTxnParam.PartnerCode = partnerCode;
            pushTxnParam.IpAddress = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();
            pushTxnParam.LoggedInUser = _httpContextAccessor.HttpContext?.User.FindFirstValue(PartnerClaimTypes.UserName);
            pushTxnParam.UserType = _httpContextAccessor.HttpContext?.User.FindFirstValue(PartnerClaimTypes.UserType);

            var (pushTxnStatus, pushTxnDetails) = await _partnerApiRepository.PushTransactionDetailsAsync(pushTxnParam);

            // if StatusCode = 411, then ProcessId is invalid
            if (pushTxnStatus.StatusCode == 411)
                return (HttpStatusCode.BadRequest, new PushTransactionResponse
                {
                    ResponseCode = ResponseCodes.CodeE602_InvalidProcessId,
                    ResponseStatus = ResponseStatuses.Error,
                    ResponseMessage = ResponseMessages.MsgE602_InvalidProcessId
                });

            if (pushTxnStatus.StatusCode != 200 || pushTxnDetails is null)
                return HandleBadRequestResponse<PushTransactionResponse>(responseMessage: pushTxnStatus.MsgText);

            // TODO: Check the response here

            var txnResultDetails = MapToAddTransactionResultDetails(pushTxnDetails);

            _vendorApiLogger.SetLogContext(trackerId: txnResultDetails.TransactionId);

            //_ = _partnerPayoutHandlerService.HandleSenderTransactionNotificationEmailing(txnResultDetails);
            _ = _partnerPayoutHandlerService.HandlePartnerLowWalletBalanceNotificationEmailing(txnResultDetails);
            _ = _partnerPayoutHandlerService.HandlePartnerRemainingFeeBalanceNotificationEmailing(txnResultDetails);

            // check if 
            if (pushTxnDetails.ComplianceFlag is true)
            {
                return await HandleSuccessPushTxnResponse(pushTxnDetails);
            }

            if (!PartnerPayoutHelper.IsPayoutProceedable(txnResultDetails))
            {
                return await HandleSuccessPushTxnResponse(pushTxnDetails);
            }

            switch (pushTxnDetails.PaymentType)
            {
                case PayoutTypes.Wallet:
                    var walletPayoutResult = await _partnerPayoutHandlerService.HandlePayoutToWalletAsync(txnResultDetails);
                    // Just return success response
                    return await HandleSuccessPushTxnResponse(pushTxnDetails);
                case PayoutTypes.Bank:
                    var bankPayoutResult = await _partnerPayoutHandlerService.HandlePayoutToBankAsync(txnResultDetails);
                    // Just return success response
                    return await HandleSuccessPushTxnResponse(pushTxnDetails);
                default:
                    // Just return success response
                    return await HandleSuccessPushTxnResponse(pushTxnDetails);
            };
        }
    }
}
