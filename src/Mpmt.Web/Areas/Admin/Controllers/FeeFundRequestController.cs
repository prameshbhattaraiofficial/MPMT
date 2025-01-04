using Microsoft.AspNetCore.Mvc;
using Mpmt.Core.Common;
using Mpmt.Core.Dtos.FeeFundRequest;
using Mpmt.Core.Dtos.PartnerReport;
using Mpmt.Core.Extensions;
using Mpmt.Services.Services.FeeFundRequest;
using Mpmt.Services.Services.PreFund;
using Mpmt.Services.Services.RoleMenuPermission;
using Mpmt.Web.Common;
using Mpmt.Web.Filter;
using System.Data;

namespace Mpmt.Web.Areas.Admin.Controllers;
[RolePremission]
[AdminAuthorization]
public class FeeFundRequestController : BaseAdminController
{
    private readonly IFeeFundRequestService _feeFundRequestService;
    private readonly IPreFundRequestServices _fundRequestServices;
    private readonly IRMPService _rMPService;

    public FeeFundRequestController(IFeeFundRequestService feeFundRequest, IPreFundRequestServices fundRequestServices, IRMPService rMPService)
    {
        _feeFundRequestService = feeFundRequest;
        _fundRequestServices = fundRequestServices;
        _rMPService = rMPService;
    }

    [HttpGet]
    public async Task<IActionResult> FeeFundRequestIndex([FromQuery] FeeFundRequestFilter requestFilter)
    {
        var actions = await _rMPService.GetActionPermissionListAsync("FeeFundRequest");
        ViewBag.actions = actions;

        var data = await _feeFundRequestService.GetFeeFundRequestAsync(requestFilter);
        if (WebHelper.IsAjaxRequest(Request))
        {
            return PartialView("_FeeFundRequestIndex", data);
        }
        return View(data);
    }
    [HttpGet("ExportFeeFundRequestToCsv")]
    public async Task<IActionResult> ExportFeeFundRequestToCsv([FromQuery] FeeFundRequestFilter request)
    {
        request.Export = 1;
        //request.PartnerCode = "admin";
        var result = await _feeFundRequestService.GetFeeFundRequestAsync(request);
        var data = result.Items;
        var (bytes, fileformate, filename) = ExportHelper.GenerateCsv(data, new string[] { }, null, "FeeFundRequestReports", true);
        return File(bytes, fileformate, filename);
    }

    [HttpGet("ExportFeeFundRequesttoexcell")]
    public async Task<IActionResult> ExportFeeFundRequesttoexcell([FromQuery] FeeFundRequestFilter request)
    {
        request.Export = 1;
        //request.PartnerCode = "admin";
        var result = await _feeFundRequestService.GetFeeFundRequestAsync(request);
        var data = result.Items;
        List<DataTable> dataTables = await IEnumerableExtensions.ToDataTablesAsync<FeeFundRequestList>(data, 500000);
        var (excelFileByteArr, fileFormat, fileName) = await ExportHelper.ToExcelAsync(dataTables, "FeeFundRequestReports", true);

        return File(excelFileByteArr, fileFormat, fileName);
    }


    [HttpGet("ExportFeeFundRequesttoPdf")]
    public async Task<FileContentResult> ExportFeeFundRequesttoPdf([FromQuery] FeeFundRequestFilter request)
    {

        request.Export = 1;
        //request.PartnerCode = "admin";
        var result = await _feeFundRequestService.GetFeeFundRequestAsync(request);
        var data = result;

        var (bytedata, format) = await ExportHelper.TopdfAsync<FeeFundRequestList>(data, "FeeFundRequest Reports");
        return File(bytedata, format, "FeeFundRequestreport.pdf");

    }


    [HttpGet]
    public async Task<IActionResult> FeeFundRequestDetails(int Id)
    {
        var data = await _fundRequestServices.GetPreFundRequestApprovedByIdAsync(Id);
        return PartialView(data);
    }
}
