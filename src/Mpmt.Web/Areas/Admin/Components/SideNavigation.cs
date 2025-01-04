using Microsoft.AspNetCore.Mvc;
using Mpmt.Services.Services.RoleMenuPermission;

namespace Mpmt.Web.Areas.Admin.Components
{
    /// <summary>
    /// The side navigation view component.
    /// </summary>
    public class SideNavigationViewComponent : ViewComponent
    {
        private readonly IRMPService _rmpService;

        public SideNavigationViewComponent(IRMPService rmpService)
        {
            _rmpService = rmpService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            string name = "admin";
            var menusList = await _rmpService.GetMenusSubmenusForCurrentUserAsync(name);
            return await Task.FromResult(View("Default", menusList));
        }
    }
}
