namespace Mpmt.Core.Dtos.Currency
{
    /// <summary>
    /// The currency details.
    /// </summary>
    public class CurrencyDetails
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets the currency name.
        /// </summary>
        public string CurrencyName { get; set; }
        /// <summary>
        /// Gets or sets the short name.
        /// </summary>
        public string ShortName { get; set; }
        /// <summary>
        /// Gets or sets the symbol.
        /// </summary>
        public string Symbol { get; set; }
        /// <summary>
        /// Gets or sets the country code.
        /// </summary>
        public string CountryCode { get; set; }
        public string CurrencyImagePath { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether is active.
        /// </summary>
        public bool IsActive { get; set; }
        /// <summary>
        /// Gets or sets the created by.
        /// </summary>
        public string CreatedBy { get; set; }
        /// <summary>
        /// Gets or sets the updated by.
        /// </summary>
        public string UpdatedBy { get; set; }
    }
}
