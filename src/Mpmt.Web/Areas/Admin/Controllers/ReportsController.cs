using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using CsvHelper.Configuration;
using CsvHelper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Mpmt.Core.Common;
using Mpmt.Core.Dtos.AgentReport;
using Mpmt.Core.Dtos.DropDown;
using Mpmt.Core.Dtos.InwardRemitanceReport;
using Mpmt.Core.Dtos.PartnerReport;
using Mpmt.Core.Dtos.Transaction;
using Mpmt.Core.Extensions;
using Mpmt.Core.ViewModel.AdminReport;
using Mpmt.Services.CashAgents;
using Mpmt.Services.Partner;
using Mpmt.Services.Services.Common;
using Mpmt.Services.Services.RoleMenuPermission;
using Mpmt.Web.Common;
using Mpmt.Web.Filter;
using System.Collections.Immutable;
using System.Data;
using System.Globalization;
using OfficeOpenXml;
using Microsoft.AspNetCore.Hosting.Server;
using ClosedXML.Excel;
using OfficeOpenXml.Style;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateAndTime.Workdays;
using Mpmt.Core.Dtos.Paging;
using DocumentFormat.OpenXml.Office2019.Excel.ThreadedComments;
using AllOverIt.Extensions;

namespace Mpmt.Web.Areas.Admin.Controllers
{
    [RolePremission]
    [AdminAuthorization]
    public class ReportsController : BaseAdminController
    {
        private readonly IPartnerReportServices _partnerReportServices;
        private readonly ICommonddlServices _commonddlServices;
        private readonly IAgentReportService _agentReportService;
        private readonly ITransactionServices _transactionServices;
        private readonly IRMPService _rMPService;
        private readonly IMapper _mapper;
        private readonly INotyfService _notify;
        private readonly IPartnerSendTxnsService _partnerSendTxnsService;
        private readonly IConfiguration _config;



        public ReportsController(IConfiguration configuration, IPartnerReportServices partnerReportServices, ICommonddlServices commonddlServices, ITransactionServices transactionServices, IRMPService rMPService, IMapper mapper, INotyfService notify, IPartnerSendTxnsService partnerSendTxnsService, IAgentReportService agentReportService)
        {
            _config = configuration;
            _partnerReportServices = partnerReportServices;
            _commonddlServices = commonddlServices;
            _transactionServices = transactionServices;
            _rMPService = rMPService;
            _mapper = mapper;
            _notify = notify;
            _partnerSendTxnsService = partnerSendTxnsService;
            _agentReportService = agentReportService;
        }

        [HttpGet]
        public async Task<IActionResult> TransactionAdminReportsIndex([FromQuery] RemitTxnReportFilter reportFilter)
        {
            var actions = await _rMPService.GetActionPermissionListAsync("Reports");

            ViewBag.actions = actions;
            ViewBag.StartDate = TempData["StartDate"];
            ViewBag.EndDate = TempData["EndDate"];

            var currencyddl = await _commonddlServices.GetCurrencyddl();
            var TransactionTypeddl = await _commonddlServices.GetTransactionTypeddl();
            var PaymentTypeddl = await _commonddlServices.GetPaymentTypeddl();
            var transactionStatus = await _commonddlServices.GetTransactionStatusddl();
            ViewBag.PaymentTypeddl = new SelectList(PaymentTypeddl, "lookup", "Text");
            ViewBag.TStatus = new SelectList(transactionStatus, "value", "Text");
            ViewBag.SCurrency = new SelectList(currencyddl, "value", "Text");
            ViewBag.DCurrency = new SelectList(currencyddl, "value", "Text");
            reportFilter.PartnerCode = "admin";
            if (ViewBag.StartDate != null && ViewBag.EndDate != null)
            {
                reportFilter.StartDate = ViewBag.StartDate;
                reportFilter.EndDate = ViewBag.EndDate;
            }
            var result = await _partnerReportServices.GetRemitTxnReportAsync(reportFilter);

            ViewBag.TransactionFilter = result.Items.FirstOrDefault();
            if (WebHelper.IsAjaxRequest(Request))
                return await Task.FromResult(PartialView("_TransactionAdminReportsIndex", result));
            return await Task.FromResult(View(result));
        }
        [HttpGet("ExportTransactionToCsv")]
        public async Task<IActionResult> ExportTransactionToCsv([FromQuery] RemitTxnReportFilter request)
        {
            request.Export = 1;
            request.PartnerCode = "admin";
            var result = await _partnerReportServices.AdminExportRemitTxnReportAsync(request);
            var data = result.Items;
            var updatedData = data.Select(r =>
            {
                r.RecipientAccountNumber = $"=\"{r.RecipientAccountNumber}\"";
                r.TransactionId = $"=\"{r.TransactionId}\"";
                r.AgentTrackerId = $"=\"{r.AgentTrackerId}\"";
                r.GatewayTxnId = $"=\"{r.GatewayTxnId}\"";
                r.PartnerTrackerId = $"=\"{r.PartnerTrackerId}\"";
                return r;
            });
            var (bytes, fileformate, filename) = ExportHelper.GenerateCsv(updatedData, new string[] { }, null, "TransactionReports", true);
            return File(bytes, fileformate, filename);
        }

        [HttpGet("ExportTransactiontoexcell")]
        public async Task<IActionResult> ExportTransactiontoexcell([FromQuery] RemitTxnReportFilter request)
        {
            request.Export = 1;
            request.PartnerCode = "admin";
            var result = await _partnerReportServices.AdminExportRemitTxnReportAsync(request);

            var data = result.Items;
            List<DataTable> dataTables = await IEnumerableExtensions.ToDataTablesAsync<AdminExportRemitTxnReport>(data, 500000);
            var (excelFileByteArr, fileFormat, fileName) = await ExportHelper.ToExcelAsync(dataTables, "TransactionReports", true);

            return File(excelFileByteArr, fileFormat, fileName);
        }


