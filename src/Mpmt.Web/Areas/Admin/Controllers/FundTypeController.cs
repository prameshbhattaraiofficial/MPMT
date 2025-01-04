using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Mpmt.Core.ViewModel.FundType;
using Mpmt.Services.Services.FundType;
using Mpmt.Services.Services.RoleMenuPermission;
using Mpmt.Web.Common;
using Mpmt.Web.Filter;
using System.Net;

namespace Mpmt.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// The fund type controller.
    /// </summary>
    [RolePremission]
    [AdminAuthorization]
    public class FundTypeController : BaseAdminController
    {
        private readonly IFundTypeServices _fundTypeServices;
        private readonly INotyfService _notyfService;
        private readonly IMapper _mapper;
        private readonly IRMPService _rMPService;

        /// <summary>
        /// Initializes a new instance of the <see cref="FundTypeController"/> class.
        /// </summary>
        /// <param name="fundTypeServices">The fund type services.</param>
        /// <param name="notyfService">The notyf service.</param>
        /// <param name="mapper">The mapper.</param>
        public FundTypeController(IFundTypeServices fundTypeServices, INotyfService notyfService, IMapper mapper, IRMPService rMPService)
        {
            _fundTypeServices = fundTypeServices;
            _notyfService = notyfService;
            _mapper = mapper;
            _rMPService = rMPService;
        }

        /// <summary>
        /// Funds the type index.
        /// </summary>
        /// <returns>A Task.</returns>
        [HttpGet]
        public async Task<IActionResult> FundTypeIndex()
        {
            var actions = await _rMPService.GetActionPermissionListAsync("FundType");
            ViewBag.actions = actions;

            var result = await _fundTypeServices.GetFundTypeAsync();
            if (WebHelper.IsAjaxRequest(Request))
                return await Task.FromResult(PartialView("_FundTypeIndex", result));

            return await Task.FromResult(View(result));
        }

        #region Add-Fund-Type

        /// <summary>
        /// Adds the fund type.
        /// </summary>
        /// <returns>A Task.</returns>
        [HttpGet]
        public async Task<IActionResult> AddFundType()
        {
            return await Task.FromResult(PartialView());
        }

        /// <summary>
        /// Adds the fund type.
        /// </summary>
        /// <param name="addFundTypeVm">The add fund type vm.</param>
        /// <returns>A Task.</returns>
        [HttpPost]
        [LogUserActivity("added fund type")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddFundType([FromForm] AddFundTypeVm addFundTypeVm)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView();
            }
            else
            {
                var responseStatus = await _fundTypeServices.AddFundTypeAsync(addFundTypeVm);
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

        #region Update-Fund-Type

        /// <summary>
        /// Updates the fund type.
        /// </summary>
        /// <param name="fundTypeId">The fund type id.</param>
        /// <returns>A Task.</returns>
        [HttpGet]
        public async Task<IActionResult> UpdateFundType(int fundTypeId)
        {
            var result = await _fundTypeServices.GetFundTypeByIdAsync(fundTypeId);
            var mappedData = _mapper.Map<UpdateFundTypeVm>(result);
            return await Task.FromResult(PartialView(mappedData));
        }

        /// <summary>
        /// Updates the fund type.
        /// </summary>
        /// <param name="updateFundTypeVm">The update fund type vm.</param>
        /// <returns>A Task.</returns>
        [HttpPost]
        [LogUserActivity("updated fund type")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateFundType([FromForm] UpdateFundTypeVm updateFundTypeVm)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView();
            }
            else
            {
                var responseStatus = await _fundTypeServices.UpdateFundTypeAsync(updateFundTypeVm);
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

        #region Delete-Fund-Type

        /// <summary>
        /// Deletes the fund type.
        /// </summary>
        /// <param name="fundTypeId">The fund type id.</param>
        /// <returns>A Task.</returns>
        [HttpGet]
        public async Task<IActionResult> DeleteFundType(int fundTypeId)
        {
            var result = await _fundTypeServices.GetFundTypeByIdAsync(fundTypeId);
            var mappedData = _mapper.Map<UpdateFundTypeVm>(result);
            return await Task.FromResult(PartialView(mappedData));
        }

        /// <summary>
        /// Deletes the fund type.
        /// </summary>
        /// <param name="deleteFundTypeVm">The delete fund type vm.</param>
        /// <returns>A Task.</returns>
        [HttpPost]
        [LogUserActivity("deleted fund type")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteFundType([FromForm] UpdateFundTypeVm deleteFundTypeVm)
        {
            if (deleteFundTypeVm.Id != 0)
            {
                var responseStatus = await _fundTypeServices.RemoveFundTypeAsync(deleteFundTypeVm);
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
