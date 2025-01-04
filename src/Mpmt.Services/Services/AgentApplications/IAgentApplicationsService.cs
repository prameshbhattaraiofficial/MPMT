using Mpmt.Core.Dtos.AgentApplications;
using Mpmt.Core.Dtos.Paging;

namespace Mpmt.Services.Services.AgentApplications;

public interface IAgentApplicationsService
{
    Task<PagedList<AgentApplicationsModel>> GetAgentApplicationsAsync(AgentApplicationsFilter requestFilter);
}
