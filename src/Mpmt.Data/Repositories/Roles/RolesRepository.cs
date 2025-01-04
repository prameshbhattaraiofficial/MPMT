using AutoMapper;
using Dapper;
using Microsoft.AspNetCore.Http;
using Mpmt.Core.Dtos.Roles;
using Mpmt.Data.Common;
using Mpmts.Core.Dtos;
using System.Data;
using System.Security.Claims;

namespace Mpmt.Data.Repositories.Roles
{
    /// <summary>
    /// The roles repository.
    /// </summary>
    public class RolesRepository : IRolesRepository
    {

        private readonly IMapper _mapper;
        private readonly ClaimsPrincipal _loggedInUser;
        private readonly IHttpContextAccessor _httpContextAccessor;
        /// <summary>
        /// Initializes a new instance of the <see cref="RolesRepository"/> class.
        /// </summary>
        /// <param name="mapper">The mapper.</param>
        /// <param name="httpContextAccessor">The http context accessor.</param>
        public RolesRepository(IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _loggedInUser = _httpContextAccessor.HttpContext?.User;
        }

        /// <summary>
        /// Adds the role async.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> AddRoleAsync(AppRole role)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@RoleName", role.RoleName);
            param.Add("@Description", role.Description);
            param.Add("@IsSystemRole", role.IsSystemRole);
            param.Add("@IsActive", role.IsActive);
            param.Add("@IsDeleted", role.IsDeleted);
            param.Add("@LoggedInUser", role.LoggedInUser);

            param.Add("@IdentityVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

            _ = await connection.ExecuteAsync("[dbo].[usp_add_role]", param, commandType: CommandType.StoredProcedure);

            var identityVal = param.Get<int>("@IdentityVal");
            var statusCode = param.Get<int>("@StatusCode");
            var msgType = param.Get<string>("@MsgType");
            var msgText = param.Get<string>("@MsgText");

            return new SprocMessage { IdentityVal = identityVal, StatusCode = statusCode, MsgType = msgType, MsgText = msgText };
        }


        /// <summary>
        /// Gets the role async.
        /// </summary>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<AppRole>> GetRoleAsync()
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            return await connection.QueryAsync<AppRole>("[dbo].[usp_get_roles]", commandType: CommandType.StoredProcedure);
        }


        /// <summary>
        /// Gets the role by id async.
        /// </summary>
        /// <param name="roleId">The role id.</param>
        /// <returns>A Task.</returns>
        public async Task<AppRole> GetRoleByIdAsync(int roleId)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@RoleId", roleId);

            return await connection.QueryFirstOrDefaultAsync<AppRole>("[dbo].[usp_get_role_by_id]", param, commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Gets the role by name async.
        /// </summary>
        /// <param name="roleName">The role name.</param>
        /// <returns>A Task.</returns>
        public async Task<AppRole> GetRoleByNameAsync(string roleName)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@RoleName", roleName);

            return await connection.QueryFirstOrDefaultAsync<AppRole>("[dbo].[usp_get_role_by_name]", param, commandType: CommandType.StoredProcedure);
        }


        /// <summary>
        /// Updates the role async.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <returns>A Task.</returns>

        public async Task<SprocMessage> RemoveRoleAsync(int roleid)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@Id", roleid);
            param.Add("@UpdatedById", 1);
            param.Add("@IdentityVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

            _ = await connection.ExecuteAsync("[dbo].[usp_role_deleteby_id]", param, commandType: CommandType.StoredProcedure);

            var identityVal = param.Get<int>("@IdentityVal");
            var statusCode = param.Get<int>("@StatusCode");
            var msgType = param.Get<string>("@MsgType");
            var msgText = param.Get<string>("@MsgText");

            return new SprocMessage { IdentityVal = identityVal, StatusCode = statusCode, MsgType = msgType, MsgText = msgText };
        }


        /// <summary>
        /// Updates the role async.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> UpdateRoleAsync(AppRole role)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@RoleId", role.Id);
            param.Add("@RoleName", role.RoleName);
            param.Add("@Description", role.Description);
            param.Add("@IsSystemRole", role.IsSystemRole);
            param.Add("@IsActive", role.IsActive);
            param.Add("@IsDeleted", role.IsDeleted);
            param.Add("@LoggedInUser", role.LoggedInUser);

