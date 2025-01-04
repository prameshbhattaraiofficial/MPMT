using AutoMapper;
using Dapper;
using Mpmt.Core.Domain.Agents;
using Mpmt.Core.Dtos.CashAgent;
using Mpmt.Core.Dtos.Paging;
using Mpmt.Core.Dtos.Partner;
using Mpmt.Core.Dtos.SuperAgent;
using Mpmt.Core.Models.Route;
using Mpmt.Core.ViewModel.CashAgent;
using Mpmt.Data.Common;
using Mpmts.Core.Dtos;
using System.Data;

namespace Mpmt.Data.Repositories.CashAgent;

public class CashAgentRepository : ICashAgentRepository
{
    private readonly IMapper _mapper;

    public CashAgentRepository(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<AgentUser> GetAgentUserByPhonenumber(string PhoneNUmber)
    {
        try
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@contact", PhoneNUmber);
            var data = await connection
                .QueryFirstOrDefaultAsync<AgentUser>("[dbo].[usp_get_agent_by_contact]", param: param, commandType: CommandType.StoredProcedure);
            return data;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task<AgentUser> GetAgentUserByUserName(string UserName)
    {
        try
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            var param = new DynamicParameters();
            param.Add("@UserName", UserName);
            var response = new AgentUser();
            //var data = await connection.QueryFirstOrDefaultAsync<AgentUser>("[dbo].[usp_get_agent_by_username]", param: param, commandType: CommandType.StoredProcedure);
            var data = await connection
                   .QueryMultipleAsync("[dbo].[usp_get_agent_by_username]", param, commandType: CommandType.StoredProcedure);
            response = await data.ReadFirstOrDefaultAsync<AgentUser>();
            var images = await data.ReadAsync<string>();
            response.LicensedocImgPath = images.ToList();
            return response;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task UpdateIs2FAAuthenticatedAsync(string Agentcode, bool Is2FAAuthenticated)
    {
        using var connection = DbConnectionManager.GetDefaultConnection();

        var param = new DynamicParameters();
        param.Add("@Agentcode", Agentcode);
        param.Add("@Is2FAAuthenticated", Is2FAAuthenticated);
        param.Add("@IdentityVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
        param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);
        param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
        param.Add("@MsgType", dbType: DbType.String, size: 50, direction: ParameterDirection.Output);

        _ = await connection.ExecuteAsync("[dbo].[usp_update_Is2FAAuthenticated_By_AgentCode]", param, commandType: CommandType.StoredProcedure);
    }

    public async Task<PagedList<AgentDetail>> GetAgentUserAsync(AgentFilter userFilter)
    {
        try
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            var param = new DynamicParameters();
            param.Add("@UserName", userFilter.UserName);
            param.Add("FullName", userFilter.FullName);
            param.Add("@Email", userFilter.Email);
            param.Add("@ContactNumber", userFilter.ContactNumber);
            param.Add("@AgentCode", userFilter.AgentCode);
            param.Add("@DistrictCode", userFilter.DistrictCode);
            param.Add("@UserType", userFilter.UserType);
            param.Add("@IsActive", userFilter.UserStatus);
            param.Add("@PageNumber", userFilter.PageNumber);
            param.Add("@PageSize", userFilter.PageSize);
            param.Add("@SortingCol", userFilter.SortBy);
            param.Add("@SortType", userFilter.SortOrder);
            param.Add("@SearchVal", userFilter.SearchVal);
            param.Add("@Export", userFilter.Export);

            var data = await connection
                .QueryMultipleAsync("[dbo].[sp_get_remit_CashAgent]", param: param, commandType: CommandType.StoredProcedure);

            var UserList = await data.ReadAsync<AgentDetail>();
            var pagedInfo = await data.ReadFirstAsync<PageInfo>();
            var mappeddata = _mapper.Map<PagedList<AgentDetail>>(pagedInfo);
            mappeddata.Items = UserList;
            return mappeddata;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    private DataTable ImageTable()
    {
        var dataTableRmp = new DataTable();
        dataTableRmp.Columns.Add("DocumentImgPath", typeof(string));
        return dataTableRmp;
    }

    public async Task<SprocMessage> IUDAgentUserAsync(CashAgentUser agentUser)
    {
        try
        {
            var ImagedataTable = ImageTable();
            if (agentUser.LicensedocImgPath != null & agentUser.LicensedocImgPath.Count > 0)
            {
                foreach (var path in agentUser.LicensedocImgPath)
                {
                    var row = ImagedataTable.NewRow();
                    row["DocumentImgPath"] = path;
                    ImagedataTable.Rows.Add(row);
                }
            }
            using var connection = DbConnectionManager.GetDefaultConnection();
            var param = new DynamicParameters();
            param.Add("@Event", agentUser.Event);
            param.Add("@AgentCode", agentUser.AgentCode);
            param.Add("@SuperAgentCode", agentUser.SuperAgentCode);
            param.Add("@DocumentImgPaths", ImagedataTable.AsTableValuedParameter("[dbo].[DocumentImages]"));
            param.Add("@FirstName", agentUser.FirstName);
            param.Add("@LastName", agentUser.LastName);
            param.Add("@Email", agentUser.Email);
            param.Add("@EmailConfirmed", agentUser.EmailConfirmed);
            param.Add("@ContactNumber", agentUser.ContactNumber);
            param.Add("@ContactNumberConfirmed", agentUser.ContactNumberConfirmed);
            param.Add("@LookupName", agentUser.LookupName);
            param.Add("@Remarks", agentUser.Remarks);
            param.Add("@UserName", agentUser.UserName);
            param.Add("@PasswordHash", agentUser.PasswordHash);
            param.Add("@PasswordSalt", agentUser.PasswordSalt);
            param.Add("@AccessCodeHash", agentUser.AccessCodeHash);
            param.Add("@AccessCodeSalt", agentUser.AccessCodeSalt);
            param.Add("@MPINHash", agentUser.MPINHash);
            param.Add("@MPINSalt", agentUser.MPINSalt);
            param.Add("@OrganizationName", agentUser.OrganizationName);
            param.Add("@OrgEmail", agentUser.OrgEmail);
            param.Add("@OrgEmailConfirmed", agentUser.OrgEmailConfirmed);
            param.Add("@CountryCode", agentUser.CountryCode);
            param.Add("@ProvinceCode", agentUser.ProvinceCode);
            param.Add("@DistrictCode", agentUser.DistrictCode);
            param.Add("@LocalLevelCode", agentUser.LocalLevelCode);
            param.Add("@City", agentUser.City);
            param.Add("@FullAddress", agentUser.FullAddress);
            param.Add("@GMTTimeZone", agentUser.GMTTimeZone);
            param.Add("@RegistrationNumber", agentUser.RegistrationNumber);
            param.Add("@CompanyLogoImgPath", agentUser.CompanyLogoImgPath);
            param.Add("@DocumentTypeId", agentUser.DocumentTypeId);
            param.Add("@DocumentNumber", agentUser.DocumentNumber);
            param.Add("@IdFrontImgPath", agentUser.IdFrontImgPath);
            param.Add("@IdBackImgPath", agentUser.IdBackImgPath);
            param.Add("@ExpiryDate", agentUser.ExpiryDate);
            param.Add("@AddressProofTypeId", agentUser.AddressProofTypeId);
            param.Add("@AddressProofImgPath", agentUser.AddressProofImgPath);
            param.Add("@IpAddress", agentUser.IpAddress);
            param.Add("@DeviceId", agentUser.DeviceId);
            param.Add("@IsActive", agentUser.IsActive);
            param.Add("@IsPrefunding", agentUser.IsPrefunding);
            param.Add("@IsDeleted", agentUser.IsDeleted);
            param.Add("@IsBlocked", agentUser.IsBlocked);
            param.Add("@FailedLoginAttempt", agentUser.FailedLoginAttempt);
            param.Add("@TemporaryLockedTillUtcDate", agentUser.TemporaryLockedTillUtcDate);
            param.Add("@LastLoginDateUtc", agentUser.LastLoginDateUtc);
            param.Add("@LastActivityDateUtc", agentUser.LastActivityDateUtc);
            param.Add("@KycStatusCode", agentUser.KycStatusCode);
            param.Add("@Is2FAAuthenticated", agentUser.Is2FAAuthenticated);
            param.Add("@AccountSecretKey", agentUser.AccountSecretKey);
            param.Add("@LicenseDocImgPath", agentUser.LicenseDocImgPath);
            param.Add("@UserType", agentUser.UserType);
            param.Add("@LoginUserId", agentUser.LoginUserId);
            param.Add("@LoginUserName", agentUser.LoginUserName);
            param.Add("@Is2FARequired", agentUser.Is2FARequired);

            param.Add("@IdentityVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

            _ = await connection.ExecuteAsync("[dbo].[Usp_IUD_CashAgent]", param, commandType: CommandType.StoredProcedure);

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

    public async Task UpdateAgentLoginActivityAsync(AgentLoginActivity agentLoginActivity)
    {
        using var connection = DbConnectionManager.GetDefaultConnection();
        var param = new DynamicParameters();
        param.Add("@AgentCode", agentLoginActivity.AgentCode);
        param.Add("@FailedLoginAttempt", agentLoginActivity.FailedLoginAttempt);
        param.Add("@TemporaryLockedTillUtcDate", agentLoginActivity.TemporaryLockedTillUtcDate);
        param.Add("@IpAddress", agentLoginActivity.IpAddress);
        param.Add("@DeviceId", agentLoginActivity.DeviceId);
        param.Add("@IsActive", agentLoginActivity.IsActive);
        param.Add("@IsBlocked", agentLoginActivity.IsBlocked);
        param.Add("@LastLoginDateUtc", agentLoginActivity.LastLoginDateUtc);
        param.Add("@LastActivityDateUtc", agentLoginActivity.LastActivityDateUtc);

        _ = await connection.ExecuteAsync("[dbo].[usp_update_agent_login_activity]", param, commandType: CommandType.StoredProcedure);
    }

    public async Task<SprocMessage> AddAgentMenuAsync(IUDAgentMenu menu)
    {
        try
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            var param = new DynamicParameters();
            param.Add("@Event", menu.Event);
            param.Add("@Id", menu.Id);
            param.Add("@Title", menu.Title);
            param.Add("@ParentId", menu.ParentId);
            param.Add("@Controller", menu.Controller);
            param.Add("@Action", menu.Action);
            param.Add("@MenuUrl", "/");
            param.Add("@IsActive", menu.IsActive);
            param.Add("@DisplayOrder", menu.DisplayOrder);
            param.Add("@ImagePath", menu.ImagePath);
            param.Add("@LoggedInUser", 1);

            param.Add("@IdentityVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

            _ = await connection.ExecuteAsync("[dbo].[usp_IUD_agentmenu]", param: param, commandType: CommandType.StoredProcedure);

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

    public async Task<IEnumerable<AgentMenuModel>> GetAgentMenuAsync()
    {
        using var connection = DbConnectionManager.GetDefaultConnection();
        return await connection.QueryAsync<AgentMenuModel>("[dbo].[usp_get_agentmenu]", commandType: CommandType.StoredProcedure);
    }

    public async Task UpdateAccountSecretKeyAsync(string AgentCode, string accountsecretkey)
    {
        using var connection = DbConnectionManager.GetDefaultConnection();

        var param = new DynamicParameters();
        param.Add("@Agentcode", AgentCode);
        param.Add("@AccountSecretKey", accountsecretkey);
        param.Add("@IdentityVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
        param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);
        param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
        param.Add("@MsgType", dbType: DbType.String, size: 50, direction: ParameterDirection.Output);

        _ = await connection.ExecuteAsync("[dbo].[usp_Update_AccountSecretKey_By_AgentCode]", param, commandType: CommandType.StoredProcedure);
    }

    public async Task<SprocMessage> UpdateMenuDisplayOrderAsync(IUDAgentMenu menuUpdate)
    {
        using var connection = DbConnectionManager.GetDefaultConnection();
        var param = new DynamicParameters();
        param.Add("Id", menuUpdate.Id);
        param.Add("@DisplayOrder", menuUpdate.DisplayOrder);
        param.Add("@LoggedInUser", 1);
        param.Add("@IdentityVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
        param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
        param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
        param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

        _ = await connection.ExecuteAsync("[dbo].[usp_agentmenu_displayOrder]", param, commandType: CommandType.StoredProcedure);

        var identityVal = param.Get<int>("@IdentityVal");
        var statusCode = param.Get<int>("@StatusCode");
        var msgType = param.Get<string>("@MsgType");
        var msgText = param.Get<string>("@MsgText");
        return new SprocMessage { IdentityVal = identityVal, StatusCode = statusCode, MsgType = msgType, MsgText = msgText };
    }

    public async Task<SprocMessage> UpdateMenuIsActiveAsync(IUDAgentMenu menuUpdate)
    {
        using var connection = DbConnectionManager.GetDefaultConnection();

        var param = new DynamicParameters();
        param.Add("Id", menuUpdate.Id);
        param.Add("@IsActive", menuUpdate.IsActive);
        param.Add("@LoggedInUser", 1);
        param.Add("@IdentityVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
        param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
        param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
        param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

        _ = await connection.ExecuteAsync("[dbo].[usp_agentmenu_updateIsActive]", param, commandType: CommandType.StoredProcedure);

        var identityVal = param.Get<int>("@IdentityVal");
        var statusCode = param.Get<int>("@StatusCode");
        var msgType = param.Get<string>("@MsgType");
        var msgText = param.Get<string>("@MsgText");
        return new SprocMessage { IdentityVal = identityVal, StatusCode = statusCode, MsgType = msgType, MsgText = msgText };
    }

    public async Task<IUDAgentMenu> GetAgentMenuByIdAsync(int MenuId)
    {
        using var connection = DbConnectionManager.GetDefaultConnection();
        var param = new DynamicParameters();
        param.Add("@Id", MenuId);
        return await connection.QueryFirstOrDefaultAsync<IUDAgentMenu>("[dbo].[usp_get_agentmenu_byId]", param, commandType: CommandType.StoredProcedure);
    }

    public async Task<PagedList<AgentUserModel>> GetAgentAsync(AgentFilterModel agentFilter)
    {
        try
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            var param = new DynamicParameters();
            param.Add("@UserName", agentFilter.UserName);
            param.Add("@Email", agentFilter.Email);
            param.Add("@AgentCode", agentFilter.AgentCode);
            param.Add("@DistrictCode", agentFilter.DistrictCode);
            param.Add("@UserStatus", agentFilter.UserStatus);

            param.Add("@PageNumber", agentFilter.PageNumber);
            param.Add("@PageSize", agentFilter.PageSize);
            param.Add("@SortingCol", agentFilter.SortBy);
            param.Add("@SortType", agentFilter.SortOrder);
            param.Add("@SearchVal", agentFilter.SearchVal);
            param.Add("@Export", agentFilter.Export);

            var data = await connection
                .QueryMultipleAsync("[dbo].[usp_get_cashagent_list]", param: param, commandType: CommandType.StoredProcedure);

            var agentList = await data.ReadAsync<AgentUserModel>();
            var pagedInfo = await data.ReadFirstAsync<PageInfo>();
            var mappedData = _mapper.Map<PagedList<AgentUserModel>>(pagedInfo);
            mappedData.Items = agentList;
            return mappedData;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task<AgentUser> GetCashAgentByAgentCodeAsync(string agentCode)
    {
        try
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            var param = new DynamicParameters();
            var response = new AgentUser();
            param.Add("@AgentCode", agentCode);
            param.Add("@Event", "A");
            var data = await connection
            .QueryMultipleAsync("[dbo].[Usp_IUD_CashAgent]", param, commandType: CommandType.StoredProcedure);
            response = await data.ReadFirstAsync<AgentUser>();
            var images = await data.ReadAsync<string>();
            response.LicensedocImgPath = images.ToList();
            return response;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task<SprocMessage> AgentChangePasswordAsync(CashAgentUser user)
    {
        using var connection = DbConnectionManager.GetDefaultConnection();

        var param = new DynamicParameters();
        param.Add("@Event", user.Event);
        param.Add("@AgentCode", user.AgentCode);
        param.Add("@UserName", user.UserName);
        param.Add("@UserType", user.UserType);
        param.Add("@PasswordHash", user.PasswordHash);
        param.Add("@PasswordSalt", user.PasswordSalt);
        param.Add("@AccessCodeHash", user.AccessCodeHash);
        param.Add("@AccessCodeSalt", user.AccessCodeSalt);
        param.Add("@MPINHash", user.MPINHash);
        param.Add("@MPINSalt", user.MPINSalt);
        param.Add("@LoginUserId", user.LoginUserId);
        param.Add("@LoginUserName", user.LoginUserName);

        param.Add("@IdentityVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
        param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
        param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
        param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);
        try
        {
            var _ = await connection.ExecuteAsync("[dbo].[Usp_IUD_CashAgent]", param, commandType: CommandType.StoredProcedure);
        }
        catch (Exception ex)
        {
            return new SprocMessage { IdentityVal = 0, StatusCode = 400, MsgType = "Error", MsgText = "Issue Inserting Data" };
        }
        var identityVal = param.Get<int>("@IdentityVal");
        var statusCode = param.Get<int>("@StatusCode");
        var msgType = param.Get<string>("@MsgType");
        var msgText = param.Get<string>("@MsgText");
        return new SprocMessage { IdentityVal = identityVal, StatusCode = statusCode, MsgType = msgType, MsgText = msgText };
    }

    public async Task<SprocMessage> AgentEmployeeChangePasswordAsync(CashAgentUser user)
    {
        using var connection = DbConnectionManager.GetDefaultConnection();

        var param = new DynamicParameters();
        param.Add("@Event", user.Event);
        param.Add("@EmployeeId", user.EmployeeId);
        param.Add("@AgentCode", user.AgentCode);
        param.Add("@UserName", user.UserName);
        param.Add("@UserType", user.UserType);
        param.Add("@PasswordHash", user.PasswordHash);
        param.Add("@PasswordSalt", user.PasswordSalt);
        param.Add("@AccessCodeHash", user.AccessCodeHash);
        param.Add("@AccessCodeSalt", user.AccessCodeSalt);
        param.Add("@MPINHash", user.MPINHash);
        param.Add("@MPINSalt", user.MPINSalt);
        param.Add("@UpdatedById", user.LoginUserId);
        param.Add("@UpdatedByName", user.LoginUserName);

        param.Add("@IdentityVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
        param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
        param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
        param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);
        try
        {
            var _ = await connection.ExecuteAsync("[dbo].[Usp_IUD_remit_Agent_employee]", param, commandType: CommandType.StoredProcedure);
        }
        catch (Exception ex)
        {
            return new SprocMessage { IdentityVal = 0, StatusCode = 400, MsgType = "Error", MsgText = "Issue Inserting Data" };
        }
        var identityVal = param.Get<int>("@IdentityVal");
        var statusCode = param.Get<int>("@StatusCode");
        var msgType = param.Get<string>("@MsgType");
        var msgText = param.Get<string>("@MsgText");
        return new SprocMessage { IdentityVal = identityVal, StatusCode = statusCode, MsgType = msgType, MsgText = msgText };
    }

    public async Task<IEnumerable<AgentMenuChild>> GetMenusSubmenusForCurrentUserByUser(string Username)
    {
        using var connection = DbConnectionManager.GetDefaultConnection();

        var param = new DynamicParameters();
        param.Add("@UserName", Username);

        var listRoleMenuPermissions = await connection
            .QueryAsync<AgentMenuChild>("[dbo].[usp_get_Agent_menu_byUsername]", param, commandType: CommandType.StoredProcedure);

        return listRoleMenuPermissions;
    }

    public async Task<IEnumerable<GetcontrollerAction>> GetListcontrollerActionAsync(string UserType)
    {
        using var connection = DbConnectionManager.GetDefaultConnection();
        var param = new DynamicParameters();
        param.Add("@UserType", UserType);
        return await connection.QueryAsync<GetcontrollerAction>("[dbo].[usp_get_menuPermission_byAgentUserType]", param, commandType: CommandType.StoredProcedure);
    }

    public async Task<SprocMessage> AddmenuPermission(AddControllerActionUserType test)
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
        param.Add("@UserType", test.UserType);
        param.Add("@createdBy", test.CreatedBy);
        param.Add("@menupermission", dataTableRmp.AsTableValuedParameter("[dbo].[MenuPermissionType]"));
        param.Add("@IdentityVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
        param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
        param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
        param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);
        try
        {
            var _ = await connection.ExecuteAsync("[dbo].[Usp_IUD_MenuPermission_AgentUserType]", param, commandType: CommandType.StoredProcedure);
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

    public async Task<bool> VerifyUserNameAsync(string userName)
    {
        using var connection = DbConnectionManager.GetDefaultConnection();
        var param = new DynamicParameters();
        param.Add("@UserName", userName);
        param.Add("@Check", dbType: DbType.Boolean, direction: ParameterDirection.Output);

        var _ = await connection.ExecuteAsync("[dbo].[usp_check_agent_username_validation]", param, commandType: CommandType.StoredProcedure);

        var CheckResult = param.Get<bool>("@Check");
        return CheckResult;
    }

    public async Task<bool> VerifyContactNumber(string contactNumber)
    {
        using var connection = DbConnectionManager.GetDefaultConnection();
        var param = new DynamicParameters();
        param.Add("@ContactNumber", contactNumber);
        param.Add("@Check", dbType: DbType.Boolean, direction: ParameterDirection.Output);

        var _ = await connection.ExecuteAsync("[dbo].[usp_checkagent_contactnumber_validation]", param, commandType: CommandType.StoredProcedure);

        var CheckResult = param.Get<bool>("@Check");
        return CheckResult;
    }

    public async Task<bool> VerifyRegistrationNumber(string registrationNumber)
    {
        using var connection = DbConnectionManager.GetDefaultConnection();
        var param = new DynamicParameters();
        param.Add("@RegistrationNumber", registrationNumber);
        param.Add("@Check", dbType: DbType.Boolean, direction: ParameterDirection.Output);

        var _ = await connection.ExecuteAsync("[dbo].[usp_checkagent_registrationnumber_validation]", param, commandType: CommandType.StoredProcedure);

        var CheckResult = param.Get<bool>("@Check");
        return CheckResult;
    }

    public async Task<PagedList<AgentLedger>> GetAgentLedgerAsync(AgentLedgerFilter AgentLedgerFilter)
    {
        try
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            var param = new DynamicParameters();
            param.Add("@AgentCode", AgentLedgerFilter.AgentCode);
            param.Add("@StartDate", AgentLedgerFilter.StartDate);
            param.Add("@EndDate", AgentLedgerFilter.EndDate);
            param.Add("@PageNumber", AgentLedgerFilter.PageNumber);
            param.Add("@PageSize", AgentLedgerFilter.PageSize);
            param.Add("@SortingCol", AgentLedgerFilter.SortBy);
            param.Add("@SortType", AgentLedgerFilter.SortOrder);
            param.Add("@SearchVal", AgentLedgerFilter.SearchVal);
            param.Add("@Export", AgentLedgerFilter.Export);
            //return await connection.QueryAsync<AgentLedger>("[dbo].[usp_get_agent_ledger]", param, commandType: CommandType.StoredProcedure);
            var data = await connection
                .QueryMultipleAsync("[dbo].[usp_get_agent_ledger]", param: param, commandType: CommandType.StoredProcedure);

            var ledgerList = await data.ReadAsync<AgentLedger>();
            var pagedInfo = await data.ReadFirstAsync<PageInfo>();
            var mappeddata = _mapper.Map<PagedList<AgentLedger>>(pagedInfo);
            mappeddata.Items = ledgerList;
            return mappeddata;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task<PagedList<AgentDetail>> GetAgentBySuperAgentAsync(AgentFilter agentFilter)
    {
        try
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            var param = new DynamicParameters();
            param.Add("@AgentCode", agentFilter.AgentCode);
            param.Add("@UserName", agentFilter.UserName);
            param.Add("@FullName", agentFilter.FullName);
            param.Add("@Email", agentFilter.Email);
            param.Add("@ContactNumber", agentFilter.ContactNumber);
            param.Add("@SuperAgentCode", agentFilter.SuperAgentCode);
            param.Add("@DistrictCode", agentFilter.DistrictCode);
            param.Add("@UserType", agentFilter.UserType);
            param.Add("@IsActive", agentFilter.UserStatus);
            param.Add("@PageNumber", agentFilter.PageNumber);
            param.Add("@PageSize", agentFilter.PageSize);
            param.Add("@SortingCol", agentFilter.SortBy);
            param.Add("@SortType", agentFilter.SortOrder);
            param.Add("@SearchVal", agentFilter.SearchVal);
            param.Add("@Export", agentFilter.Export);

            var data = await connection
                .QueryMultipleAsync("[dbo].[sp_get_agent_for_superagent_list]", param: param, commandType: CommandType.StoredProcedure);

            var UserList = await data.ReadAsync<AgentDetail>();
            var pagedInfo = await data.ReadFirstAsync<PageInfo>();
            var mappeddata = _mapper.Map<PagedList<AgentDetail>>(pagedInfo);
            mappeddata.Items = UserList;
            return mappeddata;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task<SprocMessage> RegisterAgent(RegisterAgent agentRegister)
    {
        var ImagedataTable = ImageTable();
        if (agentRegister.LicensedocImgPath != null && agentRegister.LicensedocImgPath.Count > 0)
        {
            foreach (var path in agentRegister.LicensedocImgPath)
            {
                var row = ImagedataTable.NewRow();
                row["DocumentImgPath"] = path;
                ImagedataTable.Rows.Add(row);
            }
        }
        using var connection = DbConnectionManager.GetDefaultConnection();
        var param = new DynamicParameters();

        param.Add("@Event", agentRegister.Event);
        param.Add("@FormNumber", agentRegister.FormNumber);
        param.Add("@AgentCode", agentRegister.AgentCode);
        param.Add("@UserName", agentRegister.UserName);
        param.Add("@FirstName", agentRegister.FirstName);
        param.Add("@LastName", agentRegister.LastName);
        param.Add("@PhoneNumber", agentRegister.PhoneNumber);
        param.Add("@PhoneNumberConfirmed", agentRegister.PhoneNumberConfirmed);
        param.Add("@CallingCode", agentRegister.CallingCode);
        param.Add("@OTP", agentRegister.Otp);
        param.Add("@OtpExipiryDate", agentRegister.OtpExipiryDate);
        param.Add("@WithoutFirstName", agentRegister.WithoutFirstName);
        param.Add("@Email", agentRegister.Email);
        param.Add("@EmailConfirmed", agentRegister.EmailConfirmed);
        param.Add("@PasswordHash", agentRegister.PasswordHash);
        param.Add("@PasswordSalt", agentRegister.PasswordSalt);
        param.Add("@CompanyLogoImgPath", agentRegister.CompanyLogoImgPath);
        param.Add("@OrganizationName", agentRegister.OrganizationName);
        param.Add("@RegistrationNumber", agentRegister.RegistrationNumber);
        param.Add("@District", agentRegister.District);
        param.Add("@City", agentRegister.City);
        param.Add("@Address", agentRegister.Address);
        param.Add("@DocumentImagePaths", ImagedataTable.AsTableValuedParameter("[dbo].[DocumentImagePaths]"));
        param.Add("@DocumentTypeId", agentRegister.DocumentTypeId);
        param.Add("@DocumentNumber", agentRegister.DocumentNumber);
        param.Add("@IssueDate", agentRegister.IssueDate);
        param.Add("@ExpiryDate", agentRegister.ExpiryDate);
        param.Add("@IdFrontImgPath", agentRegister.IdFrontImagePath);
        param.Add("@IdBackImgPath", agentRegister.IdBackImagePath);
        param.Add("@IsActive", agentRegister.IsActive);
        param.Add("@Maker", agentRegister.Maker);
        param.Add("@Checker", agentRegister.Checker);
        param.Add("@CreatedById", agentRegister.CreatedById);
        param.Add("@CreatedByName", agentRegister.CreatedByName);
        param.Add("@CreatedDate", agentRegister.CreatedDate);
        param.Add("@UpdatedById", agentRegister.UpdatedById);
        param.Add("@UpdatedByName", agentRegister.UpdatedByName);
        param.Add("@UpdatedDate", agentRegister.UpdatedDate);

        param.Add("@IdentityVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
        param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
        param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
        param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);
        try
        {
            _ = await connection.ExecuteAsync("[dbo].[usp_remit_agents_register]", param, commandType: CommandType.StoredProcedure);
        }
        catch (Exception ex)
        {
            return new SprocMessage { IdentityVal = 0, StatusCode = 400, MsgType = "Error", MsgText = "Error at Database" };
        }
        var identityVal = param.Get<int>("@IdentityVal");
        var statusCode = param.Get<int>("@StatusCode");
        var msgType = param.Get<string>("@MsgType");
        var msgText = param.Get<string>("@MsgText");

        return new SprocMessage { IdentityVal = identityVal, StatusCode = statusCode, MsgType = msgType, MsgText = msgText };
    }

    public async Task<AgentDetailSignUp> GetRegisterAgent(OtpValidationAgent validate)
    {
        using var connection = DbConnectionManager.GetDefaultConnection();
        var param = new DynamicParameters();
        param.Add("@Email", validate.Email);
        param.Add("@PhoneNumber", validate.PhoneNumber);
        var data = await connection
            .QueryMultipleAsync("[dbo].[usp_get_remitagent_register]", param: param, commandType: CommandType.StoredProcedure);
        var detail = await data.ReadFirstAsync<AgentDetailSignUp>();
        var imageList = await data.ReadAsync<string>();
        detail.DocumentImagePaths = imageList.ToList();
        return detail;
    }

    public async Task<SprocMessage> ValidateOtpAsync(OtpValidationAgent validate)
    {
        using var connection = DbConnectionManager.GetDefaultConnection();

        var param = new DynamicParameters();
        param.Add("@Email", validate.Email);
        param.Add("@PhoneNumber", validate.PhoneNumber);
        param.Add("@Otp", validate.Otp);

        param.Add("@IdentityVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
        param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
        param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
        param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);
        try
        {
            _ = await connection.ExecuteAsync("[dbo].[usp_valid_agentregister_otp]", param, commandType: CommandType.StoredProcedure);
        }
        catch (Exception)
        {
            return new SprocMessage { IdentityVal = 0, StatusCode = 400, MsgType = "Error", MsgText = "Error at Database" };
        }
        var identityVal = param.Get<int>("@IdentityVal");
        var statusCode = param.Get<int>("@StatusCode");
        var msgType = param.Get<string>("@MsgType");
        var msgText = param.Get<string>("@MsgText");

        return new SprocMessage { IdentityVal = identityVal, StatusCode = statusCode, MsgType = msgType, MsgText = msgText };
    }

    public async Task<AgentDetailSignUp> GetAgentDetailById(string Id)
    {
        using var connection = DbConnectionManager.GetDefaultConnection();
        var param = new DynamicParameters();
        param.Add("@Id", Id);
        var data = await connection
            .QueryMultipleAsync("[dbo].[usp_get_remitagent_register_byguidid]", param: param, commandType: CommandType.StoredProcedure);
        var detail = await data.ReadFirstAsync<AgentDetailSignUp>();
        var imageList = await data.ReadAsync<string>();
        detail.DocumentImagePaths = imageList.ToList();
        return detail;
    }

    public async Task<AgentDetailSignUp> GetAgentDetail(string Email, string phoneNumber)
    {
        using var connection = DbConnectionManager.GetDefaultConnection();
        var param = new DynamicParameters();
        param.Add("@Email", Email);
        param.Add("@PhoneNumber", phoneNumber);
        var data = await connection
            .QueryMultipleAsync("[dbo].[usp_get_remitagent_register]", param: param, commandType: CommandType.StoredProcedure);
        var detail = await data.ReadFirstAsync<AgentDetailSignUp>();
        var imageList = await data.ReadAsync<string>();
        detail.DocumentImagePaths = imageList.ToList();
        return detail;
    }

    public async Task<PagedList<CashAgentRegister>> GetRemitAgentRegisterAsync(AgentRegisterFilter request)
    {
        using var connection = DbConnectionManager.GetDefaultConnection();

        var param = new DynamicParameters();
        param.Add("@FullName", request.FullName);
        param.Add("@MobileNo", request.MobileNo);
        param.Add("@Email", request.Email);

        param.Add("@PageNumber", request.PageNumber);
        param.Add("@PageSize", request.PageSize);
        param.Add("@SortingCol", request.SortBy);
        param.Add("@SortType", request.SortOrder);
        param.Add("@SearchVal", request.SearchVal);
        param.Add("@Export", request.Export);

        var resultSets = await connection
            .QueryMultipleAsync("[dbo].[sp_get_remit_agents_registerlist]", param: param, commandType: CommandType.StoredProcedure);

        var agentRegister = await resultSets.ReadAsync<CashAgentRegister>();
        var pageInfo = await resultSets.ReadFirstAsync<PageInfo>();

        var resultData = _mapper.Map<PagedList<CashAgentRegister>>(pageInfo);
        resultData.Items = agentRegister;

        return resultData;
    }

    public async Task<SprocMessage> ApprovedRejectAgentRequest(CashAgentRequest request)
    {
        try
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@Id", request.Id);
            param.Add("@LoggedInUser", request.LoggedInUser);
            param.Add("@UserType", request.UserType);
            param.Add("@OperationMode", request.OperationMode);
            param.Add("@Email", request.Email);

            param.Add("@IdentityVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

            _ = await connection.ExecuteAsync(
                "[dbo].[usp_approvedreject_agentregister]", param: param, commandType: CommandType.StoredProcedure);

            var identityVal = param.Get<int>("@IdentityVal");
            var statusCode = param.Get<int>("@StatusCode");
            var msgType = param.Get<string>("@MsgType");
            var msgText = param.Get<string>("@MsgText");

            return new SprocMessage { StatusCode = statusCode, MsgType = msgType, MsgText = msgText, IdentityVal = identityVal };
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task<SprocMessage> AddUpdateFundRequestAsync(AddAgentFundRequest addUpdateFundRequest)
    {
        using var connection = DbConnectionManager.GetDefaultConnection();
        try
        {
            var param = new DynamicParameters();
            param.Add("@OperationMode", addUpdateFundRequest.Event);
            param.Add("@AgentCode", addUpdateFundRequest.AgentCode);
            param.Add("@FundType", addUpdateFundRequest.FundType);
            param.Add("@SourceCurrency", addUpdateFundRequest.SourceCurrency);
            param.Add("@Amount", addUpdateFundRequest.Amount);
            param.Add("@NotificationBalance", addUpdateFundRequest.NotificationBalance);
            param.Add("@TxnId", addUpdateFundRequest.TransactionId);
            param.Add("@VoucherImagePath", addUpdateFundRequest.VoucherImgPath);
            param.Add("@Remarks", addUpdateFundRequest.Remarks);
            param.Add("@LoggedInUser", addUpdateFundRequest.LoggedInUser);
            param.Add("@UserType", addUpdateFundRequest.UserType);

            param.Add("@ReturnPrimaryId", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

            _ = await connection.ExecuteAsync("[dbo].[usp_agent_prefund_addupdate]", param, commandType: CommandType.StoredProcedure);

            var identityVal = param.Get<int>("@ReturnPrimaryId");
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

    public async Task<AgentPrefundDetail> GetAgentPrefundByAgentCode(string AgentCode)
    {
        try
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@AgentCode", AgentCode);
            var data = await connection
                .QueryFirstOrDefaultAsync<AgentPrefundDetail>("[dbo].[usp_get_agent_prefund_byCode]", param: param, commandType: CommandType.StoredProcedure);
            return data;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task<PagedList<AgentAccountStatement>> GetAgentAccountSettlementReport(AgentStatementFilter filter)
    {
        using var connection = DbConnectionManager.GetDefaultConnection();

        var param = new DynamicParameters();
        param.Add("@AgentCode", filter.AgentCode);
        param.Add("@UserType", filter.UserType);
        param.Add("@DateFlag", filter.DateFlag);
        param.Add("@StartDate", filter.StartDate);
        param.Add("@EndDate", filter.EndDate);

        param.Add("@PageNumber", filter.PageNumber);
        param.Add("@PageSize", filter.PageSize);
        param.Add("@SortingCol", filter.SortBy);
        param.Add("@SortType", filter.SortOrder);
        param.Add("@SearchVal", filter.SearchVal);
        param.Add("@Export", filter.Export);

        var data = await connection
            .QueryMultipleAsync("[dbo].[usp_get_agent_account_statements]", param: param, commandType: CommandType.StoredProcedure);

        var prefundList = await data.ReadAsync<AgentAccountStatement>();
        var pagedInfo = await data.ReadFirstAsync<PageInfo>();
        var mappeddata = _mapper.Map<PagedList<AgentAccountStatement>>(pagedInfo);
        mappeddata.Items = prefundList;
        return mappeddata;
    }

    public async Task<SprocMessage> WithdrawPrefundAsync(Withdraw withdraw)
    {
        using var connection = DbConnectionManager.GetDefaultConnection();
        try
        {
            var param = new DynamicParameters();
            param.Add("@AgentCode", withdraw.AgentCode);
            param.Add("@Remarks", withdraw.Remarks);
            param.Add("@LoggedInUser", withdraw.LoggedInUser);
            param.Add("@UserType", withdraw.UserType);

            param.Add("@ReturnPrimaryId", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

            _ = await connection.ExecuteAsync("[dbo].[usp_agent_prefund_withdrawal]", param, commandType: CommandType.StoredProcedure);

            var identityVal = param.Get<int>("@ReturnPrimaryId");
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
}
