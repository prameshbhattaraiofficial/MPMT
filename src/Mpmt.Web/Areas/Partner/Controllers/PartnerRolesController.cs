using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Mpmt.Core.Dtos.DropDown;
using Mpmt.Core.Models.Route;
using Mpmt.Core.ViewModel.Role;
using Mpmt.Services.Partner.IService;
using Mpmt.Services.Services.Common;
using Mpmt.Services.Services.RoleMenuPermission;
using Mpmt.Web.Common;
using Mpmt.Web.Filter;
using System.Net;

namespace Mpmt.Web.Areas.Partner.Controllers
{
    [PartnerAuthorization]
    [RolePremission]
    public class PartnerRolesController : BasePartnerController
    {
        private readonly IPartnerRoleServices _roleServices;
        private readonly INotyfService _notyfService;
        private readonly IMapper _mapper;
        private readonly ICommonddlServices _commonddl;
        private readonly IRMPService _rMPService;

        public PartnerRolesController(IPartnerRoleServices roleServices, INotyfService notyfService, IMapper mapper, ICommonddlServices commonddl, IRMPService rMPService)
        {
            _roleServices = roleServices;
            _notyfService = notyfService;
            _mapper = mapper;
            _commonddl = commonddl;
            _rMPService = rMPService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var actions = await _rMPService.GetPartnerActionPermissionList("PartnerRoles");
            ViewBag.actions = actions;

            var role = await _roleServices.GetRoleAsync(User);
            if (WebHelper.IsAjaxRequest(Request))
                return await Task.FromResult(PartialView("_RoleIndex", role));
            return await Task.FromResult(View(role));
        }

        [HttpGet]
        public async Task<IActionResult> AddRole()
        {
            return await Task.FromResult(PartialView());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddRole([FromForm] AddPartnerRoleVm addRole)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView();
            }
            else
            {
                var RoleResult = await _roleServices.AddRoleAsync(addRole, User);
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

        [HttpGet]
        public async Task<IActionResult> UpdateRole(int id)
        {
            var role = await _roleServices.GetPartnerRoleById(id);
            var mappeddata = _mapper.Map<UpdatePartnerRoleVm>(role);
            return await Task.FromResult(PartialView(mappeddata));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateRole([FromForm] UpdatePartnerRoleVm updateRole)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView();
            }
            else
            {
                var RoleResult = await _roleServices.UpdateRoleAsync(updateRole, User);
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

        [HttpGet]
        public async Task<IActionResult> DeleteRole(int id)
        {
            var role = await _roleServices.GetPartnerRoleById(id);
            var mappeddata = _mapper.Map<UpdatePartnerRoleVm>(role);
            return await Task.FromResult(PartialView(mappeddata));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteRole([FromForm] UpdatePartnerRoleVm PartnerRoleVm)
        {
            if (PartnerRoleVm.Id != 0)
            {
                var RolesResponse = await _roleServices.RemoveRoleAsync(PartnerRoleVm.Id, User);
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

        public async Task<IActionResult> Permissions()
        {
            return await Task.FromResult(View());
        }


        public async Task<IActionResult> UpdateMenuPermission(int RoleId)
        {
            var role = await _commonddl.GetPartnerRoleddl();
            ViewBag.PartnerRole = new SelectList(role.Where(x => x.value == RoleId.ToString()), "value", "Text");
            var data = await _rMPService.GetPartnerListcontrollerAction(RoleId);
            data = data.Where(x => x.Area == "Partner" && x.IsActive ).ToList();
            ViewBag.Menu = data;
            return PartialView("_addPartnerMenuPermission");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateMenuPermission(AddcontrollerAction test)
        {
            var role = await _commonddl.GetAdminRoleddl();
            ViewBag.Role = new SelectList(role.Where(x => x.value == test.RoleId.ToString()), "value", "Text");
            var response = await _rMPService.AddPartnermenuPermission(test);
            if (response.StatusCode == 200)
            {
                _notyfService.Success(response.MsgText);
                return Ok();
            }
            var data = await _rMPService.GetcontrollerActionAsync(test.RoleId);
            data = data.Where(x => x.Area == "Admin");
            ViewBag.Menu = data;
            return PartialView("_addPartnerMenuPermission");
        }
    }
}
