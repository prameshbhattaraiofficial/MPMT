namespace Mpmt.Core.Dtos.PartnerReport;

public class RemitSettlementReport
{
    public string SN { get; set; }
    public DateTime TransactionDate { get; set; }
    public string PayoutDate { get; set; }
    public string SenderTransactionDate { get; set; }
    public string ReceiverTransactionDate { get; set; }
    public string SenderName { get; set; }  
    public string SenderContactNumber { get; set; }
    public string SenderEmail { get; set; }
    public string SenderAddress { get; set; }
    public string SenderZipCode { get; set; }
    public string SenderCountry { get; set; }
    public string ReceiverName { get; set; }
    public string RecipientAccountNumber { get; set; }
    public string RecipientBank { get; set; }
    public string RecipientCity { get; set; }
    public string RecipientAddress { get; set; }
    public string PaymentType { get; set; }
    public string Type { get; set; }
    public string TransactionId { get; set; }
    public string PartnerTrackerId { get; set; }
    public string AgentTrackerId { get; set; }
    public string GatewayTxnId { get; set; }
    public string PartnerId { get; set; }
    public string PartnerFullName { get; set; }
    public string PartnerCountry { get; set; }
    public string SourceCurrency { get; set; }
    public string DestinationCurrency { get; set; }
    public string PayoutType { get; set; }
    public decimal SendingAmount { get; set; }
    public decimal ConversionRate { get; set; }
    public decimal ServiceCharge { get; set; }
    public decimal ServiceChargeUSD { get; set; }
    public decimal NetSendingAmount { get; set; }
    public decimal CreditSendingAmount { get; set; }
    public decimal NetReceivingAmountNPR { get; set; }
    public decimal CommissionAmount { get; set; }
    public decimal PreviousBalance { get; set; }
    public decimal CurrentBalance { get; set; }
    public string Sign { get; set; }
    public string Status { get; set; }
    public string TransactionType { get; set; }
    public string AgentDistrict { get; set; }
    public string AgentName { get; set; }
    public string AgentCode { get; set; }
    public string AgentCommissionNPR { get; set; }
    public string SuperAgentName { get; set; }
    public string SuperAgentCode { get; set; }
    public string SuperAgentCommissionNPR { get; set; }

    public string AgentTransactionId { get; set; }
    public string SenderDtl { get; set; }
    public string ReceiverDtl { get; set; }
}


public class ExportRemitSettlementReport
{
    public string SenderTransactionDate { get; set; }
    public string ReceiverTransactionDate { get; set; }
    public string SenderName { get; set; }
    public string ReceiverName { get; set; }
    public string RecipientAccountNumber { get; set; }
    public string RecipientBank { get; set; }
    public string Type { get; set; }
    public string TransactionId { get; set; }
    public string AgentTransactionId { get; set; }
    public string PartnerTrackerId { get; set; }
    public string SourceCurrency { get; set; }
    public string PayoutType { get; set; }
    public decimal SendingAmount { get; set; }
    public decimal ConversionRate { get; set; }
    public decimal ServiceCharge { get; set; }
    public decimal CreditSendingAmount { get; set; }
    public decimal NetReceivingAmountNPR { get; set; }
    public decimal CommissionAmount { get; set; }
    public decimal PreviousBalance { get; set; }
    public decimal CurrentBalance { get; set; }
    public string TransactionType { get; set; }
    public string AgentDistrict { get; set; }
    public string AgentName { get; set; }
    public string AgentCode { get; set; }
    public string AgentCommissionNPR { get; set; }
    public string SuperAgentName { get; set; }
    public string SuperAgentCode { get; set; }
    public string SuperAgentCommissionNPR { get; set; }
}

public class AdminExportRemitSettlementReport
{
    public string SenderTransactionDate { get; set; }
    public string ReceiverTransactionDate { get; set; }
    public string PayoutDate { get; set; }
    public string SenderName { get; set; }
    public string ReceiverName { get; set; }
    public string RecipientAccountNumber { get; set; }
    public string RecipientBank { get; set; }
    public string Type { get; set; }
    public string TransactionId { get; set; }
    public string PartnerTrackerId { get; set; }
    public string AgentTrackerId { get; set; }
    public string GatewayTxnId { get; set; }
    public string PartnerId { get; set; }
    public string PartnerFullName { get; set; }
    public string PartnerCountry { get; set; }
    public string SourceCurrency { get; set; }
    public string PayoutType { get; set; }
    public decimal SendingAmount { get; set; }
    public decimal ConversionRate { get; set; }
    public decimal ServiceCharge { get; set; }
    public decimal CreditSendingAmount { get; set; }
    public decimal NetReceivingAmountNPR { get; set; }
    public decimal CommissionAmount { get; set; }
    public decimal PreviousBalance { get; set; }
    public decimal CurrentBalance { get; set; }
    public string Status { get; set; }
    public string TransactionType { get; set; }
    public string AgentDistrict { get; set; }
    public string AgentName { get; set; }
    public string AgentCode { get; set; }
    public string AgentCommissionNPR { get; set; }
    public string SuperAgentName { get; set; }
    public string SuperAgentCode { get; set; }
    public string SuperAgentCommissionNPR { get; set; }
}
