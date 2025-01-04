using Mpmt.Core.Dtos.AgentList;
using Mpmt.Core.Dtos.Paging;

namespace Mpmt.Data.Repositories.AgentList;

public interface IAgentListRepository
{
    Task<PagedList<AgentListDetail>> GetAgentListAsync(GetAgentListRequest request);
}