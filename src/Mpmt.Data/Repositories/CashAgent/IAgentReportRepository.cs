using Mpmt.Core.Dtos.AgentFundTransfer;
using Mpmt.Core.Dtos.AgentReport;
using Mpmt.Core.Dtos.CashAgent;
using Mpmt.Core.Dtos.Paging;
using Mpmt.Core.Models.CashAgent;
using Mpmts.Core.Dtos;

namespace Mpmt.Data.Repositories.CashAgent;

public interface IAgentReportRepository
{
    Task<(ApproveRejectReviewModel, SprocMessage)> ApproveRejectAgentFundRequestAysnc(ApproveRejectFundTransferByAdmin model);
    Task<PagedList<AgentCommissionTransactionReport>> GetAgentCommissionTxnReportAsync(AgentCommissionFilter txnFilter);
    Task<PagedList<AgentCommissionTransactionReportDetail>> GetAgentCommissionTxnReportDetailAsync(AgentCommissionFilter txnFilter);    
    Task<PagedList<FundRequest>> GetAgentFundRequestDetailsAsync(AgentFundRequestFilter txnFilter);    
    Task<AgentFundApproveRejectModel> GetDetailsRequestFundAsync(AgentFundApproveRejectModel model);
    Task<PagedList<FundRequest>> GetFundReqListForAdmin(AgentFundRequestFilter txnFilter);
    Task<PagedList<AgentSettlementReport>> GetAgentSettlementReportAsync(AgentSettlementFilter txnFilter);
    Task<IEnumerable<AgentUnsettledAmount>> GetAgentUnsettledAmountSummaryList();
}
