using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Mpmt.Agent.Common;
using Mpmt.Agent.Filter;
using Mpmt.Core.Common;
using Mpmt.Core.Dtos.AgentReport;
using Mpmt.Core.Dtos.SuperAgent;
using Mpmt.Core.Extensions;
using Mpmt.Services.CashAgents;
using Mpmt.Services.Services.Common;
using System.Data;
using System.Security.Claims;

namespace Mpmt.Agent.Controllers;

[RolePremission]
public class AgentReportController :AgentBaseController
{
    private readonly ICommonddlServices _commonddlServices;
    private readonly IAgentReportService _agentReportService;
    private readonly ICashAgentUserService _cashAgentUserService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ClaimsPrincipal _loggedInUser;

    public AgentReportController(ICommonddlServices commonddlServices, IAgentReportService agentReportService, ICashAgentUserService cashAgentUserService, IHttpContextAccessor httpContextAccessor)
    {
        _commonddlServices = commonddlServices;
        _agentReportService = agentReportService;
        _cashAgentUserService = cashAgentUserService;
        _httpContextAccessor = httpContextAccessor;
        _loggedInUser = httpContextAccessor.HttpContext.User;
    }

    public IActionResult Index()
    {
        return View();
    }
    public IActionResult TransactionReport()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> AgentCommissionTransactionReport([FromQuery] AgentCommissionFilter reportFilter)
    {
        var districtDdl = await _commonddlServices.GetAllDistrictddl();
        ViewBag.District = new SelectList(districtDdl, "value", "Text");
        var result = await _agentReportService.GetAgentCommissionTxnReportAsync(reportFilter);
        ViewBag.CommissionTransactionFilter = result.Items.FirstOrDefault();
        if (WebHelper.IsAjaxRequest(Request))
            return await Task.FromResult(PartialView("_AgentCommissionTransactionReport", result));
        return await Task.FromResult(View(result));
    }

    [HttpGet]
    public async Task<IActionResult> SettlementReport([FromQuery] AgentSettlementFilter reportFilter)
    {
        var currencyddl = await _commonddlServices.GetCurrencyddl();
        var agentListddl = await _commonddlServices.GetAgentListddl();
        var districtDdl = await _commonddlServices.GetAllDistrictddl();
        ViewBag.DistrictDdl = new SelectList(districtDdl, "value", "Text");
        ViewBag.AgentListDdl = new SelectList(agentListddl, "value", "Text");
        ViewBag.SCurrency = new SelectList(currencyddl, "value", "Text");
        ViewBag.DCurrency = new SelectList(currencyddl, "value", "Text");
        var result = await _agentReportService.GetAgentSettlementReportAsync(reportFilter);
        ViewBag.TransactionFilter = result.Items.FirstOrDefault();
        if (WebHelper.IsAjaxRequest(Request))
            return await Task.FromResult(PartialView("_SettlementReport", result));
        return await Task.FromResult(View(result));
    }

    [HttpGet]   
    public async Task<IActionResult> ViewSettlementDetail(string TransactionId, string AgentCode, string PayoutNepaliDate)
    {
        var filter = new AgentSettlementFilter
        {   
            TransactionId = TransactionId,
            StartDate = PayoutNepaliDate,
            AgentCode = AgentCode,
        };
        var settlementDetail = await _agentReportService.GetAgentSettlementReportAsync(filter);
        return PartialView("_ViewSettlementDetail", settlementDetail.Items.FirstOrDefault());
    }

    [HttpGet("ExportAgentSettlementTransactionToCsv")]
    public async Task<IActionResult> ExportAgentSettlementTransactionToCsv([FromQuery] AgentSettlementFilter request)
    {
        request.Export = 1;
        var result = await _agentReportService.GetAgentSettlementReportAsync(request);
        var data = result.Items;
        var (bytes, fileformate, filename) = ExportHelper.GenerateCsv(data, new string[] { }, null, "AgentSettlementTransactionReports", true);
        return File(bytes, fileformate, filename);
    }

