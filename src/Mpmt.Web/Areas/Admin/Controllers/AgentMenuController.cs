using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Mpmt.Core.Dtos.CashAgent;
using Mpmt.Core.Models.Route;
using Mpmt.Core.ViewModel.CashAgent;
using Mpmt.Core.ViewModel.Menu;
using Mpmt.Services.CashAgents;
using Mpmt.Services.Services.Common;
using Mpmt.Web.Areas.Admin.Controllers;
using Mpmt.Web.Common;
using Mpmt.Web.Filter;
using System.Net;

namespace Mpmt.Agent.Controllers;

public class AgentMenuController : BaseAdminController
{
    private readonly IAgentMenuService _agentMenuService;
    private readonly INotyfService _notyfService;
    private readonly ICommonddlServices _commonddl;

    public AgentMenuController(IAgentMenuService agentMenuService, INotyfService notyfService, ICommonddlServices commonddl)
    {
        _agentMenuService = agentMenuService;
        _notyfService = notyfService;
        _commonddl = commonddl;
    }

    [HttpGet]
    public async Task<IActionResult> MenuIndex()
    {
        var result = await _agentMenuService.GetAgentMenuAsync();
        var menuWithChild = new List<AgentMenuChild>();
        foreach (var menu in result)
        {
            if (menu.ParentId == 0)
            {
                var ParentMenu = new AgentMenuChild
                {
                    Child = new List<AgentMenuChild>(),
                    Id = menu.Id,
                    ImagePath = menu.ImagePath,
                    ParentId = menu.ParentId,
                    Title = menu.Title,
                    MenuUrl = menu.MenuUrl,
                    Area = menu.Area,
                    Controller = menu.Controller,
                    Action = menu.Action,
                    Status = menu.IsActive,
                    DisplayOrder = menu.DisplayOrder
                };

                foreach (var child in result)
                {
                    if (child.ParentId == menu.Id && child.Id != menu.Id)
                    {
                        var childmenu = new AgentMenuChild();
                        childmenu.Id = child.Id;
                        childmenu.ParentId = child.ParentId;
                        childmenu.ImagePath = child.ImagePath;
                        childmenu.Title = child.Title;
                        childmenu.Area = child.Area;
                        childmenu.Controller = child.Controller;
                        childmenu.Action = child.Action;
                        childmenu.MenuUrl = child.MenuUrl;
                        childmenu.Status = child.IsActive;
                        childmenu.DisplayOrder = child.DisplayOrder;
                        ParentMenu.Child.Add(childmenu);
                    }
                }
                menuWithChild.Add(ParentMenu);
            }
        }
        if (WebHelper.IsAjaxRequest(Request))
        {
            return PartialView("_MenuIndex", menuWithChild);
        }

        return View(menuWithChild);
    }

    [HttpGet]
    public async Task<IActionResult> UserTypeList()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> UpdateMenuPermission(string UserType)
    {
        var data = await _agentMenuService.GetListcontrollerActionAsync(UserType);
        ViewBag.Menu = data.ToList();
        ViewBag.UserType = UserType;
        return PartialView("_addMenuPermissions");
    }

    [HttpPost]
    [LogUserActivity("updated agent menu permission")]
    public async Task<IActionResult> UpdateMenuPermission(AddControllerActionUserType test)
    {
        var response = await _agentMenuService.AddmenuPermission(test);
        if (response.StatusCode == 200)
        {
            _notyfService.Success(response.MsgText);
            return Ok();
        }
        var data = await _agentMenuService.GetListcontrollerActionAsync(test.UserType);
        ViewBag.Menu = data;
        ViewBag.UserType = test.UserType;
        return PartialView("_addMenuPermission");
    }

    [HttpGet]
    public async Task<IActionResult> AddMenu()
    {
        var data = await _commonddl.GetAgentParentMenu();
        ViewBag.ParentMenuDdl = new SelectList(data, "value", "Text");
        return await Task.FromResult(PartialView());
    }

    [HttpPost]
    [LogUserActivity("added Menu")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddMenu([FromForm] IUDAgentMenu addMenu)
    {
        if (!ModelState.IsValid)
        {
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return PartialView();
        }
        else
        {
            var ResponseStatus = await _agentMenuService.AddAgentMenuAsync(addMenu);
            if (ResponseStatus.StatusCode == 200)
            {
                _notyfService.Success(ResponseStatus.MsgText);
                return Ok();
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                ViewBag.Error = ResponseStatus.MsgText;
                return PartialView();
            }
        }
    }

    [HttpGet]
    public async Task<IActionResult> UpdateMenu(int MenuId)
    {
        var data = await _commonddl.GetAgentParentMenu();
        ViewBag.ParentMenuddl = new SelectList(data, "value", "Text");
        var Result = await _agentMenuService.GetAgentMenuByIdAsync(MenuId);
        return await Task.FromResult(PartialView(Result));
    }

    [HttpPost]
    [LogUserActivity("updated Menu")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateMenu([FromForm] IUDAgentMenu updateMenu)
    {
        if (!ModelState.IsValid)
        {
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return PartialView();
        }
        else
        {
            var ResponseStatus = await _agentMenuService.UpdateAgentMenuAsync(updateMenu);
            if (ResponseStatus.StatusCode == 200)
            {
                _notyfService.Success(ResponseStatus.MsgText);
                return Ok();
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                ViewBag.Error = ResponseStatus.MsgText;
                return PartialView();
            }
        }
    }

    [HttpGet]
    public async Task<IActionResult> DeleteMenu(int MenuId)
    {
        var Result = await _agentMenuService.GetAgentMenuByIdAsync(MenuId);
        return await Task.FromResult(PartialView(Result));
    }

    [HttpPost]
    [LogUserActivity("deleted Menu")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteMenu([FromForm] IUDAgentMenu deleteMenu)
    {
        if (deleteMenu.Id != 0)
        {
            var ResponseStatus = await _agentMenuService.RemoveAgentMenuAsync(deleteMenu);
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
        return PartialView();
    }

    public async void UpdateMenuDisplayOrder(int Id, int DisplayOrderValue)
    {
        if (Id > 0)
        {
            var UpdateOrder = new IUDAgentMenu()
            {
                Id = Id,
                DisplayOrder = DisplayOrderValue
            };
            var response = await _agentMenuService.UpdateMenuDisplayOrderAsync(UpdateOrder);
        }
    }

    [HttpPost]
    [LogUserActivity("updated menu active status")]
    public async Task<IActionResult> UpdateMenuIsActive(int Id, bool IsActive)
    {
        if (Id > 0)
        {
            var MenuUpdate = new IUDAgentMenu
            {
                Id = Id,
                IsActive = IsActive
            };
            var response = await _agentMenuService.UpdateMenuIsActiveAsync(MenuUpdate);
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
