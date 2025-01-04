using Mpmt.Core.Dtos.Roles;
using Mpmt.Core.ViewModel.Role;
using Mpmts.Core.Dtos;

namespace Mpmt.Services.Services.Roles
{
    /// <summary>
    /// The role services.
    /// </summary>
    public interface IRoleServices
    {
        /// <summary>
        /// Gets the role async.
        /// </summary>
        /// <returns>A Task.</returns>
        Task<IEnumerable<AppRole>> GetRoleAsync();
        /// <summary>
        /// Gets the app role by id.
        /// </summary>
        /// <param name="Id">The id.</param>
        /// <returns>A Task.</returns>
        Task<AppRole> GetAppRoleById(int Id);
        /// <summary>
        /// Adds the role async.
        /// </summary>
        /// <param name="addRole">The add role.</param>
        /// <returns>A Task.</returns>
        Task<SprocMessage> AddRoleAsync(AddRoleVm addRole);
        /// <summary>
        /// Updates the role async.
        /// </summary>
        /// <param name="updateRole">The update role.</param>
        /// <returns>A Task.</returns>
        Task<SprocMessage> UpdateRoleAsync(UpdateRoleVm updateRole);
        /// <summary>
        /// Removes the role async.
        /// </summary>
        /// <param name="roleid">The roleid.</param>
        /// <returns>A Task.</returns>
        Task<SprocMessage> RemoveRoleAsync(int roleid);
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
