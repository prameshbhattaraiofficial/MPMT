using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp;
using Mpmt.Agent.Models.FundTransfer;
using Mpmt.Agent.Models.TransactionSearch;
using Mpmt.Core.Domain.Modules;
using Mpmt.Core.Dtos.AgentFundTransfer;
using Mpmt.Core.Dtos.Partner;
using Mpmt.Services.Services.AgentApplications.AgentFundTransfer;
using Mpmt.Services.Services.Notification;
using Newtonsoft.Json.Linq;
using NuGet.Protocol;

namespace Mpmt.Agent.Controllers
{
    public class FundTransferController : AgentBaseController
    {
        private readonly INotificationService _notificationService;
        private readonly IAgentFundTransfer _agentfundTransfer;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly INotyfService _notify;

        public FundTransferController(INotificationService notificationService,
            IAgentFundTransfer agentfundTransfer, IHttpContextAccessor httpContextAccessor, IMapper mapper, INotyfService notyfService)
        {
            _notificationService = notificationService;
            _agentfundTransfer = agentfundTransfer;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _notify = notyfService;
        }
        public async Task<IActionResult> Index()
        {
            FundTransferModel fundTransferModel = new FundTransferModel();
            string AgentCode = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(claims => claims.Type == "AgentCode")?.Value;
            var getFundTransferDetails = await _agentfundTransfer.GetFundTransferDetailAsync(AgentCode);
            var mappedData = _mapper.Map<FundTransferModel>(getFundTransferDetails);
            return View(mappedData);
        }
        [HttpPost]
        public async Task<IActionResult> Index(FundTransferModel fundRequest)
        {
            fundRequest.ReceivableAmount = fundRequest.isReceivable == "0" || fundRequest.isReceivable == null ? decimal.Parse("0.00") : fundRequest.ReceivableAmount;
            fundRequest.CommissionAmount = fundRequest.isCommission == "0" || fundRequest.isCommission == null ? decimal.Parse("0.00") : fundRequest.CommissionAmount;
            string AgentCode = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(claims => claims.Type == "AgentCode")?.Value;
            fundRequest.isCommission = string.IsNullOrEmpty(fundRequest.isCommission) ? "0" : "1";
            fundRequest.isReceivable = string.IsNullOrEmpty(fundRequest.isReceivable) ? "0" : "1";
            if (fundRequest.isCommission == "1" && fundRequest.isReceivable == "0")
            {
                if (fundRequest.CommissionAmount != fundRequest.TotalAmount)
                {
                    _notify.Error("Invalid amount!");
                    return RedirectToAction("Index", "FundTransfer");
                }
            }
            else if(fundRequest.isCommission == "0" && fundRequest.isReceivable == "1")
            {
                if (fundRequest.ReceivableAmount != fundRequest.TotalAmount)
                {
                    _notify.Error("Invalid amount!");
                    return RedirectToAction("Index", "FundTransfer");
                }
            }
            else if(fundRequest.isCommission == "1" && fundRequest.isReceivable == "1")
            {
                _notify.Error("Invalid request!");
                return RedirectToAction("Index", "FundTransfer");
                //if (fundRequest.ReceivableAmount+fundRequest.CommissionAmount != fundRequest.TotalAmount)
                //{
                //    _notify.Error("Invalid amount!");
                //    return RedirectToAction("Index", "FundTransfer");
                //}
            }
            var model = _mapper.Map<AgentFundTransferDto>(fundRequest);
            model.TotalAmount = fundRequest.TotalAmount;
            var (agentRequestFund, status) = await _agentfundTransfer.AgentFundRequest(model);
            if (status.StatusCode == 200)
            {
                await _notificationService.IUDNotificationAsync($"Agent {AgentCode} has requested for {fundRequest.TotalAmount} and requires admin approval", NotificationModules.FundRequest, "", "");
                _notify.Success(status.MsgText);
                return RedirectToAction("Index", "FundTransfer");
            }
            else
                _notify.Error(status.MsgText);
            return RedirectToAction("Index", "FundTransfer");
        }


        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> AgentFundRequest([FromBody] FundTransferModel fundRequest)
        {
            AgentFundTransferDto model = new AgentFundTransferDto();
            string AgentCode = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(claims => claims.Type == "AgentCode")?.Value;

            model.TotalAmount = fundRequest.TotalAmount;
            var (agentRequestFund, status) = await _agentfundTransfer.AgentFundRequest(model);
            if (status.StatusCode == 200)
            {
                await _notificationService.IUDNotificationAsync($"Agent {AgentCode} has requested for {fundRequest.TotalAmount} and requires admin approval", NotificationModules.FundRequest, "", "");
                return Json(new { success = true, message = status.MsgText });
            }
            else
                return Json(new { success = false, message = status.MsgText });
        }


    }
}
