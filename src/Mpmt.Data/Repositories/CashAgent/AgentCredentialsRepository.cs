using Dapper;
using Mpmt.Core.Domain.Payout;
using Mpmt.Core.Dtos.CashAgent;
using Mpmt.Core.Dtos.Partner;
using Mpmt.Core.Dtos.Roles;
using Mpmt.Core.ViewModel.CashAgent;
using Mpmt.Data.Common;
using Mpmts.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mpmt.Data.Repositories.CashAgent
{
    public class AgentCredentialsRepository : IAgentCredentialsRepository
    {
        public async Task<AgentCredential> GetCredentialsByAgentCodeAsync(string AgentCode)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@AgentCode ", AgentCode);

            return await connection
                .QueryFirstOrDefaultAsync<AgentCredential>("[dbo].[usp_get_Agent_credentials_ByAgentCode]", param, commandType: CommandType.StoredProcedure);
        }

        public async Task<AgentCredential> GetCredentialsByIdAsync(string credentialId)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@CredentialId", credentialId);

            return await connection
                .QueryFirstOrDefaultAsync<AgentCredential>("[dbo].[usp_get_Agent_credentials_ById]", param, commandType: CommandType.StoredProcedure);
        }

        public async Task<RemitCashAgentsDetails> GetDetailsByAgentCode(string AgentCode)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@AgentCode", AgentCode);
            var response = new RemitCashAgentsDetails();
            /* return await connection
                 .QueryMultipleA<RemitCashAgentsDetails>("[usp_get_agent_detail_by_agentcode]", param, commandType: CommandType.StoredProcedure);*/

            try {
                var superAgentDetails = await connection.QueryMultipleAsync("[usp_get_agent_detail_by_agentcode]", param, commandType: CommandType.StoredProcedure);
                
                response = await superAgentDetails.ReadFirstAsync<RemitCashAgentsDetails>();
                var Images = await superAgentDetails.ReadAsync<string>();
                response.DocumentImgPath = Images.ToList();
            }catch (Exception ex)
            {
                var res = new Exception(ex.Message);
            }
            return response;
        }

        public async Task<SprocMessage> InsertCredentialsAsync(AgentCredential cred)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
       

            var param = new DynamicParameters();
            param.Add("@OperationMode", cred.OperationMode);
            param.Add("@CredentialId", cred.CredentialId);
            param.Add("@AgentCode", cred.AgentCode);
            param.Add("@ApiUserName", cred.ApiUserName);
            param.Add("@ApiPassword", cred.ApiPassword);
            param.Add("@ApiKey", cred.ApiKey);
            param.Add("@SystemPrivateKey", cred.SystemPrivateKey);
            param.Add("@SystemPublicKey", cred.SystemPublicKey);
            param.Add("@UserPrivateKey", cred.UserPrivateKey);
            param.Add("@UserPublicKey", cred.UserPublicKey);
            param.Add("@IPAddress", cred.IPAddress);
            param.Add("@IsActive", cred.IsActive);
            param.Add("@LoggedInUserId", cred.CreatedById);
            param.Add("@LoggedInUserName", cred.CreatedByName);

            param.Add("@IdentityVal", dbType: DbType.String, size: 50, direction: ParameterDirection.Output);
            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

            _ = await connection.ExecuteAsync("[dbo].[usp_IUD_agent_credentials]", param, commandType: CommandType.StoredProcedure);

            var identityVal = param.Get<string>("@IdentityVal");
            var statusCode = param.Get<int>("@StatusCode");
            var msgType = param.Get<string>("@MsgType");
            var msgText = param.Get<string>("@MsgText");

            return new SprocMessage { StatusCode = statusCode, MsgType = msgType, MsgText = msgText };
        }

        public async Task<SprocMessage> UpdateApiKeyAsync(string AgentCode, string credentialId, string apiKey, string loggedInUserId = null, string loggedInUserName = null)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            // UAK => update api key
            const string operationMode = "UAK";

            var param = new DynamicParameters();
            param.Add("@OperationMode", operationMode);
            param.Add("@CredentialId", credentialId);
            param.Add("@AgentCode", AgentCode);
            param.Add("@ApiKey", apiKey);
            param.Add("@LoggedInUserId", loggedInUserId);
            param.Add("@LoggedInUserName", loggedInUserName);

            param.Add("@IdentityVal", dbType: DbType.String, size: 50, direction: ParameterDirection.Output);
            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

            _ = await connection.ExecuteAsync("[dbo].[usp_IUD_agent_credentials]", param, commandType: CommandType.StoredProcedure);

            var identityVal = param.Get<string>("@IdentityVal");
            var statusCode = param.Get<int>("@StatusCode");
            var msgType = param.Get<string>("@MsgType");
            var msgText = param.Get<string>("@MsgText");

            return new SprocMessage { StatusCode = statusCode, MsgType = msgType, MsgText = msgText };
        }

        public async  Task<SprocMessage> UpdateApiPasswordAsync(string AgentCode, string credentialId, string apiPassword, string loggedInUserId = null, string loggedInUserName = null)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            // UAP => update api password
            const string operationMode = "UAP";

            var param = new DynamicParameters();
            param.Add("@OperationMode", operationMode);
            param.Add("@CredentialId", credentialId);
            param.Add("@AgentCode", AgentCode);
            param.Add("@ApiPassword", apiPassword);
            param.Add("@LoggedInUserId", loggedInUserId);
            param.Add("@LoggedInUserName", loggedInUserName);

            param.Add("@IdentityVal", dbType: DbType.String, size: 50, direction: ParameterDirection.Output);
            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

            _ = await connection.ExecuteAsync("[dbo].[usp_IUD_agent_credentials]", param, commandType: CommandType.StoredProcedure);

            var identityVal = param.Get<string>("@IdentityVal");
            var statusCode = param.Get<int>("@StatusCode");
            var msgType = param.Get<string>("@MsgType");
            var msgText = param.Get<string>("@MsgText");

            return new SprocMessage { StatusCode = statusCode, MsgType = msgType, MsgText = msgText };
        }

        public async Task<SprocMessage> UpdateCredentialsAsync(AgentCredential cred)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            const string operationMode = "U";

            var param = new DynamicParameters();
            param.Add("@OperationMode", operationMode);
            param.Add("@CredentialId", cred.CredentialId);
            param.Add("@PartnerCode", cred.AgentCode);
            param.Add("@ApiUserName", cred.ApiUserName);
            param.Add("@ApiPassword", cred.ApiPassword);
            param.Add("@ApiKey", cred.ApiKey);
            param.Add("@SystemPrivateKey", cred.SystemPrivateKey);
            param.Add("@SystemPublicKey", cred.SystemPublicKey);
            param.Add("@UserPrivateKey", cred.UserPrivateKey);
            param.Add("@UserPublicKey", cred.UserPublicKey);
            param.Add("@IPAddress", cred.IPAddress);
            param.Add("@IsActive", cred.IsActive);
            param.Add("@LoggedInUserId", cred.UpdatedById);
            param.Add("@LoggedInUserName", cred.UpdatedByName);

            param.Add("@IdentityVal", dbType: DbType.String, size: 50, direction: ParameterDirection.Output);
            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

            _ = await connection.ExecuteAsync("[dbo].[usp_IUD_agent_credentials]", param, commandType: CommandType.StoredProcedure);

            var identityVal = param.Get<string>("@IdentityVal");
            var statusCode = param.Get<int>("@StatusCode");
            var msgType = param.Get<string>("@MsgType");
            var msgText = param.Get<string>("@MsgText");

            return new SprocMessage { StatusCode = statusCode, MsgType = msgType, MsgText = msgText };
        }

        public async Task<SprocMessage> UpdateSystemRsaKeyPairAsync(string AgentCode, string credentialId, string privateKey, string publicKey, string loggedInUserId = null, string loggedInUserName = null)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            // USRKP => Update system RSA key pair
            const string operationMode = "USRKP";

            var param = new DynamicParameters();
            param.Add("@OperationMode", operationMode);
            param.Add("@CredentialId", credentialId);
            param.Add("@AgentCode", AgentCode);
            param.Add("@SystemPrivateKey", privateKey);
            param.Add("@SystemPublicKey", publicKey);
            param.Add("@LoggedInUserId", loggedInUserId);
            param.Add("@LoggedInUserName", loggedInUserName);

            param.Add("@IdentityVal", dbType: DbType.String, size: 50, direction: ParameterDirection.Output);
            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);
            try
            {
                _ = await connection.ExecuteAsync("[dbo].[usp_IUD_agent_credentials]", param, commandType: CommandType.StoredProcedure);
            }catch (Exception ex)
            {

            }
           

            var identityVal = param.Get<string>("@IdentityVal");
            var statusCode = param.Get<int>("@StatusCode");
            var msgType = param.Get<string>("@MsgType");
            var msgText = param.Get<string>("@MsgText");

            return new SprocMessage { StatusCode = statusCode, MsgType = msgType, MsgText = msgText };
        }

        public async Task<SprocMessage> UpdateUserRsaKeyPairAsync(string AgentCode, string credentialId, string privateKey, string publicKey, string loggedInUserId = null, string loggedInUserName = null)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            // UURKP => Update user RSA key pair
            const string operationMode = "UURKP";

            var param = new DynamicParameters();
            param.Add("@OperationMode", operationMode);
            param.Add("@CredentialId", credentialId);
            param.Add("@AgentCode", AgentCode);
            param.Add("@UserPrivateKey", privateKey);
            param.Add("@UserPublicKey", publicKey);
            param.Add("@LoggedInUserId", loggedInUserId);
            param.Add("@LoggedInUserName", loggedInUserName);

            param.Add("@IdentityVal", dbType: DbType.String, size: 50, direction: ParameterDirection.Output);
            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

            _ = await connection.ExecuteAsync("[dbo].[usp_IUD_agent_credentials]", param, commandType: CommandType.StoredProcedure);

            var identityVal = param.Get<string>("@IdentityVal");
            var statusCode = param.Get<int>("@StatusCode");
            var msgType = param.Get<string>("@MsgType");
            var msgText = param.Get<string>("@MsgText");

            return new SprocMessage { StatusCode = statusCode, MsgType = msgType, MsgText = msgText };
        }
    }
}
