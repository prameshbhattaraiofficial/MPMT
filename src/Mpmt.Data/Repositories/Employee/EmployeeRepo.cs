using Dapper;
using Mpmt.Core.Dtos.Employee;
using Mpmt.Core.Dtos.Partner;
using Mpmt.Core.Dtos.PartnerEmployee;
using Mpmt.Core.Dtos.Users;
using Mpmt.Data.Common;
using Mpmts.Core.Dtos;
using System.Data;

namespace Mpmt.Data.Repositories.Employee
{
    /// <summary>
    /// The employee repo.
    /// </summary>
    public class EmployeeRepo : IEmployeeRepo
    {
        /// <summary>
        /// Adds the document type async.
        /// </summary>
        /// <param name="addDocumentType">The add document type.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> AddEmployeeAsync(IUDEmployee emp)
        {
            try
            {
                using var connection = DbConnectionManager.GetDefaultConnection();
                var param = new DynamicParameters();
                param.Add("@Event", "I");
                param.Add("Id", emp.Id);
                param.Add("@UserName", emp.UserName);
                param.Add("@FullName", emp.FullName);
                param.Add("@Email", emp.Email);
                param.Add("@EmailConfirmed", emp.EmailConfirmed);
                param.Add("@Gender", emp.Gender);
                param.Add("@Address", emp.Address);
                param.Add("@Department", emp.Department);
                param.Add("@DateOfBirth", emp.DateOfBirth);
                param.Add("@DateOfJoining", emp.DateOfJoining);
                param.Add("@PasswordHash", emp.PasswordHash);
                param.Add("@PasswordSalt", emp.PasswordSalt);
                param.Add("@AccessCodeHash", emp.AccessCodeHash);
                param.Add("@AccessCodeSalt", emp.AccessCodeSalt);
                param.Add("@FailedLoginAttempt", emp.FailedLoginAttempt);
                param.Add("@ProfileImageUrlPath", emp.ProfileImageUrlPath);
                param.Add("@LastIpAddress", emp.LastIpAddress);
                param.Add("@IsActive", emp.IsActive);
                param.Add("@IsDeleted", emp.IsDeleted);
                param.Add("@IsBlocked", emp.IsBlocked);
                param.Add("@Is2FAAuthenticated", emp.Is2FAAuthenticated);
                param.Add("@AccountSecretKey", emp.AccountSecretKey);
                param.Add("@RoleId", emp.RoleId);
                param.Add("@LoggedInUser", 1);
                param.Add("@IdentityVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
                param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

                _ = await connection.ExecuteAsync("[dbo].[Usp_IUD_users]", param, commandType: CommandType.StoredProcedure);

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
        /// Gets the document type async.
        /// </summary>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<EmployeeDetails>> GetEmployeeAsync()
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            return await connection.QueryAsync<EmployeeDetails>("[dbo].[usp_get_users]", commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Gets the document type by id async.
        /// </summary>
        /// <param name="documentTypeId">The document type id.</param>
        /// <returns>A Task.</returns>
        public async Task<IUDEmployee> GetEmployeeByIdAsync(int EmployeeId)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            var param = new DynamicParameters();
            param.Add("@Id", EmployeeId);
            return await connection.QueryFirstOrDefaultAsync<IUDEmployee>("[dbo].[usp_get_users_by_id]", param, commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Removes the document type async.
        /// </summary>
        /// <param name="removeDocumentType">The remove document type.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> RemoveEmployeeAsync(IUDEmployee emp)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            var param = new DynamicParameters();

            param.Add("@Event", "D");
            param.Add("Id", emp.Id);
            param.Add("@UserName", emp.UserName);
            param.Add("@FullName", emp.FullName);
            param.Add("@Email", emp.Email);
            param.Add("@EmailConfirmed", emp.EmailConfirmed);
            param.Add("@Gender", emp.Gender);
            param.Add("@Address", emp.Address);
            param.Add("@Department", emp.Department);
            param.Add("@DateOfBirth", emp.DateOfBirth);
            param.Add("@DateOfJoining", emp.DateOfJoining);
            param.Add("@PasswordHash", emp.PasswordHash);
            param.Add("@PasswordSalt", emp.PasswordSalt);
            param.Add("@AccessCodeHash", emp.AccessCodeHash);
            param.Add("@AccessCodeSalt", emp.AccessCodeSalt);
            param.Add("@FailedLoginAttempt", emp.FailedLoginAttempt);
            param.Add("@ProfileImageUrlPath", emp.ProfileImageUrlPath);
            param.Add("@LastIpAddress", emp.LastIpAddress);
            param.Add("@IsActive", emp.IsActive);
            param.Add("@IsDeleted", emp.IsDeleted);
            param.Add("@IsBlocked", emp.IsBlocked);
            param.Add("@Is2FAAuthenticated", emp.Is2FAAuthenticated);
            param.Add("@AccountSecretKey", emp.AccountSecretKey);
            param.Add("@RoleId", emp.RoleId);
            param.Add("@LoggedInUser", 1);
            param.Add("@IdentityVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

            _ = await connection.ExecuteAsync("[dbo].[Usp_IUD_users]", param, commandType: CommandType.StoredProcedure);

            var identityVal = param.Get<int>("@IdentityVal");
            var statusCode = param.Get<int>("@StatusCode");
            var msgType = param.Get<string>("@MsgType");
            var msgText = param.Get<string>("@MsgText");

            return new SprocMessage { IdentityVal = identityVal, StatusCode = statusCode, MsgType = msgType, MsgText = msgText };
        }

        /// <summary>
        /// Updates the document type async.
        /// </summary>
        /// <param name="updateDocumentType">The update document type.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> UpdateEmployeeAsync(IUDEmployee emp)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            var param = new DynamicParameters();

            param.Add("@Event", "U");
            param.Add("Id", emp.Id);
            param.Add("@UserName", emp.UserName);
            param.Add("@FullName", emp.FullName);
            param.Add("@Email", emp.Email);
            param.Add("@EmailConfirmed", emp.EmailConfirmed);
            param.Add("@Gender", emp.Gender);
            param.Add("@Address", emp.Address);
            param.Add("@Department", emp.Department);
            param.Add("@DateOfBirth", emp.DateOfBirth);
            param.Add("@DateOfJoining", emp.DateOfJoining);
            param.Add("@PasswordHash", emp.PasswordHash);
            param.Add("@PasswordSalt", emp.PasswordSalt);
            param.Add("@AccessCodeHash", emp.AccessCodeHash);
            param.Add("@AccessCodeSalt", emp.AccessCodeSalt);
            param.Add("@FailedLoginAttempt", emp.FailedLoginAttempt);
            param.Add("@ProfileImageUrlPath", emp.ProfileImageUrlPath);
            param.Add("@IsActive", emp.LastIpAddress);
            param.Add("@IsActive", emp.IsActive);
            param.Add("@IsDeleted", emp.IsDeleted);
            param.Add("@IsBlocked", emp.IsBlocked);
            param.Add("@Is2FAAuthenticated", emp.Is2FAAuthenticated);
            param.Add("@AccountSecretKey", emp.AccountSecretKey);
            param.Add("@RoleId", emp.RoleId);
            param.Add("@LoggedInUser", 1);
            param.Add("@IdentityVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

            _ = await connection.ExecuteAsync("[dbo].[Usp_IUD_users]", param, commandType: CommandType.StoredProcedure);

            var identityVal = param.Get<int>("@IdentityVal");
            var statusCode = param.Get<int>("@StatusCode");
            var msgType = param.Get<string>("@MsgType");
            var msgText = param.Get<string>("@MsgText");


            return new SprocMessage { IdentityVal = identityVal, StatusCode = statusCode, MsgType = msgType, MsgText = msgText };
        }
        public async Task<AppPartnerEmployee> GetPartnerEmployeeByEmailAsync(string email)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@Email", email);

            return await connection
                .QueryFirstOrDefaultAsync<AppPartnerEmployee>("[dbo].[usp_get_partner_employee_by_email]", param, commandType: CommandType.StoredProcedure);
        }
        public async Task<AppPartnerEmployee> GetPartnerEmployeeByUserNameAsync(string userName)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@UserName", userName);

            return await connection
                .QueryFirstOrDefaultAsync<AppPartnerEmployee>("[dbo].[usp_get_partner_Employee_by_username]", param, commandType: CommandType.StoredProcedure);
        }
        public async Task UpdatePartnerEmployeeLoginActivityAsync(UserLoginActivity loginActivity)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@UserId", loginActivity.UserId);
            param.Add("@FailedLoginAttempt", loginActivity.FailedLoginAttempt);
            param.Add("@TemporaryLockedTillUtcDate", loginActivity.TemporaryLockedTillUtcDate);
            param.Add("@LastIpAddress", loginActivity.LastIpAddress);
            param.Add("@DeviceId", loginActivity.DeviceId);
            param.Add("@IsActive", loginActivity.IsActive);
            param.Add("@IsBlocked", loginActivity.IsBlocked);
            param.Add("@LastLoginDateUtc", loginActivity.LastLoginDateUtc);
            param.Add("@LastActivityDateUtc", loginActivity.LastActivityDateUtc);

            _ = await connection.ExecuteAsync("[dbo].[usp_update_Partner_Employee_login_activity]", param, commandType: CommandType.StoredProcedure);
        }

        public async void UpdateAccountSecretKey(string email, string accountsecretkey)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@Email", email);
            param.Add("@AccountSecretKey", accountsecretkey);
            param.Add("@IdentityVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);
            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 50, direction: ParameterDirection.Output);
            var result = await connection.ExecuteAsync("[dbo].[usp_Update_AccountSecretKey_partner_employee_By_Email]", param, commandType: CommandType.StoredProcedure);
        }

        public async void UpdateIs2FAAuthenticated(string email, bool Is2FAAuthenticated)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@Email", email);
            param.Add("@Is2FAAuthenticated", Is2FAAuthenticated);
            param.Add("@IdentityVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);
            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 50, direction: ParameterDirection.Output);
            var result = await connection.ExecuteAsync("[dbo].[usp_Is2FAAuthenticated_partner_employee_By_Email]", param, commandType: CommandType.StoredProcedure);
        }

