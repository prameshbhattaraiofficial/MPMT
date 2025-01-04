namespace Mpmt.Core.Dtos.Partner
{
    /// <summary>
    /// The wallet currency.
    /// </summary>
    public class WalletCurrency
    {
        /// <summary>
        /// Gets or sets the apppartner.
        /// </summary>
        public AppPartner apppartner { get; set; }
        /// <summary>
        /// Gets or sets the partner wallet currency.
        /// </summary>
        public IEnumerable<WalletCurrencyBalance> PartnerWalletCurrency { get; set; }
    }
}
