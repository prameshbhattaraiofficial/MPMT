using Microsoft.AspNetCore.Mvc;
using Mpmt.Core.Domain;
using Mpmt.Services.Services.Common;

namespace Mpmt.PublicApi.Controllers;

public class DropdownController : BaseApiController
{
    private readonly ICommonddlServices _commonddlService;

    public DropdownController(ICommonddlServices commonddlService)
    {
        _commonddlService = commonddlService;
    }

    [HttpGet("get-district-list")]
    public async Task<IActionResult> GetDistrictList()
    {
        var districtList = await _commonddlService.GetAllDistrictddl();
        var response = new
        {
            ResponseStatus = ResponseStatuses.Success,
            ResponseCode = ResponseCodes.Code200_Success,
            ResponseMessage = "District list fetched successfully!",
            Data = districtList
        };

        return Ok(response);
    }
}
