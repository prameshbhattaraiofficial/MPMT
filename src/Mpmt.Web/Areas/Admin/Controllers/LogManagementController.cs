using Microsoft.AspNetCore.Mvc;
using Mpmt.Core.Dtos.ActivityLog;
using Mpmt.Services.Services.RoleMenuPermission;
using Mpmt.Services.Services.UserActivityLog;
using Mpmt.Web.Common;
using Mpmt.Web.Filter;

namespace Mpmt.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// The log management controller.
    /// </summary>
    [RolePremission]
    [AdminAuthorization]
    public class LogManagementController : BaseAdminController
    {
        private readonly IUserActivityLog _userActivityLog;
        private readonly IRMPService _rMPService;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogManagementController"/> class.
        /// </summary>
        /// <param name="userActivityLog">The user activity log.</param>
        public LogManagementController(IUserActivityLog userActivityLog, IRMPService rMPService)
        {
            _userActivityLog = userActivityLog;
            _rMPService = rMPService;
        }

        /// <summary>
        /// Vendors the api log report index.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns>A Task.</returns>
        public async Task<IActionResult> VendorApiLogReportIndex([FromQuery] VendorApiLogFilter filter)
        {
            var actions = await _rMPService.GetActionPermissionListAsync("LogManagement");

            ViewBag.actions = actions;
            filter.PartnerCode = "admin";
            var result = await _userActivityLog.GetVendorApiLogReport(filter);
            if (WebHelper.IsAjaxRequest(Request))
                return await Task.FromResult(PartialView("_VendorApiLogReportIndex", result));

            return await Task.FromResult(View(result));
        }

        /// <summary>
        /// Gets the request response api log.
        /// </summary>
        /// <param name="logId">The log id.</param>
        /// <returns>A Task.</returns>
        [HttpGet]
        public async Task<IActionResult> GetRequestResponseApiLog(string logId)
        {
            var requestResponse = await _userActivityLog.GetRequestResponseApiLogById(logId);
            return PartialView("_GetRequestResponseApiLog", requestResponse);
        }

        /// <summary>
        /// Gets the vendor api log by id.
        /// </summary>
        /// <param name="logId">The log id.</param>
        /// <returns>A Task.</returns>
        [HttpGet]
        public async Task<IActionResult> GetVendorApiLogById(string logId)
        {
            var requestResponse = await _userActivityLog.GetVendorApiLogById(logId);
            return PartialView("_GetVendorApiLogById", requestResponse);
        }

        /// <summary>
        /// Gets the vendor request response api log.
        /// </summary>
        /// <param name="logId">The log id.</param>
        /// <returns>A Task.</returns>
        [HttpGet]
        public async Task<IActionResult> GetVendorRequestResponseApiLog(string logId)
        {
            var vendorRequestResponse = await _userActivityLog.GetVendorRequestResponseApiLogById(logId);
            return PartialView("_GetVendorRequestResponseApiLog", vendorRequestResponse);
        }

        /// <summary>
        /// Gets the vendor request response api log2.
        /// </summary>
        /// <param name="logId">The log id.</param>
        /// <returns>A Task.</returns>
        [HttpGet]
        public async Task<IActionResult> GetVendorRequestResponseApiLog2(string logId)
        {
            var vendorRequestResponse2 = await _userActivityLog.GetVendorRequestResponseApiLogById2(logId);
            return PartialView("_GetVendorRequestResponseApiLog2", vendorRequestResponse2);
        }

        /// <summary>
        /// Gets the vendor request response api log3.
        /// </summary>
        /// <param name="logId">The log id.</param>
        /// <returns>A Task.</returns>
        [HttpGet]
        public async Task<IActionResult> GetVendorRequestResponseApiLog3(string logId)
        {
            var vendorRequestResponse3 = await _userActivityLog.GetVendorRequestResponseApiLogById3(logId);
            return PartialView("_GetVendorRequestResponseApiLog3", vendorRequestResponse3);
        }
    }
}
