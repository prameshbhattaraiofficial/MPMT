using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Mpmt.Core.Dtos.IncomeSource;
using Mpmt.Services.Services.IncomeSource;
using Mpmt.Services.Services.RoleMenuPermission;
using Mpmt.Web.Areas.Admin.ViewModels.IncomeSource;
using Mpmt.Web.Common;
using Mpmt.Web.Filter;
using System.Net;

namespace Mpmt.Web.Areas.Admin.Controllers;
[RolePremission]
[AdminAuthorization]
public class IncomeSourceController : BaseAdminController
{
    private readonly IIncomeSourceService _incomeSourceService;
    private readonly INotyfService _notyfService;
    private readonly IMapper _mapper;
    private readonly IRMPService _rMPService;

    public IncomeSourceController(IIncomeSourceService incomeSourceService, INotyfService notyfService, IMapper mapper, IRMPService rMPService)
    {
        _incomeSourceService = incomeSourceService;
        _notyfService = notyfService;
        _mapper = mapper;
        _rMPService = rMPService;
    }

    public async Task<IActionResult> IncomeSourceIndex([FromQuery] IncomeSourceFilter sourceFilter)
    {
        var actions = await _rMPService.GetActionPermissionListAsync("IncomeSource");
        ViewBag.actions = actions;

        var result = await _incomeSourceService.GetIncomeSourceAsync(sourceFilter);
        if (WebHelper.IsAjaxRequest(Request))
            return await Task.FromResult(PartialView("_IncomeSourceIndex", result));
        return await Task.FromResult(View(result));
    }

    #region Add-Income-Source
    public async Task<IActionResult> AddIncomeSource()
    {
        return await Task.FromResult(PartialView());
    }

    [HttpPost]
    [LogUserActivity("added income source")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddIncomeSource(IncomeSourceVm incomeSource)
    {
        if (!ModelState.IsValid)
        {
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return PartialView();
        }
        else
        {
            var mappedData = _mapper.Map<IUDIncomeSource>(incomeSource);
            var responseStatus = await _incomeSourceService.AddIncomeSourceAsync(mappedData, User);
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

    #region Update-Income-Source
    public async Task<IActionResult> UpdateIncomeSource(int id)
    {
        var result = await _incomeSourceService.GetIncomeSourceByIdAsync(id);
        var mappedData = _mapper.Map<IncomeSourceVm>(result);
        return await Task.FromResult(PartialView(mappedData));
    }

    [HttpPost]
    [LogUserActivity("updated income source")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateIncomeSource(IncomeSourceVm incomeSource)
    {
        if (!ModelState.IsValid)
        {
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return PartialView();
        }
        else
        {
            var mappedData = _mapper.Map<IUDIncomeSource>(incomeSource);
            var responseStatus = await _incomeSourceService.UpdateIncomeSourceAsync(mappedData, User);
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

    #region Delete-Income-Source
    public async Task<IActionResult> DeleteIncomeSource(int id)
    {
        var result = await _incomeSourceService.GetIncomeSourceByIdAsync(id);
        var mappedData = _mapper.Map<IncomeSourceVm>(result);
        return await Task.FromResult(PartialView(mappedData));
    }

    [HttpPost]
    [LogUserActivity("deleted income source")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteIncomeSource(IncomeSourceVm incomeSource)
    {
        if (incomeSource.Id != 0)
        {
            var mappedData = _mapper.Map<IUDIncomeSource>(incomeSource);
            var responseStatus = await _incomeSourceService.DeleteIncomeSourceAsync(mappedData, User);
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
