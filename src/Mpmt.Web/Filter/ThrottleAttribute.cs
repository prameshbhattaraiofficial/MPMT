using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Mpmt.Web.Filter
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class ThrottleAttribute : ActionFilterAttribute
    {
        private readonly int _seconds;
        private DateTime _lastExecution = DateTime.MinValue;
        public ThrottleAttribute(int seconds)
        {
            _seconds = seconds;
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var timeSinceLastExecution = DateTime.Now - _lastExecution;
            if (timeSinceLastExecution.TotalSeconds < _seconds)
            {
                filterContext.Result = new ContentResult
                {
                    Content = "Action is being called too frequently. Please wait."
                };
            }
            else
            {
                _lastExecution = DateTime.Now;
            }

            base.OnActionExecuting(filterContext);
        }
    }
}
