using Dapper;
using Mpmt.Core.Dtos.Partner;
using Mpmt.Data.Common;
using Mpmt.Data.Repositories.Partner.IRepository;
using Mpmts.Core.Dtos;
using System.Data;

namespace Mpmt.Data.Repositories.Partner.Repository
{
    /// <summary>
    /// The partner credentials repository.
    /// </summary>
    public class PartnerCredentialsRepository : IPartnerCredentialsRepository
    {
        /// <summary>
        /// Gets the credentials by id async.
        /// </summary>
        /// <param name="credentialId">The credential id.</param>
        /// <returns>A Task.</returns>
        public async Task<PartnerCredential> GetCredentialsByIdAsync(string credentialId)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@CredentialId", credentialId);

            return await connection
                .QueryFirstOrDefaultAsync<PartnerCredential>("[dbo].[usp_get_partner_credentials_ById]", param, commandType: CommandType.StoredProcedure);
        }
        /// <summary>
        /// Gets the credentials by partner code async.
        /// </summary>
        /// <param name="PartnerCode">The partner code.</param>
        /// <returns>A Task.</returns>
        public async Task<PartnerCredential> GetCredentialsByPartnerCodeAsync(string PartnerCode)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@PartnerCode ", PartnerCode);

            return await connection
                .QueryFirstOrDefaultAsync<PartnerCredential>("[dbo].[usp_get_partner_credentials_ByPartnerCode]", param, commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Inserts the credentials async.
        /// </summary>
        /// <param name="cred">The cred.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> InsertCredentialsAsync(PartnerCredential cred)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            const string operationMode = "I";

            var param = new DynamicParameters();
            param.Add("@OperationMode", operationMode);
            param.Add("@CredentialId", cred.CredentialId);
            param.Add("@PartnerCode", cred.PartnerCode);
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

            _ = await connection.ExecuteAsync("[dbo].[usp_iud_partner_credentials]", param, commandType: CommandType.StoredProcedure);

            var identityVal = param.Get<string>("@IdentityVal");
            var statusCode = param.Get<int>("@StatusCode");
            var msgType = param.Get<string>("@MsgType");
            var msgText = param.Get<string>("@MsgText");

            return new SprocMessage { StatusCode = statusCode, MsgType = msgType, MsgText = msgText };
        }

        /// <summary>
        /// Updates the api key async.
        /// </summary>
        /// <param name="partnerCode">The partner code.</param>
        /// <param name="credentialId">The credential id.</param>
        /// <param name="apiKey">The api key.</param>
        /// <param name="loggedInUserId">The logged in user id.</param>
        /// <param name="loggedInUserName">The logged in user name.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> UpdateApiKeyAsync(string partnerCode, string credentialId, string apiKey, string loggedInUserId = null, string loggedInUserName = null)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            // UAK => update api key
            const string operationMode = "UAK";

            var param = new DynamicParameters();
            param.Add("@OperationMode", operationMode);
            param.Add("@CredentialId", credentialId);
            param.Add("@PartnerCode", partnerCode);
            param.Add("@ApiKey", apiKey);
            param.Add("@LoggedInUserId", loggedInUserId);
            param.Add("@LoggedInUserName", loggedInUserName);

            param.Add("@IdentityVal", dbType: DbType.String, size: 50, direction: ParameterDirection.Output);
            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

            _ = await connection.ExecuteAsync("[dbo].[usp_iud_partner_credentials]", param, commandType: CommandType.StoredProcedure);

            var identityVal = param.Get<string>("@IdentityVal");
            var statusCode = param.Get<int>("@StatusCode");
            var msgType = param.Get<string>("@MsgType");
            var msgText = param.Get<string>("@MsgText");

