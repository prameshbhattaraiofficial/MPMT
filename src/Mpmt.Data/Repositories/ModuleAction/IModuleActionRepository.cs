using Mpmt.Core.Dtos.ModuleAction;
using Mpmt.Core.ViewModel.ModuleAction;
using Mpmts.Core.Dtos;

namespace Mpmt.Data.Repositories.ModuleAction
{
    /// <summary>
    /// The module action repository.
    /// </summary>
    public interface IModuleActionRepository
    {
        /// <summary>
        /// Adds the module action async.
        /// </summary>
        /// <param name="moduleaction">The moduleaction.</param>
        /// <returns>A Task.</returns>
        Task<SprocMessage> AddModuleActionAsync(IUDModuleAction moduleaction);
        /// <summary>
        /// Gets the module action async.
        /// </summary>
        /// <returns>A Task.</returns>
        Task<IEnumerable<ModuleActionModelView>> GetModuleActionAsync();
        /// <summary>
        /// Gets the module action by id async.
        /// </summary>
        /// <param name="ModuleActionId">The module action id.</param>
        /// <returns>A Task.</returns>
        Task<IUDModuleAction> GetModuleActionByIdAsync(int ModuleActionId);
        /// <summary>
        /// Removes the module action async.
        /// </summary>
        /// <param name="moduleaction">The moduleaction.</param>
        /// <returns>A Task.</returns>
        Task<SprocMessage> RemoveModuleActionAsync(IUDModuleAction moduleaction);
        /// <summary>
        /// Updates the module action async.
        /// </summary>
        /// <param name="moduleaction">The moduleaction.</param>
        /// <returns>A Task.</returns>
        Task<SprocMessage> UpdateModuleActionAsync(IUDModuleAction moduleaction);
    }
}
