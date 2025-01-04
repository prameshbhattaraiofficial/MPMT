namespace Mpmt.Core.Dtos.AgentApi
{
    public class InstrumentDetailRequest
    {
        public string MtcnNumber { get; set; }
        public string ApiUserName { get; set; }
        public string Signature { get; set; }
    }

    public class InstrumentDetailList
    {
        public string ApiUserName { get; set; }
        public string Signature { get; set; }
    }
}
