using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Mpmt.Core.Common;
using Mpmt.Core.Dtos.PartnerReport;
using Mpmt.Core.Dtos.Transaction;
using Mpmt.Core.Extensions;
using Mpmt.Services.Partner;
using Mpmt.Services.Services.Common;
using Mpmt.Services.Services.RoleMenuPermission;
using Mpmt.Web.Common;
using Mpmt.Web.Filter;
using System.Data;

namespace Mpmt.Web.Areas.Partner.Controllers;

[PartnerAuthorization]
[RolePremission]
public class ReportsController : BasePartnerController
{
    private readonly IPartnerReportServices _partnerReportServices;
    private readonly ICommonddlServices _commonddlServices;
    private readonly ITransactionServices _transactionServices;
    private readonly IRMPService _rMPService;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public ReportsController(IPartnerReportServices partnerReportServices, ICommonddlServices commonddlServices, ITransactionServices transactionServices, IRMPService rMPService, IWebHostEnvironment webHostEnvironment)
    {
        _partnerReportServices = partnerReportServices;
        _commonddlServices = commonddlServices;
        _transactionServices = transactionServices;
        _rMPService = rMPService;
        _webHostEnvironment = webHostEnvironment;
    }

    [HttpGet]
    public async Task<IActionResult> TransactionReportsIndex([FromQuery] RemitTxnReportFilter reportFilter)
    {
        var actions = await _rMPService.GetPartnerActionPermissionList("Reports");
        ViewBag.actions = actions;

        ViewBag.StartDate = TempData["StartDate"];
        ViewBag.EndDate = TempData["EndDate"];

        var currencyddl = await _commonddlServices.GetCurrencyddl();

        ViewBag.SCurrency = new SelectList(currencyddl, "value", "Text");
        ViewBag.DCurrency = new SelectList(currencyddl, "value", "Text");
        var PaymentTypeddl = await _commonddlServices.GetPaymentTypeddl();

        var transactionStatus = await _commonddlServices.GetTransactionStatusddl();
        ViewBag.PaymentTypeddl = new SelectList(PaymentTypeddl, "lookup", "Text");
        ViewBag.TStatus = new SelectList(transactionStatus, "value", "Text");
        var PartnerId = User.Claims.FirstOrDefault(x => x.Type == "PartnerCode").Value;
        reportFilter.PartnerCode = PartnerId;
        if (ViewBag.StartDate != null && ViewBag.EndDate != null)
        {
            reportFilter.StartDate = ViewBag.StartDate;
            reportFilter.EndDate = ViewBag.EndDate;
        }
        var result = await _partnerReportServices.GetRemitTxnReportAsync(reportFilter);
        ViewBag.TransactionFilter = result.Items.FirstOrDefault();
        if (WebHelper.IsAjaxRequest(Request))
            return await Task.FromResult(PartialView("_TransactionReportsIndex", result));
        return await Task.FromResult(View(result));
    }

    [HttpGet]
    public async Task<IActionResult> DownloadReceiptReport(string Id)
    {
        var result = await _transactionServices.GetReceiptDetailsById(Id);
        var (bytedata, format) = await ExportHelper.GenerateReceiptReportPdf(result, _webHostEnvironment);
        return File(bytedata, format, "SenderReceipt.pdf");
    }

    [HttpGet("ExportPartnerTransactionToCsv")]
    public async Task<IActionResult> ExportPartnerTransactionToCsv([FromQuery] RemitTxnReportFilter request)
    {
        request.Export = 1;
        var PartnerId = User.Claims.FirstOrDefault(x => x.Type == "PartnerCode").Value;
        request.PartnerCode = PartnerId;
        var result = await _partnerReportServices.ExportRemitTxnReportAsync(request);
        var data = result.Items;

        var updatedData = data.Select(r =>
        {
            r.RecipientAccountNumber = $"=\"{r.RecipientAccountNumber}\"";
            r.TransactionId = $"=\"{r.TransactionId}\"";
            r.GatewayTxnId = $"=\"{r.GatewayTxnId}\"";
            r.PartnerTrackerId = $"=\"{r.PartnerTrackerId}\"";
            return r;
        });

        var (bytes, fileformate, filename) = ExportHelper.GenerateCsv(updatedData, new string[] { }, null, "TransactionReports", true);
        return File(bytes, fileformate, filename);
    }

