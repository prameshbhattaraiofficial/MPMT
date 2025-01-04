using Microsoft.AspNetCore.Mvc;
using Mpmt.Services.Partner;
using Mpmt.Services.Partner.IService;
using Mpmt.Web.Filter;
using System.Security.Claims;

namespace Mpmt.Web.Areas.Partner.Controllers
{
    /// <summary>
    /// The profile controller.
    /// </summary>
    [PartnerAuthorization]
    public class ProfileController : BasePartnerController
    {
        private readonly IPartnerService _partnerService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPartnerRegistrationService _partnerRegistrationService;

        private readonly ClaimsPrincipal _loggedInUser;

        public ProfileController(
            IPartnerService partnerService,
            IHttpContextAccessor httpContextAccessor,
            IPartnerRegistrationService partnerRegistrationService)
        {
            _partnerService = partnerService;
            _httpContextAccessor = httpContextAccessor;

            _loggedInUser = httpContextAccessor.HttpContext.User;
            _partnerRegistrationService = partnerRegistrationService;
        }

        public async Task<IActionResult> Index()
        {
            var PartnerId = _loggedInUser.Claims.FirstOrDefault(x => x.Type == "Id").Value;
            var email = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);

            var parOremp = await _partnerRegistrationService.CheckPartnerOrEmployee(email);

            if (parOremp == "PartnerEmployee")
            {
                var partnerDetail = await _partnerService.GetPartnerEmployeeByEmailAsync(email);
                return View(partnerDetail);
            }
            else
            {
                if (int.TryParse(PartnerId, out int id))
                {
                    var Partnerdetail = await _partnerService.GetPartnerByIdAsync(id);

                    return View(Partnerdetail);
                };
            }
            //ToDo return error page
            return View();
        }
    }
}