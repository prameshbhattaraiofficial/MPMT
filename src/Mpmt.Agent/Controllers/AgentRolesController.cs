using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Mpmt.Agent.Common;
using Mpmt.Agent.Filter;
using Mpmt.Core.Dtos.DropDown;
using Mpmt.Core.Models.Route;
using Mpmt.Core.ViewModel.Role;
using Mpmt.Services.Services.Common;
using Mpmt.Services.Services.Roles;
using System.Net;

namespace Mpmt.Agent.Controllers;

[RolePremission]
public class AgentRolesController : AgentBaseController
{
    private readonly IAgentRoleServices _roleServices;
    private readonly INotyfService _notify;
    private readonly ICommonddlServices _commonddl;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AgentRolesController(IAgentRoleServices roleServices, INotyfService notyfService, IMapper mapper, ICommonddlServices commonddl, IHttpContextAccessor httpContextAccessor)
    {
        _roleServices = roleServices;
        _notify = notyfService;
        _mapper = mapper;
        _commonddl = commonddl;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var AgentCode = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(claims => claims.Type == "AgentCode")?.Value;
        var role = await _roleServices.GetRoleAsync(AgentCode);
        if (WebHelper.IsAjaxRequest(Request))
            return await Task.FromResult(PartialView("_RoleIndex", role));
        return await Task.FromResult(View(role));
    }

    [HttpGet]
    public async Task<IActionResult> AddRole()
    {
        return await Task.FromResult(PartialView());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddRole([FromForm] AddRoleVm addRole)
    {
        if (!ModelState.IsValid)
        {
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return PartialView();
        }
        else
        {
            var RoleResult = await _roleServices.AddRoleAsync(addRole);
            if (RoleResult.StatusCode == 200)
            {
                _notify.Success(RoleResult.MsgText);
                return RedirectToAction("Index", "AgentRoles");
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                ViewBag.Error = RoleResult.MsgText;
                return RedirectToAction("Index", "AgentRoles");
            }
        }
    }

    [HttpGet]
    public async Task<IActionResult> UpdateRole(int id,string AgentCode)
    {
        var role = await _roleServices.GetAppRoleById(id,AgentCode);
        var mappeddata = _mapper.Map<UpdateRoleVm>(role);
        return await Task.FromResult(PartialView(mappeddata));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateRole([FromForm] UpdateRoleVm updateRole)
    {
        if (!ModelState.IsValid)
        {
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return PartialView();
        }
        else
        {
            var RoleResult = await _roleServices.UpdateRoleAsync(updateRole);
            if (RoleResult.StatusCode == 200)
            {
                _notify.Success(RoleResult.MsgText);
                return Ok();
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                ViewBag.Error = RoleResult.MsgText;
                return PartialView();
            }
        }
    }

    [HttpGet]
    public async Task<IActionResult> DeleteRole(int id, string AgentCode)
    {
        var role = await _roleServices.GetAppRoleById(id, AgentCode);
        var mappeddata = _mapper.Map<UpdateRoleVm>(role);
        return await Task.FromResult(PartialView("_AgentRoleConfirmation", mappeddata));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteRole([FromForm] UpdateRoleVm roleVm)
    {
        if (roleVm.Id != 0)
        {
            var RolesResponse = await _roleServices.RemoveRoleAsync(roleVm.Id, roleVm.AgentCode);
            if (RolesResponse.StatusCode == 200)
            {
                _notify.Success(RolesResponse.MsgText);
                return RedirectToAction("Index", "AgentRoles");
            }
            else
            {
                //Response.StatusCode = (int)HttpStatusCode.BadRequest;
                //TempData["Error"] = RolesResponse.MsgText;
                _notify.Error(RolesResponse.MsgText);
                return RedirectToAction("Index", "AgentRoles");
            }
        }
        Response.StatusCode = (int)HttpStatusCode.NotFound;
        return PartialView();
    }

    public async Task<IActionResult> UpdateMenuPermission(int RoleId)
    {
        var role = await _commonddl.GetAgentEmployeeRoleddl();
        ViewBag.Role = new SelectList(role.Where(x => x.value == RoleId.ToString()), "value", "Text");
        var data = await _roleServices.GetListcontrollerActionAsync(RoleId);
        //data = data.Where(x => x.Area == "Agent").ToList();
        ViewBag.Menu = data;
        return PartialView("_addMenuPermission");
    }

    [HttpPost]
    public async Task<IActionResult> UpdateMenuPermission(AddcontrollerAction test)
    {
        var role = await _commonddl.GetAgentEmployeeRoleddl();
        ViewBag.Role = new SelectList(role.Where(x => x.value == test.RoleId.ToString()), "value", "Text");
        var response = await _roleServices.AddmenuPermission(test);
        if (response.StatusCode == 200)
        {
            _notify.Success(response.MsgText);
            return Ok();
        }
        var data = await _roleServices.GetListcontrollerActionAsync(2);
        data = data.Where(x => x.Area == "Agent");
        ViewBag.Menu = data;
        return PartialView("_addMenuPermission");
    }
}
