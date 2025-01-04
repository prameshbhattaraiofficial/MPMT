using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Mpmt.Core.Dtos.Logging;
using Mpmt.Data.Repositories.Logging;
using Mpmt.Services.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Mpmt.Services.Logging
{
    public class VendorApiLogger : IVendorApiLogger
    {
        private int _logUpdateCount = 0;
        private AppVendorApiLog _logContext;

        private readonly IMapper _mapper;
        private readonly IVendorApiLogRepository _vendorApiLogRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _hostEnv;

        public VendorApiLogger(
            IMapper mapper,
            IVendorApiLogRepository vendorApiLogRepository,
            IHttpContextAccessor httpContextAccessor,
            IWebHostEnvironment hostEnv)
        {
            _mapper = mapper;
            _vendorApiLogRepository = vendorApiLogRepository;
            _httpContextAccessor = httpContextAccessor;
            _hostEnv = hostEnv;
            _logContext = new AppVendorApiLog { LogId = _httpContextAccessor.HttpContext?.TraceIdentifier ?? Guid.NewGuid().ToString("D") };
        }

        public async Task LogInsertAsync(string transactionId = null, string trackerId = null, string requestInput = null, string responseOutput = null, int? responseHttpStatus = null,
            string requestUrl = null, string requestHeaders = null, string vendorRequestInput = null, string vendorResponseOutput = null, string vendorRequestURL = null,
            string vendorRequestHeaders = null, int? vendorResponseHttpStatus = null, bool? vendorResponseStatus = null, string vendorResponseState = null, string vendorResponseMessage = null,
            string vendorTransactionId = null, string vendorTrackerId = null, string vendorException = null, string vendorExceptionStackTrace = null, string vendorId = null, string vendorType = null,
            string vendorRequestInput2 = null, string vendorResponseOutput2 = null, string vendorRequestURL2 = null, string vendorRequestHeaders2 = null, int? vendorResponseHttpStatus2 = null,
            bool? vendorResponseStatus2 = null, string vendorResponseState2 = null, string vendorResponseMessage2 = null, string vendorTransactionId2 = null, string vendorTrackerId2 = null,
            string vendorException2 = null, string vendorExceptionStackTrace2 = null, string vendorId2 = null, string vendorType2 = null,
            string vendorRequestInput3 = null, string vendorResponseOutput3 = null, string vendorRequestURL3 = null, string vendorRequestHeaders3 = null, int? vendorResponseHttpStatus3 = null,
            bool? vendorResponseStatus3 = null, string vendorResponseState3 = null, string vendorResponseMessage3 = null, string vendorTransactionId3 = null, string vendorTrackerId3 = null,
            string vendorException3 = null, string vendorExceptionStackTrace3 = null, string vendorId3 = null, string vendorType3 = null,
            string clientCode = null, string partnerCode = null, string agentCode = null, string memberId = null, string memberUserName = null,
            string memberName = null, string deviceCode = null, string ipAddress = null, string platform = null, string machineName = null, string environment = null)
        {
            try
            {
                var reqestInputObject = JsonConvert.DeserializeObject<JObject>(requestInput);
                var requestProcessId = reqestInputObject.SelectToken("processId")?.ToString();
                var requestPartnerTransactionId = reqestInputObject.SelectToken("partnerTransactionId")?.ToString();

                var log = new VendorApiLogParam
                {
                    LogId = _logContext.LogId,
                    TransactionId = transactionId,
                    TrackerId = trackerId,
                    RequestInput = requestInput,
                    ResponseOutput = responseOutput,
                    ResponseHttpStatus = responseHttpStatus,
                    RequestUrl = requestUrl ?? _httpContextAccessor.HttpContext?.GetRequestUrl(),
                    RequestHeaders = requestHeaders ?? _httpContextAccessor.HttpContext?.GetRequestHeadersAsDictionaryJson(),

                    PartnerProcessId = requestProcessId,
                    PartnerTransactionId = requestPartnerTransactionId,

                    VendorRequestInput = vendorRequestInput,
                    VendorResponseOutput = vendorResponseOutput,
                    VendorRequestURL = vendorRequestURL,
                    VendorRequestHeaders = vendorRequestHeaders,
                    VendorTransactionId = vendorTransactionId,
                    VendorResponseHttpStatus = vendorResponseHttpStatus,
                    VendorResponseStatus = vendorResponseStatus,
                    VendorResponseState = vendorResponseState,
                    VendorResponseMessage = vendorResponseMessage,
                    VendorTrackerId = vendorTrackerId,
                    VendorException = vendorException,
                    VendorExceptionStackTrace = vendorExceptionStackTrace,
                    VendorId = vendorId,
                    VendorType = vendorType,

                    VendorRequestInput2 = vendorRequestInput2,
                    VendorResponseOutput2 = vendorResponseOutput2,
                    VendorRequestURL2 = vendorRequestURL2,
                    VendorRequestHeaders2 = vendorRequestHeaders2,
                    VendorResponseHttpStatus2 = vendorResponseHttpStatus2,
                    VendorResponseStatus2 = vendorResponseStatus2,
                    VendorResponseState2 = vendorResponseState2,
                    VendorResponseMessage2 = vendorResponseMessage2,
                    VendorTransactionId2 = vendorTransactionId2,
                    VendorTrackerId2 = vendorTrackerId2,
                    VendorException2 = vendorException2,
                    VendorExceptionStackTrace2 = vendorExceptionStackTrace2,
                    VendorId2 = vendorId2,
                    VendorType2 = vendorType2,

                    VendorRequestInput3 = vendorRequestInput3,
                    VendorResponseOutput3 = vendorResponseOutput3,
                    VendorRequestURL3 = vendorRequestURL3,
                    VendorRequestHeaders3 = vendorRequestHeaders3,
                    VendorResponseHttpStatus3 = vendorResponseHttpStatus3,
                    VendorResponseStatus3 = vendorResponseStatus3,
                    VendorResponseState3 = vendorResponseState3,
                    VendorResponseMessage3 = vendorResponseMessage3,
                    VendorTransactionId3 = vendorTransactionId3,
                    VendorTrackerId3 = vendorTrackerId3,
                    VendorException3 = vendorException3,
                    VendorExceptionStackTrace3 = vendorExceptionStackTrace3,
                    VendorId3 = vendorId3,
                    VendorType3 = vendorType3,

                    ClientCode = clientCode, // ?? _httpContextAccessor.HttpContext?.GetClientCode(),
                    PartnerCode = partnerCode ?? _httpContextAccessor.HttpContext?.GetPartnerCode(),
                    AgentCode = agentCode, // ?? _httpContextAccessor.HttpContext?.GetAgentCode(),
                    MemberId = memberId,
                    MemberUserName = memberUserName ?? _httpContextAccessor.HttpContext?.GetUserName(),
                    MemberName = memberName,

                    DeviceCode = deviceCode ?? _httpContextAccessor.HttpContext?.GetParsedUserAgent(),
                    IpAddress = ipAddress ?? _httpContextAccessor.HttpContext?.GetIpAddress(),
                    Platform = platform ?? _httpContextAccessor.HttpContext?.GetUserAgentDevicePlatform(),
                    MachineName = machineName ?? Environment.MachineName,
                    Environment = environment ?? _hostEnv.EnvironmentName
                };

                if (log.RequestInput is null)
                    (_, log.RequestInput) = await _httpContextAccessor.HttpContext?.GetRequestBodyAsStringAsync();

                _logContext = _mapper.Map<AppVendorApiLog>(log);

                await _vendorApiLogRepository.LogInsertAsync(log);
                IncrementLogUpdateCount();
            }
            catch (Exception) { }
        }

        public async Task LogUpdateResponseAsync(string transactionId = null, string trackerId = null, string responseOutput = null, int? responseHttpStatus = null,
            string vendorTransactionId = null, string vendorTrackerId = null, string vendorId = null, string vendorType = null,
            string vendorTransactionId2 = null, string vendorTrackerId2 = null, string vendorId2 = null, string vendorType2 = null,
            string vendorTransactionId3 = null, string vendorTrackerId3 = null, string vendorId3 = null, string vendorType3 = null)
        {
            if (_logUpdateCount < 1)
                return;

            var log = _mapper.Map<VendorApiLogParam>(_logContext);
            log.TransactionId = transactionId ?? _logContext.TransactionId;
            log.TrackerId = trackerId ?? _logContext.TrackerId;
            log.ResponseOutput = responseOutput ?? _logContext.ResponseOutput;
            log.ResponseHttpStatus = responseHttpStatus ?? _logContext.ResponseHttpStatus;

            log.VendorTransactionId = vendorTransactionId ?? _logContext.VendorTransactionId;
            log.VendorTrackerId = vendorTrackerId ?? _logContext.VendorTrackerId;
            log.VendorId = vendorId ?? _logContext.VendorId;
            log.VendorType = vendorType ?? _logContext.VendorType;

            log.VendorTransactionId2 = vendorTransactionId2 ?? _logContext.VendorTransactionId2;
            log.VendorTrackerId2 = vendorTrackerId2 ?? _logContext.VendorTrackerId2;
            log.VendorId2 = vendorId2 ?? _logContext.VendorId2;
            log.VendorType2 = vendorType2 ?? _logContext.VendorType2;

            log.VendorTransactionId3 = vendorTransactionId3 ?? _logContext.VendorTransactionId3;
            log.VendorTrackerId3 = vendorTrackerId3 ?? _logContext.VendorTrackerId3;
            log.VendorId3 = vendorId3 ?? _logContext.VendorId3;
            log.VendorType3 = vendorType3 ?? _logContext.VendorType3;

            try
            {
                _logContext = _mapper.Map<AppVendorApiLog>(log);

                await _vendorApiLogRepository.LogUpdateAsync(log);
                IncrementLogUpdateCount();
            }
            catch (Exception) { }
        }

        public async Task LogVendorApiExceptionAsync(string vendorException, string vendorExceptionStackTrace)
        {
            var log = _mapper.Map<VendorApiLogParam>(_logContext);
            log.VendorException = vendorException;
            log.VendorExceptionStackTrace = vendorExceptionStackTrace;

            try
            {
                _logContext = _mapper.Map<AppVendorApiLog>(log);

                if (_logUpdateCount > 0)
                    await _vendorApiLogRepository.LogUpdateAsync(log);
                else
                    await _vendorApiLogRepository.LogInsertAsync(log);

                IncrementLogUpdateCount();
            }
            catch (Exception) { }
        }

        public async Task LogVendorApiException2Async(string vendorException2, string vendorExceptionStackTrace2)
        {
            var log = _mapper.Map<VendorApiLogParam>(_logContext);
            log.VendorException2 = vendorException2;
            log.VendorExceptionStackTrace2 = vendorExceptionStackTrace2;

            try
            {
                _logContext = _mapper.Map<AppVendorApiLog>(log);

                if (_logUpdateCount > 0)
                    await _vendorApiLogRepository.LogUpdateAsync(log);
                else
                    await _vendorApiLogRepository.LogInsertAsync(log);

                IncrementLogUpdateCount();
            }
            catch (Exception) { }
        }

        public async Task LogVendorApiException3Async(string vendorException3, string vendorExceptionStackTrace3)
        {
            var log = _mapper.Map<VendorApiLogParam>(_logContext);
            log.VendorException3 = vendorException3;
            log.VendorExceptionStackTrace3 = vendorExceptionStackTrace3;

            try
            {
                _logContext = _mapper.Map<AppVendorApiLog>(log);

                if (_logUpdateCount > 0)
                    await _vendorApiLogRepository.LogUpdateAsync(log);
                else
                    await _vendorApiLogRepository.LogInsertAsync(log);

                IncrementLogUpdateCount();
            }
            catch (Exception) { }
        }

        public async Task LogVendorApiResponseAsync(string vendorRequestInput = null, string vendorResponseOutput = null, string vendorRequestURL = null, string vendorRequestHeaders = null,
            int? vendorResponseHttpStatus = null, bool? vendorResponseStatus = null, string vendorResponseState = null, string vendorResponseMessage = null, string vendorTransactionId = null,
            string vendorTrackerId = null, string vendorException = null, string vendorExceptionStackTrace = null, string vendorId = null, string vendorType = null)
        {
            var log = _mapper.Map<VendorApiLogParam>(_logContext);
            log.VendorRequestInput = vendorRequestInput;
            log.VendorResponseOutput = vendorResponseOutput;
            log.VendorRequestURL = vendorRequestURL;
            log.VendorRequestHeaders = vendorRequestHeaders;
            log.VendorResponseHttpStatus = vendorResponseHttpStatus;
            log.VendorResponseStatus = vendorResponseStatus;
            log.VendorResponseState = vendorResponseState;
            log.VendorResponseMessage = vendorResponseMessage;
            log.VendorTransactionId = vendorTransactionId;
            log.VendorTrackerId = vendorTrackerId;
            log.VendorException = vendorException;
            log.VendorExceptionStackTrace = vendorExceptionStackTrace;
            log.VendorId = vendorId;
            log.VendorType = vendorType;

            try
            {
                // Setting log context prevents overriding of context parameters
                //SetLogContext(vendorTransactionId: log.VendorTransactionId, vendorTrackerId: log.VendorTrackerId, vendorId: log.VendorId, vendorType: log.VendorType);
                _logContext = _mapper.Map<AppVendorApiLog>(log);

                if (_logUpdateCount > 0)
                    await _vendorApiLogRepository.LogUpdateAsync(log);
                else
                    await _vendorApiLogRepository.LogInsertAsync(log);

                IncrementLogUpdateCount();
            }
            catch (Exception) { }
        }

        public async Task LogVendorApiResponse2Async(string vendorRequestInput2 = null, string vendorResponseOutput2 = null, string vendorRequestURL2 = null, string vendorRequestHeaders2 = null,
            int? vendorResponseHttpStatus2 = null, bool? vendorResponseStatus2 = null, string vendorResponseState2 = null, string vendorResponseMessage2 = null, string vendorTransactionId2 = null,
            string vendorTrackerId2 = null, string vendorException2 = null, string vendorExceptionStackTrace2 = null, string vendorId2 = null, string vendorType2 = null)
        {
            var log = _mapper.Map<VendorApiLogParam>(_logContext);
            log.VendorRequestInput2 = vendorRequestInput2;
            log.VendorResponseOutput2 = vendorResponseOutput2;
            log.VendorRequestURL2 = vendorRequestURL2;
            log.VendorRequestHeaders2 = vendorRequestHeaders2;
            log.VendorResponseHttpStatus2 = vendorResponseHttpStatus2;
            log.VendorResponseStatus2 = vendorResponseStatus2;
            log.VendorResponseState2 = vendorResponseState2;
            log.VendorResponseMessage2 = vendorResponseMessage2;
            log.VendorTransactionId2 = vendorTransactionId2;
            log.VendorTrackerId2 = vendorTrackerId2;
            log.VendorException2 = vendorException2;
            log.VendorExceptionStackTrace2 = vendorExceptionStackTrace2;
            log.VendorId2 = vendorId2;
            log.VendorType2 = vendorType2;

            try
            {
                // Setting log context prevents overriding of context parameters
                //SetLogContext(vendorTransactionId2: log.VendorTransactionId2, vendorTrackerId2: log.VendorTrackerId2, vendorId2: log.VendorId2, vendorType2: log.VendorType2);
                _logContext = _mapper.Map<AppVendorApiLog>(log);

                if (_logUpdateCount > 0)
                    await _vendorApiLogRepository.LogUpdateAsync(log);
                else
                    await _vendorApiLogRepository.LogInsertAsync(log);

                IncrementLogUpdateCount();
            }
            catch (Exception) { }
        }

        public async Task LogVendorApiResponse3Async(string vendorRequestInput3 = null, string vendorResponseOutput3 = null, string vendorRequestURL3 = null, string vendorRequestHeaders3 = null,
            int? vendorResponseHttpStatus3 = null, bool? vendorResponseStatus3 = null, string vendorResponseState3 = null, string vendorResponseMessage3 = null, string vendorTransactionId3 = null,
            string vendorTrackerId3 = null, string vendorException3 = null, string vendorExceptionStackTrace3 = null, string vendorId3 = null, string vendorType3 = null)
        {
            var log = _mapper.Map<VendorApiLogParam>(_logContext);
            log.VendorRequestInput3 = vendorRequestInput3;
            log.VendorResponseOutput3 = vendorResponseOutput3;
            log.VendorRequestURL3 = vendorRequestURL3;
            log.VendorRequestHeaders3 = vendorRequestHeaders3;
            log.VendorResponseHttpStatus3 = vendorResponseHttpStatus3;
            log.VendorResponseStatus3 = vendorResponseStatus3;
            log.VendorResponseState3 = vendorResponseState3;
            log.VendorResponseMessage3 = vendorResponseMessage3;
            log.VendorTransactionId3 = vendorTransactionId3;
            log.VendorTrackerId3 = vendorTrackerId3;
            log.VendorException3 = vendorException3;
            log.VendorExceptionStackTrace3 = vendorExceptionStackTrace3;
            log.VendorId3 = vendorId3;
            log.VendorType3 = vendorType3;

            try
            {
                // Setting log context prevents overriding of context parameters
                //SetLogContext(vendorTransactionId3: log.VendorTransactionId3, vendorTrackerId3: log.VendorTrackerId3, vendorId3: log.VendorId3, vendorType3: log.VendorType3);
                _logContext = _mapper.Map<AppVendorApiLog>(log);

                if (_logUpdateCount > 0)
                    await _vendorApiLogRepository.LogUpdateAsync(log);
                else
                    await _vendorApiLogRepository.LogInsertAsync(log);

                IncrementLogUpdateCount();
            }
            catch (Exception) { }
        }

        public void SetLogContext(string transactionId = null, string trackerId = null, string vendorTransactionId = null, string vendorTrackerId = null,
            string vendorId = null, string vendorType = null, string vendorTransactionId2 = null, string vendorTrackerId2 = null,
            string vendorId2 = null, string vendorType2 = null, string vendorTransactionId3 = null, string vendorTrackerId3 = null,
            string vendorId3 = null, string vendorType3 = null)
        {
            _logContext.TransactionId = transactionId ?? _logContext.TransactionId;
            _logContext.TrackerId = trackerId ?? _logContext.TrackerId;
            _logContext.VendorTransactionId = vendorTransactionId ?? _logContext.VendorTransactionId;
            _logContext.VendorTrackerId = vendorTrackerId ?? _logContext.VendorTrackerId;
            _logContext.VendorId = vendorId ?? _logContext.VendorId;
            _logContext.VendorType = vendorType ?? _logContext.VendorType;
            _logContext.VendorTransactionId2 = vendorTransactionId2 ?? _logContext.VendorTransactionId2;
            _logContext.VendorTrackerId2 = vendorTrackerId2 ?? _logContext.VendorTrackerId2;
            _logContext.VendorId2 = vendorId2 ?? _logContext.VendorId2;
            _logContext.VendorType2 = vendorType2 ?? _logContext.VendorType2;
            _logContext.VendorTransactionId3 = vendorTransactionId3 ?? _logContext.VendorTransactionId3;
            _logContext.VendorTrackerId3 = vendorTrackerId3 ?? _logContext.VendorTrackerId3;
            _logContext.VendorId3 = vendorId3 ?? _logContext.VendorId3;
            _logContext.VendorType3 = vendorType3 ?? _logContext.VendorType3;
        }

        private void IncrementLogUpdateCount()
        {
            _logUpdateCount++;
        }

        public AppVendorApiLog GetLogInstance()
        {
            return _logContext;
        }
    }
}
