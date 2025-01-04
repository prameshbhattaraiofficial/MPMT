using Microsoft.AspNetCore.Mvc;
using Mpmt.Agent.Models;
using Mpmt.Services.Services.AgentDashboardService;
using System.Diagnostics;
using System.Security.Claims;

namespace Mpmt.Agent.Controllers
{
    public class HomeController : AgentBaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAgentDashboardService _agentDashboardService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ClaimsPrincipal _loggedInUser;

        public HomeController(ILogger<HomeController> logger, IAgentDashboardService agentDashboardService, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _agentDashboardService = agentDashboardService;
            _httpContextAccessor = httpContextAccessor;
            _loggedInUser = httpContextAccessor.HttpContext.User;
        }

        public async Task<IActionResult> Index()
        {
            var agentCode = _loggedInUser.FindFirstValue("AgentCode");
            var data = await _agentDashboardService.GetAgentDashBoard(agentCode);
            return View(data);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}