namespace Mpmt.Core.Dtos.AgentApi
{
    public class GetCashPayoutDetailParam
    {
        public string MtcnNumber { get; set; }
        public string AgentCode { get; set; }
        public string IpAddress { get; set; }
        public string DeviceId { get; set; }
    }
}
