using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Mpmt.Core.Dtos.TransferPurpose;
using Mpmt.Core.ViewModel.TransferPurpose;
using Mpmt.Services.Services.RoleMenuPermission;
using Mpmt.Services.Services.TransferPurpose;
using Mpmt.Web.Common;
using Mpmt.Web.Filter;
using System.Net;

namespace Mpmt.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// The transfer purpose controller.
    /// </summary>
    [RolePremission]
    [AdminAuthorization]
    public class TransferPurposeController : BaseAdminController
    {
        private readonly ITransferPurposeServices _transferPurposeServices;
        private readonly INotyfService _notyfService;
        private readonly IMapper _mapper;
        private readonly IRMPService _rMPService;

        public TransferPurposeController(ITransferPurposeServices transferPurposeServices, INotyfService notyfService, IMapper mapper, IRMPService rMPService)
        {
            _transferPurposeServices = transferPurposeServices;
            _notyfService = notyfService;
            _mapper = mapper;
            _rMPService = rMPService;
        }

        [HttpGet]
        public async Task<IActionResult> TransferPurposeIndex([FromQuery] TransferPurposeFilter transferPurposeFilter)
        {
            var actions = await _rMPService.GetActionPermissionListAsync("TransferPurpose");
            ViewBag.actions = actions;

            var result = await _transferPurposeServices.GetTransferPurposeAsync(transferPurposeFilter);

            if (WebHelper.IsAjaxRequest(Request))
                return await Task.FromResult(PartialView("_TransferPurposeIndex", result));

            return await Task.FromResult(View(result));
        }

        #region Add-Transfer-Purpose

        /// <summary>
        /// Adds the transfer purpose.
        /// </summary>
        /// <returns>A Task.</returns>
        [HttpGet]
        public async Task<IActionResult> AddTransferPurpose()
        {
            return await Task.FromResult(PartialView());
        }

        /// <summary>
        /// Adds the transfer purpose.
        /// </summary>
        /// <param name="addTransferVm">The add transfer vm.</param>
        /// <returns>A Task.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [LogUserActivity("added a transfer purpose")]
        public async Task<IActionResult> AddTransferPurpose([FromForm] AddTransferVm addTransferVm)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView();
            }
            else
            {
                var responseStatus = await _transferPurposeServices.AddTransferPurposeAsync(addTransferVm);
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

        #region Update-Transfer-Purpose

        /// <summary>
        /// Updates the transfer purpose.
        /// </summary>
        /// <param name="transferPurposeId">The transfer purpose id.</param>
        /// <returns>A Task.</returns>
        [HttpGet]
        public async Task<IActionResult> UpdateTransferPurpose(int transferPurposeId)
        {
            var result = await _transferPurposeServices.GetTransferPurposeByIdAsync(transferPurposeId);
            var mappedData = _mapper.Map<UpdateTransferVm>(result);
            return await Task.FromResult(PartialView(mappedData));
        }

        /// <summary>
        /// Updates the transfer purpose.
        /// </summary>
        /// <param name="updateTransferVm">The update transfer vm.</param>
        /// <returns>A Task.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [LogUserActivity("updated a transfer purpose")]
        public async Task<IActionResult> UpdateTransferPurpose([FromForm] UpdateTransferVm updateTransferVm)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView();
            }
            else
            {
                var responseStatus = await _transferPurposeServices.UpdateTransferPurposeAsync(updateTransferVm);
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

        #region Delete-Transfer-Purpose

        /// <summary>
        /// Deletes the transfer purpose.
        /// </summary>
        /// <param name="transferPurposeId">The transfer purpose id.</param>
        /// <returns>A Task.</returns>
        [HttpGet]
        public async Task<IActionResult> DeleteTransferPurpose(int transferPurposeId)
        {
            var result = await _transferPurposeServices.GetTransferPurposeByIdAsync(transferPurposeId);
            var mappedData = _mapper.Map<UpdateTransferVm>(result);
            return await Task.FromResult(PartialView(mappedData));
        }

        /// <summary>
        /// Deletes the transfer purpose.
        /// </summary>
        /// <param name="deleteTransferVm">The delete transfer vm.</param>
        /// <returns>A Task.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [LogUserActivity("deleted a transfer purpose")]
        public async Task<IActionResult> DeleteTransferPurpose([FromForm] UpdateTransferVm deleteTransferVm)
        {
            if (deleteTransferVm.Id != 0)
            {
                var responseStatus = await _transferPurposeServices.RemoveTransferPurposeAsync(deleteTransferVm);
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
