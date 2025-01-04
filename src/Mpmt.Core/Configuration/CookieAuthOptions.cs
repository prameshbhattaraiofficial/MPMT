namespace Mpmt.Core.Configuration
{
    /// <summary>
    /// The cookie auth options.
    /// </summary>
    public class CookieAuthOptions
    {
        /// <summary>
        /// The section name.
        /// </summary>
        public const string SectionName = "Authentication:Cookie";
        /// <summary>
        /// Gets or sets the expiration days.
        /// </summary>
        public int ExpirationDays { get; set; }
        /// <summary>
        /// Gets or sets the expiration hours.
        /// </summary>
        public int ExpirationHours { get; set; }
        /// <summary>
        /// Gets or sets the expiration minutes.
        /// </summary>
        public int ExpirationMinutes { get; set; }
        /// <summary>
        /// Gets or sets the expiration seconds.
        /// </summary>
        public int ExpirationSeconds { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether is persistent.
        /// </summary>
        public bool IsPersistent { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether sliding expiration.
        /// </summary>
        public bool SlidingExpiration { get; set; }
    }
}
