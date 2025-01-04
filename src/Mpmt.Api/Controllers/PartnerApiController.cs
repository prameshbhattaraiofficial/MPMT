using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mpmt.Api.Features.AuthenticationSchemes.PartnerApi;
using Mpmt.Core.Dtos.PartnerApi;
using Mpmt.Services.Mvc.Filters;
using Mpmt.Services.Services.PartnerApi;

namespace Mpmt.Api.Controllers
{
    [Authorize(AuthenticationSchemes = PartnerApiAuthenticationOptions.DefaultScheme)]
    public class PartnerApiController : BaseApiController
    {
        private readonly IPartnerApiService _partnerApiService;

        public PartnerApiController(IPartnerApiService partnerApiService)
        {
            _partnerApiService = partnerApiService;
        }

        [HttpPost("get-instrument-list")]
        [LogRequest]
        [LogResponse]
        public async Task<IActionResult> GetInstrumentLists(GetInstrumentListsRequest request)
        {
            var (statusCode, response) = await _partnerApiService.GetInstrumentListsAsync(request);

            return HandleResponseFromStatusCode(statusCode, response);
        }

        [HttpPost("get-txncharge-details")]
        [LogRequest]
        [LogResponse]
        public async Task<IActionResult> GetTxnChargeDetails(GetTxnChargeDetailsRequest request)
        {
            var (statusCode, response) = await _partnerApiService.GetTxnChargeDetailsAsync(request);

            return HandleResponseFromStatusCode(statusCode, response);
        }

        [HttpPost("get-process-id")]
        [LogRequest]
        [LogResponse]
        public async Task<IActionResult> GetProcessId(GetProcessIdRequest request)
        {
            var (statusCode, response) = await _partnerApiService.GetTxnProcessIdAsync(request);

            return HandleResponseFromStatusCode(statusCode, response);
        }

        [HttpPost("push-transaction-detail")]
        [LogRequest]
        [LogResponse]
        public async Task<IActionResult> PushTransaction(PushTransactionRequest request)
        {
            var (statusCode, response) = await _partnerApiService.PushTransactionAsync(request);

            return HandleResponseFromStatusCode(statusCode, response);
        }

        [HttpPost("transaction-status")]
        [LogRequest]
        [LogResponse]
        public async Task<IActionResult> TransactionStatus(TransactionStatusRequest request)
        {
            var (statusCode, response) = await _partnerApiService.GetTransactionStatusAsync(request);

            return HandleResponseFromStatusCode(statusCode, response);
        }

        [HttpPost("validate-account")]
        [LogRequest]
        [LogResponse]
        public async Task<IActionResult> ValidateAccount(ValidateAccountRequest request)
        {
            var (statusCode, response) = await _partnerApiService.ValidateAccountAsync(request);

            return HandleResponseFromStatusCode(statusCode, response);
        }

        [HttpPost("push-transaction")]
        [LogRequest]
        [LogResponse]
        public async Task<IActionResult> PushTransactionDetails(PushTransactionRequestDetals request)
        {
            var (statusCode, response) = await _partnerApiService.PushTransactionDetailsAsync(request);

            return HandleResponseFromStatusCode(statusCode, response);
        }

    }
}
