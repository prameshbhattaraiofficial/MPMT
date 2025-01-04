using Mpmt.Core.Dtos.Logging;

namespace Mpmt.Services.Logging
{
    public interface IExceptionLogger
    {
        Task LogAsync(string logId = null, string userName = null, string userAgent = null, string remoteIpAddress = null, string controllerName = null, string actionName = null, string queryString = null, string headers = null, string requestUrl = null, string httpMethod = null, string requestBody = null, string exceptionType = null, string exceptionMessage = null, string exceptionStackTrace = null, string innerExceptionMessage = null, string innerExceptionStackTrace = null, string machineName = null, string environment = null);
        Task LogAsync(Exception ex);
        Task LogAsync(ExceptionLogParam log);
    }
}
