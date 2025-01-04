using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Mpmt.Core.Dtos.AdminUser;
using Mpmt.Core.Dtos.DropDown;
using Mpmt.Core.Dtos.Users;
using Mpmt.Core.Events;
using Mpmt.Core.ViewModel.AdminUser;
using Mpmt.Services.Authentication;
using Mpmt.Services.Services.AdminUser;
using Mpmt.Services.Services.Common;
using Mpmt.Services.Services.RoleMenuPermission;
using Mpmt.Web.Common;
using Mpmt.Web.Filter;
using System.Net;

namespace Mpmt.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// The admin user controller.
    /// </summary>
    [RolePremission]
    [AdminAuthorization]
    public class AdminUserController : BaseAdminController
    {
        private readonly IEventPublisher _eventPublisher;
        private readonly IAdminUserServices _adminUserServices;
        private readonly INotyfService _notyfService;
        private readonly ICommonddlServices _commonddl;
        private readonly IRMPService _rMPService;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdminUserController"/> class.
        /// </summary>
        /// <param name="adminUserServices">The admin user services.</param>
        public AdminUserController(
            IEventPublisher eventPublisher,
            IAdminUserServices adminUserServices, INotyfService notyfService, IMapper mapper, ICommonddlServices commonddl, IRMPService rMPService)
        {
            _eventPublisher = eventPublisher;
            _adminUserServices = adminUserServices;
            _notyfService = notyfService;
            _mapper = mapper;
            _commonddl = commonddl;
            _rMPService = rMPService;
        }
        /// <summary>
        /// Users the index.
        /// </summary>
        /// <param name="userFilter">The user filter.</param>
        /// <returns>A Task.</returns>
        [HttpGet]
        public async Task<IActionResult> UserIndex([FromQuery] AdminUserFilter userFilter)
        {
            Response.Headers["Cache-Control"] = "no-store, no-cache, must-revalidate";
            Response.Headers["Pragma"] = "no-cache";
            Response.Headers["Expires"] = "0";

            var actions = await _rMPService.GetActionPermissionListAsync("AdminUser");

            ViewBag.actions = actions;

            var data = await _adminUserServices.GetAdminUserAsync(userFilter);
            if (WebHelper.IsAjaxRequest(Request))
            {
                return PartialView("_AdminUserList", data);
            }
            return View(data);
        }

        #region Validation
        [AcceptVerbs("GET", "POST")]
        public async Task<IActionResult> VerifyUserName(string userName)
        {
            if (!await _adminUserServices.VerifyUserNameAdmin(userName))
            {
                return Json($"User Name {userName} is already in use.");
            }

            return Json(true);
        }

        [AcceptVerbs("GET", "POST")]
        public async Task<IActionResult> VerifyEmail(string Email)
        {
            if (!await _adminUserServices.VerifyEmailAdmin(Email))
            {
                return Json($"Email is already in use.");
            }

            return Json(true);
        }
        #endregion

        #region Add-Admin-User
        public async Task<IActionResult> AddAdminUser()
        {
            var data = await _commonddl.Getgenderddl();
            var role = await _commonddl.GetAdminRoleddl();
            ViewBag.Gender = new SelectList(data, "value", "Text");
            ViewBag.Role = new SelectList(role, "value", "Text");
            return await Task.FromResult(PartialView());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [LogUserActivity("added admin user")]

        public async Task<IActionResult> AddAdminUser([FromForm] AdminUserVm adminUserVm)
        {
            var data = await _commonddl.Getgenderddl();
            ViewBag.Gender = new SelectList(data, "value", "Text");

            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView();
            }

            var addResult = await _adminUserServices.AddAdminUserAsync(adminUserVm, User);
            if (!addResult.Success)
            {
                Response.StatusCode = (int)(HttpStatusCode.BadRequest);
                ViewBag.Error = addResult.Errors.First();
                return PartialView();
            }

            _notyfService.Success(addResult.Message);
            return Ok();
        }
        #endregion

        #region Update-Admin-User
        [HttpGet]
        public async Task<IActionResult> UpdateAdminUser(int id)
        {
            var user = await _adminUserServices.GetAdminUserByIdAsync(id);
            var data = await _commonddl.Getgenderddl();
            var role = await _commonddl.GetAdminRoleddl();
            ViewBag.Gender = new SelectList(data, "value", "Text");
            ViewBag.Role = new SelectList(role, "value", "Text");
            var mappedData = _mapper.Map<IUDAdminUser>(user);
            return await Task.FromResult(PartialView(mappedData));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [LogUserActivity("updated admin user")]

        public async Task<IActionResult> UpdateAdminUser([FromForm] AdminUserVm adminUserVm)
        {
            var data = await _commonddl.Getgenderddl();
            var role = await _commonddl.GetAdminRoleddl();
            ViewBag.Gender = new SelectList(data, "value", "Text");
            ViewBag.Role = new SelectList(role, "value", "Text");

            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView();
            }

            var updateResult = await _adminUserServices.UpdateAdminUserAsync(adminUserVm, User);
            if (!updateResult.Success)
            {
                Response.StatusCode = (int)(HttpStatusCode.BadRequest);
                ViewBag.Error = updateResult.Errors.First();
                return PartialView();
            }

            if (!adminUserVm.IsActive)
            {
                var adminUser = await _adminUserServices.GetAdminUserByIdAsync(adminUserVm.Id);
                if (adminUser is not null)
                    await _eventPublisher.PublishAsync(new EntityUpdatedEvent<SessionAuthExpiration>(
                            new SessionAuthExpiration { UserUniqueId = adminUser.UserGuid.ToString(), ExpireBefore = DateTime.UtcNow }));
            }

            _notyfService.Success(updateResult.Message);
            return Ok();
        }
        #endregion

        [HttpGet]
        public async Task<IActionResult> DeleteAdminUser(int id)
        {
            var result = await _adminUserServices.GetAdminUserByIdAsync(id);
            var mappedData = _mapper.Map<AdminUserVm>(result);
            return await Task.FromResult(PartialView(mappedData));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [LogUserActivity("deleted admin user")]

        public async Task<IActionResult> DeleteAdminUser([FromForm] AdminUserVm adminUserVm)
        {
            if (adminUserVm.Id != 0)
            {
                var responseStatus = await _adminUserServices.DeleteAdminUserAsync(adminUserVm.Id, adminUserVm.Remarks);
                if (responseStatus.StatusCode == 200)
                {
                    _notyfService.Success(responseStatus.MsgText);

                    // expire session of a user
                    var adminUser = await _adminUserServices.GetAdminUserByIdAsync(adminUserVm.Id);
                    if (adminUser is not null)
                        await _eventPublisher.PublishAsync(new EntityUpdatedEvent<SessionAuthExpiration>(
                                new SessionAuthExpiration { UserUniqueId = adminUser.UserGuid.ToString(), ExpireBefore = DateTime.UtcNow }));

                    return Ok();
                }
                else
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    TempData["Error"] = responseStatus.MsgText;
                }
            }
            Response.StatusCode = (int)HttpStatusCode.NotFound;
            return PartialView();
        }
        #region AssignTorole
        [HttpGet]
        public async Task<IActionResult> AssignUserToRole(int id,string fullname,string email)
        {
            AssignUserRoleDto roleDto = new AssignUserRoleDto();
            roleDto.user_id = id;
            ViewBag.Email = email;
            ViewBag.FullName=fullname;

            var UserRoleById = await _commonddl.GetPartnerEmployeeRolesByIdAsync(id);
            var admiroleddl = await _commonddl.GetAdminRoleddl();

            ViewBag.AdminRoleddl = new SelectList(admiroleddl, "value", "Text");
            var Roles = new List<SelectListItem>();
            var roles = new List<int>();
            foreach (var userrole in UserRoleById)
            {
                roles.Add(int.Parse(userrole.value));
            }
            foreach (var role in admiroleddl)
            {
                if (roles.Contains(int.Parse(role.value)))
                {
                    Roles.Add(new SelectListItem() { Value = role.value, Text = role.Text, Selected = true });
                }
                else
                {
                    Roles.Add(new SelectListItem() { Value = role.value, Text = role.Text, Selected = false });
                }
            }
            ViewBag.Roles = Roles;
            ViewBag.Error = TempData["Error"] == null ? "" : TempData["Error"];
            return PartialView("_AssignRole", roleDto);
        }

        [HttpPost]
        [LogUserActivity("assigned role to user")]

        public async Task<IActionResult> AssignUserToRole([FromForm] AssignUserRoleDto AssignuserroleDto, string fullname, string email)
        {
            var UserRoleById = await _commonddl.GetPartnerEmployeeRolesByIdAsync(AssignuserroleDto.user_id);
            var admiroleddl = await _commonddl.GetAdminRoleddl();

            //ViewBag.AdminRoleddl = new SelectList(admiroleddl, "value", "Text");
            var Roles = new List<SelectListItem>();
            var roles = new List<int>();
            foreach (var userrole in UserRoleById)
            {
                roles.Add(int.Parse(userrole.value));
            }
            foreach (var role in admiroleddl)
            {
                if (roles.Contains(int.Parse(role.value)))
                {
                    Roles.Add(new SelectListItem() { Value = role.value, Text = role.Text, Selected = true });
                }
                else
                {
                    Roles.Add(new SelectListItem() { Value = role.value, Text = role.Text, Selected = false });
                }
            }
            ViewBag.Roles = Roles;

            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView("_AssignRole", AssignuserroleDto);
            }

            var result = await _adminUserServices.AssignUserRole(AssignuserroleDto.user_id, AssignuserroleDto.roleid);
            if (result.StatusCode == 200)
            {
                _notyfService.Success(result.MsgText);

                var user = await _adminUserServices.GetAdminUserByIdAsync(AssignuserroleDto.user_id);
                if (user is not null)
                    await _eventPublisher.PublishAsync(new EntityUpdatedEvent<SessionAuthExpiration>(
                            new SessionAuthExpiration { UserUniqueId = user.UserGuid.ToString(), ExpireBefore = DateTime.UtcNow }));

                return Ok();
            }

            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            ViewBag.Error = result.MsgText;
            ViewBag.Email = email;
            ViewBag.FullName = fullname;
            return PartialView("_AssignRole", AssignuserroleDto);
        }

        #endregion
    }
}
