using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Mpmt.Agent.Common;
using Mpmt.Agent.Filter;
using Mpmt.Core.Dtos.CashAgent;
using Mpmt.Core.Dtos.DropDown;
using Mpmt.Core.Dtos.SuperAgent;
using Mpmt.Core.ViewModel.CashAgent;
using Mpmt.Services.CashAgents;
using Mpmt.Services.Services.Common;
using Mpmt.Services.Services.RoleMenuPermission;
using Mpmt.Services.Services.Roles;
using System.Net;

namespace Mpmt.Agent.Controllers;

[RolePremission]
public class EmployeeManagementController : AgentBaseController
{
    private readonly ICashAgentEmployee _cashAgentEmployee;
    private readonly IAgentRoleServices _agentRoleServices;
    private readonly IMapper _mapper;
    private readonly IRMPService _rMPService;
    private readonly ICommonddlServices _commonddl;
    private readonly INotyfService _notyfService;

    public EmployeeManagementController(
        ICashAgentEmployee cashAgentEmployee,
        IMapper mapper, INotyfService notyfService,
        ICommonddlServices commonddl,
        IAgentRoleServices agentRoleServices, IRMPService rMPService)
    {
        _cashAgentEmployee = cashAgentEmployee;
        _mapper = mapper;
        _notyfService = notyfService;
        _commonddl = commonddl;
        _agentRoleServices = agentRoleServices;
        _rMPService = rMPService;
    }

    [HttpGet]
    public async Task<IActionResult> Index([FromQuery] AgentFilter AgentFilter)
    {
        var actions = await _rMPService.GetAgentActionPermissionList("EmployeeManagement");
        ViewBag.actions = actions;

        var data = await _cashAgentEmployee.getAgentEmployeeList(AgentFilter);
        if (WebHelper.IsAjaxRequest(Request))
        {
            return PartialView("_EmployeeList", data);
        }
        return View(data);
    }

    [HttpGet]
    public async Task<IActionResult> AddEmployee()
    {
        return await Task.FromResult(PartialView());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddEmployee([FromForm] CashAgentEmployeeVm CashAgentVm)
    {
        if (!ModelState.IsValid)
        {
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return PartialView(CashAgentVm);
        }

        var response = await _cashAgentEmployee.AddAgentEmployeeUserAsync(CashAgentVm);
        if (!response.Success)
        {
            Response.StatusCode = (int)(HttpStatusCode.BadRequest);
            ViewBag.Error = response.Errors.First();
            return PartialView(CashAgentVm);
        }

        _notyfService.Success(response.Message);
        return Ok();
    }

    [AcceptVerbs("GET", "POST")]
    public async Task<IActionResult> VerifyUserName(string userName)
    {
        if (!await _cashAgentEmployee.VerifyUserName(userName))
        {
            return Json($"User Name {userName} is already in use.");
        }
        return Json(true);
    }

    [HttpGet]
    public async Task<IActionResult> UpdateEmployee(string EmployeeId)
    {
        var AgentCode = User.Claims.FirstOrDefault(x => x.Type == "AgentCode").Value;
        var getUpdatedList = await _cashAgentEmployee.GetAgentEmployeeUserByEmployeeIdAsync(EmployeeId, AgentCode);
        var mapObject = MapToCashAgentEmployeeVm(getUpdatedList);
        return await Task.FromResult(PartialView(mapObject));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateEmployee([FromForm] CashAgentEmployeeVm CashAgentVm)
    {
        if (!ModelState.IsValid)
        {
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return PartialView(CashAgentVm);
        }

        var response = await _cashAgentEmployee.UpdateAgentEmployeeUserAsync(CashAgentVm);
        _notyfService.Success(response.Message);

        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> ActivateUser(string EmployeeId, bool isActive, string userName)
    {
        var user = new ActivateAgentEmployee
        {
            EmployeeId = EmployeeId,
            IsActive = isActive,
            UserName = userName
        };

        return await Task.FromResult(PartialView(user));
    }

    [HttpPost]
    public async Task<IActionResult> ActivateUser(ActivateAgentEmployee activateAgentEmployee)
    {
        if (activateAgentEmployee != null && !string.IsNullOrEmpty(activateAgentEmployee.EmployeeId))
        {
            var ResponseStatus = await _cashAgentEmployee.ActivateAgentUserAsync(activateAgentEmployee);
            if (ResponseStatus.StatusCode == 200)
            {
                _notyfService.Success(ResponseStatus.MsgText);
                return Ok();
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                TempData["Error"] = ResponseStatus.MsgText;
                return PartialView(activateAgentEmployee);
            }
        }

        Response.StatusCode = (int)HttpStatusCode.NotFound;
        return PartialView(activateAgentEmployee);
    }

    public async Task<IActionResult> DeleteUser(string EmployeeId, string UserName, string AgentCode)
    {
        var user = new ActivateAgentEmployee()
        {
            EmployeeId = EmployeeId,
            AgentCode = AgentCode,
            UserName = UserName
        };

        return await Task.FromResult(PartialView(user));
    }

    [HttpPost]
    public async Task<IActionResult> DeleteUser(ActivateAgentEmployee activateAgentEmployee)
    {
        if (activateAgentEmployee != null && !string.IsNullOrEmpty(activateAgentEmployee.EmployeeId))
        {
            var ResponseStatus = await _cashAgentEmployee.DeleteUser(activateAgentEmployee);
            if (ResponseStatus.StatusCode == 200)
            {
                _notyfService.Success(ResponseStatus.MsgText);
                return Ok();
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                ViewBag.Error = ResponseStatus.MsgText;
                return PartialView(activateAgentEmployee);
            }
        }

        Response.StatusCode = StatusCodes.Status404NotFound;
        return PartialView(activateAgentEmployee);
    }

    private CashAgentEmployeeVm MapToCashAgentEmployeeVm(AgentUser details)
    {
        ArgumentNullException.ThrowIfNull(details, nameof(details));
        return new CashAgentEmployeeVm
        {
            Id = details.EmployeeId,
            UserName = details.UserName,
            FirstName = details.FirstName,
            SuperAgentCode = details.SuperAgentCode,
            LastName = details.LastName,
            Email = details.Email,
            LicenseDocImgPath = details.LicenseDocImgPath,
            ContactNumber = details.ContactNumber,
            IsActive = details.IsActive,
        };
    }

    [HttpGet]
    public async Task<IActionResult> AssignUserToRole(int id, string fullname, string email)
    {
        AssignUserRoleDto roleDto = new AssignUserRoleDto();
        roleDto.user_id = id;
        ViewBag.Email = email;
        ViewBag.FullName = fullname;

        var UserRoleById = await _commonddl.GetAgentEmployeeRolesByIdAsync(id);
        var admiroleddl = await _commonddl.GetAgentEmployeeRoleddl();

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
        return PartialView("_AssignRole", roleDto);
    }

    [HttpPost]
    public async Task<IActionResult> AssignUserToRole([FromForm] AssignUserRoleDto AssignuserroleDto)
    {
        if (!ModelState.IsValid)
        {
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return PartialView("_AssignRole", AssignuserroleDto);
        }

        var result = await _agentRoleServices.AssignUserRole(AssignuserroleDto.user_id, AssignuserroleDto.roleid);
        if (result.StatusCode == 200)
        {
            _notyfService.Success(result.MsgText);
            return Ok();
        }

        Response.StatusCode = (int)HttpStatusCode.BadRequest;
        ViewBag.Error = result.MsgText;

        return PartialView("_AssignRole", AssignuserroleDto);
    }
}