namespace Mpmt.Core.Dtos.Users
{
    /// <summary>
    /// The user login activity.
    /// </summary>
    public class UserLoginActivity
    {
        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// Gets or sets the failed login attempt.
        /// </summary>
        public int FailedLoginAttempt { get; set; }
        /// <summary>
        /// Gets or sets the temporary locked till utc date.
        /// </summary>
        public DateTime? TemporaryLockedTillUtcDate { get; set; }
        /// <summary>
        /// Gets or sets the last ip address.
        /// </summary>
        public string LastIpAddress { get; set; }
        /// <summary>
        /// Gets or sets the device id.
        /// </summary>
        public string DeviceId { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether is active.
        /// </summary>
        public bool IsActive { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether is blocked.
        /// </summary>
        public bool IsBlocked { get; set; }
        /// <summary>
        /// Gets or sets the last login date utc.
        /// </summary>
        public DateTime? LastLoginDateUtc { get; set; }
        /// <summary>
        /// Gets or sets the last activity date utc.
        /// </summary>
        public DateTime? LastActivityDateUtc { get; set; }
    }
}
