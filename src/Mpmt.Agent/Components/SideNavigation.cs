using Microsoft.AspNetCore.Mvc;
using Mpmt.Core.Models.RMP;
using Mpmt.Core.ViewModel.CashAgent;
using Mpmt.Services.CashAgents;

namespace Mpmt.Agent.Components
{
    /// <summary>
    /// The side navigation view component.
    /// </summary>
    public class SideNavigationViewComponent : ViewComponent
    {
        private readonly IAgentMenuService _agentMenuService;

        public SideNavigationViewComponent(IAgentMenuService agentMenuService)
        {
            _agentMenuService = agentMenuService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
           
            var menusList = await _agentMenuService.GetMenusSubmenusForCurrentUserByUser();

            return await Task.FromResult(View("Default", menusList));
        }
    }
}
