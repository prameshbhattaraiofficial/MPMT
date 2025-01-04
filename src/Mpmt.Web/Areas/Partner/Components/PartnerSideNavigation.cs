using Microsoft.AspNetCore.Mvc;
using Mpmt.Core.ViewModel.Menu;
using Mpmt.Services.Services.Menu;
using Mpmt.Services.Services.RoleMenuPermission;
using System.Security.Claims;

namespace Mpmt.Web.Areas.Partner.Components
{
    public class PartnerSideNavigationViewComponent : ViewComponent
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMenuService _menuService;
        private readonly ClaimsPrincipal _Users;

        public PartnerSideNavigationViewComponent(IRMPService rmpService,
            IHttpContextAccessor httpContextAccessor, 
            IMenuService menuService)
        {
            _httpContextAccessor = httpContextAccessor;
            _menuService = menuService;
            _Users = _httpContextAccessor.HttpContext.User;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            //string name = "Partner";
            //var menusList = await _rmpService.GetMenusSubmenusForCurrentUserAsync(name);
            //return await Task.FromResult(View("Default", menusList));
            var UserName = _Users.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
            var result = await _menuService.GetPartnerMenuByUserNameAsync(UserName);
            var menutodisplay = result.Where(x => x.IsActive == true);
            var menuWithChild = new List<PartnerMenuWithPermission>();
            foreach (var menu in menutodisplay)
            {
                if (menu.ParentId == 0)
                {
                    var ParentMenu = new PartnerMenuWithPermission
                    {
                        child = new List<PartnerMenuWithPermission>(),
                        Id = menu.Id,
                        Title = menu.Title,
                        ParentId = menu.ParentId,
                        Area = menu.Area,
                        Controller = menu.Controller,
                        Action = menu.Action,
                        IsActive = menu.IsActive,
                        DisplayOrder = menu.DisplayOrder,
                        ImagePath = menu.ImagePath,
                        Permission = menu.Permission
                    };

                    foreach (var child in menutodisplay)
                    {
                        if (child.ParentId == menu.Id && child.Id != menu.Id && child.Permission)
                        {
                            var ChildMenu = new PartnerMenuWithPermission();
                            ChildMenu.Id = child.Id;
                            ChildMenu.ParentId = child.ParentId;
                            ChildMenu.ImagePath = child.ImagePath;
                            ChildMenu.Title = child.Title;
                            ChildMenu.Action = child.Action;
                            ChildMenu.Controller = child.Controller;
                            ChildMenu.Area = child.Area;
                            ChildMenu.IsActive = child.IsActive;
                            ChildMenu.DisplayOrder = child.DisplayOrder;
                            ChildMenu.Permission = child.Permission;
                            ParentMenu.child.Add(ChildMenu);
                        }
                    }
                    menuWithChild.Add(ParentMenu);
                }
            }
            return await Task.FromResult(View("Default", menuWithChild));
        }
    }
}
