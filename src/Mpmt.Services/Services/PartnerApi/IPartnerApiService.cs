using Mpmt.Core.Dtos.PartnerApi;
using System.Net;

namespace Mpmt.Services.Services.PartnerApi
{
    public interface IPartnerApiService
    {
        Task<(HttpStatusCode, object)> GetInstrumentListsAsync(GetInstrumentListsRequest request);
        Task<(HttpStatusCode, object)> GetTxnChargeDetailsAsync(GetTxnChargeDetailsRequest request);
        Task<(HttpStatusCode, object)> GetTxnProcessIdAsync(GetProcessIdRequest request);
        Task<(HttpStatusCode, object)> PushTransactionAsync(PushTransactionRequest request);
        Task<(HttpStatusCode, object)> GetTransactionStatusAsync(TransactionStatusRequest request);
        Task<(HttpStatusCode, object)> ValidateAccountAsync(ValidateAccountRequest request);
        Task<(HttpStatusCode, object)> PushTransactionDetailsAsync(PushTransactionRequestDetals request);
    }
}
