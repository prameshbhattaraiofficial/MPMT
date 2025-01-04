using Dapper;
using Microsoft.AspNetCore.Mvc;
using Mpmt.Core.Dtos.Partner;
using Mpmt.Core.Models.RMP;
using Mpmt.Core.Models.Route;
using Mpmt.Data.Common;
using Mpmts.Core.Dtos;
using System;
using System.Data;

namespace Mpmt.Data.Repositories.RoleMenuPermissionRepository;

public class RMPRepository : IRMPRepository
{
    public async Task<IEnumerable<RMPermissionModel>> GetListWithSubMenusAsync(string userName)
    {
        using var connection = DbConnectionManager.GetDefaultConnection();

        var param = new DynamicParameters();
        param.Add("@UserName", userName);

        var listRoleMenuPermissions = await connection
            .QueryAsync<RMPermissionModel>("[dbo].[sp_menus_rolemenupermissions_get_by_username]", param, commandType: CommandType.StoredProcedure);

        return listRoleMenuPermissions;
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
            .QueryAsync<GetcontrollerAction>("[dbo].[usp_get_menuPermission_byrole]", param, commandType: CommandType.StoredProcedure);

        return listRoleMenuPermissions;
    }

    public async Task<IEnumerable<GetcontrollerAction>> GetPartnerListcontrollerActionAsync(int roleId, string area = "", string controller = "", string action = "")
    {
        using var connection = DbConnectionManager.GetDefaultConnection();

        var param = new DynamicParameters();
        param.Add("@roleId", roleId);
        param.Add("@area", area);
        param.Add("@controller", controller);
        param.Add("@action", action);

        var listRoleMenuPermissions = await connection
            .QueryAsync<GetcontrollerAction>("[dbo].[usp_get_PartnermenuPermission_byrole]", param, commandType: CommandType.StoredProcedure);

        return listRoleMenuPermissions;
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
            .QueryFirstOrDefaultAsync<bool>("[dbo].[usp_get_menu_permission_status]", param, commandType: CommandType.StoredProcedure);

        return check;
    }

    public async Task<bool> CheckPartnerPermission(string area, string controller, string action, string UserName)
    {
        using var connection = DbConnectionManager.GetDefaultConnection();

        var param = new DynamicParameters();
        param.Add("@Area", area);
        param.Add("@Controller", controller);
        param.Add("@Action", action);
        param.Add("@UserName", UserName);

        var check = await connection
            .QueryFirstOrDefaultAsync<bool>("[dbo].[usp_get_partner_menu_permission_status]", param, commandType: CommandType.StoredProcedure);

        return check;
    }

    public async Task<IEnumerable<ActionPermission>> GetActionPermissionList(string area, string controller, string UserName)
    {
        using var connection = DbConnectionManager.GetDefaultConnection();

        var param = new DynamicParameters();
        param.Add("@Area", area);
        param.Add("@Controller", controller);
        param.Add("@UserName", UserName);




        var check = await connection
            .QueryAsync<ActionPermission>("[dbo].[usp_get_action_permission_by_controller_username]", param, commandType: CommandType.StoredProcedure);

        return check;
    }

    public async Task<IEnumerable<ActionPermission>> GetPartnerActionPermissionList(string area, string controller, string UserName)
    {
        using var connection = DbConnectionManager.GetDefaultConnection();

        var param = new DynamicParameters();
        param.Add("@Area", area);
        param.Add("@Controller", controller);
        param.Add("@UserName", UserName);

        var check = await connection
            .QueryAsync<ActionPermission>("[dbo].[usp_get_Partneraction_permission_by_controller_username]", param, commandType: CommandType.StoredProcedure);

        return check;
    }

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

        //var _ = await connection.ExecuteAsync("[dbo].[Usp_IUD_remit_partners]", param, commandType: CommandType.StoredProcedure);

        //var identityVal = param.Get<int>("@IdentityVal");

        try
        {
            var _ = await connection.ExecuteAsync("[dbo].[Usp_IUD_MenuPermission_test]", param, commandType: CommandType.StoredProcedure);
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

    public async Task<SprocMessage> AddPartnermenuPermissionAsync(AddcontrollerAction test)
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
            var _ = await connection.ExecuteAsync("[dbo].[Usp_IUD_PartnerMenuPermission_test]", param, commandType: CommandType.StoredProcedure);
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

    public async Task<IEnumerable<ActionPermission>> GetAgentActionPermissionList(string controller, string UserName)
    {
        using var connection = DbConnectionManager.GetDefaultConnection();

        var param = new DynamicParameters();
        param.Add("@Controller", controller);
        param.Add("@Username", UserName);

        var check = await connection
            .QueryAsync<ActionPermission>("[dbo].[usp_get_agentaction_permission_by_controller_username]", param, commandType: CommandType.StoredProcedure);

        return check;
    }

    public async Task<SprocMessage> PartnerMenuPermissionAsync(AddcontrollerAction test)
    {
        var dataTableRmp = DirectorTable();

        foreach (var menuid in test.MenusIds)
        {
            var row = dataTableRmp.NewRow();
            row["MenuId"] = menuid.Id;
            row["Permission"] = menuid.Permission;

            dataTableRmp.Rows.Add(row);
        }
        using var connection = DbConnectionManager.GetDefaultConnection();

        var param = new DynamicParameters();

        param.Add("@RoleId", test.RoleId);
        param.Add("@MenuPermission", dataTableRmp.AsTableValuedParameter("[dbo].[MenuPermissionType]"));

        param.Add("@IdentityVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
        param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
        param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
        param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

        try
        {
            var _ = await connection.ExecuteAsync("[dbo].[Usp_IUD_PartnerMenuPermission]", param, commandType: CommandType.StoredProcedure);
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

    public async Task<IEnumerable<GetcontrollerAction>> GetPartnerMenuListControllerAction(int roleId, string area = "", string controller = "", string action = "")
    {
        using var connection = DbConnectionManager.GetDefaultConnection();

        var param = new DynamicParameters();
        param.Add("@RoleId", roleId);
        param.Add("@Area", area);
        param.Add("@Controller", controller);
        param.Add("@Action", action);

        var listRoleMenuPermissions = await connection
            .QueryAsync<GetcontrollerAction>("[dbo].[usp_get_PartnerMenuList_byRoleId]", param, commandType: CommandType.StoredProcedure);

        return listRoleMenuPermissions;
    }

    public async Task<bool> CheckPartnerMenuPermission(string area, string controller, string action, string UserName)
    {
        using var connection = DbConnectionManager.GetDefaultConnection();

        var param = new DynamicParameters();
        param.Add("@Area", area);
        param.Add("@Controller", controller);
        param.Add("@Action", action);
        param.Add("@Username", UserName);

        var check = await connection
            .QueryFirstOrDefaultAsync<bool>("[dbo].[usp_get_partner_menu_permission]", param, commandType: CommandType.StoredProcedure);

        return check;
    }

   

    
}