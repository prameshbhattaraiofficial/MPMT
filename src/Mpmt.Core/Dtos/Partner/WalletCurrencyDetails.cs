namespace Mpmt.Core.Dtos.Partner
{
    /// <summary>
    /// The wallet currency details.
    /// </summary>
    public class WalletCurrencyDetails
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
        /// <summary>
        /// Gets or sets the balance.
        /// </summary>
        public decimal Balance { get; set; }
        /// <summary>
        /// Gets or sets the notification balance limit.
        /// </summary>
        public decimal NotificationBalanceLimit { get; set; }
        /// <summary>
        /// Gets or sets the markup min value.
        /// </summary>
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
        /// Gets or sets a value indicating whether is active.
        /// </summary>
        public bool IsActive { get; set; }
        public decimal CreditLimit { get; set; }
    }

    public class WalletCurrencyBalance
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
        /// <summary>
        /// Gets or sets the balance.
        /// </summary>
        public decimal Balance { get; set; }
        public decimal CreditLimit { get; set; }
        public decimal CreditBalance { get; set; }
        public decimal Ledger { get; set; }
        /// <summary>
        /// Gets or sets the notification balance limit.
        /// </summary>
        public decimal NotificationBalanceLimit { get; set; }
        /// <summary>
        /// Gets or sets the markup min value.
        /// </summary>
        public decimal MarkupMinValue { get; set; }
        /// <summary>
        /// Gets or sets the markup max value.
        /// </summary>
        public decimal MarkupMaxValue { get; set; }
        /// <summary>
        /// Gets or sets the type code.
        /// </summary>
        public string TypeCode { get; set; }
        public string IsFavourite { get; set; }
        public string IsSourceCurrencyNPR { get; set; }
        /// <summary>
        /// Gets or sets the remarks.
        /// </summary>
        public string Remarks { get; set; }
        public string CurrencyImagePath { get; set; }   
        /// <summary>
        /// Gets or sets a value indicating whether is active.
        /// </summary>
        public bool IsActive { get; set; }
    }
}
