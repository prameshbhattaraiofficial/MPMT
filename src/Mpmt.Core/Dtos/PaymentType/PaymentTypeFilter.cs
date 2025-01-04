namespace Mpmt.Core.Dtos.PaymentType
{
    /// <summary>
    /// The payment type filter.
    /// </summary>
    public class PaymentTypeFilter
    {
        /// <summary>
        /// Gets or sets the payment type name.
        /// </summary>
        public string PaymentTypeName { get; set; }
        /// <summary>
        /// Gets or sets the payment type code.
        /// </summary>
        public string PaymentTypeCode { get; set; }
        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        public int Status { get; set; }
    }
}
