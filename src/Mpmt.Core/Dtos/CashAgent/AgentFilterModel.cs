using Mpmt.Core.Dtos.Paging;

namespace Mpmt.Core.Dtos.CashAgent;

public class AgentFilterModel : PagedRequest
{
    public string AgentCode { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string DistrictCode { get; set; }
    public string UserStatus { get; set; }
    public int Export { get; set; }
}
