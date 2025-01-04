namespace Mpmt.Core.Dtos.ActivityLog
{

    /// <summary>
    /// The user activity log details.
    /// </summary>
    public class UserActivityLogDetails
    {
        public string  SN { get; set; }
        /// <summary>
        /// Gets or sets the activity date.
        /// </summary>
        public string ActivityDate { get; set; }
        /// <summary>
        /// Gets or sets the user type.
        /// </summary>
        public string UserType { get; set; }
        /// <summary>
        /// Gets or sets the user name.
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Gets or sets the i p address.
        /// </summary>
        public string IPAddress { get; set; }
        /// <summary>
        /// Gets or sets the path.
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// Gets or sets the user agent.
        /// </summary>
        public string UserAgent { get; set; }
        /// <summary>
        /// Gets or sets the action performed.
        /// </summary>
        public string ActionPerformed { get; set; }
    }
}
