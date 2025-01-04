using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Mpmt.Core.Dtos.AddressProofType;
using Mpmt.Services.Services.AddressProofType;
using Mpmt.Services.Services.RoleMenuPermission;
using Mpmt.Web.Areas.Admin.ViewModels.AddressProofType;
using Mpmt.Web.Common;
using Mpmt.Web.Filter;
using System.Net;

namespace Mpmt.Web.Areas.Admin.Controllers;
[RolePremission]
[AdminAuthorization]
public class AddressProofTypeController : BaseAdminController
{
    private readonly IAddressProofTypeService _addressProofTypeService;
    private readonly INotyfService _notyfService;
    private readonly IMapper _mapper;
    private readonly IRMPService _rMPService;

    public AddressProofTypeController(IAddressProofTypeService addressProofTypeService, INotyfService notyfService, IMapper mapper, IRMPService rMPService)
    {
        _addressProofTypeService = addressProofTypeService;
        _notyfService = notyfService;
        _mapper = mapper;
        _rMPService = rMPService;
    }

    public async Task<IActionResult> ProofTypeIndex()
    {
        var actions = await _rMPService.GetActionPermissionListAsync("AddressProofType");
        ViewBag.actions = actions;
        var result = await _addressProofTypeService.GetAddressProofTypesAsync();
        if (WebHelper.IsAjaxRequest(Request))
            return await Task.FromResult(PartialView("_ProofTypeIndex", result));
        return await Task.FromResult(View(result));
    }

    #region Add-Proof-Type
    public async Task<IActionResult> AddProofType()
    {
        return await Task.FromResult(PartialView());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [LogUserActivity("added proof type")]

    public async Task<IActionResult> AddProofType(AddressProofTypeVm addressProofType)
    {
        if (!ModelState.IsValid)
        {
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return PartialView();
        }
        else
        {
            var mappedData = _mapper.Map<IUDAddressProofType>(addressProofType);
            var responseStatus = await _addressProofTypeService.AddAddressProofTypeAsync(mappedData, User);
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

    #region Update-Proof-Type
    public async Task<IActionResult> UpdateProofType(int id)
    {
        var result = await _addressProofTypeService.GetAddressProofTypeByIdAsync(id);
        var mappedData = _mapper.Map<AddressProofTypeVm>(result);
        return await Task.FromResult(PartialView(mappedData));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [LogUserActivity("updated proof type")]

    public async Task<IActionResult> UpdateProofType(AddressProofTypeVm addressProofType)
    {
        if (!ModelState.IsValid)
        {
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return PartialView();
        }
        else
        {
            var mappedData = _mapper.Map<IUDAddressProofType>(addressProofType);
            var responseStatus = await _addressProofTypeService.UpdateAddressProofTypeAsync(mappedData, User);
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

    #region Delete-Proof-Type
    public async Task<IActionResult> DeleteProofType(int id)
    {
        var result = await _addressProofTypeService.GetAddressProofTypeByIdAsync(id);
        var mappedData = _mapper.Map<AddressProofTypeVm>(result);
        return await Task.FromResult(PartialView(mappedData));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [LogUserActivity("deleted proof type")]

    public async Task<IActionResult> DeleteProofType(AddressProofTypeVm addressProofType)
    {
        if (addressProofType.Id != 0)
        {
            var mappedData = _mapper.Map<IUDAddressProofType>(addressProofType);
            var responseStatus = await _addressProofTypeService.DeleteAddressProofTypeAsync(mappedData, User);
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