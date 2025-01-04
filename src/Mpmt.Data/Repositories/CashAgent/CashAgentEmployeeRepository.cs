using AutoMapper;
using Dapper;
using Mpmt.Core.Dtos.CashAgent;
using Mpmt.Core.Dtos.Paging;
using Mpmt.Core.Dtos.SuperAgent;
using Mpmt.Data.Common;
using Mpmts.Core.Dtos;
using System.Data;

namespace Mpmt.Data.Repositories.CashAgent
{
    public class CashAgentEmployeeRepository : ICashAgentEmployeeRepository
    {
        private readonly IMapper _mapper;

        public CashAgentEmployeeRepository(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<PagedList<AgentDetail>> GetAgentEmployeeAsync(AgentFilter AgentFilter)
        {
            try
            {
                using var connection = DbConnectionManager.GetDefaultConnection();
                var param = new DynamicParameters();

                param.Add("@Email", AgentFilter.Email);
                param.Add("@UserName", AgentFilter.UserName);
                param.Add("@ContactNumber", AgentFilter.ContactNumber);
                param.Add("@AgentCode", AgentFilter.AgentCode);
                param.Add("@IsActive", AgentFilter.UserStatus);
                param.Add("@UserType", AgentFilter.UserType);
                param.Add("@PageNumber", AgentFilter.PageNumber);
                param.Add("@PageSize", AgentFilter.PageSize);
                param.Add("@SortingCol", AgentFilter.SortBy);
                param.Add("@SortType", AgentFilter.SortOrder);
                param.Add("@SearchVal", AgentFilter.SearchVal);
                param.Add("@Export", AgentFilter.Export);

                var data = await connection
                    .QueryMultipleAsync("[dbo].[sp_get_remit_CashAgentEmployee]", param: param, commandType: CommandType.StoredProcedure);

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

        public async Task<AgentUser> GetAgentEmployeeUserByPhonenumberAsync(string PhoneNUmber)
        {
            try
            {
                using var connection = DbConnectionManager.GetDefaultConnection();

                var param = new DynamicParameters();
                param.Add("@contact", PhoneNUmber);
                var data = await connection
                    .QueryFirstOrDefaultAsync<AgentUser>("[dbo].[usp_get_agentEmployee_by_contact]", param: param, commandType: CommandType.StoredProcedure);
                return data;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<AgentUser> GetAgentEmployeeUserByEmployeeIdAsync(string EmployeeId, string agentCode)
        {
            try
            {
                using var connection = DbConnectionManager.GetDefaultConnection();

                var param = new DynamicParameters();
                param.Add("@EmployeeId", EmployeeId);
                param.Add("@AgentCode", agentCode);
                var data = await connection
                    .QueryFirstOrDefaultAsync<AgentUser>("[dbo].[usp_get_agentEmployee_by_EmployeeId]", param: param, commandType: CommandType.StoredProcedure);
                return data;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<AgentUser> GetAgentEmployeeUserByUserName(string UserName)
        {
            try
            {
                using var connection = DbConnectionManager.GetDefaultConnection();
                var param = new DynamicParameters();
                param.Add("@UserName", UserName);
                var data = await connection
                    .QueryFirstOrDefaultAsync<AgentUser>("[dbo].[usp_get_agentEmployee_by_username]", param: param, commandType: CommandType.StoredProcedure);
                return data;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<SprocMessage> IUDAgentEmployeeUserAsync(CashAgentUser agentUser)
        {
            try
            {
                using var connection = DbConnectionManager.GetDefaultConnection();
                var param = new DynamicParameters();
                param.Add("@Event", agentUser.Event);
                param.Add("@EmployeeId", agentUser.EmployeeId);
                param.Add("@AgentCode", agentUser.AgentCode);
                param.Add("@SuperAgentCode", agentUser.SuperAgentCode);
                param.Add("@FirstName", agentUser.FirstName);
                param.Add("@LastName", agentUser.LastName);
                param.Add("@Email", agentUser.Email);
                param.Add("@EmailConfirmed", agentUser.EmailConfirmed);
                param.Add("@ContactNumber", agentUser.ContactNumber);
                param.Add("@ContactNumberConfirmed", agentUser.ContactNumberConfirmed);
                param.Add("@LookupName", agentUser.LookupName);
                param.Add("@UserName", agentUser.UserName);
                param.Add("@Remarks", agentUser.Remarks);
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

                _ = await connection.ExecuteAsync("[dbo].[Usp_IUD_remit_Agent_employee]", param, commandType: CommandType.StoredProcedure);

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

        public async Task UpdateAgentEmployeeLoginActivityAsync(AgentLoginActivity agentLoginActivity, int EmployeeId)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            var param = new DynamicParameters();
            param.Add("@AgentCode", agentLoginActivity.AgentCode);
            param.Add("@EmployeeId", EmployeeId);
            param.Add("@FailedLoginAttempt", agentLoginActivity.FailedLoginAttempt);
            param.Add("@TemporaryLockedTillUtcDate", agentLoginActivity.TemporaryLockedTillUtcDate);
            param.Add("@IpAddress", agentLoginActivity.IpAddress);
            param.Add("@DeviceId", agentLoginActivity.DeviceId);
            param.Add("@IsActive", agentLoginActivity.IsActive);
            param.Add("@IsBlocked", agentLoginActivity.IsBlocked);
            param.Add("@LastLoginDateUtc", agentLoginActivity.LastLoginDateUtc);
            param.Add("@LastActivityDateUtc", agentLoginActivity.LastActivityDateUtc);

            _ = await connection.ExecuteAsync("[dbo].[usp_update_agentEmployee_login_activity]", param, commandType: CommandType.StoredProcedure);
        }

        public async Task UpdateEmployeeAccountSecretKeyAsync(string AgentCode, string accountsecretkey, int EmployeeId)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@Agentcode", AgentCode);
            param.Add("@EmployeeId", EmployeeId);
            param.Add("@AccountSecretKey", accountsecretkey);
            param.Add("@IdentityVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);
            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 50, direction: ParameterDirection.Output);

            _ = await connection.ExecuteAsync("[dbo].[usp_Update_AgentEmployee_AccountSecretKey_By_EmployeeId]", param, commandType: CommandType.StoredProcedure);
        }

        public async Task UpdateIs2FAAuthenticatedAgentEmployeeAsync(string Agentcode, bool Is2FAAuthenticated, int EmployeeId)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@Agentcode", Agentcode);
            param.Add("@EmployeeId", EmployeeId);
            param.Add("@Is2FAAuthenticated", Is2FAAuthenticated);
            param.Add("@IdentityVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);
            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 50, direction: ParameterDirection.Output);
            
            _ = await connection.ExecuteAsync("[dbo].[usp_update_Is2FAAuthenticated_By_AgentEmployeeId]", param, commandType: CommandType.StoredProcedure);
        }

        public async Task<string> CheckAgentOrEmployeeByContactNumber(string ContactNumber)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@ContactNumber", ContactNumber);
            param.Add("@UserType", dbType: DbType.String, size: 50, direction: ParameterDirection.Output);

            var result = await connection.ExecuteAsync("[dbo].[checkAgentOREmployee_byContactNumber]", param, commandType: CommandType.StoredProcedure);
            var UserType = param.Get<string>("@UserType");
            return UserType;
        }

        public async Task<string> CheckAgentOrEmployeeByUserName(string UserName)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@UserName", UserName);
            param.Add("@UserType", dbType: DbType.String, size: 50, direction: ParameterDirection.Output);

            var result = await connection.ExecuteAsync("[dbo].[checkAgentOREmployee_byUserName]", param, commandType: CommandType.StoredProcedure);
            var UserType = param.Get<string>("@UserType");
            
            return UserType;
        }

        public async Task<SprocMessage> AgentEmployeeChangePasswordAsync(CashAgentUser user)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@Event", user.Event);
            param.Add("@EmployeeId", user.EmployeeId);
            param.Add("@UserName", user.UserName);
            param.Add("@AgentCode", user.AgentCode);
            param.Add("@PasswordHash", user.PasswordHash);
            param.Add("@PasswordSalt", user.PasswordSalt);
            param.Add("@UserType", user.UserType);
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

        public async Task<bool> VerifyUserNameAsync(string userName)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            var param = new DynamicParameters();
            param.Add("@UserName", userName);
            param.Add("@Check", dbType: DbType.Boolean, direction: ParameterDirection.Output);

            var _ = await connection.ExecuteAsync("[dbo].[usp_check_agent_employee_username]", param, commandType: CommandType.StoredProcedure);

            var CheckResult = param.Get<bool>("@Check");
            return CheckResult;
        }
    }
}
