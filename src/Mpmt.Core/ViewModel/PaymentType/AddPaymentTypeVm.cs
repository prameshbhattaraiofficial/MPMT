namespace Mpmt.Core.ViewModel.PaymentType
{
    /// <summary>
    /// The add payment type vm.
    /// </summary>
    public class AddPaymentTypeVm
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
        /// Gets or sets the description.
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether is active.
        /// </summary>
        public bool IsActive { get; set; }
    }
}
