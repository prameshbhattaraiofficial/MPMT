using Mpmt.Core.Dtos.Paging;

namespace Mpmt.Core.Dtos.SuperAgent;

public class AgentLedgerFilter : PagedRequest
{
    public string AgentCode { get; set; }
    public string StartDate { get; set; }
    public string EndDate { get; set; }
    public int Export { get; set; }
}
