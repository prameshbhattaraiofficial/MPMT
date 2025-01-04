using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Mpmt.Core.Domain.Modules;
using Mpmt.Core.Domain.Partners.Register;
using Mpmt.Core.Dtos.Notification;
using Mpmt.Core.Dtos.Partner;
using Mpmt.Core.Dtos.PartnerSignUp;
using Mpmt.Core.Models.Mail;
using Mpmt.Data.Repositories.Notification;
using Mpmt.Services.Common;
using Mpmt.Services.Hubs;
using Mpmt.Services.Services.Common;
using Mpmt.Services.Services.MailingService;
using Mpmt.Services.Services.Notification;
using Mpmt.Services.Services.PartnerRegister;
using Mpmt.Services.Services.Receipt;
using Mpmt.Web.Areas.Admin.ViewModels.Paetner;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Mpmt.Web.Areas.Partner.Controllers
{
    public class RegistrationController : BasePartnerController
    {
        private readonly INotyfService _notyfService;
        private readonly IRegisterService _registerService;
        private readonly IMapper _mapper;
        private readonly INotificationService _notificationService;
        private readonly IHubContext<UserHub> _hubcontext;
        private readonly ICommonddlServices _commonddl;
        private readonly IMailService _mailService;
        private readonly INotificationRepo _notificationRepo;
        private readonly IReceiptGenerationService _receiptGenerationService;

        public RegistrationController(INotyfService notyfService,
            IRegisterService registerService, IMapper mapper
            , ICommonddlServices commonddl, IMailService mailService, IReceiptGenerationService receiptGenerationService, INotificationService notificationService, IHubContext<UserHub> hubcontext, INotificationRepo notificationRepo)
        {
            _notyfService = notyfService;
            _registerService = registerService;
            _mapper = mapper;
            _commonddl = commonddl;
            _mailService = mailService;
            _receiptGenerationService = receiptGenerationService;
            _notificationService = notificationService;
            _hubcontext = hubcontext;
            _notificationRepo = notificationRepo;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> SignUp()
        {
            var CallingCode = await _commonddl.GetCallingCodeddl();
            ViewBag.CallingCode = new SelectList(CallingCode, "Text", "lookup");

            DateTime otpGenerationTime = GetOtpGenerationTimeFromDatabase();
            TimeSpan remainingTime = otpGenerationTime.AddMinutes(5) - DateTime.Now;
            ViewBag.RemainingTimeInSeconds = (int)remainingTime.TotalSeconds;
            return View();
        }

        //[RegisterDetailFilter]
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpPartner signUpPartner)
        {

            var CallingCode = await _commonddl.GetCallingCodeddl();
            ViewBag.CallingCode = new SelectList(CallingCode, "Text", "lookup");

            if (ModelState.IsValid)
            {
                var data = new SignUpPartnerdetail()
                {
                    Email = signUpPartner.Email,
                    Password = signUpPartner.Password,
                    PhoneNumber = signUpPartner.PhoneNumber,
                    FirstName = signUpPartner.FirstName,
                    //ShortName = signUpPartner.ShortName,
                    LastName = signUpPartner.LastName,
                    Position = signUpPartner.Position,
                    Withoutfirstname = signUpPartner.Withoutfirstname,


                };

                var otp = OtpGeneration.GenerateRandom6DigitCode();
                data.Otp = otp;

                var response = await _registerService.RegisterPartner(data);
                var otpvalid = new Otpvalidatiom();
                otpvalid.Email = signUpPartner.Email;
                if (response.StatusCode == 200)
                {
                    var Content = _receiptGenerationService.GenrateMailBodyforOtp(otp);
                    var mailRequest = new MailRequestModel
                    {
                        MailFor = "confirm-email",
                        MailTo = data.Email,
                        MailSubject = "Your One-Time Password (OTP) for Registration",
                        RecipientName = data.LastName,
                        Content = Content

                    };
                    var mailServiceModel = await _mailService.EmailSettings(mailRequest);

                    Thread email = new(delegate ()
                    {
                        _mailService.SendMail(mailServiceModel);
                    });
                    email.Start();

                    return View("OTPValidation", otpvalid);

                }
                ViewBag.Error = response.MsgText;

            }
            return View(signUpPartner);
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Resendotp(string Email)
        {

            var otpvalid = new Otpvalidatiom();
            otpvalid.Email = Email;
            var otp = OtpGeneration.GenerateRandom6DigitCode();
            var response = await _registerService.ResetOtp(Email, otp);
            if (response.StatusCode == 200)
            {
                var Content = _receiptGenerationService.GenrateMailBodyforOtp(otp);
                var mailRequest = new MailRequestModel
                {
                    MailFor = "confirm-email",
                    MailTo = Email,
                    MailSubject = "Your One-Time Password (OTP) for Registration",
                    RecipientName = "User",
                    Content = Content

                };
                var mailServiceModel = await _mailService.EmailSettings(mailRequest);

                Thread email = new(delegate ()
                {
                    _mailService.SendMail(mailServiceModel);
                });
                email.Start();
                _notyfService.Success("Mail Send");
                return Ok();
            }
            else
            {
                _notyfService.Error(response.MsgText);
                return Ok();
            }

        }

        //[RegisterDetailFilter]
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Verifyotp(Otpvalidatiom validate)
        {
            if (ModelState.IsValid)
            {
                var response = await _registerService.ValidateOtp(validate.Email, validate.Otp);
                if (response.StatusCode == 200)
                {
                    var detail = await _registerService.GetRegisterPartner(validate.Email);
                    return RedirectToAction("Step2", "Registration", new { Token = detail.Id });
                }
                else
                {
                    ModelState.AddModelError("Otp", response.MsgText);
                    return View("OTPValidation", validate); ;
                }
            }

            return View("OTPValidation", validate);
        }
        //[RegisterDetailFilter]
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Step1(string Token)
        {

            var data = new SignUpStep1();

            //det data from email
            if (!string.IsNullOrEmpty(Token))
            {

                data.Token = Token;
                var detail = await _registerService.GetRegisterPartnerByID(Token);
                var partnerDirectorsList = _mapper.Map<List<Core.Dtos.Partner.Director>>(detail.Directors);
                data.Directors = partnerDirectorsList;
                data.Email = detail.Email;
                //getdetail

                return View(data);
            }
            return View("Error");

        }

        //[RegisterDetailFilter]
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Step1(SignUpStep1 step1)
        {
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(step1, new ValidationContext(step1), validationResults, true);

            if (!isValid)
            {
                foreach (var validationResult in validationResults)
                {
                    foreach (var memberName in validationResult.MemberNames)
                    {
                        ModelState.AddModelError(memberName, validationResult.ErrorMessage);
                    }
                }
            }
            //if (ModelState.IsValid)
            //{
            //foreach (var director in step1.Directors)
            //{
            //    if (TryValidateModel(director))
            //    {
            //det data from email

            if (isValid)
            {
                if (!string.IsNullOrEmpty(step1.Email))
                {

                    var detail = await _registerService.GetRegisterPartnerByID(step1.Token);
                    if (detail != null && step1.Email.Trim().ToLower() == detail.Email.Trim().ToLower())
                    {
                        var response = await _registerService.RegisterPartnerstep1(step1);
                        if (response.StatusCode == 200)
                        {
                            return RedirectToAction("Step3", "Registration", new { Token = step1.Token });
                        }

                        ViewBag.Error = response.MsgText;
                    }

                    return View("Step1", step1);
                }
                return View("Error");
            }
            else
            {
                return View("Step1", step1);
            }

        }
        //[RegisterDetailFilter]
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Step2(string Token)
        {
            var country = await _commonddl.GetCountryddl();
            ViewBag.Countryddl = new SelectList(country, "value", "Text");
            var UtctimeZone = await _commonddl.GetUtcTimeZoneddl();
            ViewBag.UtctimeZone = new SelectList(UtctimeZone, "value", "Text");
            var currencyddl = await _commonddl.GetCurrencyddl();
            ViewBag.Currency = new SelectList(currencyddl, "value", "Text");
            //det data from email
            if (!string.IsNullOrEmpty(Token))
            {

                var detail = await _registerService.GetRegisterPartnerByID(Token);
                var data = _mapper.Map<SignUpStep2>(detail);
                data.Token = Token;


                //getdetail

                return View(data);

            }
            return View("/Error");

        }

        //[RegisterDetailFilter]
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Step2(SignUpStep2 step2)
        {
            var country = await _commonddl.GetCountryddl();
            ViewBag.Countryddl = new SelectList(country, "value", "Text");
            var UtctimeZone = await _commonddl.GetUtcTimeZoneddl();
            ViewBag.UtctimeZone = new SelectList(UtctimeZone, "value", "Text");
            var currencyddl = await _commonddl.GetCurrencyddl();
            ViewBag.Currency = new SelectList(currencyddl, "value", "Text");


            if (step2.LicensedocImg is null && step2.LicensedocImgPath is null)
            {
                ViewBag.Error = "License image is required";
                return View("Step2", step2);
            }
            else if ((step2.LicensedocImg?.Count ?? 0) + (step2.LicensedocImgPath?.Count ?? 0) <= 0)
            {
                ViewBag.Error = "License image is required";
                return View("Step2", step2);
            }

            if (ModelState.IsValid)
            {
                //det data from email
                if (!string.IsNullOrEmpty(step2.Email))
                {

                    var detail = await _registerService.GetRegisterPartnerByID(step2.Token);
                    if (detail != null && step2.Email.Trim().ToLower() == detail.Email.Trim().ToLower())
                    {
                        var response = await _registerService.RegisterPartnerstep2(step2);
                        if (response.StatusCode == 200)
                        {
                            return RedirectToAction("Step1", "Registration", new { Token = step2.Token });
                        }
                        ViewBag.Error = response.MsgText;
                    }
                    return View("Step2", step2);

                }
                return View("/Error");
            }

            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Search([FromForm]string query)
       {
            try
            {
                if (!string.IsNullOrEmpty(query) && query.Length >= 1)
                {
                var suggestions = await _registerService.AddressSearch(query);
                return Ok (suggestions);
                }
              
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error fetching autocomplete suggestions: {ex.Message}");
            }
            return Ok();

           
        }


        [AllowAnonymous]
        [HttpGet]
        public async Task<JsonResult> GetAddressDetails(string PlaceId)
        {
            try
            {
                var addressDetails = await _registerService.GetPlaceDetails(PlaceId);
                return Json(addressDetails);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Json(new { });
            }
        }

        //[RegisterDetailFilter]
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Step3(string Token)
        {
            var Document = await _commonddl.GetDocumentTypeddl();
            ViewBag.DocumentType = new SelectList(Document, "value", "Text");
            ViewBag.DocumentExpirable = Document.ToDictionary(d => d.value, d => d.IsExpirable);

            var Addressprof = await _commonddl.GetAddressProofTypeddl();
            ViewBag.Addressproof = new SelectList(Addressprof, "value", "Text");
            var currencyddl = await _commonddl.GetCurrencyddl();
            ViewBag.Currency = new SelectList(currencyddl, "value", "Text");


            //det data from email
            if (!string.IsNullOrEmpty(Token))
            {
                var detail = await _registerService.GetRegisterPartnerByID(Token);
                var data = _mapper.Map<SignUpStep3>(detail);
                data.Token = Token;
                //getdetail

                return View(data);
            }
            return View("Error");

        }
        //[RegisterDetailFilter]
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Step3(SignUpStep3 step3)
        {
            var Document = await _commonddl.GetDocumentTypeddl();
            ViewBag.DocumentType = new SelectList(Document, "value", "Text");
            var Addressprof = await _commonddl.GetAddressProofTypeddl();
            ViewBag.Addressproof = new SelectList(Addressprof, "value", "Text");
            //det data from email
            if (!string.IsNullOrEmpty(step3.Email))

            {
                if (ModelState.IsValid)
                {
                    var detail = await _registerService.GetRegisterPartnerByID(step3.Token);
                    if (detail != null && step3.Email.Trim().ToLower() == detail.Email.Trim().ToLower())
                    {
                        var response = await _registerService.RegisterPartnerstep3(step3);
                        if (response.StatusCode == 200)
                        {
                            //await _notificationService.IUDNotificationAsync($"New Partner {detail.FirstName} {detail.SurName} ({detail.OrganizationName}) registration, waiting for approval", NotificationModules.PartnerRegister, "/admin/remitpartnerregistration/remitpartnerregistrationindex", "/partner/dashboard");
                            //var Acount = await _notificationService.GetAdminNotificationCountAsync();
                            //await _hubcontext.Clients.Groups("Admin").SendAsync("updateTotalCount", Acount, $"New Partner {detail.FirstName} {detail.SurName} ({detail.OrganizationName}) registration, waiting for approval");

                            var notification = new IUDNotification()
                            {
                                Event = 'I',
                                PartnerCode = "PartnerRegister",
                                Message = $"New partner registration: {detail.FirstName} {detail.SurName} ({detail.OrganizationName}) pending approval",
                                AdminLink = "/admin/remitpartnerregistration/remitpartnerregistrationindex",
                                PartnerLink = "",
                                ModuleCode = NotificationModules.PartnerRegister,
                                UserType = "Partner",
                                UserId = "1"
                            };
                            await _notificationRepo.IUDNotificationAsync(notification);
                            var Acount = await _notificationService.GetAdminNotificationCountAsync();
                            await _hubcontext.Clients.Groups("Admin").SendAsync("updateTotalCount", Acount, $"New partner registration: {detail.FirstName} {detail.SurName} ({detail.OrganizationName}) pending approval");

                            return RedirectToAction("Success");
                        }
                        ViewBag.Error = response.MsgText;
                    }
                }
                return View("Step3", step3);

            }
            return View("Error");

        }
        //[RegisterDetailFilter]
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Success()
        {
            ViewBag.Error = null;
            return View();

        }

        private DateTime GetOtpGenerationTimeFromDatabase()
        {
            return DateTime.Now.AddSeconds(-120);
        }




    }
}