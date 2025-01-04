using Dapper;
using Microsoft.AspNetCore.Http;
using Mpmt.Core.Dtos.DropDown;
using Mpmt.Core.Dtos.PartnerEmployee;
using Mpmt.Data.Common;
using Mpmts.Core.Dtos;
using System.Data;

namespace Mpmt.Data.Repositories.PartnerEmployee;

public class PartnerEmployeeRepo : IPartnerEmployeeRepo
{
    public async Task<IEnumerable<PartnerEmployeeList>> GetPartnerEmployeeAsync(string code)
    {
        using var connection = DbConnectionManager.GetDefaultConnection();
        var param = new DynamicParameters();
        param.Add("@PartnerCode", code);
        return await connection.QueryAsync<PartnerEmployeeList>("[dbo].[Usp_get_remit_partners_employees]", param, commandType: CommandType.StoredProcedure);
    }

    public async Task<PartnerEmployeeList> GetPartnerEmployeeByIdAsync(int id, string code)
    {
        using var connection = DbConnectionManager.GetDefaultConnection();
        var param = new DynamicParameters();
        param.Add("@Id", id);
        param.Add("@PartnerCode", code);
        return await connection.QueryFirstOrDefaultAsync<PartnerEmployeeList>("[dbo].[Usp_get_remit_partners_employee_byId]", param, commandType: CommandType.StoredProcedure);
    }
    public async Task<IEnumerable<Commonddl>> GetPartnerEmployeeRolesByIdAsync(int id, string code)
    {
        using var connection = DbConnectionManager.GetDefaultConnection();
        var param = new DynamicParameters();
        param.Add("@Id", id);
        param.Add("@PartnerCode", code);
        return await connection.QueryAsync<Commonddl>("[dbo].[Usp_get_remit_partners_employee_Roles_byId]", param, commandType: CommandType.StoredProcedure);
    }

