using Mpmt.Core.Dtos.AgentApi;
using Mpmt.Core.Models.Transaction;
using System.Net;

namespace Mpmt.Services.Services.AgentApi
{
    public interface IAgentApiService
    {
        Task<(HttpStatusCode, object)> AgentmtcnValidateAsync(WalletPayoutApi request);
        Task<(HttpStatusCode, object)> GetInstrumentDetailAsync(InstrumentDetailRequest request);
        Task<(HttpStatusCode, object)> GetTxnProcessIdAsync(GetProcessIdRequestAgentApi request);
        Task<(HttpStatusCode, object)> RequestPayoutAsync(RequestPayoutApi request);
        Task<(HttpStatusCode, object)> CheckPayoutStatusAsync(CheckPayoutStatusRequest request);
        Task<(HttpStatusCode, object)> GetInstrumentDetailForAgentWalletAsync(InstrumentDetailRequest request);
        Task<(HttpStatusCode, object)> RequestPayoutForAgentWalletAsync(RequestPayoutForAgentWalletApi request);
    }
}
