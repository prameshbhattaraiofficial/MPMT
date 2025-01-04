using Mpmt.Core.Domain.Partners.SendTransactions;
using Mpmt.Core.Dtos.PartnerReport;
using Mpmt.Core.Dtos.Transaction;
using Mpmts.Core.Dtos;

namespace Mpmt.Data.Repositories.Partner
{
    public interface IPartnerSendTxnsRepository
    {
        Task<(SprocMessage, SendTxnChargeAmountDetailsDto)> GetSendTxnChargeAmountDetailsAsync(GetSendTxnChargeAmountDetailsRequest request);
        Task<(SprocMessage, AddTransactionDetailsDto)> AddTransactionAsync(AddTransactionDto request);
        Task<(SprocMessage, TxnProcessIdDto)> GetProcessIdAsync(string vendorId, GetProcessId request);
        Task<(SprocMessage, SendTxnChargeAmountDetailsDto)> GetSendNepaliToOtherChargeAmountDetailsAsync(GetSendTxnChargeAmountDetailsRequest request);
        Task<(SprocMessage, AddTransactionDetailsDto)> PushBulkTransactionAsync(BulkTransactionDetailsModel bulkTransactionDetailsModel, BulkTxnBaseModel requestModel);
        Task<SprocMessage> cancelledTransactionAsysnc(RemitTxnReport model);
    }
}
