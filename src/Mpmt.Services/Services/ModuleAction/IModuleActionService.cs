using Mpmt.Core.Dtos.ModuleAction;
using Mpmt.Core.ViewModel.ModuleAction;
using Mpmts.Core.Dtos;

namespace Mpmt.Services.Services.ModuleAction
{
    /// <summary>
    /// The module action service.
    /// </summary>
    public interface IModuleActionService
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
        /// Gets the module by id async.
        /// </summary>
        /// <param name="ModuleActionId">The module action id.</param>
        /// <returns>A Task.</returns>
        Task<IUDModuleAction> GetModuleByIdAsync(int ModuleActionId);
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
