using Dapper;
using Mpmt.Core.Dtos.Agent;
using Mpmt.Core.Dtos.Partner;
using Mpmt.Data.Common;
using Mpmt.Data.Repositories.Partner;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mpmt.Data.Repositories.AgentModule
{
    public class AgentRepository : IAgentRepository
    {
        public async Task<AgentWithCredentials> GetAgentWithCredentialsByApiUserNameAsync(string apiUserName)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@ApiUserName", apiUserName);

            return await connection.QueryFirstOrDefaultAsync<AgentWithCredentials>(
                "[dbo].[usp_get_remit_agent_credentials_byapiusername]", param, commandType: CommandType.StoredProcedure);
        }
    }
}
