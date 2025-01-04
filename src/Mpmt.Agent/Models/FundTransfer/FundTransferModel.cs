using Org.BouncyCastle.Crypto.Tls;

namespace Mpmt.Agent.Models.FundTransfer
{
    public class FundTransferModel
    {
        public string AgentCode { get; set; }
        public decimal CommissionAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal ReceivableAmount { get; set; }
        public string amtTotal { get; set; }

        public string isCommission { get; set; }
        public string isReceivable { get; set; }
        public int RequestStatus { get; set; }
        public int CommRequestStatus { get; set; }
        public string PrefundBalance { get; set; }
        public bool IsAgentPrefunding { get; set; }
    }
}
