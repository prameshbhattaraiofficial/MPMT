using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Mpmt.Core.Dtos.Logging;
using Mpmt.Data.Repositories.Logging;
using Mpmt.Services.Extensions;

namespace Mpmt.Services.Logging
{
    public class ExceptionLogger : IExceptionLogger
    {
        private readonly IExceptionLogRepository _exceptionLogRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _hostEnv;

        public ExceptionLogger(
            IExceptionLogRepository exceptionLogRepository,
            IHttpContextAccessor httpContextAccessor,
            IWebHostEnvironment hostEnv)
        {
            _exceptionLogRepository = exceptionLogRepository;
            _httpContextAccessor = httpContextAccessor;
            _hostEnv = hostEnv;
        }

        public async Task LogAsync(string logId = null, string userName = null, string userAgent = null, string remoteIpAddress = null, string controllerName = null, string actionName = null, string queryString = null, string headers = null, string requestUrl = null, string httpMethod = null, string requestBody = null, string exceptionType = null, string exceptionMessage = null, string exceptionStackTrace = null, string innerExceptionMessage = null, string innerExceptionStackTrace = null, string machineName = null, string environment = null)
        {
            try
            {
                var logParam = new ExceptionLogParam
                {
                    LogId = logId,
                    UserName = userName,
                    UserAgent = userAgent,
                    RemoteIpAddress = remoteIpAddress,
                    ControllerName = controllerName,
                    ActionName = actionName,
                    QueryString = queryString,
                    Headers = headers,
                    RequestUrl = requestUrl,
                    HttpMethod = httpMethod,
                    RequestBody = requestBody,
                    ExceptionType = exceptionType,
                    ExceptionMessage = exceptionMessage,
                    ExceptionStackTrace = exceptionStackTrace,
                    InnerExceptionMessage = innerExceptionMessage,
                    InnerExceptionStackTrace = innerExceptionStackTrace,
                    MachineName = machineName,
                    Environment = environment,
                };

                if (_httpContextAccessor.HttpContext is not null)
                {
                    //logParam.UserName ??= _httpContextAccessor.HttpContext.GetUserName();
                    logParam.UserAgent ??= _httpContextAccessor.HttpContext.GetUserAgent();
                    logParam.RemoteIpAddress ??= _httpContextAccessor.HttpContext.GetIpAddress();
                    logParam.ControllerName ??= _httpContextAccessor.HttpContext.GetController();
                    logParam.ActionName ??= _httpContextAccessor.HttpContext.GetAction();
                    logParam.QueryString ??= _httpContextAccessor.HttpContext.GetQueryString();
                    logParam.Headers ??= _httpContextAccessor.HttpContext.GetRequestHeaders();
                    logParam.RequestUrl ??= _httpContextAccessor.HttpContext.GetRequestUrl();
                    logParam.HttpMethod ??= _httpContextAccessor.HttpContext.Request.Method;

                    if (logParam.RequestBody is null)
                        (_, logParam.RequestBody) = await _httpContextAccessor.HttpContext.GetRequestBodyAsStringAsync();
                }

                logParam.MachineName ??= Environment.MachineName;
                logParam.Environment ??= _hostEnv.EnvironmentName;
                logParam.LogId ??= _httpContextAccessor.HttpContext?.TraceIdentifier ?? Guid.NewGuid().ToString("D");

                await _exceptionLogRepository.AddAsync(logParam);
            }
            catch (Exception) { }
        }

        public async Task LogAsync(Exception ex)
        {
            if (ex is null)
                return;

            try
            {
                var logParam = new ExceptionLogParam
                {
                    ExceptionType = ex.GetType().FullName,
                    ExceptionMessage = ex.Message,
                    ExceptionStackTrace = ex.StackTrace,
                    InnerExceptionMessage = ex.InnerException?.Message,
                    InnerExceptionStackTrace = ex.InnerException?.StackTrace
                };

                if (_httpContextAccessor.HttpContext is not null)
                {
                    //logParam.UserName ??= _httpContextAccessor.HttpContext.GetUserName();
                    logParam.UserAgent ??= _httpContextAccessor.HttpContext.GetUserAgent();
                    logParam.RemoteIpAddress ??= _httpContextAccessor.HttpContext.GetIpAddress();
                    logParam.ControllerName ??= _httpContextAccessor.HttpContext.GetController();
                    logParam.ActionName ??= _httpContextAccessor.HttpContext.GetAction();
                    logParam.QueryString ??= _httpContextAccessor.HttpContext.GetQueryString();
                    logParam.Headers ??= _httpContextAccessor.HttpContext.GetRequestHeaders();
                    logParam.RequestUrl ??= _httpContextAccessor.HttpContext.GetRequestUrl();
                    logParam.HttpMethod ??= _httpContextAccessor.HttpContext.Request.Method;

                    if (logParam.RequestBody is null)
                        (_, logParam.RequestBody) = await _httpContextAccessor.HttpContext.GetRequestBodyAsStringAsync();
                }

                logParam.MachineName ??= Environment.MachineName;
                logParam.Environment ??= _hostEnv.EnvironmentName;
                logParam.LogId ??= _httpContextAccessor.HttpContext?.TraceIdentifier ?? Guid.NewGuid().ToString("D");

                await _exceptionLogRepository.AddAsync(logParam);
            }
            catch (Exception) { }
        }

        public async Task LogAsync(ExceptionLogParam log)
        {
            if (log is null)
                return;

            log.LogId ??= _httpContextAccessor.HttpContext?.TraceIdentifier ?? Guid.NewGuid().ToString("D");

            try
            {
                await _exceptionLogRepository.AddAsync(log);
            }
            catch (Exception) { }
        }
    }
}
