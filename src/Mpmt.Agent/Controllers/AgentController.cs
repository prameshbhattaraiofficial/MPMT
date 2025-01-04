using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Mpmt.Agent.Common;
using Mpmt.Agent.Filter;
using Mpmt.Core.Common;
using Mpmt.Core.Dtos.CashAgent;
using Mpmt.Core.Dtos.CashAgent.Commission;
using Mpmt.Core.Dtos.SuperAgent;
using Mpmt.Core.Extensions;
using Mpmt.Core.ViewModel.CashAgent;
using Mpmt.Core.ViewModel.SuperAgent;
using Mpmt.Services.CashAgents;
using Mpmt.Services.Services.Common;
using Mpmt.Services.Services.RoleMenuPermission;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Net;
using System.Security.Claims;

namespace Mpmt.Agent.Controllers;

[RolePremission]
public class AgentController : AgentBaseController
{
    private readonly ICashAgentUserService _cashAgentUserService;
    private readonly ICommonddlServices _commonddlService;
    private readonly ICashAgentEmployee _cashAgentEmployee;
    private readonly IRMPService _rMPService;
    private readonly INotyfService _notyfService;
    private readonly ICashAgentCommissionService _cashAgentCommissionService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ClaimsPrincipal _loggedInUser;
    private readonly IMapper _mapper;

    public AgentController(
        ICashAgentUserService cashAgentUserService,
        ICommonddlServices commonddlService,
        IMapper mapper,
        INotyfService notyfService,
        IHttpContextAccessor httpContextAccessor,
        ICashAgentEmployee cashAgentEmployee,
        ICashAgentCommissionService cashAgentCommissionService,
        IRMPService rMPService)
    {
        _cashAgentUserService = cashAgentUserService;
        _commonddlService = commonddlService;
        _mapper = mapper;
        _notyfService = notyfService;
        _httpContextAccessor = httpContextAccessor;
        _loggedInUser = httpContextAccessor.HttpContext.User;
        _cashAgentEmployee = cashAgentEmployee;
        _cashAgentCommissionService = cashAgentCommissionService;
        _rMPService = rMPService;
    }

    [HttpGet]
    public async Task<IActionResult> Index([FromQuery] AgentFilterModel agentFilter)
    {
        var actions = await _rMPService.GetAgentActionPermissionList("Agent");
        ViewBag.actions = actions;

        var districtList = await _commonddlService.GetAllDistrictddl();
        ViewBag.District = new SelectList(districtList, "value", "Text");
        var data = await _cashAgentUserService.GetAgentAsync(agentFilter);
        if (WebHelper.IsAjaxRequest(Request))
        {
            return PartialView("_AgentList", data);
        }
        return View(data);
    }

    public async Task<IActionResult> AgentLedger(AgentLedgerFilter AgentLedgerFilter)
    {
        AgentLedgerFilter.AgentCode = _loggedInUser.FindFirstValue("AgentCode");
        var data = await _cashAgentUserService.GetAgentLedgerAsync(AgentLedgerFilter);
        ViewBag.AgentCode = AgentLedgerFilter.AgentCode;

        if (WebHelper.IsAjaxRequest(Request))
            return PartialView("_AgentLedger", data);

        return View(data);
    }

    [HttpGet("ExportAgentLedgerToCsv")]
    public async Task<IActionResult> ExportAgentLedgerToCsv(AgentLedgerFilter request)
    {
        request.Export = 1;
        var result = await _cashAgentUserService.GetAgentLedgerAsync(request);
        var data = result.Items;
        var (bytes, fileformate, filename) = ExportHelper.GenerateCsv(data, new string[] { }, null, "AgentLedgerReports", true);
        return File(bytes, fileformate, filename);
    }

    [HttpGet("ExportAgentLedgerToExcel")]
    public async Task<IActionResult> ExportAgentLedgerToExcel(AgentLedgerFilter request)
    {
        request.Export = 1;
        var result = await _cashAgentUserService.GetAgentLedgerAsync(request);
        var data = result.Items;
        List<DataTable> dataTables = await IEnumerableExtensions.ToDataTablesAsync(data, 500000);
        var (excelFileByteArr, fileFormat, fileName) = await ExportHelper.ToExcelAsync(dataTables, "AgentLedgerReports", true);
        return File(excelFileByteArr, fileFormat, fileName);
    }

