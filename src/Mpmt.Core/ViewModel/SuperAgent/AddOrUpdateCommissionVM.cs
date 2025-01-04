using Mpmt.Core.Dtos.CashAgent.Commission;
using System.ComponentModel.DataAnnotations;

namespace Mpmt.Core.ViewModel.SuperAgent
{
    public class AddOrUpdateCommissionVM
    {
        private List<AgentCommissionRule> _commissionRules;

        [Required]
        public string AgentCode { get; set; }

        public string SuperAgentCode { get; set; }
        public string AgentType { get; set; }

        public List<AgentCommissionRule> CommissionRules { get => _commissionRules ?? new(); set => _commissionRules = value; }
    }
}
