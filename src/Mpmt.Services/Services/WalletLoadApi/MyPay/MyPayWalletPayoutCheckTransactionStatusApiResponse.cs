namespace Mpmt.Services.Services.WalletLoadApi.MyPay
{
    public class MyPayWalletPayoutCheckTransactionStatusApiResponse : MyPayBaseResponse
    {
        public string TransactionStatus { get; set; }
        public int StatusCode { get; set; }
        //public string Message { get; set; }
        //public string ResponseMessage { get; set; }
        //public string Details { get; set; }
        //public int ReponseCode { get; set; }
        public bool Status { get; set; }
    }
}
