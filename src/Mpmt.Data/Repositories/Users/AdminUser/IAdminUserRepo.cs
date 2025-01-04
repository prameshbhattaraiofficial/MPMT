using Mpmt.Core.Dtos.AdminUser;
using Mpmt.Core.Dtos.IncomeSource;
using Mpmt.Core.Dtos.Notification;
using Mpmt.Core.Dtos.Paging;
using Mpmt.Core.Dtos.Users;
using Mpmts.Core.Dtos;

namespace Mpmt.Data.Repositories.Users.AdminUser
{
    /// <summary>
    /// The admin user repo.
    /// </summary>
    public interface IAdminUserRepo
    {
        /// <summary>
        /// Gets the admin user async.
        /// </summary>
        /// <param name="userFilter">The user filter.</param>
        /// <returns>A Task.</returns>
        Task<PagedList<AdminUserDetails>> GetAdminUserAsync(AdminUserFilter userFilter);
        Task<SprocMessage> IUDAdminUserAsync(IUDAdminUser adminUser);
        Task<SprocMessage> AssignRoletoUser(int user_id, int[] roleids);
        Task<SprocMessage> AssignPartnerRole(int user_id, int[] roleids);   
        Task<IUDAdminUser> GetAdminUserById(int id);
        Task<SprocMessage> DeleteAdminUserAsync(int id, string remarks);   
        Task<bool> VerifyUserNameAdminAsync(string userName);
        Task<bool> VerifyEmailAdminAsync(string Email);
    }
}
