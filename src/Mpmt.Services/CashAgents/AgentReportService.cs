using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Mpmt.Core.Common;
using Mpmt.Core.Dtos.AgentFundTransfer;
using Mpmt.Core.Dtos.AgentReport;
using Mpmt.Core.Dtos.CashAgent;
using Mpmt.Core.Dtos.Paging;
using Mpmt.Core.Models.CashAgent;
using Mpmt.Core.Models.Mail;
using Mpmt.Data.Repositories.CashAgent;
using Mpmt.Services.Common;
using Mpmt.Services.Services.MailingService;
using Mpmts.Core.Dtos;
using System.Security.Claims;

namespace Mpmt.Services.CashAgents;

public class AgentReportService : IAgentReportService
{
    private readonly IAgentReportRepository _agentReportRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ClaimsPrincipal _loggedInUser;
    private readonly IConfiguration _config;
    private readonly IMailService _mailService;
    private readonly IFileProviderService _fileProviderService;

    public AgentReportService(IAgentReportRepository agentReportRepository, IHttpContextAccessor httpContextAccessor, IConfiguration configuration, IFileProviderService fileProviderService, IMailService mailService)
    {
        _agentReportRepository = agentReportRepository;
        _httpContextAccessor = httpContextAccessor;
        _loggedInUser = _httpContextAccessor.HttpContext?.User;
        _config = configuration;
        _fileProviderService = fileProviderService;
        _mailService = mailService;
    }

    public async Task<PagedList<AgentCommissionTransactionReport>> GetAgentCommissionTxnReportAdminAsync(AgentCommissionFilter txnFilter)
    {
        txnFilter.LoggedInUser = _loggedInUser.FindFirstValue(ClaimTypes.Name);
        txnFilter.AgentCode = "ADMIN";
        txnFilter.UserType = _loggedInUser.FindFirstValue("UserType");
        var data = await _agentReportRepository.GetAgentCommissionTxnReportAsync(txnFilter);
        return data;
    }

    public async Task<PagedList<AgentCommissionTransactionReport>> GetAgentCommissionTxnReportAsync(AgentCommissionFilter txnFilter)
    {
        txnFilter.LoggedInUser = _loggedInUser.FindFirstValue(ClaimTypes.Name);
        //txnFilter.AgentCode = _loggedInUser.FindFirstValue("AgentCode");
        txnFilter.AgentCode = txnFilter.AgentCode ?? _loggedInUser.FindFirstValue("AgentCode");
        txnFilter.UserType = _loggedInUser.FindFirstValue("UserType");
        var data = await _agentReportRepository.GetAgentCommissionTxnReportAsync(txnFilter);
        return data;
    }


    public async Task<PagedList<FundRequest>> GetAgentFundRequestReportAsync(AgentFundRequestFilter txnFilter)
    {
        txnFilter.AgentCode = "ADMIN";
        var data = await _agentReportRepository.GetFundReqListForAdmin(txnFilter);
        return data;
    }
    //agent 

    public async Task<PagedList<FundRequest>> GetAgentFundRequestDetailsAsync(AgentFundRequestFilter txnFilter)
    {
        txnFilter.AgentCode = _loggedInUser.FindFirstValue("AgentCode");
        var data = await _agentReportRepository.GetAgentFundRequestDetailsAsync(txnFilter);
        return data;
    }

    public async Task<SprocMessage> ApproveRejectAgentFundRequestAysnc(ApproveRejectFundTransferByAdmin model)
    {
        var isValidUploadImagePath = string.Empty;
        model.OperationMode = "APPROVE";

        if (model.VoucherImagePath is not null && model.VoucherImagePath.Length > 0)
        {
            var (isValidVoucherImage, _) = await FileValidatorUtils.IsValidImageAsync(model.VoucherImagePath, FileTypes.ImageFiles);
            if (!isValidVoucherImage)
            {
                SprocMessage sprocMessage = new SprocMessage();
                sprocMessage.MsgType = "Invalid document image!";
                return sprocMessage;
            }
            var folderPath = _config["Folder:TransactionPayoutDocs"];
            _fileProviderService.TryUploadFile(model.VoucherImagePath, folderPath, out isValidUploadImagePath);
            model.VoucherImage = isValidUploadImagePath;
        }

        var (_, sproc) = await _agentReportRepository.ApproveRejectAgentFundRequestAysnc(model);
        return sproc;
    }


