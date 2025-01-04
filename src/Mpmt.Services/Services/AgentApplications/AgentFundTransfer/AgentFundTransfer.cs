using Microsoft.AspNetCore.Http;
using Mpmt.Core.Domain.Payout;
using Mpmt.Core.Dtos.AgentFundTransfer;
using Mpmt.Core.Models.Mail;
using Mpmt.Data.Repositories.AgentFundTransfer;
using Mpmt.Data.Repositories.Agents.AgentTxn;
using Mpmt.Services.Extensions;
using Mpmt.Services.Services.Common;
using Mpmt.Services.Services.MailingService;
using Mpmts.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Mpmt.Services.Services.AgentApplications.AgentFundTransfer
{
    public class AgentFundTransfer: BaseService, IAgentFundTransfer
    {
        private readonly IMailService _mailService;
        private readonly IAgentFundTransferRepository _fundTransferRepo;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AgentFundTransfer(IMailService mailService,
            IAgentFundTransferRepository fundTransferRepo,IHttpContextAccessor httpContextAccessor)
        {
            _mailService = mailService;
            _fundTransferRepo = fundTransferRepo;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<(GetAgentFundRequestList, SprocMessage)> AgentFundRequest(AgentFundTransferDto model)
        {
            model.AgentCode = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(claims => claims.Type == "AgentCode")?.Value;
            model.LoggedInUser = _httpContextAccessor.HttpContext?.GetUserName();
            model.UserType = _httpContextAccessor.HttpContext.User.FindFirstValue("UserType");
            model.FundType = "CASH";
            model.Remarks = "Cash fund request amount";           
            var (data,status) = await _fundTransferRepo.AgentFundRequest(model);
            if(data is null)
            {
                return (data, status);
            }
            data.TotalAmount = model.TotalAmount;
            if (status.StatusCode == 200)
            {
                var mailContent = GenerateAgentFundRequestEmailBody(data);

                var mailServiceModel = new MailServiceModel
                {
                    MailFor = string.Empty,
                    MailTo = data.AgentEmail,
                    RecipientName = string.Empty,
                    MailSubject = "MyPay Money Transfer -Fund request email",
                    MailBody = mailContent,
                    MailBcc = string.Empty,
                    MailCc = "saroj.chaudhary@mypay.com.np",
                    Content = string.Empty
                };
                var emailThread = new Thread(() => _mailService.SendMail(mailServiceModel)) { IsBackground = true };
                emailThread.Start();
            }
            return (data,status);
        }

        public async Task<SprocMessage> FundRequest(AgentFundTransferDto model)
        {
            model.AgentCode = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(claims => claims.Type == "AgentCode")?.Value;
            model.LoggedInUser = _httpContextAccessor.HttpContext?.GetUserName();
            model.UserType = _httpContextAccessor.HttpContext.User.FindFirstValue("UserType");
            model.FundType = "CASH";
            model.Remarks = "Cash fund request amount";
            //decimal amt = decimal.Parse(model.TotalAmount.Replace("NPR.", "").Trim());
            //model.AmountTotal = amt;
            var data = await _fundTransferRepo.FundRequest(model);          
            return data;
        }

        public async Task<AgentFundTransferDto> GetFundTransferDetailAsync(string agentCode)
        {

            var data = await _fundTransferRepo.GetFundTransferDetailAsync(agentCode);
            return data;
        }

        private string GenerateAgentFundRequestEmailBody(GetAgentFundRequestList details)
        {
            var companyName = "MyPay Money Transfer Pvt. Ltd.";
            var companyAlias = "MyPay Money Transfer";
            var companyEmail = "support@mypaymt.com";

            string mailBody =
                $@"<p>Dear Valuable Agent {details.SuperAgentName?.ToString() ?? string.Empty},</p>
                    <p>Greetings of the day!!,</p>
                    <p>I would like to request you fund <strong> NPR. {details.TotalAmount?.ToString() ?? string.Empty}</strong> as todays Receivable.</p>
                    <p>Please do the needful to avoid the disruption in further transaction. </p>
                    <p>Thank you!!!</p>
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
