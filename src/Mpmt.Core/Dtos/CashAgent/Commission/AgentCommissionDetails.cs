namespace Mpmt.Core.Dtos.CashAgent.Commission
{
    public class AgentCommissionDetails : AgentInfoItem
    {
        private IEnumerable<AgentCommissionRule> _commissionRuleList;

        public IEnumerable<AgentCommissionRule> CommissionRuleList
        {
            get => _commissionRuleList ?? new List<AgentCommissionRule>();
            set => _commissionRuleList = value;
        }
    }
}
