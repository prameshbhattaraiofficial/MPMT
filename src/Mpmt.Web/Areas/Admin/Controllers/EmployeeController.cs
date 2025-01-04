using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Mpmt.Core.Dtos.DropDown;
using Mpmt.Core.Dtos.Employee;
using Mpmt.Services.Partner.IService;
using Mpmt.Services.Services.Common;
using Mpmt.Services.Services.Employee;
using Mpmt.Services.Services.RoleMenuPermission;
using Mpmt.Web.Areas.Admin.ViewModels.Paetner;
using Mpmt.Web.Common;
using Mpmt.Web.Filter;
using System.Diagnostics.Metrics;
using System.Net;

namespace Mpmt.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// The employee controller.
    /// </summary>
    [RolePremission]
    [AdminAuthorization]
    public class EmployeeController : BaseAdminController
    {
        private readonly IEmployeeService _employeeServices;
        private readonly INotyfService _notyfService;
        private readonly IMapper _mapper;
        private readonly ICommonddlServices _commonddl;
        private readonly IRMPService _rMPService;
        private readonly IPartnerEmployeeService _partnerEmployeeService;


        /// <summary>
        /// Initializes a new instance of the <see cref="EmployeeController"/> class.
        /// </summary>
        /// <param name="employeeServices">The employee services.</param>
        /// <param name="notyfService">The notyf service.</param>
        /// <param name="mapper">The mapper.</param>
        public EmployeeController(IEmployeeService employeeServices, INotyfService notyfService, IMapper mapper, ICommonddlServices commonddl, IRMPService rMPService, IPartnerEmployeeService partnerEmployeeService)
        {
            _employeeServices = employeeServices;
            _notyfService = notyfService;
            _mapper = mapper;
            _commonddl = commonddl;
            _rMPService = rMPService;
            _partnerEmployeeService = partnerEmployeeService;
        }



        /// <summary>
        /// Employees the index.
        /// </summary>
        /// <returns>A Task.</returns>
        [HttpGet]
        public async Task<IActionResult> EmployeeIndex()
        {
            var actions = await _rMPService.GetActionPermissionListAsync("Employee");

            ViewBag.actions = actions;
            var admiroleddl = await _commonddl.GetAdminRoleddl();
            ViewBag.AdminRoleddl = new SelectList(admiroleddl, "value", "Text");

            var result = await _employeeServices.GetEmployeeAsync();
            if (WebHelper.IsAjaxRequest(Request))
            {
                return PartialView("_EmployeeIndex", result);
            }

            return View(result);
        }

        /// <summary>
        /// Adds the employee.
        /// </summary>
        /// <returns>A Task.</returns>
        [HttpGet]
        public async Task<IActionResult> AddEmployee()
        {
            var admiroleddl = await _commonddl.GetAdminRoleddl();
            ViewBag.AdminRoleddl = new SelectList(admiroleddl, "value", "Text");

            return await Task.FromResult(PartialView());
        }
        /// <summary>
        /// Adds the employee.
        /// </summary>
        /// <param name="addEmployee">The add employee.</param>
        /// <returns>A Task.</returns>
        [HttpPost]
        [LogUserActivity("Added Employee")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEmployee([FromForm] IUDEmployee addEmployee)
        {
            //var data = await _commonddl.GetCountryddl();
            //ViewBag.Country = new SelectList(data, "value", "Text");

            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView();
            }

            else
            {
                var ResponseStatus = await _employeeServices.AddEmployeeAsync(addEmployee);
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
        /// Updates the employee.
        /// </summary>
        /// <param name="EmployeeId">The employee id.</param>
        /// <returns>A Task.</returns>
        [HttpGet]
        public async Task<IActionResult> UpdateEmployee(int EmployeeId)
        {
            var admiroleddl = await _commonddl.GetAdminRoleddl();
            ViewBag.AdminRoleddl = new SelectList(admiroleddl, "value", "Text");
            var Result = await _employeeServices.GetEmployeeByIdAsync(EmployeeId);
            //var mappeddata = _mapper.Map<IUDEmployee>(Result);
            return await Task.FromResult(PartialView(Result));
        }
        /// <summary>
        /// Updates the employee.
        /// </summary>
        /// <param name="updateEmployee">The update employee.</param>
        /// <returns>A Task.</returns>
        [HttpPost]
        [LogUserActivity("Updated Employee")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateEmployee([FromForm] IUDEmployee updateEmployee)
        {


            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView();
            }

            else
            {
                var ResponseStatus = await _employeeServices.UpdateEmployeeAsync(updateEmployee);
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
        /// Deletes the employee.
        /// </summary>
        /// <param name="EmployeeId">The employee id.</param>
        /// <returns>A Task.</returns>
        [HttpGet]
        public async Task<IActionResult> DeleteEmployee(int EmployeeId)
        {
            var Result = await _employeeServices.GetEmployeeByIdAsync(EmployeeId);
            //var mappeddata = _mapper.Map<UpdateBankVm>(Result);
            return await Task.FromResult(PartialView(Result));
        }
        /// <summary>
        /// Deletes the employee.
        /// </summary>
        /// <param name="updateEmployee">The update employee.</param>
        /// <returns>A Task.</returns>
        [HttpPost]
        [LogUserActivity("Deleted Employee")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteEmployee([FromForm] IUDEmployee updateEmployee)
        {
            if (updateEmployee.Id != 0)
            {
                var ResponseStatus = await _employeeServices.RemoveEmployeeAsync(updateEmployee);
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
       
    }
}
