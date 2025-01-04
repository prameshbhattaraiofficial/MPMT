using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using DocumentFormat.OpenXml.Bibliography;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Mpmt.Core.Domain.Partners.SendTransactions;
using Mpmt.Core.Dtos.AdminDashboard;
using Mpmt.Core.Dtos.DropDown;
using Mpmt.Core.Dtos.Notification;
using Mpmt.Core.Dtos.Partner;
using Mpmt.Core.Dtos.PartnerReport;
using Mpmt.Core.ViewModel.ChangePassword;
using Mpmt.Services.Partner;
using Mpmt.Services.Partner.IService;
using Mpmt.Services.Services.Common;
using Mpmt.Services.Services.Notification;
using Mpmt.Services.Services.RoleMenuPermission;
using Mpmt.Web.Areas.Partner.Models.TransferAmount;
using Mpmt.Web.Common;
using Mpmt.Web.Filter;
using System.Globalization;
using System.Net;
using System.Security.Claims;

namespace Mpmt.Web.Areas.Partner.Controllers;

[PartnerAuthorization]
public class DashboardController : BasePartnerController
{
    private readonly INotificationService _notificationdetail;
    private readonly IDashBoardServices _dashBoardServices;
    private readonly ICommonddlServices _commonddlServices;
    private readonly IMapper _mapper;
    private readonly IRMPService _rMPService;
    private readonly INotificationService _notificationService;
    private readonly INotyfService _notyfService;
    private readonly IPartnerCredentialsService _partnerCredentialsService;
    private readonly IPartnerRegistrationService _partnerRegistrationService;
    private readonly IPartnerSendTxnsService _partnerSendTxnsService;
    private readonly ClaimsPrincipal _loggedInUser;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DashboardController(INotificationService notificationdetail, IDashBoardServices dashBoardServices, IPartnerCredentialsService partnerCredentialsService, ICommonddlServices commonddlServices, IPartnerRegistrationService partnerRegistrationService, IHttpContextAccessor httpContextAccessor, IMapper mapper, IPartnerSendTxnsService partnerSendTxnsService, INotyfService notyfService, IRMPService rMPService)
    {
        _notificationdetail = notificationdetail;
        _dashBoardServices = dashBoardServices;
        _partnerCredentialsService = partnerCredentialsService;
        _commonddlServices = commonddlServices;
        _partnerRegistrationService = partnerRegistrationService;
        _httpContextAccessor = httpContextAccessor;
        _loggedInUser = _httpContextAccessor.HttpContext?.User;
        _mapper = mapper;
        _partnerSendTxnsService = partnerSendTxnsService;
        _notyfService = notyfService;
        _rMPService = rMPService;
    }

    [HttpGet]
    public async Task<IActionResult> GetPartnernotificationcount()
    {
        var data = await _notificationdetail.GetPartnerNotificationCountAsync();
        return Ok(data);
    }

    public async Task<IActionResult> Index()
    {
        List<string> frequency = new();
        List<string> labels = new();
        List<double> transactionData = new();
        List<double> volumeData = new();

        List<string> labelsPieChart = new();
        List<int> dataPieChart = new();

        var actionsTransaction = await _rMPService.GetPartnerActionPermissionList("SendTransactions");
        ViewBag.actions = actionsTransaction;

        var partnercode = User.Claims.FirstOrDefault(x => x.Type == "PartnerCode").Value;
        var sourcecurrency = await _commonddlServices.GetPartnerSourceCurrencyddl(partnercode);
        var destinationcurrency = await _commonddlServices.GetTransferModuleDestinationCurrencyddl(partnercode);

        var transferDestinationCurrency = destinationcurrency
            .Select(item => new Commonddl
            {
                value = $"{item.value}-{item.lookup}",
                Text = item.Text
            })
            .ToList();

        ViewBag.SourceCurrency = new SelectList(sourcecurrency.Where(x => x.value != "NPR"), "value", "Text");
        ViewBag.DestinationCurrency = new SelectList(transferDestinationCurrency, "value", "Text");

        var Is2FAAuthenticated = _loggedInUser.Claims?.FirstOrDefault(x => x.Type == "Is2FAAuthenticated").Value;
        ViewBag.FA = Is2FAAuthenticated;

        var data = await _dashBoardServices.GetPartnerDashBoard(partnercode);
        //var chartData = await _dashBoardServices.GetTransactionDataFrequencyWise("1Y");

        // Loop through the IEnumerable and extract desired data
        foreach (var item in data.frequencyWiseTransactions)
        {
            frequency.Add(item.Frequency.ToString()); // Convert DateTime to string
            labels.Add(item.FrequencyText);
            transactionData.Add(item.TotalTrans);
            volumeData.Add(item.Volume);
        }

        foreach (var item in data.dashboardTransactionStatus)
        {
            labelsPieChart.Add(item.StatusName);
            dataPieChart.Add(item.TotalTrans);
        }

        data.Frequency = frequency;
        data.Labels = labels;
        data.TransactionData = transactionData;
        data.VolumeData = volumeData;

        data.PieChartLabels = labelsPieChart;
        data.PieChartData = dataPieChart;

        return View(data);
    }

