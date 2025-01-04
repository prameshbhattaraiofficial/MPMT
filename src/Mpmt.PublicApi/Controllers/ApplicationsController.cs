using Microsoft.AspNetCore.Mvc;
using Mpmt.Core.Domain;
using Mpmt.Core.Models.Partners.Applications;
using Mpmt.Services.Partner;
using Mpmt.Services.Services.Common;

namespace Mpmt.PublicApi.Controllers
{
    [ApiController]
    public class ApplicationsController : BaseApiController
    {
        private readonly ICommonddlServices _commonddlService;
        private readonly IPartnerApplicationService _partnerApplicationService;

        public ApplicationsController(
            ICommonddlServices commonddlService,
            IPartnerApplicationService partnerApplicationService)
        {
            _commonddlService = commonddlService;
            _partnerApplicationService = partnerApplicationService;
        }

        [HttpGet("get-country-list")]
        public async Task<IActionResult> GetCountryList()
        {
            var countryList = await _commonddlService.GetCountryddl();
            var response = new
            {
                ResponseStatus = ResponseStatuses.Success,
                ResponseCode = ResponseCodes.Code200_Success,
                ResponseMessage = "Country list fetched successfully!",
                Data = countryList
            };

            return Ok(response);
        }

        [HttpPost("become-partner")]
        public async Task<ActionResult<ApiResponse>> BecomePartner(PartnerApplicationRequest request)
        {
            var result = await _partnerApplicationService.InsertAsync(request);

            return HandleResponseFromMpmtResult(result);
        }
    }
}
