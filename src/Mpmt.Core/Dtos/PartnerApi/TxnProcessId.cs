namespace Mpmt.Core.Dtos.PartnerApi
{
    public class TxnProcessId
    {
        public string ProcessId { get; set; }
        public DateTime? ProcessIdUtcDate { get; set; }
        public bool IsUsed { get; set; }
    }
}
