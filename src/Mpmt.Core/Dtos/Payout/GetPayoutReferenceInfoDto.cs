namespace Mpmt.Core.Dtos.Payout
{
    public class GetPayoutReferenceInfoDto
    {
        public string RemitTransactionId { get; set; }
        public string PaymentType { get; set; }
        public string AgentCode { get; set; }
        public string IpAddress { get; set; }
        public string DeviceId { get; set; }
        public string LoggedInUser { get; set; }
        public string UserType { get; set; }
    }
}
