namespace Mpmt.Core.Dtos.Partner
{
    /// <summary>
    /// The i u d update wallet currency.
    /// </summary>
    public class IUDUpdateWalletCurrency
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets the partner code.
        /// </summary>
        public string PartnerCode { get; set; }
        /// <summary>
        /// Gets or sets the source currency.
        /// </summary>
        public string SourceCurrency { get; set; }
        /// <summary>
        /// Gets or sets the destination currency.
        /// </summary>
        public string DestinationCurrency { get; set; }

        /// <<< HEAD<summary>
        /// Gets or sets the notification balance.
        /// </summary>
        public decimal NotificationBalance { get; set; }
        /// <summary>
        /// Gets or sets the markup min value.
        /// </summary>

        public decimal NotificationBalanceLimit { get; set; }
        public decimal MarkupMinValue { get; set; }
        /// <summary>
        /// Gets or sets the markup max value.
        /// </summary>
        public decimal MarkupMaxValue { get; set; }
        /// <summary>
        /// Gets or sets the type code.
        /// </summary>
        public string TypeCode { get; set; }
        /// <summary>
        /// Gets or sets the remarks.
        /// </summary>
        public string Remarks { get; set; }
        /// <summary>
        /// Gets or sets the operation mode.
        /// </summary>
        public string OperationMode { get; set; }
        /// <summary>
        /// Gets or sets the logged in user.
        /// </summary>
        public string LoggedInUser { get; set; }
        /// <summary>
        /// Gets or sets the user type.
        /// </summary>
        public string UserType { get; set; }
        public decimal CreditLimit { get; set; }
    }
}
