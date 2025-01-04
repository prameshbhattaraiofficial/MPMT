namespace Mpmt.Core.ViewModel.Employee
{
    /// <summary>
    /// The add employee.
    /// </summary>
    public class AddEmployee
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets the user name.
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// Gets or sets the full name.
        /// </summary>
        public string FullName { get; set; }
        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether email confirmed.
        /// </summary>
        public bool EmailConfirmed { get; set; }
        /// <summary>
        /// Gets or sets the mobile number.
        /// </summary>
        public string MobileNumber { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether mobile confirmed.
        /// </summary>
        public bool MobileConfirmed { get; set; }
        /// <summary>
        /// Gets or sets the gender.
        /// </summary>
        public string Gender { get; set; }
        /// <summary>
        /// Gets or sets the address.
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// Gets or sets the department.
        /// </summary>
        public string Department { get; set; }
        /// <summary>
        /// Gets or sets the date of birth.
        /// </summary>
        public DateTime DateOfBirth { get; set; }
        /// <summary>
        /// Gets or sets the date of joining.
        /// </summary>
        public DateTime DateOfJoining { get; set; }
        /// <summary>
        /// Gets or sets the password hash.
        /// </summary>
        public string PasswordHash { get; set; }
        /// <summary>
        /// Gets or sets the password salt.
        /// </summary>
        public string PasswordSalt { get; set; }
        /// <summary>
        /// Gets or sets the access code hash.
        /// </summary>
        public string AccessCodeHash { get; set; }
        /// <summary>
        /// Gets or sets the access code salt.
        /// </summary>
        public string AccessCodeSalt { get; set; }
        /// <summary>
        /// Gets or sets the failed login attempt.
        /// </summary>
        public int FailedLoginAttempt { get; set; }
        /// <summary>
        /// Gets or sets the temporary locked till utc date.
        /// </summary>
        public DateTime TemporaryLockedTillUtcDate { get; set; }

        /// <summary>
        /// Gets or sets the profile image url path.
        /// </summary>
        public string ProfileImageUrlPath { get; set; }
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
        /// Gets or sets a value indicating whether is deleted.
        /// </summary>
        public bool IsDeleted { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether is blocked.
        /// </summary>
        public bool IsBlocked { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether is2 f a authenticated.
        /// </summary>
        public bool Is2FAAuthenticated { get; set; }
        /// <summary>
        /// Gets or sets the account secret key.
        /// </summary>
        public string AccountSecretKey { get; set; }
        /// <summary>
        /// Gets or sets the last login date utc.
        /// </summary>
        public DateTime LastLoginDateUtc { get; set; }
        /// <summary>
        /// Gets or sets the last activity date utc.
        /// </summary>
        public DateTime LastActivityDateUtc { get; set; }

        /// <summary>
        /// Gets or sets the created by.
        /// </summary>
        public string CreatedBy { get; set; }
        /// <summary>
        /// Gets or sets the created local date.
        /// </summary>
        public DateTime CreatedLocalDate { get; set; }
        /// <summary>
        /// Gets or sets the created utc date.
        /// </summary>
        public DateTime CreatedUtcDate { get; set; }
        /// <summary>
        /// Gets or sets the created nepali date.
        /// </summary>
        public string CreatedNepaliDate { get; set; }
        /// <summary>
        /// Gets or sets the updated by.
        /// </summary>
        public string UpdatedBy { get; set; }
        /// <summary>
        /// Gets or sets the updated local date.
        /// </summary>
        public DateTime UpdatedLocalDate { get; set; }
        /// <summary>
        /// Gets or sets the updated utc date.
        /// </summary>
        public DateTime UpdatedUtcDate { get; set; }
        /// <summary>
        /// Gets or sets the updated nepali date.
        /// </summary>
        public string UpdatedNepaliDate { get; set; }
        /// <summary>
        /// Gets or sets the role id.
        /// </summary>
        public int RoleId { get; set; }
    }
}
