using Dapper;
using Mpmt.Core.Dtos.PartnerModule;
using Mpmt.Core.ViewModel.PartnerModule;
using Mpmt.Data.Common;
using Mpmts.Core.Dtos;
using System.Data;

namespace Mpmt.Data.Repositories.Module
{
    /// <summary>
    /// The module repository.
    /// </summary>
    public class PartnerModuleRepository : IPartnerModuleRepository
    {


        /// <summary>
        /// Adds the module async.
        /// </summary>
        /// <param name="module">The module.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> AddModuleAsync(IUDPartnerModule module)
        {
            try
            {
                using var connection = DbConnectionManager.GetDefaultConnection();
                var param = new DynamicParameters();
                param.Add("@Event", "I");
                param.Add("Id", module.Id);
                param.Add("@Module", module.Module);
                param.Add("@ParentId", module.ParentId);
                param.Add("@DisplayOrder", module.DisplayOrder);
                param.Add("@IsActive", module.IsActive);

                param.Add("@LoggedInUser", 1);
                param.Add("@IdentityVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
                param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);
                _ = await connection.ExecuteAsync("[dbo].[Usp_IUD_partner_module]", param, commandType: CommandType.StoredProcedure);
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
        /// Gets the module async.
        /// </summary>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<PartnerModuleModelView>> GetModuleAsync()
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            return await connection.QueryAsync<PartnerModuleModelView>("[dbo].[usp_get_partner_module]", commandType: CommandType.StoredProcedure);
        }



        /// <summary>
        /// Gets the module by id async.
        /// </summary>
        /// <param name="ModuleId">The module id.</param>
        /// <returns>A Task.</returns>
        public async Task<IUDPartnerModule> GetModuleByIdAsync(int ModuleId)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            var param = new DynamicParameters();
            param.Add("@Id", ModuleId);
            return await connection.QueryFirstOrDefaultAsync<IUDPartnerModule>("[dbo].[usp_get_partner_module_ById]", param, commandType: CommandType.StoredProcedure);
        }



        /// <summary>
        /// Removes the module async.
        /// </summary>
        /// <param name="module">The module.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> RemoveModuleAsync(IUDPartnerModule module)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            var param = new DynamicParameters();
            param.Add("@Event", "D");

            param.Add("Id", module.Id);
            param.Add("@Module", module.Module);
            param.Add("@ParentId", module.ParentId);
            param.Add("@DisplayOrder", module.DisplayOrder);
            param.Add("@IsActive", module.IsActive);
            param.Add("@LoggedInUser", 1);
            param.Add("@IdentityVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

            _ = await connection.ExecuteAsync("[dbo].[Usp_IUD_partner_module]", param, commandType: CommandType.StoredProcedure);

            var identityVal = param.Get<int>("@IdentityVal");
            var statusCode = param.Get<int>("@StatusCode");
            var msgType = param.Get<string>("@MsgType");
            var msgText = param.Get<string>("@MsgText");

            return new SprocMessage { IdentityVal = identityVal, StatusCode = statusCode, MsgType = msgType, MsgText = msgText };
        }



        /// <summary>
        /// Updates the module async.
        /// </summary>
        /// <param name="module">The module.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> UpdateModuleAsync(IUDPartnerModule module)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@Event", "U");
            param.Add("Id", module.Id);
            param.Add("@Module", module.Module);
            param.Add("@ParentId", module.ParentId);
            param.Add("@DisplayOrder", module.DisplayOrder);
            param.Add("@IsActive", module.IsActive);
            param.Add("@LoggedInUser", 1);
            param.Add("@IdentityVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

            _ = await connection.ExecuteAsync("[dbo].[Usp_IUD_partner_module]", param, commandType: CommandType.StoredProcedure);

            var identityVal = param.Get<int>("@IdentityVal");
            var statusCode = param.Get<int>("@StatusCode");
            var msgType = param.Get<string>("@MsgType");
            var msgText = param.Get<string>("@MsgText");


            return new SprocMessage { IdentityVal = identityVal, StatusCode = statusCode, MsgType = msgType, MsgText = msgText };
        }
        /// <summary>
        /// Updates the module display order async.
        /// </summary>
        /// <param name="moduleUpdate">The module update.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> UpdateModuleDisplayOrderAsync(IUDPartnerModule moduleUpdate)

        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("Id", moduleUpdate.Id);
            param.Add("@DisplayOrder", moduleUpdate.DisplayOrder);
            param.Add("@LoggedInUser", 1);
            param.Add("@IdentityVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

            _ = await connection.ExecuteAsync("[dbo].[usp_module_displayOrder]", param, commandType: CommandType.StoredProcedure);

            var identityVal = param.Get<int>("@IdentityVal");
            var statusCode = param.Get<int>("@StatusCode");
            var msgType = param.Get<string>("@MsgType");
            var msgText = param.Get<string>("@MsgText");
            return new SprocMessage { IdentityVal = identityVal, StatusCode = statusCode, MsgType = msgType, MsgText = msgText };

        }
        /// <summary>
        /// Updates the module is active async.
        /// </summary>
        /// <param name="moduleUpdate">The module update.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> UpdateModuleIsActiveAsync(IUDPartnerModule moduleUpdate)

        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("Id", moduleUpdate.Id);
            param.Add("@IsActive", moduleUpdate.IsActive);
            param.Add("@LoggedInUser", 1);
            param.Add("@IdentityVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

            _ = await connection.ExecuteAsync("[dbo].[usp_module_UpdateIsActive]", param, commandType: CommandType.StoredProcedure);

            var identityVal = param.Get<int>("@IdentityVal");
            var statusCode = param.Get<int>("@StatusCode");
            var msgType = param.Get<string>("@MsgType");
            var msgText = param.Get<string>("@MsgText");
            return new SprocMessage { IdentityVal = identityVal, StatusCode = statusCode, MsgType = msgType, MsgText = msgText };

        }

    }
}
