using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Mpmt.Core.Dtos.PartnerBank;
using Mpmt.Core.ViewModel.PartnerBank;
using Mpmt.Services.Services.Common;
using Mpmt.Services.Services.PartnerBank;
using Mpmt.Services.Services.RoleMenuPermission;
using Mpmt.Web.Common;
using Mpmt.Web.Filter;
using System.Net;

namespace Mpmt.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// The partner bank controller.
    /// </summary>
    [RolePremission]
    [AdminAuthorization]
    public class PartnerBankController : BaseAdminController
    {
        private readonly IPartnerBankServices _partnerBankServices;
        private readonly INotyfService _notyfService;
        private readonly IRMPService _rMPService;
        private readonly ICommonddlServices _commonddlServices;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="PartnerBankController"/> class.
        /// </summary>
        /// <param name="partnerBankServices">The partner bank services.</param>
        /// <param name="notyfService">The notyf service.</param>
        /// <param name="mapper">The mapper.</param>
        public PartnerBankController(IPartnerBankServices partnerBankServices, INotyfService notyfService,
            IRMPService rMPService,IMapper mapper, ICommonddlServices commonddlServices)
        {
            _partnerBankServices = partnerBankServices;
            _notyfService = notyfService;
            _rMPService = rMPService;
            _mapper = mapper;
            _commonddlServices = commonddlServices;
        }

        /// <summary>
        /// Partners the bank index.
        /// </summary>
        /// <param name="PartnerBankFilter">The partner bank filter.</param>
        /// <returns>A Task.</returns>
        [HttpGet]
        public async Task<IActionResult> PartnerBankIndex([FromQuery] PartnerBankFilter PartnerBankFilter)
        {
            var actions = await _rMPService.GetActionPermissionListAsync("PartnerBank");

            ViewBag.actions = actions;

            var result = await _partnerBankServices.GetPartnerBankAsync(PartnerBankFilter);
            if (WebHelper.IsAjaxRequest(Request))
                return await Task.FromResult(PartialView("_PartnerBankIndex", result));

            return await Task.FromResult(View(result));
        }

        #region Add-Partner-Bank

        /// <summary>
        /// Adds the partner bank.
        /// </summary>
        /// <returns>A Task.</returns>
        [HttpGet]
        public async Task<IActionResult> AddPartnerBank(string partnerCode)
        {
            var bank = await _commonddlServices.GetBankddl();
            ViewBag.BankName = new SelectList(bank, "lookup", "Text");
            ViewBag.PartnerCode = partnerCode;
            return await Task.FromResult(PartialView());
        }

        /// <summary>
        /// Adds the partner bank.
        /// </summary>
        /// <param name="addPartnerBankVm">The add partner bank vm.</param>
        /// <returns>A Task.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [LogUserActivity("added a partner bank")]
        public async Task<IActionResult> AddPartnerBank([FromForm] AddPartnerBankVm addPartnerBankVm)
        {
            var bank = await _commonddlServices.GetBankddl();
            ViewBag.BankName = new SelectList(bank, "lookup", "Text");
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView();
            }
            else
            {
                var responseStatus = await _partnerBankServices.AddPartnerBankAsync(addPartnerBankVm);
                if (responseStatus.StatusCode == 200)
                {
                    _notyfService.Success(responseStatus.MsgText);
                    return Ok();
                }
                else
                {
                    Response.StatusCode = (int)(HttpStatusCode.BadRequest);
                    ViewBag.Error = responseStatus.MsgText;
                    return PartialView();
                }
            }
        }

        #endregion

        #region Update-Partner-Bank

        /// <summary>
        /// Updates the partner bank.
        /// </summary>
        /// <param name="partnerId">The partner id.</param>
        /// <returns>A Task.</returns>
        [HttpGet]
        public async Task<IActionResult> UpdatePartnerBank(int partnerId)
        {
            var result = await _partnerBankServices.GetPartnerBankByPartnerIdAsync(partnerId);
            var mappedData = _mapper.Map<UpdatePartnerBankVm>(result);
            return await Task.FromResult(PartialView(mappedData));
        }

        /// <summary>
        /// Updates the partner bank.
        /// </summary>
        /// <param name="updatePartnerBankVm">The update partner bank vm.</param>
        /// <returns>A Task.</returns>
        [HttpPost]
        [LogUserActivity("updated a partner bank")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdatePartnerBank([FromForm] UpdatePartnerBankVm updatePartnerBankVm)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView();
            }
            else
            {
                var responseStatus = await _partnerBankServices.UpdatePartnerBankAsync(updatePartnerBankVm);
                if (responseStatus.StatusCode == 200)
                {
                    _notyfService.Success(responseStatus.MsgText);
                    return Ok();
                }
                else
                {
                    Response.StatusCode = (int)(HttpStatusCode.BadRequest);
                    ViewBag.Error = responseStatus.MsgText;
                    return PartialView();
                }
            }
        }

        #endregion

        #region Delete-Partner-Bank

        /// <summary>
        /// Deletes the partner bank.
        /// </summary>
        /// <param name="partnerId">The partner id.</param>
        /// <returns>A Task.</returns>
        [HttpGet]
        public async Task<IActionResult> DeletePartnerBank(int partnerId)
        {
            // -> Id not PartnerCode
            var result = await _partnerBankServices.GetPartnerBankByPartnerIdAsync(partnerId);
            var mappedData = _mapper.Map<UpdatePartnerBankVm>(result);
            return await Task.FromResult(PartialView(mappedData));
        }

        /// <summary>
        /// Deletes the partner bank.
        /// </summary>
        /// <param name="deletePartnerBankVm">The delete partner bank vm.</param>
        /// <returns>A Task.</returns>
        [HttpPost]
        [LogUserActivity("deleted a partner bank")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePartnerBank([FromForm] UpdatePartnerBankVm deletePartnerBankVm)
        {
            if (deletePartnerBankVm.Id != 0)
            {
                var responseStatus = await _partnerBankServices.RemovePartnerBankAsync(deletePartnerBankVm);
                if (responseStatus.StatusCode == 200)
                {
                    _notyfService.Success(responseStatus.MsgText);
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
        #endregion
    }
}
