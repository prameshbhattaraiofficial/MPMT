using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Mpmt.Core.Dtos.ModuleAction;
using Mpmt.Services.Services.Common;
using Mpmt.Services.Services.ModuleAction;
using Mpmt.Web.Common;
using Mpmt.Web.Filter;
using System.Net;

namespace Mpmt.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// The module action controller.
    /// </summary>
    [RolePremission]
    [AdminAuthorization]
    public class ModuleActionController : BaseAdminController
    {
        private readonly IModuleActionService _moduleactionservice;
        private readonly INotyfService _notyfService;
        private readonly ICommonddlServices _commonddl;
        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleActionController"/> class.
        /// </summary>
        /// <param name="moduleActionService">The module action service.</param>
        /// <param name="notyfService">The notyf service.</param>
        /// <param name="commonddlServices">The commonddl services.</param>
        public ModuleActionController(IModuleActionService moduleActionService, INotyfService notyfService, ICommonddlServices commonddlServices)
        {
            _moduleactionservice = moduleActionService;
            _notyfService = notyfService;
            _commonddl = commonddlServices;
        }

        /// <summary>
        /// Modules the action index.
        /// </summary>
        /// <returns>A Task.</returns>
        [HttpGet]
        public async Task<IActionResult> ModuleActionIndex()
        {
            var result = await _moduleactionservice.GetModuleActionAsync();
            if (WebHelper.IsAjaxRequest(Request))
                return await Task.FromResult(PartialView("_ModuleActionIndex", result));

            return await Task.FromResult(View(result));
        }



        /// <summary>
        /// Adds the module action.
        /// </summary>
        /// <returns>A Task.</returns>
        [HttpGet]
        public async Task<IActionResult> AddModuleAction()
        {
            var data = await _commonddl.GetParentModule();
            ViewBag.ParentModuleddl = new SelectList(data, "value", "Text");

            var action = await _commonddl.GetAction();
            ViewBag.Actionddl = new SelectList(action, "value", "Text");
            return await Task.FromResult(PartialView());
        }



        /// <summary>
        /// Adds the module action.
        /// </summary>
        /// <param name="addModuleaction">The add moduleaction.</param>
        /// <returns>A Task.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddModuleAction([FromForm] IUDModuleAction addModuleaction)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView();
            }
            else
            {
                var responseStatus = await _moduleactionservice.AddModuleActionAsync(addModuleaction);
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
        /// Updates the module action.
        /// </summary>
        /// <param name="ModuleId">The module id.</param>
        /// <returns>A Task.</returns>
        [HttpGet]
        public async Task<IActionResult> UpdateModuleAction(int moduleActionId)
        {
            var data = await _commonddl.GetParentModule();
            ViewBag.ParentModuleddl = new SelectList(data, "value", "Text");
            var action = await _commonddl.GetAction();
            ViewBag.Actionddl = new SelectList(action, "value", "Text");
            var result = await _moduleactionservice.GetModuleByIdAsync(moduleActionId);
            return await Task.FromResult(PartialView(result));
        }



        /// <summary>
        /// Updates the module action.
        /// </summary>
        /// <param name="addModuleaction">The add moduleaction.</param>
        /// <returns>A Task.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateModuleAction([FromForm] IUDModuleAction addModuleaction)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView();
            }
            else
            {
                var responseStatus = await _moduleactionservice.UpdateModuleActionAsync(addModuleaction);
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
        /// Deletes the module action.
        /// </summary>
        /// <param name="ModuleId">The module id.</param>
        /// <returns>A Task.</returns>
        [HttpGet]
        public async Task<IActionResult> DeleteModuleAction(int moduleActionId)
        {
            var result = await _moduleactionservice.GetModuleByIdAsync(moduleActionId);

            return await Task.FromResult(PartialView(result));
        }



        /// <summary>
        /// Deletes the module action.
        /// </summary>
        /// <param name="deleteModuleaction">The delete moduleaction.</param>
        /// <returns>A Task.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteModuleAction([FromForm] IUDModuleAction deleteModuleaction)
        {
            if (deleteModuleaction.Id != 0)
            {
                var responseStatus = await _moduleactionservice.RemoveModuleActionAsync(deleteModuleaction);
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
