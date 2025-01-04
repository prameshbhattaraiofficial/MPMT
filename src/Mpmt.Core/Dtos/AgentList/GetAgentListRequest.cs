using Mpmt.Core.Dtos.Paging;

namespace Mpmt.Core.Dtos.AgentList;

public class GetAgentListRequest : PagedRequest
{
    public string AgentName { get; set; }
    public string DistrictCode { get; set; }
    public int Export { get; set; }
}