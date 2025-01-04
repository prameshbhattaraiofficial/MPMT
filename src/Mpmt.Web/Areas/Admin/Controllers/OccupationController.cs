using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Mpmt.Core.Dtos.Occupation;
using Mpmt.Core.ViewModel.Occupation;
using Mpmt.Services.Services.Occupation;
using Mpmt.Services.Services.RoleMenuPermission;
using Mpmt.Web.Common;
using Mpmt.Web.Filter;
using System.Net;

namespace Mpmt.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// The occupation controller.
    /// </summary>
    [RolePremission]
    [AdminAuthorization]
    public class OccupationController : BaseAdminController
    {
        private readonly IOccupationServices _occupationServices;
        private readonly IRMPService _rMPService;
        private readonly INotyfService _notyfService;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="OccupationController"/> class.
        /// </summary>
        /// <param name="notyfService">The notyf service.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="occupationServices">The occupation services.</param>
        public OccupationController(INotyfService notyfService, IMapper mapper, IOccupationServices occupationServices, IRMPService rMPService)
        {
            _occupationServices = occupationServices;
            _rMPService = rMPService;
            _mapper = mapper;
            _notyfService = notyfService;
        }

        /// <summary>
        /// Occupations the index.
        /// </summary>
        /// <param name="occupationFilter">The occupation filter.</param>
        /// <returns>A Task.</returns>
        [HttpGet]
        public async Task<IActionResult> OccupationIndex([FromQuery] OccupationFilter occupationFilter)
        {
            var actions = await _rMPService.GetActionPermissionListAsync("Occupation");
            ViewBag.actions = actions;

            var result = await _occupationServices.GetOccupationAsync(occupationFilter);
            if (WebHelper.IsAjaxRequest(Request))
                return await Task.FromResult(PartialView("_OccupationIndex", result));

            return await Task.FromResult(View(result));
        }

        /// <summary>
        /// Adds the occupation.
        /// </summary>
        /// <returns>A Task.</returns>
        [HttpGet]
        public async Task<IActionResult> AddOccupation()
        {
            return await Task.FromResult(PartialView());
        }

        /// <summary>
        /// Adds the occupation.
        /// </summary>
        /// <param name="addOccupationVm">The add occupation vm.</param>
        /// <returns>A Task.</returns>
        [HttpPost]
        [LogUserActivity("added occupation")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOccupation([FromForm] AddOccupationVm addOccupationVm)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView();
            }
            else
            {
                var responseStatus = await _occupationServices.AddOccupationAsync(addOccupationVm);
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

        /// <summary>
        /// Updates the occupation.
        /// </summary>
        /// <param name="occupationId">The occupation id.</param>
        /// <returns>A Task.</returns>
        [HttpGet]
        public async Task<IActionResult> UpdateOccupation(int occupationId)
        {
            var result = await _occupationServices.GetOccupationByIdAsync(occupationId);
            var mappedData = _mapper.Map<UpdateOccupationVm>(result);
            return await Task.FromResult(PartialView(mappedData));
        }

        /// <summary>
        /// Updates the occupation.
        /// </summary>
        /// <param name="updateOccupationVm">The update occupation vm.</param>
        /// <returns>A Task.</returns>
        [HttpPost]
        [LogUserActivity("updated occupation")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateOccupation([FromForm] UpdateOccupationVm updateOccupationVm)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView();
            }
            else
            {
                var responseStatus = await _occupationServices.UpdateOccupationAsync(updateOccupationVm);
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

        /// <summary>
        /// Deletes the occupation.
        /// </summary>
        /// <param name="occupationId">The occupation id.</param>
        /// <returns>A Task.</returns>
        [HttpGet]
        public async Task<IActionResult> DeleteOccupation(int occupationId)
        {
            var result = await _occupationServices.GetOccupationByIdAsync(occupationId);
            var mappedData = _mapper.Map<UpdateOccupationVm>(result);
            return await Task.FromResult(PartialView(mappedData));
        }

        /// <summary>
        /// Deletes the occupation.
        /// </summary>
        /// <param name="deleteOccupationVm">The delete occupation vm.</param>
        /// <returns>A Task.</returns>
        [HttpPost]
        [LogUserActivity("deleted occupation")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteOccupation([FromForm] UpdateOccupationVm deleteOccupationVm)
        {
            if (deleteOccupationVm.Id != 0)
            {
                var responseStatus = await _occupationServices.RemoveOccupationAsync(deleteOccupationVm);
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
    }
}