using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Mpmt.Core.Models.Route;
using Mpmt.Core.ViewModel.Role;
using Mpmt.Services.Partner.IService;
using Mpmt.Services.Services.Common;
using Mpmt.Services.Services.RoleMenuPermission;
using Mpmt.Web.Common;
using Mpmt.Web.Filter;
using System.Net;

namespace Mpmt.Web.Areas.Admin.Controllers;

public class AdminPartnerRoleController : BaseAdminController
{
    private readonly INotyfService _notyfService;
    private readonly IPartnerRoleServices _roleService;
    private readonly IMapper _mapper;
    private readonly ICommonddlServices _commonddl;
    private readonly IRMPService _rMPService;

    public AdminPartnerRoleController(INotyfService notyfService, IPartnerRoleServices roleService, ICommonddlServices commonddl, IRMPService rMPService, IMapper mapper)
    {
        _notyfService = notyfService;
        _roleService = roleService;
        _commonddl = commonddl;
        _rMPService = rMPService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var role = await _roleService.GetPartnerRoleAsync();
        if (WebHelper.IsAjaxRequest(Request))
            return await Task.FromResult(PartialView("_PartnerRoleIndex", role));
        return await Task.FromResult(View(role));
    }

    [HttpGet]
    public async Task<IActionResult> AddPartnerRole()
    {
        return await Task.FromResult(PartialView());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [LogUserActivity("added partner role")]

    public async Task<IActionResult> AddPartnerRole(AddAdminPartnerRoleVm adminPartnerRoleVm)
    {
        if (!ModelState.IsValid)
        {
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return PartialView();
        }
        else
        {
            var roleResult = await _roleService.AddPartnerRoleAsync(adminPartnerRoleVm, User);
            if (roleResult.StatusCode == 200)
            {
                _notyfService.Success(roleResult.MsgText);
                return Ok();
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                ViewBag.Error = roleResult.MsgText;
                return PartialView(adminPartnerRoleVm);
            }
        }
    }

    [HttpGet]
    public async Task<IActionResult> UpdatePartnerRole(int id)
    {
        var role = await _roleService.GetAdminPartnerRoleById(id);
        var mappedData = _mapper.Map<UpdateAdminPartnerRoleVm>(role);
        return await Task.FromResult(PartialView(mappedData));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [LogUserActivity("updated partner role")]

    public async Task<IActionResult> UpdatePartnerRole(UpdateAdminPartnerRoleVm adminPartnerRoleVm)
    {
        if (!ModelState.IsValid)
        {
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return PartialView();
        }
        else
        {
            var roleResult = await _roleService.UpdatePartnerRoleAsync(adminPartnerRoleVm, User);
            if (roleResult.StatusCode == 200)
            {
                _notyfService.Success(roleResult.MsgText);
                return Ok();
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                ViewBag.Error = roleResult.MsgText;
                return PartialView(adminPartnerRoleVm);
            }
        }
    }

    [HttpGet]
    public async Task<IActionResult> DeletePartnerRole(int id)
    {
        var role = await _roleService.GetAdminPartnerRoleById(id);
        var mappedData = _mapper.Map<UpdateAdminPartnerRoleVm>(role);
        return await Task.FromResult(PartialView(mappedData));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [LogUserActivity("deleted partner role")]

    public async Task<IActionResult> DeletePartnerRole(UpdateAdminPartnerRoleVm adminPartnerRoleVm)
    {
        if (!ModelState.IsValid)
        {
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return PartialView();
        }
        else
        {
            var roleResult = await _roleService.DeletePartnerRoleAsync(adminPartnerRoleVm, User);
            if (roleResult.StatusCode == 200)
            {
                _notyfService.Success(roleResult.MsgText);
                return Ok();
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                ViewBag.Error = roleResult.MsgText;
                return PartialView(adminPartnerRoleVm);
            }
        }
    }

    [HttpGet]
    public async Task<IActionResult> PartnerMenuPermission(int RoleId)
    {
        var role = await _commonddl.GetAdminPartnerRoleddl(RoleId);
        ViewBag.PartnerRole = new SelectList(role.Where(x => x.value == RoleId.ToString()), "value", "Text");
        var data = await _rMPService.GetPartnerMenuListControllerAction(RoleId);
        data = data.Where(x => x.Area == "Partner" && x.IsActive).ToList();
        ViewBag.Menu = data;
        return PartialView("_PartnerMenuPermission");
    }

    [HttpPost]
    [LogUserActivity("changed partner menu permission")]

    public async Task<IActionResult> PartnerMenuPermission(AddcontrollerAction test)
    {
        var role = await _commonddl.GetAdminPartnerRoleddl(test.RoleId);
        ViewBag.PartnerRole = new SelectList(role.Where(x => x.value == test.RoleId.ToString()), "value", "Text");
        var response = await _rMPService.PartnerMenuPermission(test);
        if (response.StatusCode == 200)
        {
            _notyfService.Success(response.MsgText);
            return Ok();
        }
        var data = await _rMPService.GetPartnerMenuListControllerAction(test.RoleId);
        data = data.Where(x => x.Area == "Partner" && x.IsActive).ToList();
        ViewBag.Menu = data;
        return PartialView("_PartnerMenuPermission");
    }
}
