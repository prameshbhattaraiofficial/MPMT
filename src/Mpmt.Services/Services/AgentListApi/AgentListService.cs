using Mpmt.Core.Dtos.AgentList;
using Mpmt.Core.Dtos.Paging;
using Mpmt.Data.Repositories.AgentList;
using System.Net;

namespace Mpmt.Services.Services.AgentListApi;

public class AgentListService : IAgentListService
{
    private readonly IAgentListRepository _agentListRepository;

    public AgentListService(IAgentListRepository agentListRepository)
    {
        _agentListRepository = agentListRepository;
    }

    public async Task<(HttpStatusCode, PagedList<AgentListDetail>)> GetAgentListAsync(GetAgentListRequest request)
    {
        var result = await _agentListRepository.GetAgentListAsync(request);
        return (HttpStatusCode.OK, result);
    }
}