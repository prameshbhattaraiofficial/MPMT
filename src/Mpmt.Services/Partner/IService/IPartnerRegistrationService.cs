using Mpmt.Core.Domain;
using Mpmt.Core.Dtos.Partner;
using Mpmt.Core.Dtos.PartnerEmployee;
using Mpmt.Services.Users;
using Mpmts.Core.Dtos;

namespace Mpmt.Services.Partner
{
    /// <summary>
    /// The partner registration service.
    /// </summary>
    public interface IPartnerRegistrationService
    {
        /// <summary>
        /// Changes the password async.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>A Task.</returns>
        Task<ChangePasswordResult> ChangePasswordAsync(ChangePasswordRequest request);
        /// <summary>
        /// Validates the partner async.
        /// </summary>
        /// <param name="usernameOrEmail">The username or email.</param>
        /// <param name="password">The password.</param>
        /// <returns>A Task.</returns>
        Task<(LoginResults, AppPartner)> ValidatePartnerAsync(string usernameOrEmail, string password);
        Task<(LoginResults, AppPartner)> ValidatePartnerEmailAsync(string email);
        Task<(LoginResults, AppPartnerEmployee)> ValidatePartnerEmployeeAsync(string usernameOrEmail, string password);
        Task<(LoginResults, AppPartnerEmployee)> ValidatePartnerEmployeeEmailAsync(string email);

        /// <summary>
        /// Updates the account secret key.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="Accountsecretkey">The accountsecretkey.</param>
        void UpdateAccountSecretKey(string email, string Accountsecretkey);

        Task<SprocMessage> AddLoginOtpAsync(TokenVerification tokenVerification);

        Task<TokenVerification> GetOtpBypartnerCodeAsync(string partnercode ,string UserName, string OtpVerificationFor);
        void UpdateEmailConfirm(string partnercode);
        Task<string> CheckPartnerOrEmployee(string usernameOrEmail);
          
            

    }
}
