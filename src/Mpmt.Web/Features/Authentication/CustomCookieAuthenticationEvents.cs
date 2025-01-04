using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Mpmt.Core;
using Mpmt.Services.Authentication;

namespace Mpmt.Web.Features.Authentication
{
    public class CustomCookieAuthenticationEvents : CookieAuthenticationEvents
    {
        private readonly IUserAuthSessionService _userSessionValidationService;

        public CustomCookieAuthenticationEvents(IUserAuthSessionService userSessionValidationService)
        {
            _userSessionValidationService = userSessionValidationService;
        }

        public override async Task ValidatePrincipal(CookieValidatePrincipalContext context)
        {
            var userPrincipal = context.Principal;

            var uniqueId = (from c in userPrincipal.Claims
                            where c.Type == MpmtClaimTypes.UniqueId
                            select c.Value).FirstOrDefault();

            var lastChanged = (from c in userPrincipal.Claims
                               where c.Type == MpmtClaimTypes.LastChanged
                               select c.Value).FirstOrDefault();

            if (!await _userSessionValidationService.ValidateAsync(uniqueId, lastChanged))
            {
                context.RejectPrincipal();

                await context.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }
        }
    }
}
