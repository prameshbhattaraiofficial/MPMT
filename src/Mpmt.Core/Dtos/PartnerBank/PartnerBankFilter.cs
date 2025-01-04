namespace Mpmt.Core.Dtos.PartnerBank
{
    /// <summary>
    /// The partner bank filter.
    /// </summary>
    public class PartnerBankFilter
    {
        /// <summary>
        /// Gets or sets the partner code.
        /// </summary>
        public string PartnerCode { get; set; }
        /// <summary>
        /// Gets or sets the bank code.
        /// </summary>
        public string BankCode { get; set; }
        /// <summary>
        /// Gets or sets the account number.
        /// </summary>
        public string AccountNumber { get; set; }
        /// <summary>
        /// Gets or sets the account name.
        /// </summary>
        public string AccountName { get; set; }
        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        public int Status { get; set; }
    }
}
