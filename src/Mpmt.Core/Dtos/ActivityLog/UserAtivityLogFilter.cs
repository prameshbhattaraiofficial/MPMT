using Mpmt.Core.Dtos.Paging;

namespace Mpmt.Core.Dtos.ActivityLog
{
    /// <summary>
    /// The user ativity log filter.
    /// </summary>
    public class UserAtivityLogFilter : PagedRequest
    {
        /// <summary>
        /// Gets or sets the user type.
        /// </summary>
        public int UserType { get; set; }
        /// <summary>
        /// Gets or sets the user name.
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Gets or sets the user action.
        /// </summary>
        public string UserAction { get; set; }
        /// <summary>
        /// Gets or sets the from date.
        /// </summary>
        public DateTime? FromDate { get; set; }
        /// <summary>
        /// Gets or sets the to date.
        /// </summary>
        public DateTime? ToDate { get; set; }
        public int Export { get; set; }
    }
}
