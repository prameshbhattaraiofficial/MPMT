using Mpmt.Core.Dtos.Paging;

namespace Mpmt.Core.Dtos.SuperAgent;

public class AgentAccountStatement
{
    public string SN { get; set; }
    public string AgentCode { get; set; }
    public string AgentName { get; set; }
    public string AgentAddress { get; set; }
    public string District { get; set; }
    public string ReferenceId { get; set; }
    public string Type { get; set; }
    public string Particular { get; set; }
    public string Amount { get; set; }
    public string Sign { get; set; }
    public string PreviousPrefundBalance { get; set; }
    public string CurrentPrefundBalance { get; set; }
    public string PreviousSettlementBalance { get; set; }
    public string CurrentSettlementBalance { get; set; }
    public string Remarks { get; set; }
    public string UserType { get; set; }
    public string TxnBy { get; set; }
    public string NepalStandardDate { get; set; }
    public string NepaliDate { get; set; }
}

public class AgentStatementFilter : PagedRequest
{
    public string AgentCode { get; set; }
    public string StartDateBS { get; set; }
    public string EndDateBS { get; set; }
    public string DateFlag { get; set; }
    public string StartDate { get; set; }
    public string EndDate { get; set; }
    public string UserType { get; set; }
    public int Export { get; set; }
}
