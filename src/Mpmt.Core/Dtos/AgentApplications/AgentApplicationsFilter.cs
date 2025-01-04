using Mpmt.Core.Dtos.Paging;

namespace Mpmt.Core.Dtos.AgentApplications;

public class AgentApplicationsFilter : PagedRequest
{
    public string FullName { get; set; }
    public int Export { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}
