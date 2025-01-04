using Mpmt.Core.Dtos.BankLoad;
using Mpmt.Core.Dtos.WalletLoad.MyPay;
using Mpmt.Services.Services.BankLoadApi;
using System.Net;

namespace Mpmt.Services.Services.WalletLoadApi.MyPay
{
    public interface IMyPayWalletLoadApiService
    {
        Task<(HttpStatusCode, MyPayValidateWalletUserApiResponse)> ValidateWalletUserAsync(MyPayValidateWalletUserDto dto);
        Task<(HttpStatusCode, MyPayWalletPayoutApiResponse)> WalletPayoutAsync(MyPayWalletPayoutDto dto);
        Task<(HttpStatusCode, MyPayWalletPayoutCheckTransactionStatusApiResponse)> CheckTransactionStatusAsync(MyPayWalletPayoutCheckTransactionStatusDto dto);
    }
}
