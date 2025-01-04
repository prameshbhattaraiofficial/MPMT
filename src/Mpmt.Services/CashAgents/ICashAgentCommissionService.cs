using Mpmt.Core.Dtos;
using Mpmt.Core.Dtos.CashAgent.Commission;

namespace Mpmt.Services.CashAgents
{
    public interface ICashAgentCommissionService
    {
        Task<MpmtResult> AddOrUpdateAsync(List<AgentCommissionRuleType> commissionRules, string agentCode, string superAgentCode, string agentType, string userType);
        Task<AgentCommissionDetails> GetAgentCommissionDetailsAsync(string agentCode, string agentType);
        Task<IEnumerable<AgentCommissionRule>> GetCommissionSlabsBySuperAgent(string agentCode);
        Task<MpmtResult> AddOrUpdateDefaultRulesAsync(List<AgentCommissionRuleType> commissionRules, string userType);
        Task<IEnumerable<AgentDefaultCommissionRule>> GetAgentDefaultCommissionDetailsAsync();
    }
}
