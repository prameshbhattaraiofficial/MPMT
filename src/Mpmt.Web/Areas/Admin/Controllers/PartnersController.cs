using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Mpmt.Core.Common;
using Mpmt.Core.Dtos.Adjustment;
using Mpmt.Core.Dtos.DropDown;
using Mpmt.Core.Dtos.FeeAccount;
using Mpmt.Core.Dtos.Partner;
using Mpmt.Core.Dtos.PartnerStatement;
using Mpmt.Core.Dtos.WalletLoad.Statement;
using Mpmt.Core.Events;
using Mpmt.Core.Extensions;
using Mpmt.Services.Authentication;
using Mpmt.Services.Partner.IService;
using Mpmt.Services.Services.AdminUser;
using Mpmt.Services.Services.Common;
using Mpmt.Services.Services.Currency;
using Mpmt.Services.Services.PreFund;
using Mpmt.Services.Services.RoleMenuPermission;
using Mpmt.Web.Areas.Admin.ViewModels.Paetner;
using Mpmt.Web.Areas.Admin.ViewModels.Paetner.WalletCurrency;
using Mpmt.Web.Common;
using Mpmt.Web.Filter;
using System.Data;
using System.Net;
using System.Text;

namespace Mpmt.Web.Areas.Admin.Controllers;

// [ServiceFilter(typeof(RoleGroupFilterAttribute))]

[RolePremission]
[AdminAuthorization]
public class PartnersController : BaseAdminController
{
    private readonly ICurrencyServices _currencyServices;
    private readonly ICommonddlServices _commonddl;
    private readonly IAdminUserServices _adminUserServices;
    private readonly IPartnerCredentialsService _partnerCredentialsService;
    private readonly IRMPService _rMPService;
    private readonly IPartnerWalletCurrencyServices _partnerWalletCurrencyServices;
    private readonly IEventPublisher _eventPublisher;
    private readonly IPartnerService _partnerService;
    private readonly IMapper _mapper;
    private readonly INotyfService _notyfService;
    private readonly IPreFundRequestServices _fundRequestServices;

    public PartnersController(IEventPublisher eventPublisher,
        IPartnerService partnerService,
        IMapper mapper,
        INotyfService notyfService,
        ICurrencyServices currencyServices,
        IPartnerWalletCurrencyServices partnerWalletCurrencyServices,
        ICommonddlServices commonddl,
        IPartnerCredentialsService partnerCredentialsService,
        IRMPService rMPService,
        IAdminUserServices adminUserServices, IPreFundRequestServices fundRequestServices)
    {
        _eventPublisher = eventPublisher;
        _partnerService = partnerService;
        _mapper = mapper;
        _notyfService = notyfService;
        _currencyServices = currencyServices;
        _partnerWalletCurrencyServices = partnerWalletCurrencyServices;
        _commonddl = commonddl;
        _partnerCredentialsService = partnerCredentialsService;
        _rMPService = rMPService;
        _adminUserServices = adminUserServices;
        _fundRequestServices = fundRequestServices;
    }

    #region Validation
    [AcceptVerbs("GET", "POST")]
    public async Task<IActionResult> VerifyUserName(string userName)
    {
        if (!await _partnerService.VerifyUserName(userName))
        {
            return Json($"User Name {userName} is already in use.");
        }

        return Json(true);
    }

    [AcceptVerbs("GET", "POST")]
    public async Task<IActionResult> VerifyEmail(string Email)
    {
        if (!await _partnerService.VerifyEmail(Email))
        {
            return Json($"Email is already in use.");
        }

        return Json(true);
    }

    [AcceptVerbs("GET", "POST")]
    public async Task<IActionResult> VerifyShortname(string Shortname)
    {
        if (!await _partnerService.VerifyShortname(Shortname))
        {
            return Json($"Shortname {Shortname} is already in use.");
        }

        return Json(true);
    }
    #endregion

    #region partnerList
    [HttpGet]
    public async Task<IActionResult> Index([FromQuery] PartnerFilter partnerFilter)
    {
        var actions = await _rMPService.GetActionPermissionListAsync("Partners");

        ViewBag.actions = actions;

        if (string.IsNullOrEmpty(partnerFilter.SortBy))
        {
            partnerFilter.SortBy = "Date";
            partnerFilter.SortOrder = "Asc";

        };
        var Result = await _partnerService.GetPartnerListAsync(partnerFilter);
        if (WebHelper.IsAjaxRequest(Request))
            return await Task.FromResult(PartialView("_PartnerList", Result));

        return await Task.FromResult(View(Result));
    }
    #endregion

    #region PartnerWalletCurrencyList

    [HttpGet]
    public async Task<IActionResult> PartnerWalletCurrency(int partnerId)
    {
        var actions = await _rMPService.GetActionPermissionListAsync("Partners");

        ViewBag.actions = actions;
        var PartnerById = await _partnerService.GetPartnerByIdAsync(partnerId);
        var partnerwalletcurrencyList = await _partnerWalletCurrencyServices.GetPartnerWalletCurrencyBalance(PartnerById.PartnerCode);
        var walletcurrency = new WalletCurrency { apppartner = PartnerById, PartnerWalletCurrency = partnerwalletcurrencyList };
        ViewBag.PartnerCode = PartnerById.PartnerCode;

        if (WebHelper.IsAjaxRequest(Request))
            return await Task.FromResult(PartialView("_PartnerWalletCurrencyList", walletcurrency));

        return await Task.FromResult(View(walletcurrency));
    }

    #endregion

    #region Add-PartnerWalletCurrency
    [HttpGet]
    public async Task<IActionResult> AddPartnerWalletCurrency(string PartnerCode)
    {
        ViewBag.PartnerCode = PartnerCode;
        var currencyddl = await _commonddl.GetCurrencyddl();
        IEnumerable<Commonddl> destinationCurrencyDdl = null;
        destinationCurrencyDdl = currencyddl.Where(c => c.value.Equals("NPR", StringComparison.OrdinalIgnoreCase));
        if (!destinationCurrencyDdl.Any())
            destinationCurrencyDdl = new List<Commonddl> { new() { Text = "Nepalese Rupees", value = "NPR" } };

        var filteredCurrencyddl = currencyddl.Where(item => item.value != "NPR").ToList();
        ViewBag.SourceCurrencyddl = new SelectList(filteredCurrencyddl, "value", "Text");
        ViewBag.DestinationCurrencyddl = new SelectList(currencyddl, "value", "Text");
        ViewBag.DestinationCurrency = new SelectList(destinationCurrencyDdl, "value", "Text");

        return await Task.FromResult(PartialView());
    }

