using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Mpmt.Core.ViewModel.DocumentType;
using Mpmt.Services.Services.DocumentType;
using Mpmt.Services.Services.RoleMenuPermission;
using Mpmt.Web.Common;
using Mpmt.Web.Filter;
using System.Net;

namespace Mpmt.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// The document type controller.
    /// </summary>
    [RolePremission]
    [AdminAuthorization]
    public class DocumentTypeController : BaseAdminController
    {
        private readonly IDocumentTypeServices _documentTypeServices;
        private readonly INotyfService _notyfService;
        private readonly IMapper _mapper;
        private readonly IRMPService _rMPService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentTypeController"/> class.
        /// </summary>
        /// <param name="documentTypeServices">The document type services.</param>
        /// <param name="notyfService">The notyf service.</param>
        /// <param name="mapper">The mapper.</param>
        public DocumentTypeController(IDocumentTypeServices documentTypeServices, INotyfService notyfService, IMapper mapper, IRMPService rMPService)
        {
            _documentTypeServices = documentTypeServices;
            _notyfService = notyfService;
            _mapper = mapper;
            _rMPService = rMPService;
        }

        /// <summary>
        /// Documents the type index.
        /// </summary>
        /// <returns>A Task.</returns>
        [HttpGet]
        public async Task<IActionResult> DocumentTypeIndex()
        {
            var actions = await _rMPService.GetActionPermissionListAsync("DocumentType");

            ViewBag.actions = actions;

            var result = await _documentTypeServices.GetDocumentTypeAsync();
            if (WebHelper.IsAjaxRequest(Request))
                return await Task.FromResult(PartialView("_DocumentTypeIndex", result));

            return await Task.FromResult(View(result));
        }

        #region Add-Document-Type

        /// <summary>
        /// Adds the document type.
        /// </summary>
        /// <returns>A Task.</returns>
        [HttpGet]
        public async Task<IActionResult> AddDocumentType()
        {
            return await Task.FromResult(PartialView());
        }

        /// <summary>
        /// Adds the document type.
        /// </summary>
        /// <param name="addDocumentTypeVm">The add document type vm.</param>
        /// <returns>A Task.</returns>
        [HttpPost]
        [LogUserActivity("added document type")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddDocumentType([FromForm] AddDocumentTypeVm addDocumentTypeVm)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView();
            }
            else
            {
                var responseStatus = await _documentTypeServices.AddDocumentTypeAsync(addDocumentTypeVm);
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

        #region Update-Document-Type

        /// <summary>
        /// Updates the document type.
        /// </summary>
        /// <param name="documentTypeId">The document type id.</param>
        /// <returns>A Task.</returns>
        [HttpGet]
        public async Task<IActionResult> UpdateDocumentType(int documentTypeId)
        {
            var result = await _documentTypeServices.GetDocumentTypeByIdAsync(documentTypeId);
            var mappedData = _mapper.Map<UpdateDocumentTypeVm>(result);
            return await Task.FromResult(PartialView(mappedData));
        }

        /// <summary>
        /// Updates the document type.
        /// </summary>
        /// <param name="updateDocumentTypeVm">The update document type vm.</param>
        /// <returns>A Task.</returns>
        [HttpPost]
        [LogUserActivity("updated document type")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateDocumentType([FromForm] UpdateDocumentTypeVm updateDocumentTypeVm)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView();
            }
            else
            {
                var responseStatus = await _documentTypeServices.UpdateDocumentTypeAsync(updateDocumentTypeVm);
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

        #region Delete-Document-Type

        /// <summary>
        /// Deletes the document type.
        /// </summary>
        /// <param name="documentTypeId">The document type id.</param>
        /// <returns>A Task.</returns>
        [HttpGet]
        public async Task<IActionResult> DeleteDocumentType(int documentTypeId)
        {
            var result = await _documentTypeServices.GetDocumentTypeByIdAsync(documentTypeId);
            var mappedData = _mapper.Map<UpdateDocumentTypeVm>(result);
            return await Task.FromResult(PartialView(mappedData));
        }

        /// <summary>
        /// Deletes the document type.
        /// </summary>
        /// <param name="deleteDocumentTypeVm">The delete document type vm.</param>
        /// <returns>A Task.</returns>
        [HttpPost]
        [LogUserActivity("deleted document type")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteDocumentType([FromForm] UpdateDocumentTypeVm deleteDocumentTypeVm)
        {
            if (deleteDocumentTypeVm.Id != 0)
            {
                var responseStatus = await _documentTypeServices.RemoveDocumentTypeAsync(deleteDocumentTypeVm);
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
