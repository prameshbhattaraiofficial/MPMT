 using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Mpmt.Core.Domain;
using Mpmt.Core.Dtos.ComplianceRule;
using Mpmt.Core.Dtos.DropDown;
using Mpmt.Core.Dtos.PartnerReport;
using Mpmt.Core.Dtos.Roles;
using Mpmt.Core.ViewModel.AdminReport;
using Mpmt.Services.Services.Common;
using Mpmt.Services.Services.ComplianceRule;
using Mpmt.Services.Services.RoleMenuPermission;
using Mpmt.Web.Areas.Admin.ViewModels.Paetner;
using Mpmt.Web.Common;
using Mpmt.Web.Filter;
using Mpmts.Core.Dtos;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Diagnostics.Metrics;
using System.Net;


namespace Mpmt.Web.Areas.Admin.Controllers
{
    [RolePremission]
    [AdminAuthorization]
    public class ComplianceRuleController : BaseAdminController
    {
        private readonly IComplianceRuleService _service;
        private readonly INotyfService _notyfService;
        private readonly IRMPService _rMPService;
        private readonly ICommonddlServices _commonddlServices;


        public ComplianceRuleController(IComplianceRuleService service, INotyfService notyfService, IRMPService rMPService, ICommonddlServices commonddlServices )
        {
            _service = service;
            _notyfService = notyfService;
            _rMPService = rMPService;
            _commonddlServices = commonddlServices;
        }

        [HttpGet]
        public async Task<IActionResult> ComplianceRuleIndex([FromQuery] ComplianceRuleFilter filter)   
        {
            var actions = await _rMPService.GetActionPermissionListAsync("ComplianceRule");
            ViewBag.actions = actions;

            var result = await _service.GetComplianceRuleAsync(filter);
            var getFrequency = await _commonddlServices.GetFrequency();
            ViewBag.getFrequency = new SelectList(getFrequency, "value", "Text");

            if (WebHelper.IsAjaxRequest(Request))
                return await Task.FromResult(PartialView("_ComplianceRuleIndex", result));
            return await Task.FromResult(View(result));
        }

        [HttpPost]
        [LogUserActivity("updated compliance rule")]
        public async Task<IActionResult> UpdateComplianceRule(ComplianceRuleDetail complianceRule)
        {
            var getFrequency = await _commonddlServices.GetFrequency();
            ViewBag.getFrequency = new SelectList(getFrequency, "value", "Text");
            var responseStatus = await _service.UpdateComplianceRule(complianceRule);
            if (responseStatus.StatusCode == 200)
            {
                _notyfService.Success(responseStatus.MsgText);
                return Ok();
            }
            else
            {
                Response.StatusCode = (int)(HttpStatusCode.BadRequest);
                ViewBag.Error = responseStatus.MsgText;
                return PartialView("_ComplianceRuleIndex");
            }
        }

        [HttpGet]
        public async Task<IActionResult> ComplianceTransaction([FromQuery] RemitTxnReportFilter reportFilter)
        {
            var actions = await _rMPService.GetActionPermissionListAsync("ComplianceTransaction");
            ViewBag.actions = actions;
            var currencyddl = await _commonddlServices.GetCurrencyddl();
            ViewBag.SCurrency = new SelectList(currencyddl, "value", "Text");
            ViewBag.DCurrency = new SelectList(currencyddl, "value", "Text");
            reportFilter.PartnerCode = "admin";
            var complianceTransaction = await _service.GetComplianceTransactionAsync(reportFilter);
            ViewBag.TransactionFilter = complianceTransaction.Items.FirstOrDefault();
            if (WebHelper.IsAjaxRequest(Request))
                return await Task.FromResult(PartialView("_ComplianceTransactionList", complianceTransaction));
            return await Task.FromResult(View(complianceTransaction));
        }
        [HttpGet]
        public async Task<IActionResult> ReleaseComplianceTransaction(string TransactionId)
        {
            RemitTxnReport report = new RemitTxnReport();
            report.TransactionId = TransactionId;
            return PartialView("_AdminComplianceTxnRelease", report);
        }
        [HttpPost, LogUserActivity("Released compliance transaction")]
        public async Task<IActionResult> ReleaseComplianceTransaction(RemitTxnReport model)
        {
            var ReleaseComplianceTxn = await _service.ReleaseTransaction(model.TransactionId,User);

            if (ReleaseComplianceTxn.StatusCode==200)
            {
                _notyfService.Success(ReleaseComplianceTxn.MsgText);
                return Ok();
            }
            else
            {
                _notyfService.Error(ReleaseComplianceTxn.MsgText);
                return Ok();
            }
        }
        [HttpGet]
        public async Task<IActionResult> CountryComplianceSetup()
        {
            CountryComplianceRule complianceRule = new CountryComplianceRule();
            var countryList = await _service.GetAllCountryList();
            var ComplianceCountryList = await _service.GetComplianceCountryList();
            var country = new List<SelectListItem>();
            foreach (var countryItem in ComplianceCountryList) 
            {
                country.Add(new SelectListItem() { Value = countryItem.CountryCode, Text = countryItem.CountryName, Selected = true });
            }
            foreach (var countryall in countryList)
            {
                country.Add(new SelectListItem() { Value = countryall.CountryCode, Text = countryall.CountryName, Selected = false });
            }
            ViewBag.CountryList = country;
            return View();
        }
        [HttpPost]
        [LogUserActivity("updated country compliance")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CountryComplianceSetup( CountryComplianceRuleTest model)
        {
            string countryListString = string.Join(",", model.CountryCode ?? Array.Empty<string>());
            var result = await _service.AddComplianceCountryList(countryListString);
            if(result.StatusCode == 200)
            {
                _notyfService.Success("Compliance country added succesfully");
            }
            else
            {
                _notyfService.Error(result.MsgText);

            }
            return RedirectToAction("CountryComplianceSetup", "ComplianceRule");
        }
    }
}
