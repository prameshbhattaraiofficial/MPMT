using Mpmt.Core.Dtos.Roles;
using Mpmts.Core.Dtos;

namespace Mpmt.Data.Repositories.Roles
{
    /// <summary>
    /// The user roles repository.
    /// </summary>
    public interface IUserRolesRepository
    {
        /// <summary>
        /// Adds the user to roles async.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="roleIds">The role ids.</param>
        /// <returns>A Task.</returns>
        Task<SprocMessage> AddUserToRolesAsync(int userId, params int[] roleIds);
        Task<IEnumerable<UserRoles>> GetRolesByUserIdAsync(int userId);
    }
}
