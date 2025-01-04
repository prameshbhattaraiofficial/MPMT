using Dapper;
using Mpmt.Core.Dtos.CashAgent.Commission;
using Mpmt.Data.Common;
using Mpmts.Core.Dtos;
using System.Data;

namespace Mpmt.Data.Repositories.CashAgent
{
    public class CashAgentCommissionRepository : ICashAgentCommissionRepository
    {
        public async Task<SprocMessage> AddOrUpdateAsync(List<AgentCommissionRuleType> commissionRules, string agentCode, string superAgentCode, string agentType, string userType, string loggedinUser)
        {
            try
            {
                using var connection = DbConnectionManager.GetDefaultConnection();
                var dataTableRmp = GetDataTableCommissionRules();

                foreach (var rule in commissionRules)
                {
                    var row = dataTableRmp.NewRow();
                    row["MinTxnCount"] = rule.MinTxnCount;
                    row["MaxTxnCount"] = rule.MaxTxnCount;
                    row["Commission"] = rule.Commission;
                    row["MinCommission"] = rule.MinCommission;
                    row["MaxCommission"] = rule.MaxCommission;
                    row["FromDate"] = rule.FromDate;
                    row["ToDate"] = rule.ToDate;
                    dataTableRmp.Rows.Add(row);
                }

                var param = new DynamicParameters();
                param.Add("@AgentCode", agentCode);
                param.Add("@AgentCommissionSetting", dataTableRmp.AsTableValuedParameter("[dbo].[AgentCommissionSettingType]"));
                param.Add("@SuperAgentCode", superAgentCode);
                param.Add("@AgentType", agentType);
                param.Add("@UserType", userType);
                param.Add("@LoggedInUser", loggedinUser);

                param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
                param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

                var result = await connection.ExecuteAsync(
                    "[dbo].[usp_agent_commission_setting_addupdate]", param, commandType: CommandType.StoredProcedure);

                var statusCode = param.Get<int>("@StatusCode");
                var msgType = param.Get<string>("@MsgType");
                var msgText = param.Get<string>("@MsgText");

                return new SprocMessage { StatusCode = statusCode, MsgType = msgType, MsgText = msgText };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<AgentCommissionDetails> GetAgentCommissionDetailsAsync(string agentCode, string agentType)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@AgentCode", agentCode);
            param.Add("@AgentType", agentType);

            using var resultSets = await connection.QueryMultipleAsync(
                "[dbo].[usp_get_agent_commission_setting_by_agentcode]", param, commandType: CommandType.StoredProcedure);

            var agentInfoItem = await resultSets.ReadSingleOrDefaultAsync<AgentInfoItem>();
            if (agentInfoItem is null)
            {
                resultSets.Dispose();
                return null;
            }

            var commissionRuleList = await resultSets.ReadAsync<AgentCommissionRule>();

            var agentCommissionDetails = new AgentCommissionDetails
            {
                AgentCode = agentInfoItem.AgentCode,
                CategoryCode = agentInfoItem.CategoryCode,
                CategoryName = agentInfoItem.CategoryName,
                SuperAgentCode = agentInfoItem.SuperAgentCode,
                AgentType = agentInfoItem.AgentType,
                CommissionRuleList = commissionRuleList
            };

            return agentCommissionDetails;
        }

        public async Task<SprocMessage> AddOrUpdateDefaultRulesAsync(List<AgentCommissionRuleType> commissionRules, string userType, string loggedinUser)
        {
            try
            {
                using var connection = DbConnectionManager.GetDefaultConnection();
                var dataTableRmp = GetDataTableCommissionRules();

                foreach (var rule in commissionRules)
                {
                    var row = dataTableRmp.NewRow();
                    row["MinTxnCount"] = rule.MinTxnCount;
                    row["MaxTxnCount"] = rule.MaxTxnCount;
                    row["Commission"] = rule.Commission;
                    row["MinCommission"] = rule.MinCommission;
                    row["MaxCommission"] = rule.MaxCommission;
                    row["FromDate"] = rule.FromDate is null ? DBNull.Value : rule.FromDate;
                    row["ToDate"] = rule.ToDate is null ? DBNull.Value : rule.ToDate;
                    dataTableRmp.Rows.Add(row);
                }

                var param = new DynamicParameters();
                param.Add("@AgentCommissionSetting", dataTableRmp.AsTableValuedParameter("[dbo].[AgentCommissionSettingType]"));
                param.Add("@UserType", userType);
                param.Add("@LoggedInUser", loggedinUser);

                param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
                param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

                var result = await connection
                    .ExecuteAsync("[dbo].[usp_default_agent_commission_setting_addupdate]", param, commandType: CommandType.StoredProcedure);

                var statusCode = param.Get<int>("@StatusCode");
                var msgType = param.Get<string>("@MsgType");
                var msgText = param.Get<string>("@MsgText");

                return new SprocMessage { StatusCode = statusCode, MsgType = msgType, MsgText = msgText };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<AgentDefaultCommissionRule>> GetAgentDefaultCommissionDetailsAsync()
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var rules = await connection
                .QueryAsync<AgentDefaultCommissionRule>("[dbo].[usp_get_defult_agent_commission_setting]", commandType: CommandType.StoredProcedure);

            return rules;
        }

        private DataTable GetDataTableCommissionRules()
        {
            var dt = new DataTable();
            dt.Columns.Add("MinTxnCount", typeof(int)).AllowDBNull = true;
            dt.Columns.Add("MaxTxnCount", typeof(int)).AllowDBNull = true;
            dt.Columns.Add("Commission", typeof(decimal)).AllowDBNull = true;
            dt.Columns.Add("MinCommission", typeof(decimal)).AllowDBNull = true;
            dt.Columns.Add("MaxCommission", typeof(decimal)).AllowDBNull = true;
            dt.Columns.Add("FromDate", typeof(DateTime)).AllowDBNull = true;
            dt.Columns.Add("ToDate", typeof(DateTime)).AllowDBNull = true;

            return dt;
        }

        public async Task<IEnumerable<AgentCommissionRule>> GetCommissionSlabsBySuperAgent(string agentCode)
        {
            try
            {
                using var connection = DbConnectionManager.GetDefaultConnection();

                var param = new DynamicParameters();
                param.Add("@SuperAgentCode", agentCode);

                using var resultSets = await connection.QueryMultipleAsync(
                    "[dbo].[usp_get_commission_slabs_by_superagentcode]", param, commandType: CommandType.StoredProcedure);

                var commissionRuleList = await resultSets.ReadAsync<AgentCommissionRule>();

                return commissionRuleList;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
