using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Mpmt.Core.Dtos.Banks;
using Mpmt.Core.ViewModel.Bank;
using Mpmt.Services.Services.Bank;
using Mpmt.Services.Services.Common;
using Mpmt.Services.Services.RoleMenuPermission;
using Mpmt.Web.Common;
using Mpmt.Web.Filter;
using System.Net;

namespace Mpmt.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// The bank controller.
    /// </summary>
    [RolePremission]
    [Authorize]
    [AdminAuthorization]
    public class BankController : BaseAdminController
    {
        private readonly ICommonddlServices _commonddl;
        private readonly IRMPService _rMPService;
        private readonly IBankServices _bankServices;
        private readonly INotyfService _notyfService;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="BankController"/> class.
        /// </summary>
        /// <param name="notyfService">The notyf service.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="bankServices">The bank services.</param>
        /// <param name="commonddl">The commonddl.</param>
        public BankController(INotyfService notyfService, IMapper mapper, IBankServices bankServices, ICommonddlServices commonddl, IRMPService rMPService)
        {

            _notyfService = notyfService;
            _mapper = mapper;
            _bankServices = bankServices;
            _commonddl = commonddl;
            _rMPService = rMPService;
        }

        /// <summary>
        /// Banks the index.
        /// </summary>
        /// <param name="bankFilter">The bank filter.</param>
        /// <returns>A Task.</returns>
        [HttpGet]
        public async Task<IActionResult> BankIndex([FromQuery] BankFilter bankFilter)
        {
            var actions = await _rMPService.GetActionPermissionListAsync("Bank");

            ViewBag.actions = actions;

            var data = await _commonddl.GetCountryddl();
            ViewBag.Countryddl = new SelectList(data, "value", "Text");

            var Result = await _bankServices.GetBankAsync(bankFilter);
            if (WebHelper.IsAjaxRequest(Request))
                return await Task.FromResult(PartialView("_BankIndex", Result));

            return await Task.FromResult(View(Result));
        }

        #region Bank-Add

        /// <summary>
        /// Adds the bank.
        /// </summary>
        /// <returns>A Task.</returns>
        [HttpGet]
        public async Task<IActionResult> AddBank()
        {
            var data = await _commonddl.GetCountryddl();
            ViewBag.Country = new SelectList(data, "value", "Text");

            return await Task.FromResult(PartialView());
        }

        /// <summary>
        /// Adds the bank.
        /// </summary>
        /// <param name="addBankVm">The add bank vm.</param>
        /// <returns>A Task.</returns>
        [HttpPost]
        [LogUserActivity("added Bank")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddBank([FromForm] AddBankVm addBankVm)
        {
            var data = await _commonddl.GetCountryddl();
            ViewBag.Country = new SelectList(data, "value", "Text");

            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView();
            }

            else
            {
                var ResponseStatus = await _bankServices.AddBankAsync(addBankVm);
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
        #endregion

        #region Bank-Update

        /// <summary>
        /// Updates the bank.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>A Task.</returns>
        [HttpGet]
        public async Task<IActionResult> UpdateBank(int id)
        {
            var data = await _commonddl.GetCountryddl();
            ViewBag.Country = new SelectList(data, "value", "Text");

            var Result = await _bankServices.GetBankByIdAsync(id);
            var mappeddata = _mapper.Map<UpdateBankVm>(Result);
            return await Task.FromResult(PartialView(mappeddata));
        }

        /// <summary>
        /// Updates the bank.
        /// </summary>
        /// <param name="updateBank">The update bank.</param>
        /// <returns>A Task.</returns>
        [HttpPost]
        [LogUserActivity("Updated Bank")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateBank([FromForm] UpdateBankVm updateBank)
        {
            var data = await _commonddl.GetCountryddl();
            ViewBag.Country = new SelectList(data, "value", "Text");

            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView();
            }

            else
            {
                var ResponseStatus = await _bankServices.UpdateBankAsync(updateBank);
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
        #endregion

        #region Bank-Delete

        /// <summary>
        /// Deletes the bank.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>A Task.</returns>
        [HttpGet]
        public async Task<IActionResult> DeleteBank(int id)
        {
            var Result = await _bankServices.GetBankByIdAsync(id);
            var mappeddata = _mapper.Map<UpdateBankVm>(Result);
            return await Task.FromResult(PartialView(mappeddata));
        }

        /// <summary>
        /// Deletes the bank.
        /// </summary>
        /// <param name="updateBank">The update bank.</param>
        /// <returns>A Task.</returns>
        [HttpPost]
        [LogUserActivity("Deleted Bank")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteBank([FromForm] UpdateBankVm updateBank)
        {
            if (updateBank.Id != 0)
            {
                var ResponseStatus = await _bankServices.RemoveBankAsync(updateBank);
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
        #endregion

    }
}
