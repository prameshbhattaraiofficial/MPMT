using Dapper;
using DocumentFormat.OpenXml.Drawing.Spreadsheet;
using Mpmt.Core.Dtos.Roles;
using Mpmt.Core.Models.Route;
using Mpmt.Data.Common;
using Mpmts.Core.Dtos;
using System.Data;

namespace Mpmt.Data.Repositories.Roles;

public class AgentRolesRepository : IAgentRolesRepository
{
    public async Task<SprocMessage> AddmenuPermission(AddcontrollerAction test)
    {
        var dataTableRmp = DirectorTable();
        foreach (var menuid in test.MenusIds)
        {
            var row = dataTableRmp.NewRow();
            row["menuId"] = menuid.Id;
            row["Permission"] = menuid.Permission;
            dataTableRmp.Rows.Add(row);
        }
        using var connection = DbConnectionManager.GetDefaultConnection();
        var param = new DynamicParameters();
        param.Add("@roleId", test.RoleId);
        param.Add("@menupermission", dataTableRmp.AsTableValuedParameter("[dbo].[MenuPermissionType]"));
        param.Add("@IdentityVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
        param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
        param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
        param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);
        try
        {
            var _ = await connection.ExecuteAsync("[dbo].[Usp_IUD_MenuPermission_Agent]", param, commandType: CommandType.StoredProcedure);
        }
        catch (Exception)
        {
            return new SprocMessage { IdentityVal = 0, StatusCode = 400, MsgType = "Error", MsgText = "Issue Inserting Data" };
        }
        var identityVal = param.Get<int>("@IdentityVal");
        var statusCode = param.Get<int>("@StatusCode");
        var msgType = param.Get<string>("@MsgType");
        var msgText = param.Get<string>("@MsgText");
        return new SprocMessage { IdentityVal = identityVal, StatusCode = statusCode, MsgType = msgType, MsgText = msgText };
    }

    private DataTable DirectorTable()
    {
        var dataTableRmp = new DataTable();
        dataTableRmp.Columns.Add("menuId", typeof(int));
        dataTableRmp.Columns.Add("Permission", typeof(bool));
        return dataTableRmp;
    }

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
        param.Add("@AgentCode", role.AgentCode);

        param.Add("@IdentityVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
        param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
        param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
        param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

        _ = await connection.ExecuteAsync("[dbo].[usp_add_agentemployee_role]", param, commandType: CommandType.StoredProcedure);

        var identityVal = param.Get<int>("@IdentityVal");
        var statusCode = param.Get<int>("@StatusCode");
        var msgType = param.Get<string>("@MsgType");
        var msgText = param.Get<string>("@MsgText");

        return new SprocMessage { IdentityVal = identityVal, StatusCode = statusCode, MsgType = msgType, MsgText = msgText };
    }

    public async Task<SprocMessage> AssignRoletoUser(int user_id, int[] roleids)
    {
        try
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            var param = new DynamicParameters();

            param.Add("@EmployeeId", user_id);
            param.Add("@RoleIds", string.Join(',', roleids));
            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

            _ = await connection.ExecuteAsync("[dbo].[usp_add_roles_to_agentemployee]", param, commandType: CommandType.StoredProcedure);

            var statusCode = param.Get<int>("@StatusCode");
            var msgType = param.Get<string>("@MsgType");
            var msgText = param.Get<string>("@MsgText");
            return new SprocMessage { IdentityVal = 0, StatusCode = statusCode, MsgType = msgType, MsgText = msgText };
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task<IEnumerable<GetcontrollerAction>> GetListcontrollerActionAsync(int roleId, string area = "", string controller = "", string action = "")
    {
        using var connection = DbConnectionManager.GetDefaultConnection();
        var param = new DynamicParameters();
        param.Add("@roleId", roleId);
        param.Add("@area", area);
        param.Add("@controller", controller);
        param.Add("@action", action);
        var listRoleMenuPermissions = await connection
            .QueryAsync<GetcontrollerAction>("[dbo].[usp_get_menuPermission_byAgentEmployeeRole]", param: param, commandType: CommandType.StoredProcedure);
        return listRoleMenuPermissions;
    }

    public async Task<IEnumerable<AppRole>> GetRoleAsync(string AgentCode)
    {
        using var connection = DbConnectionManager.GetDefaultConnection();
        var param = new DynamicParameters();
        param.Add("@AgentCode", AgentCode);
        var listRole = await connection
           .QueryAsync<AppRole>("[dbo].[usp_get_agentemployee_roles]", param: param, commandType: CommandType.StoredProcedure);
        return listRole;
        //return await connection.QueryAsync<AppRole>("[dbo].[usp_get_agentemployee_roles]", commandType: CommandType.StoredProcedure);
    }

    public async Task<AppRole> GetRoleByIdAsync(int roleId, string AgentCode)
    {
        using var connection = DbConnectionManager.GetDefaultConnection();
        var param = new DynamicParameters();
        param.Add("@RoleId", roleId);
        param.Add("@AgentCode", AgentCode);
        return await connection.QueryFirstOrDefaultAsync<AppRole>("[dbo].[usp_get_agentemployee_role_by_id]", param, commandType: CommandType.StoredProcedure);
    }

    public async Task<SprocMessage> RemoveRoleAsync(int roleid, string AgentCode)
    {
        using var connection = DbConnectionManager.GetDefaultConnection();

        var param = new DynamicParameters();
        param.Add("@Id", roleid);
        param.Add("@AgentCode", AgentCode);
        param.Add("@UpdatedById", 1);
        param.Add("@IdentityVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
        param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
        param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
        param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

        _ = await connection.ExecuteAsync("[dbo].[usp_agentemployee_role_deleteby_id]", param, commandType: CommandType.StoredProcedure);

        var identityVal = param.Get<int>("@IdentityVal");
        var statusCode = param.Get<int>("@StatusCode");
        var msgType = param.Get<string>("@MsgType");
        var msgText = param.Get<string>("@MsgText");

        return new SprocMessage { IdentityVal = identityVal, StatusCode = statusCode, MsgType = msgType, MsgText = msgText };
    }

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
        param.Add("@AgentCode", role.AgentCode);

        param.Add("@IdentityVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
        param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
        param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
        param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

        _ = await connection.ExecuteAsync("[dbo].[usp_update_agentemployee_role]", param, commandType: CommandType.StoredProcedure);

        var identityVal = param.Get<int>("@IdentityVal");
        var statusCode = param.Get<int>("@StatusCode");
        var msgType = param.Get<string>("@MsgType");
        var msgText = param.Get<string>("@MsgText");

        return new SprocMessage { IdentityVal = identityVal, StatusCode = statusCode, MsgType = msgType, MsgText = msgText };
    }

    public async Task<bool> CheckPermission(string area, string controller, string action, string UserName)
    {
        using var connection = DbConnectionManager.GetDefaultConnection();

        var param = new DynamicParameters();
        param.Add("@Area", area);
        param.Add("@Controller", controller);
        param.Add("@Action", action);
        param.Add("@UserName", UserName);

        var check = await connection
            .QueryFirstOrDefaultAsync<bool>("[dbo].[usp_get_menu_permission_status_agentEmployee]", param, commandType: CommandType.StoredProcedure);

        return check;
    }
}