    [HttpPost]
    public async Task<IActionResult> Index(string filterOption = "1Y")
    {
        List<string> frequency = new();
        List<string> labels = new();
        List<double> transactionData = new();
        List<double> volumeData = new();
        var chartData = await _dashBoardServices.GetTransactionDataFrequencyWise(filterOption);
        foreach (var item in chartData)
        {
            frequency.Add(item.Frequency.ToString()); // Convert DateTime to string
            labels.Add(item.FrequencyText);
            transactionData.Add(item.TotalTrans);
            volumeData.Add(item.Volume);
        }
        return Json(new
        {
            frequency,
            labels,
            transactionData,
            volumeData
        });
    }

    [HttpGet]
    public async Task<IActionResult> GetTopSenderList(string period)
    {
        var data = await _dashBoardServices.GetPartnerSenderDashboard(period, null, null);
        var dashboard = new PartnerDashBoard {
            dashboardPartnerSenders = data.ToList()
        };
        return PartialView("_PartnerSendersTable",dashboard);
    }

    [HttpPost]
    public async Task<IActionResult> SortPartnerBalanceList(string columnName, string sortOrder)
    {
        var data = await _dashBoardServices.GetPartnerBalanceDashboard(columnName, sortOrder);
        var dashboard = new PartnerDashBoard
        {
            dashboardWalletBalances = data.ToList()
        };
        return Json(new { partnerBalance = data });
    }

    [HttpPost]
    public async Task<IActionResult> SortTopPartnerSenderList(string columnName, string sortOrder, string selectedPeriod)
    {
        if (string.IsNullOrEmpty(selectedPeriod))
        {
            selectedPeriod = "DAILY";
        }
        var data = await _dashBoardServices.GetPartnerSenderDashboard(selectedPeriod, columnName, sortOrder);
        var dashboard = new PartnerDashBoard
        {
            dashboardPartnerSenders = data.ToList()
        };
        return Json(new { partnerSender = data });  
    }

