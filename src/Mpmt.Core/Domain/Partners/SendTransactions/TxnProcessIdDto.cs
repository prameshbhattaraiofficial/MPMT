namespace Mpmt.Core.Domain.Partners.SendTransactions
{
    public class TxnProcessIdDto
    {
        public string ProcessId { get; set; }
        public DateTime? ProcessIdUtcDate { get; set; }
        public bool IsUsed { get; set; }
    }
}
