namespace Mpmt.Core.Dtos.ServiceCharge
{
    /// <summary>
    /// The service charge filter.
    /// </summary>
    public class ServiceChargeFilter
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
        /// Gets or sets the payment type.
        /// </summary>
        public string PaymentType { get; set; }
        /// <summary>
        /// Gets or sets the charge category.
        /// </summary>
        public string ChargeCategory { get; set; }
        /// <summary>
        /// Gets or sets the from date.
        /// </summary>
        public DateTime? FromDate { get; set; }
        /// <summary>
        /// Gets or sets the to date.
        /// </summary>
        public DateTime? ToDate { get; set; }
    }
}