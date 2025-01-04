using Dapper;
using Mpmt.Core.Dtos.ConversionRate;
using Mpmt.Core.Dtos.Partner;
using Mpmt.Core.Dtos.Users;
using Mpmt.Core.ViewModel.ForgotPassword;
using Mpmt.Data.Common;
using Mpmts.Core.Dtos;
using System.Data;
using System.Reflection;

namespace Mpmt.Data.Repositories.Users
{
    /// <summary>
    /// The users repository.
    /// </summary>
    public class UsersRepository : IUsersRepository
    {
        /// <summary>
        /// Adds the user async.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> AddUserAsync(AppUser user)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@UserName", user.UserName);
            param.Add("@FullName", user.FullName);
            param.Add("@Email", user.Email);
            param.Add("@EmailConfirmed", user.EmailConfirmed);
            // param.Add("@CountryCallingCode", user.CountryCallingCode);
            param.Add("@MobileNumber", user.MobileNumber);
            param.Add("@MobileConfirmed", user.MobileConfirmed);
            param.Add("@Gender", user.Gender);
            param.Add("@Address", user.Address);
            param.Add("@Department", user.Department);
            param.Add("@DateOfBirth", user.DateOfBirth);
            param.Add("@DateOfJoining", user.DateOfJoining);
            param.Add("@PasswordHash", user.PasswordHash);
            param.Add("@PasswordSalt", user.PasswordSalt);
            param.Add("@AccessCodeHash", user.AccessCodeHash);
            param.Add("@AccessCodeSalt", user.AccessCodeSalt);
            param.Add("@FailedLoginAttempt", user.FailedLoginAttempt);
            param.Add("@TemporaryLockedTillUtcDate", user.TemporaryLockedTillUtcDate);
            param.Add("@ProfileImageUrlPath", user.ProfileImageUrlPath);
            param.Add("@LastIpAddress", user.LastIpAddress);
            param.Add("@DeviceId", user.DeviceId);
            param.Add("@IsActive", user.IsActive);
            param.Add("@IsDeleted", user.IsDeleted);
            param.Add("@IsBlocked", user.IsBlocked);
            param.Add("@LastLoginDateUtc", user.LastLoginDateUtc);
            param.Add("@LastActivityDateUtc", user.LastActivityDateUtc);
            param.Add("@LoggedInUser", user.CreatedBy);

            param.Add("@IdentityVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

            _ = await connection.ExecuteAsync("[dbo].[usp_add_user]", param, commandType: CommandType.StoredProcedure);

            var identityVal = param.Get<int>("@IdentityVal");
            var statusCode = param.Get<int>("@StatusCode");
            var msgType = param.Get<string>("@MsgType");
            var msgText = param.Get<string>("@MsgText");

            return new SprocMessage { IdentityVal = identityVal, StatusCode = statusCode, MsgType = msgType, MsgText = msgText };
        }
        public async Task<SprocMessage> ChangePasswordAsync(AppUser user)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@Event", "CP");
            param.Add("@Id", user.Id);
            param.Add("@UserName", user.UserName);
            param.Add("@PasswordHash", user.PasswordHash);
            param.Add("@PasswordSalt", user.PasswordSalt);
            param.Add("@LoggedInUser", user.UpdatedBy);

            param.Add("@IdentityVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

            _ = await connection.ExecuteAsync("[dbo].[Usp_IUD_users_changepassword]", param, commandType: CommandType.StoredProcedure);

            var identityVal = param.Get<int>("@IdentityVal");
            var statusCode = param.Get<int>("@StatusCode");
            var msgType = param.Get<string>("@MsgType");
            var msgText = param.Get<string>("@MsgText");

            return new SprocMessage { IdentityVal = identityVal, StatusCode = statusCode, MsgType = msgType, MsgText = msgText };
        }

        /// <summary>
        /// Checks the user exists by email async.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns>A Task.</returns>
        public async Task<bool> CheckUserExistsByEmailAsync(string email)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@Email", email);

