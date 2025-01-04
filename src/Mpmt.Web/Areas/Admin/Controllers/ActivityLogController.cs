using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mpmt.Core.Dtos.ActivityLog;
using Mpmt.Services.Services.RoleMenuPermission;
using Mpmt.Services.Services.UserActivityLog;
using Mpmt.Web.Common;
using Mpmt.Web.Filter;

namespace Mpmt.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// The activity log controller.
    /// </summary>
    [RolePremission]
    [Authorize]
    [AdminAuthorization]
    public class ActivityLogController : BaseAdminController
    {
        private readonly IUserActivityLog _userActivityLog;
        private readonly IRMPService _rMPService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityLogController"/> class.
        /// </summary>
        /// <param name="userActivityLog">The user activity log.</param>
        public ActivityLogController(IUserActivityLog userActivityLog, IRMPService rMPService)
        {
            _userActivityLog = userActivityLog;
            _rMPService = rMPService;
        }
        /// <summary>
        /// Users the activity log.
        /// </summary>
        /// <param name="ativityLogFilter">The ativity log filter.</param>
        /// <returns>A Task.</returns>
        [HttpGet]
        public async Task<IActionResult> UserActivityLog([FromQuery] UserAtivityLogFilter ativityLogFilter)
        {
            var actions = await _rMPService.GetActionPermissionListAsync("ActivityLog");

            ViewBag.actions = actions;
            Response.Headers["Cache-Control"] = "no-store, no-cache, must-revalidate";
            Response.Headers["Pragma"] = "no-cache";
            Response.Headers["Expires"] = "0";

            var data = await _userActivityLog.GetActivityLogAsync(ativityLogFilter);
            if (WebHelper.IsAjaxRequest(Request))
            {
                return  await Task.FromResult(PartialView("_ActivityLogList", data));
            }
            return await Task.FromResult(View(data));
        }
    }
}
