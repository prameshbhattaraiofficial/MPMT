using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Mpmt.Core.Dtos.Module;
using Mpmt.Core.ViewModel.Module;
using Mpmt.Services.Services.Common;
using Mpmt.Services.Services.Module;
using Mpmt.Web.Common;
using Mpmt.Web.Filter;
using System.Net;

namespace Mpmt.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// The module controller.
    /// </summary>
    [ServiceFilter(typeof(RoleGroupFilterAttribute))]
    [AdminAuthorization]
    public class ModuleController : BaseAdminController
    {
        private readonly IModuleService _moduleServices;
        private readonly INotyfService _notyfService;
        private readonly ICommonddlServices _commonddl;
        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleController"/> class.
        /// </summary>
        /// <param name="moduleService">The module service.</param>
        /// <param name="notyfService">The notyf service.</param>
        public ModuleController(IModuleService moduleService, INotyfService notyfService, ICommonddlServices commonddl)
        {
            _moduleServices = moduleService;
            _notyfService = notyfService;
            _commonddl = commonddl;
            ;
        }

        /// <summary>
        /// Modules the index.
        /// </summary>
        /// <returns>A Task.</returns>
        [HttpGet]
        public async Task<IActionResult> ModuleIndex()
        {
            var result = await _moduleServices.GetModuleAsync();
            var ModulewithChild = new List<ModuleWithChild>();
            foreach (var menu in result)
            {
                if (menu.ParentId == 0)
                {
                    var Parentmenu = new ModuleWithChild
                    {
                        child = new List<ModuleWithChild>(),
                        Id = menu.Id,

                        ParentId = menu.ParentId,
                        Module = menu.Module,
                        IsActive = menu.IsActive,
                        DisplayOrder = menu.DisplayOrder
                    };

                    foreach (var child in result)
                    {
                        if (child.ParentId == menu.Id && child.Id != menu.Id)
                        {
                            var childmenu = new ModuleWithChild();
                            childmenu.Id = child.Id;
                            childmenu.ParentId = child.ParentId;
                            childmenu.Module = child.Module;
                            childmenu.IsActive = child.IsActive;
                            ;
                            childmenu.DisplayOrder = child.DisplayOrder;
                            Parentmenu.child.Add(childmenu);
                        }
                    }
                    ModulewithChild.Add(Parentmenu);
                }
            }



            if (WebHelper.IsAjaxRequest(Request))
                return await Task.FromResult(PartialView("_ModuleIndex", ModulewithChild));

            return await Task.FromResult(View(ModulewithChild));
        }


        /// <summary>
        /// Adds the module.
        /// </summary>
        /// <returns>A Task.</returns>
        [HttpGet]
        public async Task<IActionResult> AddModule()
        {
            var data = await _commonddl.GetParentModule();
            ViewBag.ParentModuleddl = new SelectList(data, "value", "Text");
            return await Task.FromResult(PartialView());
        }


        /// <summary>
        /// Adds the module.
        /// </summary>
        /// <param name="addModule">The add module.</param>
        /// <returns>A Task.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddModule([FromForm] IUDModule addModule)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView();
            }
            else
            {
                var responseStatus = await _moduleServices.AddModuleAsync(addModule);
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
        /// Updates the module.
        /// </summary>
        /// <param name="ModuleId">The module id.</param>
        /// <returns>A Task.</returns>
        [HttpGet]
        public async Task<IActionResult> UpdateModule(int ModuleId)
        {
            var data = await _commonddl.GetParentModule();
            ViewBag.ParentModuleddl = new SelectList(data, "value", "Text");
            var result = await _moduleServices.GetModuleByIdAsync(ModuleId);
            return await Task.FromResult(PartialView(result));
        }


        /// <summary>
        /// Updates the module.
        /// </summary>
        /// <param name="updateModule">The update module.</param>
        /// <returns>A Task.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateModule([FromForm] IUDModule updateModule)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView();
            }
            else
            {
                var responseStatus = await _moduleServices.UpdateModuleAsync(updateModule);
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
        /// Deletes the module.
        /// </summary>
        /// <param name="ModuleId">The module id.</param>
        /// <returns>A Task.</returns>
        [HttpGet]
        public async Task<IActionResult> DeleteModule(int ModuleId)
        {
            var result = await _moduleServices.GetModuleByIdAsync(ModuleId);

            return await Task.FromResult(PartialView(result));
        }


        /// <summary>
        /// Deletes the module.
        /// </summary>
        /// <param name="deleteModule">The delete module.</param>
        /// <returns>A Task.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteModule([FromForm] IUDModule deleteModule)
        {
            if (deleteModule.Id != 0)
            {
                var responseStatus = await _moduleServices.RemoveModuleAsync(deleteModule);
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
        public async void UpdateModuleDisplayOrder(int Id, int DisplayOrderValue)
        {
            if (Id > 0)
            {
                var UpdateOrder = new IUDModule()
                {
                    Id = Id,
                    DisplayOrder = DisplayOrderValue
                };
                var response = await _moduleServices.UpdateModuleDisplayOrderAsync(UpdateOrder);
            }

        }
        /// <summary>
        /// Updates the menu isactive.
        /// </summary>
        /// <param name="Id">The id.</param>
        /// <param name="IsActive">If true, is active.</param>
        /// <returns>A Task.</returns>
        [HttpPost]
        public async Task<IActionResult> UpdateModuleIsactive(int Id, bool IsActive)
        {
            if (Id > 0)
            {
                var Menuupdate = new IUDModule
                {
                    Id = Id,
                    IsActive = IsActive
                };
                var response = await _moduleServices.UpdateModuleIsActiveAsync(Menuupdate);
                if (response.StatusCode == 200)
                {
                    _notyfService.Success(response.MsgText);
                    return Ok();

                }
                _notyfService.Error(response.MsgText);
            }
            return Ok();

        }
    }
}
