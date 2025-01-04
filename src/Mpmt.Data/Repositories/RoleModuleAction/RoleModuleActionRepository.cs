using Dapper;
using Microsoft.AspNetCore.Http;
using Mpmt.Core.Dtos.RoleModuleAction;
using Mpmt.Core.Models.Route;
using Mpmt.Core.ViewModel.AreaControllerAction;
using Mpmt.Core.ViewModel.RoleModuleAction;
using Mpmt.Data.Common;
using Mpmts.Core.Dtos;
using System.Data;

namespace Mpmt.Data.Repositories.RoleModuleAction
{
    /// <summary>
    /// The role module action repository.
    /// </summary>
    public class RoleModuleActionRepository : IRoleModuleActionRepository
    {

        /// <summary>
        /// Adds the role module action async.
        /// </summary>
        /// <param name="rolemoduleaction">The rolemoduleaction.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> AddRoleModuleActionAsync(IUDRoleModuleAction rolemoduleaction)
        {

            var identityVal = 0;
            var statusCode = 0;
            var msgType = string.Empty;
            var msgText = string.Empty;

            try
            {
                foreach (var item in rolemoduleaction.ActionIds)
                {
                    using var connection = DbConnectionManager.GetDefaultConnection();
                    var param = new DynamicParameters();
                    param.Add("@Event", "I");
                    param.Add("Id", rolemoduleaction.Id);
                    param.Add("@ModuleId", rolemoduleaction.ModuleId);
                    param.Add("@ActionId", item);
                    param.Add("@RoleId", rolemoduleaction.RoleId);
                    param.Add("@Module", rolemoduleaction.Module);
                    param.Add("@Action", rolemoduleaction.Action);
                    param.Add("@LoggedInUser", 1);
                    param.Add("@IdentityVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
                    param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
                    param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
                    param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);
                    _ = await connection.ExecuteAsync("[dbo].[Usp_IUD_role_module_action]", param, commandType: CommandType.StoredProcedure);
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
        /// Gets the role module action async.
        /// </summary>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<RoleModuleActionModelView>> GetRoleModuleActionAsync()
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            return await connection.QueryAsync<RoleModuleActionModelView>("[dbo].[usp_get_role_module_Action]", commandType: CommandType.StoredProcedure);
        }
      
        public async Task<IEnumerable<AreaControllerAction>> GetAreaControllerAction()
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            var result = await connection.QueryAsync<AreaControllerAction>("[dbo].[usp_get_area_controller_action]", commandType: CommandType.StoredProcedure);
            return result;
        }
        public async Task<IEnumerable<AreaControllerAction>> GetAreaControllerActionAgent()
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            var result = await connection.QueryAsync<AreaControllerAction>("[dbo].[usp_get_area_controller_action_agent]", commandType: CommandType.StoredProcedure);
            return result;
        }


        /// <summary>
        /// Gets the role module action by id async.
        /// </summary>
        /// <param name="RoleModuleActionId">The role module action id.</param>
        /// <returns>A Task.</returns>
        public async Task<IUDRoleModuleAction> GetRoleModuleActionByIdAsync(int RoleModuleActionId)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            var param = new DynamicParameters();
            param.Add("@Id", RoleModuleActionId);
            var result = await connection.QueryAsync<IUDRoleModuleAction>("[dbo].[usp_get_role_module_action_ById]", param, commandType: CommandType.StoredProcedure);
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
        /// Removes the role module action async.
        /// </summary>
        /// <param name="rolemoduleaction">The rolemoduleaction.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> RemoveRoleModuleActionAsync(IUDRoleModuleAction rolemoduleaction)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            var param = new DynamicParameters();
            param.Add("@Event", "D");
            param.Add("Id", rolemoduleaction.Id);
            param.Add("@ModuleId", rolemoduleaction.ModuleId);
            param.Add("@ActionId", rolemoduleaction.ActionId);
            param.Add("@RoleId", rolemoduleaction.RoleId);
            param.Add("@Module", rolemoduleaction.Module);
            param.Add("@Action", rolemoduleaction.Action);
            param.Add("@LoggedInUser", 1);
            param.Add("@IdentityVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

            _ = await connection.ExecuteAsync("[dbo].[Usp_IUD_role_module_action]", param, commandType: CommandType.StoredProcedure);

            var identityVal = param.Get<int>("@IdentityVal");
            var statusCode = param.Get<int>("@StatusCode");
            var msgType = param.Get<string>("@MsgType");
            var msgText = param.Get<string>("@MsgText");

            return new SprocMessage { IdentityVal = identityVal, StatusCode = statusCode, MsgType = msgType, MsgText = msgText };
        }





