using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Mpmt.Web.Areas.Admin.Components
{
    /// <summary>
    /// The top menu bar view component.
    /// </summary>
    public class TopMenuBarViewComponent : ViewComponent
    {
        /// <summary>
        /// Invokes the async.
        /// </summary>
        /// <returns>A Task.</returns>

        private readonly ClaimsPrincipal _loggedInUser;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TopMenuBarViewComponent(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _loggedInUser = _httpContextAccessor.HttpContext?.User;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            ViewBag.AgentName = _loggedInUser.FindFirstValue(ClaimTypes.Name);

            return await Task.FromResult(View());
        }
    }
}