    [HttpGet("ExportPartnerTransactiontoexcell")]
    public async Task<IActionResult> ExportPartnerTransactiontoexcell([FromQuery] RemitTxnReportFilter request)
    {
        request.Export = 1;
        var PartnerId = User.Claims.FirstOrDefault(x => x.Type == "PartnerCode").Value;
        request.PartnerCode = PartnerId;
        var result = await _partnerReportServices.ExportRemitTxnReportAsync(request);
        var data = result.Items;
        List<DataTable> dataTables = await IEnumerableExtensions.ToDataTablesAsync<ExportRemitTxnReport>(data, 500000);
        var (excelFileByteArr, fileFormat, fileName) = await ExportHelper.ToExcelAsync(dataTables, "TransactionReports", true);

        return File(excelFileByteArr, fileFormat, fileName);
    }


    [HttpGet("ExportPartnerTransactiontoPdf")]
    public async Task<FileContentResult> ExportPartnerTransactiontoPdf([FromQuery] RemitTxnReportFilter request)
    {
        request.Export = 1;
        var PartnerId = User.Claims.FirstOrDefault(x => x.Type == "PartnerCode").Value;
        request.PartnerCode = PartnerId;
        var result = await _partnerReportServices.ExportRemitTxnReportAsync(request);
        var data = result;

        var (bytedata, format) = await ExportHelper.TopdfAsync<ExportRemitTxnReport>(data, "Transactional Reports");
        return File(bytedata, format, "txnreport.pdf");

    }

    [HttpGet]
    public async Task<IActionResult> RemitSettlementReport([FromQuery] RemitSettlementReportFilter reportFilter)
    {
        var actions = await _rMPService.GetActionPermissionListAsync("Reports");
        ViewBag.actions = actions;

        var currencyddl = await _commonddlServices.GetCurrencyddl();
        ViewBag.SCurrency = new SelectList(currencyddl, "value", "Text");
        ViewBag.DCurrency = new SelectList(currencyddl, "value", "Text");
        var PartnerId = User.Claims.FirstOrDefault(x => x.Type == "PartnerCode").Value;
        reportFilter.PartnerCode = PartnerId;
        var result = await _partnerReportServices.GetRemitSettlementReportAsync(reportFilter);
        if (result is not null)
            ViewBag.TransactionFilter = result.Items.FirstOrDefault();
        if (WebHelper.IsAjaxRequest(Request))
            return await Task.FromResult(PartialView("_RemitSettlementReport", result));
        return await Task.FromResult(View(result));
    }
    [HttpGet("ExportPartnerRemitSettlementToCsv")]
    public async Task<IActionResult> ExportPartnerRemitSettlementToCsv([FromQuery] RemitSettlementReportFilter request)
    {
        request.Export = 1;
        var PartnerId = User.Claims.FirstOrDefault(x => x.Type == "PartnerCode").Value;
        request.PartnerCode = PartnerId;
        var result = await _partnerReportServices.ExportRemitSettlementReportAsync(request);
        var data = result.Items;
        var (bytes, fileformate, filename) = ExportHelper.GenerateCsv(data, new string[] { }, null, "RemitSettlementReports", true);
        return File(bytes, fileformate, filename);
    }

    [HttpGet("ExportPartnerRemitSettlementtoexcell")]
    public async Task<IActionResult> ExportPartnerRemitSettlementtoexcell([FromQuery] RemitSettlementReportFilter request)
    {
        request.Export = 1;
        var PartnerId = User.Claims.FirstOrDefault(x => x.Type == "PartnerCode").Value;
        request.PartnerCode = PartnerId;
        var result = await _partnerReportServices.ExportRemitSettlementReportAsync(request);
        var data = result.Items;
        List<DataTable> dataTables = await IEnumerableExtensions.ToDataTablesAsync<ExportRemitSettlementReport>(data, 500000);
        var (excelFileByteArr, fileFormat, fileName) = await ExportHelper.ToExcelAsync(dataTables, "RemitSettlementReports", true);

        return File(excelFileByteArr, fileFormat, fileName);
    }


