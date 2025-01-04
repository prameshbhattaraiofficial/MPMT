namespace Mpmt.Core.Dtos.CashAgent;

public class AgentUnsettledAmount
{
    public string District { get; set; }
    public string AgentCode { get; set; }
    public string Agent { get; set; }
    public string ContactNumber { get; set; }
    public decimal CommissionAmount { get; set; }
    public decimal ReceivableAmount { get; set; }
    public decimal TotalAmount { get; set; }
}
