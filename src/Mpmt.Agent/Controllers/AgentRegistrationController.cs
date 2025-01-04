using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Mpmt.Core.Dtos.CashAgent;
using Mpmt.Core.Models.Mail;
using Mpmt.Services.CashAgents;
using Mpmt.Services.Common;
using Mpmt.Services.Services.Common;
using Mpmt.Services.Services.MailingService;
using Mpmt.Services.Services.Receipt;
using Mpmt.Services.Services.Sms;

namespace Mpmt.Agent.Controllers;

public class AgentRegistrationController : AgentBaseController
{
    private readonly ICommonddlServices _commonddl;
    private readonly IMailService _mailService;
    private readonly IMapper _mapper;
    private readonly ISmsService _smsService;
    private readonly ICashAgentUserService _cashAgentUserService;
    private readonly IAgentRegistrationService _agentRegistrationService;
    private readonly IReceiptGenerationService _receiptGenerationService;

    public AgentRegistrationController(ICommonddlServices commonddl, IAgentRegistrationService agentRegistrationService, IReceiptGenerationService receiptGenerationService, IMailService mailService, ICashAgentUserService cashAgentUserService, ISmsService smsService)
    {
        _commonddl = commonddl;
        _agentRegistrationService = agentRegistrationService;
        _receiptGenerationService = receiptGenerationService;
        _mailService = mailService;
        _cashAgentUserService = cashAgentUserService;
        _smsService = smsService;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> SignUp()
    {
        var CallingCode = await _commonddl.GetCallingCodeddl();
        ViewBag.CallingCode = new SelectList(CallingCode, "Text", "lookup");
        return View();
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> SignUp(SignUpAgent signUpAgent)
    {
        var CallingCode = await _commonddl.GetCallingCodeddl();
        ViewBag.CallingCode = new SelectList(CallingCode, "Text", "lookup");
        if (ModelState.IsValid)
        {
            var Otp = OtpGeneration.GenerateRandom6DigitCode();
            signUpAgent.Otp = Otp;
            var message = $"MyPay Money Transfer One-Time Password (OTP) to complete registration is: {Otp}. DO NOT SHARE your Password/OTP with anyone.";
            var response = await _agentRegistrationService.RegisterAgent(signUpAgent);
            var otpValid = new OtpValidationAgent();
            otpValid.Email = signUpAgent.Email;
            otpValid.PhoneNumber = signUpAgent.PhoneNumber;
            if (response.StatusCode == 200)
            {
                var Content = _receiptGenerationService.GenrateMailBodyforOtp(Otp);
                var mailRequest = new MailRequestModel
                {
                    MailFor = "confirm-email",
                    MailTo = signUpAgent.Email,
                    MailSubject = "Your One-Time Password (OTP) for Registration",
                    RecipientName = signUpAgent.LastName,
                    Content = Content
                };
                var mailServiceModel = await _mailService.EmailSettings(mailRequest);
                Thread email = new(delegate ()
                {
                    _mailService.SendMail(mailServiceModel);
                });
                await _smsService.SendAsync(message, signUpAgent.PhoneNumber);
                email.Start();
                return View("OTPValidation", otpValid);
            }
            ViewBag.Error = response.MsgText;
        }
        return View(signUpAgent);
    }

    [AcceptVerbs("GET", "POST")]
    [AllowAnonymous]
    public async Task<IActionResult> VerifyContactNumber(string PhoneNumber)
    {
        if (!await _cashAgentUserService.VerifyContactNumber(PhoneNumber))
        {
            return Json($"Contact Number is already in use.");
        }
        return Json(true);
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> VerifyOtp(OtpValidationAgent validate)
    {
        var response = await _agentRegistrationService.ValidateOtp(validate);
        if (response.StatusCode == 200)
        {
            var detail = await _agentRegistrationService.GetRegisterAgent(validate);
            return RedirectToAction("Step2", "AgentRegistration", new { Token = detail.Id });
        }
        else
        {
            ModelState.AddModelError("Otp", response.MsgText);
            return View("OTPValidation", validate); ;
        }
    }

    [AcceptVerbs("GET", "POST")]
    [AllowAnonymous]
    public async Task<IActionResult> VerifyUserName(string userName)
    {
        if (!await _cashAgentUserService.VerifyUserName(userName))
        {
            return Json($"Username is already in use.");
        }
        return Json(true);
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> Step2(string token)
    {
        var Document = await _commonddl.GetDocumentTypeddl();
        var districtList = await _commonddl.GetAllDistrictddl();
        ViewBag.District = new SelectList(districtList, "value", "Text");
        ViewBag.DocumentType = new SelectList(Document, "value", "Text");
        if (!string.IsNullOrEmpty(token))
        {
            var detail = await _agentRegistrationService.GetRegisterAgentByID(token);
            var data = new SignUpAgentStep2
            {
                Email = detail.Email,
                PhoneNumber = detail.PhoneNumber,
                Token = token
            };
            return View(data);
        }
        return View("Error");
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Step2(SignUpAgentStep2 signUpAgent)
    {
        var Document = await _commonddl.GetDocumentTypeddl();
        var districtList = await _commonddl.GetAllDistrictddl();
        ViewBag.District = new SelectList(districtList, "value", "Text");
        ViewBag.DocumentType = new SelectList(Document, "value", "Text");
        if (!string.IsNullOrEmpty(signUpAgent.Email))
        {
            if (ModelState.IsValid)
            {
                var detail = await _agentRegistrationService.GetRegisterAgentByID(signUpAgent.Token);
                if (detail != null && signUpAgent.Email.Trim().ToLower() == detail.Email.Trim().ToLower())
                {
                    var response = await _agentRegistrationService.RegisterAgentStep2(signUpAgent);
                    if (response.StatusCode == 200)
                    {
                        return RedirectToAction("Success");
                    }
                    ViewBag.Error = response.MsgText;
                }
            }
            return View("Step2", signUpAgent);
        }
        return View("Error");
    }

    [AllowAnonymous]
    [HttpGet]
    public IActionResult Success()
    {
        ViewBag.Error = null;
        return View();
    }
}