        public async Task<TokenVerification> GetOtpBypartnerEmployeeCodeAsync(string partnercode,string UserName, string OtpVerificationFor)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();

            param.Add("@PartnerCode", partnercode);
            param.Add("@UserName", UserName);
            param.Add("@OtpVerificationFor", OtpVerificationFor);

            var data = await connection
                .QueryFirstOrDefaultAsync<TokenVerification>("[dbo].[usp_getverificationtoken_bypartnercode_Username]", param: param, commandType: CommandType.StoredProcedure);

            return data;

        }
        public async void UpdateEmailConfirmAsync(string partnercode, string UserName)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();

            param.Add("@PartnerCode", partnercode);
            param.Add("@UserName", UserName);

            await connection
                .ExecuteAsync("[dbo].[UpdateEmailConfirm_bypartnercode_UserName]", param: param, commandType: CommandType.StoredProcedure);



        }
        public async Task<string> CheckPartnerOrEmployeeByEmail(string email)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@Email", email);
            param.Add("@UserType", dbType: DbType.String, size: 50, direction: ParameterDirection.Output);

            var result = await connection.ExecuteAsync("[dbo].[checkPartnerOREmployee_byEmail]", param, commandType: CommandType.StoredProcedure);
            var UserType = param.Get<string>("@UserType");
            return UserType;
        }

        public async Task<string> CheckPartnerOrEmployeeByUserName(string UserName)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@UserName", UserName);
            param.Add("@UserType", dbType: DbType.String, size: 50, direction: ParameterDirection.Output);

            var result = await connection.ExecuteAsync("[dbo].[checkPartnerOREmployee_byUserName]", param, commandType: CommandType.StoredProcedure);
            var UserType = param.Get<string>("@UserType");
            return UserType;
        }
    }
}
