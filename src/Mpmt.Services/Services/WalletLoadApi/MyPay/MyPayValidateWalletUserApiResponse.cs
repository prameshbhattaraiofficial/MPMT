namespace Mpmt.Services.Services.WalletLoadApi.MyPay
{
    public class MyPayValidateWalletUserApiResponse : MyPayBaseResponse
    {
        public string AccountStatus { get; set; }
        public string FullName { get; set; }
        public string ContactNumber { get; set; }
        public bool IsAccountValidated { get; set; }
        public bool kycstatus { get; set; }
    }
}
