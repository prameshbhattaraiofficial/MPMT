using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Mpmt.Core.Dtos.Roles;
using Mpmt.Core.ViewModel.Role;
using Mpmt.Services.Services.Menu;
using Mpmt.Services.Services.RoleMenuPermission;
using Mpmt.Services.Services.Roles;
using Mpmt.Web.Common;
using Mpmt.Web.Filter;
using System.Net;

namespace Mpmt.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// The roles controller.
    /// </summary>
    [RolePremission]
    [AdminAuthorization]
    public class RolesController : BaseAdminController
    {

        private readonly IRoleServices _roleServices;
        private readonly IMenuService _menuServices;
        private readonly INotyfService _notyfService;
        private readonly IMapper _mapper;
        private readonly IRMPService _rMPService;

        /// <summary>
        /// Initializes a new instance of the <see cref="RolesController"/> class.
        /// </summary>
        /// <param name="roleServices">The role services.</param>
        /// <param name="notyfService">The notyf service.</param>
        /// <param name="mapper">The mapper.</param>
        public RolesController(IRoleServices roleServices, IMenuService menuService, INotyfService notyfService, IMapper mapper, IRMPService rMPService)
        {
            _roleServices = roleServices;
            _notyfService = notyfService;
            _menuServices = menuService;
            _mapper = mapper;
            _rMPService = rMPService;
        }

        /// <summary>
        /// Indices the.
        /// </summary>
        /// <returns>A Task.</returns>
        [HttpGet]

        public async Task<IActionResult> Index()
        {
            var actions = await _rMPService.GetActionPermissionListAsync("Roles");

            ViewBag.actions = actions;

            var role = await _roleServices.GetRoleAsync();
            if (WebHelper.IsAjaxRequest(Request))
                return await Task.FromResult(PartialView("_RoleIndex", role));

            return await Task.FromResult(View(role));
        }


        /// <summary>
        /// Adds the role.
        /// </summary>
        /// <returns>A Task.</returns>
        [HttpGet]
        public async Task<IActionResult> AddRole()
        {
            return await Task.FromResult(PartialView());
        }

        /// <summary>
        /// Adds the role.
        /// </summary>
        /// <param name="addRole">The add role.</param>
        /// <returns>A Task.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [LogUserActivity("added a new role")]
        public async Task<IActionResult> AddRole([FromForm] AddRoleVm addRole)
        {

            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView();
            }

            else
            {
                var RoleResult = await _roleServices.AddRoleAsync(addRole);
                if (RoleResult.StatusCode == 200)
                {
                    _notyfService.Success(RoleResult.MsgText);
                    return Ok();
                }
                else
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    ViewBag.Error = RoleResult.MsgText;
                    return PartialView();
                }
            }

        }

        /// <summary>
        /// Updates the role.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>A Task.</returns>
        [HttpGet]
        public async Task<IActionResult> UpdateRole(int id)
        {
            var role = await _roleServices.GetAppRoleById(id);
            var mappeddata = _mapper.Map<UpdateRoleVm>(role);
            return await Task.FromResult(PartialView(mappeddata));
        }

        /// <summary>
        /// Updates the role.
        /// </summary>
        /// <param name="updateRole">The update role.</param>
        /// <returns>A Task.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [LogUserActivity("updated a role")]
        public async Task<IActionResult> UpdateRole([FromForm] UpdateRoleVm updateRole)
        {

            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView();
            }

            else
            {
                var RoleResult = await _roleServices.UpdateRoleAsync(updateRole);
                if (RoleResult.StatusCode == 200)
                {
                    _notyfService.Success(RoleResult.MsgText);
                    return Ok();
                }
                else
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    ViewBag.Error = RoleResult.MsgText;
                    return PartialView();
                }
            }

        }

        #region Role-Delete

        /// <summary>
        /// Deletes the role.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>A Task.</returns>
        [HttpGet]
        public async Task<IActionResult> DeleteRole(int id)
        {
            var role = await _roleServices.GetAppRoleById(id);
            var mappeddata = _mapper.Map<UpdateRoleVm>(role);
            return await Task.FromResult(PartialView(mappeddata));
        }

        /// <summary>
        /// Deletes the role.
        /// </summary>
        /// <param name="roleVm">The role vm.</param>
        /// <returns>A Task.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [LogUserActivity("deleted a role")]
        public async Task<IActionResult> DeleteRole([FromForm] UpdateRoleVm roleVm)
        {
            if (roleVm.Id != 0)
            {
                var RolesResponse = await _roleServices.RemoveRoleAsync(roleVm.Id);
                if (RolesResponse.StatusCode == 200)
                {
                    _notyfService.Success(RolesResponse.MsgText);
                    return Ok();
                }
                else
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    TempData["Error"] = RolesResponse.MsgText;
                }
            }
            Response.StatusCode = (int)HttpStatusCode.NotFound;
            return PartialView();
        }
        #endregion




        /// <summary>
        /// Permissions the.
        /// </summary>
        /// <returns>A Task.</returns>
        public async Task<IActionResult> Permissions()
        {
            return await Task.FromResult(View());
        }
        #region Assign Role
        /// <summary>
        /// Assigns the role.
        /// </summary>
        /// <param name="Id">The id.</param>
        /// <returns>A Task.</returns>
        [HttpGet]
        public async Task<IActionResult> AssignRole(int Id)
        {
            var roleDetails = await _roleServices.GetAppRoleById(Id);
            int roleId = roleDetails.Id;
            ViewBag.roleid = roleId;
            var menulist = await _roleServices.GetMenuByRoleId(roleId);

            var menuListwithchield = new List<MenuRoleWithchild>();
            foreach (var menu in menulist)
            {
                if (menu.parentId == 0)
                {
                    var parentmenu = new MenuRoleWithchild()
                    {
                        Child = new List<MenuByRole>(),
                        menuId = menu.menuId,
                        title = menu.title,
                        parentId = menu.parentId,
                        menuUrl = menu.menuUrl,
                        displayOrder = menu.displayOrder,
                        imagePath = menu.imagePath,
                        viewPer = menu.viewPer,
                        createPer = menu.createPer,
                        updatePer = menu.updatePer,
                        deletePer = menu.deletePer,

                    };
                    foreach (var child in menulist)
                    {
                        if (child.parentId == menu.menuId)
                        {
                            var childmenu = new MenuByRole()
                            {
                                menuId = child.menuId,
                                title = child.title,
                                parentId = child.parentId,
                                menuUrl = child.menuUrl,
                                displayOrder = child.displayOrder,
                                imagePath = child.imagePath,
                                viewPer = child.viewPer,
                                createPer = child.createPer,
                                updatePer = child.updatePer,
                                deletePer = child.deletePer,

                            };
                            parentmenu.Child.Add(childmenu);
                        }
                    }
                    menuListwithchield.Add(parentmenu);
                }
            }



            ViewBag.Viewall = menulist.All(x => x.viewPer) == true ? "Checked" : "Unchecked";
            ViewBag.Createall = menulist.All(x => x.createPer) == true ? "Checked" : "Unchecked";
            ViewBag.Editall = menulist.All(x => x.updatePer) == true ? "Checked" : "Unchecked";
            ViewBag.Deleteall = menulist.All(x => x.deletePer) == true ? "Checked" : "Unchecked";
            ViewBag.Rolename = roleDetails.RoleName;
            ViewBag.Username = User.Identity == null ? "Error" : User.Identity.Name;
            return PartialView(menuListwithchield);
        }

        /// <summary>
        /// Assigns the role.
        /// </summary>
        /// <param name="permissions">The permissions.</param>
        /// <returns>A Task.</returns>
        [HttpPost]
        //[ValidateAntiForgeryToken]
        [LogUserActivity("assigned permissions to a role")]
        public async Task<IActionResult> AssignRole([FromBody] List<Permission> permissions)
        {
            if (permissions is not null)
            {
                var roleDetails = await _roleServices.GetAppRoleById(permissions.FirstOrDefault().RoleId);
                int roleId = roleDetails.Id;
                ViewBag.roleid = roleId;
                ViewBag.Username = User.Identity == null ? "Error" : User.Identity.Name;
                ViewBag.Rolename = roleDetails.RoleName;
                IEnumerable<MenuByRole> menulist = await _roleServices.GetMenuByRoleId(roleId);
                List<MenuByRole> menuItems = new List<MenuByRole>();
                foreach (var item in permissions)
                {
                    MenuByRole menuItem = menulist.Where(x => x.menuId == item.menuid).FirstOrDefault();
                    if (menuItem.viewPer != item.viewper ||
                      menuItem.createPer != item.Createper ||
                      menuItem.updatePer != item.Updateper ||
                      menuItem.deletePer != item.Deleteper)
                    {
                        menuItem.viewPer = item.viewper;
                        menuItem.createPer = item.Createper;
                        menuItem.updatePer = item.Updateper;
                        menuItem.deletePer = item.Deleteper;
                        menuItems.Add(menuItem);
                    }

                }
                if (menuItems.Count > 0)
                {
                    var result = await _roleServices.UpdateMenuToRole(menuItems, Convert.ToInt32(roleDetails.Id));
                    _notyfService.Success(result.MsgText);
                    return Json(true);
                }
                else
                {
                    _notyfService.Warning("No Any Permission Changes");
                    return Json(false);
                }
            }
            else
            {
                return Json(false);
            }

        }

        #endregion
    }
}
