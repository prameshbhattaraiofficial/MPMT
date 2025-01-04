using Mpmt.Core.Dtos.Partner;
using Mpmt.Core.Dtos.Users;
using Mpmt.Core.ViewModel.ForgotPassword;
using Mpmts.Core.Dtos;

namespace Mpmt.Data.Repositories.Users
{
    /// <summary>
    /// The users repository.
    /// </summary>
    public interface IUsersRepository
    {
   
        Task<SprocMessage> AddUserAsync(AppUser user);
        Task<SprocMessage> ChangePasswordAsync(AppUser user);   
        Task<SprocMessage> ForgotPasswordAsync(PasswordResetModel model);
        Task<(SprocMessage, PasswordResetModel)> RequestTokenValidationAsync(string token);
        Task<(SprocMessage, PasswordResetModel)> ResetTokenValidationAsync(PasswordResetModel resetModel);

        Task<bool> CheckUserExistsByEmailAsync(string email);
       
        Task<bool> CheckUserExistsByUserNameAsync(string userName);
        
        Task<AppUser> GetUserByIdAsync(int id);
       
        Task<AppUser> GetUserByUserNameAsync(string userName);
       
        Task<AppUser> GetUserByEmailAsync(string email);
        
        Task UpdateUserLoginActivityAsync(UserLoginActivity loginActivity);

        void UpdateAccountSecretKey(string email, string accountsecretkey);
        void UpdateIs2FAAuthenticated(string email, bool Is2FAAuthenticated);
        Task<SprocMessage> AddLoginOtpAsync(TokenVerification tokenVerification);
        Task<TokenVerification> GetOtpByUsernameAsync(string UserName);
        void UpdateEmailConfirmAsync(string UserName);
    }
}
