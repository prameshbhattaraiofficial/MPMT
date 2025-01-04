using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc;
using Mpmt.Agent.Common;
using Mpmt.Core.Dtos.CashAgent;
using Mpmt.Services.CashAgents;
using Mpmt.Services.Partner;
using System.Security.Claims;

namespace Mpmt.Agent.Controllers
{
    public class AgentFundRequestController : AgentBaseController
    {

        private readonly IAgentReportService _agentrReportServices;
        public AgentFundRequestController(IAgentReportService agentReportServices)
        {
            _agentrReportServices = agentReportServices;
   
        }

        public async Task<IActionResult> Index(AgentFundRequestFilter model)
        {

            var details = await _agentrReportServices.GetAgentFundRequestDetailsAsync(model);           
            if (WebHelper.IsAjaxRequest(Request))
                return await Task.FromResult(PartialView("_AgentFundRequestList", details));
            return await Task.FromResult(View(details));         
        }

      
    }
}
