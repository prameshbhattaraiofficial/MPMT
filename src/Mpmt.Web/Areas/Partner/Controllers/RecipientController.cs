using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Mpmt.Core.Domain.Partners.Recipient;
using Mpmt.Core.Dtos.DropDown;
using Mpmt.Services.Partner;
using Mpmt.Services.Services.Common;
using Mpmt.Services.Services.RoleMenuPermission;
using Mpmt.Web.Areas.Partner.ViewModels;
using Mpmt.Web.Common;
using Mpmt.Web.Filter;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;

namespace Mpmt.Web.Areas.Partner.Controllers
{
    [PartnerAuthorization]
    [RolePremission]
    public class RecipientController : BasePartnerController
    {
        private readonly IRMPService _rMPService;
        private readonly IPartnerRecipentServices _partnerRecipent;
        private readonly ICommonddlServices _commonddl;
        private readonly INotyfService _notyfService;
        private readonly IMapper _mapper;
        private readonly ICommonddlServices _commonddlService;

        public RecipientController(IRMPService rMPService,
            IPartnerRecipentServices partnerRecipent, ICommonddlServices commonddl, INotyfService notyfService, IMapper mapper, ICommonddlServices commonddlService)
        {
            _rMPService = rMPService;
            _partnerRecipent = partnerRecipent;
            _commonddl = commonddl;
            _notyfService = notyfService;
            _mapper = mapper;
            _commonddlService = commonddlService;
        }

        [HttpGet]
        public async Task<JsonResult> GetRecipientDistrictList([FromQuery][Required] string provinceCode)
        {
            var provinceDdl = await _commonddlService.GetDistrictddl(provinceCode);
            var data = new SelectList(provinceDdl, "value", "Text");
            return Json(data);
        }

        [HttpGet]
        public async Task<JsonResult> GetRecipientLocalLevelList([FromQuery][Required] string districtCode)
        {
            var districtDdl = await _commonddlService.Getlocallevelddl(districtCode);
            var data = new SelectList(districtDdl, "value", "Text");
            return Json(data);
        }

        [HttpGet]
        public async Task<IActionResult> RecipientsView([FromQuery] RecipientFilter recipientFilter)
        {
            var actions = await _rMPService.GetPartnerActionPermissionList("Recipient");
            ViewBag.actions = actions;

            Response.Headers["Cache-Control"] = "no-store, no-cache, must-revalidate";
            Response.Headers["Pragma"] = "no-cache";
            Response.Headers["Expires"] = "0";
            ViewBag.SenderId = recipientFilter.SenderId;
            var data = await _partnerRecipent.GetRecipientsAsync(recipientFilter,User);
            if (WebHelper.IsAjaxRequest(Request))
            {
                return PartialView("_PartnerRecipientsList", data);
            }
            return View(data);
        }

