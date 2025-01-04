using Dapper;
using DocumentFormat.OpenXml.Drawing.Spreadsheet;
using Microsoft.AspNetCore.Http;
using Mpmt.Core.Dtos.Partner;
using Mpmt.Core.Dtos.Roles;
using Mpmt.Data.Common;
using Mpmts.Core.Dtos;
using System.Data;
using System.Security.Claims;

namespace Mpmt.Data.Repositories.PartnerRoles
{
    public class PartnerRolesRepository : IPartnerRolesRepository
    {
        private readonly ClaimsPrincipal _loggedInUser;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PartnerRolesRepository(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _loggedInUser = _httpContextAccessor.HttpContext?.User;
        }

        public async Task<SprocMessage> AddRoleAsync(PartnerRole role)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            var param = new DynamicParameters();
            param.Add("@Event", role.Event);
            param.Add("@RoleName", role.RoleName);
            param.Add("@PartnerCode", role.PartnerCode);
            param.Add("@Description", role.Description);
            param.Add("@IsSystemRole", role.IsSystemRole);
            param.Add("@IsActive", role.IsActive);
            param.Add("@IsDeleted", role.IsDeleted);
            param.Add("@LoggedInUser", role.LoggedInUser);
            param.Add("@UserType", role.UserType);

            param.Add("@IdentityVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);
            try
            {
                _ = await connection.ExecuteAsync("[dbo].[Usp_IUD_partner_roles]", param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                throw;
            }

            var identityVal = param.Get<int>("@IdentityVal");
            var statusCode = param.Get<int>("@StatusCode");
            var msgType = param.Get<string>("@MsgType");
            var msgText = param.Get<string>("@MsgText");

            return new SprocMessage { IdentityVal = identityVal, StatusCode = statusCode, MsgType = msgType, MsgText = msgText };
        }

        public async Task<IEnumerable<PartnerRoleDetail>> GetRoleAsync(string partnercode)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            var param = new DynamicParameters();
            param.Add("@partnerCode", partnercode);
            return await connection.QueryAsync<PartnerRoleDetail>("[dbo].[usp_get_partner_roles]", param, commandType: CommandType.StoredProcedure);
        }

        public async Task<PartnerRoleDetail> GetRoleByIdAsync(int roleId)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            var param = new DynamicParameters();
            param.Add("@Id", roleId);
            return await connection.QueryFirstOrDefaultAsync<PartnerRoleDetail>("[dbo].[usp_get_partner_role_by_id]", param, commandType: CommandType.StoredProcedure);
        }

        public async Task<PartnerRole> GetRoleByNameAsync(string roleName)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@RoleName", roleName);

