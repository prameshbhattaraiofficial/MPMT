namespace Mpmt.Core.Dtos.KYCRemark
{
    /// <summary>
    /// The i u d kyc remark.
    /// </summary>
    public class IUDKycRemark
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets the remarks name.
        /// </summary>
        public string RemarksName { get; set; }
        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether is active.
        /// </summary>
        public bool IsActive { get; set; }
        /// <summary>
        /// Gets or sets the logged in user.
        /// </summary>
        public int LoggedInUser { get; set; }
        /// <summary>
        /// Gets or sets the logged in user name.
        /// </summary>
        public string LoggedInUserName { get; set; }
    }
}
