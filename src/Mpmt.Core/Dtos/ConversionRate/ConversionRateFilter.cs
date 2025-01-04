namespace Mpmt.Core.Dtos.ConversionRate
{
    /// <summary>
    /// The conversion rate filter.
    /// </summary>
    public class ConversionRateFilter
    {
        /// <summary>
        /// Gets or sets the source currency.
        /// </summary>
        public string SourceCurrency { get; set; }
        /// <summary>
        /// Gets or sets the destination currency.
        /// </summary>
        public string DestinationCurrency { get; set; }
        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        public int Status { get; set; }
    }
}
