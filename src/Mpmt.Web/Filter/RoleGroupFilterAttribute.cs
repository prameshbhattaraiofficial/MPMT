using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Mpmt.Core.Dtos.Partner;
using Mpmt.Core.Dtos.Roles;
using Mpmt.Services.Partner.IService;
using Mpmt.Services.Services.Roles;
using System.Security.Claims;

namespace Mpmt.Web.Filter
{
    /// <summary>
    /// The role group filter attribute.
    /// </summary>
    public class RoleGroupFilterAttribute : ActionFilterAttribute
    {
        public string UserType = string.Empty;
        private readonly ClaimsPrincipal _loggedInUser;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRoleServices _roleServices;
        private readonly IPartnerRoleServices _partnerroleServices;
        private readonly INotyfService _notyfService;
        private readonly string _message;





        /// <summary>
        /// Initializes a new instance of the <see cref="RoleGroupFilterAttribute"/> class.
        /// </summary>
        /// <param name="httpContextAccessor">The http context accessor.</param>
        /// <param name="roleServices">The role services.</param>
        /// <param name="partnerRoleServices">The partner role services.</param>
        public RoleGroupFilterAttribute(IHttpContextAccessor httpContextAccessor, IRoleServices roleServices, IPartnerRoleServices partnerRoleServices, INotyfService notyfService)
        {
            _httpContextAccessor = httpContextAccessor;
            _loggedInUser = _httpContextAccessor.HttpContext?.User;
            _roleServices = roleServices;
            _partnerroleServices = partnerRoleServices;
            _notyfService = notyfService;
            _message = "You have no access to this Action";
        }



        /// <summary>
        /// Ons the action executing.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
        }        
    
    }
}
