using Mpmt.Core.Dtos.Agent;
using Mpmt.Core.Dtos.AgentApi;
using Mpmt.Core.Dtos.PartnerApi;
using Mpmts.Core.Dtos;

namespace Mpmt.Data.Repositories.AgentModule
{
    public interface IAgentApiRepository
    {
        Task<(SprocMessage, PayoutResponse)> AgentWalletValidateLoadAsync(AgentPayoutModel model);
        Task<(SprocMessage, CashPayoutDetailApi)> GetCashPayoutDetailAsync(GetCashPayoutDetailParam reqParam);
        Task<(SprocMessage, TxnProcessId)> GetTxnProcessIdAsync(string vendorId, string referenceId);
        Task<(SprocMessage, PayoutDetailsApi)> RequestPayoutAsync(RequestPayoutParam reqParam);
        Task<(SprocMessage, CheckPayoutStatusDetail)> CheckPayoutStatusAsync(string remitTransactionId, string agentCode);
        Task<(SprocMessage, AgentWalletApi)> GetInstrumentDetailForAgentWalletAsync(GetCashPayoutDetailParam getChargeDetailsParam);
    }
}
