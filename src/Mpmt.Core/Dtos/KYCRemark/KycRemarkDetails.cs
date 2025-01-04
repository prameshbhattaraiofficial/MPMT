namespace Mpmt.Core.Dtos.KYCRemark
{
    /// <summary>
    /// The kyc remark details.
    /// </summary>
    public class KycRemarkDetails
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
        /// Gets or sets the created by name.
        /// </summary>
        public string CreatedByName { get; set; }
        /// <summary>
        /// Gets or sets the updated by name.
        /// </summary>
        public string UpdatedByName { get; set; }
    }
}
