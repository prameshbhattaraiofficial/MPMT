using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Mpmt.Core.Dtos.PaymentType;
using Mpmt.Core.ViewModel.PaymentType;
using Mpmt.Services.Services.PaymentType;
using Mpmt.Services.Services.RoleMenuPermission;
using Mpmt.Web.Common;
using Mpmt.Web.Filter;
using System.Net;

namespace Mpmt.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// The payment type controller.
    /// </summary>
    [RolePremission]
    [AdminAuthorization]
    public class PaymentTypeController : BaseAdminController
    {
        private readonly IPaymentTypeServices _paymentTypeServices;
        private readonly INotyfService _notyfService;
        private readonly IMapper _mapper;
        private readonly IRMPService _rMPService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentTypeController"/> class.
        /// </summary>
        /// <param name="paymentTypeServices">The payment type services.</param>
        /// <param name="notyfService">The notyf service.</param>
        /// <param name="mapper">The mapper.</param>
        public PaymentTypeController(IPaymentTypeServices paymentTypeServices, INotyfService notyfService, IMapper mapper, IRMPService rMPService)
        {
            _paymentTypeServices = paymentTypeServices;
            _notyfService = notyfService;
            _mapper = mapper;
            _rMPService = rMPService;
        }

        /// <summary>
        /// Payments the type index.
        /// </summary>
        /// <param name="PaymentTypeFilter">The payment type filter.</param>
        /// <returns>A Task.</returns>
        [HttpGet]
        public async Task<IActionResult> PaymentTypeIndex([FromQuery] PaymentTypeFilter PaymentTypeFilter)
        {
            var actions = await _rMPService.GetActionPermissionListAsync("PaymentType");
            ViewBag.actions = actions;

            var result = await _paymentTypeServices.GetPaymentTypeAsync(PaymentTypeFilter);
            if (WebHelper.IsAjaxRequest(Request))
                return await Task.FromResult(PartialView("_PaymentTypeIndex", result));

            return await Task.FromResult(View(result));
        }

        #region Add-Conversion-Rate

        /// <summary>
        /// Adds the payment type.
        /// </summary>
        /// <returns>A Task.</returns>
        [HttpGet]
        public async Task<IActionResult> AddPaymentType()
        {
            return await Task.FromResult(PartialView());
        }

        /// <summary>
        /// Adds the payment type.
        /// </summary>
        /// <param name="addPaymentTypeVm">The add payment type vm.</param>
        /// <returns>A Task.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [LogUserActivity("added a payment type")]
        public async Task<IActionResult> AddPaymentType([FromForm] AddPaymentTypeVm addPaymentTypeVm)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView();
            }
            else
            {
                var responseStatus = await _paymentTypeServices.AddPaymentTypeAsync(addPaymentTypeVm);
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

        #region Update-Conversion-Rate

        /// <summary>
        /// Updates the payment type.
        /// </summary>
        /// <param name="paymentTypeId">The payment type id.</param>
        /// <returns>A Task.</returns>
        [HttpGet]
        public async Task<IActionResult> UpdatePaymentType(int paymentTypeId)
        {
            var result = await _paymentTypeServices.GetPaymentTypeByIdAsync(paymentTypeId);
            var mappedData = _mapper.Map<UpdatePaymentTypeVm>(result);
            return await Task.FromResult(PartialView(mappedData));
        }

        /// <summary>
        /// Updates the payment type.
        /// </summary>
        /// <param name="updatePaymentTypeVm">The update payment type vm.</param>
        /// <returns>A Task.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [LogUserActivity("updated a payment type")]
        public async Task<IActionResult> UpdatePaymentType([FromForm] UpdatePaymentTypeVm updatePaymentTypeVm)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView();
            }
            else
            {
                var responseStatus = await _paymentTypeServices.UpdatePaymentTypeAsync(updatePaymentTypeVm);
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

        #region Delete-Conversion-Rate

        /// <summary>
        /// Deletes the payment type.
        /// </summary>
        /// <param name="paymentTypeId">The payment type id.</param>
        /// <returns>A Task.</returns>
        [HttpGet]
        public async Task<IActionResult> DeletePaymentType(int paymentTypeId)
        {
            var result = await _paymentTypeServices.GetPaymentTypeByIdAsync(paymentTypeId);
            var mappedData = _mapper.Map<UpdatePaymentTypeVm>(result);
            return await Task.FromResult(PartialView(mappedData));
        }

        /// <summary>
        /// Deletes the payment type.
        /// </summary>
        /// <param name="deletePaymentTypeVm">The delete payment type vm.</param>
        /// <returns>A Task.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [LogUserActivity("deleted a payment type")]
        public async Task<IActionResult> DeletePaymentType([FromForm] UpdatePaymentTypeVm deletePaymentTypeVm)
        {
            if (deletePaymentTypeVm.Id != 0)
            {
                var responseStatus = await _paymentTypeServices.RemovePaymentTypeAsync(deletePaymentTypeVm);
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
