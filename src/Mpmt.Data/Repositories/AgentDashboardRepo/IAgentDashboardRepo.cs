using Mpmt.Core.Dtos.Agent;

namespace Mpmt.Data.Repositories.AgentDashboardRepo
{
    public interface IAgentDashboardRepo
    {
        Task<AgentDashboard> GetAgentDashBoard(string AgentCode);
    }
}
