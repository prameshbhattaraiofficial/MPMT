using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Mpmt.Web.Areas.Partner.Components
{
    /// <summary>
    /// The top menu bar view component.
    /// </summary>
    public class PartnerTopMenuBarViewComponent : ViewComponent
    {
        private readonly ClaimsPrincipal _loggedInUser;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PartnerTopMenuBarViewComponent(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _loggedInUser = _httpContextAccessor.HttpContext?.User;
        }

        /// <summary>
        /// Invokes the async.
        /// </summary>
        /// <returns>A Task.</returns>
        public async Task<IViewComponentResult> InvokeAsync()
        {
            ViewBag.PartnerName = _loggedInUser.FindFirstValue(ClaimTypes.Name);
            return await Task.FromResult(View());
        }
    }
}
