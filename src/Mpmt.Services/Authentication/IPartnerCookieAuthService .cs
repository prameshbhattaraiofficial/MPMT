using Microsoft.AspNetCore.Mvc;
using Mpmt.Core.Dtos.Partner;
using Mpmt.Core.Dtos.PartnerEmployee;

namespace Mpmt.Services.Authentication
{
    /// <summary>
    /// The user cookie auth service.
    /// </summary>
    public interface IPartnerCookieAuthService
    {
        /// <summary>
        /// Signs the in async.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="returnUrl">The return url.</param>
        /// <param name="isPersistent">If true, is persistent.</param>
        /// <returns>A Task.</returns>
        Task<IActionResult> SignInPartnerAsync(AppPartner user, string returnUrl, string Code, bool isPersistent = false);
        Task<bool> LoginLogout();
        Task<bool> partnerLoginLogout();
        Task<bool> NormalSignInPartnerAsync(AppPartner user,string roleName);
        Task<bool> NormalSignInPartnerEmployeeAsync(AppPartnerEmployee user, string RoleName);
        Task<IActionResult> SignInPartnerEmployeeAsync(AppPartnerEmployee user, string returnUrl, string Code, bool isPersistent = false);
        void UpdateEmployeeAccountSecretKey(string email, string Accountsecretkey);
        Task<TokenVerification> GetOtpBypartnerEmployeeCodeAsync(string partnercode, string UserName, string OtpVerificationFor);
        void UpdateEmployeeEmailConfirm(string partnercode,string UserName);
        /// <summary>
        /// Signs the out async.
        /// </summary>
        /// <returns>A Task.</returns>
        Task SignOutPartnerAsync();

        /// <summary>
        /// Gets the authenticated user async.
        /// </summary>
        /// <returns>A Task.</returns>
        Task<AppPartner> GetAuthenticatedPartnerAsync();
    }
}
