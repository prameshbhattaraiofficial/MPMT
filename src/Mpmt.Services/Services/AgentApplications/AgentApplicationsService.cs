using Mpmt.Core.Dtos.AgentApplications;
using Mpmt.Core.Dtos.Paging;
using Mpmt.Data.Repositories.Agents;

namespace Mpmt.Services.Services.AgentApplications;

public class AgentApplicationsService : IAgentApplicationsService
{
    private readonly IAgentRegistrationRepository _applicationsRepo;

    public AgentApplicationsService(IAgentRegistrationRepository applicationsRepo)
    {
        _applicationsRepo = applicationsRepo;
    }

    public async Task<PagedList<AgentApplicationsModel>> GetAgentApplicationsAsync(AgentApplicationsFilter requestFilter)
    {
        var data = await _applicationsRepo.GetAgentApplicationsAsync(requestFilter);
        return data;
    }
}
