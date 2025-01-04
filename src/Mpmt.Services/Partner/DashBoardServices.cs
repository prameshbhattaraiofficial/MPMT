using AutoMapper;
using Microsoft.AspNetCore.Http;
using Mpmt.Core.Domain.Partners.SendTransactions;
using Mpmt.Core.Dtos.Partner;
using Mpmt.Core.Models.Mail;
using Mpmt.Data.Repositories.Partner;
using Mpmt.Services.Common;
using Mpmt.Services.Services.MailingService;
using Mpmts.Core.Dtos;
using System.Security.Claims;

namespace Mpmt.Services.Partner;

public class DashBoardServices : IDashBoardServices
{
    private readonly IPartnerDashBoardRepo _dashBoardRepo;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;
    private readonly IPartnerRepository _partnerRepository;
    private readonly IMailService _mailService;

    public DashBoardServices(IPartnerDashBoardRepo dashBoardRepo, IHttpContextAccessor httpContextAccessor, IMapper mapper, IMailService mailService, IPartnerRepository partnerRepository)
    {
        _dashBoardRepo = dashBoardRepo;
        _httpContextAccessor = httpContextAccessor;
        _mapper = mapper;
        _mailService = mailService;
        _partnerRepository = partnerRepository;
    }

    public async Task<PartnerDashBoard> GetPartnerDashBoard(string Partnercode)
    {
        var data = await _dashBoardRepo.GetPartnerDashBoard(Partnercode);
        return data;
    }

    public async Task<SendTransferAmountDetailDto> GetTransferAmountDetailsAsync(GetSendTransferAmountDetailRequest request)
    {
        var result = await _dashBoardRepo.GetTransferAmountDetailsAsync(request);
        return result;
    }

    public string GenrateMailBodyforOtp(string Otp)
    {
        var companyName = "MyPay Money Transfer Pvt Ltd";
        var companyEmail = "info@MPMT.com";

        string mailBody =
            $@"
                <br>
              
                <p>We are pleased to provide you with the One-Time Password (OTP) necessary to Complete Fund Transfer!!</p>
                
                <P>Your OTP is: {Otp} <p>
                  
                <p style='color=red;'>Important! Do not share your File</p>
                <p>Please remember that this OTP is valid for a single use and has a limited timeframe for use. For your security, do not share this OTP with anyone, including our support team. </P>
                <br>             
                <p>If you have any queries, Please contact us at,</p>
                <p>ATTN : Please do not reply to this email. This mailbox is not monitored and you will not receive a response.</p>
                <p>{companyName},<br>
                Radhe Radhe Bhaktapur, Nepal.<br>
                {companyEmail}</p>

                <p>Warm Regards,<br>
                {companyName}</p>


                <p>CONFIDENTIALITY NOTICE: This transmittal is a confidential communication or may otherwise be privileged. If you are not the intended recipient, you are hereby notified that you have received this transmittal in error and that any review, dissemination, distribution or copying of this transmittal is strictly prohibited. If you have received this communication in error, please notify this office, and immediately delete this message and all its attachments, if any.</p>";

        return mailBody;
    }

    public async Task SendOtpVerification()
    {
        var otp = OtpGeneration.GenerateRandom6DigitCode();

        var mailRequest = new MailRequestModel
        {
            MailFor = "transfer-fund",
            MailTo = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email),
            MailSubject = "Your One-Time Password (OTP) for Transfer Fund",
            RecipientName = "",
            Content = GenrateMailBodyforOtp(otp)

        };
        var mailServiceModel = await _mailService.EmailSettings(mailRequest);

        Thread email = new(delegate ()
        {
            _mailService.SendMail(mailServiceModel);
        });
        email.Start();

        var addtoken = new TokenVerification
        {
            UserId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue("Id")),
            PartnerCode = _httpContextAccessor.HttpContext.User.FindFirstValue("PartnerCode"),
            UserName = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name).Value,
            Email = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email),
            VerificationCode = otp,
            VerificationType = "Email",
            OtpVerificationFor = "Fund-Transfer",
            SendToEmail = true,
            SendToMobile = false,
            IsConsumed = false,
            ExpiredDate = DateTime.Now.AddMinutes(2)
        };

        //var response = await _dashBoardRepo.PartnerDashboardOTPAsync(addtoken);
        var response = await _partnerRepository.AddLoginOtpAsync(addtoken);
        //return new RedirectResult("Partner/Dashboard/TokenVerification");
    }

    public async Task<SprocMessage> SendTransferAmount(GetSendTransferAmountDetailRequest request)
    {
        var partnerCode = _httpContextAccessor.HttpContext.User.FindFirstValue("PartnerCode");
        request.PartnerCode = partnerCode;
        request.LoggedInUser = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
        request.UserType = _httpContextAccessor.HttpContext.User.FindFirstValue("UserType");
        var response = await _dashBoardRepo.SendTransferAmount(request);
        return response;
    }

    public async Task<SprocMessage> CheckWalletBalance(GetSendTransferAmountDetailRequest request)
    {
        var partnerCode = _httpContextAccessor.HttpContext.User.FindFirstValue("PartnerCode");
        request.PartnerCode = partnerCode;
        var response = await _dashBoardRepo.CheckWalletBalance(request);
        return response;
    }

    public async Task<IEnumerable<FrequencyWiseTransaction>> GetTransactionDataFrequencyWise(string frequency)
    {
        var partnerCode = _httpContextAccessor.HttpContext.User.FindFirstValue("PartnerCode");
        var result = await _dashBoardRepo.GetTransactionDataFrequencyWise(frequency, partnerCode);
        return result;
    }

    public async Task<IEnumerable<DashboardTransactionStatus>> GetTransactionStatusDashboard(string frequency)
    {
        var partnerCode = _httpContextAccessor.HttpContext.User.FindFirstValue("PartnerCode");
        var result = await _dashBoardRepo.GetTransactionStatusDashboard(frequency, partnerCode);
        return result;
    }

    public async Task<IEnumerable<DashboardPartnerSender>> GetPartnerSenderDashboard(string frequency, string filterBy, string orderBy)
    {
        var partnerCode = _httpContextAccessor.HttpContext.User.FindFirstValue("PartnerCode");
        var result = await _dashBoardRepo.GetPartnerSenderDashboard(partnerCode, frequency, filterBy, orderBy);
        return result;
    }

    public async Task<IEnumerable<DashboardWalletBalance>> GetPartnerBalanceDashboard(string filterBy, string orderBy)
    {
        var partnerCode = _httpContextAccessor.HttpContext.User.FindFirstValue("PartnerCode");
        var result = await _dashBoardRepo.GetPartnerBalanceDashboard(partnerCode, filterBy, orderBy);
        return result;
    }
}