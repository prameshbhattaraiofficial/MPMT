namespace Mpmt.Services.Services.BankLoadApi
{
    public class MyPayBankLoadApiConfig
    {
        public const string SectionName = "VendorApi:MyPayWalletLoadApi";
        public string BaseUrl { get; set; }
        public string MerchantId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ApiKey { get; set; }
    }
}
