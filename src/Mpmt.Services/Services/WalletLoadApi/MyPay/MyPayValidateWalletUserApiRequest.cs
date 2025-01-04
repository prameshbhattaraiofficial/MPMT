namespace Mpmt.Services.Services.WalletLoadApi.MyPay
{
    public class MyPayValidateWalletUserApiRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ContactNumber { get; set; }
        public string Reference { get; set; }
        public string MerchantId { get; set; }
        public string AuthTokenString { get; set; }
    }
}
