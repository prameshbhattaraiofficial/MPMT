using Microsoft.AspNetCore.Mvc;
using Mpmt.Core.Domain;
using Mpmt.Core.Models.Agents;
using Mpmt.Services.Partner;

namespace Mpmt.PublicApi.Controllers;

[ApiController]
public class AgentRegistrationController : BaseApiController
{
    private readonly IAgentRegistrationService _agentRegistrationService;

    public AgentRegistrationController(IAgentRegistrationService agentRegistrationService)
    {
        _agentRegistrationService = agentRegistrationService;
    }

    [HttpPost("become-agent")]
    public async Task<ActionResult<ApiResponse>> BecomeAgent(AgentRegistration request)
    {
        var result = await _agentRegistrationService.InsertAsync(request);
        return HandleResponseFromMpmtResult(result);
    }
}
