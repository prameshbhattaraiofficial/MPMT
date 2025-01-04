using Microsoft.AspNetCore.Mvc;
using Mpmt.Core.Dtos.Users;

namespace Mpmt.Services.Authentication
{
    /// <summary>
    /// The user cookie auth service.
    /// </summary>
    public interface IUserCookieAuthService
    {
        /// <summary>
        /// Signs the in async.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="returnUrl">The return url.</param>
        /// <param name="isPersistent">If true, is persistent.</param>
        /// <returns>A Task.</returns>
        Task<IActionResult> SignInAsync(AppUser user, string returnUrl, string Code, bool isPersistent = false);
        Task<bool> NormalAdminSignInAsync(AppUser user,string RoleName);
        /// <summary>
        /// Signs the out async.
        /// </summary>
        /// <returns>A Task.</returns>
        Task SignOutAsync();
        Task<bool> LoginLogout(AppUser user);
        /// <summary>
        /// Gets the authenticated user async.
        /// </summary>
        /// <returns>A Task.</returns>
        Task<AppUser> GetAuthenticatedUserAsync();
        /// <summary>
        /// Signs the in async.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="returnUrl">The return url.</param>
        /// <param name="isPersistent">If true, is persistent.</param>
        /// <returns>A Task.</returns>
       // Task<IActionResult> SignInPartnerAsync(AppPartner user, string returnUrl, bool isPersistent = false);
        /// <summary>
        /// Gets the authenticated partner async.
        /// </summary>
        /// <returns>A Task.</returns>
       // Task<AppPartner> GetAuthenticatedPartnerAsync();
        /// <summary>
        /// Signs the out partner async.
        /// </summary>
        /// <returns>A Task.</returns>
       // Task SignOutPartnerAsync();
    }
}
