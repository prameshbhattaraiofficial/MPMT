using Mpmt.Core.Dtos.Agent;

namespace Mpmt.Services.Services.AgentDashboardService
{
    public interface IAgentDashboardService
    {
        Task<AgentDashboard> GetAgentDashBoard(string AgentCode);
    }
}
