using Dapper;
using Mpmt.Core.Dtos.Roles;
using Mpmt.Data.Common;
using Mpmts.Core.Dtos;
using System.Data;

namespace Mpmt.Data.Repositories.Roles
{
    /// <summary>
    /// The user roles repository.
    /// </summary>
    public class UserRolesRepository : IUserRolesRepository
    {
        /// <summary>
        /// Adds the user to roles async.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="roleIds">The role ids.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> AddUserToRolesAsync(int userId, params int[] roleIds)
        {
            var roleIdsConcatenated = string.Join(',', roleIds);

            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@UserId", userId);
            param.Add("@RoleIds", roleIdsConcatenated);

            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

            _ = await connection.ExecuteAsync("[dbo].[usp_add_roles_to_user]", param, commandType: CommandType.StoredProcedure);

            var statusCode = param.Get<int>("@StatusCode");
            var msgType = param.Get<string>("@MsgType");
            var msgText = param.Get<string>("@MsgText");

            return new SprocMessage { StatusCode = statusCode, MsgType = msgType, MsgText = msgText };
        }

        public async Task<IEnumerable<UserRoles>> GetRolesByUserIdAsync(int userId)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@UserId", userId);

            var roles = await connection.QueryAsync<UserRoles>("[dbo].[usp_get_user_roles_byUserId]", param, commandType: CommandType.StoredProcedure);
            return roles;
        }
    }
}
