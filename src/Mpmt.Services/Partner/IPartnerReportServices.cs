using Mpmt.Core.Dtos.CashAgent;
using Mpmt.Core.Dtos.InwardRemitanceReport;
using Mpmt.Core.Dtos.Paging;
using Mpmt.Core.Dtos.PartnerReport;
using Mpmt.Core.Dtos.Transaction;
using Mpmt.Core.Models.CashAgent;

namespace Mpmt.Services.Partner
{
    public interface IPartnerReportServices
    {
        Task<PagedList<RemitTxnReport>> GetRemitTxnReportAsync(RemitTxnReportFilter txnFilter);
        Task<PagedList<RemitSettlementReport>> GetRemitSettlementReportAsync(RemitSettlementReportFilter filter);
        Task<PagedList<RemitSettlementReport>> GetSettlementReportAsync(RemitSettlementReportFilter filter);    
        Task<PagedList<RemitFeeTxnReport>> GetRemitFeeTxnReportAsync(RemitTxnReportFilter txnFilter);
        Task<PagedList<CommisionTransction>> GetRemitCommissionTxnReportAsync(CommissionTransactionFilter txnFilter);
        Task<(SumofTransaction, PagedList<InwardRemitanceDto>)> GetInwardRemitanceReport(InwardRemitanceFilterDto model);
        Task<(SumofTransaction, PagedList<InwardRemitanceCompanyWiseDto>)> GetInwardRemitanceCompanyWiseReport(InwardRemitanceFilterDto model);
        Task<(SumofTransaction, PagedList<InwardRemitanceAgentWiseReport>)> GetRemitanceCompanySelfOrAgentOrSubAgentInwardDetails(InwardRemitanceFilterDto model);
        Task<PagedList<ActionTakenByRemitanceDto>> GetActionTakenbyTheRemitanceReport(InwardRemitanceFilterDto model);

        Task<PagedList<ExportRemitSettlementReport>> ExportSettlementReportAsync(RemitSettlementReportFilter filter);
        Task<PagedList<ExportRemitTxnReport>> ExportRemitTxnReportAsync(RemitTxnReportFilter request);
        Task<PagedList<AdminExportRemitTxnReport>> AdminExportRemitTxnReportAsync(RemitTxnReportFilter request);
        Task<PagedList<AdminExportRemitSettlementReport>> ExportAdminRemitSettlementReportAsync(RemitSettlementReportFilter request);
        Task<PagedList<ExportRemitSettlementReport>> ExportRemitSettlementReportAsync(RemitSettlementReportFilter request);
        Task<PagedList<AdminExportRemitSettlementReport>> ExportAdminSettlementReportAsync(RemitSettlementReportFilter request);
    }
}
