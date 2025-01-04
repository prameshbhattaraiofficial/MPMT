namespace Mpmt.Core.Dtos.PartnerApi
{
    public class TransactionStatusDetails
    {
        public string TransactionId { get; set; }
        public string PartnerTransactionId { get; set; }
        public string PaymentType { get; set; }
        public string PayoutStatus { get; set; }
        public string PayoutStatusCode { get; set; }
        public string ComplianceStatus { get; set; }
        public string ComplianceStatusCode { get; set; }
        public string ComplianceReleaseStatus { get; set; }
        public string DebitStatus { get; set; }
        public string DebitStatusCode { get; set; }
        public string TransactionStateRemarks { get; set; }
        public DateTime? TransactionDate { get; set; }
        public DateTime? PayoutDate { get; set; }
    }
}