    [HttpGet("ExportPartnerRemitSettlementtoPdf")]
    public async Task<FileContentResult> ExportPartnerRemitSettlementtoPdf([FromQuery] RemitSettlementReportFilter request)
    {

        request.Export = 1;
        var PartnerId = User.Claims.FirstOrDefault(x => x.Type == "PartnerCode").Value;
        request.PartnerCode = PartnerId;
        var result = await _partnerReportServices.ExportRemitSettlementReportAsync(request);
        var data = result;

        var (bytedata, format) = await ExportHelper.TopdfAsync<ExportRemitSettlementReport>(data, "RemitSettlemental Reports");
        return File(bytedata, format, "RemitSettlementreport.pdf");

    }



    [HttpGet]
    public async Task<IActionResult> SettlementReport([FromQuery] RemitSettlementReportFilter reportFilter)
    {
        var actions = await _rMPService.GetActionPermissionListAsync("Reports");
        ViewBag.actions = actions;

        var currencyddl = await _commonddlServices.GetCurrencyddl();
        var paymentTypeddl = await _commonddlServices.GetPaymentTypeddl();
        ViewBag.SCurrency = new SelectList(currencyddl, "value", "Text");
        ViewBag.paymentTypeddl = new SelectList(paymentTypeddl, "lookup", "Text");
        ViewBag.DCurrency = new SelectList(currencyddl, "value", "Text");
        var PartnerId = User.Claims.FirstOrDefault(x => x.Type == "PartnerCode").Value;
        reportFilter.PartnerCode = PartnerId;
        var result = await _partnerReportServices.GetSettlementReportAsync(reportFilter);
        ViewBag.TransactionFilter = result.Items.FirstOrDefault();
        if (WebHelper.IsAjaxRequest(Request))
            return await Task.FromResult(PartialView("_SettlementReport", result));
        return await Task.FromResult(View(result));
    }
    [HttpGet("ExportPartnerSettlementToCsv")]
    public async Task<IActionResult> ExportPartnerSettlementToCsv([FromQuery] RemitSettlementReportFilter request)
    {
        request.Export = 1;
        var PartnerId = User.Claims.FirstOrDefault(x => x.Type == "PartnerCode").Value;
        request.PartnerCode = PartnerId;
        var result = await _partnerReportServices.ExportSettlementReportAsync(request);
        var data = result.Items;
        var (bytes, fileformate, filename) = ExportHelper.GenerateCsv(data, new string[] { }, null, "SettlementReports", true);
        return File(bytes, fileformate, filename);
    }

    [HttpGet("ExportPartnerSettlementtoexcell")]
    public async Task<IActionResult> ExportPartnerSettlementtoexcell([FromQuery] RemitSettlementReportFilter request)
    {
        request.Export = 1;
        var PartnerId = User.Claims.FirstOrDefault(x => x.Type == "PartnerCode").Value;
        request.PartnerCode = PartnerId;
        var result = await _partnerReportServices.ExportSettlementReportAsync(request);
        var data = result.Items;
        List<DataTable> dataTables = await IEnumerableExtensions.ToDataTablesAsync<ExportRemitSettlementReport>(data, 500000);
        var (excelFileByteArr, fileFormat, fileName) = await ExportHelper.ToExcelAsync(dataTables, "SettlementReports", true);

        return File(excelFileByteArr, fileFormat, fileName);
    }


    [HttpGet("ExportPartnerSettlementtoPdf")]
    public async Task<FileContentResult> ExportPartnerSettlementtoPdf([FromQuery] RemitSettlementReportFilter request)
    {

        request.Export = 1;
        var PartnerId = User.Claims.FirstOrDefault(x => x.Type == "PartnerCode").Value;
        request.PartnerCode = PartnerId;
        var result = await _partnerReportServices.ExportSettlementReportAsync(request);
        var data = result;

        var (bytedata, format) = await ExportHelper.TopdfAsync<ExportRemitSettlementReport>(data, "Settlemental Reports");
        return File(bytedata, format, "Settlementreport.pdf");

    }