            return await connection.QueryFirstOrDefaultAsync<PartnerRole>("[dbo].[usp_get_partner_role_by_name]", param, commandType: CommandType.StoredProcedure);
        }

        public async Task<SprocMessage> RemoveRoleAsync(int roleid, string loggedInUser, string PartnerCode)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            var param = new DynamicParameters();
            param.Add("@Event", 'D');
            param.Add("@Id", roleid);
            param.Add("@PartnerCode", PartnerCode);
            param.Add("@RoleName", "");
            param.Add("@Description", "");
            param.Add("@IsSystemRole", null);
            param.Add("@IsActive", null);
            param.Add("@IsDeleted", null);
            param.Add("@LoggedInUser", loggedInUser);
            param.Add("@UserType", "Partner");

            param.Add("@IdentityVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);
            try
            {
                _ = await connection.ExecuteAsync("[dbo].[Usp_IUD_partner_roles]", param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception)
            {
                throw;
            }

            var identityVal = param.Get<int>("@IdentityVal");
            var statusCode = param.Get<int>("@StatusCode");
            var msgType = param.Get<string>("@MsgType");
            var msgText = param.Get<string>("@MsgText");

            return new SprocMessage { IdentityVal = identityVal, StatusCode = statusCode, MsgType = msgType, MsgText = msgText };
        }

        public async Task<SprocMessage> UpdateRoleAsync(PartnerRole role)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            var param = new DynamicParameters();
            param.Add("@Event", role.Event);
            param.Add("@Id", role.Id);
            param.Add("@RoleName", role.RoleName);
            param.Add("@PartnerCode", role.PartnerCode);
            param.Add("@Description", role.Description);
            param.Add("@IsSystemRole", role.IsSystemRole);
            param.Add("@IsActive", role.IsActive);
            param.Add("@IsDeleted", role.IsDeleted);
            param.Add("@LoggedInUser", role.LoggedInUser);
            param.Add("@UserType", role.UserType);

            param.Add("@IdentityVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);
            try
            {
                _ = await connection.ExecuteAsync("[dbo].[Usp_IUD_partner_roles]", param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception)
            {
                throw;
            }

            var identityVal = param.Get<int>("@IdentityVal");
            var statusCode = param.Get<int>("@StatusCode");
            var msgType = param.Get<string>("@MsgType");
            var msgText = param.Get<string>("@MsgText");

            return new SprocMessage { IdentityVal = identityVal, StatusCode = statusCode, MsgType = msgType, MsgText = msgText };
        }

        public async Task<IEnumerable<AppPartner>> GetPartnerRole()
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            var email = _loggedInUser.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email).Value;
            param.Add("@Email", email);

            return await connection
                .QueryAsync<AppPartner>("[dbo].[usp_get_partner_by_id]", param, commandType: CommandType.StoredProcedure);
        }

        public async Task<SprocMessage> IUDPartnerRoleAsync(IUDPartnerRole role)
        {
            try
            {
                using var connection = DbConnectionManager.GetDefaultConnection();
                var param = new DynamicParameters();
                param.Add("@Event", role.Event);
                param.Add("@Id", role.Id);
                param.Add("@RoleName", role.RoleName);
                param.Add("@Description", role.Description);
                param.Add("@IsSystemRole", role.IsSystemRole);
                param.Add("@IsActive", role.IsActive);
                param.Add("@IsDeleted", role.IsDeleted);
                param.Add("@LoggedInUser", role.LoggedInUser);
                param.Add("@UserType", role.UserType);

                param.Add("@IdentityVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
                param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

                _ = await connection.ExecuteAsync("[dbo].[Usp_IUD_AdminPartnerRole]", param, commandType: CommandType.StoredProcedure);

                var identityVal = param.Get<int>("@IdentityVal");
                var statusCode = param.Get<int>("@StatusCode");
                var msgType = param.Get<string>("@MsgType");
                var msgText = param.Get<string>("@MsgText");

                return new SprocMessage { IdentityVal = identityVal, StatusCode = statusCode, MsgType = msgType, MsgText = msgText };
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IEnumerable<PartnerRoleList>> GetPartnerRoleAsync()
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            return await connection.QueryAsync<PartnerRoleList>("[dbo].[usp_get_admin_partner_roles]", commandType: CommandType.StoredProcedure);
        }

        public async Task<PartnerRoleList> GetPartnerRoleByIdAsync(int id)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            var param = new DynamicParameters();
            param.Add("@Id", id);
            return await connection.QueryFirstOrDefaultAsync<PartnerRoleList>("[dbo].[usp_get_admin_partner_role_by_id]", param, commandType: CommandType.StoredProcedure);
        }

        public async Task<PartnerRoleList> GetPartnerRoleByNameAsync(string roleName)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@RoleName", roleName);

            return await connection.QueryFirstOrDefaultAsync<PartnerRoleList>("[dbo].[usp_get_admin_partner_role_by_name]", param, commandType: CommandType.StoredProcedure);
        }

        public async Task<SprocMessage> AddPartnerRoleAsync(PartnerAdminRole role)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@RoleName", role.RoleName);
            param.Add("@Description", role.Description);
            param.Add("@IsSystemRole", role.IsSystemRole);
            param.Add("@IsActive", role.IsActive);
            param.Add("@LoggedInUser", role.UserType);

            param.Add("@IdentityVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

            _ = await connection.ExecuteAsync("[dbo].[usp_add_admin_partner_role]", param, commandType: CommandType.StoredProcedure);

            var identityVal = param.Get<int>("@IdentityVal");
            var statusCode = param.Get<int>("@StatusCode");
            var msgType = param.Get<string>("@MsgType");
            var msgText = param.Get<string>("@MsgText");

            return new SprocMessage { IdentityVal = identityVal, StatusCode = statusCode, MsgType = msgType, MsgText = msgText };
        }
    }
}