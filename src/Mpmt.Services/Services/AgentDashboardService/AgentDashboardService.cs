using Mpmt.Core.Dtos.Agent;
using Mpmt.Data.Repositories.AgentDashboardRepo;

namespace Mpmt.Services.Services.AgentDashboardService
{
    public class AgentDashboardService : IAgentDashboardService
    {
        private readonly IAgentDashboardRepo _dashBoardRepo;

        public AgentDashboardService(IAgentDashboardRepo dashBoardRepo)
        {
            _dashBoardRepo = dashBoardRepo;
        }

        public async Task<AgentDashboard> GetAgentDashBoard(string AgentCode)
        {
            var data = await _dashBoardRepo.GetAgentDashBoard(AgentCode);
            return data;
        }
    }
}
