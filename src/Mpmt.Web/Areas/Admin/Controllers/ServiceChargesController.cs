using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Mpmt.Core.Dtos.DropDown;
using Mpmt.Core.Dtos.ServiceCharge;
using Mpmt.Core.ViewModel.ServiceCharge;
using Mpmt.Services.Services.Common;
using Mpmt.Services.Services.RoleMenuPermission;
using Mpmt.Services.Services.ServiceCharge;
using Mpmt.Web.Common;
using Mpmt.Web.Filter;
using System.Net;

namespace Mpmt.Web.Areas.Admin.Controllers
{
    [RolePremission]
    [AdminAuthorization]
    public class ServiceChargesController : BaseAdminController
    {
        private readonly IServiceChargeServices _serviceChargeServices;
        private readonly INotyfService _notyfService;
        private readonly ICommonddlServices _commonddlServices;
        private readonly IRMPService _rMPService;
        private readonly IMapper _mapper;

        public ServiceChargesController(IServiceChargeServices serviceChargeServices, INotyfService notyfService, IMapper mapper, ICommonddlServices commonddlServices, IRMPService rMPService)
        {
            _serviceChargeServices = serviceChargeServices;
            _notyfService = notyfService;
            _mapper = mapper;
            _commonddlServices = commonddlServices;
            _rMPService = rMPService;
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] ServiceChargeFilter serviceChargeFilter)
        {
            var actions = await _rMPService.GetActionPermissionListAsync("ServiceCharges");
            ViewBag.actions = actions;

            var currencyddl = await _commonddlServices.GetCurrencyddl();
            var chargeCategoryddl = await _commonddlServices.GetServiceChargeCategoryddl();
            var paymentTypeddl = await _commonddlServices.GetPaymentTypeddl();
            ViewBag.Currency = new SelectList(currencyddl, "value", "Text");
            ViewBag.ChargeCategory = new SelectList(chargeCategoryddl, "value", "Text");
            ViewBag.PaymentType = new SelectList(paymentTypeddl, "value", "Text");
            var result = await _serviceChargeServices.GetServiceChargeAsync(serviceChargeFilter);
            if (WebHelper.IsAjaxRequest(Request))
                return await Task.FromResult(PartialView("_ServiceChargeIndex", result));
            return await Task.FromResult(View(result));
        }

        #region Add-Service-Charges

