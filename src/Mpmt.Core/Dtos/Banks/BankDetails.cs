namespace Mpmt.Core.Dtos.Banks
{
    /// <summary>
    /// The bank details.
    /// </summary>
    public class BankDetails
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets the bank name.
        /// </summary>
        public string BankName { get; set; }
        /// <summary>
        /// Gets or sets the bank code.
        /// </summary>
        public string BankCode { get; set; }
        /// <summary>
        /// Gets or sets the branch code.
        /// </summary>
        public string BranchCode { get; set; }
        /// <summary>
        /// Gets or sets the country code.
        /// </summary>
        public string CountryCode { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether is active.
        /// </summary>
        public bool IsActive { get; set; }

    }
}
