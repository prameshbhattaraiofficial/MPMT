namespace Mpmt.Core.Dtos.Banks
{
    /// <summary>
    /// The bank filter.
    /// </summary>
    public class BankFilter
    {
        /// <summary>
        /// Gets or sets the bank name.
        /// </summary>
        public string BankName { get; set; }
        /// <summary>
        /// Gets or sets the country code.
        /// </summary>
        public string CountryCode { get; set; }
        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        public int Status { get; set; }
    }
}
