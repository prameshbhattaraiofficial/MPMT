using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Mpmt.Web.Filter
{

    public class AdminAuthorization : TypeFilterAttribute
    {
        public AdminAuthorization() : base(typeof(AdminAuthorizationFilter))
        {
            
        }

    }
    public class AdminAuthorizationFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.Result = new StatusCodeResult(StatusCodes.Status401Unauthorized);
                return;
            }
            //checkusertype too 
            if (!context.HttpContext.User.IsInRole("AdminAccess"))
            {
                context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);
                return;
            }
        }
    }
}
