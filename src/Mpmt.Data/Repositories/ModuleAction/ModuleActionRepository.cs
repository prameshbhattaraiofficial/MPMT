using Dapper;
using Mpmt.Core.Dtos.ModuleAction;
using Mpmt.Core.ViewModel.ModuleAction;
using Mpmt.Data.Common;
using Mpmts.Core.Dtos;
using System.Data;

namespace Mpmt.Data.Repositories.ModuleAction
{
    /// <summary>
    /// The module action repository.
    /// </summary>
    public class ModuleActionRepository : IModuleActionRepository
    {

        /// <summary>
        /// Adds the module action async.
        /// </summary>
        /// <param name="moduleaction">The moduleaction.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> AddModuleActionAsync(IUDModuleAction moduleaction)
        {
            var identityVal = 0;
            var statusCode = 0;
            var msgType = string.Empty;
            var msgText = string.Empty;
            try
            {
                foreach (var item in moduleaction.ActionIds)
                {
                    using var connection = DbConnectionManager.GetDefaultConnection();
                    var param = new DynamicParameters();
                    param.Add("@Event", "I");
                    param.Add("Id", moduleaction.Id);
                    param.Add("@ModuleId", moduleaction.ModuleId);
                    param.Add("@ActionId", item);
                    param.Add("@ActionName", moduleaction.ActionName);
                    param.Add("@DisplayOrder", moduleaction.DisplayOrder);
                    param.Add("@IsActive", moduleaction.IsActive);
                    param.Add("@LoggedInUser", 1);
                    param.Add("@IdentityVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
                    param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
                    param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
                    param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);
                    _ = await connection.ExecuteAsync("[dbo].[Usp_IUD_module_action]", param, commandType: CommandType.StoredProcedure);
                    identityVal = param.Get<int>("@IdentityVal");
                    statusCode = param.Get<int>("@StatusCode");
                    msgType = param.Get<string>("@MsgType");
                    msgText = param.Get<string>("@MsgText");


                }
                return new SprocMessage { IdentityVal = identityVal, StatusCode = statusCode, MsgType = msgType, MsgText = msgText };
            }
            catch (Exception)
            {
                throw;
            }
        }




        /// <summary>
        /// Gets the module action async.
        /// </summary>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<ModuleActionModelView>> GetModuleActionAsync()
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            return await connection.QueryAsync<ModuleActionModelView>("[dbo].[usp_get_module_action]", commandType: CommandType.StoredProcedure);
        }




        /// <summary>
        /// Gets the module action by id async.
        /// </summary>
        /// <param name="ModuleId">The module id.</param>
        /// <returns>A Task.</returns>
        public async Task<IUDModuleAction> GetModuleActionByIdAsync(int ModuleActionId)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            var param = new DynamicParameters();
            param.Add("@Id", ModuleActionId);
            var result = await connection.QueryAsync<IUDModuleAction>("[dbo].[usp_get_module_action_ById]", param, commandType: CommandType.StoredProcedure);
            int[] array = new int[result.Count()];
            var i = 0;
            foreach (var item in result)
            {
                array[i] = item.ActionId;
                i++;
            }

            var data = result.FirstOrDefault();
            data.ActionIds = array;

            return data;
        }




        /// <summary>
        /// Removes the module action async.
        /// </summary>
        /// <param name="moduleaction">The moduleaction.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> RemoveModuleActionAsync(IUDModuleAction moduleaction)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            var param = new DynamicParameters();
            param.Add("@Event", "D");

            param.Add("Id", moduleaction.Id);
            param.Add("@ModuleId", moduleaction.ModuleId);
            param.Add("@ActionId", moduleaction.ActionId);
            param.Add("@ActionName", moduleaction.ActionName);
            param.Add("@DisplayOrder", moduleaction.DisplayOrder);
            param.Add("@IsActive", moduleaction.IsActive);

            param.Add("@LoggedInUser", 1);
            param.Add("@IdentityVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

            _ = await connection.ExecuteAsync("[dbo].[Usp_IUD_module_action]", param, commandType: CommandType.StoredProcedure);

            var identityVal = param.Get<int>("@IdentityVal");
            var statusCode = param.Get<int>("@StatusCode");
            var msgType = param.Get<string>("@MsgType");
            var msgText = param.Get<string>("@MsgText");

            return new SprocMessage { IdentityVal = identityVal, StatusCode = statusCode, MsgType = msgType, MsgText = msgText };
        }




        /// <summary>
        /// Updates the module action async.
        /// </summary>
        /// <param name="moduleaction">The moduleaction.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> UpdateModuleActionAsync(IUDModuleAction moduleaction)
        {
            var identityVal = 0;
            var statusCode = 0;
            var msgType = string.Empty;
            var msgText = string.Empty;
            foreach (var item in moduleaction.ActionIds)
            {
                using var connection = DbConnectionManager.GetDefaultConnection();
                var param = new DynamicParameters();
                param.Add("@Event", "U");
                param.Add("Id", moduleaction.Id);
                param.Add("@ModuleId", moduleaction.ModuleId);
                param.Add("@ActionId", item);
                param.Add("@ActionName", moduleaction.ActionName);
                param.Add("@DisplayOrder", moduleaction.DisplayOrder);
                param.Add("@IsActive", moduleaction.IsActive);
                param.Add("@LoggedInUser", 1);
                param.Add("@IdentityVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
                param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

                _ = await connection.ExecuteAsync("[dbo].[Usp_IUD_module_action]", param, commandType: CommandType.StoredProcedure);

                identityVal = param.Get<int>("@IdentityVal");
                statusCode = param.Get<int>("@StatusCode");
                msgType = param.Get<string>("@MsgType");
                msgText = param.Get<string>("@MsgText");
            }

            return new SprocMessage { IdentityVal = identityVal, StatusCode = statusCode, MsgType = msgType, MsgText = msgText };
        }


    }
}
