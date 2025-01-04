namespace Mpmt.Core.Domain.Partners.SendTransactions
{
    public class AddTransactionDetailsDto
    {
        public string TransactionId { get; set; }
        public string ReferenceTokenNo { get; set; }
        public string PaymentType { get; set; }
        public bool? TransactionApprovalRequired { get; set; }
        public string WalletNumber { get; set; }
        public string WalletName { get; set; }
        public string WalletHolderName { get; set; }
        public string BankCode { get; set; }
        public string BankName { get; set; }
        public string BankAccountNumber { get; set; }
        public string BankAccountHolderName { get; set; }
        public decimal? SendingAmount { get; set; }
        public decimal? ServiceCharge { get; set; }
        public decimal? NetSendingAmount { get; set; }
        public decimal? ConversionRate { get; set; }
        public decimal? PayableAmount { get; set; }
        public string SenderEmail { get; set; }
        public string PartnerEmail { get; set; }
        public bool? SendWalletNotificationEmail { get; set; }
        public decimal? RemainingWalletBal { get; set; }
        public string WalletCurrency { get; set; }
        public string TransactionDate { get; set; }
        public bool? SendFeeNotificationEmail { get; set; }
        public decimal? RemainingFeeBal { get; set; }
        public bool? FeeCreditLimitOverFlow { get; set; }
        public bool ComplianceFlag { get; set; }
    }
}
