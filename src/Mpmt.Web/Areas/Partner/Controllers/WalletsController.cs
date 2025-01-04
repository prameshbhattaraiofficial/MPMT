using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Mpmt.Core.Common;
using Mpmt.Core.Domain.Modules;
using Mpmt.Core.Dtos.DropDown;
using Mpmt.Core.Dtos.FeeAccount;
using Mpmt.Core.Dtos.Partner;
using Mpmt.Core.Dtos.PartnerStatement;
using Mpmt.Core.Dtos.PrefundRequest;
using Mpmt.Core.Dtos.WalletLoad.Statement;
using Mpmt.Core.Extensions;
using Mpmt.Services.Hubs;
using Mpmt.Services.Partner;
using Mpmt.Services.Partner.IService;
using Mpmt.Services.Services.Common;
using Mpmt.Services.Services.Notification;
using Mpmt.Services.Services.PreFund;
using Mpmt.Services.Services.RoleMenuPermission;
using Mpmt.Web.Areas.Admin.ViewModels.Paetner;
using Mpmt.Web.Common;
using Mpmt.Web.Filter;
using System.Data;
using System.Net;
using System.Security.Claims;

namespace Mpmt.Web.Areas.Partner.Controllers;

[PartnerAuthorization]
[RolePremission]
public class WalletsController : BasePartnerController
{
    private readonly INotificationService _notificationService;
    private readonly IHubContext<UserHub> _hubcontext;
    private readonly IPartnerService _partnerService;
    private readonly IPartnerWalletCurrencyServices _partnerWalletCurrencyServices;
    private readonly IPreFundRequestServices _fundRequestServices;
    private readonly IRMPService _rMPService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ICommonddlServices _commonddl;
    private readonly IPartnerRegistrationService _partnerRegistrationService;
    private readonly IMapper _mapper;
    private readonly INotyfService _notyfService;

    public WalletsController(INotificationService notificationService,
        IHubContext<UserHub> hubcontext,
        IPartnerService partnerService, IPartnerWalletCurrencyServices partnerWalletCurrencyServices, IMapper mapper,
        INotyfService notyfService, ICommonddlServices commonddl, IPreFundRequestServices fundRequestServices, IRMPService rMPService, IHttpContextAccessor httpContextAccessor, IPartnerRegistrationService partnerRegistrationService)
    {
        _notificationService = notificationService;
        _hubcontext = hubcontext;
        _partnerService = partnerService;
        _partnerWalletCurrencyServices = partnerWalletCurrencyServices;
        _mapper = mapper;
        _notyfService = notyfService;
        _commonddl = commonddl;
        _fundRequestServices = fundRequestServices;
        _rMPService = rMPService;
        _httpContextAccessor = httpContextAccessor;
        _partnerRegistrationService = partnerRegistrationService;
    }

    #region List,View-PrefundRequest

    [HttpGet]
    public async Task<IActionResult> PreFundIndex([FromQuery] PrefundRequestFilter requestFilter)
    {
        IEnumerable<Commonddl> statusdetailDdl = await _commonddl.GetStatusListDdl();
        ViewBag.statusListDdl = new SelectList(statusdetailDdl, "value", "Text");

        var actions = await _rMPService.GetPartnerActionPermissionList("Wallets");
        ViewBag.actions = actions;

        Response.Headers["Cache-Control"] = "no-store, no-cache, must-revalidate";
        Response.Headers["Pragma"] = "no-cache";
        Response.Headers["Expires"] = "0";
        string partnercode = User.Claims.FirstOrDefault(x => x.Type == "PartnerCode").Value;
        requestFilter.PartnerCode = partnercode;
        var data = await _fundRequestServices.GetPreFundRequestAsync(requestFilter);
        if (WebHelper.IsAjaxRequest(Request))
        {
            return PartialView("_PreFundList", data);
        }

        return View(data);
    }