    [HttpPost]
    [LogUserActivity("added partner wallet currency")]
    public async Task<IActionResult> AddPartnerWalletCurrency(PartnerWalletCurrencyVm walletCurrencyVm)
    {
        ViewBag.PartnerCode = walletCurrencyVm.PartnerCode;
        IEnumerable<Commonddl> destinationCurrencyDdl = null;
        var currencyddl = await _commonddl.GetCurrencyddl();
        var filteredCurrencyddl = currencyddl.Where(item => item.value != "NPR").ToList();
        destinationCurrencyDdl = currencyddl.Where(c => c.value.Equals("NPR", StringComparison.OrdinalIgnoreCase));
        if (!destinationCurrencyDdl.Any())
            destinationCurrencyDdl = new List<Commonddl> { new() { Text = "Nepalese Rupees", value = "NPR" } };
        ViewBag.SourceCurrencyddl = new SelectList(filteredCurrencyddl, "value", "Text");
        ViewBag.DestinationCurrency = new SelectList(destinationCurrencyDdl, "value", "Text");
        //ViewBag.SourceCurrencyddl = new SelectList(currencyddl, "value", "Text");
        //ViewBag.DestinationCurrencyddl = new SelectList(currencyddl.Where(x => x.value != walletCurrencyVm.SourceCurrency), "value", "Text");
        if (!ModelState.IsValid)
        {
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return PartialView(walletCurrencyVm);
        }
        else
        {
            var mappeddata = _mapper.Map<IUDUpdateWalletCurrency>(walletCurrencyVm);
            var ResponseStatus = await _partnerWalletCurrencyServices.AddWalletCurrencyAsync(mappeddata, User);
            if (ResponseStatus.StatusCode == 200)
            {
                _notyfService.Success(ResponseStatus.MsgText);
                return Ok();
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                ViewBag.Error = ResponseStatus.MsgText;
                return PartialView(walletCurrencyVm);
            }
        }
    }
    #endregion

    [HttpGet]
    public async Task<IActionResult> AddPartnerFeeWallet(string PartnerCode)
    {
        ViewBag.PartnerCode = PartnerCode;
        var currencyddl = await _commonddl.GetCurrencyddl();
        ViewBag.SourceCurrencyddl = new SelectList(currencyddl, "value", "Text");
        return await Task.FromResult(PartialView());
    }

    [HttpPost]
    [LogUserActivity("Added Partner Fee Wallet")]
    public async Task<IActionResult> AddPartnerFeeWallet(FeeWalletVm feeWallet)
    {
        ViewBag.PartnerCode = feeWallet.PartnerCode;
        var currencyddl = await _commonddl.GetCurrencyddl();
        ViewBag.SourceCurrencyddl = new SelectList(currencyddl, "value", "Text");
        if (!ModelState.IsValid)
        {
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return PartialView(feeWallet);
        }
        else
        {
            var mappeddata = _mapper.Map<IUDUpdateWalletCurrency>(feeWallet);
            var ResponseStatus = await _partnerWalletCurrencyServices.AddFeeWalletAsync(mappeddata, User);
            if (ResponseStatus.StatusCode == 200)
            {
                _notyfService.Success(ResponseStatus.MsgText);
                return Ok();
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                ViewBag.Error = ResponseStatus.MsgText;
                return PartialView(feeWallet);
            }
        }
    }

    #region Update-PartnerWalletCurrency    
    [HttpGet]
    public async Task<IActionResult> UpdatePartnerWalletCurrency(int Id)
    {
        var partnerwalletcurrencyDetails = await _partnerWalletCurrencyServices.GetPartnerWalletCurrencyById(Id);
        var mappeddata = _mapper.Map<PartnerWalletCurrencyVm>(partnerwalletcurrencyDetails);
        ViewBag.PartnerCode = partnerwalletcurrencyDetails.PartnerCode;
        var currencyddl = await _commonddl.GetCurrencyddl();
        ViewBag.SourceCurrencyddl = new SelectList(currencyddl, "value", "Text");
        ViewBag.DestinationCurrencyddl = new SelectList(currencyddl, "value", "Text");

        return await Task.FromResult(PartialView(mappeddata));
    }

