namespace Mpmt.Core.Dtos.Transaction
{
    public class RemitTransactionList
    {
        public int SN { get; set; }
        public string TransactionId { get; set; }
        public DateTime TransactionDate { get; set; }
        public string TransactionDateString { get; set; }
        public string SourceCurrency { get; set; }
        public string DestinationCurrency { get; set; }
        public string PaymentType { get; set; }
        public decimal Amount { get; set; }
        public string ServiceCharge { get; set; }
        public decimal SendAmount { get; set; }
        public decimal ReceiveAmount { get; set; }
        public decimal PartnerServiceCharge { get; set; }
        public string SenderDtl { get; set; }
        public string ReceiverDtl { get; set; }
        public string Status { get; set; }

    }
}