    [HttpPost]
    public IActionResult TransactionReportsRedirection(string StartDate)
    {
        DateTime startDate;
        RemitTxnReportFilter filter;

        // Check if StartDate includes time (indicating full date format)
        if (DateTime.TryParseExact(StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out startDate))
        {
            // If parsing is successful, StartDate is already in complete format
            filter = new RemitTxnReportFilter
            {
                StartDate = startDate,
                EndDate = startDate
            };
        }
        else if (DateTime.TryParseExact(StartDate, "yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out startDate))
        {
            startDate = Convert.ToDateTime(StartDate + "-01-01");
            filter = new RemitTxnReportFilter
            {
                StartDate = startDate,
                EndDate = Convert.ToDateTime(StartDate + "-12-31")
            };
        }
        else
        {
            // If parsing still fails, assume incomplete format (yyyy-MM)
            //startDate = new DateTime(int.Parse(StartDate.Substring(0, 4)), int.Parse(StartDate.Substring(5, 2)), 1);
            startDate = Convert.ToDateTime(StartDate + "-01");
            filter = new RemitTxnReportFilter
            {
                StartDate = startDate,
                EndDate = startDate.AddMonths(1).AddDays(-1)
            };
        }
        TempData["StartDate"] = filter.StartDate;
        TempData["EndDate"] = filter.EndDate;

        var redirectUrl = Url.Action("TransactionReportsIndex", "Reports");
        return Json(new { redirectUrl });
    }

    [HttpPost]
    public async Task<JsonResult> TransactionStatus(string timeframe)
    {
        List<string> PieChartLabels = new();
        List<int> PieChartData = new();
        var data = await _dashBoardServices.GetTransactionStatusDashboard(timeframe);
        foreach (var item in data)
        {
            PieChartLabels.Add(item.StatusName);
            PieChartData.Add(item.TotalTrans);
        }
        return Json(new
        {
            labels = PieChartLabels,
            data = PieChartData
        });
    }

    [HttpGet]
    public IActionResult TermsAndConditions()
    {
        return PartialView("_TermsAndConditions");
    }

    [HttpGet]
    public async Task<IActionResult> Getnotification()
    {
        var data = await _notificationdetail.GetPartnerNotificationAsync();
        return Ok(data);
    }

    [HttpPost]
    public async Task<IActionResult> MarkAsRead(string id)
    {
        Guid data = Guid.Parse(id);
        await _notificationdetail.MarkPartnerNotificationAsRead(data);
        return Ok();
    }

    public async Task<IActionResult> AddBulkSender()
    {
        return await Task.FromResult(PartialView("_addBulkSender"));
    }

    [HttpGet]
    public async Task<IActionResult> Notifications()
    {
        NotificationsFilter filter = new NotificationsFilter()
        {
            UserType = "PARTNER",
            PartnerCode = User.Claims.FirstOrDefault(x => x.Type == "PartnerCode").Value
        };
        var result = await _notificationdetail.GetNotificationAsync(filter);

        if (WebHelper.IsAjaxRequest(Request))
            return await Task.FromResult(PartialView("_Notifications", result));
        return await Task.FromResult(View(result));
    }

    [HttpPost]
    public async Task<IActionResult> GetTransferAmount(decimal sourceAmount = 0, string sourceCurrency = "", string destinationCurrency = "")
    {
        var transferAmount = new GetConvertedTransferAmount();

        if (!string.IsNullOrEmpty(destinationCurrency))
        {
            transferAmount.SourceAmount = sourceAmount;
            transferAmount.SourceCurrency = sourceCurrency;
            transferAmount.DestinationCurrency = destinationCurrency.Split("-")[0];

            var mappedData = _mapper.Map<GetSendTransferAmountDetailRequest>(transferAmount);
            var transferAmountDetails = await _dashBoardServices.GetTransferAmountDetailsAsync(mappedData);
            var result = transferAmountDetails?.DestinationAmount ?? 0.0m;

            var data = Newtonsoft.Json.JsonConvert.SerializeObject(new { SC = transferAmountDetails?.SourceCurrency ?? "", DC = transferAmountDetails?.DestinationCurrency ?? "", amount = result, CR = transferAmountDetails?.ExchangeRate ?? 0.0m });
            return Json(data);
        }
        return Json(null);
    }

    [HttpGet]
    public async Task<IActionResult> SendTransferAmount(GetConvertedTransferAmount amount)
    {
        if (amount.SourceCurrency == null || amount.DestinationCurrency == null)
        {
            return BadRequest();
        }
        else
        {
            var mappedData = _mapper.Map<GetSendTransferAmountDetailRequest>(amount);
            var responseStatus = await _dashBoardServices.CheckWalletBalance(mappedData);
            if (responseStatus.StatusCode == 200)
            {
                await _dashBoardServices.SendOtpVerification();
                return await Task.FromResult(PartialView(amount));
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { success = false, error = responseStatus.MsgText });
            }
        }
    }

    [HttpPost]
    public async Task<IActionResult> SendTransferAmount(GetConvertedTransferAmount amount, string otp)
    {
        if (string.IsNullOrEmpty(amount.OTP))
        {
            ViewBag.Error = "Please Enter OTP";
            ModelState.AddModelError("", "Please Enter OTP");
            Response.StatusCode = (int)(HttpStatusCode.BadRequest);
            return PartialView(amount);
        }

        var partnerCode = User.Claims.FirstOrDefault(x => x.Type == "PartnerCode").Value;
        var UserName = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name).Value;
        var data = await _partnerRegistrationService.GetOtpBypartnerCodeAsync(partnerCode, UserName, "Fund-Transfer");
        if (data == null) return RedirectToAction("Index");
        if (data.ExpiredDate < DateTime.UtcNow)
        {
            ViewBag.Error = "Otp Expired";
            ModelState.AddModelError("", "Otp Expired");
            Response.StatusCode = (int)(HttpStatusCode.BadRequest);
            return PartialView(amount);
        }
        if (data.OtpAttemptCount > 5)
        {
            ViewBag.Error = "Otp Attempt Count Exceeded";
            ModelState.AddModelError("", "Otp Attempt Count Exceeded");
            Response.StatusCode = (int)(HttpStatusCode.BadRequest);
            return PartialView(amount);
        }
        if (data.VerificationCode != amount.OTP)
        {
            ViewBag.Error = "Invalid OTP";
            ModelState.AddModelError("", "Invalid OTP");
            Response.StatusCode = (int)(HttpStatusCode.BadRequest);
            return PartialView(amount);
        }

        string[] values = amount.DestinationCurrency.Split('-');
        var sendAmount = new GetConvertedTransferAmount
        {
            SourceCurrency = amount.SourceCurrency,
            DestinationCurrency = values[0],
            AccountType = values[1],
            SourceAmount = amount.SourceAmount,
            Remarks = amount.Remarks,
            DestinationAmount = amount.DestinationAmount
        };
        var mappedData = _mapper.Map<GetSendTransferAmountDetailRequest>(sendAmount);
        var responseStatus = await _dashBoardServices.SendTransferAmount(mappedData);
        if (responseStatus.StatusCode == 200)
        {
            _notyfService.Success(responseStatus.MsgText);
            return Ok();
        }
        else
        {
            Response.StatusCode = (int)(HttpStatusCode.BadRequest);
            ViewBag.Error = responseStatus.MsgText;
            Response.StatusCode = (int)(HttpStatusCode.BadRequest);
            return PartialView(amount);
        }
    }