        public async Task<IActionResult> AddServiceCharge()
        {
            var currencyddl = await _commonddlServices.GetCurrencyddl();
            IEnumerable<Commonddl> destinationCurrencyDdl = null;
            destinationCurrencyDdl = currencyddl.Where(c => c.value.Equals("NPR", StringComparison.OrdinalIgnoreCase));
            if (!destinationCurrencyDdl.Any())
                destinationCurrencyDdl = new List<Commonddl> { new() { Text = "Nepalese Rupees", value = "NPR" } };

            var chargeCategoryddl = await _commonddlServices.GetServiceChargeCategoryddl();
            var paymentTypeddl = await _commonddlServices.GetPaymentTypeddl();
            ViewBag.SCurrency = new SelectList(currencyddl, "value", "Text");
            ViewBag.DCurrency = new SelectList(currencyddl, "value", "Text");
            ViewBag.ChargeCategory = new SelectList(chargeCategoryddl, "value", "Text");
            ViewBag.PaymentType = new SelectList(paymentTypeddl, "value", "Text");
            ViewBag.DestinationCurrency = new SelectList(destinationCurrencyDdl, "value", "Text");

            return await Task.FromResult(PartialView(new List<AddServiceChargeVm> { new AddServiceChargeVm() }));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [LogUserActivity("added a service charge")]
        public async Task<IActionResult> AddServiceCharge(List<AddServiceChargeVm> serviceChargeVms, int ChargeCategoryId, string SourceCurrency, string DestinationCurrency, int PaymentTypeId)
        {
            var currencyddl = await _commonddlServices.GetCurrencyddl();
            IEnumerable<Commonddl> destinationCurrencyDdl = null;
            destinationCurrencyDdl = currencyddl.Where(c => c.value.Equals("NPR", StringComparison.OrdinalIgnoreCase));
            if (!destinationCurrencyDdl.Any())
                destinationCurrencyDdl = new List<Commonddl> { new() { Text = "Nepalese Rupees", value = "NPR" } };

            var chargeCategoryddl = await _commonddlServices.GetServiceChargeCategoryddl();
            var paymentTypeddl = await _commonddlServices.GetPaymentTypeddl();
            ViewBag.SCurrency = new SelectList(currencyddl, "value", "Text", SourceCurrency);
            ViewBag.DCurrency = new SelectList(currencyddl, "value", "Text", DestinationCurrency);
            ViewBag.ChargeCategory = new SelectList(chargeCategoryddl, "value", "Text", ChargeCategoryId);
            ViewBag.PaymentType = new SelectList(paymentTypeddl, "value", "Text", PaymentTypeId);
            ViewBag.DestinationCurrency = new SelectList(destinationCurrencyDdl, "value", "Text");
            if (string.IsNullOrEmpty(DestinationCurrency))
                DestinationCurrency = "NPR";
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView(serviceChargeVms);
            }
            else
            {
                var responseStatus = await _serviceChargeServices.AddServiceChargeAsync(serviceChargeVms, ChargeCategoryId, PaymentTypeId, SourceCurrency, DestinationCurrency);
                if (responseStatus.StatusCode == 200)
                {
                    _notyfService.Success("Service charge added succesfully.");
                    //return RedirectToAction("Index");
                    return Ok();
                }
                else
                {
                    Response.StatusCode = (int)(HttpStatusCode.BadRequest);
                    ViewBag.Error = responseStatus.MsgText;
                    _notyfService.Error(responseStatus.MsgText);
                    return PartialView(serviceChargeVms);
                }
            }
        }

        #endregion

        #region Delete-Service-Charge

