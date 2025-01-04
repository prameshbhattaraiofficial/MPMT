using Mpmt.Core.Dtos.AgentReport;

namespace Mpmt.Core.Dtos.Agent;

public class AgentDashboard
{
    public string TotalUser { get; set; }
    public string TodayUser { get; set; }
    public string WeekUser { get; set; }
    public string MonthUser { get; set; }
    public string TotalPayout{ get; set; }
    public string TodayPayout{ get; set; }
    public string WeekPayout{ get; set; }
    public string MonthPayout { get; set; }
    public string TotalCommissionTransaction { get; set; }
    public string TodayCommissionTransaction { get; set; }
    public string WeekCommissionTransaction { get; set; }
    public string MonthCommissionTransaction { get; set; }
    public IEnumerable<AgentSettlementReport> SettlementReport { get; set; }
}
