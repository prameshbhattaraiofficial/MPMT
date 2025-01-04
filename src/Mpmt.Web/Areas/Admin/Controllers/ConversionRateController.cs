using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Mpmt.Core.Common;
using Mpmt.Core.Dtos.ConversionRate;
using Mpmt.Core.Dtos.ConversionRateHistory;
using Mpmt.Core.Dtos.WebCrawler;
using Mpmt.Core.Extensions;
using Mpmt.Core.Models.Mail;
using Mpmt.Core.ViewModel.ConversionRate;
using Mpmt.Services.Services.Common;
using Mpmt.Services.Services.ConversionRate;
using Mpmt.Services.Services.MailingService;
using Mpmt.Services.Services.RoleMenuPermission;
using Mpmt.Web.Common;
using Mpmt.Web.Filter;
using System.Data;
using System.Net;
using System.Text;

namespace Mpmt.Web.Areas.Admin.Controllers;

/// <summary>
/// The conversion rate controller.
/// </summary>
[RolePremission]
[AdminAuthorization]
public class ConversionRateController : BaseAdminController
{
    private readonly IConversionRateServices _conversionRateServices;
    private readonly INotyfService _notyfService;
    private readonly ICommonddlServices _commonddlServices;
    private readonly IRMPService _rMPService;
    private readonly IMailService _mailService;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="ConversionRateController"/> class.
    /// </summary>
    /// <param name="conversionRateServices">The conversion rate services.</param>
    /// <param name="notyfService">The notyf service.</param>
    /// <param name="mapper">The mapper.</param>
    /// <param name="commonddlServices">The commonddl services.</param>
    public ConversionRateController(IConversionRateServices conversionRateServices, INotyfService notyfService, IMapper mapper, ICommonddlServices commonddlServices, IRMPService rMPService, IMailService mailService)
    {
        _conversionRateServices = conversionRateServices;
        _notyfService = notyfService;
        _mapper = mapper;
        _commonddlServices = commonddlServices;
        _rMPService = rMPService;
        _mailService = mailService;
    }

    /// <summary>
    /// Conversions the rate index.
    /// </summary>
    /// <param name="ConversionRateFilter">The conversion rate filter.</param>
    /// <returns>A Task.</returns>
    [HttpGet]
    public async Task<IActionResult> ConversionRateIndex([FromQuery] ConversionRateFilter ConversionRateFilter)
    {
        var actions = await _rMPService.GetActionPermissionListAsync("ConversionRate");

        ViewBag.actions = actions;
        var result = await _conversionRateServices.GetConversionRateAsync(ConversionRateFilter);
        if (WebHelper.IsAjaxRequest(Request))
            return await Task.FromResult(PartialView("_ConversionRateIndex", result));

        return await Task.FromResult(View(result));
    }

    [HttpGet]
    public async Task<IActionResult> ExchangeRateHistory([FromQuery] ExchangeRateFilter exchangeRateFilter)
    {
        var actions = await _rMPService.GetActionPermissionListAsync("ConversionRate");
        var currencyddl = await _commonddlServices.GetCurrencyddl();
        ViewBag.Wallet = exchangeRateFilter.Wallet;
        if (exchangeRateFilter.WalletCurrency is null)
            exchangeRateFilter.WalletCurrency = exchangeRateFilter.Wallet;
        ViewBag.Actions = actions;
        ViewBag.Currency = new SelectList(currencyddl, "value", "Text");
        var result = await _conversionRateServices.GetExchangeRateHistoryAsync(exchangeRateFilter);
        ViewBag.ExchangeRateFilter = result.Items.FirstOrDefault();
        if (WebHelper.IsAjaxRequest(Request))
            return await Task.FromResult(PartialView("_ExchangeRateHistory", result));

        return await Task.FromResult(View(result));
    }

    [HttpGet("ExportExchangeRateToCSV")]
    [LogUserActivity("exported exchange rate to CSV")]
    public async Task<IActionResult> ExportExchangeRateToCSV([FromQuery] ExchangeRateFilter request)
    {
        request.Export = 1;
        var result = await _conversionRateServices.GetExchangeRateHistoryAsync(request);
        var data = result.Items;
        var (bytes, fileformate, filename) = ExportHelper.GenerateCsv(data, new string[] { }, null, "ExchangeRateHistory", true);
        return File(bytes, fileformate, filename);
    }