    public async Task<SprocMessage> IUDPartnerEmployeeAsync(IUDPartnerEmployee partnerEmployee)
    {
        try
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@Event", partnerEmployee.Event);
            param.Add("@Id", partnerEmployee.Id);
            param.Add("@PartnerCode", partnerEmployee.PartnerCode);
            param.Add("@FirstName", partnerEmployee.FirstName);
            param.Add("@SurName", partnerEmployee.SurName);
            param.Add("@IsSurNamePresent", partnerEmployee.IsSurNamePresent);
            param.Add("@MobileNumber", partnerEmployee.MobileNumber);
            param.Add("@MobileConfirmed", partnerEmployee.MobileConfirmed);
            param.Add("@Email", partnerEmployee.Email);
            param.Add("@EmailConfirmed", partnerEmployee.EmailConfirmed);
            param.Add("@Post", partnerEmployee.Post);
            param.Add("@GenderId", partnerEmployee.GenderId);
            param.Add("@UserName", partnerEmployee.UserName);
            param.Add("@Remarks", partnerEmployee.Remarks);
            param.Add("@PasswordHash", partnerEmployee.PasswordHash);
            param.Add("@PasswordSalt", partnerEmployee.PasswordSalt);
            param.Add("@AccessCodeHash", partnerEmployee.AccessCodeHash);
            param.Add("@AccessCodeSalt", partnerEmployee.AccessCodeSalt);
            param.Add("@MPINHash", partnerEmployee.MPINHash);
            param.Add("@MPINSalt", partnerEmployee.MPINSalt);
            param.Add("@IpAddress", partnerEmployee.IpAddress);
            param.Add("@LastIpAddress", partnerEmployee.LastIpAddress);
            param.Add("@DeviceId", partnerEmployee.DeviceId);
            param.Add("@IsActive", partnerEmployee.IsActive);
            param.Add("@IsDeleted", partnerEmployee.IsDeleted);
            param.Add("@IsBlocked", partnerEmployee.IsBlocked);
            param.Add("@FailedLoginAttempt", partnerEmployee.FailedLoginAttempt);
            param.Add("@TemporaryLockedTillUtcDate", partnerEmployee.TemporaryLockedTillUtcDate);
            param.Add("@LastLoginDateUtc", partnerEmployee.LastLoginDateUtc);
            param.Add("@LastActivityDateUtc", partnerEmployee.LastActivityDateUtc);
            param.Add("@Is2FAAuthenticated", partnerEmployee.Is2FAAuthenticated);
            param.Add("@AccountSecretKey", partnerEmployee.AccountSecretKey);
            param.Add("@CreatedById", partnerEmployee.CreatedById);
            param.Add("@CreatedByName", partnerEmployee.CreatedByName);
            param.Add("@CreatedDate", partnerEmployee.CreatedDate);
            param.Add("@UpdatedById", partnerEmployee.UpdatedById);
            param.Add("@UpdatedByName", partnerEmployee.UpdatedByName);
            param.Add("@UpdatedDate", partnerEmployee.UpdatedDate);

            param.Add("@IdentityVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

            _ = await connection.ExecuteAsync("[dbo].[Usp_IUD_remit_partners_employee]", param, commandType: CommandType.StoredProcedure);

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
    public async Task<SprocMessage> PartnerEmployeeChangePasswordAsync(IUDPartnerEmployee partnerEmployee)
    {
        try
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@Event", partnerEmployee.Event);
            param.Add("@Id", partnerEmployee.Id);
            param.Add("@PartnerCode", partnerEmployee.PartnerCode);
            param.Add("@UserName", partnerEmployee.UserName);
            param.Add("@PasswordHash", partnerEmployee.PasswordHash);
            param.Add("@PasswordSalt", partnerEmployee.PasswordSalt);
            param.Add("@AccessCodeHash", partnerEmployee.AccessCodeHash);
            param.Add("@AccessCodeSalt", partnerEmployee.AccessCodeSalt);
            param.Add("@MPINHash", partnerEmployee.MPINHash);
            param.Add("@MPINSalt", partnerEmployee.MPINSalt);
            param.Add("@UpdatedById", partnerEmployee.UpdatedById);
            param.Add("@UpdatedByName", partnerEmployee.UpdatedByName);
            

            param.Add("@IdentityVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

            _ = await connection.ExecuteAsync("[dbo].[Usp_IUD_remit_partners_employee_changePassword]", param, commandType: CommandType.StoredProcedure);

            var identityVal = param.Get<int>("@IdentityVal");
            var statusCode = param.Get<int>("@StatusCode");
            var msgType = param.Get<string>("@MsgType");
            var msgText = param.Get<string>("@MsgText");

            return new SprocMessage { IdentityVal = identityVal, StatusCode = statusCode, MsgType = msgType, MsgText = msgText };
        }
        catch (Exception ex)
        {
            return new SprocMessage { IdentityVal = 0, StatusCode = 400, MsgType = "Error", MsgText = "Please Try again later." };
        }
    }
    public async Task<SprocMessage> AssignUserRoleAsync(string PartnerId, int user_id, int[] roleids)
    {
        try
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@partnerCode", PartnerId);
            param.Add("@PartnerEmployeeId", user_id);
            //param.Add("@RoleIds", string.Join(',', roleids));
            param.Add("@RoleIds", string.Join(",", roleids ?? Array.Empty<int>()));



            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

            _ = await connection.ExecuteAsync("[dbo].[usp_add_roles_to_partnerEmployee]", param, commandType: CommandType.StoredProcedure);


            var statusCode = param.Get<int>("@StatusCode");
            var msgType = param.Get<string>("@MsgType");
            var msgText = param.Get<string>("@MsgText");

            return new SprocMessage { IdentityVal = 0, StatusCode = statusCode, MsgType = msgType, MsgText = msgText };
        }
        catch (Exception)
        {
            throw;
        }
    }
}
