using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Mpmt.Core.Dtos.RoleModuleAction;
using Mpmt.Services.Services.Common;
using Mpmt.Services.Services.RoleMenuPermission;
using Mpmt.Services.Services.RoleModuleAction;
using Mpmt.Web.Common;
using Mpmt.Web.Filter;
using System.Net;

namespace Mpmt.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// The role module action controller.
    /// </summary>
    [RolePremission]
    [AdminAuthorization]
    public class RoleModuleActionController : BaseAdminController
    {
        private readonly IRoleModuleActionService _rolemoduleactionservice;
        private readonly INotyfService _notyfService;
        private readonly ICommonddlServices _commonddl;
        private readonly IRMPService _rMPService;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleModuleActionController"/> class.
        /// </summary>
        /// <param name="roleModuleActionService">The role module action service.</param>
        /// <param name="notyfService">The notyf service.</param>
        /// <param name="commonddl">The commonddl.</param>
        public RoleModuleActionController(IRoleModuleActionService roleModuleActionService, INotyfService notyfService, ICommonddlServices commonddl, IRMPService rMPService)
        {
            _rolemoduleactionservice = roleModuleActionService;
            _notyfService = notyfService;
            _commonddl = commonddl;
            _rMPService = rMPService;
        }

        /// <summary>
        /// Roles the module action index.
        /// </summary>
        /// <returns>A Task.</returns>
        [HttpGet]
        public async Task<IActionResult> RoleModuleActionIndex()
        {
            var actions = await _rMPService.GetActionPermissionListAsync("RoleModuleAction");

            ViewBag.actions = actions;
            var result = await _rolemoduleactionservice.GetRoleModuleActionAsync();
            if (WebHelper.IsAjaxRequest(Request))
                return await Task.FromResult(PartialView("_RoleModuleActionIndex", result));

            return await Task.FromResult(View(result));
        }



        /// <summary>
        /// Adds the role module action.
        /// </summary>
        /// <returns>A Task.</returns>
        [HttpGet]
        public async Task<IActionResult> AddRoleModuleAction()
        {
            var data = await _commonddl.GetParentModule();
            ViewBag.ParentModuleddl = new SelectList(data, "value", "Text");

            var action = await _commonddl.GetAction();
            ViewBag.Actionddl = new SelectList(action, "value", "Text");

            var role = await _commonddl.GetAdminRoleddl();
            ViewBag.Roleddl = new SelectList(role, "value", "Text");
            return await Task.FromResult(PartialView());
        }



        /// <summary>
        /// Adds the role module action.
        /// </summary>
        /// <param name="addRoleModuleaction">The add role moduleaction.</param>
        /// <returns>A Task.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [LogUserActivity("added a role module action")]
        public async Task<IActionResult> AddRoleModuleAction([FromForm] IUDRoleModuleAction addRoleModuleaction)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView();
            }
            else
            {
                var responseStatus = await _rolemoduleactionservice.AddRoleModuleActionAsync(addRoleModuleaction);
                if (responseStatus.StatusCode == 200)
                {
                    _notyfService.Success(responseStatus.MsgText);
                    return Ok();
                }
                else
                {
                    Response.StatusCode = (int)(HttpStatusCode.BadRequest);
                    ViewBag.Error = responseStatus.MsgText;
                    return PartialView();
                }
            }
        }




        /// <summary>
        /// Updates the role module action.
        /// </summary>
        /// <param name="rolemoduleActionId">The rolemodule action id.</param>
        /// <returns>A Task.</returns>
        [HttpGet]
        public async Task<IActionResult> UpdateRoleModuleAction(int rolemoduleActionId)
        {
            var role = await _commonddl.GetAdminRoleddl();
            ViewBag.Roleddl = new SelectList(role, "value", "Text");
            var data = await _commonddl.GetParentModule();
            ViewBag.ParentModuleddl = new SelectList(data, "value", "Text");
            var action = await _commonddl.GetActionsByModuleId(Convert.ToString(rolemoduleActionId));
            ViewBag.Actionddl = new SelectList(action, "value", "Text");
            var result = await _rolemoduleactionservice.GetRoleModuleByIdAsync(rolemoduleActionId);
            return await Task.FromResult(PartialView(result));
        }




        /// <summary>
        /// Updates the role module action.
        /// </summary>
        /// <param name="addRoleModuleaction">The add role moduleaction.</param>
        /// <returns>A Task.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [LogUserActivity("updated a role module action")]
        public async Task<IActionResult> UpdateRoleModuleAction([FromForm] IUDRoleModuleAction addRoleModuleaction)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView();
            }
            else
            {
                var responseStatus = await _rolemoduleactionservice.UpdateRoleModuleActionAsync(addRoleModuleaction);
                if (responseStatus.StatusCode == 200)
                {
                    _notyfService.Success(responseStatus.MsgText);
                    return Ok();
                }
                else
                {
                    Response.StatusCode = (int)(HttpStatusCode.BadRequest);
                    ViewBag.Error = responseStatus.MsgText;
                    return PartialView();
                }
            }
        }




        /// <summary>
        /// Deletes the role module action.
        /// </summary>
        /// <param name="RolemoduleActionId">The rolemodule action id.</param>
        /// <returns>A Task.</returns>
        [HttpGet]
        public async Task<IActionResult> DeleteRoleModuleAction(int RolemoduleActionId)
        {
            var result = await _rolemoduleactionservice.GetRoleModuleByIdAsync(RolemoduleActionId);

            return await Task.FromResult(PartialView(result));
        }




        /// <summary>
        /// Deletes the module action.
        /// </summary>
        /// <param name="deleteModuleaction">The delete moduleaction.</param>
        /// <returns>A Task.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [LogUserActivity("deleted a role module action")]
        public async Task<IActionResult> DeleteRoleModuleAction([FromForm] IUDRoleModuleAction deleteRoleModuleaction)
        {
            if (deleteRoleModuleaction.Id != 0)
            {
                var responseStatus = await _rolemoduleactionservice.RemoveRoleModuleActionAsync(deleteRoleModuleaction);
                if (responseStatus.StatusCode == 200)
                {
                    _notyfService.Success(responseStatus.MsgText);
                    return Ok();
                }
                else
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    TempData["Error"] = responseStatus.MsgText;
                }
            }
            Response.StatusCode = (int)HttpStatusCode.NotFound;
            return PartialView();
        }
        /// <summary>
        /// Gets the actions by module id.
        /// </summary>
        /// <param name="ModuleId">The module id.</param>
        [HttpGet]
        public async Task<JsonResult> GetActionsByModuleId(string ModuleId)
        {


            var response = await _commonddl.GetActionsByModuleId(ModuleId);
            return new JsonResult(response);



        }

    }
}
