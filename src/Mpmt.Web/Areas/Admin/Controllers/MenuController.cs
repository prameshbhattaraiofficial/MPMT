using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Mpmt.Core.Dtos.Menu;
using Mpmt.Core.ViewModel.Menu;
using Mpmt.Services.Services.Common;
using Mpmt.Services.Services.Menu;
using Mpmt.Services.Services.RoleMenuPermission;
using Mpmt.Web.Common;
using Mpmt.Web.Filter;
using System.Net;
namespace Mpmt.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// The menu controller.
    /// </summary>
    [RolePremission]
    [AdminAuthorization]
    public class MenuController : BaseAdminController
    {
        private readonly IMenuService _menuServices;
        private readonly INotyfService _notyfService;
        private readonly ICommonddlServices _commonddl;
        private readonly IRMPService _rMPService;

        //  private readonly IMapper _mapper;
        /// <summary>
        /// Initializes a new instance of the <see cref="MenuController"/> class.
        /// </summary>
        /// <param name="menuService">The menu service.</param>
        /// <param name="notyfService">The notyf service.</param>
        public MenuController(IMenuService menuService, INotyfService notyfService, ICommonddlServices commonddlServices, IRMPService rMPService)
        {
            _menuServices = menuService;
            _notyfService = notyfService;
            _commonddl = commonddlServices;
            _rMPService = rMPService;
        }

        /// <summary>
        /// Menus the index.
        /// </summary>
        /// <returns>A Task.</returns>
        [HttpGet]
        public async Task<IActionResult> MenuIndex()
        {
            var actions = await _rMPService.GetActionPermissionListAsync("Menu");
            ViewBag.actions = actions;
            var result = await _menuServices.GetMenuAsync();
            var Menuwithchield = new List<Menuwithchield>();
            foreach (var menu in result)
            {
                if (menu.ParentId == 0)
                {
                    var Parentmenu = new Menuwithchield
                    {
                        child = new List<Menuwithchield>(),
                        Id = menu.Id,
                        ImagePath = menu.ImagePath,
                        ParentId = menu.ParentId,
                        Title = menu.Title,
                        MenuUrl = menu.MenuUrl,
                        Status = menu.IsActive,
                        DisplayOrder = menu.DisplayOrder
                    };

                    foreach (var child in result)
                    {
                        if (child.ParentId == menu.Id && child.Id != menu.Id)
                        {
                            var childmenu = new Menuwithchield();
                            childmenu.Id = child.Id;
                            childmenu.ParentId = child.ParentId;
                            childmenu.ImagePath = child.ImagePath;
                            childmenu.Title = child.Title;
                            childmenu.MenuUrl = child.MenuUrl;
                            childmenu.Status = child.IsActive;
                            childmenu.DisplayOrder = child.DisplayOrder;
                            Parentmenu.child.Add(childmenu);
                        }
                    }
                    Menuwithchield.Add(Parentmenu);
                }
            }






            if (WebHelper.IsAjaxRequest(Request))
            {
                return PartialView("_PartnerMenuTable", Menuwithchield);
            }

            return View(Menuwithchield);
        }


        /// <summary>
        /// Adds the menu.
        /// </summary>
        /// <returns>A Task.</returns>
        [HttpGet]
        public async Task<IActionResult> AddMenu()
        {
            var data = await _commonddl.GetParentMenu();
            ViewBag.ParentMenuddl = new SelectList(data, "value", "Text");

            return await Task.FromResult(PartialView());
        }

        /// <summary>
        /// Adds the menu.
        /// </summary>
        /// <param name="addMenu">The add menu.</param>
        /// <returns>A Task.</returns>
        [HttpPost]
        [LogUserActivity("Added Menu")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddMenu([FromForm] IUDMenu addMenu)
        {


            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView();
            }

            else
            {
                var ResponseStatus = await _menuServices.AddMenuAsync(addMenu);
                if (ResponseStatus.StatusCode == 200)
                {
                    _notyfService.Success(ResponseStatus.MsgText);
                    return Ok();
                }
                else
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    ViewBag.Error = ResponseStatus.MsgText;
                    return PartialView();
                }
            }

        }

        /// <summary>
        /// Updates the menu.
        /// </summary>
        /// <param name="EmployeeId">The employee id.</param>
        /// <returns>A Task.</returns>
        [HttpGet]
        public async Task<IActionResult> UpdateMenu(int MenuId)
        {
            var data = await _commonddl.GetParentMenu();
            ViewBag.ParentMenuddl = new SelectList(data, "value", "Text");
            var Result = await _menuServices.GetMenuByIdAsync(MenuId);
            //var mappeddata = _mapper.Map<IUDEmployee>(Result);
            return await Task.FromResult(PartialView(Result));
        }

        /// <summary>
        /// Updates the menu.
        /// </summary>
        /// <param name="updateMenu">The update menu.</param>
        /// <returns>A Task.</returns>
        [HttpPost]
        [LogUserActivity("Updated Menu")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateMenu([FromForm] IUDMenu updateMenu)
        {


            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView();
            }

            else
            {
                var ResponseStatus = await _menuServices.UpdateMenuAsync(updateMenu);
                if (ResponseStatus.StatusCode == 200)
                {
                    _notyfService.Success(ResponseStatus.MsgText);
                    return Ok();
                }
                else
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    ViewBag.Error = ResponseStatus.MsgText;
                    return PartialView();
                }
            }

        }

        /// <summary>
        /// Deletes the menu.
        /// </summary>
        /// <param name="EmployeeId">The employee id.</param>
        /// <returns>A Task.</returns>
        [HttpGet]
        public async Task<IActionResult> DeleteMenu(int MenuId)
        {
            var Result = await _menuServices.GetMenuByIdAsync(MenuId);

            return await Task.FromResult(PartialView(Result));
        }

        /// <summary>
        /// Deletes the menu.
        /// </summary>
        /// <param name="updateMenu">The update menu.</param>
        /// <returns>A Task.</returns>
        [HttpPost]
        [LogUserActivity("Deleted Menu")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteMenu([FromForm] IUDMenu updateMenu)
        {
            if (updateMenu.Id != 0)
            {
                var ResponseStatus = await _menuServices.RemoveMenuAsync(updateMenu);
                if (ResponseStatus.StatusCode == 200)
                {
                    _notyfService.Success(ResponseStatus.MsgText);
                    return Ok();
                }
                else
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    TempData["Error"] = ResponseStatus.MsgText;
                }
            }
            Response.StatusCode = (int)HttpStatusCode.NotFound;
            return PartialView();
        }
        /// <summary>
        /// Updates the menu display order.
        /// </summary>
        /// <param name="Id">The id.</param>
        /// <param name="DisplayOrderValue">The display order value.</param>

        public async void UpdateMenuDisplayOrder(int Id, int DisplayOrderValue)
        {
            if (Id > 0)
            {
                var UpdateOrder = new IUDMenu()
                {
                    Id = Id,
                    DisplayOrder = DisplayOrderValue
                };
                var response = await _menuServices.UpdateMenuDisplayOrderAsync(UpdateOrder);
            }

        }
        /// <summary>
        /// Updates the menu isactive.
        /// </summary>
        /// <param name="Id">The id.</param>
        /// <param name="IsActive">If true, is active.</param>
        /// <returns>A Task.</returns>
        [HttpPost]
        [LogUserActivity("updated menu active status")]
        public async Task<IActionResult> UpdateMenuIsactive(int Id, bool IsActive)
        {
            if (Id > 0)
            {
                var Menuupdate = new IUDMenu
                {
                    Id = Id,
                    IsActive = IsActive
                };
                var response = await _menuServices.UpdateMenuIsActiveAsync(Menuupdate);
                if (response.StatusCode == 200)
                {
                    _notyfService.Success(response.MsgText);
                    return Ok();
                }
                _notyfService.Error(response.MsgText);
            }
            return Ok();

        }

    }
}
