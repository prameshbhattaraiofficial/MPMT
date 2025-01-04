namespace Mpmt.Core.Configuration
{
    public class MypayConfig
    {
        public const string Name = "Testhttp";
        public static string SectionName => $"TesthttpConfig:{Name}";
        public string BaseUrl { get; set; }
        public string ApiKey { get; set; }
        public string MerchantId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