    [HttpGet("ExportAgentSettlementTransactionToExcel")]
    public async Task<IActionResult> ExportAgentSettlementTransactionToExcel([FromQuery] AgentSettlementFilter request)
    {
        request.Export = 1;
        var result = await _agentReportService.GetAgentSettlementReportAsync(request);
        var data = result.Items;
        List<DataTable> dataTables = await IEnumerableExtensions.ToDataTablesAsync(data, 500000);
        var (excelFileByteArr, fileFormat, fileName) = await ExportHelper.ToExcelAsync(dataTables, "AgentSettlementTransactionReports", true);
        return File(excelFileByteArr, fileFormat, fileName);
    }

    [HttpGet("ExportAgentSettlementTransactionToPdf")]
    public async Task<FileContentResult> ExportAgentSettlementTransactionToPdf([FromQuery] AgentSettlementFilter request)
    {
        request.Export = 1;
        var result = await _agentReportService.GetAgentSettlementReportAsync(request);
        var data = result;
        var (bytedata, format) = await ExportHelper.TopdfAsync(data, "AgentSettlementTransactionReports");
        return File(bytedata, format, "AgentSettlementTransactionreport.pdf");
    }

    [HttpGet]
    public async Task<IActionResult> ViewCommissionDetail(string TransactionId)
    {
        var filter = new AgentCommissionFilter
        {
            TransactionId = TransactionId
        };
        var commissionDetail = await _agentReportService.GetAgentCommissionTxnReportAsync(filter);
        return PartialView("_ViewCommissionDetail", commissionDetail.Items.FirstOrDefault());
    }

    [HttpGet("ExportAgentCommissionTransactionToCsv")]
    public async Task<IActionResult> ExportAgentCommissionTransactionToCsv([FromQuery] AgentCommissionFilter request)
    {
        request.Export = 1;
        var result = await _agentReportService.GetAgentCommissionTxnReportAsync(request);
        var data = result.Items;
        var (bytes, fileformate, filename) = ExportHelper.GenerateCsv(data, new string[] { }, null, "AgentCommissionTransactionReports", true);
        return File(bytes, fileformate, filename);
    }

    [HttpGet("ExportAgentCommissionTransactionToExcel")]
    public async Task<IActionResult> ExportAgentCommissionTransactionToExcel([FromQuery] AgentCommissionFilter request)
    {
        request.Export = 1;
        var result = await _agentReportService.GetAgentCommissionTxnReportAsync(request);
        var data = result.Items;
        List<DataTable> dataTables = await IEnumerableExtensions.ToDataTablesAsync(data, 500000);
        var (excelFileByteArr, fileFormat, fileName) = await ExportHelper.ToExcelAsync(dataTables, "AgentCommissionTransactionReports", true);
        return File(excelFileByteArr, fileFormat, fileName);
    }

    [HttpGet("ExportAgentCommissionTransactionToPdf")]
    public async Task<FileContentResult> ExportAgentCommissionTransactionToPdf([FromQuery] AgentCommissionFilter request)
    {
        request.Export = 1;
        var result = await _agentReportService.GetAgentCommissionTxnReportAsync(request);
        var data = result;
        var (bytedata, format) = await ExportHelper.TopdfAsync(data, "AgentCommissionTransactionReports");
        return File(bytedata, format, "AgentCommissionTransactionreport.pdf");
    }

    [HttpGet]
    public async Task<IActionResult> AgentSettlementReport(AgentStatementFilter model)
    {
        model.StartDate = string.IsNullOrEmpty(model.StartDateBS) ? model.StartDate : model.StartDateBS;
        model.EndDate = string.IsNullOrEmpty(model.EndDateBS) ? model.EndDate : model.EndDateBS;
        model.AgentCode = _loggedInUser.FindFirstValue("AgentCode");
        ViewBag.AgentCode = _loggedInUser.FindFirstValue("AgentCode");
        var data = await _cashAgentUserService.GetAgentAccountSettlementReport(model);
        if (model.Export == 1)
        {
            var datas = data.Items;
            List<DataTable> dataTables = await IEnumerableExtensions.ToDataTablesAsync<AgentAccountStatement>(datas, 500000);
            var (excelFileByteArr, fileFormat, fileName) = await ExportHelper.ToExcelAsync(dataTables, "AgentAccountSettlement", true);
            return File(excelFileByteArr, fileFormat, fileName);
        }
        if (WebHelper.IsAjaxRequest(Request))
            return await Task.FromResult(PartialView("_AgentSettlement", data));
        return await Task.FromResult(View(data));
    }
}