        [HttpGet("ExportTransactiontoPdf")]
        public async Task<FileContentResult> ExportTransactiontoPdf([FromQuery] RemitTxnReportFilter request)
        {

            request.Export = 1;
            request.PartnerCode = "admin";
            var result = await _partnerReportServices.AdminExportRemitTxnReportAsync(request);
            var data = result;

            var (bytedata, format) = await ExportHelper.TopdfAsync<AdminExportRemitTxnReport>(data, "Transactional Reports");
            return File(bytedata, format, "txnreport.pdf");

        }

        #region CashAgentReport

        [HttpGet]
        public async Task<IActionResult> AgentCommissionTransactionReport([FromQuery] AgentCommissionFilter reportFilter)
        {
            var districtDdl = await _commonddlServices.GetAllDistrictddl();
            ViewBag.District = new SelectList(districtDdl, "value", "Text");
            var result = await _agentReportService.GetAgentCommissionTxnReportAdminAsync(reportFilter);
            ViewBag.CommissionTransactionFilter = result.Items.FirstOrDefault();
            if (WebHelper.IsAjaxRequest(Request))
                return await Task.FromResult(PartialView("_AgentCommissionTransactionReport", result));
            return await Task.FromResult(View(result));
        }

        [HttpGet]
        public async Task<IActionResult> AgentCommissionTransactionReportDetail([FromQuery] AgentCommissionFilter reportFilter)
        {
            var districtDdl = await _commonddlServices.GetAllDistrictddl();
            ViewBag.District = new SelectList(districtDdl, "value", "Text");
            var result = await _agentReportService.GetAgentCommissionTxnReportDetailAdminAsync(reportFilter);
            ViewBag.CommissionTransactionFilter = result.Items.FirstOrDefault();
            if (WebHelper.IsAjaxRequest(Request))
                return await Task.FromResult(PartialView("_AgentCommissionTransactionReportDetail", result));
            return await Task.FromResult(View(result));
        }

        [HttpGet]
        public async Task<IActionResult> ViewCommissionDetail(string TransactionId)
        {
            var filter = new AgentCommissionFilter
            {
                TransactionId = TransactionId
            };
            var commissionDetail = await _agentReportService.GetAgentCommissionTxnReportAdminAsync(filter);
            return PartialView("_ViewCommissionDetail", commissionDetail.Items.FirstOrDefault());
        }

        [HttpGet]
        public async Task<IActionResult> ViewCommissionDetails(string TransactionId)
        {
            var filter = new AgentCommissionFilter
            {
                TransactionId = TransactionId
            };
            var commissionDetail = await _agentReportService.GetAgentCommissionTxnReportDetailAdminAsync(filter);
            return PartialView("_ViewCommissionDetails", commissionDetail.Items.FirstOrDefault());
        }

        [HttpGet("ExportAgentCommissionTransactionDetailToCsv")]
        public async Task<IActionResult> ExportAgentCommissionTransactionDetailToCsv([FromQuery] AgentCommissionFilter request)
        {
            request.Export = 1;
            var result = await _agentReportService.GetAgentCommissionTxnReportDetailAdminAsync(request);
            var data = result.Items;
            var (bytes, fileformate, filename) = ExportHelper.GenerateCsv(data, new string[] { }, null, "AgentCommissionTransactionReports", true);
            return File(bytes, fileformate, filename);
        }

        [HttpGet("ExportAgentCommissionTransactionDetailToExcel")]
        public async Task<IActionResult> ExportAgentCommissionTransactionDetailToExcel([FromQuery] AgentCommissionFilter request)
        {
            request.Export = 1;
            var result = await _agentReportService.GetAgentCommissionTxnReportDetailAdminAsync(request);
            var data = result.Items;
            List<DataTable> dataTables = await IEnumerableExtensions.ToDataTablesAsync(data, 500000);
            var (excelFileByteArr, fileFormat, fileName) = await ExportHelper.ToExcelAsync(dataTables, "AgentCommissionTransactionReports", true);
            return File(excelFileByteArr, fileFormat, fileName);
        }

        [HttpGet("ExportAgentCommissionTransactionDetailToPdf")]
        public async Task<FileContentResult> ExportAgentCommissionTransactionDetailToPdf([FromQuery] AgentCommissionFilter request)
        {
            request.Export = 1;
            var result = await _agentReportService.GetAgentCommissionTxnReportDetailAdminAsync(request);
            var data = result;
            var (bytedata, format) = await ExportHelper.TopdfAsync(data, "AgentCommissionTransactionReports");
            return File(bytedata, format, "AgentCommissionTransactionreport.pdf");
        }

        [HttpGet("ExportAgentCommissionTransactionToCsv")]
        public async Task<IActionResult> ExportAgentCommissionTransactionToCsv([FromQuery] AgentCommissionFilter request)
        {
            request.Export = 1;
            var result = await _agentReportService.GetAgentCommissionTxnReportAdminAsync(request);
            var data = result.Items;
            var (bytes, fileformate, filename) = ExportHelper.GenerateCsv(data, new string[] { }, null, "AgentCommissionTransactionReports", true);
            return File(bytes, fileformate, filename);
        }

        [HttpGet("ExportAgentCommissionTransactionToExcel")]
        public async Task<IActionResult> ExportAgentCommissionTransactionToExcel([FromQuery] AgentCommissionFilter request)
        {
            request.Export = 1;
            var result = await _agentReportService.GetAgentCommissionTxnReportAdminAsync(request);
            var data = result.Items;
            List<DataTable> dataTables = await IEnumerableExtensions.ToDataTablesAsync(data, 500000);
            var (excelFileByteArr, fileFormat, fileName) = await ExportHelper.ToExcelAsync(dataTables, "AgentCommissionTransactionReports", true);
            return File(excelFileByteArr, fileFormat, fileName);
        }

