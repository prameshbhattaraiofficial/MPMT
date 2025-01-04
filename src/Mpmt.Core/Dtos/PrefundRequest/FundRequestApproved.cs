namespace Mpmt.Core.Dtos.PrefundRequest
{
    /// <summary>
    /// The fund request approved view.
    /// </summary>
    public class FundRequestApprovedView
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// Gets or sets the partner code.
        /// </summary>
        public string PartnerCode { get; set; }
        /// <summary>
        /// Gets or sets the fund type.
        /// </summary>
        public string FundType { get; set; }
        /// <summary>
        /// Gets or sets the source currency.
        /// </summary>
        public string SourceCurrency { get; set; }
        /// <summary>
        /// Gets or sets the destination currency.
        /// </summary>
        public string DestinationCurrency { get; set; }
        /// <summary>
        /// Gets or sets the amount.
        /// </summary>
        public string Amount { get; set; }
        /// <summary>
        /// Gets or sets the remarks.
        /// </summary>
        public string Remarks { get; set; }
        /// <summary>
        /// Gets or sets the sign.
        /// </summary>
        public string Sign { get; set; }
        /// <summary>
        /// Gets or sets the transaction id.
        /// </summary>
        public string TransactionId { get; set; }
        /// <summary>
        /// Gets or sets the request status.
        /// </summary>
        public string RequestStatus { get; set; }
        /// <summary>
        /// Gets or sets the voucher img path.
        /// </summary>
        public string VoucherImgPath { get; set; }
        /// <summary>
        /// Gets or sets the created by name.
        /// </summary>
        public string CreatedByName { get; set; }
        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        public string CreatedDate { get; set; }
        /// <summary>
        /// Gets or sets the updated date.
        /// </summary>
        public string UpdatedDate { get; set; }
    }
}