            return await connection
                .QueryFirstAsync<bool>("[dbo].[usp_check_user_exists_by_email]", param: param, commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Checks the user exists by user name async.
        /// </summary>
        /// <param name="userName">The user name.</param>
        /// <returns>A Task.</returns>
        public async Task<bool> CheckUserExistsByUserNameAsync(string userName)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@UserName", userName);

            return await connection
                .QueryFirstAsync<bool>("[dbo].[usp_check_user_exists_by_username]", param: param, commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Gets the user by email async.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns>A Task.</returns>
        public async Task<AppUser> GetUserByEmailAsync(string email)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@Email", email);

            return await connection
                .QueryFirstOrDefaultAsync<AppUser>("[dbo].[usp_get_user_by_email]", param, commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Gets the user by id async.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>A Task.</returns>
        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@UserId", id);

            return await connection
                .QueryFirstOrDefaultAsync<AppUser>("[dbo].[usp_get_user_by_id]", param, commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Gets the user by user name async.
        /// </summary>
        /// <param name="userName">The user name.</param>
        /// <returns>A Task.</returns>
        public async Task<AppUser> GetUserByUserNameAsync(string userName)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@UserName", userName);

            return await connection
                .QueryFirstOrDefaultAsync<AppUser>("[dbo].[usp_get_user_by_username]", param, commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Updates the user login activity async.
        /// </summary>
        /// <param name="loginActivity">The login activity.</param>
        /// <returns>A Task.</returns>
        public async Task UpdateUserLoginActivityAsync(UserLoginActivity loginActivity)
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

            _ = await connection.ExecuteAsync("[dbo].[usp_update_user_login_activity]", param, commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Updates the account secret key.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="accountsecretkey">The accountsecretkey.</param>
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
            var result = await connection.ExecuteAsync("[dbo].[usp_Update_AccountSecretKey_By_Email]", param, commandType: CommandType.StoredProcedure);
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
            var result = await connection.ExecuteAsync("[dbo].[usp_update_Is2FAAuthenticated_By_Email]", param, commandType: CommandType.StoredProcedure);
        }
        public async Task<SprocMessage> AddLoginOtpAsync(TokenVerification tokenVerification)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();
            param.Add("@UserId", tokenVerification.UserId);
            param.Add("@PartnerCode", tokenVerification.PartnerCode);
            param.Add("@Username", tokenVerification.UserName);
            param.Add("@Email", tokenVerification.Email);
            param.Add("@CountryCode", tokenVerification.CountryCallingCode);
            param.Add("@Mobile", tokenVerification.Mobile);
            param.Add("@VerificationCode", tokenVerification.VerificationCode);
            param.Add("@VerificationType", tokenVerification.VerificationType);
            param.Add("@SendToMobile", tokenVerification.SendToMobile);
            param.Add("@SendToEmail", tokenVerification.SendToEmail);
            param.Add("@ExpiryDate", tokenVerification.ExpiredDate);
            param.Add("@IsConsumed", tokenVerification.IsConsumed);
            param.Add("@OtpVerificationFor", tokenVerification.OtpVerificationFor);

            param.Add("@ReturnPrimaryId", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
            param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);


            try
            {
                var _ = await connection.ExecuteAsync("[dbo].[Usp_IUD_Token_Verification]", param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception)
            {
                return new SprocMessage { IdentityVal = 0, StatusCode = 400, MsgType = "Error", MsgText = "Issue Inserting Data" };
            }

            var identityVal = param.Get<int>("@ReturnPrimaryId");
            var statusCode = param.Get<int>("@StatusCode");
            var msgType = param.Get<string>("@MsgType");
            var msgText = param.Get<string>("@MsgText");




            return new SprocMessage { IdentityVal = identityVal, StatusCode = statusCode, MsgType = msgType, MsgText = msgText };
        }

        public async Task<TokenVerification> GetOtpByUsernameAsync(string UserName)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();

            param.Add("@UserName", UserName);

            var data = await connection
                .QueryFirstOrDefaultAsync<TokenVerification>("[dbo].[usp_getverificationtoken_byUsername_foradmin]", param: param, commandType: CommandType.StoredProcedure);

            return data;

        }
        public async void UpdateEmailConfirmAsync(string Username)
        {
            using var connection = DbConnectionManager.GetDefaultConnection();

            var param = new DynamicParameters();

            param.Add("@UserName", Username);

            await connection
                .ExecuteAsync("[dbo].[UpdateEmailConfirm_byUserName]", param: param, commandType: CommandType.StoredProcedure);



        }

        public async Task<SprocMessage> ForgotPasswordAsync(PasswordResetModel model)
        {
            try
            {
                using var connection = DbConnectionManager.GetDefaultConnection();

                var param = new DynamicParameters();
                param.Add("@UserId", model.UserId);
                param.Add("@UserType", model.UserType);
                param.Add("@ResetRequestToken", model.ResetRequestToken);
                param.Add("@ResetToken", model.ResetToken);

                param.Add("@IdentityVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("@MsgType", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
                param.Add("@MsgText", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

                _ = await connection.ExecuteAsync("[dbo].[usp_forgot_password_request]", param:param, commandType: CommandType.StoredProcedure);

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

        public async Task<(SprocMessage, PasswordResetModel)> RequestTokenValidationAsync(string token)
        {
            try
            {
                using var connection = DbConnectionManager.GetDefaultConnection();
                var param = new DynamicParameters();
                param.Add("@ResetRequestToken", token);
                var data = await connection
                    .QueryMultipleAsync("[dbo].[usp_request_token_validation]", param: param, commandType: CommandType.StoredProcedure);
                var userData = await data.ReadFirstOrDefaultAsync<PasswordResetModel>();
                //var sprocMessage = await data.ReadSingleOrDefaultAsync<SprocMessage>();
                var sprocMessage = await data.ReadSingleOrDefaultAsync<SprocMessage>();
                return (sprocMessage, userData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<(SprocMessage, PasswordResetModel)> ResetTokenValidationAsync(PasswordResetModel resetModel)
        {
            try
            {
                using var connection = DbConnectionManager.GetDefaultConnection();

                var param = new DynamicParameters();
                param.Add("@ResetToken", resetModel.ResetToken);
                param.Add("@ResetRequestToken", resetModel.ResetRequestToken);

                var data = await connection
                .QueryMultipleAsync("[dbo].[usp_reset_token_validation]", param: param, commandType: CommandType.StoredProcedure);
                var userData = await data.ReadFirstOrDefaultAsync<PasswordResetModel>();
                //var sprocMessage = await data.ReadSingleOrDefaultAsync<SprocMessage>();
                var sprocMessage = await data.ReadFirstOrDefaultAsync<SprocMessage>();
                return (sprocMessage, userData);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