        [HttpGet("ExportAgentCommissionTransactionToPdf")]
        public async Task<FileContentResult> ExportAgentCommissionTransactionToPdf([FromQuery] AgentCommissionFilter request)
        {
            request.Export = 1;
            var result = await _agentReportService.GetAgentCommissionTxnReportAdminAsync(request);
            var data = result;
            var (bytedata, format) = await ExportHelper.TopdfAsync(data, "AgentCommissionTransactionReports");
            return File(bytedata, format, "AgentCommissionTransactionreport.pdf");
        }

        #endregion
        //Settlement Report

        [HttpGet]
        public async Task<IActionResult> RemitSettlementReport([FromQuery] RemitSettlementReportFilter reportFilter)
        {
            var actions = await _rMPService.GetActionPermissionListAsync("Reports");
            ViewBag.actions = actions;

            var currencyddl = await _commonddlServices.GetCurrencyddl();
            ViewBag.SCurrency = new SelectList(currencyddl, "value", "Text");
            ViewBag.DCurrency = new SelectList(currencyddl, "value", "Text");
            reportFilter.PartnerCode = "admin";
            var result = await _partnerReportServices.GetRemitSettlementReportAsync(reportFilter);
            ViewBag.TransactionFilter = result.Items.FirstOrDefault();
            if (WebHelper.IsAjaxRequest(Request))
                return await Task.FromResult(PartialView("_RemitSettlementReport", result));
            return await Task.FromResult(View(result));
        }
        [HttpGet("ExportRemitSettlementToCsv")]
        public async Task<IActionResult> ExportRemitSettlementToCsv([FromQuery] RemitSettlementReportFilter request)
        {
            request.Export = 1;
            request.PartnerCode = "admin";
            var result = await _partnerReportServices.ExportAdminSettlementReportAsync(request);
            var data = result.Items;
            var (bytes, fileformate, filename) = ExportHelper.GenerateCsv(data, new string[] { }, null, "RemitSettlementReports", true);
            return File(bytes, fileformate, filename);
        }

        [HttpGet("ExportRemitSettlementtoexcell")]
        public async Task<IActionResult> ExportRemitSettlementtoexcell([FromQuery] RemitSettlementReportFilter request)
        {
            request.Export = 1;
            request.PartnerCode = "admin";
            var result = await _partnerReportServices.ExportAdminSettlementReportAsync(request);
            var data = result.Items;
            List<DataTable> dataTables = await IEnumerableExtensions.ToDataTablesAsync<AdminExportRemitSettlementReport>(data, 500000);
            var (excelFileByteArr, fileFormat, fileName) = await ExportHelper.ToExcelAsync(dataTables, "RemitSettlementReports", true);

            return File(excelFileByteArr, fileFormat, fileName);
        }


        [HttpGet("ExportRemitSettlementtoPdf")]
        public async Task<FileContentResult> ExportRemitSettlementtoPdf([FromQuery] RemitSettlementReportFilter request)
        {

            request.Export = 1;
            request.PartnerCode = "admin";
            var result = await _partnerReportServices.ExportAdminSettlementReportAsync(request);
            var data = result;

            var (bytedata, format) = await ExportHelper.TopdfAsync<AdminExportRemitSettlementReport>(data, "RemitSettlemental Reports");
            return File(bytedata, format, "RemitSettlementreport.pdf");

        }
        //Settlement Details Report

        [HttpGet]
        public async Task<IActionResult> SettlementReport([FromQuery] RemitSettlementReportFilter reportFilter)
        {
            var actions = await _rMPService.GetActionPermissionListAsync("Reports");
            ViewBag.actions = actions;

            var currencyddl = await _commonddlServices.GetCurrencyddl();
            var agentListddl = await _commonddlServices.GetAgentListddl();
            var districtDdl = await _commonddlServices.GetAllDistrictddl();
            var paymentTypeddl = await _commonddlServices.GetPaymentTypeddl();
            ViewBag.DistrictDdl = new SelectList(districtDdl, "value", "Text");
            ViewBag.paymentTypeddl = new SelectList(paymentTypeddl, "lookup", "Text");
            ViewBag.AgentListDdl = new SelectList(agentListddl, "value", "Text");
            ViewBag.SCurrency = new SelectList(currencyddl, "value", "Text");
            ViewBag.DCurrency = new SelectList(currencyddl, "value", "Text");
            reportFilter.PartnerCode = "admin";
            var result = await _partnerReportServices.GetSettlementReportAsync(reportFilter);

            ViewBag.TransactionFilter = result.Items.FirstOrDefault();
            if (WebHelper.IsAjaxRequest(Request))
                return await Task.FromResult(PartialView("_SettlementReport", result));
            return await Task.FromResult(View(result));
        }
        [HttpGet("ExportSettlementToCsv")]
        public async Task<IActionResult> ExportSettlementToCsv([FromQuery] RemitSettlementReportFilter request)
        {
            request.Export = 1;
            request.PartnerCode = "admin";
            var result = await _partnerReportServices.ExportAdminRemitSettlementReportAsync(request);
            var data = result.Items;
            var updatedData = data.Select(r =>
            {
                r.RecipientAccountNumber = $"=\"{r.RecipientAccountNumber}\"";
                r.TransactionId = $"=\"{r.TransactionId}\"";
                r.AgentTrackerId = $"=\"{r.AgentTrackerId}\"";
                r.GatewayTxnId = $"=\"{r.GatewayTxnId}\"";
                r.PartnerTrackerId = $"=\"{r.PartnerTrackerId}\"";
                return r;
            });
            var (bytes, fileformate, filename) = ExportHelper.GenerateCsv(updatedData, new string[] { }, null, "SettlementReports", true);
            return File(bytes, fileformate, filename);
        }

