namespace Mpmt.Services.Services.BankLoadApi
{
    public class MyPayBaseResponse
    {
        public string Message { get; set; }
        public string responseMessage { get; set; }
        public string Details { get; set; }
        public int ReponseCode { get; set; }
        public bool status { get; set; }
        public string ios_version { get; set; }
        public string AndroidVersion { get; set; }
        public string CouponCode { get; set; }
        public bool IsCouponUnlocked { get; set; }
    }
}
