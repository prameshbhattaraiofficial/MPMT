namespace Mpmt.Core.Dtos.Payment.MyPay
{
    public class MyPayOrderGeneratedResponse : MyPayBaseApiResponse
    {
        public string MerchantTransactionId { get; set; }
        public string RedirectURL { get; set; }
    }
    public class MyPayBaseApiResponse
    {
        public string Message { get; set; }
        public string ResponseMessage { get; set; }
        public string Details { get; set; }
        public int ReponseCode { get; set; }
        public bool status { get; set; }
        public string Ios_version { get; set; }
        public string AndroidVersion { get; set; }
    }
}
