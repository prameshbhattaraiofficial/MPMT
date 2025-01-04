using Mpmt.Core.Domain;
using Mpmt.Core.Dtos.Partner;
using Mpmt.Core.Dtos.Users;
using Mpmt.Core.ViewModel.ChangePassword;
using Mpmt.Core.ViewModel.ForgotPassword;
using Mpmts.Core.Dtos;

namespace Mpmt.Services.Users
{
    /// <summary>
    /// The user registration service.
    /// </summary>
    public interface IUserRegistrationService
    {
        /// <summary>
        /// Validates the user async.
        /// </summary>
        /// <param name="usernameOrEmail">The username or email.</param>
        /// <param name="password">The password.</param>
        /// <returns>A Task.</returns>
        Task<(LoginResults, AppUser)> ValidateUserAsync(string usernameOrEmail, string password);
        Task<(LoginResults, AppUser)> ValidateUserEmailAsync(string email);
        /// <summary>
        /// Registers the user async.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>A Task.</returns>
        Task<UserRegistrationResult> RegisterUserAsync(UserRegistrationRequest request);
        /// <summary>
        /// Changes the password async.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>A Task.</returns>
        Task<SprocMessage> ChangePasswordAsync(PartnerChangePasswordVM request);
        Task<SprocMessage> ResetPasswordAsync(PasswordResetModel request);
        Task<SprocMessage> ForgotPasswordAsync(PasswordResetModel reset);
        /// <summary>
        /// Updates the account secret key.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="accountsecretkey">The accountsecretkey.</param>
        void UpdateAccountSecretKey(string email, string accountsecretkey);
        Task<TokenVerification> GetOtpByUsernameAsync(string UserName);
        Task<SprocMessage> AddLoginOtpAsync(TokenVerification tokenVerification);
        void UpdateEmailConfirm(string UserName);
        Task<AppUser> GetUserByEmail(string Email);
    }
}
