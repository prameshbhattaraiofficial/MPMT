namespace Mpmt.Core.Dtos.ConversionRate
{
    /// <summary>
    /// The i u d conversion rate.
    /// </summary>
    public class IUDConversionRate
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets the source currency.
        /// </summary>
        public string SourceCurrency { get; set; }
        /// <summary>
        /// Gets or sets the destination currency.
        /// </summary>
        public string DestinationCurrency { get; set; }
        /// <summary>
        /// Gets or sets the unit value.
        /// </summary>
        public int UnitValue { get; set; }
        /// <summary>
        /// Gets or sets the buying rate.
        /// </summary>
        public decimal BuyingRate { get; set; }
        /// <summary>
        /// Gets or sets the selling rate.
        /// </summary>
        public decimal SellingRate { get; set; }
        /// <summary>
        /// Gets or sets the current rate.
        /// </summary>
        public decimal CurrentRate { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether is active.
        /// </summary>
        public bool IsActive { get; set; }
        /// <summary>
        /// Gets or sets the logged in user.
        /// </summary>
        public string UserType { get; set; }
        /// <summary>
        /// Gets or sets the logged in user name.
        /// </summary>
        public string LoggedInUserName { get; set; }
        public bool IsSendNotificationEmail { get; set; }
    }
}