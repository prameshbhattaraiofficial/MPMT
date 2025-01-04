namespace Mpmt.Core.Dtos.AdminUser
{
    /// <summary>
    /// The admin user details.
    /// </summary>
    public class AdminUserDetails
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets the s n.
        /// </summary>
        public int SN { get; set; }
        /// <summary>
        /// Gets or sets the full name.
        /// </summary>
        public string FullName { get; set; }
        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        public string Email { get; set; }
        public Guid UserGuid { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether email confirmed.
        /// </summary>
        public bool EmailConfirmed { get; set; }
        /// <summary>
        /// Gets or sets the gender.
        /// </summary>
        public string Gender { get; set; }
        /// <summary>
        /// Gets or sets the address.
        /// </summary>
        public string Address { get; set; }
        public string ProfileImageUrlPath { get; set; } 
        /// <summary>
        /// Gets or sets the date of birth.
        /// </summary>
        public DateTime? DateOfBirth { get; set; }
        /// <summary>
        /// Gets or sets the mobile number.
        /// </summary>
        public string MobileNumber { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether mobile confirmed.
        /// </summary>
        public bool MobileConfirmed { get; set; }
        /// <summary>
        /// Gets or sets the registered date.
        /// </summary>
        public DateTime? RegisteredDate { get; set; }
        /// <summary>
        /// Gets or sets the failed login attempt.
        /// </summary>
        public int failedLoginAttempt { get; set; }
        /// <summary>
        /// Gets or sets the temporary locked till date.
        /// </summary>
        public DateTime? TemporaryLockedTillDate { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether is active.
        /// </summary>
        public bool IsActive { get; set; }
        public string Role { get; set; }
    }
}
