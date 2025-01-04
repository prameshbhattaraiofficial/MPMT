using DocumentFormat.OpenXml.EMMA;
using Mpmt.Core.Dtos.InwardRemitanceReport;
using Mpmt.Core.Dtos.Paging;
using Mpmt.Core.Dtos.PartnerReport;
using Mpmt.Core.Dtos.Transaction;
using Mpmt.Data.Repositories.Partner;

namespace Mpmt.Services.Partner
{
    public class PartnerReportServices : IPartnerReportServices
    {
        private readonly IPartnerReportRepo _partnerReport;

        public PartnerReportServices(IPartnerReportRepo partnerReport)
        {
            _partnerReport = partnerReport;
        }

        public async Task<PagedList<RemitTxnReport>> GetRemitTxnReportAsync(RemitTxnReportFilter txnFilter)
        {
            var data = await _partnerReport.GetRemitTxnReportAsync(txnFilter);
            return data;
        }

        public async Task<PagedList<RemitSettlementReport>> GetRemitSettlementReportAsync(RemitSettlementReportFilter filter)
        {
            var data = await _partnerReport.GetRemitSettlementReportAsync(filter);
            return data;
        }

        public async Task<PagedList<RemitSettlementReport>> GetSettlementReportAsync(RemitSettlementReportFilter filter)
        {
            var data = await _partnerReport.GetSettlementReportAsync(filter);
            return data;
        }   

        public async Task<PagedList<RemitFeeTxnReport>> GetRemitFeeTxnReportAsync(RemitTxnReportFilter txnFilter)
        {
            var data = await _partnerReport.GetRemitFeeTxnReportAsync(txnFilter);
            return data;
        }

        public async Task<PagedList<CommisionTransction>> GetRemitCommissionTxnReportAsync(CommissionTransactionFilter txnFilter)
        {
            var data = await _partnerReport.GetRemitCommissionTxnReportAsync(txnFilter);
            return data;
        }

        public async Task<(SumofTransaction, PagedList<InwardRemitanceDto>)> GetInwardRemitanceReport(InwardRemitanceFilterDto model)
        {
            var data = await _partnerReport.GetInwardRemitanceReport(model);
            return data;
        }

        public async Task<(SumofTransaction, PagedList<InwardRemitanceCompanyWiseDto>)> GetInwardRemitanceCompanyWiseReport(InwardRemitanceFilterDto model)
        {
            var data = await _partnerReport.GetInwardRemitanceCompanyWiseReport(model);
            return data;
        }

        public async Task<(SumofTransaction, PagedList<InwardRemitanceAgentWiseReport>)> GetRemitanceCompanySelfOrAgentOrSubAgentInwardDetails(InwardRemitanceFilterDto model)
        {
            var data = await _partnerReport.GetRemitanceCompanySelfOrAgentOrSubAgentInwardDetails(model);
            return data;
        }

        public async Task<PagedList<ActionTakenByRemitanceDto>> GetActionTakenbyTheRemitanceReport(InwardRemitanceFilterDto model)
        {
            var data = await _partnerReport.GetActionTakenbyTheRemitanceReport(model);
            return data;
        }

        public async Task<PagedList<ExportRemitSettlementReport>> ExportSettlementReportAsync(RemitSettlementReportFilter filter)
        {
            var data = await _partnerReport.ExportSettlementReportAsync(filter);
            return data;
        }

        public async Task<PagedList<ExportRemitTxnReport>> ExportRemitTxnReportAsync(RemitTxnReportFilter request)
        {
            var data = await _partnerReport.ExportRemitTxnReportAsync(request);
            return data;
        }

        public async Task<PagedList<AdminExportRemitTxnReport>> AdminExportRemitTxnReportAsync(RemitTxnReportFilter request)
        {
            var data = await _partnerReport.AdminExportRemitTxnReportAsync(request);
            return data;
        }

        public async Task<PagedList<AdminExportRemitSettlementReport>> ExportAdminRemitSettlementReportAsync(RemitSettlementReportFilter request)
        {
            var data = await _partnerReport.ExportAdminRemitSettlementReportAsync(request);
            return data;
        }

        public async Task<PagedList<ExportRemitSettlementReport>> ExportRemitSettlementReportAsync(RemitSettlementReportFilter request)
        {
            var data = await _partnerReport.ExportRemitSettlementReportAsync(request);
            return data;
        }

        public async Task<PagedList<AdminExportRemitSettlementReport>> ExportAdminSettlementReportAsync(RemitSettlementReportFilter request)
        {
            var data = await _partnerReport.ExportAdminSettlementReportAsync(request);
            return data;
        }
    }
}
