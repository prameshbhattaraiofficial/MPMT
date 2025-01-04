namespace Mpmt.Core.Dtos.FundType
{
    /// <summary>
    /// The fund type details.
    /// </summary>
    public class FundTypeDetails
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets the fund type.
        /// </summary>
        public string FundType { get; set; }
        /// <summary>
        /// Gets or sets the fund type code.
        /// </summary>
        public string FundTypeCode { get; set; }
        /// <summary>
        /// Gets or sets the remarks.
        /// </summary>
        public string Remarks { get; set; }
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
