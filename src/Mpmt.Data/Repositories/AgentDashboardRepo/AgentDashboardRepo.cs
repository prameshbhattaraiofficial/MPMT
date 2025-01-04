using AutoMapper;
using Dapper;
using Mpmt.Core.Dtos.Agent;
using Mpmt.Core.Dtos.AgentReport;
using Mpmt.Data.Common;
using System.Data;

namespace Mpmt.Data.Repositories.AgentDashboardRepo
{
    public class AgentDashboardRepo : IAgentDashboardRepo
    {
        public readonly IMapper _mapper;

        public AgentDashboardRepo(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<AgentDashboard> GetAgentDashBoard(string AgentCode)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@AgentCode", AgentCode);

            var data = await connection
                .QueryMultipleAsync("[dbo].[usp_agentdashboard]", param: param, commandType: CommandType.StoredProcedure);
            var dashboardData = await data.ReadFirstAsync<AgentDashboard>();
            var settlementData = await data.ReadAsync<AgentSettlementReport>();

            var mappedsettlementData = _mapper.Map<List<AgentSettlementReport>>(settlementData);
            dashboardData.SettlementReport = mappedsettlementData;
            return dashboardData;
        }
    }
}