            return new SprocMessage { StatusCode = statusCode, MsgType = msgType, MsgText = msgText };
        }

        /// <summary>
        /// Updates the api password async.
        /// </summary>
        /// <param name="partnerCode">The partner code.</param>
        /// <param name="credentialId">The credential id.</param>
        /// <param name="apiPassword">The api password.</param>
        /// <param name="loggedInUserId">The logged in user id.</param>
        /// <param name="loggedInUserName">The logged in user name.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> UpdateApiPasswordAsync(string partnerCode, string credentialId, string apiPassword, string loggedInUserId = null, string loggedInUserName = null)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            // UAP => update api password
            const string operationMode = "UAP";

            var param = new DynamicParameters();
            param.Add("@OperationMode", operationMode);
            param.Add("@CredentialId", credentialId);
            param.Add("@PartnerCode", partnerCode);
            param.Add("@ApiPassword", apiPassword);
            param.Add("@LoggedInUserId", loggedInUserId);
            param.Add("@LoggedInUserName", loggedInUserName);

            param.Add("@IdentityVal", dbType: DbType.String, size: 50, direction: ParameterDirection.Output);
            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

            _ = await connection.ExecuteAsync("[dbo].[usp_iud_partner_credentials]", param, commandType: CommandType.StoredProcedure);

            var identityVal = param.Get<string>("@IdentityVal");
            var statusCode = param.Get<int>("@StatusCode");
            var msgType = param.Get<string>("@MsgType");
            var msgText = param.Get<string>("@MsgText");

            return new SprocMessage { StatusCode = statusCode, MsgType = msgType, MsgText = msgText };
        }

        /// <summary>
        /// Updates the credentials async.
        /// </summary>
        /// <param name="cred">The cred.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> UpdateCredentialsAsync(PartnerCredential cred)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            const string operationMode = "U";

            var param = new DynamicParameters();
            param.Add("@OperationMode", operationMode);
            param.Add("@CredentialId", cred.CredentialId);
            param.Add("@PartnerCode", cred.PartnerCode);
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

            _ = await connection.ExecuteAsync("[dbo].[usp_iud_partner_credentials]", param, commandType: CommandType.StoredProcedure);

            var identityVal = param.Get<string>("@IdentityVal");
            var statusCode = param.Get<int>("@StatusCode");
            var msgType = param.Get<string>("@MsgType");
            var msgText = param.Get<string>("@MsgText");

            return new SprocMessage { StatusCode = statusCode, MsgType = msgType, MsgText = msgText };
        }

        /// <summary>
        /// Updates the system rsa key pair async.
        /// </summary>
        /// <param name="partnerCode">The partner code.</param>
        /// <param name="credentialId">The credential id.</param>
        /// <param name="privateKey">The private key.</param>
        /// <param name="publicKey">The public key.</param>
        /// <param name="loggedInUserId">The logged in user id.</param>
        /// <param name="loggedInUserName">The logged in user name.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> UpdateSystemRsaKeyPairAsync(string partnerCode, string credentialId, string privateKey, string publicKey, string loggedInUserId = null, string loggedInUserName = null)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            // USRKP => Update system RSA key pair
            const string operationMode = "USRKP";

            var param = new DynamicParameters();
            param.Add("@OperationMode", operationMode);
            param.Add("@CredentialId", credentialId);
            param.Add("@PartnerCode", privateKey);
            param.Add("@SystemPrivateKey", privateKey);
            param.Add("@SystemPublicKey", publicKey);
            param.Add("@LoggedInUserId", loggedInUserId);
            param.Add("@LoggedInUserName", loggedInUserName);

            param.Add("@IdentityVal", dbType: DbType.String, size: 50, direction: ParameterDirection.Output);
            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

            _ = await connection.ExecuteAsync("[dbo].[usp_iud_partner_credentials]", param, commandType: CommandType.StoredProcedure);

            var identityVal = param.Get<string>("@IdentityVal");
            var statusCode = param.Get<int>("@StatusCode");
            var msgType = param.Get<string>("@MsgType");
            var msgText = param.Get<string>("@MsgText");

            return new SprocMessage { StatusCode = statusCode, MsgType = msgType, MsgText = msgText };
        }

        /// <summary>
        /// Updates the user rsa key pair async.
        /// </summary>
        /// <param name="partnerCode">The partner code.</param>
        /// <param name="credentialId">The credential id.</param>
        /// <param name="privateKey">The private key.</param>
        /// <param name="publicKey">The public key.</param>
        /// <param name="loggedInUserId">The logged in user id.</param>
        /// <param name="loggedInUserName">The logged in user name.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> UpdateUserRsaKeyPairAsync(string partnerCode, string credentialId, string privateKey, string publicKey, string loggedInUserId = null, string loggedInUserName = null)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            // UURKP => Update user RSA key pair
            const string operationMode = "UURKP";

            var param = new DynamicParameters();
            param.Add("@OperationMode", operationMode);
            param.Add("@CredentialId", credentialId);
            param.Add("@PartnerCode", privateKey);
            param.Add("@UserPrivateKey", privateKey);
            param.Add("@UserPublicKey", publicKey);
            param.Add("@LoggedInUserId", loggedInUserId);
            param.Add("@LoggedInUserName", loggedInUserName);

            param.Add("@IdentityVal", dbType: DbType.String, size: 50, direction: ParameterDirection.Output);
            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

            _ = await connection.ExecuteAsync("[dbo].[usp_iud_partner_credentials]", param, commandType: CommandType.StoredProcedure);

            var identityVal = param.Get<string>("@IdentityVal");
            var statusCode = param.Get<int>("@StatusCode");
            var msgType = param.Get<string>("@MsgType");
            var msgText = param.Get<string>("@MsgText");

            return new SprocMessage { StatusCode = statusCode, MsgType = msgType, MsgText = msgText };
        }
    }
}
