using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Mpmt.Core.Dtos.Relation;
using Mpmt.Core.ViewModel.Relation;
using Mpmt.Services.Services.Relation;
using Mpmt.Services.Services.RoleMenuPermission;
using Mpmt.Web.Common;
using Mpmt.Web.Filter;
using System.Net;

namespace Mpmt.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// The relation controller.
    /// </summary>
    [RolePremission]
    [AdminAuthorization]
    public class RelationController : BaseAdminController
    {
        private readonly IRelationServices _relationServices;
        private readonly INotyfService _notyfService;
        private readonly IMapper _mapper;
        private readonly IRMPService _rMPService;

        /// <summary>
        /// Initializes a new instance of the <see cref="RelationController"/> class.
        /// </summary>
        /// <param name="relationServices">The relation services.</param>
        /// <param name="notyfService">The notyf service.</param>
        /// <param name="mapper">The mapper.</param>
        public RelationController(IRelationServices relationServices, INotyfService notyfService, IMapper mapper, IRMPService rMPService)
        {
            _relationServices = relationServices;
            _notyfService = notyfService;
            _mapper = mapper;
            _rMPService = rMPService;
        }

        /// <summary>
        /// Relations the index.
        /// </summary>
        /// <param name="relationFilter">The relation filter.</param>
        /// <returns>A Task.</returns>
        [HttpGet]
        public async Task<IActionResult> RelationIndex([FromQuery] RelationFilter relationFilter)
        {
            var actions = await _rMPService.GetActionPermissionListAsync("Relation");
            ViewBag.actions = actions;


            var result = await _relationServices.GetRelationAsync(relationFilter);
            if (WebHelper.IsAjaxRequest(Request))
                return await Task.FromResult(PartialView("_RelationIndex", result));

            return await Task.FromResult(View(result));
        }

        /// <summary>
        /// Adds the relation.
        /// </summary>
        /// <returns>A Task.</returns>
        [HttpGet]
        public async Task<IActionResult> AddRelation()
        {
            return await Task.FromResult(PartialView());
        }

        /// <summary>
        /// Adds the relation.
        /// </summary>
        /// <param name="addRelationVm">The add relation vm.</param>
        /// <returns>A Task.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [LogUserActivity("added a new relation")]
        public async Task<IActionResult> AddRelation([FromForm] AddRelationVm addRelationVm)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView();
            }
            else
            {
                var responseStatus = await _relationServices.AddRelationAsync(addRelationVm);
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
        /// Updates the relation.
        /// </summary>
        /// <param name="relationId">The relation id.</param>
        /// <returns>A Task.</returns>
        [HttpGet]
        public async Task<IActionResult> UpdateRelation(int relationId)
        {
            var result = await _relationServices.GetRelationByIdAsync(relationId);
            var mappedData = _mapper.Map<UpdateRelationVm>(result);
            return await Task.FromResult(PartialView(mappedData));
        }

        /// <summary>
        /// Updates the relation.
        /// </summary>
        /// <param name="updateRelationVm">The update relation vm.</param>
        /// <returns>A Task.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [LogUserActivity("updated a relation")]
        public async Task<IActionResult> UpdateRelation([FromForm] UpdateRelationVm updateRelationVm)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView();
            }
            else
            {
                var responseStatus = await _relationServices.UpdateRelationAsync(updateRelationVm);
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
        /// Deletes the relation.
        /// </summary>
        /// <param name="relationId">The relation id.</param>
        /// <returns>A Task.</returns>
        [HttpGet]
        public async Task<IActionResult> DeleteRelation(int relationId)
        {
            var result = await _relationServices.GetRelationByIdAsync(relationId);
            var mappedData = _mapper.Map<UpdateRelationVm>(result);
            return await Task.FromResult(PartialView(mappedData));
        }

        /// <summary>
        /// Deletes the relation.
        /// </summary>
        /// <param name="deleteRelationVm">The delete relation vm.</param>
        /// <returns>A Task.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [LogUserActivity("deleted a relation")]
        public async Task<IActionResult> DeleteRelation([FromForm] UpdateRelationVm deleteRelationVm)
        {
            if (deleteRelationVm.Id != 0)
            {
                var responseStatus = await _relationServices.RemoveRelationAsync(deleteRelationVm);
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
