using Microsoft.AspNetCore.Mvc;
using Mpmt.Core.Dtos.AgentList;
using Mpmt.Services.Services.AgentListApi;

namespace Mpmt.PublicApi.Controllers;

[ApiController]
public class AgentListController : BaseApiController
{
    private readonly IAgentListService _agentListService;

    public AgentListController(IAgentListService agentListService)
    {
        _agentListService = agentListService;
    }

    [HttpPost("get-agent-list")]
    public async Task<IActionResult> GetAgentList(GetAgentListRequest request)
    {
        var (statusCode, response) = await _agentListService.GetAgentListAsync(request);
        return HandleResponseFromStatusCode(statusCode, response);
    }
}