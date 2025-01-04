using Mpmt.Core.Dtos.Paging;

namespace Mpmt.Core.Dtos.AdminUser
{
    /// <summary>
    /// The admin user filter.
    /// </summary>
    public class AdminUserFilter : PagedRequest
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
        /// Gets or sets the mobile number.
        /// </summary>
        public string MobileNumber { get; set; }
        /// <summary>
        /// Gets or sets the kyc status code.
        /// </summary>
        public string KycStatusCode { get; set; }
        /// <summary>
        /// Gets or sets the country code.
        /// </summary>
        public string CountryCode { get; set; }
        /// <summary>
        /// Gets or sets the user status.
        /// </summary>
        public int UserStatus { get; set; }
        /// <summary>
        /// Gets or sets the export.
        /// </summary>
        public int Export { get; set; } = 0;
    }
}