    #region changePartnerPassword

    [HttpGet]
    public async Task<IActionResult> ChangePassword()
    {
        return PartialView();
    }

    [HttpPost]
    public async Task<IActionResult> ChangePassword(PartnerChangePasswordVM changepassword)
    {
        if (!ModelState.IsValid)
        {
            return PartialView(changepassword);
        }
        var responseStatus = await _partnerCredentialsService.ChangePartnerPassword(changepassword);
        if (responseStatus.StatusCode == 200)
        {
            _notyfService.Success(responseStatus.MsgText);
            return Ok();
        }
        else
        {
            Response.StatusCode = (int)(HttpStatusCode.BadRequest);
            ViewBag.Error = responseStatus.MsgText;
            return PartialView(changepassword);
        }
    }

    #endregion changePartnerPassword

    #region Bind-ResetKey,Resetpassword

    [HttpGet]
    public async Task<string> ChangeApiKey([FromQuery] string CredentailsId)
    {
        var partnerCode = User.Claims.FirstOrDefault(x => x.Type == "PartnerCode").Value;
        var (status, Apikey) = await _partnerCredentialsService.RegenerateApiKeyAsync(partnerCode, CredentailsId);
        return Apikey;
    }

    [HttpGet]
    public async Task<string> ChangeApipassword([FromQuery] string CredentailsId)
    {
        var partnerCode = User.Claims.FirstOrDefault(x => x.Type == "PartnerCode").Value;
        var (status, Apipasswrd) = await _partnerCredentialsService.RegenerateApiPasswordAsync(partnerCode, CredentailsId);
        return Apipasswrd;
    }

    #endregion Bind-ResetKey,Resetpassword
}