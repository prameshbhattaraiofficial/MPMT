using Mpmt.Core.Dtos.Module;
using Mpmt.Core.ViewModel.Module;
using Mpmts.Core.Dtos;

namespace Mpmt.Services.Services.Module
{
    /// <summary>
    /// The module service.
    /// </summary>
    public interface IModuleService
    {
        /// <summary>
        /// Adds the module async.
        /// </summary>
        /// <param name="module">The module.</param>
        /// <returns>A Task.</returns>
        Task<SprocMessage> AddModuleAsync(IUDModule module);
        /// <summary>
        /// Gets the module async.
        /// </summary>
        /// <returns>A Task.</returns>
        Task<IEnumerable<ModuleViewModel>> GetModuleAsync();
        /// <summary>
        /// Gets the module by id async.
        /// </summary>
        /// <param name="ModuleId">The module id.</param>
        /// <returns>A Task.</returns>
        Task<IUDModule> GetModuleByIdAsync(int ModuleId);
        /// <summary>
        /// Removes the module async.
        /// </summary>
        /// <param name="module">The module.</param>
        /// <returns>A Task.</returns>
        Task<SprocMessage> RemoveModuleAsync(IUDModule module);
        /// <summary>
        /// Updates the module async.
        /// </summary>
        /// <param name="module">The module.</param>
        /// <returns>A Task.</returns>
        Task<SprocMessage> UpdateModuleAsync(IUDModule module);
        /// <summary>
        /// Updates the module display order async.
        /// </summary>
        /// <param name="module">The module.</param>
        /// <returns>A Task.</returns>
        Task<SprocMessage> UpdateModuleDisplayOrderAsync(IUDModule module);
        /// <summary>
        /// Updates the module is active async.
        /// </summary>
        /// <param name="module">The module.</param>
        /// <returns>A Task.</returns>
        Task<SprocMessage> UpdateModuleIsActiveAsync(IUDModule module);
    }
}
