using Dapper;
using Mpmt.Core.Dtos.TransferPurpose;
using Mpmt.Data.Common;
using Mpmts.Core.Dtos;
using System.Data;

namespace Mpmt.Data.Repositories.TransferPurpose
{
    /// <summary>
    /// The transfer purpose repo.
    /// </summary>
    public class TransferPurposeRepo : ITransferPurposeRepo
    {
        /// <summary>
        /// Adds the transfer purpose async.
        /// </summary>
        /// <param name="addTransferPurpose">The add transfer purpose.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> AddTransferPurposeAsync(IUDTransferPurpose addTransferPurpose)
        {
            try
            {
                using var connection = DbConnectionManager.GetDefaultConnection();
                var param = new DynamicParameters();
                param.Add("@Event", "I");
                param.Add("Id", addTransferPurpose.Id);
                param.Add("@PurposeName", addTransferPurpose.PurposeName);
                param.Add("@Description", addTransferPurpose.Description);
                param.Add("@IsActive", addTransferPurpose.IsActive);
                param.Add("@LoggedInUser", 1);
                param.Add("@LoggedInUserName", "Admin");
                param.Add("@IdentityVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
                param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

                _ = await connection.ExecuteAsync("[dbo].[Usp_IUD_transfer_purpose]", param, commandType: CommandType.StoredProcedure);

                var identityVal = param.Get<int>("@IdentityVal");
                var statusCode = param.Get<int>("@StatusCode");
                var msgType = param.Get<string>("@MsgType");
                var msgText = param.Get<string>("@MsgText");

                return new SprocMessage { IdentityVal = identityVal, StatusCode = statusCode, MsgType = msgType, MsgText = msgText };
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// Gets the transfer purpose async.
        /// </summary>
        /// <param name="transferPurposeFilter">The transfer purpose filter.</param>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<TransferPurposeDetails>> GetTransferPurposeAsync(TransferPurposeFilter transferPurposeFilter)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            var param = new DynamicParameters();
            param.Add("@TransferPurposeName", transferPurposeFilter.TransferPurposeName);
            param.Add("@Status", transferPurposeFilter.Status);
            return await connection.QueryAsync<TransferPurposeDetails>("[dbo].[usp_get_transfer_purpose]", param, commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Gets the transfer purpose by id async.
        /// </summary>
        /// <param name="transferPurposeId">The transfer purpose id.</param>
        /// <returns>A Task.</returns>
        public async Task<TransferPurposeDetails> GetTransferPurposeByIdAsync(int transferPurposeId)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            var param = new DynamicParameters();
            param.Add("@Id", transferPurposeId);
            return await connection.QueryFirstOrDefaultAsync<TransferPurposeDetails>("[dbo].[usp_get_transferpurpose_ById]", param, commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Removes the transfer purpose async.
        /// </summary>
        /// <param name="removeTransferPurpose">The remove transfer purpose.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> RemoveTransferPurposeAsync(IUDTransferPurpose removeTransferPurpose)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            var param = new DynamicParameters();

            param.Add("@Event", "D");
            param.Add("Id", removeTransferPurpose.Id);
            param.Add("@PurposeName", removeTransferPurpose.PurposeName);
            param.Add("@Description", removeTransferPurpose.Description);
            param.Add("@IsActive", removeTransferPurpose.IsActive);
            param.Add("@LoggedInUser", 1);
            param.Add("@LoggedInUserName", "Admin");
            param.Add("@IdentityVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

            _ = await connection.ExecuteAsync("[dbo].[Usp_IUD_transfer_purpose]", param, commandType: CommandType.StoredProcedure);

            var identityVal = param.Get<int>("@IdentityVal");
            var statusCode = param.Get<int>("@StatusCode");
            var msgType = param.Get<string>("@MsgType");
            var msgText = param.Get<string>("@MsgText");

            return new SprocMessage { IdentityVal = identityVal, StatusCode = statusCode, MsgType = msgType, MsgText = msgText };
        }

        /// <summary>
        /// Updates the transfer purpose async.
        /// </summary>
        /// <param name="updateTransferPurpose">The update transfer purpose.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> UpdateTransferPurposeAsync(IUDTransferPurpose updateTransferPurpose)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            var param = new DynamicParameters();
            param.Add("@Event", "U");
            param.Add("Id", updateTransferPurpose.Id);
            param.Add("@PurposeName", updateTransferPurpose.PurposeName);
            param.Add("@Description", updateTransferPurpose.Description);
            param.Add("@IsActive", updateTransferPurpose.IsActive);
            param.Add("@LoggedInUser", 1);
            param.Add("@LoggedInUserName", "Admin");
            param.Add("@IdentityVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

            _ = await connection.ExecuteAsync("[dbo].[Usp_IUD_transfer_purpose]", param, commandType: CommandType.StoredProcedure);

            var identityVal = param.Get<int>("@IdentityVal");
            var statusCode = param.Get<int>("@StatusCode");
            var msgType = param.Get<string>("@MsgType");
            var msgText = param.Get<string>("@MsgText");

            return new SprocMessage { IdentityVal = identityVal, StatusCode = statusCode, MsgType = msgType, MsgText = msgText };
        }
    }
}
