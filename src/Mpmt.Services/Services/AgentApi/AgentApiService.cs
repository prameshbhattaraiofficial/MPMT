using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Mpmt.Core.Common;
using Mpmt.Core.Domain;
using Mpmt.Core.Domain.Agent;
using Mpmt.Core.Dtos.Agent;
using Mpmt.Core.Dtos.AgentApi;
using Mpmt.Core.Dtos.Roles;
using Mpmt.Core.Models.Transaction;
using Mpmt.Data.Repositories.AgentModule;
using Mpmt.Services.Common;
using Mpmt.Services.Extensions;
using Mpmt.Services.Logging;
using NepDate;
using System.Globalization;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace Mpmt.Services.Services.AgentApi
{
    public class AgentApiService : IAgentApiService
    {
        private readonly IConfiguration _config;
        private readonly IExceptionLogger _exceptionLogger;
        private readonly IAgentApiLogger _agentApiLogger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAgentApiRepository _agentApiRepository;
        private readonly IFileProviderService _fileProviderService;

        public AgentApiService(
            IConfiguration config,
            IExceptionLogger exceptionLogger,
            IAgentApiLogger agentApiLogger,
            IHttpContextAccessor httpContextAccessor,
            IAgentApiRepository agentApiRepository,
            IFileProviderService fileProviderService)
        {
            _config = config;
            _exceptionLogger = exceptionLogger;
            _agentApiLogger = agentApiLogger;
            _httpContextAccessor = httpContextAccessor;
            _agentApiRepository = agentApiRepository;
            _fileProviderService = fileProviderService;
        }

        public async Task<(HttpStatusCode, object)> AgentmtcnValidateAsync(WalletPayoutApi request)
        {
            var agentCode = _httpContextAccessor.HttpContext?.User.FindFirstValue(AgentClaimTypes.AgentCode);
            var apiUserName = _httpContextAccessor.HttpContext?.User.FindFirstValue(AgentClaimTypes.ApiUserName);

            if (string.IsNullOrEmpty(agentCode))
                return HandleBadRequestResponse<PayoutResponse>();

            if (string.IsNullOrEmpty(apiUserName))
                return HandleBadRequestResponse<PayoutResponse>();

            if (!apiUserName.Equals(request.ApiUserName))
                return HandleBadRequestResponse<PayoutResponse>(responseDetailMessage: "One or more field validations failed",
                    fieldErrors: new() { new() { Field = nameof(request.ApiUserName), Message = $"Invalid {nameof(request.ApiUserName)}." } });

            if (!decimal.TryParse(request.Amount, out var Amount) || Amount <= 0)
                return HandleBadRequestResponse<PayoutResponse>(responseDetailMessage: "One or more field validations failed",
                    fieldErrors: new() { new() { Field = nameof(request.Amount), Message = $"Invalid {nameof(request.Amount)}." } });

            //var systemPrivatekey = _httpContextAccessor.HttpContext?.User.FindFirstValue(AgentClaimTypes.SystemPrivateKey);
            var systemPublickey = _httpContextAccessor.HttpContext?.User.FindFirstValue(AgentClaimTypes.SystemPublicKey);

            //var encryptMtCNNo = EncryPtMCTN(request.MTCN, systemPublickey);
          
            var signatureData = $"{request.MTCN},{request.Amount},{request.Country},{request.ApiUserName},{request.SenderName},{request.WalletHolderName}";
            var isSignatureValid = await IsSignatureValidAsync(request.Signature, signatureData);

            if (!isSignatureValid)
                return HandleInvalidSignatureTokenResponse<PayoutResponse>();

           //var devrypttMtCNNo = DecryptMtcnNumber(request.MTCN, systemPrivatekey);

            if (!TryDecrypt(request.MTCN, out var mtcnNumber))
                return HandleInvalidEncryptionErrorResponse<PayoutResponse>();

            string hostName = Dns.GetHostName();

            var getChargeDetailsParam = new AgentPayoutModel
            {
                AgentCode = agentCode,
                Country = request.Country,
                SenderName = request.SenderName,
                MTCN = mtcnNumber,
                Amount = request.Amount,
                DeviceId = request.DeviceId,
                WalletHolderName = request.WalletHolderName,
                IPAddress = Dns.GetHostEntry(hostName).AddressList[0].ToString()
            };

            var (sprocStatus, response) = await _agentApiRepository.AgentWalletValidateLoadAsync(getChargeDetailsParam);

            if (sprocStatus.StatusCode != 200)
                return HandleBadRequestResponse<PayoutResponse>(responseMessage: sprocStatus.MsgText);

            if (response is null)
                return HandleBadRequestResponse<PayoutResponse>(responseMessage: "Unable to fetch transaction details.");

            //PayoutResponse resp = new PayoutResponse();

            response.ResponseCode = ResponseCodes.Code200_Success;
            response.ResponseStatus = ResponseStatuses.Success;
            response.ResponseMessage = "Transaction details validated successfully!";


            return (HttpStatusCode.OK, response);
        }

        public async Task<(HttpStatusCode, object)> GetInstrumentDetailAsync(InstrumentDetailRequest request)
        {
            var agentCode = _httpContextAccessor.HttpContext?.User.FindFirstValue(AgentClaimTypes.AgentCode);
            var apiUserName = _httpContextAccessor.HttpContext?.User.FindFirstValue(AgentClaimTypes.ApiUserName);

            _agentApiLogger.SetLogContext(trackerId: request.MtcnNumber);

            if (string.IsNullOrEmpty(agentCode))
                return HandleBadRequestResponse<GetInstrumentDetailResponse>();

            if (string.IsNullOrEmpty(apiUserName))
                return HandleBadRequestResponse<GetInstrumentDetailResponse>();

            var fieldValidationResult = ValidateGetInstrumentDetailModel(request);
            if (!fieldValidationResult.Success)
                return HandleBadRequestResponse<GetInstrumentDetailResponse>(responseDetailMessage: "One or more field validations failed",
                    fieldErrors: fieldValidationResult.Errors);

            if (!apiUserName.Equals(request.ApiUserName))
                return HandleBadRequestResponse<GetInstrumentDetailResponse>(responseDetailMessage: "One or more field validations failed",
                    fieldErrors: new() { new() { Field = nameof(request.ApiUserName), Message = "Invalid ApiUserName." } });

            var signatureData = $"{request.ApiUserName},{request.MtcnNumber}";
            var isSignatureValid = await IsSignatureValidAsync(request.Signature, signatureData);

            if (!isSignatureValid)
                return HandleInvalidSignatureTokenResponse<GetInstrumentDetailResponse>();

            if (!TryDecrypt(request.MtcnNumber, out var mtcnNumber))
                return HandleInvalidEncryptionErrorResponse<GetInstrumentDetailResponse>();

            //// override previous encrypted mtcn number with its decrypted mtcn number
            _agentApiLogger.SetLogContext(trackerId: mtcnNumber);

            _agentApiLogger.SetLogContext(trackerId: request.MtcnNumber);

            var getChargeDetailsParam = new GetCashPayoutDetailParam
            {
                MtcnNumber = mtcnNumber,
                AgentCode = agentCode,
                IpAddress = _httpContextAccessor.HttpContext?.GetIpAddress(),
            };

            var (sprocStatus, details) = await _agentApiRepository.GetCashPayoutDetailAsync(getChargeDetailsParam);
            //var (sprocStatus, details) = await _agentApiRepository.GetInstrumentDetailForAgentWalletAsync(getChargeDetailsParam);

            if (sprocStatus.StatusCode != 200)
                return HandleBadRequestResponse<GetInstrumentDetailResponse>(responseMessage: sprocStatus.MsgText);

            if (details is null)
                return HandleBadRequestResponse<GetInstrumentDetailResponse>(responseMessage: "Invalid Control Number!.");

            var response = MapToInstrumentDetailResponse(details);
            //var response = details;
            
            response.ResponseCode = ResponseCodes.Code200_Success;
            response.ResponseStatus = ResponseStatuses.Success;
            response.ResponseMessage = "Instrument details fetched successfully!";

            return (HttpStatusCode.OK, response);
        }

        public async Task<(HttpStatusCode, object)> GetInstrumentDetailForAgentWalletAsync(InstrumentDetailRequest request)
        {
            var agentCode = _httpContextAccessor.HttpContext?.User.FindFirstValue(AgentClaimTypes.AgentCode);
            var apiUserName = _httpContextAccessor.HttpContext?.User.FindFirstValue(AgentClaimTypes.ApiUserName);

            _agentApiLogger.SetLogContext(trackerId: request.MtcnNumber);

            if (string.IsNullOrEmpty(agentCode))
                return HandleBadRequestResponse<AgentWalletApi>();

            if (string.IsNullOrEmpty(apiUserName))
                return HandleBadRequestResponse<AgentWalletApi>();

            var fieldValidationResult = ValidateGetInstrumentDetailModel(request);
            if (!fieldValidationResult.Success)
                return HandleBadRequestResponse<AgentWalletApi>(responseDetailMessage: "One or more field validations failed",
                    fieldErrors: fieldValidationResult.Errors);

            if (!apiUserName.Equals(request.ApiUserName))
                return HandleBadRequestResponse<AgentWalletApi>(responseDetailMessage: "One or more field validations failed",
                    fieldErrors: new() { new() { Field = nameof(request.ApiUserName), Message = "Invalid ApiUserName." } });

            var signatureData = $"{request.ApiUserName},{request.MtcnNumber}";
            var isSignatureValid = await IsSignatureValidAsync(request.Signature, signatureData);

            if (!isSignatureValid)
                return HandleInvalidSignatureTokenResponse<GetInstrumentDetailResponse>();

            if (!TryDecrypt(request.MtcnNumber, out var mtcnNumber))
                return HandleInvalidEncryptionErrorResponse<GetInstrumentDetailResponse>();

            // override previous encrypted mtcn number with its decrypted mtcn number
            _agentApiLogger.SetLogContext(trackerId: mtcnNumber);

            _agentApiLogger.SetLogContext(trackerId: request.MtcnNumber);

            var getChargeDetailsParam = new GetCashPayoutDetailParam
            {
                MtcnNumber = mtcnNumber,
                AgentCode = agentCode,
                IpAddress = _httpContextAccessor.HttpContext?.GetIpAddress(),
            };

            //var (sprocStatus, details) = await _agentApiRepository.GetCashPayoutDetailAsync(getChargeDetailsParam);
            var (sprocStatus, details) = await _agentApiRepository.GetInstrumentDetailForAgentWalletAsync(getChargeDetailsParam);

            if (sprocStatus.StatusCode != 200)
                return HandleBadRequestResponse<AgentWalletApi>(responseMessage: sprocStatus.MsgText);

            if (details is null)
                return HandleBadRequestResponse<AgentWalletApi>(responseMessage: "Invalid Control Number!.");

            //var response = MapToInstrumentDetailResponse(details);
            var response = details;
            if (response.TransactionStatusCode != "DEFER")
            {
                response.CountryList = null;
                response.SenderDocumentTypeList = null;
                response.ResponseCode = ResponseCodes.Code200_Success;
                response.ResponseStatus = ResponseStatuses.Success;
                response.ResponseMessage = details.TransactionRemarks;
                return (HttpStatusCode.OK, response);
            }
            response.ResponseCode = ResponseCodes.Code200_Success;
            response.ResponseStatus = ResponseStatuses.Success;
            response.ResponseMessage = "Instrument details fetched successfully!";

            return (HttpStatusCode.OK, response);
        }

        public async Task<(HttpStatusCode, object)> GetTxnProcessIdAsync(GetProcessIdRequestAgentApi request)
        {
            var apiUserName = _httpContextAccessor.HttpContext?.User.FindFirstValue(AgentClaimTypes.ApiUserName);
            var agentCode = _httpContextAccessor.HttpContext?.User.FindFirstValue(AgentClaimTypes.AgentCode);

            _agentApiLogger.SetLogContext(trackerId: request.ReferenceId);

            if (string.IsNullOrEmpty(apiUserName))
                return HandleBadRequestResponse<GetProcessIdResponse>();

            if (string.IsNullOrEmpty(agentCode))
                return HandleBadRequestResponse<GetProcessIdResponse>();

            var fieldValidationResult = ValidateGetProcessIdRequestModel(request);
            if (!fieldValidationResult.Success)
                return HandleBadRequestResponse<GetProcessIdResponse>(responseDetailMessage: "One or more field validations failed",
                    fieldErrors: fieldValidationResult.Errors);

            if (!apiUserName.Equals(request.ApiUserName))
                return HandleBadRequestResponse<GetProcessIdResponse>(responseDetailMessage: "One or more field validations failed",
                    fieldErrors: new() { new() { Field = nameof(request.ApiUserName), Message = "Invalid ApiUserName." } });

            var signatureData = $"{request.ApiUserName},{request.ReferenceId}";
            var isSignatureValid = await IsSignatureValidAsync(request.Signature, signatureData);

            if (!isSignatureValid)
                return HandleInvalidSignatureTokenResponse<GetProcessIdResponse>();

            var (sprocMessage, processId) = await _agentApiRepository.GetTxnProcessIdAsync(agentCode, request.ReferenceId);
            if (sprocMessage.StatusCode != 200)
                return HandleBadRequestResponse<GetProcessIdResponse>(responseMessage: "Duplicate ReferenceId");

            if (string.IsNullOrWhiteSpace(processId?.ProcessId))
                return HandleBadRequestResponse<GetProcessIdResponse>(responseMessage: "Unable to get process ID.");

            var response = new GetProcessIdResponse
            {
                ProcessId = processId.ProcessId,
                ResponseCode = ResponseCodes.Code200_Success,
                ResponseStatus = ResponseStatuses.Success,
                ResponseMessage = "Process ID fetched successfully!"
            };

            return (HttpStatusCode.OK, response);
        }

        public async Task<(HttpStatusCode, object)> RequestPayoutAsync(RequestPayoutApi request)
        {
            var agentCode = _httpContextAccessor.HttpContext?.User.FindFirstValue(AgentClaimTypes.AgentCode);
            var apiUserName = _httpContextAccessor.HttpContext?.User.FindFirstValue(AgentClaimTypes.ApiUserName);

            _agentApiLogger.SetLogContext(transactionId: request.RemitTransactionId, trackerId: request.AgentTransactionId);

            if (string.IsNullOrEmpty(agentCode))
                return HandleBadRequestResponse<RequestPayoutResponse>();

            if (string.IsNullOrEmpty(apiUserName))
                return HandleBadRequestResponse<RequestPayoutResponse>();

            var fieldValidationResult = ValidateRequestPayoutModel(request);
            if (!fieldValidationResult.Success)
                return HandleBadRequestResponse<RequestPayoutResponse>(responseDetailMessage: "One or more field validations failed",
                    fieldErrors: fieldValidationResult.Errors);

            if (!apiUserName.Equals(request.ApiUserName))
                return HandleBadRequestResponse<RequestPayoutResponse>(responseDetailMessage: "One or more field validations failed",
                    fieldErrors: new() { new() { Field = nameof(request.ApiUserName), Message = "Invalid ApiUserName." } });

            //var signatureData = $"{request.ApiUserName},{request.RemitTransactionId},{request.ProcessId},{request.AgentTransactionId},{request.PaymentType},{request.FullName},{request.ContactNumber},{request.Country},{request.Province},{request.District},{request.LocalBody},{request.Address},{request.DocumentTypeCode},{request.DocumentNumber}";
            var signatureData = $"{request.ApiUserName},{request.RemitTransactionId},{request.ProcessId},{request.AgentTransactionId},{request.PaymentType},{request.ContactNumber},{request.DocumentTypeCode},{request.DocumentNumber}";
            var isSignatureValid = await IsSignatureValidAsync(request.Signature, signatureData);

            if (!isSignatureValid)
                return HandleInvalidSignatureTokenResponse<RequestPayoutResponse>();

            var docBasePath = _config["Folder:TransactionPayoutDocs"] ?? "";
            var docUploadPath = $"{docBasePath.TrimEnd('/')}/{DateTime.Now.Year}/{DateTime.Now:MMdd}/";

            string docFrontImagePath = null;
            string docBackImagePath = null;

            if (request.DocumentFrontImage is not null && !_fileProviderService.TryUploadFile(request.DocumentFrontImage, docUploadPath, out docFrontImagePath))
                return HandleInternalServerErrorResponse<RequestPayoutResponse>();

            if (request.DocumentBackImage is not null && !_fileProviderService.TryUploadFile(request.DocumentFrontImage, docUploadPath, out docBackImagePath))
                return HandleInternalServerErrorResponse<RequestPayoutResponse>();

            var param = new RequestPayoutParam
            {
                PayoutTransactionType = "AGENTAPITRANSACTION",
                UserType = "AGENT",

                RemitTransactionId = request.RemitTransactionId,
                ProcessId = request.ProcessId,
                AgentTransactionId = request.AgentTransactionId,
                PaymentType = request.PaymentType,
                //FullName = request.FullName,
                ContactNumber = request.ContactNumber,
                //Country = request.Country,
                //Province = request.Province,
                //District = request.District,
                //LocalBody = request.LocalBody,
                //Address = request.Address,
                DocumentTypeCode = request.DocumentTypeCode,
                // DocumentType = DocumentType,
                DocumentNumber = request.DocumentNumber,
                DocumentIssueDate = request.DocumentIssueDateAD,
                DocumentExpiryDate = request.DocumentExpiryDateAD,
                DocumentIssueDateNepali = request.DocumentIssueDateBS,
                DocumentExpiryDateNepali = request.DocumentExpiryDateBS,

                DocumentFrontImagePath = docFrontImagePath,
                DocumentBackImagePath = docBackImagePath,

                AgentCode = agentCode,
                Username = apiUserName,
                LoggedInUser = apiUserName,
                DeviceId = "APICLIENT",
                IpAddress = _httpContextAccessor.HttpContext?.GetIpAddress(),
            };

            var (sprocStatus, details) = await _agentApiRepository.RequestPayoutAsync(param);

            var ClearFileUploadsForFailedOperation = () =>
            {
                if (docFrontImagePath is not null)
                    _fileProviderService.DeleteFile(docFrontImagePath);

                if (docBackImagePath is not null)
                    _fileProviderService.DeleteFile(docBackImagePath);
            };

            if (sprocStatus.StatusCode == 411)
            {
                ClearFileUploadsForFailedOperation();
                return HandleInvalidProcessIdResponse<RequestPayoutResponse>();
            }

            if (sprocStatus.StatusCode != 200 || details is null)
            {
                ClearFileUploadsForFailedOperation();
                return HandleBadRequestResponse<RequestPayoutResponse>(responseMessage: sprocStatus.MsgText);
            }

            var response = MapToRequestPayoutResponse(details);
            response.ResponseCode = ResponseCodes.Code200_Success;
            response.ResponseStatus = ResponseStatuses.Success;
            response.ResponseMessage = "Payout successful!";

            return (HttpStatusCode.OK, response);
        }


        public async Task<(HttpStatusCode, object)> CheckPayoutStatusAsync(CheckPayoutStatusRequest request)
        {
            var agentCode = _httpContextAccessor.HttpContext?.User.FindFirstValue(AgentClaimTypes.AgentCode);
            var apiUserName = _httpContextAccessor.HttpContext?.User.FindFirstValue(AgentClaimTypes.ApiUserName);

            _agentApiLogger.SetLogContext(transactionId: request.RemitTransactionId);

            if (string.IsNullOrEmpty(agentCode))
                return HandleBadRequestResponse<RequestPayoutResponse>();

            if (string.IsNullOrEmpty(apiUserName))
                return HandleBadRequestResponse<RequestPayoutResponse>();

            var fieldValidationResult = ValidateCheckPayoutStatusModel(request);
            if (!fieldValidationResult.Success)
                return HandleBadRequestResponse<RequestPayoutResponse>(responseDetailMessage: "One or more field validations failed",
                    fieldErrors: fieldValidationResult.Errors);

            if (!apiUserName.Equals(request.ApiUserName))
                return HandleBadRequestResponse<RequestPayoutResponse>(responseDetailMessage: "One or more field validations failed",
                    fieldErrors: new() { new() { Field = nameof(request.ApiUserName), Message = "Invalid ApiUserName." } });

            var signatureData = $"{request.ApiUserName},{request.RemitTransactionId}";
            var isSignatureValid = await IsSignatureValidAsync(request.Signature, signatureData);

            if (!isSignatureValid)
                return HandleInvalidSignatureTokenResponse<RequestPayoutResponse>();

            var (sprocStatus, details) = await _agentApiRepository.CheckPayoutStatusAsync(request.RemitTransactionId, agentCode);

            if (sprocStatus.StatusCode != 200)
            {
                var errRes = MapToCheckPayoutStatusResponse(details);
                errRes.ResponseCode = ResponseCodes.Code400_BadRequest;
                errRes.ResponseStatus = ResponseStatuses.Error;
                errRes.ResponseMessage = sprocStatus.MsgText;

                return (HttpStatusCode.BadRequest, errRes);
            }

            var response = MapToCheckPayoutStatusResponse(details);
            response.ResponseCode = ResponseCodes.Code200_Success;
            response.ResponseStatus = ResponseStatuses.Success;
            response.ResponseMessage = "Status fetched successfully!";

            return (HttpStatusCode.OK, response);
        }

        public string DecryptMtcnNumber(string MCTNNo, string privateKeyString)
        {
            try
            {
                var mtcnBytes = Convert.FromBase64String(MCTNNo);

                var pvtKey = RsaCryptoUtils.ImportPrivateKeyPem(privateKeyString);

                var mtcnPlainBytes = RsaCryptoUtils.DecryptData(mtcnBytes, pvtKey);

                var mtcnNumber2 = Encoding.UTF8.GetString(mtcnPlainBytes);

                return mtcnNumber2;
            }
            catch (Exception e)
            {
                // log exception
                return null;
            }
        }

        public string EncryPtMCTN(string MCTNNo, string publicKey)
        {
            try
            {
                var mtcnBytes = Encoding.UTF8.GetBytes(MCTNNo);

                var publKey = RsaCryptoUtils.ImportPublicKeyPem(publicKey);

                var mtcnPlainBytes = RsaCryptoUtils.EncryptData(mtcnBytes, publKey);

                var mtcnNumber = Convert.ToBase64String(mtcnPlainBytes);

                return mtcnNumber;
            }
            catch (Exception e)
            {
                // log exception
                return null;
            }

        }

        public GetInstrumentDetailResponse MapToInstrumentDetailResponse(CashPayoutDetailApi detail)
        {
            ArgumentNullException.ThrowIfNull(detail, nameof(detail));

            return new GetInstrumentDetailResponse
            {
                PaymentStatusCode = detail.PaymentStatusCode,
                PaymentStatusRemarks = detail.PaymentStatusRemarks,
                TransactionId = detail.TransactionId,
                PaymentType = detail.PaymentType,
                PaymentStatus = detail.PaymentStatus,
                SourceCurrency = detail.SourceCurrency,
                DestinationCurrency = detail.DestinationCurrency,
                PaymentAmountNPR = detail.PaymentAmountNPR,
                SendingDate = detail.SendingDate?.ToString("yyyy-MM-ddTHH:mm:ss.fff"),
                SendingNepaliDate = detail.SendingNepaliDate,
                SenderFullName = detail.SenderFullName,
                SenderContactNumber = detail.SenderContactNumber,
                SenderEmail = detail.SenderEmail,
                SenderCountry = detail.SenderCountry,
                SenderAddress = detail.SenderAddress,
                SenderRelationWithReceiver = detail.SenderRelationWithReceiver,
                ReceiverFullName = detail.ReceiverFullName,
                ReceiverContactNumber = detail.ReceiverContactNumber,
                ReceiverEmail = detail.ReceiverEmail,
                ReceiverCountry = detail.ReceiverCountry,
                ReceiverProvince = detail.ReceiverProvince,
                ReceiverDistrict = detail.ReceiverDistrict,
                ReceiverLocalBody = detail.ReceiverLocalBody,
                ReceiverAddress = detail.ReceiverAddress,
                ReceiverRelationWithSender = detail.ReceiverRelationWithSender,

                DocumentTypeList = detail.DocumentTypeList,
            };
        }

        public RequestPayoutResponse MapToRequestPayoutResponse(PayoutDetailsApi details)
        {
            ArgumentNullException.ThrowIfNull(details, nameof(details));

            return new RequestPayoutResponse
            {
                TransactionId = details.TransactionId,
                AmountNPR = details.AmountNPR,
                SourceCurrency = details.SourceCurrency,
                DestinationCurrency = details.DestinationCurrency,
                SenderFullName = details.SenderFullName,
                SenderCountry = details.SenderCountry,
                SenderAddress = details.SenderAddress,
                SenderContactNumber = details.SenderContactNumber,
                ReceiverFullName = details.ReceiverFullName,
                ReceiverProvice = details.ReceiverProvice,
                ReceiverDistrict = details.ReceiverDistrict,
                ReceiverLocalBody = details.ReceiverLocalBody,
                ReceiverAddress = details.ReceiverAddress,
                ReceiverContactNumber = details.ReceiverContactNumber,
                ReceiverDocumentType = details.ReceiverDocumentType,
                ReceiverDocumentNumber = details.ReceiverDocumentNumber,
                AgentName = details.AgentName,
                AgentContactNumber = details.AgentContactNumber,
                AgentAddress = details.AgentAddress,
                AgentCity = details.AgentCity,
                AgentDistrict = details.AgentDistrict,
                AgentCountry = details.AgentCountry,
                AgentOrganizationName = details.AgentOrganizationName,
                //ControlNumber = details.ControlNumber,
                Status = details.Status,
                AgentCommissionNPR = details.AgentCommissionNPR,
                ModeOfPayment = details.ModeOfPayment,
                TransactionDate = details.TransactionDate?.ToString("yyyy-MM-ddTHH:mm:ss.fff"),
                TransactionDateNepali = details.TransactionDateNepali
            };
        }

        private CheckPayoutStatusResponse MapToCheckPayoutStatusResponse(CheckPayoutStatusDetail details)
        {
            ArgumentNullException.ThrowIfNull(details, nameof(details));

            return new CheckPayoutStatusResponse
            {
                PaymentStatusCode = details.PaymentStatusCode,
                PaymentStatusRemarks = details.PaymentStatusRemarks,
                TransactionId = details.TransactionId,
                AmountNPR = details.AmountNPR,
                SourceCurrency = details.SourceCurrency,
                DestinationCurrency = details.DestinationCurrency,
                SenderFullName = details.SenderFullName,
                SenderCountry = details.SenderCountry,
                SenderAddress = details.SenderAddress,
                SenderContactNumber = details.SenderContactNumber,
                ReceiverFullName = details.ReceiverFullName,
                ReceiverProvice = details.ReceiverProvice,
                ReceiverDistrict = details.ReceiverDistrict,
                ReceiverLocalBody = details.ReceiverLocalBody,
                ReceiverAddress = details.ReceiverAddress,
                ReceiverContactNumber = details.ReceiverContactNumber,
                ReceiverDocumentType = details.ReceiverDocumentType,
                ReceiverDocumentNumber = details.ReceiverDocumentNumber,
                AgentName = details.AgentName,
                AgentContactNumber = details.AgentContactNumber,
                AgentAddress = details.AgentAddress,
                AgentCity = details.AgentCity,
                AgentDistrict = details.AgentDistrict,
                AgentCountry = details.AgentCountry,
                AgentOrganizationName = details.AgentOrganizationName,
                //ControlNumber = details.ControlNumber,
                Status = details.Status,
                ModeOfPayment = details.ModeOfPayment,
                TransactionDate = details.TransactionDate?.ToString("yyyy-MM-ddTHH:mm:ss.fff"),
                TransactionDateNepali = details.TransactionDateNepali,
                AgentCommissionNPR = details.AgentCommissionNPR
            };
        }

        private FieldValidationResult ValidateGetInstrumentDetailModel(InstrumentDetailRequest request)
        {
            var result = new FieldValidationResult();

            if (string.IsNullOrWhiteSpace(request.MtcnNumber))
                result.AddError(new() { Field = nameof(request.MtcnNumber), Message = $"{nameof(request.MtcnNumber)} is required" });

            if (string.IsNullOrWhiteSpace(request.ApiUserName))
                result.AddError(new() { Field = nameof(request.ApiUserName), Message = $"{nameof(request.ApiUserName)} is required" });

            if (string.IsNullOrWhiteSpace(request.Signature))
                result.AddError(new() { Field = nameof(request.Signature), Message = $"{nameof(request.Signature)} is required" });

            return result;
        }

        private FieldValidationResult ValidateGetProcessIdRequestModel(GetProcessIdRequestAgentApi request)
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

        private FieldValidationResult ValidateRequestPayoutModel(RequestPayoutApi request)
        {
            var result = new FieldValidationResult();

            if (string.IsNullOrWhiteSpace(request.ApiUserName))
                result.AddError(new() { Field = nameof(request.ApiUserName), Message = $"{nameof(request.ApiUserName)} is required" });

            if (string.IsNullOrWhiteSpace(request.RemitTransactionId))
                result.AddError(new() { Field = nameof(request.RemitTransactionId), Message = $"{nameof(request.RemitTransactionId)} is required" });

            if (string.IsNullOrWhiteSpace(request.ProcessId))
                result.AddError(new() { Field = nameof(request.ProcessId), Message = $"{nameof(request.ProcessId)} is required" });

            if (string.IsNullOrWhiteSpace(request.AgentTransactionId))
                result.AddError(new() { Field = nameof(request.AgentTransactionId), Message = $"{nameof(request.AgentTransactionId)} is required" });

            if (string.IsNullOrWhiteSpace(request.PaymentType))
                result.AddError(new() { Field = nameof(request.PaymentType), Message = $"{nameof(request.PaymentType)} is required" });

            //if (string.IsNullOrWhiteSpace(request.FullName))
            //    result.AddError(new() { Field = nameof(request.FullName), Message = $"{nameof(request.FullName)} is required" });

            if (string.IsNullOrWhiteSpace(request.ContactNumber))
                result.AddError(new() { Field = nameof(request.ContactNumber), Message = $"{nameof(request.ContactNumber)} is required" });

            //if (string.IsNullOrWhiteSpace(request.Country))
            //    result.AddError(new() { Field = nameof(request.Country), Message = $"{nameof(request.Country)} is required" });

            //if (string.IsNullOrWhiteSpace(request.Province))
            //    result.AddError(new() { Field = nameof(request.Province), Message = $"{nameof(request.Province)} is required" });

            //if (string.IsNullOrWhiteSpace(request.District))
            //    result.AddError(new() { Field = nameof(request.District), Message = $"{nameof(request.District)} is required" });

            //if (string.IsNullOrWhiteSpace(request.LocalBody))
            //    result.AddError(new() { Field = nameof(request.LocalBody), Message = $"{nameof(request.LocalBody)} is required" });

            //if (string.IsNullOrWhiteSpace(request.Address))
            //    result.AddError(new() { Field = nameof(request.Address), Message = $"{nameof(request.Address)} is required" });

            if (string.IsNullOrWhiteSpace(request.DocumentTypeCode))
                result.AddError(new() { Field = nameof(request.DocumentTypeCode), Message = $"{nameof(request.DocumentTypeCode)} is required" });

            //if (string.IsNullOrWhiteSpace(request.DocumentType))
            //    result.AddError(new() { Field = nameof(request.DocumentType), Message = $"{nameof(request.DocumentType)} is required" });

            if (string.IsNullOrWhiteSpace(request.DocumentNumber))
                result.AddError(new() { Field = nameof(request.DocumentNumber), Message = $"{nameof(request.DocumentNumber)} is required" });

            // Issue date validation
            const int dateOffsetYears = -150;
            if (!string.IsNullOrWhiteSpace(request.DocumentIssueDateAD))
            {
                if (!DateTime.TryParseExact(request.DocumentIssueDateAD, "yyyy-MM-dd", null, DateTimeStyles.None, out var dateParsedAD)
                    || dateParsedAD > DateTime.Now
                    || dateParsedAD < DateTime.Now.AddYears(dateOffsetYears))
                    result.AddError(new() { Field = nameof(request.DocumentIssueDateAD), Message = $"{nameof(request.DocumentIssueDateAD)} is invalid" });
            }
            else if (!string.IsNullOrWhiteSpace(request.DocumentIssueDateBS))
            {
                if (!NepaliDate.TryParse(request.DocumentIssueDateBS, out var dateParsedBS)
                    || dateParsedBS.EnglishDate > DateTime.Now
                    || dateParsedBS.EnglishDate < DateTime.Now.AddYears(dateOffsetYears))
                    result.AddError(new() { Field = nameof(request.DocumentIssueDateBS), Message = $"{nameof(request.DocumentIssueDateBS)} is invalid" });
            }
            else
            {
                result.AddError(new() { Field = nameof(request.DocumentIssueDateAD), Message = $"{nameof(request.DocumentIssueDateAD)} or {nameof(request.DocumentIssueDateBS)} is required" });
                result.AddError(new() { Field = nameof(request.DocumentIssueDateBS), Message = $"{nameof(request.DocumentIssueDateAD)} or {nameof(request.DocumentIssueDateBS)} is required" });
            }

            // Review this (Optional)
            if (!string.IsNullOrWhiteSpace(request.DocumentExpiryDateAD) && !DateTime.TryParseExact(request.DocumentExpiryDateAD, "yyyy-MM-dd", null, DateTimeStyles.None, out _))
                result.AddError(new() { Field = nameof(request.DocumentExpiryDateAD), Message = $"{nameof(request.DocumentExpiryDateAD)} is invalid" });

            // Review this (Optional)
            if (!string.IsNullOrWhiteSpace(request.DocumentExpiryDateBS) && !NepaliDate.TryParse(request.DocumentExpiryDateBS, out _))
                result.AddError(new() { Field = nameof(request.DocumentExpiryDateBS), Message = $"{nameof(request.DocumentExpiryDateBS)} is invalid" });

            const long fileSizeInbytes = 3 * 1024 * 1024;

            // Review this (optional)
            if (request.DocumentFrontImage is not null)
            {
                if (request.DocumentFrontImage.Length > fileSizeInbytes)
                    result.AddError(new() { Field = nameof(request.DocumentFrontImage), Message = $"{nameof(request.DocumentFrontImage)} file size exceeded. Max size {fileSizeInbytes / 1024 / 1024} MB" });

                if (!FileValidatorUtils.TryValidateImage(request.DocumentFrontImage, out _))
                    result.AddError(new() { Field = nameof(request.DocumentFrontImage), Message = $"{nameof(request.DocumentFrontImage)} is invalid" });
            }

            // Review this (optional)
            if (request.DocumentBackImage is not null)
            {
                if (request.DocumentBackImage.Length > fileSizeInbytes)
                    result.AddError(new() { Field = nameof(request.DocumentBackImage), Message = $"{nameof(request.DocumentBackImage)} file size exceeded. Max size {fileSizeInbytes / 1024 / 1024} MB" });

                if (!FileValidatorUtils.TryValidateImage(request.DocumentBackImage, out _))
                    result.AddError(new() { Field = nameof(request.DocumentBackImage), Message = $"{nameof(request.DocumentBackImage)} is invalid" });
            }

            if (string.IsNullOrWhiteSpace(request.Signature))
                result.AddError(new() { Field = nameof(request.Signature), Message = $"{nameof(request.Signature)} is required" });

            return result;
        }

        private FieldValidationResult ValidateRequestPayoutAgentWalletModel(RequestPayoutForAgentWalletApi request)
        {
            var result = new FieldValidationResult();

            if (string.IsNullOrWhiteSpace(request.ApiUserName))
                result.AddError(new() { Field = nameof(request.ApiUserName), Message = $"{nameof(request.ApiUserName)} is required" });

            if (string.IsNullOrWhiteSpace(request.RemitTransactionId))
                result.AddError(new() { Field = nameof(request.RemitTransactionId), Message = $"{nameof(request.RemitTransactionId)} is required" });

            if (string.IsNullOrWhiteSpace(request.ProcessId))
                result.AddError(new() { Field = nameof(request.ProcessId), Message = $"{nameof(request.ProcessId)} is required" });

            if (string.IsNullOrWhiteSpace(request.AgentTransactionId))
                result.AddError(new() { Field = nameof(request.AgentTransactionId), Message = $"{nameof(request.AgentTransactionId)} is required" });

            if (string.IsNullOrWhiteSpace(request.PaymentType))
                result.AddError(new() { Field = nameof(request.PaymentType), Message = $"{nameof(request.PaymentType)} is required" });

          
            if (string.IsNullOrWhiteSpace(request.DocumentTypeCode))
                result.AddError(new() { Field = nameof(request.DocumentTypeCode), Message = $"{nameof(request.DocumentTypeCode)} is required" });

            if (string.IsNullOrWhiteSpace(request.DocumentNumber))
                result.AddError(new() { Field = nameof(request.DocumentNumber), Message = $"{nameof(request.DocumentNumber)} is required" });

            // Issue date validation
            const int dateOffsetYears = -150;
            if (!string.IsNullOrWhiteSpace(request.DocumentIssueDateAD))
            {
                if (!DateTime.TryParseExact(request.DocumentIssueDateAD, "yyyy-MM-dd", null, DateTimeStyles.None, out var dateParsedAD)
                    || dateParsedAD > DateTime.Now
                    || dateParsedAD < DateTime.Now.AddYears(dateOffsetYears))
                    result.AddError(new() { Field = nameof(request.DocumentIssueDateAD), Message = $"{nameof(request.DocumentIssueDateAD)} is invalid" });
            }
            else if (!string.IsNullOrWhiteSpace(request.DocumentIssueDateBS))
            {
                if (!NepaliDate.TryParse(request.DocumentIssueDateBS, out var dateParsedBS)
                    || dateParsedBS.EnglishDate > DateTime.Now
                    || dateParsedBS.EnglishDate < DateTime.Now.AddYears(dateOffsetYears))
                    result.AddError(new() { Field = nameof(request.DocumentIssueDateBS), Message = $"{nameof(request.DocumentIssueDateBS)} is invalid" });
            }
            else
            {
                result.AddError(new() { Field = nameof(request.DocumentIssueDateAD), Message = $"{nameof(request.DocumentIssueDateAD)} or {nameof(request.DocumentIssueDateBS)} is required" });
                result.AddError(new() { Field = nameof(request.DocumentIssueDateBS), Message = $"{nameof(request.DocumentIssueDateAD)} or {nameof(request.DocumentIssueDateBS)} is required" });
            }

            // Review this (Optional)
            if (!string.IsNullOrWhiteSpace(request.DocumentExpiryDateAD) && !DateTime.TryParseExact(request.DocumentExpiryDateAD, "yyyy-MM-dd", null, DateTimeStyles.None, out _))
                result.AddError(new() { Field = nameof(request.DocumentExpiryDateAD), Message = $"{nameof(request.DocumentExpiryDateAD)} is invalid" });

            // Review this (Optional)
            if (!string.IsNullOrWhiteSpace(request.DocumentExpiryDateBS) && !NepaliDate.TryParse(request.DocumentExpiryDateBS, out _))
                result.AddError(new() { Field = nameof(request.DocumentExpiryDateBS), Message = $"{nameof(request.DocumentExpiryDateBS)} is invalid" });

            const long fileSizeInbytes = 3 * 1024 * 1024;

            // Review this (optional)
            if (request.DocumentFrontImage is not null)
            {
                if (request.DocumentFrontImage.Length > fileSizeInbytes)
                    result.AddError(new() { Field = nameof(request.DocumentFrontImage), Message = $"{nameof(request.DocumentFrontImage)} file size exceeded. Max size {fileSizeInbytes / 1024 / 1024} MB" });

                if (!FileValidatorUtils.TryValidateImage(request.DocumentFrontImage, out _))
                    result.AddError(new() { Field = nameof(request.DocumentFrontImage), Message = $"{nameof(request.DocumentFrontImage)} is invalid" });
            }

            // Review this (optional)
            if (request.DocumentBackImage is not null)
            {
                if (request.DocumentBackImage.Length > fileSizeInbytes)
                    result.AddError(new() { Field = nameof(request.DocumentBackImage), Message = $"{nameof(request.DocumentBackImage)} file size exceeded. Max size {fileSizeInbytes / 1024 / 1024} MB" });

                if (!FileValidatorUtils.TryValidateImage(request.DocumentBackImage, out _))
                    result.AddError(new() { Field = nameof(request.DocumentBackImage), Message = $"{nameof(request.DocumentBackImage)} is invalid" });
            }

            if (string.IsNullOrWhiteSpace(request.Signature))
                result.AddError(new() { Field = nameof(request.Signature), Message = $"{nameof(request.Signature)} is required" });

            return result;
        }

        private FieldValidationResult ValidateCheckPayoutStatusModel(CheckPayoutStatusRequest request)
        {
            var result = new FieldValidationResult();

            if (string.IsNullOrWhiteSpace(request.ApiUserName))
                result.AddError(new() { Field = nameof(request.ApiUserName), Message = $"{nameof(request.ApiUserName)} is required" });

            if (string.IsNullOrWhiteSpace(request.RemitTransactionId))
                result.AddError(new() { Field = nameof(request.RemitTransactionId), Message = $"{nameof(request.RemitTransactionId)} is required" });

            if (string.IsNullOrWhiteSpace(request.Signature))
                result.AddError(new() { Field = nameof(request.Signature), Message = $"{nameof(request.Signature)} is required" });

            return result;
        }

        private bool TryDecrypt(string cipherText, out string plainText)
        {
            plainText = null;

            if (string.IsNullOrWhiteSpace(cipherText))
                return false;

            var pvtKey = _httpContextAccessor.HttpContext?.User.FindFirstValue(AgentClaimTypes.SystemPrivateKey);
            if (pvtKey is null)
                return false;

            try
            {
                var pvtKeyParam = RsaCryptoUtils.ImportPrivateKeyPem(pvtKey);
                var cipheredDataBytes = Convert.FromBase64String(cipherText);

                var plainBytes = RsaCryptoUtils.DecryptData(cipheredDataBytes, pvtKeyParam, decryptionAlgorithm: RsaCryptoUtils.AlgorithmRsaNoneOaepWithSha256AndMgf1Padding);
                plainText = Encoding.UTF8.GetString(plainBytes);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private async Task<bool> IsSignatureValidAsync(string signature, string signatureData)
        {
            if (signature is null || signatureData is null)
                return false;

            var userPublicKey = _httpContextAccessor.HttpContext?.User.FindFirstValue(AgentClaimTypes.UserPublicKey);
            if (string.IsNullOrWhiteSpace(userPublicKey))
                return false;

            try
            {
                var rsaKeyParam = RsaCryptoUtils.ImportPublicKeyPem(userPublicKey);

                var signatureBytes = Convert.FromBase64String(signature);
                var dataBytes = Encoding.UTF8.GetBytes(signatureData);

                return RsaCryptoUtils.VerifySignature(dataBytes, signatureBytes, rsaKeyParam);
            }
            catch (Exception ex)
            {
                await _exceptionLogger.LogAsync(ex);
                return false;
            }
        }

        private (HttpStatusCode, object) HandleInvalidSignatureTokenResponse<T>() where T : ApiResponse
        {
            var instance = Activator.CreateInstance<T>();
            instance.ResponseCode = ResponseCodes.CodeE601_InvalidSignatureToken;
            instance.ResponseStatus = ResponseStatuses.Error;
            instance.ResponseMessage = ResponseMessages.MsgE601_InvalidSignatureToken;

            return (HttpStatusCode.BadRequest, instance);
        }

        private (HttpStatusCode, object) HandleInvalidProcessIdResponse<T>() where T : ApiResponse
        {
            var instance = Activator.CreateInstance<T>();
            instance.ResponseCode = ResponseCodes.CodeE602_InvalidProcessId;
            instance.ResponseStatus = ResponseStatuses.Error;
            instance.ResponseMessage = ResponseMessages.MsgE602_InvalidProcessId;

            return (HttpStatusCode.BadRequest, instance);
        }

        private (HttpStatusCode, T) HandleInvalidEncryptionErrorResponse<T>(string responseMessage = null, string responseDetailMessage = null) where T : ApiResponse
        {
            var instance = Activator.CreateInstance<T>();
            instance.ResponseCode = ResponseCodes.CodeE603_InvalidEncryption;
            instance.ResponseStatus = ResponseStatuses.Error;
            instance.ResponseMessage = ResponseMessages.MsgE603_InvalidEncryption;
            instance.ResponseDetailMessage = "Possible reasons include incorrect encryption key, data corruption, or encryption algorithm mismatch. Please review the provided data and encryption parameters and try again.";

            if (responseMessage is not null)
                instance.ResponseMessage = responseMessage;

            if (responseDetailMessage is not null)
                instance.ResponseDetailMessage = responseDetailMessage;

            return (HttpStatusCode.BadRequest, instance);
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

        private (HttpStatusCode, T) HandleInternalServerErrorResponse<T>(string responseMessage = null, string responseDetailMessage = null) where T : ApiResponse
        {
            var instance = Activator.CreateInstance<T>();
            instance.ResponseCode = ResponseCodes.Code500_InternalServerError;
            instance.ResponseStatus = ResponseStatuses.Error;
            instance.ResponseMessage = ResponseMessages.Msg500_InternalServerError;

            if (responseMessage is not null)
                instance.ResponseMessage = responseMessage;

            if (responseDetailMessage is not null)
                instance.ResponseDetailMessage = responseDetailMessage;

            return (HttpStatusCode.BadRequest, instance);
        }

        public async Task<(HttpStatusCode, object)> RequestPayoutForAgentWalletAsync(RequestPayoutForAgentWalletApi request)
        {
            var agentCode = _httpContextAccessor.HttpContext?.User.FindFirstValue(AgentClaimTypes.AgentCode);
            var apiUserName = _httpContextAccessor.HttpContext?.User.FindFirstValue(AgentClaimTypes.ApiUserName);

            _agentApiLogger.SetLogContext(transactionId: request.RemitTransactionId, trackerId: request.AgentTransactionId);

            if (string.IsNullOrEmpty(agentCode))
                return HandleBadRequestResponse<RequestPayoutResponse>();

            if (string.IsNullOrEmpty(apiUserName))
                return HandleBadRequestResponse<RequestPayoutResponse>();

            var fieldValidationResult = ValidateRequestPayoutAgentWalletModel(request);
            if (!fieldValidationResult.Success)
                return HandleBadRequestResponse<RequestPayoutResponse>(responseDetailMessage: "One or more field validations failed",
                    fieldErrors: fieldValidationResult.Errors);

            if (!apiUserName.Equals(request.ApiUserName))
                return HandleBadRequestResponse<RequestPayoutResponse>(responseDetailMessage: "One or more field validations failed",
                    fieldErrors: new() { new() { Field = nameof(request.ApiUserName), Message = "Invalid ApiUserName." } });

            var signatureData = $"{request.ApiUserName},{request.RemitTransactionId},{request.ProcessId},{request.AgentTransactionId},{request.PaymentType},{request.DocumentTypeCode},{request.DocumentNumber}";
            var isSignatureValid = await IsSignatureValidAsync(request.Signature, signatureData);

            if (!isSignatureValid)
                return HandleInvalidSignatureTokenResponse<RequestPayoutResponse>();

            var docBasePath = _config["Folder:TransactionPayoutDocs"] ?? "";
            var docUploadPath = $"{docBasePath.TrimEnd('/')}/{DateTime.Now.Year}/{DateTime.Now:MMdd}/";

            string docFrontImagePath = null;
            string docBackImagePath = null;

            if (request.DocumentFrontImage is not null && !_fileProviderService.TryUploadFile(request.DocumentFrontImage, docUploadPath, out docFrontImagePath))
                return HandleInternalServerErrorResponse<RequestPayoutResponse>();

            if (request.DocumentBackImage is not null && !_fileProviderService.TryUploadFile(request.DocumentFrontImage, docUploadPath, out docBackImagePath))
                return HandleInternalServerErrorResponse<RequestPayoutResponse>();

            var param = new RequestPayoutParam
            {
                PayoutTransactionType = "AGENTAPITRANSACTION",
                UserType = "AGENT",
                RemitTransactionId = request.RemitTransactionId,
                ProcessId = request.ProcessId,
                AgentTransactionId = request.AgentTransactionId,
                PaymentType = request.PaymentType,
                DocumentTypeCode = request.DocumentTypeCode,
                DocumentNumber = request.DocumentNumber,
                DocumentIssueDate = request.DocumentIssueDateAD,
                DocumentExpiryDate = request.DocumentExpiryDateAD,
                DocumentIssueDateNepali = request.DocumentIssueDateBS,
                DocumentExpiryDateNepali = request.DocumentExpiryDateBS,
                DocumentFrontImagePath = docFrontImagePath,
                DocumentBackImagePath = docBackImagePath,
                AgentCode = agentCode,
                Username = apiUserName,
                LoggedInUser = apiUserName,
                DeviceId = "APICLIENT",
                IpAddress = _httpContextAccessor.HttpContext?.GetIpAddress(),
            };

            var (sprocStatus, details) = await _agentApiRepository.RequestPayoutAsync(param);

            var ClearFileUploadsForFailedOperation = () =>
            {
                if (docFrontImagePath is not null)
                    _fileProviderService.DeleteFile(docFrontImagePath);

                if (docBackImagePath is not null)
                    _fileProviderService.DeleteFile(docBackImagePath);
            };

            if (sprocStatus.StatusCode == 411)
            {
                ClearFileUploadsForFailedOperation();
                return HandleInvalidProcessIdResponse<RequestPayoutResponse>();
            }

            if (sprocStatus.StatusCode != 200 || details is null)
            {
                ClearFileUploadsForFailedOperation();
                return HandleBadRequestResponse<RequestPayoutResponse>(responseMessage: sprocStatus.MsgText);
            }

            var response = MapToRequestPayoutResponse(details);
            response.ResponseCode = ResponseCodes.Code200_Success;
            response.ResponseStatus = ResponseStatuses.Success;
            response.ResponseMessage = "Payout successful!";

            return (HttpStatusCode.OK, response);
        }
    }
}