    [HttpGet]
    public async Task<IActionResult> PreFundDetails(int Id)
    {
        var data = await _fundRequestServices.GetPreFundRequestByIdAsync(Id);
        return PartialView(data);
    }

    #endregion List,View-PrefundRequest

    #region WalletCurrencyList

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        int partnerId = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "Id").Value);
        var PartnerById = new AppPartner();
        var email = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
        var parOremp = await _partnerRegistrationService.CheckPartnerOrEmployee(email);

        if (parOremp == "PartnerEmployee")
        {
            PartnerById = await _partnerService.GetPartnerEmployeeByEmailAsync(email);
        }
        else
        {
            PartnerById = await _partnerService.GetPartnerByIdAsync(partnerId);
        }

        var partnerwalletcurrencyList = await _partnerWalletCurrencyServices.GetPartnerWalletCurrencyBalance(PartnerById.PartnerCode);
        var walletcurrency = new WalletCurrency { apppartner = PartnerById, PartnerWalletCurrency = partnerwalletcurrencyList };
        ViewBag.partnerFundType = PartnerById.FundType;
        var partnercode = User.Claims.FirstOrDefault(x => x.Type == "PartnerCode").Value;
        var sourcecurrency = await _commonddl.GetPartnerSourceCurrencyddl(partnercode);
        var destinationcurrency = await _commonddl.GetPartnerDestinationCurrencyddl(partnercode);
        var PaymentType = await _commonddl.GetPaymentTypeddl();
        ViewBag.SourceCurrencyDdl = new SelectList(sourcecurrency, "value", "Text");
        ViewBag.destinationCurrencyDdl = new SelectList(destinationcurrency, "value", "Text");
        ViewBag.PaymentTypeDdl = new SelectList(PaymentType, "lookup", "Text");

        if (WebHelper.IsAjaxRequest(Request))
            return await Task.FromResult(PartialView("_WalletCurrencyList", walletcurrency));

        return await Task.FromResult(View(walletcurrency));
    }

    #endregion WalletCurrencyList

    [HttpGet]
    public async Task<IActionResult> GetFeeAccount([FromQuery] string sourceCurrency)
    {
        var partnerCode = User.Claims.FirstOrDefault(x => x.Type == "PartnerCode").Value;
        var currency = await _commonddl.GetCurrencyddl();
        ViewBag.Currency = new SelectList(currency, "value", "Text");
        var feeAccount = await _partnerService.GetFeeAccountAsync(partnerCode, sourceCurrency);
        if (WebHelper.IsAjaxRequest(Request))
            return await Task.FromResult(PartialView("_FeeAccountList", feeAccount));
        return await Task.FromResult(View(feeAccount));
    }

    [HttpGet]
    public async Task<IActionResult> AddFeeFundRequest(int walletcurrencyid)
    {
        var partnerwalletcurrencyDetails = await _partnerWalletCurrencyServices.GetFeeWalletCurrencyById(walletcurrencyid);
        ViewBag.PartnerCode = partnerwalletcurrencyDetails.PartnerCode;
        ViewBag.WalletcurrencyDetails = partnerwalletcurrencyDetails;
        var Fundtype = await _commonddl.GetFundTypeddl();
        ViewBag.FundTypeddl = new SelectList(Fundtype.Where(x => x.lookup == "FEE"), "value", "Text");
        return await Task.FromResult(PartialView());
    }

    [HttpPost]
    public async Task<IActionResult> AddFeeFundRequest(AddUpdateFundRequestVm addUpdateFundRequest)
    {
        var partnerwalletcurrencyDetails = await _partnerWalletCurrencyServices.GetFeeWalletCurrencyById(addUpdateFundRequest.WalletId);
        ViewBag.PartnerCode = addUpdateFundRequest.PartnerCode;
        ViewBag.WalletcurrencyDetails = partnerwalletcurrencyDetails;
        var Fundtype = await _commonddl.GetFundTypeddl();
        ViewBag.FundTypeddl = new SelectList(Fundtype.Where(x => x.lookup == "FEE"), "value", "Text");
        if (!ModelState.IsValid)
        {
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return PartialView();
        }
        else
        {
            var mappeddata = _mapper.Map<AddUpdateFundRequest>(addUpdateFundRequest);
            var ResponseStatus = await _partnerWalletCurrencyServices.AddFeeBalanceAsync(mappeddata, User);
            if (ResponseStatus.StatusCode == 200)
            {
                _notyfService.Success(ResponseStatus.MsgText);
                return Ok();
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                ViewBag.Error = ResponseStatus.MsgText;
                return PartialView();
            }
        }
    }

    #region Add-FundRequest

    [HttpGet]
    public async Task<IActionResult> AddFund(int walletcurrencyid, string FundType, string partnerFullName, string organization, string currency)
    {
        var partnerwalletcurrencyDetails = await _partnerWalletCurrencyServices.GetPartnerWalletCurrencyById(walletcurrencyid);
        ViewBag.PartnerCode = partnerwalletcurrencyDetails.PartnerCode;
        ViewBag.WalletcurrencyDetails = partnerwalletcurrencyDetails;
        ViewBag.Currency = currency;
        ViewBag.PartnerFullName = partnerFullName;
        ViewBag.Organization = organization;
        var Fundtype = await _commonddl.GetFundTypeddl();
        ViewBag.FundTypeddl = new SelectList(Fundtype.Where(x => x.lookup != "FEE" && x.Text == FundType), "value", "Text");
        return await Task.FromResult(PartialView());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddFund(AddUpdateFundRequestVm addUpdateFundRequest)
    {
        var partnerwalletcurrencyDetails = await _partnerWalletCurrencyServices.GetPartnerWalletCurrencyById(addUpdateFundRequest.WalletId);
        ViewBag.PartnerCode = addUpdateFundRequest.PartnerCode;
        ViewBag.WalletcurrencyDetails = partnerwalletcurrencyDetails;
        var Fundtype = await _commonddl.GetFundTypeddl();
        ViewBag.FundTypeddl = new SelectList(Fundtype.Where(x => x.value == addUpdateFundRequest.FundTypeId.ToString()), "value", "Text");
        if (!ModelState.IsValid)
        {
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return PartialView(addUpdateFundRequest);
        }

        var mappeddata = _mapper.Map<AddUpdateFundRequest>(addUpdateFundRequest);
        mappeddata.Remarks = addUpdateFundRequest.Remarks;
        var addFundStatus = await _partnerWalletCurrencyServices.AddFundRequestAsync(mappeddata, User);

        if (!addFundStatus.Success)
        {
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            ViewBag.Error = addFundStatus.Errors.First();
            return PartialView(addUpdateFundRequest);
        }
        await _notificationService.IUDNotificationAsync($"A fund request of {addUpdateFundRequest.Currency} {addUpdateFundRequest.Amount} has been submitted by {addUpdateFundRequest.PartnerFullName} ({addUpdateFundRequest.Organization})", NotificationModules.FundRequest, "/admin/prefundrequest/prefundrequestindex", "/partner/wallets/prefundindex");
        var Acount = await _notificationService.GetAdminNotificationCountAsync();
        var Pcount = await _notificationService.GetPartnerNotificationCountAsync();
        //await _hubcontext.Clients.Groups("Admin").SendAsync("updateTotalCount", Acount, "Fund Requested");
        await _hubcontext.Clients.Groups("Admin").SendAsync("updateTotalCount", Acount, $"A fund request of {addUpdateFundRequest.Currency} {addUpdateFundRequest.Amount} has been submitted by {addUpdateFundRequest.PartnerFullName} ({addUpdateFundRequest.Organization})");
        await _hubcontext.Clients.Groups(addUpdateFundRequest.PartnerCode).SendAsync("partnerupdateTotalCount", Pcount);

        _notyfService.Success(addFundStatus.Message);
        return Ok();
    }

    #endregion Add-FundRequest

    #region List,View-PrefundRequestApproved

    [HttpGet]
    public async Task<IActionResult> PreFundApprovedIndex([FromQuery] PrefundRequestFilter requestFilter)
    {
        Response.Headers["Cache-Control"] = "no-store, no-cache, must-revalidate";
        Response.Headers["Pragma"] = "no-cache";
        Response.Headers["Expires"] = "0";

        string partnercode = User.Claims.FirstOrDefault(x => x.Type == "PartnerCode").Value;
        requestFilter.PartnerCode = partnercode;
        var data = await _fundRequestServices.GetPreFundRequestApprovedAsync(requestFilter);
        if (WebHelper.IsAjaxRequest(Request))
        {
            return PartialView("_PreFundApprovedList", data);
        }

        return View(data);
    }

    [HttpGet]
    public async Task<IActionResult> PreFundApprovedDetails(int Id)
    {
        var data = await _fundRequestServices.GetPreFundRequestApprovedByIdAsync(Id);
        return PartialView(data);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateCurrencyStatus(string partnercode, string sourcecurrency, bool currStatus)
    {
        var data = await _fundRequestServices.isSourceCurrencyNPR(partnercode, sourcecurrency, currStatus);
        return PartialView(data);
    }
    [HttpGet]
    public async Task<IActionResult> PartnerStatement(string walletcurrencyid, Statement model)
    {
        model.StartDate = string.IsNullOrEmpty(model.StartDateBS) ? model.StartDate : model.StartDateBS;
        model.EndDate = string.IsNullOrEmpty(model.EndDateBS) ? model.EndDate : model.EndDateBS;
        string partnercode = User.Claims.FirstOrDefault(x => x.Type == "PartnerCode").Value;
        model.Partnercode = partnercode;
        ViewBag.Walletcurrencyid = model.walletCurrencyById;
        var data = await _fundRequestServices.GetSatementDetails(walletcurrencyid, model);
        if (model.Export == 1)
        {
            var datas = data.Items;
            List<DataTable> dataTables = await IEnumerableExtensions.ToDataTablesAsync<PartnerWalletStatement>(datas, 500000);
            var (excelFileByteArr, fileFormat, fileName) = await ExportHelper.ToExcelAsync(dataTables, "PartnerWalletSattement", true);
            return File(excelFileByteArr, fileFormat, fileName);
        }
        if (WebHelper.IsAjaxRequest(Request))
            return await Task.FromResult(PartialView("_WalletSattement", data));
        return await Task.FromResult(View(data));
    }
    [HttpGet]
    public async Task<IActionResult> PartnerFeeAccountStatement(FeeAccountStatementFilter model)
    {
        model.StartDate = string.IsNullOrEmpty(model.StartDateBS) ? model.StartDate : model.StartDateBS;
        model.EndDate = string.IsNullOrEmpty(model.EndDateBS) ? model.EndDate : model.EndDateBS;
        string partnercode = User.Claims.FirstOrDefault(x => x.Type == "PartnerCode").Value;
        model.PartnerCode = partnercode;
        ViewBag.Walletcurrencyid = model.WalletCurrency;
        var data = await _fundRequestServices.GetFeeAccountSatementDetails(model);
        if (model.Export == 1)
        {
            var datas = data.Items;
            List<DataTable> dataTables = await IEnumerableExtensions.ToDataTablesAsync<FeeAccountStatement>(datas, 500000);
            var (excelFileByteArr, fileFormat, fileName) = await ExportHelper.ToExcelAsync(dataTables, "PartnerFeeAccountStatement", true);
            return File(excelFileByteArr, fileFormat, fileName);
        }
        if (WebHelper.IsAjaxRequest(Request))
            return await Task.FromResult(PartialView("_FeeAccountStatementList", data));
        return await Task.FromResult(View(data));
    }
    #endregion


}