using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Mpmt.Core.Common;
using Mpmt.Core.Dtos.Agent;
using Mpmt.Core.Models.Mail;
using Mpmt.Data.Repositories.Agents.AgentTxn;
using Mpmt.Services.Common;
using Mpmt.Services.Extensions;
using Mpmt.Services.Services.Common;
using Mpmt.Services.Services.MailingService;
using Mpmts.Core.Dtos;
using System.Security.Claims;

namespace Mpmt.Services.Services.AgentTxn;

public class TransactionService : BaseService, ITransactionService
{
    private readonly IAgentTransactionRepository _repo;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IConfiguration _config;
    private readonly IFileProviderService _fileProviderService;
    private readonly IMailService _mailService;

    public TransactionService(IAgentTransactionRepository repo, IHttpContextAccessor httpContextAccessor, IConfiguration configuration, IFileProviderService fileProviderService, IMailService mailService)
    {
        _repo = repo;
        _httpContextAccessor = httpContextAccessor;
        _config = configuration;
        _fileProviderService = fileProviderService;
        _mailService = mailService;
    }

    public async Task<(AgentTxnModel, SprocMessage)> checkControlNumberAsynce(string controlNumber)
    {
        var AgentCode = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(claims => claims.Type == "AgentCode")?.Value;
        var (response, statusDetail) = await _repo.checkControlNumberAsynce(controlNumber, AgentCode);
        return (response, statusDetail);
    }

    public async Task<string> GetProcessIdAsync(string agentCode, string referenceId)
    {
        string response = await _repo.GetProcessIdAsync(agentCode, referenceId);
        return response;
    }

    public async Task<(AgentPayOutReceipt, SprocMessage)> payoutTransactionByAgentAysnc(AgentTxnModel model)
    {
        AgentPayOutReceipt agentpout = new AgentPayOutReceipt();

        var isValidUploadFrontImagePath = string.Empty;

        //var isValidUploadBackImagePath = string.Empty;
        var isValidUploadImageBackPath = string.Empty;

        model.AgentCode = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(claims => claims.Type == "AgentCode")?.Value;
        model.IpAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
        model.DeviceId = _httpContextAccessor.HttpContext?.GetUserAgentDevicePlatform();
        var usertype = _httpContextAccessor.HttpContext.User.FindFirstValue("UserType");
        var username = _httpContextAccessor.HttpContext?.GetUserName();
        var referenceId = Guid.NewGuid().ToString();
        model.processId = await GetProcessId(model.AgentCode, referenceId);

        if (model.UploadImage is not null && model.UploadImage.Length > 0)
        {
            var (isValidUploadFrontImage, _) = await FileValidatorUtils.IsValidImageAsync(model.UploadImage, FileTypes.ImageFiles);
            if (!isValidUploadFrontImage)
            {
                SprocMessage sprocMessage = new SprocMessage();
                sprocMessage.MsgType = "Invalid document image!";
                return (agentpout, sprocMessage);
            }
            var folderPath = _config["Folder:TransactionPayoutDocs"];
            _fileProviderService.TryUploadFile(model.UploadImage, folderPath, out isValidUploadFrontImagePath);
            model.UploadImagePath = isValidUploadFrontImagePath;
        }

        if (model.UploadBackImage is not null && model.UploadBackImage.Length > 0)
        {
            var (isValidUploadImageBack, _) = await FileValidatorUtils.IsValidImageAsync(model.UploadBackImage, FileTypes.ImageFiles);

            if (!isValidUploadImageBack)
            {
                SprocMessage sprocMessage = new SprocMessage();
                sprocMessage.MsgType = "Invalid document image!";
                return (agentpout, sprocMessage);
            }
            var folderPath = _config["Folder:TransactionPayoutDocs"];
            _fileProviderService.TryUploadFile(model.UploadBackImage, folderPath, out isValidUploadImageBackPath);
            model.UploadBackImagePath = isValidUploadImageBackPath;
        }
        var (data, status) = await _repo.payoutTransactionByAgentAysnc(model);
        if(status.StatusCode == 200)
        {
            if(data.SendPrefundNotificationEmail)
            {
                HandleAgentPayoutNotificationBalanceEmail(data);
            }
        }
        return (data, status);
    }

    public Task<string> GetProcessId(string AgentCode, string ReferenceId)
    {
        var processId = _repo.GetProcessIdAsync(AgentCode, ReferenceId);
        return processId;
    }

    private void HandleAgentPayoutNotificationBalanceEmail(AgentPayOutReceipt details)
    {
        var mailContent = GenerateAgentPayoutNotificationBalanceEmail(details);

        var mailServiceModel = new MailServiceModel
        {
            MailFor = "prefund-balance",
            MailTo = _config["NotificationEmail:LowPrefundBalance:TO"],
            MailCc = _config["NotificationEmail:LowPrefundBalance:CC"],
            MailBcc = _config["NotificationEmail:LowPrefundBalance:BCC"],
            RecipientName = details.AgentName,
            MailSubject = "MyPay Money Transfer - Low Prefund Balance Alert",
            MailBody = mailContent
        };
        var emailThread = new Thread(() => _mailService.SendMail(mailServiceModel)) { IsBackground = true };
        emailThread.Start();
    }   

    private string GenerateAgentPayoutNotificationBalanceEmail(AgentPayOutReceipt details)
    {
        var companyName = "MyPay Money Transfer Pvt. Ltd.";
        var companyAlias = "MyPay Money Transfer";
        var companyEmail = "info@mypay.com.np";

        string mailBody =
            $@"<p>Dear User,</p>
            <p>The prefunding amount left of agent {details.AgentName} is <strong>{details.CurrentPrefundBal} NPR</strong>. Please prefund soon to avoid any payout disruption.</p>
            <p>Thank you!</p>
            <br />
            <h3><u>{companyAlias} Service Contact Information:</u></h3>
            <p>
                {companyName}<br>
                Contact No: 01-5907481/01-5907482/01-5970139<br>
                Email: {companyEmail}<br>
                Website: https://mypay.com.np/<br>
                Address: Radhe Radhe Bhaktapur, Nepal<br>
            </p>
            <p>Thank you for choosing {companyAlias} Service!</p>

        <p>CONFIDENTIALITY NOTICE: This transmittal is a confidential communication or may otherwise be privileged. If you are not the intended recipient, you are hereby notified that you have received this transmittal in error and that any review, dissemination, distribution or copying of this transmittal is strictly prohibited. If you have received this communication in error, please notify this office, and immediately delete this message and all its attachments, if any.</p>";

        return mailBody;
    }
}