    [HttpGet]
    public async Task<IActionResult> RemitFeeTransactionReport([FromQuery] RemitTxnReportFilter reportFilter)
    {
        var currencyddl = await _commonddlServices.GetCurrencyddl();
        ViewBag.SCurrency = new SelectList(currencyddl, "value", "Text");
        ViewBag.DCurrency = new SelectList(currencyddl, "value", "Text");
        var PartnerId = User.Claims.FirstOrDefault(x => x.Type == "PartnerCode").Value;
        reportFilter.PartnerCode = PartnerId;
        var result = await _partnerReportServices.GetRemitFeeTxnReportAsync(reportFilter);
        ViewBag.TransactionFilter = result.Items.FirstOrDefault();
        if (WebHelper.IsAjaxRequest(Request))
            return await Task.FromResult(PartialView("_RemitFeeTransactionReport", result));
        return await Task.FromResult(View(result));
    }

    [HttpGet("ExportPartnerRemitFeeTransactionToCsv")]
    public async Task<IActionResult> ExportPartnerRemitFeeTransactionToCsv([FromQuery] RemitTxnReportFilter request)
    {
        request.Export = 1;
        var PartnerId = User.Claims.FirstOrDefault(x => x.Type == "PartnerCode").Value;
        request.PartnerCode = PartnerId;
        var result = await _partnerReportServices.GetRemitFeeTxnReportAsync(request);
        var data = result.Items;
        var (bytes, fileformate, filename) = ExportHelper.GenerateCsv(data, new string[] { }, null, "RemitFeeTransactionReports", true);
        return File(bytes, fileformate, filename);
    }

    [HttpGet("ExportPartnerRemitFeeTransactiontoexcell")]
    public async Task<IActionResult> ExportPartnerRemitFeeTransactiontoexcell([FromQuery] RemitTxnReportFilter request)
    {
        request.Export = 1;
        var PartnerId = User.Claims.FirstOrDefault(x => x.Type == "PartnerCode").Value;
        request.PartnerCode = PartnerId;
        var result = await _partnerReportServices.GetRemitFeeTxnReportAsync(request);
        var data = result.Items;
        List<DataTable> dataTables = await IEnumerableExtensions.ToDataTablesAsync<RemitFeeTxnReport>(data, 500000);
        var (excelFileByteArr, fileFormat, fileName) = await ExportHelper.ToExcelAsync(dataTables, "RemitFeeTransactionReports", true);

        return File(excelFileByteArr, fileFormat, fileName);
    }


    [HttpGet("ExportPartnerRemitFeeTransactiontoPdf")]
    public async Task<FileContentResult> ExportPartnerRemitFeeTransactiontoPdf([FromQuery] RemitTxnReportFilter request)
    {

        request.Export = 1;
        var PartnerId = User.Claims.FirstOrDefault(x => x.Type == "PartnerCode").Value;
        request.PartnerCode = PartnerId;
        var result = await _partnerReportServices.GetRemitFeeTxnReportAsync(request);
        var data = result;

        var (bytedata, format) = await ExportHelper.TopdfAsync<RemitFeeTxnReport>(data, "RemitFeeTransaction Reports");
        return File(bytedata, format, "RemitFeeTransactionreport.pdf");

    }

    [HttpGet]
    public async Task<IActionResult> RemitCommissionTransactionReport([FromQuery] CommissionTransactionFilter reportFilter)
    {
        var currencyddl = await _commonddlServices.GetCurrencyddl();
        ViewBag.SCurrency = new SelectList(currencyddl, "value", "Text");
        ViewBag.DCurrency = new SelectList(currencyddl, "value", "Text");
        var PartnerId = User.Claims.FirstOrDefault(x => x.Type == "PartnerCode").Value;
        reportFilter.PartnerCode = PartnerId;
        var result = await _partnerReportServices.GetRemitCommissionTxnReportAsync(reportFilter);
        ViewBag.TransactionFilter = result.Items.FirstOrDefault();
        if (WebHelper.IsAjaxRequest(Request))
            return await Task.FromResult(PartialView("_RemitCommissionTransactionReport", result));
        return await Task.FromResult(View(result));
    }


