using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Mpmt.Core.Domain.Modules;
using Mpmt.Core.Dtos.DropDown;
using Mpmt.Core.Dtos.Partner;
using Mpmt.Core.Dtos.PrefundRequest;
using Mpmt.Core.Models.Mail;
using Mpmt.Services.Hubs;
using Mpmt.Services.Services.Common;
using Mpmt.Services.Services.MailingService;
using Mpmt.Services.Services.Notification;
using Mpmt.Services.Services.PreFund;
using Mpmt.Services.Services.RoleMenuPermission;
using Mpmt.Web.Common;
using Mpmt.Web.Filter;
using System.Net;

namespace Mpmt.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// The pre fund request controller.
    /// </summary>
    [RolePremission]
    [AdminAuthorization]
    public class PreFundRequestController : BaseAdminController
    {
        private readonly INotificationService _notificationService;
        private readonly IHubContext<UserHub> _hubcontext;
        private readonly IPreFundRequestServices _fundRequestServices;
        private readonly INotyfService _notyfService;
        private readonly IMapper _mapper;
        private readonly IRMPService _rMPService;
        private readonly IMailService _mailService;
        private readonly ICommonddlServices _commonddl;

        /// <summary>
        /// Initializes a new instance of the <see cref="PreFundRequestController"/> class.
        /// </summary>
        /// <param name="fundRequestServices">The fund request services.</param>
        /// <param name="notyfService">The notyf service.</param>
        /// <param name="mapper">The mapper.</param>
        public PreFundRequestController(INotificationService notificationService,
            IHubContext<UserHub> hubcontext,
            IPreFundRequestServices fundRequestServices,
            INotyfService notyfService,
            IMapper mapper,ICommonddlServices commonddl,
            IRMPService rMPService,

            IMailService mailService)
        {
            _notificationService = notificationService;
            _hubcontext = hubcontext;
            _fundRequestServices = fundRequestServices;
            _notyfService = notyfService;
            _mapper = mapper;
            _rMPService = rMPService;
            _mailService = mailService;
            _commonddl = commonddl;

        }

        #region List,View-PrefundRequest
        /// <summary>
        /// Pres the fund request index.
        /// </summary>
        /// <param name="requestFilter">The request filter.</param>
        /// <returns>A Task.</returns>
        [HttpGet]
        public async Task<IActionResult> PreFundRequestIndex([FromQuery] PrefundRequestFilter requestFilter)
        {
            Response.Headers["Cache-Control"] = "no-store, no-cache, must-revalidate";
            Response.Headers["Pragma"] = "no-cache";
            Response.Headers["Expires"] = "0";

            IEnumerable<Commonddl> statusdetailDdl = await _commonddl.GetStatusListDdl();
            ViewBag.statusListDdl = new SelectList(statusdetailDdl, "value", "Text");


            var actions = await _rMPService.GetActionPermissionListAsync("PreFundRequest");

            ViewBag.actions = actions;

            var data = await _fundRequestServices.GetPreFundRequestAsync(requestFilter);
            if (WebHelper.IsAjaxRequest(Request))
            {
                return PartialView("_PreFundRequestList", data);
            }

            return View(data);
        }


        /// <summary>
        /// Pres the fund request details.
        /// </summary>
        /// <param name="Id">The id.</param>
        /// <returns>A Task.</returns>
        [HttpGet]
        public async Task<IActionResult> PreFundRequestDetails(int Id)
        {
            var actions = await _rMPService.GetActionPermissionListAsync("PreFundRequest");
            ViewBag.actions = actions;

            var data = await _fundRequestServices.GetPreFundRequestByIdAsync(Id);
            return PartialView(data);
        }

        #endregion

        #region List,View-PrefundRequestApproved
        /// <summary>
        /// Pres the fund request approved index.
        /// </summary>
        /// <param name="requestFilter">The request filter.</param>
        /// <returns>A Task.</returns>
        [HttpGet]
        public async Task<IActionResult> PreFundRequestApprovedIndex([FromQuery] PrefundRequestFilter requestFilter)
        {
            Response.Headers["Cache-Control"] = "no-store, no-cache, must-revalidate";
            Response.Headers["Pragma"] = "no-cache";
            Response.Headers["Expires"] = "0";
            var actions = await _rMPService.GetActionPermissionListAsync("PreFundRequest");

            ViewBag.actions = actions;

            var data = await _fundRequestServices.GetPreFundRequestApprovedAsync(requestFilter);
            if (WebHelper.IsAjaxRequest(Request))
            {
                return PartialView("_PreFundRequestApprovedList", data);
            }

            return View(data);
        }

        /// <summary>
        /// Pres the fund request approved details.
        /// </summary>
        /// <param name="Id">The id.</param>
        /// <returns>A Task.</returns>
        [HttpGet]
        public async Task<IActionResult> PreFundRequestApprovedDetails(int Id)
        {
            var data = await _fundRequestServices.GetPreFundRequestApprovedByIdAsync(Id);
            return PartialView(data);
        }
        #endregion

        #region Approved-PreFundRequest

        /// <summary>
        /// Approveds the pre fund request.
        /// </summary>
        /// <param name="Id">The id.</param>
        /// <returns>A Task.</returns>
        [HttpGet]
        public async Task<IActionResult> ApprovedPreFundRequest(int Id)
        {
            var PrefundrequestDeatils = await _fundRequestServices.GetPreFundRequestByIdAsync(Id);
            return await Task.FromResult(PartialView(PrefundrequestDeatils));
        }

        /// <summary>
        /// Approveds the pre fund request.
        /// </summary>
        /// <param name="preFundRequest">The pre fund request.</param>
        /// <returns>A Task.</returns>
        [HttpPost]
        [LogUserActivity("approved a prefund request")]
        public async Task<IActionResult> ApprovedPreFundRequest(PreFundRequest preFundRequest)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView();
            }

            var mappeddata = new FundRequestStatusUpdate { FundRequestId = preFundRequest.Id };
            var approveReqStatus = await _fundRequestServices.ApprovedFundRequestStatusAsync(mappeddata, User);
            if (approveReqStatus.StatusCode != 200)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                ViewBag.Error = approveReqStatus.MsgText;
                return PartialView(preFundRequest);
            }

            //var prefundDetails = await _fundRequestServices.GetPreFundRequestByIdAsync(preFundRequest.Id);
            var prefundDetails = preFundRequest;
            if (prefundDetails?.Email is not null)
            {
                var mailContent = GenerateFundRequestApprovedEmailBody(prefundDetails);
                var mailServiceModel = new MailServiceModel
                {
                    MailFor = string.Empty,
                    MailTo = prefundDetails.Email,
                    RecipientName = string.Empty,
                    MailSubject = "MyPay Money Transfer - Prefund Request Approved",
                    MailBody = mailContent,
                    MailBcc = string.Empty,
                    MailCc = "saroj.chaudhary@mypay.com.np",
                    Content = string.Empty
                };
                var emailThread = new Thread(() => _mailService.SendMail(mailServiceModel)) { IsBackground = true };
                emailThread.Start();
            }
            await _notificationService.IUDNotificationAsync($"Fund Request Approved!", NotificationModules.FundRequest, "/admin/prefundrequest/prefundrequestapprovedindex", "/partner/wallets/prefundapprovedindex", preFundRequest.PartnerCode);
            var Acount = await _notificationService.GetAdminNotificationCountAsync();
            var Pcount = await _notificationService.GetPartnerNotificationCountAsync();
            await _hubcontext.Clients.Groups("Admin").SendAsync("updateTotalCount", Acount);
            await _hubcontext.Clients.Groups(preFundRequest.PartnerCode).SendAsync("partnerupdateTotalCount", Pcount);

            _notyfService.Success(approveReqStatus.MsgText);
            return Ok();
        }

        #endregion

        #region Reject-PreFundRequest

        /// <summary>
        /// Rejects the pre fund request.
        /// </summary>
        /// <param name="Id">The id.</param>
        /// <returns>A Task.</returns>
        [HttpGet]
        public async Task<IActionResult> RejectPreFundRequest(int Id)
        {
            var PrefundrequestDeatils = await _fundRequestServices.GetPreFundRequestByIdAsync(Id);
            return await Task.FromResult(PartialView(PrefundrequestDeatils));
        }

        /// <summary>
        /// Rejects the pre fund request.
        /// </summary>
        /// <param name="preFundRequest">The pre fund request.</param>
        /// <returns>A Task.</returns>
        [HttpPost]
        [LogUserActivity("rejected a prefund request")]
        public async Task<IActionResult> RejectPreFundRequest(PreFundRequest preFundRequest)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView();
            }

            var mappeddata = new FundRequestStatusUpdate { FundRequestId = preFundRequest.Id, Remarks = preFundRequest.Remarks };
            var ResponseStatus = await _fundRequestServices.RejectFundRequestStatusAsync(mappeddata, User);
            if (ResponseStatus.StatusCode != 200)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                ViewBag.Error = ResponseStatus.MsgText;
                return PartialView(preFundRequest);
            }

            var prefundDetails = await _fundRequestServices.GetPreFundRequestByIdAsync(preFundRequest.Id);
            if (prefundDetails?.Email is not null)
            {
                var mailContent = GenerateFundRequestRejectedEmailBody(prefundDetails);
                var mailServiceModel = new MailServiceModel
                {
                    MailFor = string.Empty,
                    MailTo = prefundDetails.Email,
                    RecipientName = string.Empty,
                    MailSubject = "MyPay Money Transfer - Prefund Request Rejected",
                    MailBody = mailContent,
                    MailBcc = string.Empty,
                    MailCc = "saroj.chaudhary@mypay.com.np",
                    Content = string.Empty
                };
                var emailThread = new Thread(() => _mailService.SendMail(mailServiceModel)) { IsBackground = true };
                emailThread.Start();
            }
            await _notificationService.IUDNotificationAsync($"Fund Request Rejected!", NotificationModules.FundRequest, "/admin/prefundrequest/prefundrequestindex", "/partner/wallets/prefundindex", preFundRequest.PartnerCode);
            var Acount = await _notificationService.GetAdminNotificationCountAsync();
            var Pcount = await _notificationService.GetPartnerNotificationCountAsync();
            await _hubcontext.Clients.Groups("Admin").SendAsync("updateTotalCount", Acount);
            await _hubcontext.Clients.Groups(preFundRequest.PartnerCode).SendAsync("partnerupdateTotalCount", Pcount);
            _notyfService.Success(ResponseStatus.MsgText);
            return Ok();
        }

        #endregion

        private string GenerateFundRequestApprovedEmailBody(PreFundRequest details)
        {
            var companyName = "MyPay Money Transfer Pvt. Ltd.";
            var companyAlias = "MyPay Money Transfer";
            var companyEmail = "support@mypaymt.com";

            string mailBody =
                $@"<p>Dear partner,</p>
                    <p>We are pleased to inform you that your prefund request with ID {details.Id} for {details.Amount ?? string.Empty} {details.SourceCurrency ?? string.Empty} has been approved. Your commitment to our partnership is greatly appreciated, and we are here to support your financial goals.</p>
                    <p>Thank you!</p>
                    <br />
                    <h3><u>{companyAlias} Service Contact Information:</u></h3>
                    <p>
                        {companyName}<br>
                        Contact No.: <br>
                        Email: {companyEmail}<br>
                        Website: <br>
                        Address: <br>
                    </p>
                    <p>Thank you for choosing {companyAlias} Service!</p>

                <p>CONFIDENTIALITY NOTICE: This transmittal is a confidential communication or may otherwise be privileged. If you are not the intended recipient, you are hereby notified that you have received this transmittal in error and that any review, dissemination, distribution or copying of this transmittal is strictly prohibited. If you have received this communication in error, please notify this office, and immediately delete this message and all its attachments, if any.</p>";

            return mailBody;
        }

        private string GenerateFundRequestRejectedEmailBody(PreFundRequest details)
        {
            var companyName = "MyPay Money Transfer Pvt. Ltd.";
            var companyAlias = "MyPay Money Transfer";
            var companyEmail = "support@mypaymt.com";

            string mailBody =
                $@"<p>Dear partner,</p>
                    <p>We regret to inform you that your prefund request with ID {details.Id} for {details.Amount ?? string.Empty} {details.SourceCurrency ?? string.Empty} has been carefully reviewed and, unfortunately, it has been rejected due to failing to meet the requirements for the fund request.</p>
                    <p>Thank you!</p>
                    <br />
                    <h3><u>{companyAlias} Service Contact Information:</u></h3>
                    <p>
                        {companyName}<br>
                        Contact No.: <br>
                        Email: {companyEmail}<br>
                        Website: <br>
                        Address: <br>
                    </p>
                    <p>Thank you for choosing {companyAlias} Service!</p>

                <p>CONFIDENTIALITY NOTICE: This transmittal is a confidential communication or may otherwise be privileged. If you are not the intended recipient, you are hereby notified that you have received this transmittal in error and that any review, dissemination, distribution or copying of this transmittal is strictly prohibited. If you have received this communication in error, please notify this office, and immediately delete this message and all its attachments, if any.</p>";

            return mailBody;
        }
    }
}
