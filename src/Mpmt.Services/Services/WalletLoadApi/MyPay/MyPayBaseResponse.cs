namespace Mpmt.Services.Services.WalletLoadApi.MyPay
{
    public class MyPayBaseResponse
    {
        public string Message { get; set; }
        public string ResponseMessage { get; set; }
        public string Details { get; set; }
        public int ReponseCode { get; set; }
        public bool status { get; set; }
        public string Ios_version { get; set; }
        public string CouponCode { get; set; }
        public bool IsCouponUnlocked { get; set; }
        public string TransactionUniqueId { get; set; }
        public string AndroidVersion { get; set; }
    }
}
