using Microsoft.AspNetCore.Http;
using Mpmt.Core.Dtos;
using Mpmt.Core.Dtos.CashAgent.Commission;
using Mpmt.Data.Repositories.CashAgent;
using System.Security.Claims;

namespace Mpmt.Services.CashAgents
{
    public class CashAgentCommissionService : ICashAgentCommissionService
    {
        private readonly ICashAgentCommissionRepository _commissionRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CashAgentCommissionService(
            ICashAgentCommissionRepository commissionRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _commissionRepository = commissionRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<MpmtResult> AddOrUpdateAsync(List<AgentCommissionRuleType> commissionRules, string agentCode, string superAgentCode, string agentType, string userType)
        {
            var result = new MpmtResult();

            var loggedInUser = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Name);

            var addUpdateResult = await _commissionRepository.AddOrUpdateAsync(
                commissionRules, agentCode, superAgentCode, agentType, userType, loggedInUser);

            if (addUpdateResult.StatusCode != 200)
            {
                result.AddError(addUpdateResult.StatusCode, addUpdateResult.MsgText);
                return result;
            }

            result.AddSuccess(addUpdateResult.StatusCode, addUpdateResult.MsgText);
            return result;
        }

        public async Task<AgentCommissionDetails> GetAgentCommissionDetailsAsync(string agentCode, string agentType)
        {
            return await _commissionRepository.GetAgentCommissionDetailsAsync(agentCode, agentType);
        }

        public async Task<IEnumerable<AgentDefaultCommissionRule>> GetAgentDefaultCommissionDetailsAsync()
        {
            return await _commissionRepository.GetAgentDefaultCommissionDetailsAsync();
        }

        public async Task<MpmtResult> AddOrUpdateDefaultRulesAsync(List<AgentCommissionRuleType> commissionRules, string userType)
        {
            var result = new MpmtResult();

            var loggedInUser = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Name);
            var addUpdateResult = await _commissionRepository.AddOrUpdateDefaultRulesAsync(commissionRules, userType, loggedInUser);

            if (addUpdateResult.StatusCode != 200)
            {
                result.AddError(addUpdateResult.StatusCode, addUpdateResult.MsgText);
                return result;
            }

            result.AddSuccess(addUpdateResult.StatusCode, addUpdateResult.MsgText);
            return result;
        }

        public async Task<IEnumerable<AgentCommissionRule>> GetCommissionSlabsBySuperAgent(string agentCode)
        {
            return await _commissionRepository.GetCommissionSlabsBySuperAgent(agentCode);
        }
    }
}
