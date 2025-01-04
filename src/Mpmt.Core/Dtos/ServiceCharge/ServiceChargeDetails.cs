namespace Mpmt.Core.Dtos.ServiceCharge
{
    /// <summary>
    /// The service charge details.
    /// </summary>
    public class ServiceChargeDetails
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets the sn.
        /// </summary>
        public int Sn { get; set; }
        /// <summary>
        /// Gets or sets the charge category id.
        /// </summary>
        public int ChargeCategoryId { get; set; }
        /// <summary>
        /// Gets or sets the payment type id.
        /// </summary>
        public int PaymentTypeId { get; set; }
        /// <summary>
        /// Gets or sets the service category.
        /// </summary>
        public string ServiceCategory { get; set; }
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
        /// Gets or sets a value indicating whether is active.
        /// </summary>
        public bool IsActive { get; set; }
        /// <summary>
        /// Gets or sets the registered date.
        /// </summary>
        public DateTime RegisteredDate { get; set; }
        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        public DateTime CreatedDate { get; set; }
    }
}