        [HttpGet("ExportSettlementtoexcell")]
        public async Task<IActionResult> ExportSettlementtoexcell([FromQuery] RemitSettlementReportFilter request)
        {
            request.Export = 1;
            request.PartnerCode = "admin";
            var result = await _partnerReportServices.ExportAdminRemitSettlementReportAsync(request);
            var data = result.Items;
            List<DataTable> dataTables = await IEnumerableExtensions.ToDataTablesAsync<AdminExportRemitSettlementReport>(data, 500000);
            var (excelFileByteArr, fileFormat, fileName) = await ExportHelper.ToExcelAsync(dataTables, "SettlementReports", true);

            return File(excelFileByteArr, fileFormat, fileName);
        }


        [HttpGet("ExportSettlementtoPdf")]
        public async Task<FileContentResult> ExportSettlementtoPdf([FromQuery] RemitSettlementReportFilter request)
        {

            request.Export = 1;
            request.PartnerCode = "admin";
            var result = await _partnerReportServices.ExportAdminRemitSettlementReportAsync(request);
            var data = result;

            var (bytedata, format) = await ExportHelper.TopdfAsync<AdminExportRemitSettlementReport>(data, "Settlemental Reports");
            return File(bytedata, format, "Settlementreport.pdf");

        }

        [HttpGet]
        public async Task<IActionResult> RemitFeeTransactionReport([FromQuery] RemitTxnReportFilter reportFilter)
        {
            var currencyddl = await _commonddlServices.GetCurrencyddl();
            ViewBag.SCurrency = new SelectList(currencyddl, "value", "Text");
            ViewBag.DCurrency = new SelectList(currencyddl, "value", "Text");
            reportFilter.PartnerCode = "admin";
            var result = await _partnerReportServices.GetRemitFeeTxnReportAsync(reportFilter);
            ViewBag.TransactionFilter = result.Items.FirstOrDefault();
            if (WebHelper.IsAjaxRequest(Request))
                return await Task.FromResult(PartialView("_RemitFeeTransactionReport", result));
            return await Task.FromResult(View(result));
        }


        [HttpGet("ExportRemitFeeTransactionToCsv")]
        public async Task<IActionResult> ExportRemitFeeTransactionToCsv([FromQuery] RemitTxnReportFilter request)
        {
            request.Export = 1;
            request.PartnerCode = "admin";
            var result = await _partnerReportServices.GetRemitFeeTxnReportAsync(request);
            var data = result.Items;
            var (bytes, fileformate, filename) = ExportHelper.GenerateCsv(data, new string[] { }, null, "RemitFeeTransactionReports", true);
            return File(bytes, fileformate, filename);
        }

        [HttpGet("ExportRemitFeeTransactiontoexcell")]
        public async Task<IActionResult> ExportRemitFeeTransactiontoexcell([FromQuery] RemitTxnReportFilter request)
        {
            request.Export = 1;
            request.PartnerCode = "admin";
            var result = await _partnerReportServices.GetRemitFeeTxnReportAsync(request);
            var data = result.Items;
            List<DataTable> dataTables = await IEnumerableExtensions.ToDataTablesAsync<RemitFeeTxnReport>(data, 500000);
            var (excelFileByteArr, fileFormat, fileName) = await ExportHelper.ToExcelAsync(dataTables, "RemitFeeTransactionReports", true);

            return File(excelFileByteArr, fileFormat, fileName);
        }


        [HttpGet("ExportRemitFeeTransactiontoPdf")]
        public async Task<FileContentResult> ExportRemitFeeTransactiontoPdf([FromQuery] RemitTxnReportFilter request)
        {

            request.Export = 1;
            request.PartnerCode = "admin";
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
            reportFilter.PartnerCode = "admin";
            var result = await _partnerReportServices.GetRemitCommissionTxnReportAsync(reportFilter);
            ViewBag.CommissionTransactionFilter = result.Items.FirstOrDefault();
            if (WebHelper.IsAjaxRequest(Request))
                return await Task.FromResult(PartialView("_RemitCommissionTransactionReport", result));
            return await Task.FromResult(View(result));
        }

        [HttpGet("ExportRemitCommissionTransactionToCsv")]
        public async Task<IActionResult> ExportRemitCommissionTransactionToCsv([FromQuery] CommissionTransactionFilter request)
        {
            request.Export = 1;
            request.PartnerCode = "admin";
            var result = await _partnerReportServices.GetRemitCommissionTxnReportAsync(request);
            var data = result.Items;
            var (bytes, fileformate, filename) = ExportHelper.GenerateCsv(data, new string[] { }, null, "RemitCommissionTransactionReports", true);
            return File(bytes, fileformate, filename);
        }

        [HttpGet("ExportRemitCommissionTransactiontoexcell")]
        public async Task<IActionResult> ExportRemitCommissionTransactiontoexcell([FromQuery] CommissionTransactionFilter request)
        {
            request.Export = 1;
            request.PartnerCode = "admin";
            var result = await _partnerReportServices.GetRemitCommissionTxnReportAsync(request);
            var data = result.Items;
            List<DataTable> dataTables = await IEnumerableExtensions.ToDataTablesAsync<CommisionTransction>(data, 500000);
            var (excelFileByteArr, fileFormat, fileName) = await ExportHelper.ToExcelAsync(dataTables, "RemitCommissionTransactionReports", true);

            return File(excelFileByteArr, fileFormat, fileName);
        }


