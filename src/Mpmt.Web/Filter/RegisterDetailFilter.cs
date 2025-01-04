using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;


namespace Mpmt.Web.Filter
{
    public class RegisterDetailFilter : ActionFilterAttribute
    {


        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string currentUrl = filterContext.HttpContext.Request.Path.Value;
            string referringUrl = filterContext.HttpContext.Request.Headers["Referer"];

            var allow = filterContext.HttpContext.Session.GetString("_allow");
            if (allow == "allow")
            {

            }
            else
            {
                filterContext.Result = new RedirectResult("/Error");
            }

            base.OnActionExecuting(filterContext);
        }
    }
}
