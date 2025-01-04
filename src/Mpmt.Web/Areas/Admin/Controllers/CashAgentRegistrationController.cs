using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Mpmt.Core.Dtos.CashAgent;
using Mpmt.Services.CashAgents;
using Mpmt.Web.Common;
using Mpmt.Web.Filter;
using System.Net;

namespace Mpmt.Web.Areas.Admin.Controllers;

[RolePremission]
[AdminAuthorization]
public class CashAgentRegistrationController : BaseAdminController
{
    private readonly ICashAgentUserService _cashAgentUserService;
    private readonly INotyfService _notyfService;

    public CashAgentRegistrationController(ICashAgentUserService cashAgentUserService, INotyfService notyfService)
    {
        _cashAgentUserService = cashAgentUserService;
        _notyfService = notyfService;
    }

    [HttpGet]
    public async Task<IActionResult> Index([FromQuery] AgentRegisterFilter request)
    {
        var data = await _cashAgentUserService.GetRemitAgentRegisterAsync(request);
        if (WebHelper.IsAjaxRequest(Request))
        {
            return PartialView("_CashAgentRegistrationList", data);
        }
        return View(data);
    }

    [HttpGet]
    public async Task<IActionResult> ApprovedCashAgentRegistration(Guid id, string email)
    {
        return await Task.FromResult(PartialView(new CashAgentRegister { Id = id, Email = email }));
    }

    [HttpPost]
    [LogUserActivity("approved cash agent registration")]
    public async Task<IActionResult> ApprovedCashAgentRegistration(CashAgentRegister remitPartner)
    {
        var request = new CashAgentRequest { Id = remitPartner.Id, Email = remitPartner.Email };
        var ResponseStatus = await _cashAgentUserService.ApprovedAgentRequest(request, User);
        if (ResponseStatus.StatusCode == 200)
        {
            _notyfService.Success(ResponseStatus.MsgText);
            return Ok();
        }
        else
        {
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            ViewBag.Error = ResponseStatus.MsgText;
            return PartialView(remitPartner);
        }
    }

    [HttpGet]
    public async Task<IActionResult> RejectCashAgentRegistration(Guid id, string email)
    {
        return await Task.FromResult(PartialView(new CashAgentRegister { Id = id, Email = email }));
    }

    [HttpPost]
    [LogUserActivity("rejected cash agent registration")]
    public async Task<IActionResult> RejectCashAgentRegistration(CashAgentRegister remitPartner)
    {
        var request = new CashAgentRequest { Id = remitPartner.Id, Email = remitPartner.Email };
        var ResponseStatus = await _cashAgentUserService.RejectAgentRequest(request, User);
        if (ResponseStatus.StatusCode == 200)
        {
            _notyfService.Success(ResponseStatus.MsgText);
            return Ok();
        }
        else
        {
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            ViewBag.Error = ResponseStatus.MsgText;
            return PartialView(remitPartner);
        }
    }

    [HttpGet]
    public async Task<IActionResult> CashAgentRegistrationDetails(string Email, string phone)
    {
        var agentDetail = await _cashAgentUserService.GetAgentDetail(Email,phone);
        return PartialView(agentDetail);
    }
}
