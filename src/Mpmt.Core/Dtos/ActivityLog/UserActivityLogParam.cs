namespace Mpmt.Core.Dtos.ActivityLog
{
    /// <summary>
    /// The user activity log param.
    /// </summary>
    public class UserActivityLogParam
    {
        /// <summary>
        /// Gets or sets the user name.
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether is customer.
        /// </summary>
        public bool IsCustomer { get; set; }
        /// <summary>
        /// Gets or sets the user agent.
        /// </summary>
        public string UserAgent { get; set; }
        /// <summary>
        /// Gets or sets the remote ip address.
        /// </summary>
        public string RemoteIpAddress { get; set; }
        /// <summary>
        /// Gets or sets the http method.
        /// </summary>
        public string HttpMethod { get; set; }
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
        /// Gets or sets a value indicating whether form is data.
        /// </summary>
        public bool IsFormData { get; set; }
        /// <summary>
        /// Gets or sets the request body.
        /// </summary>
        public string RequestBody { get; set; }
        /// <summary>
        /// Gets or sets the headers.
        /// </summary>
        public string Headers { get; set; }
        /// <summary>
        /// Gets or sets the request url.
        /// </summary>
        public string RequestUrl { get; set; }
        /// <summary>
        /// Gets or sets the machine name.
        /// </summary>
        public string MachineName { get; set; }
        /// <summary>
        /// Gets or sets the environment.
        /// </summary>
        public string Environment { get; set; }
        /// <summary>
        /// Gets or sets the user action.
        /// </summary>
        public string UserAction { get; set; }
    }
}
