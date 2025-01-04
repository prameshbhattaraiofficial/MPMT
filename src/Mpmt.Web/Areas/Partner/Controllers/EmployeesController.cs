using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Mpmt.Core.Dtos.DropDown;
using Mpmt.Core.Dtos.PartnerEmployee;
using Mpmt.Core.Events;
using Mpmt.Core.ViewModel.AdminUser;
using Mpmt.Services.Authentication;
using Mpmt.Services.Partner.IService;
using Mpmt.Services.Services.RoleMenuPermission;
using Mpmt.Web.Common;
using Mpmt.Web.Filter;
using System.Data;
using System.Net;
using static Mpmt.Web.Areas.Partner.Controllers.EmployeesController;

namespace Mpmt.Web.Areas.Partner.Controllers;

[PartnerAuthorization]
[RolePremission]
public class EmployeesController : BasePartnerController
{
    private readonly IEventPublisher _eventPublisher;
    private readonly IRMPService _rMPService;
    private readonly IPartnerEmployeeService _partnerEmployeeService;
    private readonly INotyfService _notyfService;
    private readonly IMapper _mapper;

    public EmployeesController(
        IEventPublisher eventPublisher,
        IRMPService rMPService,
        IPartnerEmployeeService partnerEmployeeService, INotyfService notyfService, IMapper mapper)
    {
        _eventPublisher = eventPublisher;
        _rMPService = rMPService;
        _partnerEmployeeService = partnerEmployeeService;
        _notyfService = notyfService;
        _mapper = mapper;
    }

