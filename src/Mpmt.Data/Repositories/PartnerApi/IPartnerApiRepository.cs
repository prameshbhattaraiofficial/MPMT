using Mpmt.Core.Dtos.PartnerApi;
using Mpmts.Core.Dtos;

namespace Mpmt.Data.Repositories.PartnerApi
{
    public interface IPartnerApiRepository
    {
        Task<InstrumentLists> GetInstrumentListsAsync(string partnerCode);
        Task<(SprocMessage, TxnChargeDetails)> GetTxnChargeDetailsAsync(GetTxnChargeDetailsParam detailsParam);
        Task<(SprocMessage, TxnProcessId)> GetTxnProcessIdAsync(string vendorId, string referenceId);
        Task<(SprocMessage, PushTransactionDetails)> PushTransactionAsync(PushTransactionParam pushTxnParam);
        Task<TransactionStatusDetails> GetTransactionStatusAsync(string partnerCode, string remitTransactionId);
        Task<(SprocMessage, PushTransactionDetails)> PushTransactionDetailsAsync(PushTransactionRequestDetailsParam pushTxnParam);
        Task<SprocMessage> validateReferenceNumber(ValidateAccountRequest request, string partnerCode);
    }
}
