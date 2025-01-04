namespace Mpmt.Core.Dtos.PartnerReport;

public class RemitTxnReport
{
    public int SN { get; set; }
    public DateTime TransactionDate { get; set; }
    public string PayoutDate { get; set; }    
    public string TransactionDateString { get; set; }
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
    public bool IsPayout { get; set; }
    public bool IsCancel { get; set; }
    public bool IsMarkSuccess { get; set; }
    public string AgentTrackerId { get; set; }
    public string GatewayTxnId { get; set; }
    public string TransactionId { get; set; }
    public string PartnerTrackerId { get; set; }
    public string PartnerId { get; set; }
    public string PartnerFullName { get; set; }
    public string SourceCurrency { get; set; }  
    public string DestinationCurrency { get; set; }
    public string PaymentType { get; set; }
    public string PaymentTypeCode { get; set; }
    public decimal Amount { get; set; }
    public decimal ServiceCharge { get; set; }
    public decimal SendAmount { get; set; }
    public decimal ReceiveAmount { get; set; }
    public decimal PartnerServiceCharge { get; set; }
    public string Status { get; set; }
    public string StatusCode { get; set; }
    public string ComplianceReleased { get; set; }
    public string SenderUserStatus { get; set; }
    public string ReceiverUserStatus { get; set; }
    public string Remarks { get; set; }
    public string CancelledUserType { get; set; }
    public string CancelledBy { get; set; }
    public string CancelledDate { get; set; }

    public string SenderDtl { get; set; }
    public string ReceiverDtl { get; set; }
    public string AgentTransactionId { get; set; }
    public string ComplianceTransaction { get; set; }
    public string ControlNumber { get; set; }
    public bool isCancel { get; set; }
}

public class ExportRemitTxnReport
{
    public string SenderTransactionDate { get; set; }
    public string ReceiverTransactionDate { get; set; }
    public string SenderName { get; set; }
    public string SenderCountry { get; set; }
    public string ReceiverName { get; set; }
    public string RecipientAccountNumber { get; set; }
    public string RecipientBank { get; set; }
    public string TransactionId { get; set; }
    public string PartnerTrackerId { get; set; }
    public string GatewayTxnId { get; set; }
    public string SourceCurrency { get; set; }
    public string PaymentType { get; set; }
    public decimal Amount { get; set; }
    public decimal ServiceCharge { get; set; }
    public decimal SendAmount { get; set; }
    public decimal ReceiveAmount { get; set; }
    public string SenderUserStatus { get; set; }
    public string ReceiverUserStatus { get; set; }
    public string Remarks { get; set; }
    public string ComplianceTransaction { get; set; }
    public string ComplianceReleased { get; set; }
    public string ControlNumber { get; set; }
    public string CancelledUserType { get; set; }
    public string CancelledBy { get; set; }
    public string CancelledDate { get; set; }
}

public class AdminExportRemitTxnReport
{
    public string SenderTransactionDate { get; set; }
    public string ReceiverTransactionDate { get; set; }
    public string PayoutDate { get; set; }
    public string SenderName { get; set; }
    public string SenderCountry { get; set; }
    public string ReceiverName { get; set; }
    public string RecipientAccountNumber { get; set; }
    public string RecipientBank { get; set; }
    public string TransactionId { get; set; }
    public string AgentTrackerId { get; set; }
    public string GatewayTxnId { get; set; }
    public string PartnerTrackerId { get; set; }
    public string PartnerId { get; set; }
    public string PartnerFullName { get; set; }
    public string SourceCurrency { get; set; }
    public string PaymentType { get; set; }
    public decimal Amount { get; set; }
    public decimal ServiceCharge { get; set; }
    public decimal SendAmount { get; set; }
    public decimal ReceiveAmount { get; set; }
    public string SenderUserStatus { get; set; }
    public string ReceiverUserStatus { get; set; }
    public string Remarks { get; set; }
    public string ComplianceTransaction { get; set; }
    public string ComplianceReleased { get; set; }
    public string ControlNumber { get; set; }
    public string CancelledUserType { get; set; }
    public string CancelledBy { get; set; }
    public string CancelledDate { get; set; }
}