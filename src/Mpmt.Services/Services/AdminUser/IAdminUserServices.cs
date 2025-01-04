using Mpmt.Core.Dtos;
using Mpmt.Core.Dtos.AdminUser;
using Mpmt.Core.Dtos.Notification;
using Mpmt.Core.Dtos.Paging;
using Mpmt.Core.Dtos.Users;
using Mpmt.Core.ViewModel.AdminUser;
using Mpmts.Core.Dtos;
using System.Security.Claims;

namespace Mpmt.Services.Services.AdminUser
{
    /// <summary>
    /// The admin user services.
    /// </summary>
    public interface IAdminUserServices
    {
        /// <summary>
        /// Gets the admin user async.
        /// </summary>
        /// <param name="userFilter">The user filter.</param>
        /// <returns>A Task.</returns>
        Task<PagedList<AdminUserDetails>> GetAdminUserAsync(AdminUserFilter userFilter);
        Task<IUDAdminUser> GetAdminUserByIdAsync(int id);
        Task<MpmtResult> AddAdminUserAsync(AdminUserVm adminUser, ClaimsPrincipal claim);
        Task<MpmtResult> UpdateAdminUserAsync(AdminUserVm adminUser, ClaimsPrincipal claim);
        Task<SprocMessage> AssignUserRole(int user_id, int[] roleids);  
        Task<SprocMessage> AssignPartnerRole(int user_id, int[] roleids);
        Task<SprocMessage> DeleteAdminUserAsync(int id, string remarks);
        Task<bool> VerifyUserNameAdmin(string userName);
        Task<bool> VerifyEmailAdmin(string Email);
    }
}
