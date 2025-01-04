using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mpmt.Api.Features.AuthenticationSchemes.AgentApi;
using Mpmt.Core.Dtos.AgentApi;
using Mpmt.Core.Dtos.PartnerApi;
using Mpmt.Core.Models.Transaction;
using Mpmt.Services.Mvc.Filters;
using Mpmt.Services.Services.AgentApi;
using Mpmt.Services.Services.PartnerApi;
using Org.BouncyCastle.Asn1.Ocsp;

namespace Mpmt.Api.Controllers
{
    [Authorize(AuthenticationSchemes = AgentApiAuthenticationOptions.DefaultScheme)]
    public class MyPayApiController : BaseApiController
    {
        private readonly IAgentApiService _agentApiService;
        private readonly IPartnerApiService _partnerApiService;

        public MyPayApiController(IAgentApiService agentApiService,IPartnerApiService partnerApiService)
        {
            _agentApiService = agentApiService;
            _partnerApiService = partnerApiService;
        }


        [HttpPost("mypay-agent-wallet-payout-validation")]
        [LogRequestAgent]
        [LogResponseAgent]
        public async Task<IActionResult> MyPayWalletPayout(WalletPayoutApi request)
        {
           
            var (statusCode, response) = await _agentApiService.AgentmtcnValidateAsync(request);
            return HandleResponseFromStatusCode(statusCode, response);
        }

        [HttpPost("get-instrument-details")]
        [LogRequestAgent]
        [LogResponseAgent]
        public async Task<IActionResult> GetInstrumentDetail(InstrumentDetailRequest request)
        {
            var (statusCode, response) = await _agentApiService.GetInstrumentDetailForAgentWalletAsync(request);

            return HandleResponseFromStatusCode(statusCode, response);
        }

        //[HttpPost("get-instrument-details-list")]
        //[LogRequestAgent]
        //[LogResponseAgent]
        //public async Task<IActionResult> GetInstrumentDetailsList(GetInstrumentListsRequest request)
        //{
        //    var (statusCode, response) = await _partnerApiService.GetInstrumentListsAsync(request);

        //    return HandleResponseFromStatusCode(statusCode, response);
        //}


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
        public async Task<IActionResult> RequestPayout([FromForm] RequestPayoutForAgentWalletApi request)
        {
            var (statusCode, response) = await _agentApiService.RequestPayoutForAgentWalletAsync(request);

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
