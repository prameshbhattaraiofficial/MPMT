namespace Mpmt.Core.Dtos.AgentReport;

public class AgentCommissionTransactionReport
{
    public string SN { get; set; }
    public string CreatedDate { get; set; }
    public string CreatedNepaliDate { get; set; }
    public string TransactionId { get; set; }
    public string RemitTransactionId { get; set; }

    public string SenderName { get; set; }
    public string SenderContactNumber { get; set; }
    public string SenderCountry { get; set; }
    public string SenderAddress { get; set; }
    public string SenderEmail { get; set; }
    public string RecipientName { get; set; }
    public string RecipientContactNumber { get; set; }
    public string RecipientDistrict { get; set; }
    public string RecipientAddress { get; set; }
    public string AgentCode { get; set; }
    public string AgentName { get; set; }
    public string AgentDistrict { get; set; }
    public string OrganizationName { get; set; }
    public string PaymentType { get; set; }
    public string ReceivingAmountNPR { get; set; }
    public string Commission { get; set; }
    public string TransactionType { get; set; }

}

public class AgentCommissionTransactionReportDetail
{
    public int SN { get; set; }
    public string TransactionId { get; set; }
    public string ParentTransactionId { get; set; }
    public string AgentTransactionId { get; set; }
    public string AgentCode { get; set; }
    public string AgentName { get; set; }
    public string AgentOrganization { get; set; }
    public decimal AgentCommission { get; set; }
    public string SuperAgentCode { get; set; }
    public string SuperAgentName { get; set; }
    public string SuperAgentOrganization { get; set; }
    public decimal SuperAgentCommission { get; set; }
    public string PaymentType { get; set; }
    public string PayoutTransactionType { get; set; }
    public string SourceCurrency { get; set; }
    public string DestinationCurrency { get; set; }
    public decimal ReceivingAmountNPR { get; set; }
    public string SenderName { get; set; }
    public string SenderContactNumber { get; set; }
    public string SenderCountry { get; set; }
    public string SenderAddress { get; set; }
    public string SenderEmail { get; set; }
    public string RecipientName { get; set; }
    public string RecipientContactNumber { get; set; }
    public string RecipientCountry { get; set; }
    public string RecipientDistrict { get; set; }
    public string RecipientAddress { get; set; }
    public string Status { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime CreatedUtcDate { get; set; }
    public string CreatedNepaliDate { get; set; }
}
