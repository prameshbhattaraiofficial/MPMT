using AutoMapper;
using Dapper;
using Mpmt.Core.Dtos.AdminUser;
using Mpmt.Core.Dtos.IncomeSource;
using Mpmt.Core.Dtos.Notification;
using Mpmt.Core.Dtos.Paging;
using Mpmt.Core.Dtos.Users;
using Mpmt.Data.Common;
using Mpmts.Core.Dtos;
using System.Data;
using System.Diagnostics;

namespace Mpmt.Data.Repositories.Users.AdminUser
{
    /// <summary>
    /// The admin user repo.
    /// </summary>
    public class AdminUserRepo : IAdminUserRepo
    {
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdminUserRepo"/> class.
        /// </summary>
        /// <param name="mapper">The mapper.</param>
        public AdminUserRepo(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<SprocMessage> DeleteAdminUserAsync(int id, string remarks)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@Event", 'D');
            param.Add("@Id", id);
            param.Add("@Remarks", remarks);

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
        /// Gets the admin user async.
        /// </summary>
        /// <param name="userFilter">The user filter.</param>
        /// <returns>A Task.</returns>
        public async Task<PagedList<AdminUserDetails>> GetAdminUserAsync(AdminUserFilter userFilter)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@UserName", userFilter.UserName);
            param.Add("@Email", userFilter.Email);
            param.Add("@MobileNo", userFilter.MobileNumber);
            param.Add("@KycStatusCode", userFilter.KycStatusCode);
            param.Add("@countryCode", userFilter.CountryCode);
            param.Add("@UserStatus", userFilter.UserStatus);
            param.Add("@PageNumber", userFilter.PageNumber);
            param.Add("@PageSize", userFilter.PageSize);
            param.Add("@SortingCol", userFilter.SortBy);
            param.Add("@SortType", userFilter.SortOrder);
            param.Add("@SearchVal", userFilter.SearchVal);
            param.Add("@Export", userFilter.Export);
            var data = await connection
                .QueryMultipleAsync("[dbo].[sp_get_Users_list]", param: param, commandType: CommandType.StoredProcedure);

            var UserList = await data.ReadAsync<AdminUserDetails>();
            var pagedInfo = await data.ReadFirstAsync<PageInfo>();
            var mappeddata = _mapper.Map<PagedList<AdminUserDetails>>(pagedInfo);
            mappeddata.Items = UserList;
            return mappeddata;
        }

        public async Task<IUDAdminUser> GetAdminUserById(int id)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            var param = new DynamicParameters();
            param.Add("@Id", id);
            return await connection.QueryFirstOrDefaultAsync<IUDAdminUser>("[dbo].[usp_get_users_by_id]", param, commandType: CommandType.StoredProcedure);
        }

        public async Task<bool> VerifyUserNameAdminAsync(string userName)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            var param = new DynamicParameters();
            param.Add("@UserName", userName);
            param.Add("@Check", dbType: DbType.Boolean, direction: ParameterDirection.Output);

            var _ = await connection.ExecuteAsync("[dbo].[usp_username_validation]", param, commandType: CommandType.StoredProcedure);

            var CheckResult = param.Get<bool>("@Check");
            return CheckResult;
        }

        public async Task<bool> VerifyEmailAdminAsync(string Email)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();
            var param = new DynamicParameters();
            param.Add("@Email", Email);
            param.Add("@Check", dbType: DbType.Boolean, direction: ParameterDirection.Output);

            var _ = await connection.ExecuteAsync("[dbo].[usp_email_validation]", param, commandType: CommandType.StoredProcedure);

            var CheckResult = param.Get<bool>("@Check");
            return CheckResult;
        }

        public async Task<SprocMessage> IUDAdminUserAsync(IUDAdminUser adminUser)
        {
            try
            {
                using var connection = DbConnectionManager.GetDefaultConnection();

                var param = new DynamicParameters();
                param.Add("@Event", adminUser.Event);
                param.Add("@Id", adminUser.Id);
                param.Add("UserName", adminUser.UserName);
                param.Add("FullName", adminUser.FullName);
                param.Add("Email", adminUser.Email);
                param.Add("EmailConfirmed", adminUser.EmailConfirmed);
                param.Add("MobileNumber", adminUser.MobileNumber);
                param.Add("MobileConfirmed", adminUser.MobileConfirmed);
                param.Add("Gender", adminUser.Gender);
                param.Add("Address", adminUser.Address);
                param.Add("Department", adminUser.Department);
                param.Add("DateOfBirth", adminUser.DateOfBirth);
                param.Add("DateOfJoining", DateTime.Now);
                param.Add("PasswordHash", adminUser.PasswordHash);
                param.Add("PasswordSalt", adminUser.PasswordSalt);
                param.Add("AccessCodeHash", adminUser.AccessCodeHash);
                param.Add("AccessCodeSalt", adminUser.AccessCodeSalt);
                param.Add("FailedLoginAttempt", adminUser.FailedLoginAttempt);
                param.Add("ProfileImageUrlPath", adminUser.ProfileImageUrlPath);
                param.Add("LastIpAddress", adminUser.LastIpAddress);
                param.Add("DeviceId", adminUser.DeviceId);
                param.Add("IsActive", adminUser.IsActive);
                param.Add("IsDeleted", adminUser.IsDeleted);
                param.Add("IsBlocked", adminUser.IsBlocked);
                param.Add("Is2FAAuthenticated", adminUser.Is2FAAuthenticated);
                param.Add("AccountSecretKey", adminUser.AccountSecretKey);
                param.Add("RoleId", adminUser.RoleId);
                param.Add("LoggedInUser", adminUser.LoggedInUser);

                param.Add("@IdentityVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
                param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

                try
                {
                    _ = await connection.ExecuteAsync("[dbo].[Usp_IUD_users]", param, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                }

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

        public async Task<SprocMessage>  AssignRoletoUser(int user_id, int[] roleids)
        {
            try
            {
                using var connection = DbConnectionManager.GetDefaultConnection();

                var param = new DynamicParameters();
               
                param.Add("@UserId", user_id);
                //param.Add("@RoleIds", string.Join(',', roleids));
                param.Add("@RoleIds", string.Join(",", roleids ?? Array.Empty<int>()));

                param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
                param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

                _ = await connection.ExecuteAsync("[dbo].[usp_add_roles_to_user]", param, commandType: CommandType.StoredProcedure);


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

        public async Task<SprocMessage> AssignPartnerRole(int user_id, int[] roleids)
        {
            try
            {
                using var connection = DbConnectionManager.GetDefaultConnection();

                var param = new DynamicParameters();

                param.Add("@PartnerId", user_id);
                //param.Add("@RoleIds", string.Join(',', roleids));
                param.Add("@RoleIds", string.Join(",", roleids ?? Array.Empty<int>()));

                param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
                param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

                _ = await connection.ExecuteAsync("[dbo].[usp_add_roles_to_partner]", param, commandType: CommandType.StoredProcedure);

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
    }
}