        /// <summary>
        /// Updates the role module action async.
        /// </summary>
        /// <param name="rolemoduleaction">The rolemoduleaction.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> UpdateRoleModuleActionAsync(IUDRoleModuleAction rolemoduleaction)
        {
            var identityVal = 0;
            var statusCode = 0;
            var msgType = string.Empty;
            var msgText = string.Empty;
            foreach (var item in rolemoduleaction.ActionIds)
            {
                using var connection = DbConnectionManager.GetDefaultConnection();

                var param = new DynamicParameters();
                param.Add("@Event", "U");
                param.Add("Id", rolemoduleaction.Id);
                param.Add("@ModuleId", rolemoduleaction.ModuleId);
                param.Add("@ActionId", item);
                param.Add("@RoleId", rolemoduleaction.RoleId);
                param.Add("@Module", rolemoduleaction.Module);
                param.Add("@Action", rolemoduleaction.Action);
                param.Add("@LoggedInUser", 1);
                param.Add("@IdentityVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
                param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

                _ = await connection.ExecuteAsync("[dbo].[Usp_IUD_role_module_action]", param, commandType: CommandType.StoredProcedure);

                identityVal = param.Get<int>("@IdentityVal");
                statusCode = param.Get<int>("@StatusCode");
                msgType = param.Get<string>("@MsgType");
                msgText = param.Get<string>("@MsgText");
            }

            return new SprocMessage { IdentityVal = identityVal, StatusCode = statusCode, MsgType = msgType, MsgText = msgText };
        }
        public async Task<SprocMessage> InsertControllerActionAsync(Controlleraction controlleraction)
        {
            //todo use TVP
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("Id", 0);
            param.Add("@Areacontrolleraction", controlleraction.Areacontrolleraction);
            param.Add("@Area", controlleraction.Area);
            param.Add("@Controller", controlleraction.Controller);
            param.Add("@Action", controlleraction.Action);
            param.Add("@CreatedBy", controlleraction.CreatedBy);
            param.Add("@IdentityVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

            _ = await connection.ExecuteAsync("[dbo].[Usp_IUD_areacontrolleraction]", param, commandType: CommandType.StoredProcedure);

            var identityVal = param.Get<int>("@IdentityVal");
            var statusCode = param.Get<int>("@StatusCode");
            var msgType = param.Get<string>("@MsgType");
            var msgText = param.Get<string>("@MsgText");

            return new SprocMessage { IdentityVal = identityVal, StatusCode = statusCode, MsgType = msgType, MsgText = msgText };
        }
        public async Task<SprocMessage> InsertControllerActionAgentAsync(Controlleraction controlleraction)
        {
            //todo use TVP
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("Id", 0);
            param.Add("@Areacontrolleraction", controlleraction.Areacontrolleraction);
            param.Add("@Area", controlleraction.Area);
            param.Add("@Controller", controlleraction.Controller);
            param.Add("@Action", controlleraction.Action);
            param.Add("@CreatedBy", controlleraction.CreatedBy);
            param.Add("@IdentityVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

            _ = await connection.ExecuteAsync("[dbo].[Usp_IUD_areacontrolleractionAgent]", param, commandType: CommandType.StoredProcedure);

            var identityVal = param.Get<int>("@IdentityVal");
            var statusCode = param.Get<int>("@StatusCode");
            var msgType = param.Get<string>("@MsgType");
            var msgText = param.Get<string>("@MsgText");

            return new SprocMessage { IdentityVal = identityVal, StatusCode = statusCode, MsgType = msgType, MsgText = msgText };
        }

    }
}
