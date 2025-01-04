using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Mpmt.Core.Dtos.AgentApplications;
using Mpmt.Services.Services.AgentApplications;
using Mpmt.Services.Services.RoleMenuPermission;
using Mpmt.Web.Common;
using Mpmt.Web.Filter;

namespace Mpmt.Web.Areas.Admin.Controllers;

[RolePremission]
[AdminAuthorization]
public class AgentApplicationsController : BaseAdminController
{
    private readonly IMapper _mapper;
    private readonly IRMPService _rMPService;
    private readonly IAgentApplicationsService _agentApplicationsService;

    public AgentApplicationsController(IMapper mapper, IRMPService rMPService, IAgentApplicationsService agentApplicationsService)
    {
        _mapper = mapper;
        _rMPService = rMPService;
        _agentApplicationsService = agentApplicationsService;
    }

    [HttpGet]
    public async Task<IActionResult> AgentApplicationsIndex([FromQuery] AgentApplicationsFilter requestFilter)
    {
        Response.Headers["Cache-Control"] = "no-store, no-cache, must-revalidate";
        Response.Headers["Pragma"] = "no-cache";
        Response.Headers["Expires"] = "0";

        var actions = await _rMPService.GetActionPermissionListAsync("AgentApplications");
        ViewBag.actions = actions;

        var data = await _agentApplicationsService.GetAgentApplicationsAsync(requestFilter);
        if (WebHelper.IsAjaxRequest(Request))
        {
            return PartialView("_AgentApplicationsList", data);
        }
        return View(data);
    }

    [HttpGet]
    public async Task<IActionResult> ViewAgentApplicationsDetail(AgentApplicationsModel model)
    {
        return PartialView("_ViewAgentApplicationsDetail", model);
    }
}
