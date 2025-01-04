namespace Mpmt.Core
{
    /// <summary>
    /// The mpmt user defaults.
    /// </summary>
    public static partial class MpmtUserDefaults
    {
        /// <summary>
        /// Gets a system name of 'sysadmin' role
        /// </summary>
        public static string SystemAdminRoleName = "SuperAdmin";
        public static string SystemPartnerRoleName = "PARTNER";

        /// <summary>
        /// Gets a system name of 'system' user object
        /// </summary>
        public static string SystemUserName = "system";

        /// <summary>
        /// Gets a system name of 'admin' user object
        /// </summary>
        public static string AdminUserName = "admin";

        /// <summary>
        /// Gets a maximum login failures to lockout account. Set 0 to disable this feature
        /// </summary>
        public static int UserMaxFailedAllowedLoginAttempts = 5;

        /// <summary>
        /// Gets a number of minutes to lockout users (for login failures).
        /// </summary>
        public static int FailedPasswordLockoutMinutes = 10;

        public static int PasswordChangeExpiryDayLength = 7;
        public static int PasswordChangeExpiryLimit = 30;
    }   
}
