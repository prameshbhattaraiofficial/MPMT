using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mpmt.Api.Features.AuthenticationSchemes.AgentApi;
using Mpmt.Core.Dtos.AgentApi;
using Mpmt.Services.Mvc.Filters;
using Mpmt.Services.Services.AgentApi;

namespace Mpmt.Api.Controllers
{
    [Authorize(AuthenticationSchemes = AgentApiAuthenticationOptions.DefaultScheme)]
    public class AgentApiController : BaseApiController
    {
        private readonly IAgentApiService _agentApiService;

        public AgentApiController(IAgentApiService agentApiService)
        {
            _agentApiService = agentApiService;
        }

        [HttpPost("get-instrument-details")]
        [LogRequestAgent]
        [LogResponseAgent]
        public async Task<IActionResult> GetInstrumentDetail(InstrumentDetailRequest request)
        {
            var (statusCode, response) = await _agentApiService.GetInstrumentDetailAsync(request);

            return HandleResponseFromStatusCode(statusCode, response);
        }

        [HttpPost("get-process-id")]
        [LogRequestAgent]
        [LogResponseAgent]
        public async Task<IActionResult> GetProcessId(GetProcessIdRequestAgentApi request)
        {
            var (statusCode, response) = await _agentApiService.GetTxnProcessIdAsync(request);

            return HandleResponseFromStatusCode(statusCode, response);
        }

        [HttpPost("request-payout")]
        [LogRequestAgent]
        [LogResponseAgent]
        public async Task<IActionResult> RequestPayout([FromForm] RequestPayoutApi request)
        {
            var (statusCode, response) = await _agentApiService.RequestPayoutAsync(request);

            return HandleResponseFromStatusCode(statusCode, response);
        }

        [HttpPost("check-payout-status")]
        [LogRequestAgent]
        [LogResponseAgent]
        public async Task<IActionResult> CheckPayoutStatus(CheckPayoutStatusRequest request)
        {
            var (statusCode, response) = await _agentApiService.CheckPayoutStatusAsync(request);

            return HandleResponseFromStatusCode(statusCode, response);
        }
    }
}


 