    [HttpPost]
    [LogUserActivity("updated partner wallet currency")]
    public async Task<IActionResult> UpdatePartnerWalletCurrency(PartnerWalletCurrencyVm walletCurrencyVm)
    {
        ViewBag.PartnerCode = walletCurrencyVm.PartnerCode;
        var currencyddl = await _commonddl.GetCurrencyddl();
        ViewBag.SourceCurrencyddl = new SelectList(currencyddl, "value", "Text");
        ViewBag.DestinationCurrencyddl = new SelectList(currencyddl, "value", "Text");
        if (!ModelState.IsValid)
        {
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return PartialView();
        }
        else
        {
            var mappeddata = _mapper.Map<IUDUpdateWalletCurrency>(walletCurrencyVm);
            var ResponseStatus = await _partnerWalletCurrencyServices.UpdateWalletCurrencyAsync(mappeddata, User);
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

    #endregion

    [HttpGet]
    public async Task<IActionResult> UpdatePartnerFeeWallet(int Id, string sourceCurrency)
    {
        var partnerwalletcurrencyDetails = await _partnerWalletCurrencyServices.GetFeeWalletCurrencyById(Id);
        var mappeddata = _mapper.Map<FeeWalletVm>(partnerwalletcurrencyDetails);
        ViewBag.PartnerCode = partnerwalletcurrencyDetails.PartnerCode;
        var currencyddl = await _commonddl.GetCurrencyddl();
        ViewBag.SourceCurrencyddl = new SelectList(currencyddl.Where(x => x.value == sourceCurrency), "value", "Text");
        return await Task.FromResult(PartialView(mappeddata));
    }

    [HttpPost]
    [LogUserActivity("updated partner fee wallet")]
    public async Task<IActionResult> UpdatePartnerFeeWallet(FeeWalletVm walletCurrencyVm)
    {
        ViewBag.PartnerCode = walletCurrencyVm.PartnerCode;
        var currencyddl = await _commonddl.GetCurrencyddl();
        ViewBag.SourceCurrencyddl = new SelectList(currencyddl.Where(x => x.value == walletCurrencyVm.SourceCurrency), "value", "Text");
        if (!ModelState.IsValid)
        {
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return PartialView();
        }
        else
        {
            var mappeddata = _mapper.Map<IUDUpdateWalletCurrency>(walletCurrencyVm);
            var ResponseStatus = await _partnerWalletCurrencyServices.UpdateFeeWalletAsync(mappeddata, User);
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

    #region Remove-PartnerWalletCurrency
    [HttpGet]
    public async Task<IActionResult> RemovePartnerWalletCurrency(int Id)
    {
        var partnerwalletcurrencyDetails = await _partnerWalletCurrencyServices.GetPartnerWalletCurrencyById(Id);
        var mappeddata = _mapper.Map<PartnerWalletCurrencyVm>(partnerwalletcurrencyDetails);
        ViewBag.PartnerCode = partnerwalletcurrencyDetails.PartnerCode;

        return await Task.FromResult(PartialView(mappeddata));
    }

    [HttpPost]
    [LogUserActivity("removed partner wallet currency")]
    public async Task<IActionResult> RemovePartnerWalletCurrency(PartnerWalletCurrencyVm walletCurrencyVm)
    {
        ViewBag.PartnerCode = walletCurrencyVm.PartnerCode;

        if (walletCurrencyVm.Id <= 0)
        {
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return PartialView();
        }
        else
        {
            var mappeddata = _mapper.Map<IUDUpdateWalletCurrency>(walletCurrencyVm);
            var ResponseStatus = await _partnerWalletCurrencyServices.RemoveWalletCurrencyAsync(mappeddata, User);
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

    #endregion

    #region Add-FundRequest
    [HttpGet]
    public async Task<IActionResult> AddFundRequest(int walletcurrencyid, string fundType)
    {
        var partnerwalletcurrencyDetails = await _partnerWalletCurrencyServices.GetPartnerWalletCurrencyById(walletcurrencyid);
        ViewBag.PartnerCode = partnerwalletcurrencyDetails.PartnerCode;
        ViewBag.WalletcurrencyDetails = partnerwalletcurrencyDetails;
        var Fundtype = await _commonddl.GetFundTypeddl();
        ViewBag.FundTypeddl = new SelectList(Fundtype.Where(x => x.lookup != "FEE" && x.Text == fundType), "value", "Text");
        return await Task.FromResult(PartialView());
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

    [HttpGet]
    public async Task<IActionResult> ViewPartner(int partnerId = 0)
    {
        if (partnerId > 0)
        {
            var partner = await _partnerService.GetPartnerByIdAsync(partnerId);
            if (partner != null)
            {
                var detail = _mapper.Map<PartnerVM>(partner);

                var ChargeCategoryType = await _commonddl.GetChargeCategoryTypeddl();
                var FundType = await _commonddl.GetFundTypeddl();
                var AddressProof = await _commonddl.GetAddressProofTypeddl();
                var Document = await _commonddl.GetDocumentTypeddl();

                var documentTypeItem = Document.FirstOrDefault(x => x.value == detail.DocumentTypeId.ToString())?.Text;
                var addressProof = AddressProof.FirstOrDefault(x => x.value == detail.AddressProofTypeId.ToString())?.Text;
                var fundType = FundType.FirstOrDefault(x => x.value == detail.FundTypeId.ToString())?.Text;
                var chargeCategory = ChargeCategoryType.FirstOrDefault(x => x.value == detail.ChargeCategoryId.ToString())?.Text;

                ViewBag.DocumentType = (documentTypeItem != null) ? documentTypeItem : "";
                ViewBag.Addressproof = (addressProof != null) ? addressProof : "";
                ViewBag.FundType = (fundType != null) ? fundType : "";
                ViewBag.ChargeCategoryType = (chargeCategory != null) ? chargeCategory : "";

                return await Task.FromResult(PartialView("ViewPartner", detail));
            }
        }
        //display error view
        _notyfService.Error("Partner Not Found!");
        return await Task.FromResult(PartialView("PartnerError"));
    }

    public async Task<IActionResult> ViewPartnerDetail(int partnerId = 0)
    {
        if (partnerId > 0)
        {
            var partnerDetail = await _partnerService.GetPartnerByIdAsync(partnerId);
            return await Task.FromResult(PartialView("ViewPartnerDetail", partnerDetail));
        }
        _notyfService.Error("Partner Not Found!");
        return await Task.FromResult(PartialView("PartnerError"));
    }

    [HttpPost]
    [LogUserActivity("added fund request")]
    public async Task<IActionResult> AddFundRequest(AddUpdateFundRequestVm addUpdateFundRequest)
    {
        var partnerwalletcurrencyDetails = await _partnerWalletCurrencyServices.GetPartnerWalletCurrencyById(addUpdateFundRequest.WalletId);
        ViewBag.PartnerCode = addUpdateFundRequest.PartnerCode;
        ViewBag.WalletcurrencyDetails = partnerwalletcurrencyDetails;
        var Fundtype = await _commonddl.GetFundTypeddl();
        ViewBag.FundTypeddl = new SelectList(Fundtype.Where(x => x.lookup != "FEE" && x.value == addUpdateFundRequest.FundTypeId.ToString()), "value", "Text");
        if (!ModelState.IsValid)
        {
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            if (ModelState["VoucherImg"].Errors.Any())
                ViewBag.Error = ModelState["VoucherImg"].Errors.FirstOrDefault().ErrorMessage;
            return PartialView();
        }

        var mappeddata = _mapper.Map<AddUpdateFundRequest>(addUpdateFundRequest);
        var addFundStatus = await _partnerWalletCurrencyServices.AddFundRequestAsync(mappeddata, User);
        if (!addFundStatus.Success)
        {
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            ViewBag.Error = addFundStatus.Errors.First();
            return PartialView();
        }

        _notyfService.Success(addFundStatus.Message);
        return Ok();
    }

    #endregion

    [HttpPost]
    [LogUserActivity("added fee fund request")]
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

    #region UPdate-Partner
    [HttpGet]
    public async Task<IActionResult> UpdatePartner(int partnerId)
    {
        var partner = await _partnerService.GetPartnerByIdAsync(partnerId);
        var partnerDirectors = await _partnerService.GetPartnerdirectorsListAsync(partner.PartnerCode);
        var UpdatePartnerdata = _mapper.Map<UpdatePartnerrequest>(partner);
        var partnerDirectorsList = _mapper.Map<List<Core.Dtos.Partner.Director>>(partnerDirectors);
        UpdatePartnerdata.Directors = partnerDirectorsList;
        var calcod = partner.MobileNumber.Split("-");
        if (calcod.Length > 1)
        {
            UpdatePartnerdata.MobileNumber = calcod[1];
            UpdatePartnerdata.CallingCode = calcod[0];
        }
        else
        {
            UpdatePartnerdata.MobileNumber = calcod[0];
        }

        UpdatePartnerdata.GMTTimeZone += "|" + UpdatePartnerdata.GMTTimeZoneId;
        var data = await _commonddl.GetCountryddl();
        ViewBag.Countryddl = new SelectList(data, "value", "Text");
        var Document = await _commonddl.GetDocumentTypeddl();
        ViewBag.DocumentType = new SelectList(Document, "value", "Text");
        var Addressprof = await _commonddl.GetAddressProofTypeddl();
        ViewBag.Addressproof = new SelectList(Addressprof, "value", "Text");
        var currencyddl = await _commonddl.GetCurrencyddl();
        ViewBag.Currency = new SelectList(currencyddl, "value", "Text");
        var UtctimeZone = await _commonddl.GetUtcTimeZoneddl();

        ViewBag.UtctimeZoneLookup = new SelectList(UtctimeZone.Select(x => new
        {
            Value = x.value + "|" + x.lookup,
            x.Text
        }), "Value", "Text");
        ViewBag.UtctimeZone = new SelectList(UtctimeZone, "value", "Text");

        var ChargeCategoryType = await _commonddl.GetChargeCategoryTypeddl();
        ViewBag.ChargeCategoryType = new SelectList(ChargeCategoryType, "value", "Text");
        var FundType = await _commonddl.GetFundTypeddl();
        ViewBag.FundType = new SelectList(FundType, "value", "Text");
        var CallingCode = await _commonddl.GetCallingCodeddl();
        ViewBag.CallingCode = new SelectList(CallingCode, "Text", "lookup");

        return PartialView("UpdatePartner", UpdatePartnerdata);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [LogUserActivity("updated a partner")]
    public async Task<IActionResult> UpdatePartner(UpdatePartnerrequest requestdata)
    {
        var data = await _commonddl.GetCountryddl();
        ViewBag.Countryddl = new SelectList(data, "value", "Text");
        var Document = await _commonddl.GetDocumentTypeddl();
        ViewBag.DocumentType = new SelectList(Document, "value", "Text");
        var Addressprof = await _commonddl.GetAddressProofTypeddl();
        ViewBag.Addressproof = new SelectList(Addressprof, "value", "Text");
        var currencyddl = await _commonddl.GetCurrencyddl();
        ViewBag.Currency = new SelectList(currencyddl, "value", "Text");
        var UtctimeZone = await _commonddl.GetUtcTimeZoneddl();

        ViewBag.UtctimeZoneLookup = new SelectList(UtctimeZone.Select(x => new
        {
            Value = x.value + "|" + x.lookup,
            x.Text
        }), "Value", "Text");
        ViewBag.UtctimeZone = new SelectList(UtctimeZone, "value", "Text");

        var ChargeCategoryType = await _commonddl.GetChargeCategoryTypeddl();
        ViewBag.ChargeCategoryType = new SelectList(ChargeCategoryType, "value", "Text");
        var CallingCode = await _commonddl.GetCallingCodeddl();
        ViewBag.CallingCode = new SelectList(CallingCode, "Text", "lookup");
        var FundType = await _commonddl.GetFundTypeddl();
        ViewBag.FundType = new SelectList(FundType, "value", "Text");
        ViewBag.CallingCodeval = requestdata.CallingCode;

        if (!ModelState.IsValid)
        {
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return PartialView("UpdatePartner", requestdata);
        }

        if (requestdata.LicenseDocument is null && requestdata.LicensedocImgPath is null)
        {
            ViewBag.Error = "License image is required";
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return PartialView("UpdatePartner", requestdata);
        }
        else if ((requestdata.LicenseDocument?.Count ?? 0) + (requestdata.LicensedocImgPath?.Count ?? 0) <= 0)
        {
            ViewBag.Error = "License image is required";
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return PartialView("UpdatePartner", requestdata);
        }

        string[] values = requestdata.GMTTimeZone.Split('|');
        requestdata.GMTTimeZone = values[0];
        requestdata.GMTTimeZoneId = values[1];

        var updateResult = await _partnerService.UpdatePartnerAsync(requestdata);
        if (!updateResult.Success)
        {
            Response.StatusCode = (int)(HttpStatusCode.BadRequest);
            ViewBag.Error = updateResult.Errors.First();
            return PartialView(requestdata);
        }

        // expire the session of the partner after making the partner inactive
        if (!requestdata.IsActive)
        {
            var partner = await _partnerService.GetPartnerByIdAsync(requestdata.Id);
            if (partner is not null)
                await _eventPublisher.PublishAsync(new EntityUpdatedEvent<SessionAuthExpiration>(
                            new SessionAuthExpiration { UserUniqueId = partner.UserGuid.ToString(), ExpireBefore = DateTime.UtcNow }));
        }

        _notyfService.Success(updateResult.Message);
        return Ok();
    }

    [HttpGet]
    public IActionResult DeleteImage(string ImageUrl)
    {
        var img = ImageUrl;
        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> ToggelActive(bool Actionval, int UserId)
    {
        var data = await _partnerService.UpdateRemitPartnerStatusAsync(Actionval, UserId);
        if (data.StatusCode == 200)
        {
            _notyfService.Success(data.MsgText);

            // expire the session of the partner after making the partner inactive
            if (!Actionval)
            {
                var partner = await _partnerService.GetPartnerByIdAsync(UserId);
                if (partner is not null)
                    await _eventPublisher.PublishAsync(new EntityUpdatedEvent<SessionAuthExpiration>(
                        new SessionAuthExpiration { UserUniqueId = partner.UserGuid.ToString(), ExpireBefore = DateTime.UtcNow }));
            }
        }
        else
        {
            _notyfService.Error("Action Failed!!");
        }
        return Ok();
    }

    #endregion

    #region Add-Partners

    [HttpGet]
    public async Task<IActionResult> AddPartner()
    {
        var data = await _commonddl.GetCountryddl();
        ViewBag.Countryddl = new SelectList(data, "value", "Text");
        var Document = await _commonddl.GetDocumentTypeddl();
        ViewBag.DocumentType = new SelectList(Document, "value", "Text");
        var Addressprof = await _commonddl.GetAddressProofTypeddl();
        ViewBag.Addressproof = new SelectList(Addressprof, "value", "Text");
        var currencyddl = await _commonddl.GetCurrencyddl();
        ViewBag.Currency = new SelectList(currencyddl, "value", "Text");
        var UtctimeZone = await _commonddl.GetUtcTimeZoneddl();

        ViewBag.UtctimeZoneLookup = new SelectList(UtctimeZone.Select(x => new
        {
            Value = x.value + "|" + x.lookup,
            x.Text
        }), "Value", "Text");

        ViewBag.UtctimeZone = new SelectList(UtctimeZone, "value", "Text");

        var ChargeCategoryType = await _commonddl.GetChargeCategoryTypeddl();
        ViewBag.ChargeCategoryType = new SelectList(ChargeCategoryType, "value", "Text");
        var FundType = await _commonddl.GetFundTypeddl();
        ViewBag.FundType = new SelectList(FundType, "value", "Text");
        var CallingCode = await _commonddl.GetCallingCodeddl();
        ViewBag.CallingCode = new SelectList(CallingCode, "Text", "lookup");

        return await Task.FromResult(PartialView());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [LogUserActivity("added a partner")]
    public async Task<IActionResult> AddPartner([FromForm] AddPatnerRequest patnerDto)
    {
        var data = await _commonddl.GetCountryddl();
        ViewBag.Countryddl = new SelectList(data, "value", "Text");
        var currencyddl = await _commonddl.GetCurrencyddl();
        ViewBag.Currency = new SelectList(currencyddl, "value", "Text");
        var UtctimeZone = await _commonddl.GetUtcTimeZoneddl();

        ViewBag.UtctimeZoneLookup = new SelectList(UtctimeZone.Select(x => new
        {
            Value = x.value + "|" + x.lookup,
            x.Text
        }), "Value", "Text");
        ViewBag.UtctimeZone = new SelectList(UtctimeZone, "value", "Text");

        var ChargeCategoryType = await _commonddl.GetChargeCategoryTypeddl();
        ViewBag.ChargeCategoryType = new SelectList(ChargeCategoryType, "value", "Text");
        var FundType = await _commonddl.GetFundTypeddl();
        ViewBag.FundType = new SelectList(FundType, "value", "Text");
        var Document = await _commonddl.GetDocumentTypeddl();
        ViewBag.DocumentType = new SelectList(Document, "value", "Text");
        var Addressprof = await _commonddl.GetAddressProofTypeddl();
        ViewBag.Addressproof = new SelectList(Addressprof, "value", "Text");
        var CallingCode = await _commonddl.GetCallingCodeddl();
        ViewBag.CallingCode = new SelectList(CallingCode, "Text", "lookup");

        if (!ModelState.IsValid)
        {
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return PartialView(patnerDto);
        }

        string[] values = patnerDto.GMTTimeZone.Split('|');
        patnerDto.GMTTimeZone = values[0];
        patnerDto.GMTTimeZoneId = values[1];

        var addPartnerResult = await _partnerService.AddPartnerAsync(patnerDto);
        if (!addPartnerResult.Success)
        {
            Response.StatusCode = (int)(HttpStatusCode.BadRequest);
            ViewBag.Error = addPartnerResult.Errors.First();
            return PartialView(patnerDto);
        }

        _notyfService.Success(addPartnerResult.Message);
        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> DeletePartner(int partnerId = 0)
    {
        var data = new DeletePartnerVM()
        {
            Id = partnerId,
        };
        return await Task.FromResult(PartialView("DeletePartner", data));
    }

    [HttpPost]
    [LogUserActivity("deleted a partner")]
    public async Task<IActionResult> DeletePartner(DeletePartnerVM partnerId)
    {
        if (partnerId.Id <= 0)
        {
            _notyfService.Error("Unable to delete");
            return Ok();
        }
        var data = await _partnerService.GetPartnerByIdAsync(partnerId.Id);
        if (data != null)
        {
            data.Remarks = partnerId.Remarks;
            var response = await _partnerService.DeletePartnerAsync(data);
            if (response.StatusCode == 200)
            {
                _notyfService.Success(response.MsgText);
                // expire the session of the partner after successful deletion of the partner
                await _eventPublisher.PublishAsync(new EntityUpdatedEvent<SessionAuthExpiration>(
                    new SessionAuthExpiration { UserUniqueId = data.UserGuid.ToString(), ExpireBefore = DateTime.UtcNow }));
                return Ok();
            }
            _notyfService.Error(response.MsgText);
            return Ok();
        }
        _notyfService.Error("Unable to delete");
        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> AddCredentialsPartner(int partnerId = 0)
    {
        if (partnerId > 0)
        {
            var partner = await _partnerService.GetPartnerByIdAsync(partnerId);
            if (partner != null)
            {
                var apikey = new AddApiKeys()
                {
                    PartnerCode = partner.PartnerCode,
                };
                return await Task.FromResult(PartialView("AddCredentials", apikey));
            }
        }
        //display error view
        _notyfService.Error("Partner Not Found!");
        return await Task.FromResult(PartialView("PartnerError"));
    }

    [HttpPost]
    [LogUserActivity("added credentials to a partner")]
    public async Task<IActionResult> AddCredentialsPartner(AddApiKeys apikey)
    {
        if (ModelState.IsValid)
        {
            var data = new PartnerCredentialInsertRequest
            {
                PartnerCode = apikey.PartnerCode,
                ApiUserName = apikey.ApiUserName,
                IPAddress = apikey.IPAddress,
                IsActive = apikey.IsActive.ToString(),
            };

            var addResult = await _partnerCredentialsService.AddCredentialsAsync(data);
            if (addResult.Success)
            {
                _notyfService.Success(addResult.Message);
                return Ok();
            }
            else
            {
                Response.StatusCode = (int)(HttpStatusCode.BadRequest);
                ViewBag.Error = addResult.Errors.FirstOrDefault();
                return PartialView("AddCredentials", apikey);
            }
        }
        Response.StatusCode = (int)HttpStatusCode.BadRequest;
        return PartialView("AddCredentials", apikey);
    }

    [HttpGet]
    public async Task<IActionResult> UpdateCredentialsPartner(int partnerId = 0)
    {
        if (partnerId > 0)
        {
            var partner = await _partnerService.GetPartnerByIdAsync(partnerId);

            if (!string.IsNullOrEmpty(partner.PartnerCode))
            {
                var partnercode = await _partnerCredentialsService.GetCredentialsByPartnerCodeAsync(partner.PartnerCode);
                string[] ipAddressArray = partnercode.IPAddress.Split(',');
                if (partner != null)
                {
                    var apikey = new UpdateApiKeys()
                    {
                        PartnerCode = partnercode.PartnerCode,
                        IPAddress = ipAddressArray,
                        ApiUserName = partnercode.ApiUserName,
                        IsActive = bool.Parse(partnercode.IsActive),
                        //  CredentialId = partner.crede
                    };
                    return await Task.FromResult(PartialView("UpdateCredentials", apikey));
                }
            }
            //display error view
            _notyfService.Error("Partner Not Found!");
            return await Task.FromResult(PartialView("PartnerError"));

        }
        return await Task.FromResult(PartialView("UpdateCredentials"));
    }

    [HttpPost]
    [LogUserActivity("updated partner credentials")]
    public async Task<IActionResult> UpdateCredentialsPartner(UpdateApiKeys apikey)
    {
        if (ModelState.IsValid)
        {
            var credential = await _partnerCredentialsService.GetCredentialsByPartnerCodeAsync(apikey.PartnerCode);
            var addapikeys = _mapper.Map<PartnerCredentialUpdateRequest>(apikey);
            addapikeys.ApiUserName = credential.ApiUserName;
            addapikeys.CredentialId = credential.CredentialId;
            var updateResult = await _partnerCredentialsService.UpdateCredentialsAsync(addapikeys);
            if (updateResult.Success)
            {
                _notyfService.Success(updateResult.Message);
                return Ok();
            }
            else
            {
                Response.StatusCode = (int)(HttpStatusCode.BadRequest);
                ViewBag.Error = updateResult.Errors.FirstOrDefault();
                return PartialView("UpdateCredentials", apikey);
            }
        }
        Response.StatusCode = (int)HttpStatusCode.BadRequest;
        return PartialView("UpdateCredentials", apikey);
    }

    #endregion

    #region Credentails
    [HttpGet]
    public async Task<IActionResult> ViewCredentialsPartner(int partnerId = 0)
    {
        if (partnerId > 0)
        {
            var partner = await _partnerService.GetPartnerByIdAsync(partnerId);
            if (!String.IsNullOrEmpty(partner.PartnerCode))
            {
                var credential = await _partnerCredentialsService.GetCredentialsByPartnerCodeAsync(partner.PartnerCode);
                if (credential != null)
                {
                    var apikey = new UpdateApikeyVM()
                    {
                        CredentialId = credential.CredentialId,
                        Apikey = credential.ApiKey,
                        PartnerCode = partner.PartnerCode,
                    };

                    return await Task.FromResult(PartialView("ViewPartnerApiKeys", apikey));
                }
            }
        }
        //display error view
        return await Task.FromResult(PartialView("PartnerError"));
    }

    [HttpPost]
    [LogUserActivity("changed partner API keys")]
    public async Task<IActionResult> ViewCredentialsPartner(UpdateApikeyVM ViewModel)
    {
        if (ViewModel != null)
        {
            if (!String.IsNullOrEmpty(ViewModel.PartnerCode) && !String.IsNullOrEmpty(ViewModel.CredentialId))
            {
                var (status, Apikey) = await _partnerCredentialsService.RegenerateApiKeyAsync(ViewModel.PartnerCode, ViewModel.CredentialId);
                if (status.StatusCode == 200)
                {
                    var apikey = new UpdateApikeyVM()
                    {
                        CredentialId = ViewModel.CredentialId,
                        Apikey = Apikey,
                        PartnerCode = ViewModel.PartnerCode
                    };
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    _notyfService.Success(status.MsgType);
                    return await Task.FromResult(PartialView("ViewPartnerApiKeys", apikey));
                }
                _notyfService.Success(status.MsgType);
                return Ok();
            }
        }
        _notyfService.Success("Partner Not Found!");
        return Ok();
    }

    [HttpPost]
    [LogUserActivity("exported partner credentials")]
    public async Task<IActionResult> ExportCredential([FromBody] ExportCredentialsModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (string.IsNullOrWhiteSpace(model.PartnerCode))
            return BadRequest(ModelState);

        var credential = await _partnerCredentialsService.GetCredentialsByPartnerCodeAsync(model.PartnerCode);
        if (credential is null)
            return BadRequest();

        switch (model.CredentialType.ToUpper())
        {
            case "USERPRIVATEKEY":
                if (string.IsNullOrWhiteSpace(credential.UserPrivateKey))
                    return BadRequest();

                var userPrivateKeyBytes = Encoding.UTF8.GetBytes(credential.UserPrivateKey);

                Response.Headers.Add("Content-Disposition", "attachment; filename=" + "UserPrivateKey.pem");
                Response.Headers.Add("Content-Type", "application/x-pem-file");

                return File(userPrivateKeyBytes, "application/x-pem-file");

            case "USERPUBLICKEY":
                if (string.IsNullOrWhiteSpace(credential.UserPublicKey))
                    return BadRequest();

                var userPublicKeyBytes = Encoding.UTF8.GetBytes(credential.UserPublicKey);

                Response.Headers.Add("Content-Disposition", "attachment; filename=" + "UserPublicKey.pem");
                Response.Headers.Add("Content-Type", "application/x-pem-file");

                return File(userPublicKeyBytes, "application/x-pem-file");

            case "SYSTEMPRIVATEKEY":
                if (string.IsNullOrWhiteSpace(credential.SystemPrivateKey))
                    return BadRequest();

                var systemPrivateKeyBytes = Encoding.UTF8.GetBytes(credential.SystemPrivateKey);

                Response.Headers.Add("Content-Disposition", "attachment; filename=" + "SystemPrivateKey.pem");
                Response.Headers.Add("Content-Type", "application/x-pem-file");

                return File(systemPrivateKeyBytes, "application/x-pem-file");

            case "SYSTEMPUBLICKEY":
                if (string.IsNullOrWhiteSpace(credential.SystemPublicKey))
                    return BadRequest();

                var systemPublicKeyBytes = Encoding.UTF8.GetBytes(credential.SystemPublicKey);

                Response.Headers.Add("Content-Disposition", "attachment; filename=" + "SystemPublicKey.pem");
                Response.Headers.Add("Content-Type", "application/x-pem-file");

                return File(systemPublicKeyBytes, "application/x-pem-file");
            default:
                return BadRequest();
        };
    }

    [HttpGet]
    public async Task<IActionResult> ViewPasswordCredentialsPartner(int partnerId = 0)
    {
        if (partnerId > 0)
        {
            var partner = await _partnerService.GetPartnerByIdAsync(partnerId);
            if (!String.IsNullOrEmpty(partner.PartnerCode))
            {
                var credential = await _partnerCredentialsService.GetCredentialsByPartnerCodeAsync(partner.PartnerCode);
                if (credential != null)
                {
                    var apikey = new UpdateApiPasswordVM()
                    {
                        CredentialId = credential.CredentialId,
                        ApiPassword = credential.ApiPassword,
                        PartnerCode = partner.PartnerCode,
                        ApiUserName = credential.ApiUserName
                    };

                    return await Task.FromResult(PartialView("ViewPartnerApiPassword", apikey));
                }
            }
        }
        //display error view
        return await Task.FromResult(PartialView("PartnerError"));
    }

    [HttpPost]
    [LogUserActivity("changed API password")]
    public async Task<IActionResult> ViewPasswordCredentialsPartner(UpdateApiPasswordVM ViewModel)
    {
        if (ViewModel != null)
        {
            if (!string.IsNullOrEmpty(ViewModel.PartnerCode) && !string.IsNullOrEmpty(ViewModel.CredentialId))
            {
                var credential = await _partnerCredentialsService.GetCredentialsByPartnerCodeAsync(ViewModel.PartnerCode);

                if (ViewModel.CredentialId != credential.CredentialId)
                {
                    _notyfService.Success("Partner Not Found!");
                    return Ok();
                }

                var (status, apiPassword) = await _partnerCredentialsService.RegenerateApiPasswordAsync(ViewModel.PartnerCode, ViewModel.CredentialId);
                if (status.StatusCode == 200)
                {
                    var apikey = new UpdateApiPasswordVM()
                    {
                        CredentialId = ViewModel.CredentialId,
                        ApiPassword = apiPassword,
                        PartnerCode = ViewModel.PartnerCode,
                        ApiUserName = credential.ApiUserName,
                    };

                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    _notyfService.Success(status.MsgType);
                    return await Task.FromResult(PartialView("ViewPartnerApiPassword", apikey));
                }
                _notyfService.Success(status.MsgType);
                return Ok();
            }
        }
        _notyfService.Success("Partner Not Found!");
        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> ViewPasswordSystemRsakeyPair(int partnerId = 0)
    {
        if (partnerId > 0)
        {
            var partner = await _partnerService.GetPartnerByIdAsync(partnerId);
            if (!String.IsNullOrEmpty(partner.PartnerCode))
            {
                var credential = await _partnerCredentialsService.GetCredentialsByPartnerCodeAsync(partner.PartnerCode);
                if (credential != null)
                {
                    var apikey = new SystemRsakeyPairVM()
                    {
                        CredentialId = credential.CredentialId,
                        PublicKey = credential.SystemPublicKey,
                        PrivetKey = credential.SystemPrivateKey,
                        PartnerCode = partner.PartnerCode
                    };
                    return await Task.FromResult(PartialView("ViewPartnerSystemRsaKeyPair", apikey));
                }
            }
        }
        //display error view
        return await Task.FromResult(PartialView("PartnerError"));
    }

    [HttpPost]
    [LogUserActivity("changed system RSA key pair")]
    public async Task<IActionResult> ViewPasswordSystemRsakeyPair(SystemRsakeyPairVM ViewModel)
    {
        if (ViewModel != null)
        {
            if (!string.IsNullOrEmpty(ViewModel.PartnerCode) && !string.IsNullOrEmpty(ViewModel.CredentialId))
            {
                var (status, privatekey, publickey) = await _partnerCredentialsService.RegenerateSystemRsaKeyPairAsync(ViewModel.PartnerCode, ViewModel.CredentialId);
                if (status.StatusCode == 200)
                {
                    var apikey = new SystemRsakeyPairVM()
                    {
                        CredentialId = ViewModel.CredentialId,
                        PublicKey = publickey,
                        PrivetKey = privatekey,
                        PartnerCode = ViewModel.PartnerCode
                    };

                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    _notyfService.Success(status.MsgType);
                    return await Task.FromResult(PartialView("ViewPartnerSystemRsaKeyPair", apikey));
                }
                _notyfService.Success(status.MsgType);
                return Ok();
            }
        }
        _notyfService.Success("Partner Not Found!");
        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> ViewPasswordUserRsakeyPair(int partnerId = 0)
    {
        if (partnerId > 0)
        {
            var partner = await _partnerService.GetPartnerByIdAsync(partnerId);
            if (!String.IsNullOrEmpty(partner.PartnerCode))
            {
                var credential = await _partnerCredentialsService.GetCredentialsByPartnerCodeAsync(partner.PartnerCode);
                if (credential != null)
                {
                    var apikey = new SystemRsakeyPairVM()
                    {
                        CredentialId = credential.CredentialId,
                        PublicKey = credential.UserPublicKey,
                        PrivetKey = credential.UserPrivateKey,
                        PartnerCode = partner.PartnerCode
                    };
                    return await Task.FromResult(PartialView("ViewPartnerUserRsaKeyPair", apikey));
                }
            }
        }
        //display error view
        return await Task.FromResult(PartialView("PartnerError"));
    }

    [HttpPost]
    [LogUserActivity("changed user RSA key pair")]
    public async Task<IActionResult> ViewPasswordUserRsakeyPair(SystemRsakeyPairVM ViewModel)
    {
        if (ViewModel != null)
        {
            if (!string.IsNullOrEmpty(ViewModel.PartnerCode) && !string.IsNullOrEmpty(ViewModel.CredentialId))
            {
                var (status, privatekey, publickey) = await _partnerCredentialsService.RegenerateUserRsaKeyPairAsync(ViewModel.PartnerCode, ViewModel.CredentialId);
                if (status.StatusCode == 200)
                {
                    var apikey = new SystemRsakeyPairVM()
                    {
                        CredentialId = ViewModel.CredentialId,
                        PrivetKey = privatekey,
                        PublicKey = publickey,
                        PartnerCode = ViewModel.PartnerCode
                    };
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    _notyfService.Success(status.MsgType);
                    return await Task.FromResult(PartialView("ViewPartnerUserRsaKeyPair", apikey));
                }
                _notyfService.Success(status.MsgType);
                return Ok();
            }
        }
        _notyfService.Success("Partner Not Found!");
        return Ok();
    }

    public async Task<IActionResult> BankDetails()
    {
        return await Task.FromResult(View());
    }

    #endregion

    #region DropDownBind
    [HttpGet]
    public async Task<IActionResult> GetDestinationCurrency([FromQuery] string SourceCurrency, string PartnerCode)
    {
        ViewBag.PartnerCode = PartnerCode;
        var result = await _commonddl.GetCurrencyddl();
        if (string.IsNullOrEmpty(SourceCurrency))
        {
            return Json(new SelectList(result, "value", "Text"));
        }
        else
        {
            return Json(new SelectList(result.Where(x => x.value != SourceCurrency), "value", "Text"));
        }
    }

    #endregion

    [HttpGet]
    public async Task<IActionResult> GetFeeAccount([FromQuery] string PartnerCode, string sourceCurrency)
    {
        var actions = await _rMPService.GetActionPermissionListAsync("Partners");

        ViewBag.actions = actions;
        ViewBag.FeePartnerCode = PartnerCode;
        var currency = await _commonddl.GetCurrencyddl();
        ViewBag.Currency = new SelectList(currency, "value", "Text");
        var feeAccount = await _partnerService.GetFeeAccountAsync(PartnerCode, sourceCurrency);
        if (WebHelper.IsAjaxRequest(Request))
            return await Task.FromResult(PartialView("_FeeAccountList", feeAccount));

        return await Task.FromResult(View(feeAccount));
    }

    #region Assign-Partner-To-Role

    [HttpGet]
    public async Task<IActionResult> AssignPartnerToRole(int Id, string fullName, string userName)
    {
        AssignUserRoleDto roleDto = new AssignUserRoleDto();
        roleDto.user_id = Id;
        ViewBag.UserName = userName;
        ViewBag.FullName = fullName;

        var partnerRoleById = await _commonddl.GetPartnerRolesByIdAsync(Id);
        var partnerRoles = await _commonddl.GetPartnerRolesddl();

        ViewBag.PartnerRoles = new SelectList(partnerRoles, "value", "Text");
        var data = new List<SelectListItem>();
        var roles = new List<int>();

        foreach (var role in partnerRoleById)
        {
            roles.Add(int.Parse(role.value));
        }
        foreach (var role in partnerRoles)
        {
            if (roles.Contains(int.Parse(role.value)))
            {
                data.Add(new SelectListItem() { Value = role.value, Text = role.Text, Selected = true });
            }
            else
            {
                data.Add(new SelectListItem() { Value = role.value, Text = role.Text, Selected = false });
            }
        }
        ViewBag.Roles = data;
        ViewBag.Error = TempData["Error"] == null ? "" : TempData["Error"];
        return PartialView("AssignPartnerToRole", roleDto);
    }

    [HttpPost]
    [LogUserActivity("assigned role to partner")]
    public async Task<IActionResult> AssignPartnerToRole([FromForm] AssignUserRoleDto assignUserRoleDto, string fullname, string username)
    {
        var partnerRoleById = await _commonddl.GetPartnerRolesByIdAsync(assignUserRoleDto.user_id);
        var partnerRoles = await _commonddl.GetPartnerRolesddl();

        var data = new List<SelectListItem>();
        var roles = new List<int>();

        foreach (var role in partnerRoleById)
        {
            roles.Add(int.Parse(role.value));
        }
        foreach (var role in partnerRoles)
        {
            if (roles.Contains(int.Parse(role.value)))
            {
                data.Add(new SelectListItem() { Value = role.value, Text = role.Text, Selected = true });
            }
            else
            {
                data.Add(new SelectListItem() { Value = role.value, Text = role.Text, Selected = false });
            }
        }
        ViewBag.Roles = data;

        if (!ModelState.IsValid)
        {
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return PartialView("AssignPartnerToRole", assignUserRoleDto);
        }
        var result = await _adminUserServices.AssignPartnerRole(assignUserRoleDto.user_id, assignUserRoleDto.roleid);
        if (result.StatusCode == 200)
        {
            _notyfService.Success(result.MsgText);
            return Ok();
        }
        Response.StatusCode = (int)HttpStatusCode.BadRequest;
        ViewBag.Error = result.MsgText;
        ViewBag.UserName = username;
        ViewBag.FullName = fullname;
        return PartialView("AssignPartnerToRole", assignUserRoleDto);
    }

    [HttpGet]
    public async Task<IActionResult> WalletStatement(string walletcurrencyid, Statement model)
    {
        model.StartDate = string.IsNullOrEmpty(model.StartDateBS) ? model.StartDate : model.StartDateBS;
        model.EndDate = string.IsNullOrEmpty(model.EndDateBS) ? model.EndDate : model.EndDateBS;
        ViewBag.Walletcurrencyid = model.walletCurrencyById;
        ViewBag.PartnerCode = model.Partnercode;
        var data = await _fundRequestServices.GetSatementDetails(walletcurrencyid, model);
        if (model.Export == 1)
        {
            var datas = data.Items;
            List<DataTable> dataTables = await IEnumerableExtensions.ToDataTablesAsync<PartnerWalletStatement>(datas, 500000);
            var (excelFileByteArr, fileFormat, fileName) = await ExportHelper.ToExcelAsync(dataTables, "WalletStatement", true);
            return File(excelFileByteArr, fileFormat, fileName);
        }
        if (WebHelper.IsAjaxRequest(Request))
            return await Task.FromResult(PartialView("_WalletStatement", data));
        return await Task.FromResult(View(data));
    }

    [HttpGet]
    public async Task<IActionResult> PartnerFeeAccountStatement(FeeAccountStatementFilter model)
    {
        model.StartDate = string.IsNullOrEmpty(model.StartDateBS) ? model.StartDate : model.StartDateBS;
        model.EndDate = string.IsNullOrEmpty(model.EndDateBS) ? model.EndDate : model.EndDateBS;
        ViewBag.Walletcurrencyid = model.WalletCurrency;
        ViewBag.PartnerCode = model.PartnerCode;
        var data = await _fundRequestServices.GetFeeAccountSatementDetails(model);
        if (model.Export == 1)
        {
            var datas = data.Items;
            List<DataTable> dataTables = await IEnumerableExtensions.ToDataTablesAsync<FeeAccountStatement>(datas, 500000);
            var (excelFileByteArr, fileFormat, fileName) = await ExportHelper.ToExcelAsync(dataTables, "FeeAccountStatement", true);
            return File(excelFileByteArr, fileFormat, fileName);
        }
        if (WebHelper.IsAjaxRequest(Request))
            return await Task.FromResult(PartialView("_PartnerFeeAccountStatement", data));
        return await Task.FromResult(View(data));
    }

    #endregion

    #region Adjustment-Wallet

    [HttpGet]
    public async Task<IActionResult> AdjustmentWallet(string walletCurrencyId, string partnerCode)
    {
        var adjustment = new AdjustmentWallet()
        {
            WalletCurrency = walletCurrencyId,
            PartnerCode = partnerCode
        };
        return await Task.FromResult(PartialView(adjustment));
    }

    [HttpPost]
    public async Task<IActionResult> AdjustmentWallet(AdjustmentWallet adjustment)
    {
        if (!ModelState.IsValid)
        {
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return PartialView("AdjustmentWallet", adjustment);
        }

        var result = await _partnerService.PartnerWalletAdjustment(adjustment, User);
        if (result.StatusCode == 200)
        {
            _notyfService.Success(result.MsgText);
            return Ok();
        }

        Response.StatusCode = (int)HttpStatusCode.BadRequest;
        ViewBag.Error = result.MsgText;
        return PartialView("AdjustmentWallet", adjustment);
    }

    #endregion
}
