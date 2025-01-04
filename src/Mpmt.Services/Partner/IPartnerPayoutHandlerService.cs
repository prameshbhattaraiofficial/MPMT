using Mpmt.Core.Domain.Admin.Reports;
using Mpmt.Core.Domain.Payout;
using Mpmt.Core.Dtos;
using Mpmt.Core.Dtos.Transaction;

namespace Mpmt.Services.Partner
{
    public interface IPartnerPayoutHandlerService
    {
        Task<MpmtResult> HandlePayoutToWalletAsync(AddTransactionResultDetails addTxnDetails);
        Task<MpmtResult> HandlePayoutToBankAsync(AddTransactionResultDetails addTxnDetails);
        bool HandlePartnerLowWalletBalanceNotificationEmailing(AddTransactionResultDetails details);
        bool HandleSenderTransactionNotificationEmailing(AddTransactionResultDetails details);
        bool HandlePartnerRemainingFeeBalanceNotificationEmailing(AddTransactionResultDetails txnResultDetails);
        Task<MpmtResult> HandleByAdminPayoutToBankAsync(TransactionDetailsAdmin txnDetails);
        Task<MpmtResult> HandleByAdminPayoutToWalletAsync(TransactionDetailsAdmin txnResultDetails);
        Task<MpmtResult> HandleByAdminCheckStatusWalletAsync(TransactionDetailsAdmin txnResultDetails);
        Task<MpmtResult> HandleByBulkTransactionToWalletAsync(BulkTransactionDetailsModel request, BulkTxnBaseModel requestModel);
        Task<MpmtResult> HandleByBulkTransactionToBankAsync(BulkTransactionDetailsModel request, BulkTxnBaseModel requestModel);
        bool HandleBulkTransactionEmailing(BulkTxnBaseModel modelRequest);
    }
}
