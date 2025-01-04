namespace Mpmt.Core.Dtos.AgentApi
{
    public class CheckPayoutStatusRequest
    {
        public string ApiUserName { get; set; }
        public string RemitTransactionId { get; set; }
        public string Signature { get; set; }
    }
}
