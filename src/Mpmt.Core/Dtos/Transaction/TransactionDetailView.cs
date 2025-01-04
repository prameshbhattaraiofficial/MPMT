namespace Mpmt.Core.Dtos.Transaction
{
    public class TransactionDetailView
    {
        public string TransactionId
        {
            get; set;
        }
        public string TransactionDate { get; set; }
        public string PartnerCode
        {
            get; set;
        }
        public string PartnerName
        {
            get; set;
        }
        public string SourceCurrency
        {
            get; set;
        }
        public string DestinationCurrency
        {
            get; set;
        }
        public decimal SendingAmount
        {
            get; set;
        }
        public decimal ServiceCharge
        {
            get; set;
        }
        public decimal ConversionRate
        {
            get; set;
        }
        public decimal NetSendingAmount
        {
            get; set;
        }
        public decimal NetReceivingAmount
        {
            get; set;
        }
        public decimal CreditSendingAmount
        {
            get; set;
        }
        public bool SenderRegistered
        {
            get; set;
        }
        public string MemberId
        {
            get; set;
        }
        public bool RecipientRegistered
        {
            get; set;
        }
        public string RecipientId
        {
            get; set;
        }
        public string Sign
        {
            get; set;
        }
        public string TransactionType
        {
            get; set;
        }
        public string WalletType
        {
            get; set;
        }
        public string TokenNumber
        {
            get; set;
        }
        public string PaymentType
        {
            get; set;
        }
        public decimal PartnerServiceCharge
        {
            get; set;
        }
        public decimal PartnerCommission
        {
            get; set;
        }
        public decimal CurrentBalance
        {
            get; set;
        }
        public decimal PreviousBalance
        {
            get; set;
        }
        public int PaymentTypeId
        {
            get; set;
        }
        public string BankName
        {
            get; set;
        }
        public string BankCode
        {
            get; set;
        }
        public string Branch
        {
            get; set;
        }
        public string AccountHolderName
        {
            get; set;
        }
        public string AccountNumber
        {
            get; set;
        }
        public string WalletName
        {
            get; set;
        }
        public string WalletCode
        {
            get; set;
        }
        public string WalletNumber
        {
            get; set;
        }
        public string WalletHolderName
        {
            get; set;
        }
        public bool TransactionApproval
        {
            get; set;
        }
        public string PartnerTransactionId
        {
            get; set;
        }
        public string PartnerTrackerId
        {
            get; set;
        }
        public string PartnerStatus
        {
            get; set;
        }
        public string AgentCode
        {
            get; set;
        }
        public string AgentTransactionId
        {
            get; set;
        }
        public string AgentTrackerId
        {
            get; set;
        }
        public string AgentStatus
        {
            get; set;
        }
        public string GatewayTxnId
        {
            get; set;
        }
        public string ComplianceStatusCode
        {
            get; set;
        }
        public string ComplianceStatus
        {
            get; set;
        }
        public string StatusCode
        {
            get; set;
        }
        public string IpAddress
        {
            get; set;
        }
        public string DeviceId
        {
            get; set;
        }
        public string CreatedUserType
        {
            get; set;
        }
        public int CreatedById
        {
            get; set;
        }
        public string CreatedByName
        {
            get; set;
        }
        public string SenderCreatedDate
        {
            get; set;
        }
        public string ReceiverCreatedDate
        {
            get; set;
        }
        public string Remarks
        {
            get; set;
        }
        public string FeeCreditLimitOverflow { get; set; }

    }
}
