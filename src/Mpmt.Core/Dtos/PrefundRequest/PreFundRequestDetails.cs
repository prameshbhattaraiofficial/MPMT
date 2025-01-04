namespace Mpmt.Core.Dtos.PrefundRequest
{
    /// <summary>
    /// The pre fund request details.
    /// </summary>
    public class PreFundRequestDetails
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets the sn.
        /// </summary>
        public string Sn { get; set; }
        /// <summary>
        /// Gets or sets the partner code.
        /// </summary>
        public string PartnerCode { get; set; }
        public string OrganizationName { get; set; }
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
        public decimal Amount { get; set; }
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
        /// Gets or sets the request status code.
        /// </summary>
        public string RequestStatusCode { get; set; }
        public string RequestedUserType { get; set; }
        public string FundRequestedDate { get; set; }
        public string FundRequestedBy { get; set; }
        public string FundUpdatedDate {  get; set; }
        public string FundUpdatedBy { get; set; }
        public string UpdatedUserType { get; set; }
        /// <summary>
        /// Gets or sets the registered date.
        /// </summary>
        public string RegisteredDate { get; set; }
        /// <summary>
        /// Gets or sets the voucher img path.
        /// </summary>
        public string VoucherImgPath { get; set; }
        public string UpdatedDate { get; set; }
        public string ExchangeRate { get; set; }
       

    }
}
