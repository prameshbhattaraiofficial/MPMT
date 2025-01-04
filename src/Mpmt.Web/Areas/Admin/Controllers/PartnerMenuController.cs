using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Mpmt.Core.Dtos.DropDown;
using Mpmt.Core.Dtos.Menu;
using Mpmt.Core.ViewModel.Menu;
using Mpmt.Services.Services.Common;
using Mpmt.Services.Services.Menu;
using Mpmt.Services.Services.RoleMenuPermission;
using Mpmt.Web.Common;
using Mpmt.Web.Filter;
using System.Net;

namespace Mpmt.Web.Areas.Admin.Controllers
{
    [AdminAuthorization]
    [RolePremission]
    public class PartnerMenuController : BaseAdminController
    {
        private readonly IMenuService _menuService;
        private readonly INotyfService _notyfService;
        private readonly ICommonddlServices _commonddlServices;
        private readonly IRMPService _rMPService;

        public PartnerMenuController(IMenuService menuService, INotyfService notyfService, ICommonddlServices commonddlServices, IRMPService rMPService)
        {
            _menuService = menuService;
            _notyfService = notyfService;
            _commonddlServices = commonddlServices;
            _rMPService = rMPService;
        }
        public async Task<IActionResult> Index()
        {
            var actions = await _rMPService.GetActionPermissionListAsync("PartnerMenu");
            ViewBag.actions = actions;

            var result = await _menuService.GetPartnerMenuAsync();
            var Menuwithchield = new List<PartnerMenu>();
            foreach (var menu in result)
            {
                if (menu.ParentId == 0)
                {
                    var Parentmenu = new PartnerMenu
                    {
                        child = new List<PartnerMenu>(),
                        Id = menu.Id,
                        ImagePath = menu.ImagePath,
                        ParentId = menu.ParentId,
                        Title = menu.Title,
                        Action = menu.Action,
                        Controller = menu.Controller,
                        Area = menu.Area,
                        IsActive = menu.IsActive,
                        DisplayOrder = menu.DisplayOrder
                    };

                    foreach (var child in result)
                    {
                        if (child.ParentId == menu.Id && child.Id != menu.Id)
                        {
                            var childmenu = new PartnerMenu();
                            childmenu.Id = child.Id;
                            childmenu.ParentId = child.ParentId;
                            childmenu.ImagePath = child.ImagePath;
                            childmenu.Title = child.Title;
                            childmenu.Action = child.Action;
                            childmenu.Controller = child.Controller;
                            childmenu.Area = child.Area;
                            childmenu.IsActive = child.IsActive;
                            childmenu.DisplayOrder = child.DisplayOrder;
                            Parentmenu.child.Add(childmenu);
                        }
                    }
                    Menuwithchield.Add(Parentmenu);
                }
            }
            if (WebHelper.IsAjaxRequest(Request))
            {
                return PartialView("_PartnerMenuTable", Menuwithchield);
            }

            return View(Menuwithchield);

        }
        [HttpGet]
        public async Task<IActionResult> AddMenu()
        {
            var data = await _commonddlServices.GetPartnerParentMenu();
            ViewBag.ParentMenuddl = new SelectList(data, "value", "Text");

            return await Task.FromResult(PartialView("_AddMenu"));
        }


        [HttpPost]
        [LogUserActivity("Added Partner Menu")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddMenu([FromForm] IUDPartnerMenu addMenu)
        {


            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView("_AddMenu", addMenu);
            }

            else
            {
                var ResponseStatus = await _menuService.AddPartnerMenuAsync(addMenu);
                if (ResponseStatus.StatusCode == 200)
                {
                    _notyfService.Success(ResponseStatus.MsgText);
                    return Ok();
                }
                else
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    ViewBag.Error = ResponseStatus.MsgText;
                    return PartialView("_AddMenu", addMenu);
                }
            }

        }

        [HttpGet]
        public async Task<IActionResult> UpdateMenu(int MenuId)
        {
            var data = await _commonddlServices.GetPartnerParentMenu();
            ViewBag.ParentMenuddl = new SelectList(data, "value", "Text");
            var Result = await _menuService.GetPartnerMenuByIdAsync(MenuId);
            //var mappeddata = _mapper.Map<IUDEmployee>(Result);
            return await Task.FromResult(PartialView("_UpdateMenu", Result));
        }


        [HttpPost]
        [LogUserActivity("Updated partner Menu")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateMenu([FromForm] IUDPartnerMenu updateMenu)
        {


            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView("_UpdateMenu", updateMenu);
            }

            else
            {
                var ResponseStatus = await _menuService.UpdatePartnerMenuAsync(updateMenu);
                if (ResponseStatus.StatusCode == 200)
                {
                    _notyfService.Success(ResponseStatus.MsgText);
                    return Ok();
                }
                else
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    ViewBag.Error = ResponseStatus.MsgText;
                    return PartialView("_UpdateMenu", updateMenu);
                }
            }

        }


        [HttpGet]
        public async Task<IActionResult> DeleteMenu(int MenuId)
        {
            var Result = await _menuService.GetPartnerMenuByIdAsync(MenuId);

            return await Task.FromResult(PartialView("_DeleteMneu",Result));
        }


        [HttpPost]
        [LogUserActivity("Deleted partner Menu")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteMenu([FromForm] IUDPartnerMenu updateMenu)
        {
            if (updateMenu.Id != 0)
            {
                var ResponseStatus = await _menuService.RemovePartnerMenuAsync(updateMenu);
                if (ResponseStatus.StatusCode == 200)
                {
                    _notyfService.Success(ResponseStatus.MsgText);
                    return Ok();
                }
                else
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    TempData["Error"] = ResponseStatus.MsgText;
                }
            }
            Response.StatusCode = (int)HttpStatusCode.NotFound;
            return PartialView("_DeleteMenu", updateMenu);
        }


        public async void UpdateMenuDisplayOrder(int Id, int DisplayOrderValue)
        {
            if (Id > 0)
            {
                var UpdateOrder = new IUDMenu()
                {
                    Id = Id,
                    DisplayOrder = DisplayOrderValue
                };
                var response = await _menuService.UpdatePartnerMenuDisplayOrderAsync(UpdateOrder);
            }

        }

        [HttpPost]
        public async Task<IActionResult> UpdateMenuIsactive(int Id, bool IsActive)
        {
            if (Id > 0)
            {
                var Menuupdate = new IUDMenu
                {
                    Id = Id,
                    IsActive = IsActive
                };
                var response = await _menuService.UpdatePartnerMenuIsActiveAsync(Menuupdate);
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
