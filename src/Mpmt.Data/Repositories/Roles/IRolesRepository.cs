using Mpmt.Core.Dtos.Roles;
using Mpmts.Core.Dtos;

namespace Mpmt.Data.Repositories.Roles
{
    /// <summary>
    /// The roles repository.
    /// </summary>
    public interface IRolesRepository
    {
        /// <summary>
        /// Adds the role async.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <returns>A Task.</returns>
        Task<SprocMessage> AddRoleAsync(AppRole role);
        /// <summary>
        /// Gets the role by name async.
        /// </summary>
        /// <param name="roleName">The role name.</param>
        /// <returns>A Task.</returns>
        Task<AppRole> GetRoleByNameAsync(string roleName);
        /// <summary>
        /// Gets the role by id async.
        /// </summary>
        /// <param name="roleId">The role id.</param>
        /// <returns>A Task.</returns>
        Task<AppRole> GetRoleByIdAsync(int roleId);
        /// <summary>
        /// Updates the role async.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <returns>A Task.</returns>
        Task<SprocMessage> UpdateRoleAsync(AppRole role);
        /// <summary>
        /// Removes the role async.
        /// </summary>
        /// <param name="roleid">The roleid.</param>
        /// <returns>A Task.</returns>
        Task<SprocMessage> RemoveRoleAsync(int roleid);
        /// <summary>
        /// Gets the role async.
        /// </summary>
        /// <returns>A Task.</returns>
        Task<IEnumerable<AppRole>> GetRoleAsync();
        /// <summary>
        /// Gets the admin role.
        /// </summary>
        /// <returns>A Task.</returns>
        Task<IEnumerable<ModuleActionClass>> GetAdminRole();
        /// <summary>
        /// Gets the menu by role id.
        /// </summary>
        /// <param name="roleid">The roleid.</param>
        /// <returns>A Task.</returns>
        Task<IEnumerable<MenuByRole>> GetMenuByRoleId(int roleid);
        /// <summary>
        /// Updates the menu to role.
        /// </summary>
        /// <param name="menuByRoles">The menu by roles.</param>
        /// <param name="roleId">The role id.</param>
        /// <returns>A Task.</returns>
        Task<SprocMessage> UpdateMenuToRole(List<MenuByRole> menuByRoles, int roleId);

    }
}
