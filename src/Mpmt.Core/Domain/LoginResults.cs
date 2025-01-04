namespace Mpmt.Core.Domain
{
    /// <summary>
    /// The login results.
    /// </summary>
    public enum LoginResults
    {
        /// <summary>
        /// Login successful
        /// </summary>
        Successful = 1,

        /// <summary>
        /// User not exist (email or username)
        /// </summary>
        NotExist = 2,

        /// <summary>
        /// Wrong password
        /// </summary>
        WrongPassword = 3,

        /// <summary>
        /// Account have not been activated
        /// </summary>
        NotActive = 4,

        /// <summary>
        /// User has been deleted 
        /// </summary>
        Deleted = 5,

        /// <summary>
        /// Locked out
        /// </summary>
        LockedOut = 6,

        /// <summary>
        /// User blocked
        /// </summary>
        Blocked = 7,

        /// <summary>
        /// Requires multi-factor authentication
        /// </summary>
        MultiFactorAuthenticationRequired = 8,

        InitiateFactorAuthentication = 9,
        PasswordExpired = 10

    }
}