        [HttpGet]
        public async Task<IActionResult> DeleteServiceCharge(int CategoryId, string SourceCurrency, string DestinationCurrency, int PaymentTypeId)
        {
            var (result, data) = await _serviceChargeServices.GetServiceChargeByIdAsync(CategoryId, SourceCurrency, DestinationCurrency, PaymentTypeId);

            return await Task.FromResult(PartialView(data));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [LogUserActivity("deleted a service charge")]
        public async Task<IActionResult> DeleteServiceCharge(ServiceChargeSelect serviceChargeSelect)
        {
            if (serviceChargeSelect.ChargeCategoryId != 0)
            {
                var responseStatus = await _serviceChargeServices.RemoveServiceChargeAsync(serviceChargeSelect);
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
            return PartialView(serviceChargeSelect);
        }

        #endregion

        #region Update-Service-Charge

        [HttpGet]
        public async Task<IActionResult> UpdateServiceCharge(int CategoryId, string SourceCurrency, string DestinationCurrency, int PaymentTypeId)
        {
            var (result, data) = await _serviceChargeServices.GetServiceChargeByIdAsync(CategoryId, SourceCurrency, DestinationCurrency, PaymentTypeId);
            var mappedData = _mapper.Map<List<AddServiceChargeVm>>(result);
            var currencyddl = await _commonddlServices.GetCurrencyddl();
            IEnumerable<Commonddl> destinationCurrencyDdl = null;
            destinationCurrencyDdl = currencyddl.Where(c => c.value.Equals("NPR", StringComparison.OrdinalIgnoreCase));
            if (!destinationCurrencyDdl.Any())
                destinationCurrencyDdl = new List<Commonddl> { new() { Text = "Nepalese Rupees", value = "NPR" } };
            var chargeCategoryddl = await _commonddlServices.GetServiceChargeCategoryddl();
            var paymentTypeddl = await _commonddlServices.GetPaymentTypeddl();
            ViewBag.SCurrency = new SelectList(currencyddl.Where(x => x.value == data.SourceCurrency), "value", "Text");
            ViewBag.DCurrency = new SelectList(currencyddl.Where(x => x.value == data.DestinationCurrency), "value", "Text");
            ViewBag.ChargeCategory = new SelectList(chargeCategoryddl.Where(x => x.value == data.ChargeCategoryId.ToString()), "value", "Text");
            ViewBag.PaymentType = new SelectList(paymentTypeddl.Where(x => x.value == data.PaymentTypeId.ToString()), "value", "Text");
            ViewBag.DestinationCurrency = new SelectList(destinationCurrencyDdl, "value", "Text");

            //ViewBag.SCurrency = data.SourceCurrency;
            //ViewBag.DCurrency = data.DestinationCurrency;
            //ViewBag.ChargeCategory = chargeCategoryddl.FirstOrDefault(x => x.value == data.ChargeCategoryId.ToString()).Text;
            //ViewBag.PaymentType = paymentTypeddl.FirstOrDefault(x => x.value == data.PaymentTypeId.ToString()).Text;
            return await Task.FromResult(PartialView(mappedData));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [LogUserActivity("updated a service charge")]
        public async Task<IActionResult> UpdateServiceCharge([FromForm] List<AddServiceChargeVm> serviceChargeVms, int ChargeCategoryId, string SourceCurrency, string DestinationCurrency, int PaymentTypeId)
        {
            var currencyddl = await _commonddlServices.GetCurrencyddl();
            var chargeCategoryddl = await _commonddlServices.GetServiceChargeCategoryddl();
            var paymentTypeddl = await _commonddlServices.GetPaymentTypeddl();
            IEnumerable<Commonddl> destinationCurrencyDdl = null;
            destinationCurrencyDdl = currencyddl.Where(c => c.value.Equals("NPR", StringComparison.OrdinalIgnoreCase));
            if (!destinationCurrencyDdl.Any())
                destinationCurrencyDdl = new List<Commonddl> { new() { Text = "Nepalese Rupees", value = "NPR" } };
            ViewBag.SCurrency = new SelectList(currencyddl.Where(x => x.value == SourceCurrency), "value", "Text");
            ViewBag.DCurrency = new SelectList(currencyddl.Where(x => x.value == DestinationCurrency), "value", "Text");
            ViewBag.ChargeCategory = new SelectList(chargeCategoryddl.Where(x => x.value == ChargeCategoryId.ToString()), "value", "Text");
            ViewBag.PaymentType = new SelectList(paymentTypeddl.Where(x => x.value == PaymentTypeId.ToString()), "value", "Text");
            ViewBag.DestinationCurrency = new SelectList(destinationCurrencyDdl, "value", "Text");
            //ViewBag.SCurrency = SourceCurrency;
            //ViewBag.DCurrency = DestinationCurrency;
            //ViewBag.ChargeCategory = chargeCategoryddl.FirstOrDefault(x => x.value == ChargeCategoryId.ToString()).Text;
            //ViewBag.PaymentType = paymentTypeddl.FirstOrDefault(x => x.value == PaymentTypeId.ToString()).Text;
            if (string.IsNullOrEmpty(DestinationCurrency))
                DestinationCurrency = "NPR";
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView(serviceChargeVms);
            }
            else
            {
                var responseStatus = await _serviceChargeServices.UpdateServiceChargeAsync(serviceChargeVms, ChargeCategoryId, PaymentTypeId, SourceCurrency, DestinationCurrency);
                if (responseStatus.StatusCode == 200)
                {
                    _notyfService.Success(responseStatus.MsgText);
                    return Ok();
                }
                else
                {
                    Response.StatusCode = (int)(HttpStatusCode.BadRequest);
                    ViewBag.Error = responseStatus.MsgText;
                    return PartialView(serviceChargeVms);
                }
            }
        }

        #endregion
    }
}
