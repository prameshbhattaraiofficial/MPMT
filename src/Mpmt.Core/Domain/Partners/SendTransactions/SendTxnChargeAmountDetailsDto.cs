namespace Mpmt.Core.Domain.Partners.SendTransactions
{
    public class SendTxnChargeAmountDetailsDto
    {
        public string SourceCurrency { get; set; }
        public string DestinationCurrency { get; set; }
        public string SendingAmount { get; set; }
        public string ConversionRate { get; set; }
        public string NetSendingAmount { get; set; }
        public string ReceivingAmountNPR { get; set; }
        public string ServiceCharge { get; set; }
        public string Commission { get; set; }
        public string PartnerServiceCharge { get; set; }
        public string CashPayoutSendTxnLimitNPR { get; set; }
        public string WalletSendTxnLimitNPR { get; set; }
        public string BankSendTxnLimitNPR { get; set; }
    }
}
