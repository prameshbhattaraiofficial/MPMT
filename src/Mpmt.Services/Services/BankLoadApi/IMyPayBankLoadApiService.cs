using Mpmt.Core.Dtos.BankLoad;
using Mpmt.Core.Dtos.PartnerApi;
using Mpmt.Core.Dtos.WalletLoad.MyPay;
using Mpmt.Services.Services.WalletLoadApi.MyPay;
using Mpmts.Core.Dtos;
using System.Net;

namespace Mpmt.Services.Services.BankLoadApi
{
    public interface IMyPayBankLoadApiService
    {
        Task<(HttpStatusCode, MyPayValidateBankUserApiResponse)> ValidateBankUserAsync(MyPayValidateBankUserDto dto);
        Task<(HttpStatusCode, MyPayBankPayoutApiResponse)> BankPayoutAsync(MyPayBankPayoutDto dto);
        Task validateReferenceNumber(ValidateAccountRequest request);
    }
}
