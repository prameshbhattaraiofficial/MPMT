namespace Mpmt.Core.Dtos.ServiceCharge
{
    /// <summary>
    /// The service charge select.
    /// </summary>
    public class ServiceChargeSelect
    {
        /// <summary>
        /// Gets or sets the charge category id.
        /// </summary>
        public int ChargeCategoryId { get; set; }
        /// <summary>
        /// Gets or sets the source currency.
        /// </summary>
        public string SourceCurrency { get; set; }
        /// <summary>
        /// Gets or sets the destination currency.
        /// </summary>
        public string DestinationCurrency { get; set; }
        /// <summary>
        /// Gets or sets the payment type id.
        /// </summary>
        public int PaymentTypeId { get; set; }
    }
}