    [HttpGet("ExportExchangeRateToExcel")]
    [LogUserActivity("exported exchange rate to Excel")]
    public async Task<IActionResult> ExportExchangeRateToExcel([FromQuery] ExchangeRateFilter request)
    {
        request.Export = 1;
        var result = await _conversionRateServices.GetExchangeRateHistoryAsync(request);

        var data = result.Items;
        List<DataTable> dataTables = await IEnumerableExtensions.ToDataTablesAsync<ExchangeRateHistoryDetails>(data, 500000);
        var (excelFileByteArr, fileFormat, fileName) = await ExportHelper.ToExcelAsync(dataTables, "ExchangeRateHistory", true);

        return File(excelFileByteArr, fileFormat, fileName);
    }

    [HttpGet("ExportExchangeRateToPDF")]
    [LogUserActivity("exported exchange rate to PDF")]
    public async Task<FileContentResult> ExportExchangeRateToPDF([FromQuery] ExchangeRateFilter request)
    {
        request.Export = 1;
        var result = await _conversionRateServices.GetExchangeRateHistoryAsync(request);
        var data = result;

        var (bytedata, format) = await ExportHelper.TopdfAsync(data, "Conversion Rate History");
        return File(bytedata, format, "ExchangeRateHistory.pdf");
    }

    #region Add-Conversion-Rate

    /// <summary>
    /// Adds the conversion rate.
    /// </summary>
    /// <returns>A Task.</returns>
    [HttpGet]
    public async Task<IActionResult> AddConversionRate()
    {
        var currencyddl = await _commonddlServices.GetCurrencyddl();
        ViewBag.Currency = new SelectList(currencyddl, "value", "Text");
        return await Task.FromResult(PartialView());
    }

