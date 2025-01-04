using Mpmt.Core.Dtos;
using Mpmt.Core.Dtos.Partner;
using Mpmt.Core.Dtos.Users;
using Mpmt.Core.ViewModel.ForgotPassword;
using Mpmts.Core.Dtos;

namespace Mpmt.Services.Users
{
    /// <summary>
    /// The users service.
    /// </summary>
    public interface IUsersService
    {
        /// <summary>
        /// Adds the user async.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>A Task.</returns>
        Task<MpmtResult> AddUserAsync(AppUser user);
        Task<SprocMessage> ChangePasswordAsync(AppUser user);
        Task<SprocMessage> ForgotPasswordAsync(PasswordResetModel reset);
        Task<(SprocMessage, PasswordResetModel)> RequestTokenValidationAsync(string token);
        Task<(SprocMessage, PasswordResetModel)> ResetTokenValidationAsync(PasswordResetModel resetModel);
        Task SendPasswordResetList(string link, string emails);
        /// <summary>
        /// Checks the user exists by email async.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns>A Task.</returns>
        Task<bool> CheckUserExistsByEmailAsync(string email);
        /// <summary>
        /// Checks the user exists by user name async.
        /// </summary>
        /// <param name="userName">The user name.</param>
        /// <returns>A Task.</returns>
        Task<bool> CheckUserExistsByUserNameAsync(string userName);
        /// <summary>
        /// Gets the user by id async.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>A Task.</returns>
        Task<AppUser> GetUserByIdAsync(int id);
        /// <summary>
        /// Gets the user by user name async.
        /// </summary>
        /// <param name="userName">The user name.</param>
        /// <returns>A Task.</returns>
        Task<AppUser> GetUserByUserNameAsync(string userName);
        /// <summary>
        /// Gets the user by email async.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns>A Task.</returns>
        Task<AppUser> GetUserByEmailAsync(string email);
        /// <summary>
        /// Updates the user login activity async.
        /// </summary>
        /// <param name="loginActivity">The login activity.</param>
        /// <returns>A Task.</returns>
        Task UpdateUserLoginActivityAsync(UserLoginActivity loginActivity);
        /// <summary>
        /// Updates the account secret key.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="accountsecretkey">The accountsecretkey.</param>
        /// <returns>A Task.</returns>
        void UpdateAccountSecretKey(string email, string accountsecretkey);
        Task<SprocMessage> AddLoginOtpAsync(TokenVerification tokenVerification);
        Task<TokenVerification> GetOtpByUserNameAsync(string UserName);
        void UpdateEmailConfirmAsync(string UserName);


    }
}