        [HttpGet("ExportRemitCommissionTransactiontoPdf")]
        public async Task<FileContentResult> ExportRemitCommissionTransactiontoPdf([FromQuery] CommissionTransactionFilter request)
        {

            request.Export = 1;
            request.PartnerCode = "admin";
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
        public async Task<IActionResult> TransactionStatus(string TransactionId)
        {
            var senderdetail = await _transactionServices.GetTransactionStatus(TransactionId);
            return PartialView("_TransactionStatus", senderdetail);
        }

        [HttpGet]
        public async Task<IActionResult> ReciverDetail(string TransactionId)
        {
            var recipientsdetail = await _transactionServices.GetRecipientByTxnId(TransactionId);
            if (recipientsdetail.First().PaymentType.ToUpper() == "CASH")
            {
                recipientsdetail.First().WalletNumber = "CASH";
            }
            return PartialView("_ReceiverDetails", recipientsdetail);
        }

        [HttpGet]
        public async Task<IActionResult> ViewDetail(string TransactionId)
        {
            var transectiondetail = await _transactionServices.GetTxnById(TransactionId);
            return PartialView("_ViewTransactionDetail", transectiondetail);
        }

        [HttpGet]
        public async Task<IActionResult> PaymentDetail(string TransactionId)
        {
            var transectiondetail = await _transactionServices.PaymentDetailsByTxnIdAsync(TransactionId);
            return PartialView("_PaymentTypes", transectiondetail);
        }
        [HttpGet]
        public async Task<IActionResult> ManageAccountDetail(string TransactionId)
        {
            try
            {
                if (string.IsNullOrEmpty(TransactionId))
                {
                    return PartialView("Error");
                }
                IEnumerable<Commondropdown> bankListDdl = await _commonddlServices.GetBankddl();
                ViewBag.BankListDdl = new SelectList(bankListDdl, "lookup", "Text");

                var managedetails = await _transactionServices.ManageDetailByTxnIdAsync(TransactionId);
                var mappedData = _mapper.Map<ReceiverAccountDetailsVM>(managedetails);
                mappedData.PaymentType = mappedData.PaymentType.ToUpper();

                return PartialView("_ManageAccountDetail", mappedData);
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        [HttpPost, ValidateAntiForgeryToken]
        [LogUserActivity("updated a receiver account details")]
        public async Task<IActionResult> ManageAccountDetail(ReceiverAccountDetailsVM viewmodel)
        {
            IEnumerable<Commondropdown> bankListDdl = await _commonddlServices.GetBankddl();
            ViewBag.BankListDdl = new SelectList(bankListDdl, "lookup", "Text");

            if (string.IsNullOrEmpty(viewmodel.TransactionId))
            {
                _notify.Error("Unable to update detail");
                return Ok();
            }

            if (viewmodel.PaymentStatusCode == "53")
            {
                _notify.Error("Invalid action! Transaction already cancelled.");
                return Ok();
            }

            if (viewmodel.PaymentStatusCode == "55")
            {
                _notify.Error("Invalid action! Transaction already completed successfully.");
                return Ok();
            }

            if (viewmodel.GatewayTxnId.IsNotNullOrEmpty())
            {
                _notify.Error("Invalid action! Transaction already completed successfully.");
                return Ok();
            }

            var response = await _transactionServices.UpdateAccountDetails(viewmodel);

            if (response.StatusCode == 200)
            {
                _notify.Success(response.MsgText);
                return Ok();
            }
            _notify.Error(response.MsgText);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> EditCashoutDetails(string TransactionId)
        {
            try
            {
                if (string.IsNullOrEmpty(TransactionId))
                {
                    return PartialView("Error");
                }
                IEnumerable<Commonddl> districtDdl = await _commonddlServices.GetAllDistrictddl();
                ViewBag.Districtddl = new SelectList(districtDdl, "value", "Text");

                var managedetails = await _transactionServices.ReceiverDetailsCashoutByTxnIdAsync(TransactionId);
                var mappedData = _mapper.Map<ReceiverAccountDetailsCashoutVM>(managedetails);

                return PartialView("EditCashoutDetails", mappedData);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        [LogUserActivity("updated a receiver account details")]
        public async Task<IActionResult> EditCashoutDetails(ReceiverAccountDetailsCashoutVM viewmodel)
        {
            IEnumerable<Commonddl> districtDdl = await _commonddlServices.GetAllDistrictddl();
            ViewBag.Districtddl = new SelectList(districtDdl, "value", "Text");

            if (string.IsNullOrEmpty(viewmodel.TransactionId))
            {
                _notify.Error("Unable to update detail");
                return Ok();
            }

            var response = await _transactionServices.UpdateReceiverCashoutDetails(viewmodel);

            if (response.StatusCode == 200)
            {
                _notify.Success(response.MsgText);
                return Ok();
            }
            _notify.Error(response.MsgText);
            return Ok();
        }

        public async Task<IActionResult> PayoutTransaction(string TransactionId)
        {
            RemitTxnReport report = new RemitTxnReport();
            report.TransactionId = TransactionId;
            return PartialView("_PayoutAdminTransaction", report);
        }
        [HttpPost]
        [LogUserActivity("made payout transaction")]
        public async Task<IActionResult> PayoutTransaction(RemitTxnReport model)
        {
            var managedetails = await _transactionServices.GetTransactionParameterByTxnId(model.TransactionId);
            if (managedetails != null)
            {
                if (managedetails.PaymentStatusCode == "53")
                {
                    _notify.Error("Invalid action! Transaction already cancelled.");
                    return Ok();
                }

                if (managedetails.PaymentStatusCode == "55")
                {
                    _notify.Error("Invalid action! Transaction already completed success.");
                    return Ok();
                }

                if (managedetails.PaymentType.ToUpper() == "CASH")
                {
                    _notify.Error("Invalid action! Admin payout cannot be done for Cash transaction.");
                    return Ok();
                }

                if (!string.IsNullOrWhiteSpace(managedetails.ComplianceStatusCode) && !managedetails.ComplianceReleased)
                {
                    _notify.Error("Invalid action! Transaction is in Compliance but not released.");
                    return Ok();
                }

                var txnAddResult = await _partnerSendTxnsService.AddByAdminTransactionAsync(managedetails);

                if (managedetails.PaymentType.ToUpper() == "WALLET")
                {
                    if (txnAddResult.Errors.Count() > 0 && txnAddResult.ResultCode == 400)
                    {
                        _notify.Error(txnAddResult.Errors.First().ToString());
                        return Ok();
                    }
                    else if (txnAddResult.ResultCode == 1 && !string.IsNullOrEmpty(txnAddResult.Message))
                    {
                        _notify.Success(txnAddResult.Message);
                        return Ok();
                    }
                    else if (!txnAddResult.Success && txnAddResult.ResultCode == 1)
                    {
                        _notify.Error(txnAddResult.Errors.First().ToString());
                        return Ok();
                    }
                    else
                    {
                        _notify.Error("Something went wrong,please verify account details and try again !");
                        return Ok();
                    }
                }
                else
                {
                    if (!txnAddResult.Success && txnAddResult.ResultCode != 1)
                    {
                        if (string.IsNullOrEmpty(txnAddResult.Errors[0]))
                        {
                            _notify.Error("Something went wrong,please verify account details and try again !");
                            return Ok();
                        }
                        _notify.Error(txnAddResult.Errors.First().ToString());
                        return Ok();
                    }
                    else if (txnAddResult.ResultCode == 1 && !string.IsNullOrEmpty(txnAddResult.Message))
                    {
                        _notify.Success(txnAddResult.Message);
                        return Ok();
                    }
                    else
                    {
                        _notify.Error("Something went wrong,please verify account details and try again !");
                        return Ok();
                    }
                }
            }
            _notify.Error("Something went wrong,please verify account details and try again !");
            return Ok();
        }

        public async Task<IActionResult> CheckStatusTransaction(string TransactionId)
        {
            RemitTxnReport report = new RemitTxnReport();
            report.TransactionId = TransactionId;
            return PartialView("_AdminCheckStatus", report);
        }
        [HttpPost, LogUserActivity("checkout payout transaction")]
        public async Task<IActionResult> CheckStatusTransaction(RemitTxnReport model)
        {
            var managedetails = await _transactionServices.GetTransactionParameterByTxnId(model.TransactionId);
            if (managedetails != null)
            {
                var txnAddResult = await _partnerSendTxnsService.CheckStatusTransactionAsync(managedetails);
                if (txnAddResult.ResultCode != 0 && !txnAddResult.Success)
                {
                    _notify.Error(txnAddResult.Errors.First().ToString());
                    return Ok();
                }
                else
                {
                    _notify.Success(txnAddResult.Errors.First().ToString());
                    return Ok();
                }
            }
            return Ok();
        }


        public async Task<IActionResult> CancelTransaction(string TransactionId)
        {
            RemitTxnReport report = new RemitTxnReport();
            report.TransactionId = TransactionId;
            return await Task.FromResult(PartialView("_AdminCancelTransaction", report));
        }

        [HttpPost, LogUserActivity("cancelled transaction by admin")]
        public async Task<IActionResult> CancelTransaction(RemitTxnReport model)
        {
            if (string.IsNullOrWhiteSpace(model.Remarks))
            {
                _notify.Error("Remarks is required!!!");
                return RedirectToAction("TransactionAdminReportsIndex", "Reports");
            }

            var managedetails = await _transactionServices.GetTransactionParameterByTxnId(model.TransactionId);
            if (managedetails != null)
            {
                if (managedetails.PaymentStatusCode == "53")
                {
                    _notify.Error("Invalid action! Transaction already cancelled.");
                    return RedirectToAction("TransactionAdminReportsIndex", "Reports");
                }

                if (managedetails.PaymentStatusCode == "55")
                {
                    _notify.Error("Invalid action! Transaction already completed success.");
                    return RedirectToAction("TransactionAdminReportsIndex", "Reports");
                }

            }

            var status = await _partnerSendTxnsService.cancelledTransactionAsysnc(model);
            if (status.StatusCode == 200)
            {
                _notify.Success(status.MsgText);
                return RedirectToAction("TransactionAdminReportsIndex", "Reports");
            }
            else
            {
                _notify.Error(status.MsgText);
                return RedirectToAction("TransactionAdminReportsIndex", "Reports");
            }
        }

        public async Task<IActionResult> InwardRemittanceCountryWiseReport([FromQuery] InwardRemitanceFilterDto model)
        {
            model.StartDate = string.IsNullOrEmpty(model.StartDateBS) ? model.StartDate : model.StartDateBS;
            model.EndDate = string.IsNullOrEmpty(model.EndDateBS) ? model.EndDate : model.EndDateBS;
            var (listsum, list) = await _partnerReportServices.GetInwardRemitanceReport(model);
            if (model.Export == 1)
            {
                string filename = $"{DateTime.UtcNow:yyyyMMddHHmmssffff}_{Guid.NewGuid():N}" + "_" + "excelexport.xlsx";
                return File(await ExportToExcelCountryWiseInwardReport(filename, model), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Anextures_3-4.xlsx");
            }
            if (list != null)
                ViewData["grid"] = listsum;
            if (WebHelper.IsAjaxRequest(Request))
                return await Task.FromResult(PartialView("_InwardRemittanceReport", list));
            return await Task.FromResult(View(list));
        }

        public async Task<IActionResult> InwardRemittanceCompanyWiseReport([FromQuery] InwardRemitanceFilterDto model)
        {
            model.StartDate = string.IsNullOrEmpty(model.StartDateBS) ? model.StartDate : model.StartDateBS;
            model.EndDate = string.IsNullOrEmpty(model.EndDateBS) ? model.EndDate : model.EndDateBS;
            var (listsum, list) = await _partnerReportServices.GetInwardRemitanceCompanyWiseReport(model);
            if (model.Export == 1)
            {
                string filename = $"{DateTime.UtcNow:yyyyMMddHHmmssffff}_{Guid.NewGuid():N}" + "_" + "excelexport.xlsx";
                return File(await ExportToExcelCompanyWiseInwardReport(filename, model), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Anextures 5.xlsx");
            }
            if (list != null)
                ViewData["grid"] = listsum;
            if (WebHelper.IsAjaxRequest(Request))
                return await Task.FromResult(PartialView("_InwardRemittanceCompanyWiseReport", list));
            return await Task.FromResult(View(list));
        }

        public async Task<IActionResult> RemittanceCompanySelfOrAgentOrSubAgentInwardDetails([FromQuery] InwardRemitanceFilterDto model)
        {
            model.StartDate = string.IsNullOrEmpty(model.StartDateBS) ? model.StartDate : model.StartDateBS;
            model.EndDate = string.IsNullOrEmpty(model.EndDateBS) ? model.EndDate : model.EndDateBS;
            var (listsum, list) = await _partnerReportServices.GetRemitanceCompanySelfOrAgentOrSubAgentInwardDetails(model);
            if (model.Export == 1)
            {
                string filename = $"{DateTime.UtcNow:yyyyMMddHHmmssffff}_{Guid.NewGuid():N}" + "_" + "Anexture 1.xlsx";
                return File(await ExportToExcelAgentWiseInwardReport(filename, model), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Anextures 5.xlsx");
            }
            if (list != null)
                ViewData["grid"] = listsum;
            //if (WebHelper.IsAjaxRequest(Request))
            //    return await Task.FromResult(PartialView("_GetStaffList", list));
            if (WebHelper.IsAjaxRequest(Request))
                return await Task.FromResult(PartialView("_InwardRemittanceAgentWiseReport", list));
            return await Task.FromResult(View(list));
        }

        public async Task<IActionResult> ActionTakenByTheRemittanceReport([FromQuery] InwardRemitanceFilterDto model)
        {
            model.StartDate = string.IsNullOrEmpty(model.StartDateBS) ? model.StartDate : model.StartDateBS;
            model.EndDate = string.IsNullOrEmpty(model.EndDateBS) ? model.EndDate : model.EndDateBS;
            var list = await _partnerReportServices.GetActionTakenbyTheRemitanceReport(model);
            if (model.Export == 1)
            {
                string filename = $"{DateTime.UtcNow:yyyyMMddHHmmssffff}_{Guid.NewGuid():N}" + "_" + "Anexture 2.xlsx";
                return File(await ExportToExcelActionTakenbyTheRemitanceReport(filename, model), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Anextures 2.xlsx");
            }
            if (WebHelper.IsAjaxRequest(Request))
                return await Task.FromResult(PartialView("_ActionTakenByRemittanceReport", list));
            return await Task.FromResult(View(list));
        }
        #region export using existing 

        [HttpGet("ExportToExcelCountryWiseInwardReport")]
        public async Task<byte[]> ExportToExcelCountryWiseInwardReport(string excelFile, InwardRemitanceFilterDto model)
        {
            //string filePath = "D:\\Projects\\MPMT\\src\\Mpmt.Web\\wwwroot\\ReportFile\\Anextures_3-4.xlsx";
            string fPaths = "wwwroot\\ReportFile\\Anextures_3-4.xlsx";
            string filePath = Path.GetFullPath(fPaths);
            var (listsum, list) = await _partnerReportServices.GetInwardRemitanceReport(model);
            FileInfo fileInfo = new FileInfo(filePath);
            using (ExcelPackage package = new ExcelPackage(new System.IO.FileInfo(filePath)))
            {
                var worksheetcount = package.Workbook.Worksheets.Count();
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                int countList = list.Items.Count();
                int i = 0;
                foreach (var item in list.Items)
                {
                    worksheet.Cells["A" + (i + 4)].Value = i + 1;
                    worksheet.Cells["B" + (i + 4)].Value = item.CountryCodeAlpha3;
                    worksheet.Cells["C" + (i + 4)].Value = item.CountryName;
                    worksheet.Cells["D" + (i + 4)].Value = item.NoOfTransaction;
                    worksheet.Cells["E" + (i + 4)].Value = item.TotalAmountUsd;
                    worksheet.Cells["F" + (i + 4)].Value = item.TotalAmountNpr;

                    for (int j = 1; j <= 6; j++)
                    {
                        var cell = worksheet.Cells[i + 4, j];
                        var border = cell.Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    }
                    i++;
                }
                worksheet.Cells[$"A{countList + 3 + 1}:C{countList + 3 + 1}"].Merge = true;
                worksheet.Cells["D" + (countList + 3 + 1)].Value = listsum.GrandTotalTxn;
                worksheet.Cells["E" + (countList + 3 + 1)].Value = listsum.GrandTotalAmountUSD;
                worksheet.Cells["F" + (countList + 3 + 1)].Value = listsum.GrandTotalAmountNPR;
                worksheet.Cells["A" + (countList + 3 + 1)].Value = "Total:";

                using (var range = worksheet.Cells[$"A{countList + 3 + 1}:F{countList + 3 + 1}"])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                }

                //string outputFilePath = "D:\\Projects\\MPMT\\src\\Mpmt.Web\\wwwroot\\ReportFile\\ReportExport\\" + excelFile;
                //package.SaveAs(new System.IO.FileInfo(outputFilePath));
                return package.GetAsByteArray();

            }
        }
        [HttpGet("ExportToExcelCompanyWiseInwardReport")]
        public async Task<byte[]> ExportToExcelCompanyWiseInwardReport(string excelFile, InwardRemitanceFilterDto model)
        {
            //string filePath = "D:\\Projects\\MPMT\\src\\Mpmt.Web\\wwwroot\\ReportFile\\Anextures_5.xlsx";
            string fPaths = "wwwroot\\ReportFile\\Anextures_5.xlsx";
            string filePath = Path.GetFullPath(fPaths);
            var (listsum, list) = await _partnerReportServices.GetInwardRemitanceCompanyWiseReport(model);
            FileInfo fileInfo = new FileInfo(filePath);
            using (ExcelPackage package = new ExcelPackage(new System.IO.FileInfo(filePath)))
            {
                var worksheetcount = package.Workbook.Worksheets.Count();
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                int countList = list.Items.Count();
                int i = 0;
                foreach (var item in list.Items)
                {
                    worksheet.Cells["A" + (i + 5)].Value = i + 1;
                    worksheet.Cells["B" + (i + 5)].Value = item.PartnerOrgName;
                    worksheet.Cells["C" + (i + 5)].Value = item.CountryName;
                    worksheet.Cells["D" + (i + 5)].Value = item.PrincipalOrBranch;
                    worksheet.Cells["E" + (i + 5)].Value = item.NoOfTransaction;
                    worksheet.Cells["F" + (i + 5)].Value = item.TotalAmountUsd;
                    worksheet.Cells["G" + (i + 5)].Value = item.TotalAmountNpr;
                    for (int j = 1; j <= 7; j++)
                    {
                        var cell = worksheet.Cells[i + 5, j];
                        var border = cell.Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    }
                    i++;
                }
                worksheet.Cells[$"A{countList + 4 + 1}:C{countList + 4 + 1}"].Merge = true;
                worksheet.Cells["E" + (countList + 4 + 1)].Value = listsum.GrandTotalTxn;
                worksheet.Cells["F" + (countList + 4 + 1)].Value = listsum.GrandTotalAmountUSD;
                worksheet.Cells["G" + (countList + 4 + 1)].Value = listsum.GrandTotalAmountNPR;
                worksheet.Cells["A" + (countList + 4 + 1)].Value = "Total:";

                using (var range = worksheet.Cells[$"A{countList + 4 + 1}:G{countList + 4 + 1}"])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                }
                //string outputFilePath = "D:\\Projects\\MPMT\\src\\Mpmt.Web\\wwwroot\\ReportFile\\ReportExport\\" + excelFile;
                //package.SaveAs(new System.IO.FileInfo(outputFilePath));
                return package.GetAsByteArray();
            }
        }
        [HttpGet("ExportToExcelCompanyWiseInwardReport")]
        public async Task<byte[]> ExportToExcelAgentWiseInwardReport(string excelFile, InwardRemitanceFilterDto model)
        {
            //string filePath = "D:\\Projects\\MPMT\\src\\Mpmt.Web\\wwwroot\\ReportFile\\Anexture_1.xlsx";
            string fPaths = "wwwroot\\ReportFile\\Anexture_1.xlsx";
            string filePath = Path.GetFullPath(fPaths);

            var (listsum, list) = await _partnerReportServices.GetRemitanceCompanySelfOrAgentOrSubAgentInwardDetails(model);

            FileInfo fileInfo = new FileInfo(filePath);
            using (ExcelPackage package = new ExcelPackage(new System.IO.FileInfo(filePath)))
            {
                var worksheetcount = package.Workbook.Worksheets.Count();
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                int countList = list.Items.Count();
                int i = 0;
                foreach (var item in list.Items)
                {
                    worksheet.Cells["A" + (i + 5)].Value = i + 1;
                    worksheet.Cells["B" + (i + 5)].Value = item.Agent;
                    worksheet.Cells["C" + (i + 5)].Value = item.Province;
                    worksheet.Cells["D" + (i + 5)].Value = item.District;
                    worksheet.Cells["E" + (i + 5)].Value = item.LocalLevel;
                    worksheet.Cells["F" + (i + 5)].Value = item.Pan;
                    worksheet.Cells["G" + (i + 5)].Value = item.TotalAmountNPR;

                    for (int j = 1; j <= 7; j++)
                    {
                        var cell = worksheet.Cells[i + 5, j];
                        var border = cell.Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    }

                    i++;
                }
                worksheet.Cells[$"A{countList + 4 + 1}:F{countList + 4 + 1}"].Merge = true;
                worksheet.Cells["G" + (countList + 4 + 1)].Value = listsum.GrandTotalAmountNPR;
                worksheet.Cells["A" + (countList + 4 + 1)].Value = "Total:";

                using (var range = worksheet.Cells[$"A{countList + 4 + 1}:G{countList + 4 + 1}"])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                }

                //string outputFilePath = "D:\\Projects\\MPMT\\src\\Mpmt.Web\\wwwroot\\ReportFile\\ReportExport\\" + excelFile;
                //package.SaveAs(new System.IO.FileInfo(outputFilePath));
                return package.GetAsByteArray();
            }
        }

        [HttpGet("ExportToExcelCompanyWiseInwardReport")]
        public async Task<byte[]> ExportToExcelActionTakenbyTheRemitanceReport(string excelFile, InwardRemitanceFilterDto model)
        {
            //string filePath = "D:\\Projects\\MPMT\\src\\Mpmt.Web\\wwwroot\\ReportFile\\Anexture_1.xlsx";
            string fPaths = "wwwroot\\ReportFile\\Anextures_2.xlsx";
            string filePath = Path.GetFullPath(fPaths);
            var list = await _partnerReportServices.GetActionTakenbyTheRemitanceReport(model);
            FileInfo fileInfo = new FileInfo(filePath);
            using (ExcelPackage package = new ExcelPackage(new System.IO.FileInfo(filePath)))
            {
                var worksheetcount = package.Workbook.Worksheets.Count();
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                int countList = list.Items.Count();
                int i = 0;
                foreach (var item in list.Items)
                {
                    worksheet.Cells["A" + (i + 5)].Value = i + 1;
                    worksheet.Cells["B" + (i + 5)].Value = item.Agent;
                    worksheet.Cells["C" + (i + 5)].Value = item.Province;
                    worksheet.Cells["D" + (i + 5)].Value = item.District;
                    worksheet.Cells["E" + (i + 5)].Value = item.LocalLevel;
                    worksheet.Cells["F" + (i + 5)].Value = item.RegisteredEngDate;
                    worksheet.Cells["G" + (i + 5)].Value = "";
                    worksheet.Cells["H" + (i + 5)].Value = "";
                    worksheet.Cells["I" + (i + 5)].Value = "";
                    worksheet.Cells["J" + (i + 5)].Value = "";

                    for (int j = 1; j <= 10; j++)
                    {
                        var cell = worksheet.Cells[i + 5, j];
                        var border = cell.Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    }

                    i++;
                }
                return package.GetAsByteArray();
            }
        }
        #endregion
    }

}
