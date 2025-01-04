using Microsoft.AspNetCore.Mvc;
using Mpmt.Core.Dtos.AdminUser;
using Mpmt.Services.Services.AdminUser;
using Mpmt.Web.Filter;
using System.Security.Claims;

namespace Mpmt.Web.Areas.Admin.Controllers
{
    //[RolePremission]
    [AdminAuthorization]
    public class ProfileController : BaseAdminController
    {
        private readonly IAdminUserServices _adminUserServices;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProfileController(IAdminUserServices adminUserServices, IHttpContextAccessor httpContextAccessor)
        {
            _adminUserServices = adminUserServices;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<IActionResult> Index()
        {
            var username = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
            var userdetail = new AdminUserFilter()
            {
                UserName = username
            };
            var user = await _adminUserServices.GetAdminUserAsync(userdetail);
            if(user == null)
            {
                return NotFound();
            }
            return View(user.Items.FirstOrDefault());
        }
    }
}
