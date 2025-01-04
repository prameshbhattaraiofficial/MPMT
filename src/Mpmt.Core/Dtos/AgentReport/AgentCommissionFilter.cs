using Mpmt.Core.Dtos.Paging;

namespace Mpmt.Core.Dtos.AgentReport;

public class AgentCommissionFilter : PagedRequest
{
    public string AgentCode { get; set; }
    public string StartDate { get; set; }
    public string EndDate { get; set; }
    public string DistrictCode { get; set; }
    public string TransactionId { get; set; }
    public string AgentOrganizationName { get; set; }
    public string RecipientContactNumber { get; set; }
    public string TransactionType { get; set; }
    public string UserType { get; set; }
    public string LoggedInUser { get; set; }
    public int Export { get; set; }
}
