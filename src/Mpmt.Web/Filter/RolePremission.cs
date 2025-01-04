using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Mpmt.Data.Repositories.RoleMenuPermissionRepository;
using System.Security.Claims;

namespace Mpmt.Web.Filter
{
    /// <summary>
    /// The role premission.
    /// </summary>
    public class RolePremission : TypeFilterAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RolePremission"/> class.
        /// </summary>
        public RolePremission() : base(typeof(PermissionActionFilter))
        {
        }
    }
    /// <summary>
    /// The permission action filter.
    /// </summary>
    public class PermissionActionFilter : Attribute, IAsyncActionFilter
    {
        public string UserType = string.Empty;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ClaimsPrincipal _loggedInUser;
        private readonly IRMPRepository _rMPRepository;
        /// <summary>
        /// Initializes a new instance of the <see cref="PermissionActionFilter"/> class.
        /// </summary>
        /// <param name="httpContextAccessor">The http context accessor.</param>
        /// <param name="rMPRepository">The r m p repository.</param>
        public PermissionActionFilter(IHttpContextAccessor httpContextAccessor, IRMPRepository rMPRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _rMPRepository = rMPRepository;
            _loggedInUser = _httpContextAccessor.HttpContext?.User;
        }
        /// <summary>
        /// Ons the action execution async.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="next">The next.</param>
        /// <returns>A Task.</returns>
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var roles = _loggedInUser.FindAll(ClaimTypes.Role).Select(x => x.Value.ToUpper()).ToList();
            var UserType = _loggedInUser.Claims?.FirstOrDefault(x => x.Type == "UserType").Value;
            if (roles.Contains("SUPERADMIN") && UserType.ToUpper() == "ADMIN")
            {
                await next();
                return;
            }
            if (roles.Contains("PARTNERADMIN") && UserType.ToUpper() == "PARTNER")
            {
                var UserName = _loggedInUser.FindFirstValue(ClaimTypes.Name);

                ((Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)context.ActionDescriptor).RouteValues.TryGetValue("Area", out var area);
                var actionName = ((Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)context.ActionDescriptor).ActionName;
                var controllerName = ((Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)context.ActionDescriptor).ControllerName;

                var permissionresponse = await _rMPRepository.CheckPartnerMenuPermission(area, controllerName, actionName, UserName);


                if (!permissionresponse)
                {
                    context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);
                    return;
                }

                var resultContext = await next();
            }
            if (UserType.ToUpper() == "ADMIN")
            {
                var UserName = _loggedInUser.FindFirstValue(ClaimTypes.Name);

                ((Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)context.ActionDescriptor).RouteValues.TryGetValue("Area", out var area);
                var actionName = ((Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)context.ActionDescriptor).ActionName;
                var controllerName = ((Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)context.ActionDescriptor).ControllerName;

                var permissionresponse = await _rMPRepository.CheckPermission(area, controllerName, actionName, UserName);


                if (!permissionresponse)
                {
                    context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);
                    return;
                }

                // Call the action methodgi
                var resultContext = await next();
            }
            if (UserType.ToUpper() == "PARTNEREMPLOYEE")
            {
                var UserName = _loggedInUser.FindFirstValue(ClaimTypes.Name);

                ((Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)context.ActionDescriptor).RouteValues.TryGetValue("Area", out var area);
                var actionName = ((Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)context.ActionDescriptor).ActionName;
                var controllerName = ((Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)context.ActionDescriptor).ControllerName;

                var permissionresponse = await _rMPRepository.CheckPartnerPermission(area, controllerName, actionName, UserName);


                if (!permissionresponse)
                {
                    context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);
                    return;
                }

                // Call the action method
                var resultContext = await next();

            }
            context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);
            return;


        }
    }

}