    [HttpGet("ExportAgentLedgerToPdf")]
    public async Task<FileContentResult> ExportAgentLedgerToPdf(AgentLedgerFilter request)
    {
        request.Export = 1;
        var result = await _cashAgentUserService.GetAgentLedgerAsync(request);
        var data = result;
        var (bytedata, format) = await ExportHelper.TopdfAsync(data, "AgentLedgerReports");
        return File(bytedata, format, "AgentLedgerReport.pdf");
    }

    [HttpGet]
    public async Task<IActionResult> AddCommissionRules([Required] string AgentCode, string AgentType)
    {
        var agentCommissionDtl = await _cashAgentCommissionService.GetAgentCommissionDetailsAsync(AgentCode, AgentType);
        if (agentCommissionDtl is null)
        {
            //_notyfService.Error("Commission rules not defined for the agent!");
            return BadRequest();
        }

        var vm = new AddOrUpdateCommissionVM
        {
            AgentCode = agentCommissionDtl.AgentCode,
            SuperAgentCode = agentCommissionDtl.SuperAgentCode,
            AgentType = agentCommissionDtl.AgentType,
            CommissionRules = agentCommissionDtl?.CommissionRuleList is null || !agentCommissionDtl.CommissionRuleList.Any()
            ? new List<AgentCommissionRule> { new() { AgentCode = AgentCode, FromDate = DateTime.Now, ToDate = DateTime.Now } }
            : agentCommissionDtl.CommissionRuleList.ToList()
        };

        return PartialView("_AddCommissionRules", vm);
    }

    [HttpPost]
    public async Task<IActionResult> AddCommissionRules(AddOrUpdateCommissionVM model)
    {
        var rules = model.CommissionRules
            .Select(x => _mapper.Map<AgentCommissionRuleType>(x))
            .ToList();

        var addUpdateResult = await _cashAgentCommissionService.AddOrUpdateAsync(rules, model.AgentCode, model.SuperAgentCode, model.AgentType, "SUPERAGENT");
        if (!addUpdateResult.Success)
        {
            Response.StatusCode = (int)(HttpStatusCode.BadRequest);
            ViewBag.Error = addUpdateResult.Errors.First();

            var agentCommissionDtl = await _cashAgentCommissionService.GetAgentCommissionDetailsAsync(model.AgentCode, null);

            var vm = new AddOrUpdateCommissionVM
            {
                AgentCode = agentCommissionDtl.AgentCode ?? model.AgentCode,
                SuperAgentCode = agentCommissionDtl?.SuperAgentCode,
                AgentType = agentCommissionDtl?.AgentType,
                CommissionRules = agentCommissionDtl?.CommissionRuleList is null || !agentCommissionDtl.CommissionRuleList.Any()
                ? new List<AgentCommissionRule> { new() { AgentCode = agentCommissionDtl.AgentCode, FromDate = DateTime.Now, ToDate = DateTime.Now } }
                : agentCommissionDtl.CommissionRuleList.ToList()
            };

            return PartialView("_AddCommissionRules", vm);
        }

        _notyfService.Success(addUpdateResult.Message);
        return Ok();
    }

    public async Task<IActionResult> Profile()
    {
        var country = await _commonddlService.GetCountryddl();
        var UserName = _loggedInUser.FindFirstValue(ClaimTypes.Name);
        var userType = _loggedInUser.FindFirstValue("UserType");
        var AgentProfileDetail = await _cashAgentUserService.GetAgentByUserNameAsync(UserName);
        if (userType.ToUpper() == "EMPLOYEE")
        {
            AgentProfileDetail = await _cashAgentEmployee.GetAgentEmployeeByUserNameAsync(UserName);
        }
        ViewBag.CountryName = country.Where(c => c.value == AgentProfileDetail.CountryCode).FirstOrDefault()?.Text;
        return View(AgentProfileDetail);
    }

