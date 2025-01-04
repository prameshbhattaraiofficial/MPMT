using Mpmt.Core.Dtos.BankLoad;
using Mpmt.Core.Dtos.WalletLoad.MyPay;
using Mpmts.Core.Dtos;

namespace Mpmt.Data.Repositories.Payout
{
    public interface IMyPayWalletLoadRepository
    {
        Task<SprocMessage> LogWalletUserValidationAsync(MyPayValidateWalletUserLogParam log);
        Task<SprocMessage> LogRemitToWalletPayoutAsync(MyPayWalletPayoutLogParam log);
        Task<SprocMessage> LogWalletPayoutTxnStatusAsync(MyPayWalletLoadPayoutTxnStatusLogParam log);
        //Task<SprocMessage> LogRemitToBankPayoutAsync(MyPayBankPayoutLogParam payoutLogParam);
    }
}
