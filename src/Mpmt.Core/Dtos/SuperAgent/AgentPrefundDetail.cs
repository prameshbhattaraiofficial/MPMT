namespace Mpmt.Core.Dtos.SuperAgent;

public class AgentPrefundDetail
{
    public string? AgentCode { get; set; }
    public string? UserType { get; set; }
    public string? SourceCurrency { get; set; }
    public decimal? PrefundBalance { get; set; }
    public decimal? SettlementBalance { get; set; }
    public decimal? CommissionBalance { get; set; }
    public decimal? NotificationBalance { get; set; }
}
