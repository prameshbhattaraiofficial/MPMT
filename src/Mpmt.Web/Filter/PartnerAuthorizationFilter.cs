using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Mpmt.Web.Filter
{

    public class PartnerAuthorization : TypeFilterAttribute
    {
        public PartnerAuthorization() : base(typeof(PartnerAuthorizationFilter))
        {
            
        }

    }
    public class PartnerAuthorizationFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.Result = new StatusCodeResult(StatusCodes.Status401Unauthorized);
                return;
            }
            //checkusertype too 
            if (!context.HttpContext.User.IsInRole("Access"))
            {
                context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);
                return;
            }
        }
    }
}
