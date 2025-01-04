namespace Mpmt.Services.Services.BankLoadApi
{
    public class MyPayValidateBankUserApiRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string AccountNumber { get; set; }
        public string BankCode { get; set; }
        public string AccountName { get; set; }
        public string Reference { get; set; }
        public string MerchantId { get; set; }
        public string Description { get; set; }
        public decimal? Amount { get; set; }
        public string AuthTokenString { get; set; }
    }
}
