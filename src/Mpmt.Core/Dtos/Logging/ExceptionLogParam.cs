namespace Mpmt.Core.Dtos.Logging
{
    public class ExceptionLogParam
    {
        public string LogId { get; set; }
        public string UserName { get; set; }
        public string UserAgent { get; set; }
        public string RemoteIpAddress { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public string QueryString { get; set; }
        public string Headers { get; set; }
        public string RequestUrl { get; set; }
        public string HttpMethod { get; set; }
        public string RequestBody { get; set; }
        public string ExceptionType { get; set; }
        public string ExceptionMessage { get; set; }
        public string ExceptionStackTrace { get; set; }
        public string InnerExceptionMessage { get; set; }
        public string InnerExceptionStackTrace { get; set; }
        public string MachineName { get; set; }
        public string Environment { get; set; }
    }
}
