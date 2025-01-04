using Microsoft.AspNetCore.Mvc;
using Mpmt.Core.Dtos.ActivityLog;
using Mpmt.Services.Services.UserActivityLog;
using Mpmt.Web.Common;
using Mpmt.Web.Filter;

namespace Mpmt.Web.Areas.Partner.Controllers
{
    [PartnerAuthorization]
    [RolePremission]
    public class LogManagementController : BasePartnerController
    {
        private readonly IUserActivityLog _userActivityLog;

        public LogManagementController(IUserActivityLog userActivityLog)
        {
            _userActivityLog = userActivityLog;
        }

        public async Task<IActionResult> VendorApiLogReportIndex([FromQuery] VendorApiLogFilter filter)
        {
            filter.PartnerCode = User.Claims.FirstOrDefault(x => x.Type == "PartnerCode").Value;
            var result = await _userActivityLog.GetVendorApiLogReport(filter);
            if (WebHelper.IsAjaxRequest(Request))
                return await Task.FromResult(PartialView("_VendorApiLogReportIndex", result));

            return await Task.FromResult(View(result));
        }

        [HttpGet]
        public async Task<IActionResult> GetRequestResponseApiLog(string logId)
        {
            var requestResponse = await _userActivityLog.GetRequestResponseApiLogById(logId);
            return PartialView("_GetRequestResponseApiLog", requestResponse);
        }

        [HttpGet]
        public async Task<IActionResult> GetVendorApiLogById(string logId)
        {
            var requestResponse = await _userActivityLog.GetVendorApiLogById(logId);
            return PartialView("_GetVendorApiLogById", requestResponse);
        }

        [HttpGet]
        public async Task<IActionResult> GetVendorRequestResponseApiLog(string logId)
        {
            var vendorRequestResponse = await _userActivityLog.GetVendorRequestResponseApiLogById(logId);
            return PartialView("_GetVendorRequestResponseApiLog", vendorRequestResponse);
        }

        [HttpGet]
        public async Task<IActionResult> GetVendorRequestResponseApiLog2(string logId)
        {
            var vendorRequestResponse2 = await _userActivityLog.GetVendorRequestResponseApiLogById2(logId);
            return PartialView("_GetVendorRequestResponseApiLog2", vendorRequestResponse2);
        }

        [HttpGet]
        public async Task<IActionResult> GetVendorRequestResponseApiLog3(string logId)
        {
            var vendorRequestResponse3 = await _userActivityLog.GetVendorRequestResponseApiLogById3(logId);
            return PartialView("_GetVendorRequestResponseApiLog3", vendorRequestResponse3);
        }
    }
}
