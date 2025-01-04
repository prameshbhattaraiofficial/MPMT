namespace Mpmt.Services.Services.BankLoadApi
{
    public class MyPayBankPayoutApiRequest
    {
        public string Amount { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        public string BankCode { get; set; }
        public string Reference { get; set; }
        public string Description {get;set; }
        public string MerchantId { get; set; }
        public string AuthTokenString { get; set; }
    }
}
