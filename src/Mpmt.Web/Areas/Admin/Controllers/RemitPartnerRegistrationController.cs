using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Mpmt.Core.Dtos.Partner;
using Mpmt.Services.Partner;
using Mpmt.Services.Services.RoleMenuPermission;
using Mpmt.Web.Common;
using Mpmt.Web.Filter;
using System.Net;

namespace Mpmt.Web.Areas.Admin.Controllers
{
    [RolePremission]
    [AdminAuthorization]
    public class RemitPartnerRegistrationController : BaseAdminController
    {
        private readonly IRemitPartnerRegisterServices _remitPartnerRegisterServices;
        private readonly IRMPService _rMPService;
        private readonly INotyfService _notyfService;
        private readonly IMapper _mapper;

        public RemitPartnerRegistrationController(IRemitPartnerRegisterServices remitPartnerRegisterServices,
             IRMPService rMPService, INotyfService notyfService, IMapper mapper)
        {
            _remitPartnerRegisterServices = remitPartnerRegisterServices;
            _rMPService = rMPService;
            _notyfService = notyfService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> RemitPartnerRegistrationIndex([FromQuery] RemitPartnerRegisterFilter request)
        {
            var actions = await _rMPService.GetActionPermissionListAsync("RemitPartnerRegistration");

            ViewBag.actions = actions;
            var data = await _remitPartnerRegisterServices.GetRemitPartnerAsync(request);
            if (WebHelper.IsAjaxRequest(Request))
            {
                return PartialView("_RemitPartnerRegistrationList", data);
            }
            return View(data);
        }

        [HttpGet]
        public async Task<IActionResult> ApprovedRemitPartnerRegistration(Guid id, string email)
        {
            return await Task.FromResult(PartialView(new RemitPartnerRegister { Id = id, Email = email }));
        }

        [HttpPost]
        [LogUserActivity("approved a remit partner registration")]
        public async Task<IActionResult> ApprovedRemitPartnerRegistration(RemitPartnerRegister remitPartner)
        {
            var request = new RemitPartnerRequest { Id = remitPartner.Id, Email = remitPartner.Email ,shortName = remitPartner.Shortname };
            var ResponseStatus = await _remitPartnerRegisterServices.ApprovedPartnerRequest(request, User);
            if (ResponseStatus.StatusCode == 200)
            {
                _notyfService.Success(ResponseStatus.MsgText);
                return Ok();
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                ViewBag.Error = ResponseStatus.MsgText;
                return PartialView(remitPartner);
            }

        }

        [HttpGet]
        public async Task<IActionResult> RejectRemitPartnerRegistration(Guid id, string email)
        {
            return await Task.FromResult(PartialView(new RemitPartnerRegister { Id = id, Email = email }));
        }

        [HttpPost]
        [LogUserActivity("rejected a remit partner registration")]
        public async Task<IActionResult> RejectRemitPartnerRegistration(RemitPartnerRegister remitPartner)
        {
            var request = new RemitPartnerRequest { Id = remitPartner.Id, Email = remitPartner.Email };
            var ResponseStatus = await _remitPartnerRegisterServices.RejectPartnerRequest(request, User);
            if (ResponseStatus.StatusCode == 200)
            {
                _notyfService.Success(ResponseStatus.MsgText);
                return Ok();
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                ViewBag.Error = ResponseStatus.MsgText;
                return PartialView(remitPartner);
            }
        }

        [HttpGet]
        public async Task<IActionResult> RemitPartnerRegistrationDetails(string Email)
        {
            var remitPartner = await _remitPartnerRegisterServices.GetRegisterPartner(Email);
            return PartialView(remitPartner);
        }


      
    }
}
