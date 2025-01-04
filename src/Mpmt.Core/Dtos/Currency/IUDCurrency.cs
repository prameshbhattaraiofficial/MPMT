namespace Mpmt.Core.Dtos.Currency
{
    /// <summary>
    /// The i u d currency.
    /// </summary>
    public class IUDCurrency
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
        public string CurrencyImgPath { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether is active.
        /// </summary>
        public bool IsActive { get; set; }
        /// <summary>
        /// Gets or sets the logged in user.
        /// </summary>
        public string LoggedInUser { get; set; }
        /// <summary>
        /// Gets or sets the logged in user name.
        /// </summary>
        public string LoggedInUserName { get; set; }
    }
}