    [HttpGet]
    public async Task<IActionResult> ChangePassword()
    {
        return await Task.FromResult(PartialView("_ChangePassword"));
    }

    [HttpPost]
    public async Task<IActionResult> ChangePassword(AgentChangePasswordVM changepassword)
    {
        if (!ModelState.IsValid)
        {
            return PartialView(changepassword);
        }
        var responseStatus = await _cashAgentUserService.ChangeAgentPassword(changepassword);
        if (responseStatus.StatusCode == 200)
        {
            _notyfService.Success(responseStatus.MsgText);
            return Ok();
        }
        else
        {
            Response.StatusCode = (int)(HttpStatusCode.BadRequest);
            ViewBag.Error = responseStatus.MsgText;
            return PartialView(changepassword);
        }
    }

    [AcceptVerbs("GET", "POST")]
    public async Task<IActionResult> VerifyUserName(string userName)
    {
        if (!await _cashAgentUserService.VerifyUserName(userName))
        {
            return Json($"User Name {userName} is already in use.");
        }
        return Json(true);
    }

    [AcceptVerbs("GET", "POST")]
    public async Task<IActionResult> VerifyContactNumber(string contactNumber)
    {
        if (!await _cashAgentUserService.VerifyContactNumber(contactNumber))
        {
            return Json($"Contact Number is already in use.");
        }
        return Json(true);
    }

    [AcceptVerbs("GET", "POST")]
    public async Task<IActionResult> VerifyRegistrationNumber(string registrationNumber)
    {
        if (!await _cashAgentUserService.VerifyRegistrationNumber(registrationNumber))
        {
            return Json($"Registration Number is already in use.");
        }
        return Json(true);
    }

