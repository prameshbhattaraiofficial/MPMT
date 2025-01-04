using AspNetCoreHero.ToastNotification.Abstractions;
using Google.Authenticator;
using Microsoft.AspNetCore.Mvc;
using Mpmt.Core.Dtos.AdminDashboard;
using Mpmt.Core.Dtos.Notification;
using Mpmt.Core.Dtos.Partner;
using Mpmt.Core.Dtos.PartnerReport;
using Mpmt.Core.ViewModel.ChangePassword;
using Mpmt.Services.Partner;
using Mpmt.Services.Services.AdminDashBoard;
using Mpmt.Services.Services.Common;
using Mpmt.Services.Services.Notification;
using Mpmt.Services.Users;
using Mpmt.Web.Common;
using Mpmt.Web.Filter;
using System.Globalization;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace Mpmt.Web.Areas.Admin.Controllers;

[AdminAuthorization]
public class AdminDashboardController : BaseAdminController
{
    private readonly IAdminDashBoardServices _dashBoardServices;
    private readonly INotificationService _notificationdetail;
    private readonly INotyfService _notyfService;
    private readonly ICommonddlServices _commonddlServices;
    private readonly INotificationService _notificationService;
    private readonly ITransactionServices _transactionServices;
    private readonly IUserRegistrationService _userRegistrationService;
    private readonly ClaimsPrincipal _loggedInUser;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AdminDashboardController(INotificationService notificationdetail,
        INotyfService notyfService,
        ICommonddlServices commonddlServices, IAdminDashBoardServices dashBoardServices, IUserRegistrationService userRegistrationService, IHttpContextAccessor httpContextAccessor, ITransactionServices transactionServices)
    {
        _notificationdetail = notificationdetail;
        _notyfService = notyfService;
        _commonddlServices = commonddlServices;
        _dashBoardServices = dashBoardServices;
        _userRegistrationService = userRegistrationService;
        _httpContextAccessor = httpContextAccessor;
        _loggedInUser = _httpContextAccessor.HttpContext?.User;
        _transactionServices = transactionServices;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        List<string> frequency = new();
        List<string> labels = new();
        List<double> transactionData = new();
        List<double> volumeData = new();

        List<string> labelsPieChart = new();
        List<int> dataPieChart = new();

        var data = await _dashBoardServices.GetAdminDashBoard();

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
    public async Task<IActionResult> GetTopPartnerList(string period)
    {
        var data = await _dashBoardServices.GetTopPartnerDashboard(period, null, null);
        var dashboard = new AdminDashboardDetails
        {
            dashboardTopPartner = data.ToList()
        };
        return PartialView("_PartnersDashboardTable", dashboard);
    }

    [HttpPost]
    public async Task<IActionResult> SortTopPartnerList(string columnName, string sortOrder, string selectedPeriod)
    {
        if (string.IsNullOrEmpty(selectedPeriod))
        {   
            selectedPeriod = "DAILY";
        }
        var data = await _dashBoardServices.GetTopPartnerDashboard(selectedPeriod, columnName, sortOrder);
        var dashboard = new AdminDashboardDetails
        {
            dashboardTopPartner = data.ToList()
        };
        return Json(new { partnerData = data });
    }

    [HttpGet]
    public async Task<IActionResult> GetTopAgentList(string period)
    {
        var data = await _dashBoardServices.GetTopAgentDashboard(period, null, null);
        var dashboard = new AdminDashboardDetails
        {
            dashboardTopAgent = data.ToList()
        };
        return PartialView("_AgentsDashboardTable", dashboard);
    }

    [HttpPost]
    public async Task<IActionResult> SortTopAgentList(string columnName, string sortOrder, string selectedPeriod)
    {
        if (string.IsNullOrEmpty(selectedPeriod))
        {
            selectedPeriod = "DAILY";
        }
        var data = await _dashBoardServices.GetTopAgentDashboard(selectedPeriod, columnName, sortOrder);
        var dashboard = new AdminDashboardDetails
        {
            dashboardTopAgent = data.ToList()
        };
        return Json(new { agentData = data });
    }

    [HttpGet]
    public async Task<IActionResult> GetTopAgentLocationList(string period)
    {
        var data = await _dashBoardServices.GetTopAgentLocationDashboard(period, null, null);
        var dashboard = new AdminDashboardDetails
        {
            dashboardTopAgentLocation = data.ToList()
        };
        return PartialView("_AgentsLocationDashboardTable", dashboard);
    }

    [HttpPost]
    public async Task<IActionResult> SortTopAgentLocationList(string columnName, string sortOrder, string selectedPeriod)
    {
        if (string.IsNullOrEmpty(selectedPeriod))   
        {
            selectedPeriod = "DAILY";
        }
        var data = await _dashBoardServices.GetTopAgentLocationDashboard(selectedPeriod, columnName, sortOrder);
        var dashboard = new AdminDashboardDetails
        {
            dashboardTopAgentLocation = data.ToList()
        };
        return Json(new { agentLocationData = data });      
    }

    [HttpPost]
    public async Task<IActionResult> SortPartnerBalanceList(string columnName, string sortOrder)
    {
        var data = await _dashBoardServices.GetPartnerBalanceApproxDaysDashboard(columnName, sortOrder);
        var dashboard = new AdminDashboardDetails
        {
            dashboardPartnerBalance = data.ToList()
        };
        return Json(new { partnerBalance = data });
    }
        
    [HttpGet]
    public async Task<IActionResult> GetPaymentMode(string period)
    {
        var data = await _dashBoardServices.GetPaymentModeDashboard(period, null, null);
        var dashboard = new AdminDashboardDetails
        {
            dashboardPaymentMode = data.ToList()
        };
        return PartialView("_PaymentModeDashboardTable", dashboard);
    }

    [HttpPost]
    public async Task<IActionResult> SortPaymentModeList(string columnName, string sortOrder, string selectedPeriod)
    {
        if (string.IsNullOrEmpty(selectedPeriod))
        {
            selectedPeriod = "DAILY";
        }
        var data = await _dashBoardServices.GetPaymentModeDashboard(selectedPeriod, columnName, sortOrder);
        var dashboard = new AdminDashboardDetails
        {
            dashboardPaymentMode = data.ToList()
        };
        return Json(new { paymentMode = data });
    }
        
    [HttpGet]
    public async Task<IActionResult> GetThresholdTransaction(string period)
    {
        var data = await _dashBoardServices.GetThresholdDataDashboard(period, null, null);
        var dashboard = new AdminDashboardDetails
        {
            dashboardThresholdTrans = data.ToList()
        };
        return PartialView("_ThresholdTransactionTable", dashboard);
    }

    [HttpPost]
    public async Task<IActionResult> SortThresholdTransaction(string columnName, string sortOrder, string selectedPeriod)
    {
        if (string.IsNullOrEmpty(selectedPeriod))
        {
            selectedPeriod = "DAILY";
        }
        var data = await _dashBoardServices.GetThresholdDataDashboard(selectedPeriod, columnName, sortOrder);
        var dashboard = new AdminDashboardDetails
        {
            dashboardThresholdTrans = data.ToList() 
        };
        return Json(new { thresholdTransaction = data });   
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

        var redirectUrl = Url.Action("transactionadminreportsindex", "Reports");
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
    public async Task<IActionResult> Getnotification()
    {
        var data = await _notificationdetail.GetAdminNotificationAsync();
        return Ok(data);
    }

    [HttpPost]
    public async Task<IActionResult> MarkAsRead(string id)
    {
        Guid data = Guid.Parse(id);
        await _notificationdetail.MarkNotificationAsRead(data);
        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> GetAdminnotificationcount()
    {
        var data = await _notificationdetail.GetAdminNotificationCountAsync();
        return Ok(data);
    }

    [HttpGet]
    public async Task<IActionResult> SenderDetail(string TransactionId)
    {
        var senderdetail = await _transactionServices.GetSenderByTxnId(TransactionId);
        return PartialView("_SenderDetails", senderdetail);
    }

    [HttpGet]
    public async Task<IActionResult> Notifications([FromQuery] NotificationsFilter filter)
    {
        filter.UserType = "ADMIN";
        var result = await _notificationdetail.GetNotificationAsync(filter);

        if (WebHelper.IsAjaxRequest(Request))
            return await Task.FromResult(PartialView("_Notifications", result));
        return await Task.FromResult(View(result));
    }

    [HttpGet]
    public async Task<IActionResult> ReciverDetail(string TransactionId)
    {
        var recipientsdetail = await _transactionServices.GetRecipientByTxnId(TransactionId);
        return PartialView("_ReceiverDetails", recipientsdetail);
    }

    [HttpGet]
    public IActionResult EnableAuthenticator()
    {
        var email = _loggedInUser.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email).Value;
        EnableAuthenticatorModel Model = new EnableAuthenticatorModel();
        var userEmail = email;
        var twoFactorAuthenticator = new TwoFactorAuthenticator();
        var secretCode = Guid.NewGuid().ToString().Replace("-", "")[0..10];
        var accountSecretKey = $"{secretCode}-{userEmail}";
        var setupCode = twoFactorAuthenticator.GenerateSetupCode("2FA", userEmail,
            Encoding.ASCII.GetBytes(accountSecretKey));
        Model.SharedKey = setupCode.ManualEntryKey;
        string AuthenticatorUriFormat = string.Format("otpauth://totp/twofact:{0}?secret={1}&issuer=twofact&digits=6", userEmail, setupCode.ManualEntryKey);
        Model.AuthenticatorUri = AuthenticatorUriFormat;
        UpdateAccountSecretKey(userEmail, accountSecretKey);
        // return await Task.FromResult(PartialView("_EnableAuthenticator", Model));
        return PartialView("EnableAuthenticator", Model);
    }

    public void UpdateAccountSecretKey(string email, string Accountsecretkey)
    {
        _userRegistrationService.UpdateAccountSecretKey(email, Accountsecretkey);
    }

    #region changePartnerPassword

    [HttpGet]
    public async Task<IActionResult> AdminchangePassword()
    {
        return PartialView();
    }

    [HttpPost]
    [LogUserActivity("changed admin password")]
    public async Task<IActionResult> AdminchangePassword(PartnerChangePasswordVM changepassword)
    {
        if (!ModelState.IsValid)
        {
            Response.StatusCode = (int)(HttpStatusCode.BadRequest);
            return PartialView(changepassword);
        }
        var responseStatus = await _userRegistrationService.ChangePasswordAsync(changepassword);
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

    #endregion
}