            param.Add("@IdentityVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

            _ = await connection.ExecuteAsync("[dbo].[usp_update_role]", param, commandType: CommandType.StoredProcedure);

            var identityVal = param.Get<int>("@IdentityVal");
            var statusCode = param.Get<int>("@StatusCode");
            var msgType = param.Get<string>("@MsgType");
            var msgText = param.Get<string>("@MsgText");

            return new SprocMessage { IdentityVal = identityVal, StatusCode = statusCode, MsgType = msgType, MsgText = msgText };
        }
        /// <summary>
        /// Gets the admin role.
        /// </summary>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<ModuleActionClass>> GetAdminRole()
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            var email = _loggedInUser.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email).Value;
            param.Add("@Email", email);

            return await connection
                .QueryAsync<ModuleActionClass>("[dbo].[usp_get_admin_role_by_Email]", param, commandType: CommandType.StoredProcedure);
        }
        /// <summary>
        /// Gets the menu by role id.
        /// </summary>
        /// <param name="roleid">The roleid.</param>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<MenuByRole>> GetMenuByRoleId(int roleid)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            var param = new DynamicParameters();
            param.Add("@RoleId", roleid);
            return await connection
                .QueryAsync<MenuByRole>("[dbo].[usp_rolemenupermissions_get_byroleid]", param, commandType: CommandType.StoredProcedure);
        }
        /// <summary>
        /// Updates the menu to role.
        /// </summary>
        /// <param name="menuByRoles">The menu by roles.</param>
        /// <param name="roleId">The role id.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> UpdateMenuToRole(List<MenuByRole> menuByRoles, int roleId)
        {
            if (roleId < 1)
                return (new SprocMessage { MsgText = "Invalid RoleId", StatusCode = 400, IdentityVal = 0, MsgType = "Error" });

            var updatedDate = DateTime.Now;

            string updatedBy = "1";

            var listRoleMenuPermissionsType = menuByRoles.Select(x => new RoleMenuPermissionsType
            {
                MenuId = x.menuId,
                ViewPer = x.viewPer,
                CreatePer = x.createPer,
                UpdatePer = x.updatePer,
                DeletePer = x.deletePer,
                UpdatedDate = updatedDate,
                UpdatedBy = updatedBy
            });

            var dataTableRmp = GetDataTableRoleMenuPermissions();

            foreach (var rmpType in listRoleMenuPermissionsType)
            {
                var row = dataTableRmp.NewRow();
                row["MenuId"] = rmpType.MenuId;
                row["ViewPer"] = rmpType.ViewPer;
                row["CreatePer"] = rmpType.CreatePer;
                row["UpdatePer"] = rmpType.UpdatePer;
                row["DeletePer"] = rmpType.DeletePer;
                row["UpdatedDate"] = rmpType.UpdatedDate!;
                row["UpdatedBy"] = rmpType.UpdatedBy;
                dataTableRmp.Rows.Add(row);
            }

            try
            {
                using var connection = DbConnectionManager.GetDefaultConnection();


                var param = new DynamicParameters();
                param.Add("@RoleId", roleId);
                param.Add("@RoleMenuPermissions", dataTableRmp.AsTableValuedParameter("[dbo].[RoleMenuPermissionsType]"));
                param.Add("@sqlActionStatus", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);
                var result = await connection.ExecuteAsync("[dbo].[usp_rolemenupermissions_upsert_byroleid]", param: param, commandType: CommandType.StoredProcedure);
                var sqlActionStatus = param.Get<int>("@sqlActionStatus");
                if (sqlActionStatus < 0)
                    return (new SprocMessage { MsgText = "Server Error", StatusCode = 400, IdentityVal = 0, MsgType = "Error" });

                return (new SprocMessage { MsgText = "Updated SucessFully", StatusCode = 200, IdentityVal = 1, MsgType = "Sucess" });
            }


            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// Gets the data table role menu permissions.
        /// </summary>
        /// <returns>A DataTable.</returns>
        private DataTable GetDataTableRoleMenuPermissions()
        {
            var dataTableRmp = new DataTable();
            dataTableRmp.Columns.Add("MenuId", typeof(int));
            dataTableRmp.Columns.Add("ViewPer", typeof(bool));
            dataTableRmp.Columns.Add("CreatePer", typeof(bool));
            dataTableRmp.Columns.Add("UpdatePer", typeof(bool));
            dataTableRmp.Columns.Add("DeletePer", typeof(bool));
            dataTableRmp.Columns.Add("CreatedDate", typeof(DateTime)).AllowDBNull = true;
            dataTableRmp.Columns.Add("CreatedBy", typeof(string)).AllowDBNull = true;
            dataTableRmp.Columns.Add("UpdatedDate", typeof(DateTime)).AllowDBNull = true;
            dataTableRmp.Columns.Add("UpdatedBy", typeof(string)).AllowDBNull = true;
            dataTableRmp.Columns.Add("IsActive", typeof(bool));
            return dataTableRmp;
        }
    }
}