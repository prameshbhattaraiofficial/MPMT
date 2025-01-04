namespace Mpmt.Core.Dtos.ActivityLog
{
    /// <summary>
    /// The app exception log param.
    /// </summary>
    public class AppExceptionLogParam
    {
        /// <summary>
        /// Gets or sets the user name.
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// Gets or sets the user agent.
        /// </summary>
        public string UserAgent { get; set; }
        /// <summary>
        /// Gets or sets the remote ip address.
        /// </summary>
        public string RemoteIpAddress { get; set; }
        /// <summary>
        /// Gets or sets the controller name.
        /// </summary>
        public string ControllerName { get; set; }
        /// <summary>
        /// Gets or sets the action name.
        /// </summary>
        public string ActionName { get; set; }
        /// <summary>
        /// Gets or sets the query string.
        /// </summary>
        public string QueryString { get; set; }
        /// <summary>
        /// Gets or sets the form data.
        /// </summary>
        public string FormData { get; set; }
        /// <summary>
        /// Gets or sets the headers.
        /// </summary>
        public string Headers { get; set; }
        /// <summary>
        /// Gets or sets the request url.
        /// </summary>
        public string RequestUrl { get; set; }
        /// <summary>
        /// Gets or sets the http method.
        /// </summary>
        public string HttpMethod { get; set; }
        /// <summary>
        /// Gets or sets the request body.
        /// </summary>
        public string RequestBody { get; set; }
        /// <summary>
        /// Gets or sets the exception type.
        /// </summary>
        public string ExceptionType { get; set; }
        /// <summary>
        /// Gets or sets the exception message.
        /// </summary>
        public string ExceptionMessage { get; set; }
        /// <summary>
        /// Gets or sets the exception stack trace.
        /// </summary>
        public string ExceptionStackTrace { get; set; }
        /// <summary>
        /// Gets or sets the inner exception message.
        /// </summary>
        public string InnerExceptionMessage { get; set; }
        /// <summary>
        /// Gets or sets the inner exception stack trace.
        /// </summary>
        public string InnerExceptionStackTrace { get; set; }
        /// <summary>
        /// Gets or sets the machine name.
        /// </summary>
        public string MachineName { get; set; }
        /// <summary>
        /// Gets or sets the environment.
        /// </summary>
        public string Environment { get; set; }
    }
}
