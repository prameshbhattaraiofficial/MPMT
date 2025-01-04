using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Mpmt.Core.Dtos.DropDown;
using Mpmt.Services.Services.Common;
using Mpmt.Services.Services.Notification;
using Mpmt.Web.Common;
using System.Net;

namespace Mpmt.Web.Areas.Admin.Controllers
{
    public class NotificationController : BaseAdminController
    {
        private readonly INotificationService _notificationService;
        private readonly INotyfService _notyfService;
        private readonly ICommonddlServices _commonddl;

        public NotificationController(INotificationService notificationService, ICommonddlServices commonddl, INotyfService notyfService)
        {
            _notificationService = notificationService;
            _commonddl = commonddl;
            _notyfService = notyfService;
        }

        public async Task<IActionResult> ModuleIndex()
        {
            var result = await _notificationService.GetNotificationModuleAsync();
            if (WebHelper.IsAjaxRequest(Request))
                return await Task.FromResult(PartialView("_ModuleIndex", result));
            return await Task.FromResult(View(result));
        }

        [HttpGet]
        public async Task<IActionResult> AssignNotificationModuleRole(int id)
        {
            AssignNotificationModuleRoleDto roleDto = new AssignNotificationModuleRoleDto()
            {
                moduleid = id
            };

            var UserRoleById = await _commonddl.GetNotificationModuleRolesByModuleIdAsync(id);
            var admiroleddl = await _commonddl.GetAdminRoleddl();

            ViewBag.AdminRoleddl = new SelectList(admiroleddl, "value", "Text");
            var Roles = new List<SelectListItem>();
            var roles = new List<int>();
            foreach (var userrole in UserRoleById)
            {
                roles.Add(int.Parse(userrole.value));
            }
            foreach (var role in admiroleddl)
            {
                if (roles.Contains(int.Parse(role.value)))
                {
                    Roles.Add(new SelectListItem() { Value = role.value, Text = role.Text, Selected = true });
                }
                else
                {
                    Roles.Add(new SelectListItem() { Value = role.value, Text = role.Text, Selected = false });
                }
            }
            ViewBag.Roles = Roles;
            return PartialView("_AssignModuleRole", roleDto);
        }

        [HttpPost]
        public async Task<IActionResult> AssignNotificationModuleRole([FromForm] AssignNotificationModuleRoleDto assignModuleDto)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView("_AssignModuleRole", assignModuleDto);
            }
            else
            {
                var result = await _notificationService.AssignModuleRole(assignModuleDto.moduleid, assignModuleDto.roleid);
                if (result.StatusCode == 200)
                {
                    _notyfService.Success(result.MsgText);
                    return Ok();
                }
                else
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    ViewBag.Error = result.MsgText;
                    return PartialView("_AssignModuleRole", assignModuleDto);
                }
            }
        }
    }
}
