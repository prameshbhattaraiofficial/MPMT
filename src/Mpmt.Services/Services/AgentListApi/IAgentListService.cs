using Mpmt.Core.Dtos.AgentList;
using Mpmt.Core.Dtos.Paging;
using System.Net;

namespace Mpmt.Services.Services.AgentListApi;

public interface IAgentListService
{
    Task<(HttpStatusCode, PagedList<AgentListDetail>)> GetAgentListAsync(GetAgentListRequest request);
}