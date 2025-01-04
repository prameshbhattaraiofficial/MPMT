namespace Mpmt.Core.Dtos.WalletLoad.MyPay
{
    public class MyPayWalletPayoutLogParam
    {
        public string PayoutRefereneNo { get; set; }
        public string AgentTransactionId { get; set; }
        public string Message { get; set; }
        public string ResponseMessage { get; set; }
        public string Details { get; set; }
        public string ResponseCode { get; set; }
        public bool Status { get; set; }
        public string UserType { get; set; }
        public string LoggedInUser { get; set; }
    }
}
