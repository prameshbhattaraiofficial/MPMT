namespace Mpmt.Services.Services.WalletLoadApi.MyPay
{
    public class MyPayWalletPayoutCheckTransactionStatusApiRequest
    {
        public string TransactionId { get; set; }
        public string TransactionReference { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Reference { get; set; }
        public string MerchantId { get; set; }
        public string AuthTokenString { get; set; }
    }
}
