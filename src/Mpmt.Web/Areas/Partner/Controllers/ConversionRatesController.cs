using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Mpmt.Core.Dtos.ConversionRate;
using Mpmt.Core.ViewModel.ConversionRate;
using Mpmt.Services.Services.Common;
using Mpmt.Services.Services.ConversionRate;
using Mpmt.Services.Services.RoleMenuPermission;
using Mpmt.Web.Common;
using Mpmt.Web.Filter;
using System.Net;
using System.Security.Claims;

namespace Mpmt.Web.Areas.Partner.Controllers
{
    [PartnerAuthorization]
    [RolePremission]
    public class ConversionRatesController : BasePartnerController
    {
        private readonly IRMPService _rMPService;
        private readonly IPartnerConversionRateServices _partnerConversionRateServices;
        private readonly INotyfService _notyfService;
        private readonly ICommonddlServices _commonddlServices;
        private readonly IMapper _mapper;
        private readonly ClaimsPrincipal _loggedInUser;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ConversionRatesController(IRMPService rMPService,
            IPartnerConversionRateServices partnerConversionRateServices, INotyfService notyfService, ICommonddlServices commonddlServices, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _rMPService = rMPService;
            _partnerConversionRateServices = partnerConversionRateServices;
            _notyfService = notyfService;
            _commonddlServices = commonddlServices;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _loggedInUser = _httpContextAccessor.HttpContext.User;
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] PartnerConversionRateFilter conversionRateFilter)
        {
            var actions = await _rMPService.GetPartnerActionPermissionList("ConversionRates");
            ViewBag.actions = actions;

            var PartnerId = _loggedInUser.Claims.FirstOrDefault(x => x.Type == "PartnerCode").Value;
            var conversion = new PartnerConversionRateFilter { PartnerCode = PartnerId };
            var result = await _partnerConversionRateServices.GetConversionRateAsync(conversion);
            if (WebHelper.IsAjaxRequest(Request))
                return await Task.FromResult(PartialView("_ConversionRateIndex", result));
            return await Task.FromResult(View(result));
        }

        [HttpGet]
        public async Task<IActionResult> AddConversionRate(string SourceCurrency, string DestinationCurrency)
        {
            var PartnerId = _loggedInUser.Claims.FirstOrDefault(x => x.Type == "PartnerCode").Value;
            var conversion = new PartnerConversionRateFilter { PartnerCode = PartnerId, SourceCurrency = SourceCurrency, DestinationCurrency = DestinationCurrency };
            var (result, data) = await _partnerConversionRateServices.GetConversionRateDetailAsync(conversion);
            var currencyddl = await _commonddlServices.GetCurrencyddl();
            ViewBag.SCurrency = data.SourceCurrency;
            ViewBag.DCurrency = data.DestinationCurrency;
            ViewBag.UnitValue = data.UnitValue;
            ViewBag.BuyingRate = data.MinRate;
            ViewBag.SellingRate = data.MaxRate;
            ViewBag.CurrentRate = data.CurrentRate;
            var mappedData = _mapper.Map<List<AddPartnerConversionRateVm>>(result);
            return await Task.FromResult(PartialView(mappedData));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddConversionRate(List<AddPartnerConversionRateVm> partnerConversionRateVms, string SourceCurrency, string DestinationCurrency, int UnitValue, decimal BuyingRate, decimal SellingRate, decimal CurrentRate)
        {
            var PartnerId = _loggedInUser.Claims.FirstOrDefault(x => x.Type == "PartnerCode").Value;
            var conversion = new PartnerConversionRateFilter { PartnerCode = PartnerId, SourceCurrency = SourceCurrency, DestinationCurrency = DestinationCurrency };
            var (result, data) = await _partnerConversionRateServices.GetConversionRateDetailAsync(conversion);
            var currencyddl = await _commonddlServices.GetCurrencyddl();
            ViewBag.SCurrency = data.SourceCurrency;
            ViewBag.DCurrency = data.DestinationCurrency;
            ViewBag.UnitValue = data.UnitValue;
            ViewBag.BuyingRate = data.MinRate;
            ViewBag.SellingRate = data.MaxRate;
            ViewBag.CurrentRate = data.CurrentRate;
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView(partnerConversionRateVms);
            }
            else
            {
                var conversionRate = new PartnerConversionRate
                {
                    PartnerCode = PartnerId,
                    UnitValue = UnitValue,
                    MinRate = BuyingRate,
                    MaxRate = SellingRate,
                    CurrentRate = CurrentRate,
                    SourceCurrency = SourceCurrency,
                    DestinationCurrency = DestinationCurrency
                };
                var responseStatus = await _partnerConversionRateServices.AddConversionRateAsync(partnerConversionRateVms, conversionRate);
                if (responseStatus.StatusCode == 200)
                {
                    _notyfService.Success(responseStatus.MsgText);
                    return RedirectToAction("Index");
                }
                else
                {
                    Response.StatusCode = (int)(HttpStatusCode.BadRequest);
                    ViewBag.Error = responseStatus.MsgText;
                    return PartialView(partnerConversionRateVms);
                }
            }
        }

        [HttpGet]
        public async Task<IActionResult> ViewConversionRate(string SourceCurrency, string DestinationCurrency)
        {
            var PartnerId = _loggedInUser.Claims.FirstOrDefault(x => x.Type == "PartnerCode").Value;
            var conversionFilter = new PartnerConversionRateFilter
            {
                PartnerCode = PartnerId,
                SourceCurrency = SourceCurrency,
                DestinationCurrency = DestinationCurrency
            };
            var (result, data) = await _partnerConversionRateServices.ViewConversionRateDetailAsync(conversionFilter);
            var mappedData = _mapper.Map<List<AddPartnerConversionRateVm>>(result);
            ViewBag.ConversionRate = data;
            var currencyddl = await _commonddlServices.GetCurrencyddl();
            var PaymentTypeddl = await _commonddlServices.GetPaymentTypeddl();
            ViewBag.SCurrency = new SelectList(currencyddl, "value", "Text", data.SourceCurrency);
            ViewBag.DCurrency = new SelectList(currencyddl, "value", "Text", data.DestinationCurrency);
            ViewBag.PaymentType = new SelectList(PaymentTypeddl, "value", "Text");
            return await Task.FromResult(PartialView(mappedData));
        }

        [HttpPost]
        public async Task<IActionResult> ViewConversionRate(PartnerConversionRateFilter conversionFilter)
        {
            var PartnerId = _loggedInUser.Claims.FirstOrDefault(x => x.Type == "PartnerCode").Value;
            conversionFilter.PartnerCode = PartnerId;
            var (result, data) = await _partnerConversionRateServices.ViewConversionRateDetailAsync(conversionFilter);
            var mappedData = _mapper.Map<List<AddPartnerConversionRateVm>>(result);
            ViewBag.ConversionRate = data;
            var currencyddl = await _commonddlServices.GetCurrencyddl();
            var PaymentTypeddl = await _commonddlServices.GetPaymentTypeddl();
            ViewBag.SCurrency = new SelectList(currencyddl, "value", "Text", data.SourceCurrency);
            ViewBag.DCurrency = new SelectList(currencyddl, "value", "Text", data.DestinationCurrency);
            ViewBag.PaymentType = new SelectList(PaymentTypeddl, "value", "Text",conversionFilter.PaymentTypeId);

            if (WebHelper.IsAjaxRequest(Request))
                return await Task.FromResult(PartialView("_ViewServiceCharge", mappedData));


            return await Task.FromResult(PartialView(mappedData));
        }

    }
}
