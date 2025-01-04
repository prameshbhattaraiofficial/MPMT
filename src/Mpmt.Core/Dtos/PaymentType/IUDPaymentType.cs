namespace Mpmt.Core.Dtos.PaymentType
{
    /// <summary>
    /// The i u d payment type.
    /// </summary>
    public class IUDPaymentType
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }
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
        /// <summary>
        /// Gets or sets the logged in user.
        /// </summary>
        public int LoggedInUser { get; set; }
        /// <summary>
        /// Gets or sets the logged in user name.
        /// </summary>
        public string LoggedInUserName { get; set; }
    }
}