    [HttpGet]
    public async Task<IActionResult> AddAgent()
    {
        var districtList = await _commonddlService.GetAllDistrictddl();
        ViewBag.District = new SelectList(districtList, "value", "Text");
        var agentCategory = await _commonddlService.GetServiceChargeCategoryddl();
        ViewBag.AgentCategory = new SelectList(agentCategory, "value", "Text");
        return await Task.FromResult(PartialView());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddAgent([FromForm] CashAgentVm CashAgentVm)
    {
        if (!ModelState.IsValid)
        {
            var districtList = await _commonddlService.GetAllDistrictddl();
            ViewBag.District = new SelectList(districtList, "value", "Text");
            var agentCategory = await _commonddlService.GetServiceChargeCategoryddl();
            ViewBag.AgentCategory = new SelectList(agentCategory, "value", "Text");
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return PartialView(CashAgentVm);
        }
        else
        {
            var districtList = await _commonddlService.GetAllDistrictddl();
            ViewBag.District = new SelectList(districtList, "value", "Text");
            var agentCategory = await _commonddlService.GetServiceChargeCategoryddl();
            ViewBag.AgentCategory = new SelectList(agentCategory, "value", "Text");
            var mappedData = _mapper.Map<CashAgentUserVm>(CashAgentVm);
            var addResult = await _cashAgentUserService.AddAgentAsync(mappedData);
            if (!addResult.Success)
            {
                Response.StatusCode = (int)(HttpStatusCode.BadRequest);
                ViewBag.Error = addResult.Errors;
                return PartialView(CashAgentVm);
            }
            _notyfService.Success(addResult.Message);
            return Ok();
        }
    }

    [HttpGet]
    public async Task<IActionResult> UpdateAgent(string AgentCode)
    {
        var districtList = await _commonddlService.GetAllDistrictddl();
        ViewBag.AgentCode = AgentCode;
        ViewBag.District = new SelectList(districtList, "value", "Text");
        var agentCategory = await _commonddlService.GetServiceChargeCategoryddl();
        ViewBag.AgentCategory = new SelectList(agentCategory, "value", "Text");
        var getUpdatedList = await _cashAgentUserService.GetCashAgentByAgentCodeAsync(AgentCode);
        var mapObject = MapToCashAgentUserVm(getUpdatedList);
        return await Task.FromResult(PartialView(mapObject));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateAgent([FromForm] CashAgentUserVm CashAgentVm)
    {
        if (!ModelState.IsValid)
        {
            var districtList = await _commonddlService.GetAllDistrictddl();
            ViewBag.District = new SelectList(districtList, "value", "Text");
            var agentCategory = await _commonddlService.GetServiceChargeCategoryddl();
            ViewBag.AgentCategory = new SelectList(agentCategory, "value", "Text");
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return PartialView(CashAgentVm);
        }
        else
        {
            var districtList = await _commonddlService.GetAllDistrictddl();
            ViewBag.District = new SelectList(districtList, "value", "Text");
            var agentCategory = await _commonddlService.GetServiceChargeCategoryddl();
            ViewBag.AgentCategory = new SelectList(agentCategory, "value", "Text");
            var addResult = await _cashAgentUserService.UpdateAgentAsync(CashAgentVm);
            if (!addResult.Success)
            {
                Response.StatusCode = (int)(HttpStatusCode.BadRequest);
                ViewBag.Error = addResult.Errors;
                return PartialView(CashAgentVm);
            }
            _notyfService.Success(addResult.Message);
            return Ok();
        }
    }

    public async Task<IActionResult> ActivateUser(string AgentCode, bool isActive)
    {
        var user = new ActivateAgent
        {
            AgentCode = AgentCode,
            IsActive = isActive
        };

        return await Task.FromResult(PartialView(user));
    }

    [HttpPost]
    public async Task<IActionResult> ActivateUser(ActivateAgent activateAgent)
    {
        if (activateAgent != null && !string.IsNullOrEmpty(activateAgent.AgentCode))
        {
            var ResponseStatus = await _cashAgentUserService.ActivateAgentUserAsync(activateAgent);
            if (ResponseStatus.StatusCode == 200)
            {
                _notyfService.Success(ResponseStatus.MsgText);
                return Ok();
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                TempData["Error"] = ResponseStatus.MsgText;
                return PartialView(activateAgent);
            }
        }
        else
        {
            Response.StatusCode = (int)HttpStatusCode.NotFound;
            return PartialView(activateAgent);
        }
    }

    public async Task<IActionResult> DeleteUser(string AgentCode)
    {
        var user = new ActivateAgent { AgentCode = AgentCode };

        return await Task.FromResult(PartialView(user));
    }

    [HttpPost]
    public async Task<IActionResult> DeleteUser(ActivateAgent activateAgent)
    {
        if (activateAgent != null && !string.IsNullOrEmpty(activateAgent.AgentCode))
        {
            var ResponseStatus = await _cashAgentUserService.DeleteUser(activateAgent);
            if (ResponseStatus.StatusCode == 200)
            {
                _notyfService.Success(ResponseStatus.MsgText);
                return Ok();
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                ViewBag.Error = ResponseStatus.MsgText;
                return PartialView(activateAgent);
            }
        }
        else
        {
            Response.StatusCode = (int)HttpStatusCode.NotFound;
            return PartialView(activateAgent);
        }
    }

    private CashAgentUserVm MapToCashAgentUserVm(AgentUser details)
    {
        ArgumentNullException.ThrowIfNull(details, nameof(details));
        return new CashAgentUserVm
        {
            AgentCode = details.AgentCode,
            FirstName = details.FirstName,
            SuperAgentCode = details.SuperAgentCode,
            LastName = details.LastName,
            Email = details.Email,
            AgentCategoryId = details.AgentCategoryId,
            LicensedocImgPath = details.LicensedocImgPath,
            UserName = details.UserName,
            UserType = details.UserType,
            ContactNumber = details.ContactNumber,
            FullAddress = details.FullAddress,
            DistrictCode = details.DistrictCode,
            DocumentImagepath = details.CompanyLogoImgPath,
            OrganizationName = details.OrganizationName,
            RegistrationNumber = details.RegistrationNumber,
            IsActive = details.IsActive
        };
    }
}