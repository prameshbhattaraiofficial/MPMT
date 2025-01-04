using Dapper;
using Mpmt.Core.Dtos.PartnerAction;
using Mpmt.Core.ViewModel.PartnerAction;
using Mpmt.Data.Common;
using Mpmts.Core.Dtos;
using System.Data;

namespace Mpmt.Data.Repositories.PartnerAction
{
    public class PartnerActionRepository: IPartnerActionRepository
    {
        /// <summary>
        /// Adds the action async.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> AddActionAsync(IUDPartnerAction action)
        {
            try
            {
                using var connection = DbConnectionManager.GetDefaultConnection();
                var param = new DynamicParameters();
                param.Add("@Event", "I");
                param.Add("Id", action.Id);
                param.Add("@Action", action.Action);
                param.Add("@DisplayOrder", action.DisplayOrder);
                param.Add("@IsActive", action.IsActive);

                param.Add("@LoggedInUser", "Admin");
                param.Add("@IdentityVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
                param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);
                _ = await connection.ExecuteAsync("[dbo].[Usp_IUD_partner_action]", param, commandType: CommandType.StoredProcedure);
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
        /// Gets the action async.
        /// </summary>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<PartnerActionModelView>> GetActionAsync()
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            return await connection.QueryAsync<PartnerActionModelView>("[dbo].[usp_get_partner_action]", commandType: CommandType.StoredProcedure);
        }




        /// <summary>
        /// Gets the action by id async.
        /// </summary>
        /// <param name="ActionId">The action id.</param>
        /// <returns>A Task.</returns>
        public async Task<IUDPartnerAction> GetActionByIdAsync(int ActionId)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            var param = new DynamicParameters();
            param.Add("@Id", ActionId);
            return await connection.QueryFirstOrDefaultAsync<IUDPartnerAction>("[dbo].[usp_get_partner_action_ById]", param, commandType: CommandType.StoredProcedure);
        }




        /// <summary>
        /// Removes the action async.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> RemoveActionAsync(IUDPartnerAction action)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            var param = new DynamicParameters();
            param.Add("@Event", "D");

            param.Add("Id", action.Id);
            param.Add("@Action", action.Action);
            param.Add("@DisplayOrder", action.DisplayOrder);
            param.Add("@IsActive", action.IsActive);
            param.Add("@LoggedInUser", 1);
            param.Add("@IdentityVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

            _ = await connection.ExecuteAsync("[dbo].[Usp_IUD_partner_action]", param, commandType: CommandType.StoredProcedure);

            var identityVal = param.Get<int>("@IdentityVal");
            var statusCode = param.Get<int>("@StatusCode");
            var msgType = param.Get<string>("@MsgType");
            var msgText = param.Get<string>("@MsgText");

            return new SprocMessage { IdentityVal = identityVal, StatusCode = statusCode, MsgType = msgType, MsgText = msgText };
        }



        /// <summary>
        /// Updates the action async.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> UpdateActionAsync(IUDPartnerAction action)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@Event", "U");
            param.Add("Id", action.Id);
            param.Add("@Action", action.Action);
            param.Add("@DisplayOrder", action.DisplayOrder);
            param.Add("@IsActive", action.IsActive);
            param.Add("@LoggedInUser", 1);
            param.Add("@IdentityVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

            _ = await connection.ExecuteAsync("[dbo].[Usp_IUD_partner_action]", param, commandType: CommandType.StoredProcedure);

            var identityVal = param.Get<int>("@IdentityVal");
            var statusCode = param.Get<int>("@StatusCode");
            var msgType = param.Get<string>("@MsgType");
            var msgText = param.Get<string>("@MsgText");



            return new SprocMessage { IdentityVal = identityVal, StatusCode = statusCode, MsgType = msgType, MsgText = msgText };
        }
    }
}
