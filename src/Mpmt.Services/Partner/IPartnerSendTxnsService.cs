using Mpmt.Core.Domain;
using Mpmt.Core.Domain.Admin.Reports;
using Mpmt.Core.Domain.Partners.SendTransactions;
using Mpmt.Core.Dtos;
using Mpmt.Core.Dtos.PartnerReport;
using Mpmt.Core.Dtos.Transaction;
using Mpmts.Core.Dtos;

namespace Mpmt.Services.Partner
{
    public interface IPartnerSendTxnsService
    {
        Task<(SprocMessage, SendTxnChargeAmountDetailsDto)> GetSendTxnChargeAmountDetailsAsync(GetSendTxnChargeAmountDetailsRequest request);
        Task<(MpmtResult, TxnProcessIdDto)> GetProcessIdAsync(GetProcessId request);
        Task<MpmtResult> AddTransactionAsync(AddTransactionDto request);
        Task<MpmtResult> AddByAdminTransactionAsync(TransactionDetailsAdmin managedetails);
        Task<MpmtResult> CheckStatusTransactionAsync(TransactionDetailsAdmin managedetails);
        Task<(SprocMessage, SendTxnChargeAmountDetailsDto)> GetSendNepaliToOtherChargeAmountDetailsAsync(GetSendTxnChargeAmountDetailsRequest request);
        Task<(FieldValidationResult, BulkTransactionResponse)> ValidateBulkTransactionCells(BulkTransactionDetailsModel bulkTransactionDetailsModel);
        Task<MpmtResult> ValidateBulkTxnApiAsync(BulkTransactionDetailsModel bulkTransactionDetailsModel);
        Task<(SprocMessage, AddTransactionDetailsDto)> PushBulkTransactionServiceAsync(BulkTransactionDetailsModel bulkTransactionDetailsModel);
        bool SendBulkTransactionEmailAsync(BulkTxnBaseModel modelRequest);
        Task<SprocMessage> cancelledTransactionAsysnc(RemitTxnReport model);
    }
}