    [HttpGet("ExportPartnerRemitCommissionTransactionToCsv")]
    public async Task<IActionResult> ExportPartnerRemitCommissionTransactionToCsv([FromQuery] CommissionTransactionFilter request)
    {
        request.Export = 1;
        var PartnerId = User.Claims.FirstOrDefault(x => x.Type == "PartnerCode").Value;
        request.PartnerCode = PartnerId;
        var result = await _partnerReportServices.GetRemitCommissionTxnReportAsync(request);
        var data = result.Items;
        var (bytes, fileformate, filename) = ExportHelper.GenerateCsv(data, new string[] { }, null, "RemitCommissionTransactionReports", true);
        return File(bytes, fileformate, filename);
    }

    [HttpGet("ExportPartnerRemitCommissionTransactiontoexcell")]
    public async Task<IActionResult> ExportPartnerRemitCommissionTransactiontoexcell([FromQuery] CommissionTransactionFilter request)
    {
        request.Export = 1;
        var PartnerId = User.Claims.FirstOrDefault(x => x.Type == "PartnerCode").Value;
        request.PartnerCode = PartnerId;
        var result = await _partnerReportServices.GetRemitCommissionTxnReportAsync(request);
        var data = result.Items;
        List<DataTable> dataTables = await IEnumerableExtensions.ToDataTablesAsync<CommisionTransction>(data, 500000);
        var (excelFileByteArr, fileFormat, fileName) = await ExportHelper.ToExcelAsync(dataTables, "RemitCommissionTransactionReports", true);

        return File(excelFileByteArr, fileFormat, fileName);
    }


    [HttpGet("ExportPartnerRemitCommissionTransactiontoPdf")]
    public async Task<FileContentResult> ExportPartnerRemitCommissionTransactiontoPdf([FromQuery] CommissionTransactionFilter request)
    {

        request.Export = 1;
        var PartnerId = User.Claims.FirstOrDefault(x => x.Type == "PartnerCode").Value;
        request.PartnerCode = PartnerId;
        var result = await _partnerReportServices.GetRemitCommissionTxnReportAsync(request);
        var data = result;

        var (bytedata, format) = await ExportHelper.TopdfAsync<CommisionTransction>(data, "RemitCommissionTransaction Reports");
        return File(bytedata, format, "RemitCommissionTransactionreport.pdf");

    }


    [HttpGet]
    public async Task<IActionResult> SenderDetail(string TransactionId)
    {
        var senderdetail = await _transactionServices.GetSenderByTxnId(TransactionId);
        return PartialView("_SenderDetails", senderdetail);
    }

    [HttpGet]
    public async Task<IActionResult> ReciverDetail(string TransactionId)
    {
        var recipientsdetail = await _transactionServices.GetRecipientByTxnId(TransactionId);
        return PartialView("_ReceiverDetails", recipientsdetail);
    }

    [HttpGet]
    public async Task<IActionResult> PaymentDetail(string TransactionId)
    {
        var transectiondetail = await _transactionServices.PaymentDetailsByTxnIdAsync(TransactionId);
        return PartialView("_PaymentTypes", transectiondetail);
    }

    [HttpGet]
    public async Task<IActionResult> ViewDetail(string TransactionId)
    {
        var transectiondetail = await _transactionServices.GetTxnById(TransactionId);
        return PartialView("_ViewTransactionDetail", transectiondetail);
    }

    [HttpGet]
    public async Task<IActionResult> TransactionStatus(string TransactionId)
    {
        var senderdetail = await _transactionServices.GetTransactionStatus(TransactionId);
        return PartialView("_TransactionStatus", senderdetail);
    }
}
