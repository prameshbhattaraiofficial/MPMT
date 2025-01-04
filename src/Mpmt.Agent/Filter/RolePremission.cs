using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Mpmt.Services.Services.Roles;
using System.Security.Claims;

namespace Mpmt.Agent.Filter
{
    public class RolePremission : TypeFilterAttribute
    {
        public RolePremission() : base(typeof(PermissionActionFilter))
        {
        }
    }

    public class PermissionActionFilter : Attribute, IAsyncActionFilter
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAgentRoleServices _agentRoleServices;
        private readonly ClaimsPrincipal _loggedInUser;

        public PermissionActionFilter(IHttpContextAccessor httpContextAccessor, IAgentRoleServices agentRoleServices)
        {
            _httpContextAccessor = httpContextAccessor;
            _agentRoleServices = agentRoleServices;
            _loggedInUser = _httpContextAccessor.HttpContext?.User;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var roles = _loggedInUser.FindAll(ClaimTypes.Role).Select(x => x.Value.ToUpper()).ToList();
            var UserType = _loggedInUser.Claims?.FirstOrDefault(x => x.Type == "UserType").Value;
            if (UserType.ToUpper() == "SUPERAGENT")
            {
                var resultContext = await next();
            }
            else if (UserType.ToUpper() == "AGENT")
            {
                var resultContext = await next();
            }
            else if (UserType.ToUpper() == "EMPLOYEE")
            {
                var UserName = _loggedInUser.FindFirstValue(ClaimTypes.Name);

                ((ControllerActionDescriptor)context.ActionDescriptor).RouteValues.TryGetValue("Area", out var area);
                var actionName = ((ControllerActionDescriptor)context.ActionDescriptor).ActionName;
                var controllerName = ((ControllerActionDescriptor)context.ActionDescriptor).ControllerName;

                var permissionresponse = await _agentRoleServices.CheckPermission(area, controllerName, actionName, UserName);

                if (!permissionresponse)
                {
                    context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);
                    return;
                }
                // Call the action methodgi
                var resultContext = await next();
            }
            context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);
            return;
        }
    }

}