    public async Task<IActionResult> Index()
    {
        var actions = await _rMPService.GetPartnerActionPermissionList("Employees");
        ViewBag.actions = actions;

        var PartnerId = User.Claims.FirstOrDefault(x => x.Type == "PartnerCode").Value;
        var result = await _partnerEmployeeService.GetPartnerEmployeeAsync(PartnerId);
        if (WebHelper.IsAjaxRequest(Request))
            return await Task.FromResult(PartialView("_EmployeeListIndex", result));
        return await Task.FromResult(View(result));
    }
    #region AssignTorole
    [HttpGet]
    public async Task<IActionResult> AssignUserToRole(int id)
    {
        var PartnerId = User.Claims.FirstOrDefault(x => x.Type == "PartnerCode").Value;
        var result = await _partnerEmployeeService.GetPartnerEmployeeByIdAsync(id, PartnerId);
        ViewBag.UserName = result.UserName;
        //ViewBag.FullName = result.FirstName + " " + result.SurName;
        ViewBag.FullName = $"{result.FirstName} {result.SurName}";
        var UserRoleById = await _partnerEmployeeService.GetPartnerEmployeeRolesByIdAsync(id, PartnerId);
        var roleList = await _partnerEmployeeService.GetPartnerEmployeeRolesAsync(PartnerId);
        AssignUserRoleDto roleDto = new AssignUserRoleDto();
        roleDto.user_id = id;
        var data = new List<SelectListItem>();
        var roles = new List<int>();
        foreach (var userrole in UserRoleById)
        {
            roles.Add(int.Parse(userrole.value));
        }
        foreach (var role in roleList)
        {
            if (roles.Contains(int.Parse(role.value)))
            {
                data.Add(new SelectListItem() { Value = role.value, Text = role.Text, Selected = true });
            }
            else
            {
                data.Add(new SelectListItem() { Value = role.value, Text = role.Text, Selected = false });
            }
        }

        ViewBag.Roles = data;
        //ViewBag.Roles = new SelectList(roleList, "value", "Text", UserRoleById);


        ViewBag.Error = TempData["Error"] == null ? "" : TempData["Error"];
        return PartialView("AssignRoles", roleDto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AssignUserToRole([FromForm] AssignUserRoleDto AssignuserroleDto)
    {
        var PartnerId = User.Claims.FirstOrDefault(x => x.Type == "PartnerCode").Value;
        var value = await _partnerEmployeeService.GetPartnerEmployeeByIdAsync(AssignuserroleDto.user_id, PartnerId);

        ViewBag.UserName = value.UserName;
        ViewBag.FullName = $"{value.FirstName} {value.SurName}";

        var UserRoleById = await _partnerEmployeeService.GetPartnerEmployeeRolesByIdAsync(AssignuserroleDto.user_id, PartnerId);
        var roleList = await _partnerEmployeeService.GetPartnerEmployeeRolesAsync(PartnerId);
        AssignUserRoleDto roleDto = new AssignUserRoleDto();
        roleDto.user_id = AssignuserroleDto.user_id;
        var data = new List<SelectListItem>();
        var roles = new List<int>();
        foreach (var userrole in UserRoleById)
        {
            roles.Add(int.Parse(userrole.value));
        }
        foreach (var role in roleList)
        {
            if (roles.Contains(int.Parse(role.value)))
            {
                data.Add(new SelectListItem() { Value = role.value, Text = role.Text, Selected = true });
            }
            else
            {
                data.Add(new SelectListItem() { Value = role.value, Text = role.Text, Selected = false });
            }
        }

        ViewBag.Roles = data;

        if (!ModelState.IsValid)
        {
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return PartialView("AssignRoles", AssignuserroleDto);
        }

        //var PartnerId = User.Claims.FirstOrDefault(x => x.Type == "PartnerCode").Value;
        var result = await _partnerEmployeeService.AssignUserRole(PartnerId, AssignuserroleDto.user_id, AssignuserroleDto.roleid);

        if (result.StatusCode != 200)
        {
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            ViewBag.Error = result.MsgText;
            return PartialView("AssignRoles", AssignuserroleDto);
        }

        _notyfService.Success(result.MsgText);

        // expire employee auth session
        var user = await _partnerEmployeeService.GetPartnerEmployeeByIdAsync(AssignuserroleDto.user_id, PartnerId);
        if (user is not null)
            await _eventPublisher.PublishAsync(new EntityUpdatedEvent<SessionAuthExpiration>(
                    new SessionAuthExpiration { UserUniqueId = user.UserGuid.ToString(), ExpireBefore = DateTime.UtcNow }));

        return Ok();
    }
    #endregion
    #region Add-Partner-Employee
    public async Task<IActionResult> AddPartnerEmployee()
    {
        return await Task.FromResult(PartialView());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddPartnerEmployee(IUDPartnerEmployee employee)
    {
        if (!ModelState.IsValid)
        {
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return PartialView();
        }
        else
        {
            var PartnerId = User.Claims.FirstOrDefault(x => x.Type == "PartnerCode").Value;
            employee.PartnerCode = PartnerId;
            var responseStatus = await _partnerEmployeeService.AddPartnerEmployeeAsync(employee);
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
    #endregion

    #region Update-Partner-Employee
    public async Task<IActionResult> UpdatePartnerEmployee(int id)
    {
        var PartnerId = User.Claims.FirstOrDefault(x => x.Type == "PartnerCode").Value;
        var result = await _partnerEmployeeService.GetPartnerEmployeeByIdAsync(id, PartnerId);
        var mappedData = _mapper.Map<IUDPartnerEmployee>(result);
        return await Task.FromResult(PartialView(mappedData));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdatePartnerEmployee(IUDPartnerEmployee employee)
    {
        if (!ModelState.IsValid)
        {
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return PartialView();
        }

        var PartnerId = User.Claims.FirstOrDefault(x => x.Type == "PartnerCode").Value;
        employee.PartnerCode = PartnerId;
        var responseStatus = await _partnerEmployeeService.UpdatePartnerEmployeeAsync(employee);

        if (responseStatus.StatusCode != 200)
        {
            Response.StatusCode = (int)(HttpStatusCode.BadRequest);
            ViewBag.Error = responseStatus.MsgText;
            return PartialView();
        }

        _notyfService.Success(responseStatus.MsgText);

        // expire employee auth session
        var user = await _partnerEmployeeService.GetPartnerEmployeeByIdAsync(employee.Id, PartnerId);
        if (user is not null)
            await _eventPublisher.PublishAsync(new EntityUpdatedEvent<SessionAuthExpiration>(
                    new SessionAuthExpiration { UserUniqueId = user.UserGuid.ToString(), ExpireBefore = DateTime.UtcNow }));

        return Ok();
    }
    #endregion

    #region Delete-Partner-Employee
    public async Task<IActionResult> DeletePartnerEmployee(int id)
    {
        var PartnerId = User.Claims.FirstOrDefault(x => x.Type == "PartnerCode").Value;
        var result = await _partnerEmployeeService.GetPartnerEmployeeByIdAsync(id, PartnerId);
        var mappedData = _mapper.Map<IUDPartnerEmployee>(result);

        return await Task.FromResult(PartialView(mappedData));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeletePartnerEmployee(IUDPartnerEmployee employee)
    {
        if (employee.Id < 1)
        {
            Response.StatusCode = (int)HttpStatusCode.NotFound;
            return PartialView();
        }

        var PartnerId = User.Claims.FirstOrDefault(x => x.Type == "PartnerCode").Value;
        employee.PartnerCode = PartnerId;
        var responseStatus = await _partnerEmployeeService.DeletePartnerEmployeeAsync(employee);
        
        if (responseStatus.StatusCode != 200)
        {
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            TempData["Error"] = responseStatus.MsgText;
            return PartialView();
        }

        _notyfService.Success(responseStatus.MsgText);

        // expire employee auth session
        var user = await _partnerEmployeeService.GetPartnerEmployeeByIdAsync(employee.Id, PartnerId);
        if (user is not null)
            await _eventPublisher.PublishAsync(new EntityUpdatedEvent<SessionAuthExpiration>(
                    new SessionAuthExpiration { UserUniqueId = user.UserGuid.ToString(), ExpireBefore = DateTime.UtcNow }));

        return Ok();
    }
    #endregion

    public IActionResult RoleList()
    {
        return View();
    }
}
