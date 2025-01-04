using Mpmt.Core.Dtos.RoleModuleAction;
using Mpmt.Core.ViewModel.RoleModuleAction;
using Mpmts.Core.Dtos;

namespace Mpmt.Services.Services.RoleModuleAction
{
    /// <summary>
    /// The role module action service.
    /// </summary>
    public interface IRoleModuleActionService
    {
        /// <summary>
        /// Adds the role module action async.
        /// </summary>
        /// <param name="rolemoduleaction">The rolemoduleaction.</param>
        /// <returns>A Task.</returns>
        Task<SprocMessage> AddRoleModuleActionAsync(IUDRoleModuleAction rolemoduleaction);
        /// <summary>
        /// Gets the role module action async.
        /// </summary>
        /// <returns>A Task.</returns>
        Task<IEnumerable<RoleModuleActionModelView>> GetRoleModuleActionAsync();
        /// <summary>
        /// Gets the role module by id async.
        /// </summary>
        /// <param name="RoleModuleActionId">The role module action id.</param>
        /// <returns>A Task.</returns>
        Task<IUDRoleModuleAction> GetRoleModuleByIdAsync(int RoleModuleActionId);
        /// <summary>
        /// Removes the role module action async.
        /// </summary>
        /// <param name="rolemoduleaction">The rolemoduleaction.</param>
        /// <returns>A Task.</returns>
        Task<SprocMessage> RemoveRoleModuleActionAsync(IUDRoleModuleAction rolemoduleaction);
        /// <summary>
        /// Updates the role module action async.
        /// </summary>
        /// <param name="rolemoduleaction">The rolemoduleaction.</param>
        /// <returns>A Task.</returns>
        Task<SprocMessage> UpdateRoleModuleActionAsync(IUDRoleModuleAction rolemoduleaction);


    }
}
