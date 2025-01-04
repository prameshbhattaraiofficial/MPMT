using Mpmt.Core.Dtos.AgentFundTransfer;
using Mpmt.Core.Dtos.AgentReport;
using Mpmt.Core.Dtos.CashAgent;
using Mpmt.Core.Dtos.Paging;
using Mpmt.Core.Models.CashAgent;
using Mpmts.Core.Dtos;

namespace Mpmt.Services.CashAgents;

public interface IAgentReportService
{
    Task<PagedList<AgentCommissionTransactionReport>> GetAgentCommissionTxnReportAsync(AgentCommissionFilter txnFilter);

    Task<PagedList<AgentCommissionTransactionReport>> GetAgentCommissionTxnReportAdminAsync(AgentCommissionFilter txnFilter);
    Task<PagedList<AgentCommissionTransactionReportDetail>> GetAgentCommissionTxnReportDetailAdminAsync(AgentCommissionFilter txnFilter);   
    Task<PagedList<FundRequest>> GetAgentFundRequestReportAsync(AgentFundRequestFilter txnFilter);
    Task<PagedList<FundRequest>> GetAgentFundRequestDetailsAsync(AgentFundRequestFilter model);
    Task<SprocMessage> ApproveRejectAgentFundRequestAysnc(ApproveRejectFundTransferByAdmin model);
    Task<SprocMessage> ReviewAgentFundRequestAysnc(ApproveRejectFundTransferByAdmin model); 
    Task<AgentFundApproveRejectModel> GetDetailsRequestFundAsync(AgentFundApproveRejectModel model);
    Task<PagedList<AgentSettlementReport>> GetAgentSettlementReportAsync(AgentSettlementFilter txnFilter);
    Task<IEnumerable<AgentUnsettledAmount>> GetAgentUnsettledAmountSummaryList();
}
