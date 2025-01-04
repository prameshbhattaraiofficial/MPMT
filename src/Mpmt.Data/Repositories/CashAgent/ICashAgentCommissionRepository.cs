using Mpmt.Core.Dtos.CashAgent.Commission;
using Mpmts.Core.Dtos;

namespace Mpmt.Data.Repositories.CashAgent
{
    public interface ICashAgentCommissionRepository
    {
        Task<SprocMessage> AddOrUpdateAsync(List<AgentCommissionRuleType> commissionRules, string agentCode, string superAgentCode, string agentType, string userType, string loggedinUser);
        Task<AgentCommissionDetails> GetAgentCommissionDetailsAsync(string agentCode, string agentType);
        Task<IEnumerable<AgentCommissionRule>> GetCommissionSlabsBySuperAgent(string agentCode);
        Task<SprocMessage> AddOrUpdateDefaultRulesAsync(List<AgentCommissionRuleType> commissionRules, string userType, string loggedinUser);
        Task<IEnumerable<AgentDefaultCommissionRule>> GetAgentDefaultCommissionDetailsAsync();
    }
}
