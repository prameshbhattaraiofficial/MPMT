using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Mpmt.Core.Dtos.Action;
using Mpmt.Services.Services.Action;
using Mpmt.Services.Services.RoleMenuPermission;
using Mpmt.Web.Common;
using Mpmt.Web.Filter;
using System.Net;

namespace Mpmt.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// The action controller.
    /// </summary>
    [RolePremission]
    [AdminAuthorization]
    public class ActionController : BaseAdminController
    {
        private readonly IActionService _actionServices;
        private readonly INotyfService _notyfService;
        private readonly IRMPService _rMPService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionController"/> class.
        /// </summary>
        /// <param name="actionService">The action service.</param>
        /// <param name="notyfService">The notyf service.</param>
        public ActionController(IActionService actionService, INotyfService notyfService, IRMPService rMPService)
        {
            _actionServices = actionService;
            _notyfService = notyfService;
            _rMPService = rMPService;
        }

        /// <summary>
        /// Actions the index.
        /// </summary>
        /// <returns>A Task.</returns>
        [HttpGet]
        public async Task<IActionResult> ActionIndex()
        {
            var actions = await _rMPService.GetActionPermissionListAsync("Action");

            ViewBag.actions = actions;
            var result = await _actionServices.GetActionAsync();
            if (WebHelper.IsAjaxRequest(Request))
                return await Task.FromResult(PartialView("_ActionIndex", result));

            return await Task.FromResult(View(result));
        }



        /// <summary>
        /// Adds the action.
        /// </summary>
        /// <returns>A Task.</returns>
        [HttpGet]
        public async Task<IActionResult> AddAction()
        {
            return await Task.FromResult(PartialView());

        }



        /// <summary>
        /// Adds the action.
        /// </summary>
        /// <param name="addAction">The add action.</param>
        /// <returns>A Task.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [LogUserActivity("added action")]

        public async Task<IActionResult> AddAction([FromForm] IUDAction addAction)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView();

            }
            else
            {
                var responseStatus = await _actionServices.AddActionAsync(addAction);
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
        /// Updates the action.
        /// </summary>
        /// <param name="ActionId">The action id.</param>
        /// <returns>A Task.</returns>
        [HttpGet]
        public async Task<IActionResult> UpdateAction(int ActionId)
        {
            var result = await _actionServices.GetActionByIdAsync(ActionId);
            return await Task.FromResult(PartialView(result));

        }



        /// <summary>
        /// Updates the action.
        /// </summary>
        /// <param name="updateAction">The update action.</param>
        /// <returns>A Task.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [LogUserActivity("updated action")]

        public async Task<IActionResult> UpdateAction([FromForm] IUDAction updateAction)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView();
            }
            else
            {
                var responseStatus = await _actionServices.UpdateActionAsync(updateAction);
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
        /// Deletes the action.
        /// </summary>
        /// <param name="ActionId">The action id.</param>
        /// <returns>A Task.</returns>
        [HttpGet]
        public async Task<IActionResult> DeleteAction(int ActionId)
        {
            var result = await _actionServices.GetActionByIdAsync(ActionId);
            return await Task.FromResult(PartialView(result));
        }



        /// <summary>
        /// Deletes the action.
        /// </summary>
        /// <param name="deleteAction">The delete action.</param>
        /// <returns>A Task.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [LogUserActivity("Deleted action")]

        public async Task<IActionResult> DeleteAction([FromForm] IUDAction deleteAction)
        {
            if (deleteAction.Id != 0)
            {
                var responseStatus = await _actionServices.RemoveActionAsync(deleteAction);
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

    }
}