        #region Add-Recipients
        [HttpGet]
        public async Task<IActionResult> RecipientsAdd(int SenderId)
        {
            var country = await _commonddl.GetCountryddl();
            var state = await _commonddl.Getprovinceddl(null);
            var District = await _commonddl.GetDistrictddl(null);
            var LocalBody = await _commonddl.Getlocallevelddl(null);
            var Relationship = await _commonddl.GetRelationShipddl();
            var bank = await _commonddl.GetBankddl();
            var Currency = await _commonddl.GetCurrencyddl();
            var PaymentType = await _commonddl.GetPaymentTypeddl();
            var UtcTimeZone = await _commonddl.GetUtcTimeZoneddl();


            ViewBag.Countryddl = new SelectList(country, "value", "Text");
            ViewBag.SourceCurrencyddl = new SelectList(Currency, "value", "Text");
            ViewBag.DestinationCurrencyddl = new SelectList(Currency, "value", "Text");
            ViewBag.Paymenttypeddl = new SelectList(PaymentType, "value", "Text");
            ViewBag.stateddl = new SelectList(state, "value", "Text");
            ViewBag.Relationshipddl = new SelectList(Relationship, "value", "Text");
            ViewBag.bankddl = new SelectList(bank, "value", "Text");
            ViewBag.UtcTimeZoneddl = new SelectList(UtcTimeZone, "value", "Text");
            ViewBag.Districtddl = new SelectList(District, "value", "Text");
            ViewBag.LocalBodyddl = new SelectList(LocalBody, "value", "Text");

            var data = new RecipientsAddUpdateViewmodel();
            data.SenderId = SenderId;
            //ViewBag.SenderId = SenderId;
            return PartialView(data);
        }
        [HttpPost]
        [LogUserActivity("Add Recipient")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> RecipientsAdd(RecipientsAddUpdateViewmodel recipientsAddUpdate)
        {
            var country = await _commonddl.GetCountryddl();
            var state = await _commonddl.Getprovinceddl(null);
            var District = await _commonddl.GetDistrictddl(null);
            var LocalBody = await _commonddl.Getlocallevelddl(null);
            var Relationship = await _commonddl.GetRelationShipddl();
            var bank = await _commonddl.GetBankddl();
            var Currency = await _commonddl.GetCurrencyddl();
            var PaymentType = await _commonddl.GetPaymentTypeddl();
            var UtcTimeZone = await _commonddl.GetUtcTimeZoneddl();


            ViewBag.Countryddl = new SelectList(country, "value", "Text");
            ViewBag.SourceCurrencyddl = new SelectList(Currency, "value", "Text");
            ViewBag.DestinationCurrencyddl = new SelectList(Currency, "value", "Text");
            ViewBag.Paymenttypeddl = new SelectList(PaymentType, "value", "Text");
            ViewBag.stateddl = new SelectList(state, "value", "Text");
            ViewBag.Relationshipddl = new SelectList(Relationship, "value", "Text");
            ViewBag.bankddl = new SelectList(bank, "value", "Text");
            ViewBag.UtcTimeZoneddl = new SelectList(UtcTimeZone, "value", "Text");
            ViewBag.Districtddl = new SelectList(District, "value", "Text");
            ViewBag.LocalBodyddl = new SelectList(LocalBody, "value", "Text");

            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView();
            }
            else
            {
                var mappeddata = _mapper.Map<RecipientAddUpdate>(recipientsAddUpdate);
                var ResponseStatus = await _partnerRecipent.AddRecipientAsync(mappeddata, User);
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

        #region Update-Recipients
        [HttpGet]
        public async Task<IActionResult> RecipientsUpdate(int Recipientid)
        {
            var country = await _commonddl.GetCountryddl();
            var state = await _commonddl.Getprovinceddl(null);
            var District = await _commonddl.GetDistrictddl(null);
            var LocalBody = await _commonddl.Getlocallevelddl(null);
            var Relationship = await _commonddl.GetRelationShipddl();
            var bank = await _commonddl.GetBankddl();
            var Currency = await _commonddl.GetCurrencyddl();
            var PaymentType = await _commonddl.GetPaymentTypeddl();
            var UtcTimeZone = await _commonddl.GetUtcTimeZoneddl();


            ViewBag.Countryddl = new SelectList(country, "value", "Text");
            ViewBag.SourceCurrencyddl = new SelectList(Currency, "value", "Text");
            ViewBag.DestinationCurrencyddl = new SelectList(Currency, "value", "Text");
            ViewBag.Paymenttypeddl = new SelectList(PaymentType, "value", "Text");
            ViewBag.stateddl = new SelectList(state, "value", "Text");
            ViewBag.Relationshipddl = new SelectList(Relationship, "value", "Text");
            ViewBag.bankddl = new SelectList(bank, "value", "Text");
            ViewBag.UtcTimeZoneddl = new SelectList(UtcTimeZone, "value", "Text");
            ViewBag.Districtddl = new SelectList(District, "value", "Text");
            ViewBag.LocalBodyddl = new SelectList(LocalBody, "value", "Text");

            var data = await _partnerRecipent.GetRecipientsByIdAsync(Recipientid);
            var mappeddata = _mapper.Map<RecipientsAddUpdateVm>(data);
            return PartialView(mappeddata);
        }

        [HttpPost]
        [LogUserActivity("Update Recipient")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RecipientsUpdate(RecipientsAddUpdateVm recipientsAddUpdate)
        {
            var country = await _commonddl.GetCountryddl();
            var state = await _commonddl.Getprovinceddl(null);
            var District = await _commonddl.GetDistrictddl(null);
            var LocalBody = await _commonddl.Getlocallevelddl(null);
            var Relationship = await _commonddl.GetRelationShipddl();
            var bank = await _commonddl.GetBankddl();
            var Currency = await _commonddl.GetCurrencyddl();
            var PaymentType = await _commonddl.GetPaymentTypeddl();
            var UtcTimeZone = await _commonddl.GetUtcTimeZoneddl();


            ViewBag.Countryddl = new SelectList(country, "value", "Text");
            ViewBag.SourceCurrencyddl = new SelectList(Currency, "value", "Text");
            ViewBag.DestinationCurrencyddl = new SelectList(Currency, "value", "Text");
            ViewBag.Paymenttypeddl = new SelectList(PaymentType, "value", "Text");
            ViewBag.stateddl = new SelectList(state, "value", "Text");
            ViewBag.Relationshipddl = new SelectList(Relationship, "value", "Text");
            ViewBag.bankddl = new SelectList(bank, "value", "Text");
            ViewBag.UtcTimeZoneddl = new SelectList(UtcTimeZone, "value", "Text");
            ViewBag.Districtddl = new SelectList(District, "value", "Text");
            ViewBag.LocalBodyddl = new SelectList(LocalBody, "value", "Text");

            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView();
            }
            else
            {
                var mappeddata = _mapper.Map<RecipientAddUpdate>(recipientsAddUpdate);
                var ResponseStatus = await _partnerRecipent.UpdateRecipientAsync(mappeddata, User);
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

        #region Details-Recipients
        [HttpGet]
        public async Task<IActionResult> RecipientsDetails(int Recipientid)
        {
            var data = await _partnerRecipent.GetRecipientsByIdAsync(Recipientid);
            var mappeddata = _mapper.Map<RecipientsAddUpdateVm>(data);
            return PartialView(mappeddata);
        }
        #endregion


        #region Ddl
        [HttpGet]
        public async Task<JsonResult> GetProvisionList([FromQuery] string CountryCode = "NP")
        {
            
             var Provision = await _commonddl.Getprovinceddl(CountryCode);
            var data = Provision.Select(x => new { x.Text, x.value });
            var jsonString = JsonSerializer.Serialize(data);
            return Json(jsonString);
        }

        [HttpGet]
        public async Task<JsonResult> GetDistrictList([FromQuery] string provinceCode)
        {
          
             var District = await _commonddl.GetDistrictddl(provinceCode);
            var data = District.Select(x=> new { x.Text, x.value } );
            var jsonString = JsonSerializer.Serialize(data);
            return Json(jsonString);
           // return Ok(data);
        }

        [HttpGet]
        public async Task<JsonResult> GetLocalLevalList([FromQuery] string DistrictCode)
        {
         
                var LocalLeval = await _commonddl.Getlocallevelddl(DistrictCode);

            var data = LocalLeval.Select(x => new { x.Text, x.value });
            var jsonString = JsonSerializer.Serialize(data);
            return Json(jsonString);
        }
        #endregion
    }
}
