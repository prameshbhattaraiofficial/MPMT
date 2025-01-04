using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Mpmt.Core.Dtos.ServiceChargeCategory;
using Mpmt.Core.ViewModel.ServiceChargeCategory;
using Mpmt.Services.Services.RoleMenuPermission;
using Mpmt.Services.Services.ServiceChargeCategory;
using Mpmt.Web.Common;
using Mpmt.Web.Filter;
using System.Net;

namespace Mpmt.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// The service category controller.
    /// </summary>
    /// 
    [RolePremission]
    [AdminAuthorization]
    public class ServiceCategoryController : BaseAdminController
    {
        private readonly IServiceCategoryServices _serviceCategoryServices;
        private readonly INotyfService _notyfService;
        private readonly IMapper _mapper;
        private readonly IRMPService _rMPService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceCategoryController"/> class.
        /// </summary>
        /// <param name="serviceCategoryServices">The service category services.</param>
        /// <param name="notyfService">The notyf service.</param>
        /// <param name="mapper">The mapper.</param>
        public ServiceCategoryController(IServiceCategoryServices serviceCategoryServices, INotyfService notyfService, IMapper mapper, IRMPService rMPService)
        {
            _serviceCategoryServices = serviceCategoryServices;
            _notyfService = notyfService;
            _mapper = mapper;
            _rMPService = rMPService;
        }

        /// <summary>
        /// Services the category index.
        /// </summary>
        /// <param name="serviceCategoryFilter">The service category filter.</param>
        /// <returns>A Task.</returns>
        [HttpGet]
        public async Task<IActionResult> ServiceCategoryIndex([FromQuery] ServiceCategoryFilter serviceCategoryFilter)
        {
            var actions = await _rMPService.GetActionPermissionListAsync("ServiceCategory");
            ViewBag.actions = actions;

            var result = await _serviceCategoryServices.GetServiceCategoryAsync(serviceCategoryFilter);
            if (WebHelper.IsAjaxRequest(Request))
                return await Task.FromResult(PartialView("_ServiceCategoryIndex", result));

            return await Task.FromResult(View(result));
        }

        #region Add-Service-Category

        /// <summary>
        /// Adds the service category.
        /// </summary>
        /// <returns>A Task.</returns>
        [HttpGet]
        public async Task<IActionResult> AddServiceCategory()
        {
            return await Task.FromResult(PartialView());
        }

        /// <summary>
        /// Adds the service category.
        /// </summary>
        /// <param name="addServiceCategoryVm">The add service category vm.</param>
        /// <returns>A Task.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [LogUserActivity("added a service category")]
        public async Task<IActionResult> AddServiceCategory([FromForm] AddServiceCategoryVm addServiceCategoryVm)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView();
            }
            else
            {
                var responseStatus = await _serviceCategoryServices.AddServiceCategoryAsync(addServiceCategoryVm);
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

        #region Update-Service-Category

        /// <summary>
        /// Updates the service category.
        /// </summary>
        /// <param name="serviceCategoryId">The service category id.</param>
        /// <returns>A Task.</returns>
        [HttpGet]
        [LogUserActivity("updated a service category")]
        public async Task<IActionResult> UpdateServiceCategory(int serviceCategoryId)
        {
            var result = await _serviceCategoryServices.GetServiceCategoryByIdAsync(serviceCategoryId);
            var mappedData = _mapper.Map<UpdateServiceCategoryVm>(result);
            return await Task.FromResult(PartialView(mappedData));
        }

        /// <summary>
        /// Updates the service category.
        /// </summary>
        /// <param name="updateServiceCategoryVm">The update service category vm.</param>
        /// <returns>A Task.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [LogUserActivity("updated a service category")]
        public async Task<IActionResult> UpdateServiceCategory([FromForm] UpdateServiceCategoryVm updateServiceCategoryVm)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView();
            }
            else
            {
                var responseStatus = await _serviceCategoryServices.UpdateServiceCategoryAsync(updateServiceCategoryVm);
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

        #region Delete-Service-Category

        /// <summary>
        /// Deletes the service category.
        /// </summary>
        /// <param name="serviceCategoryId">The service category id.</param>
        /// <returns>A Task.</returns>
        [HttpGet]
        public async Task<IActionResult> DeleteServiceCategory(int serviceCategoryId)
        {
            var result = await _serviceCategoryServices.GetServiceCategoryByIdAsync(serviceCategoryId);
            var mappedData = _mapper.Map<UpdateServiceCategoryVm>(result);
            return await Task.FromResult(PartialView(mappedData));
        }

        /// <summary>
        /// Deletes the service category.
        /// </summary>
        /// <param name="deleteServiceCategoryVm">The delete service category vm.</param>
        /// <returns>A Task.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [LogUserActivity("deleted a service category")]
        public async Task<IActionResult> DeleteServiceCategory([FromForm] UpdateServiceCategoryVm deleteServiceCategoryVm)
        {
            if (deleteServiceCategoryVm.Id != 0)
            {
                var responseStatus = await _serviceCategoryServices.RemoveServiceCategoryAsync(deleteServiceCategoryVm);
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
