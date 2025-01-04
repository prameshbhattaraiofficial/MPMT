using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Mpmt.Core.Dtos.KYCRemark;
using Mpmt.Core.ViewModel.KYCRemark;
using Mpmt.Services.Services.KYCRemark;
using Mpmt.Services.Services.RoleMenuPermission;
using Mpmt.Web.Common;
using Mpmt.Web.Filter;
using System.Net;

namespace Mpmt.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// The kyc remark controller.
    /// </summary>
    [RolePremission]
    [AdminAuthorization]
    public class KycRemarkController : BaseAdminController
    {
        private readonly IKycRemarkServices _kycRemarkServices;
        private readonly INotyfService _notyfService;
        private readonly IMapper _mapper;
        private readonly IRMPService _rMPService;

        /// <summary>
        /// Initializes a new instance of the <see cref="KycRemarkController"/> class.
        /// </summary>
        /// <param name="kycRemarkServices">The kyc remark services.</param>
        /// <param name="notyfService">The notyf service.</param>
        /// <param name="mapper">The mapper.</param>
        public KycRemarkController(IKycRemarkServices kycRemarkServices, INotyfService notyfService, IMapper mapper, IRMPService rMPService)
        {
            _kycRemarkServices = kycRemarkServices;
            _notyfService = notyfService;
            _mapper = mapper;
            _rMPService = rMPService;
        }

        /// <summary>
        /// Kycs the remark index.
        /// </summary>
        /// <param name="kycRemarkFilter">The kyc remark filter.</param>
        /// <returns>A Task.</returns>
        [HttpGet]
        public async Task<IActionResult> KycRemarkIndex([FromQuery] KycRemarkFilter kycRemarkFilter)
        {
            var actions = await _rMPService.GetActionPermissionListAsync("KycRemark");
            ViewBag.actions = actions;

            var result = await _kycRemarkServices.GetKycRemarkAsync(kycRemarkFilter);
            if (WebHelper.IsAjaxRequest(Request))
                return await Task.FromResult(PartialView("_KycRemarkIndex", result));

            return await Task.FromResult(View(result));
        }

        #region Add-KYC-Remark

        /// <summary>
        /// Adds the kyc remark.
        /// </summary>
        /// <returns>A Task.</returns>
        [HttpGet]
        public async Task<IActionResult> AddKycRemark()
        {
            return await Task.FromResult(PartialView());
        }

        /// <summary>
        /// Adds the kyc remark.
        /// </summary>
        /// <param name="addKycRemarkVm">The add kyc remark vm.</param>
        /// <returns>A Task.</returns>
        [HttpPost]
        [LogUserActivity("added KYC remarks")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddKycRemark([FromForm] AddKycRemarkVm addKycRemarkVm)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView();
            }
            else
            {
                var responseStatus = await _kycRemarkServices.AddKycRemarkAsync(addKycRemarkVm);
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

        #region Update-KYC-Remark

        /// <summary>
        /// Updates the kyc remark.
        /// </summary>
        /// <param name="kycRemarkId">The kyc remark id.</param>
        /// <returns>A Task.</returns>
        [HttpGet]
        public async Task<IActionResult> UpdateKycRemark(int kycRemarkId)
        {
            var result = await _kycRemarkServices.GetKycRemarkByIdAsync(kycRemarkId);
            var mappedData = _mapper.Map<UpdateKycRemarkVm>(result);
            return await Task.FromResult(PartialView(mappedData));
        }

        /// <summary>
        /// Updates the kyc remark.
        /// </summary>
        /// <param name="updateKycRemarkVm">The update kyc remark vm.</param>
        /// <returns>A Task.</returns>
        [HttpPost]
        [LogUserActivity("updated KYC remarks")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateKycRemark([FromForm] UpdateKycRemarkVm updateKycRemarkVm)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView();
            }
            else
            {
                var responseStatus = await _kycRemarkServices.UpdateKycRemarkAsync(updateKycRemarkVm);
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

        #region Delete-KYC-Remark

        /// <summary>
        /// Deletes the kyc remark.
        /// </summary>
        /// <param name="kycRemarkId">The kyc remark id.</param>
        /// <returns>A Task.</returns>
        [HttpGet]
        public async Task<IActionResult> DeleteKycRemark(int kycRemarkId)
        {
            var result = await _kycRemarkServices.GetKycRemarkByIdAsync(kycRemarkId);
            var mappedData = _mapper.Map<UpdateKycRemarkVm>(result);
            return await Task.FromResult(PartialView(mappedData));
        }

        /// <summary>
        /// Deletes the kyc remark.
        /// </summary>
        /// <param name="deleteKycRemarkVm">The delete kyc remark vm.</param>
        /// <returns>A Task.</returns>
        [HttpPost]
        [LogUserActivity("deleted KYC remarks")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteKycRemark([FromForm] UpdateKycRemarkVm deleteKycRemarkVm)
        {
            if (deleteKycRemarkVm.Id != 0)
            {
                var responseStatus = await _kycRemarkServices.RemoveKycRemarkAsync(deleteKycRemarkVm);
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
