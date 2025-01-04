using Mpmt.Core.Domain.Payout;

namespace Mpmt.Services.Partner
{
    public static class PartnerPayoutHelper
    {
        public static bool IsPayoutProceedable(AddTransactionResultDetails details)
        {
            // payout is not Proceedable if Transaction Approval Required
            if (!details.TransactionApprovalRequired.HasValue)
                return false;
            if (details.TransactionApprovalRequired.HasValue && details.TransactionApprovalRequired.Value)
                return false;

            // payout is not Proceedable if FeeCreditLimitOverFlow reached
            if (!details.FeeCreditLimitOverFlow.HasValue)
                return false;
            if (details.FeeCreditLimitOverFlow.HasValue && details.FeeCreditLimitOverFlow.Value)
                return false;

            return true;
        }
    }
}
