namespace Mpmt.Core.Dtos.CashAgent.Commission
{
    public class AgentDefaultCommissionRule
    {
        public int MinTxnCount { get; set; }
        public int MaxTxnCount { get; set; }
        public decimal Commission { get; set; }
        public decimal MinCommission { get; set; }
        public decimal MaxCommission { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