    /// <summary>
    /// Adds the conversion rate.
    /// </summary>
    /// <param name="addConversionRateVm">The add conversion rate vm.</param>
    /// <returns>A Task.</returns>
    [HttpPost]
    [LogUserActivity("added conversion rate")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddConversionRate([FromForm] AddConversionRateVm addConversionRateVm)
    {
        var currencyddl = await _commonddlServices.GetCurrencyddl();
        ViewBag.Currency = new SelectList(currencyddl, "value", "Text");
        if (!ModelState.IsValid)
        {
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return PartialView();
        }
        else
        {
            var responseStatus = await _conversionRateServices.AddConversionRateAsync(addConversionRateVm);
            if (responseStatus.StatusCode == 200)
            {
                _notyfService.Success(responseStatus.MsgText);
                return Ok();
            }
            else
            {
                Response.StatusCode = (int)(HttpStatusCode.BadRequest);
                ViewBag.Error = responseStatus.MsgText;
                return PartialView();
            }
        }
    }

    #endregion

    #region Update-Conversion-Rate

    /// <summary>
    /// Updates the conversion rate.
    /// </summary>
    /// <param name="conversionRateId">The conversion rate id.</param>
    /// <returns>A Task.</returns>
    [HttpGet]
    public async Task<IActionResult> UpdateConversionRate(int conversionRateId)
    {
        var currencyddl = await _commonddlServices.GetCurrencyddl();
        ViewBag.Currency = new SelectList(currencyddl, "value", "Text");
        var result = await _conversionRateServices.GetConversionRateByIdAsync(conversionRateId);
        var mappedData = _mapper.Map<UpdateConversionRateVm>(result);
        return await Task.FromResult(PartialView(mappedData));
    }

    /// <summary>
    /// Updates the conversion rate.
    /// </summary>
    /// <param name="updateConversionRateVm">The update conversion rate vm.</param>
    /// <returns>A Task.</returns>
    [HttpPost]
    [LogUserActivity("updated conversion rate")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateConversionRate([FromForm] UpdateConversionRateVm updateConversionRateVm)
    {
        var currencyddl = await _commonddlServices.GetCurrencyddl();
        ViewBag.Currency = new SelectList(currencyddl, "value", "Text");
        if (!ModelState.IsValid)
        {
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return PartialView();
        }
        else
        {
            var (responseStatus, data) = await _conversionRateServices.UpdateConversionRateAsync(updateConversionRateVm);
            if (responseStatus.StatusCode == 200)
            {
                if (updateConversionRateVm.IsSendNotificationEmail)
                {
                    var groupedPartners = data.GroupBy(p => p.PartnerCode);

                    foreach (var group in groupedPartners)
                    {
                        var changedPartnerRate = group.Select(partner => new ExchangeRateChangedListPartner
                        {
                            PartnerCode = partner.PartnerCode,
                            PartnerName = partner.PartnerName,
                            Email = partner.Email,
                            Currency = partner.Currency,
                            PrevRate = partner.PrevRate,
                            CurrRate = partner.CurrRate,
                            UpdatedDate = partner.UpdatedDate
                        }).ToList();
                        var mailBody = GenerateMailBody(changedPartnerRate, changedPartnerRate.FirstOrDefault().PartnerName, changedPartnerRate.FirstOrDefault().UpdatedDate);
                        await SendEmailWithAttachment(mailBody, changedPartnerRate.FirstOrDefault().Email, changedPartnerRate.FirstOrDefault().PartnerName);
                    }
                }
                _notyfService.Success(responseStatus.MsgText);
                return Ok();
            }
            else
            {
                Response.StatusCode = (int)(HttpStatusCode.BadRequest);
                ViewBag.Error = responseStatus.MsgText;
                return PartialView();
            }
        }
    }

    public async Task SendEmailWithAttachment(string mailBody, string mailTo, string partnerName)
    {
        var mailRequest = new MailRequestModel
        {
            MailFor = "exchange-rate-change",
            MailTo = mailTo,
            MailSubject = "Exchange Rate Update",
            RecipientName = partnerName,
            Content = mailBody
        };
        var mailServiceModel = await _mailService.EmailSettings(mailRequest);
        Thread email = new(delegate ()
        {
            _mailService.SendMail(mailServiceModel);
        });
        email.Start();
    }

    public string GenerateMailBody(List<ExchangeRateChangedListPartner> rateChangedListPartners, string partnerName, DateTime updatedDate)
    {
        var companyName = "MyPay Money Transfer Pvt. Ltd.";
        var companyEmail = "info@mypaymt.com";

        string mailBody =
            $@"
                <p>Following Exchange rates have been updated on: <b>{updatedDate.ToString("yyyy-MMM-dd hh:mm:ss tt")} (NST)</b></p>
                    {GetTableHtml(rateChangedListPartners)}
                <br>    
                <p style='font-weight:bold; color: red!important;'>This is automated email regarding Exchange rate as per FEDAN. You will receive email only when rates gets changed.</p>
                <h3><u>MyPay Money Transfer Service Contact Information:</u></h3>
                    <p>
                        {companyName}<br>
                        Contact No.: 015908052<br>
                        Email: {companyEmail}<br>
                        Website: https://www.mypaymt.com/<br>
                        Address: Radhe Radhe, Bhaktapur, Nepal<br>
                    </p>
                    <p>Thank you for choosing {companyName} Service!</p>
                <p style='color=orange;>ATTN : Please do not reply to this email. This mailbox is not monitored and you will not receive a response.</p>
                <br>
                <p style='color=red;'>CONFIDENTIALITY NOTICE: This transmittal is a confidential communication or may otherwise be privileged. If you are not the intended recipient, you are hereby notified that you have received this transmittal in error and that any review, dissemination, distribution or copying of this transmittal is strictly prohibited. If you have received this communication in error, please notify this office, and immediately delete this message and all its attachments, if any.</p>";
        return mailBody;
    }

    private string GetTableHtml(List<ExchangeRateChangedListPartner> data)
    {
        if (data == null || data.Count == 0)
        {
            return "<p>No exchange rate data available.</p>";
        }

        StringBuilder tableHtml = new StringBuilder();
        tableHtml.AppendLine("<style>");
        tableHtml.AppendLine("table {");
        tableHtml.AppendLine("  border-collapse: collapse;");
        tableHtml.AppendLine("  width: 100%;");
        tableHtml.AppendLine("}");
        tableHtml.AppendLine("th, td {");
        tableHtml.AppendLine("  padding: 8px 16px;"); // Increased padding for more space
        tableHtml.AppendLine("  text-align: left;");
        tableHtml.AppendLine("  border-bottom: 1px solid #ddd;");
        tableHtml.AppendLine("}");
        tableHtml.AppendLine("tr:nth-child(even) {background-color: #f2f2f2}");
        tableHtml.AppendLine("th {");
        tableHtml.AppendLine("  background-color: #2b7f00;");
        tableHtml.AppendLine("  color: white;");
        tableHtml.AppendLine("}");
        tableHtml.AppendLine("</style>");

        tableHtml.AppendLine("<table style='border-collapse: collapse; width: 100%;'>");
        tableHtml.AppendLine("<thead style='padding: 8px 16px; text-align: left; border-bottom: 1px solid #ddd; background-color: #2b7f00; color: white;'>");
        tableHtml.AppendLine("<tr>");
        tableHtml.AppendLine("<th>SN</th>");
        tableHtml.AppendLine("<th>Currency</th>");
        tableHtml.AppendLine("<th>Current Rate</th>");
        tableHtml.AppendLine("<th>Previous Rate</th>");
        tableHtml.AppendLine("</tr>");
        tableHtml.AppendLine("</thead>");
        tableHtml.AppendLine("<tbody>");

        int sn = 1;

        foreach (var partner in data)
        {
            tableHtml.AppendLine("<tr>");
            tableHtml.AppendLine($"<td style='padding: 8px 16px; text-align: left; border-bottom: 1px solid #ddd;'>{sn++}</td>");
            tableHtml.AppendLine($"<td style='padding: 8px 16px; text-align: left; border-bottom: 1px solid #ddd;'>{partner.Currency}</td>");
            tableHtml.AppendLine($"<td style='padding: 8px 16px; text-align: left; border-bottom: 1px solid #ddd;'>{partner.CurrRate.ToString("F2")}</td>");
            tableHtml.AppendLine($"<td style='padding: 8px 16px; text-align: left; border-bottom: 1px solid #ddd;'>{partner.PrevRate.ToString("F2")}</td>");
            tableHtml.AppendLine("</tr>");
        }

        tableHtml.AppendLine("</tbody>");
        tableHtml.AppendLine("</table>");

        return tableHtml.ToString();
    }

    #endregion

    #region Delete-Conversion-Rate

    /// <summary>
    /// Deletes the conversion rate.
    /// </summary>
    /// <param name="conversionRateId">The conversion rate id.</param>
    /// <returns>A Task.</returns>
    [HttpGet]
    public async Task<IActionResult> DeleteConversionRate(int conversionRateId)
    {
        // -> Id not PartnerCode
        var result = await _conversionRateServices.GetConversionRateByIdAsync(conversionRateId);
        var mappedData = _mapper.Map<UpdateConversionRateVm>(result);
        return await Task.FromResult(PartialView(mappedData));
    }

    /// <summary>
    /// Deletes the conversion rate.
    /// </summary>
    /// <param name="deleteConversionRateVm">The delete conversion rate vm.</param>
    /// <returns>A Task.</returns>
    [HttpPost]
    [LogUserActivity("deleted conversion rate")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConversionRate([FromForm] UpdateConversionRateVm deleteConversionRateVm)
    {
        if (deleteConversionRateVm.Id != 0)
        {
            var responseStatus = await _conversionRateServices.RemoveConversionRateAsync(deleteConversionRateVm);
            if (responseStatus.StatusCode == 200)
            {
                _notyfService.Success(responseStatus.MsgText);
                return Ok();
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                TempData["Error"] = responseStatus.MsgText;
            }
        }
        Response.StatusCode = (int)HttpStatusCode.NotFound;
        return PartialView();
    }
    #endregion
}
