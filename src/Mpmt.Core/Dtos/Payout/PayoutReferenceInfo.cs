namespace Mpmt.Core.Dtos.Payout
{
    public class PayoutReferenceInfo
    {
        public string PayoutReferenceNo { get; set; }
        public string RemitTransactionId { get; set; }
        public string PaymentType { get; set; }
        public decimal? PayableAmount { get; set; }
        public string WalletNumber { get; set; }
        public string WalletName { get; set; }
        public string WalletHolderName { get; set; }
        public string BankCode { get; set; }
        public string BankName { get; set; }
        public string BankAccountNumber { get; set; }
        public string BankAccountHolderName { get; set; }
    }
}