    public async Task<PagedList<AgentSettlementReport>> GetAgentSettlementReportAsync(AgentSettlementFilter txnFilter)
    {
        txnFilter.LoggedInUser = _loggedInUser.FindFirstValue(ClaimTypes.Name);
        //txnFilter.AgentCode = _loggedInUser.FindFirstValue("AgentCode");
        txnFilter.AgentCode = txnFilter.AgentCode ?? _loggedInUser.FindFirstValue("AgentCode");
        txnFilter.UserType = _loggedInUser.FindFirstValue("UserType");
        var data = await _agentReportRepository.GetAgentSettlementReportAsync(txnFilter);
        return data;
    }

    public async Task<AgentFundApproveRejectModel> GetDetailsRequestFundAsync(AgentFundApproveRejectModel model)
    {
        var data = await _agentReportRepository.GetDetailsRequestFundAsync(model);
        return data;
    }

    public async Task<IEnumerable<AgentUnsettledAmount>> GetAgentUnsettledAmountSummaryList()
    {
        var data = await _agentReportRepository.GetAgentUnsettledAmountSummaryList();
        return data;
    }

    public async Task<SprocMessage> ReviewAgentFundRequestAysnc(ApproveRejectFundTransferByAdmin model)
    {
        model.OperationMode = "REVIEWED";
        var (data, sproc) = await _agentReportRepository.ApproveRejectAgentFundRequestAysnc(model);
        if (data.IsPrefunding)
        {
            HandleAgentFundRequestReviewedEmail(data);
        }
        return sproc;
    }
        
    private void HandleAgentFundRequestReviewedEmail(ApproveRejectReviewModel details)
    {
        var mailContent = GenerateAgentFundRequestReviewedEmail(details);

        var mailServiceModel = new MailServiceModel
        {
            MailFor = "fund-request-reviewed",
            MailTo = details.AgentEmail,
            MailCc = details.MpmtAdminEmail,
            RecipientName = details.AgentName,
            MailSubject = "MyPay Money Transfer - Fund request reviewed",
            MailBody = mailContent
        };
        var emailThread = new Thread(() => _mailService.SendMail(mailServiceModel)) { IsBackground = true };
        emailThread.Start();
    }
        
    private string GenerateAgentFundRequestReviewedEmail(ApproveRejectReviewModel details)
    {
        var companyName = "MyPay Money Transfer Pvt. Ltd.";
        var companyAlias = "MyPay Money Transfer";
        var companyEmail = "info@mypay.com.np";

        string mailBody =
            $@"<p>Dear {details.AgentName}</p>
        <p>Your fund request has been reviewed succesfully!</p>
        <p>Thank you!</p>
        <br />
        <h3><u>{companyAlias} Service Contact Information:</u></h3>
        <p>
            {companyName}<br>
            Contact No.: 01-5907481/01-5907482/01-5970139<br>
            Email: {companyEmail}<br>
            Website: https://mypay.com.np/<br>
            Address: Radhe Radhe Bhaktapur, Nepal<br>
        </p>
        <p>Thank you for choosing {companyAlias} Service!</p>

    <p>CONFIDENTIALITY NOTICE: This transmittal is a confidential communication or may otherwise be privileged. If you are not the intended recipient, you are hereby notified that you have received this transmittal in error and that any review, dissemination, distribution or copying of this transmittal is strictly prohibited. If you have received this communication in error, please notify this office, and immediately delete this message and all its attachments, if any.</p>";

        return mailBody;
    }

    public async Task<PagedList<AgentCommissionTransactionReportDetail>> GetAgentCommissionTxnReportDetailAdminAsync(AgentCommissionFilter txnFilter)
    {
        txnFilter.LoggedInUser = _loggedInUser.FindFirstValue(ClaimTypes.Name);
        txnFilter.AgentCode = "ADMIN";
        txnFilter.UserType = _loggedInUser.FindFirstValue("UserType");
        var data = await _agentReportRepository.GetAgentCommissionTxnReportDetailAsync(txnFilter);
        return data;
    }
}
