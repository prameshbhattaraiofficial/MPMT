using Mpmt.Core.Domain.Agents;
using Mpmt.Core.Dtos.AgentApplications;
using Mpmt.Core.Dtos.Paging;
using Mpmts.Core.Dtos;

namespace Mpmt.Data.Repositories.Agents;

public interface IAgentRegistrationRepository
{
    Task<SprocMessage> InsertAsync(AgentRegister request);
    Task<PagedList<AgentApplicationsModel>> GetAgentApplicationsAsync(AgentApplicationsFilter requestFilter);
}
