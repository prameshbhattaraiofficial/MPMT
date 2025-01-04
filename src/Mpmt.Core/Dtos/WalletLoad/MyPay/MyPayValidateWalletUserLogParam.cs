namespace Mpmt.Core.Dtos.WalletLoad.MyPay
{
    public class MyPayValidateWalletUserLogParam
    {
        public string RemitTransactionId { get; set; }
        public string AgentCode { get; set; }
        public string AccountStatus { get; set; }
        public string ContactNumber { get; set; }
        public bool IsAccountValidated { get; set; }
        public string Message { get; set; }
        public string ResponseMessage { get; set; }
        public string ResponseCode { get; set; }
        public bool Status { get; set; }
        public string LoggedInUser { get; set; }
        public string UserType { get; set; }
    }
}
