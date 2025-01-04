
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using Mpmt.Core.Common.MvcHelper;
using Mpmt.Core.Dtos.DropDown;
using Mpmt.Core.Models.Route;
using Mpmt.Data.Repositories.RoleMenuPermissionRepository;
using Mpmt.Services.Services.Common;
using Mpmt.Web.Filter;


namespace Mpmt.Web.Areas.Admin.Controllers
{
    [AdminAuthorization]
    public class MenuPremissionController : BaseAdminController
    {
        private readonly IRMPRepository _rMPRepository;
        private readonly ICommonddlServices _commonddl;
        private readonly INotyfService _notyfService;

        public MenuPremissionController(IRMPRepository rMPRepository, ICommonddlServices commonddl,INotyfService notyfService)
        {
            _rMPRepository = rMPRepository;
            _commonddl = commonddl;
            _notyfService = notyfService;
        }
        public IActionResult Index()
        {

            return View();

        }
        public async Task<IActionResult> AddMenuPermission()
        {
            var data = await _rMPRepository.GetListcontrollerActionAsync(0);
            data = data.Where(x => x.Area == "Admin").ToList();
            ViewBag.Menu = data;
            var role = await _commonddl.GetAdminRoleddl();
            ViewBag.Role = new SelectList(role, "value", "Text");
            return PartialView("_addMenuPermission");
        }
        [HttpPost]
        [LogUserActivity("added menu permission")]
        public async Task<IActionResult> AddMenuPermissionpost(AddcontrollerAction test)
        {
            var response = await _rMPRepository.AddmenuPermission(test);
            if (response.StatusCode == 200)
            {
                return Ok();
            }
            var data = await _rMPRepository.GetListcontrollerActionAsync(0);
            data = data.Where(x => x.Area == "Admin");
            ViewBag.Menu = data;
            return PartialView("_addMenuPermission");
        }

        public async Task<IActionResult> UpdateMenuPermission(int RoleId)
        {
            var role = await _commonddl.GetAdminRoleddl();
            ViewBag.Role = new SelectList(role.Where(x => x.value == RoleId.ToString()), "value", "Text");
            var data = await _rMPRepository.GetListcontrollerActionAsync(RoleId);
            data = data.Where(x => x.Area == "Admin").ToList();
            ViewBag.Menu = data;
            return PartialView("_addMenuPermission");
        }
        [HttpPost]
        [LogUserActivity("updated menu permission")]
        public async Task<IActionResult> UpdateMenuPermission(AddcontrollerAction test)
        {
            var role = await _commonddl.GetAdminRoleddl();
            ViewBag.Role = new SelectList(role.Where(x => x.value == test.RoleId.ToString()), "value", "Text");
            var response = await _rMPRepository.AddmenuPermission(test);
            if(response.StatusCode == 200)
            {
                _notyfService.Success(response.MsgText);
                return Ok();
            }
            var data = await _rMPRepository.GetListcontrollerActionAsync(2);
            data = data.Where(x => x.Area == "Admin");
            ViewBag.Menu = data;
            return PartialView("_addMenuPermission");
        }
    }
}
