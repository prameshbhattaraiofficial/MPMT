using Mpmt.Core.Dtos.PartnerModule;
using Mpmt.Core.ViewModel.PartnerModule;
using Mpmts.Core.Dtos;

namespace Mpmt.Data.Repositories.Module
{
    /// <summary>
    /// The module repository.
    /// </summary>
    public interface IPartnerModuleRepository
    {
        /// <summary>
        /// Adds the module async.
        /// </summary>
        /// <param name="module">The module.</param>
        /// <returns>A Task.</returns>
        Task<SprocMessage> AddModuleAsync(IUDPartnerModule module);
        /// <summary>
        /// Gets the module async.
        /// </summary>
        /// <returns>A Task.</returns>
        Task<IEnumerable<PartnerModuleModelView>> GetModuleAsync();
        /// <summary>
        /// Gets the module by id async.
        /// </summary>
        /// <param name="ModuleId">The module id.</param>
        /// <returns>A Task.</returns>
        Task<IUDPartnerModule> GetModuleByIdAsync(int ModuleId);
        /// <summary>
        /// Removes the module async.
        /// </summary>
        /// <param name="module">The module.</param>
        /// <returns>A Task.</returns>
        Task<SprocMessage> RemoveModuleAsync(IUDPartnerModule module);
        /// <summary>
        /// Updates the module async.
        /// </summary>
        /// <param name="module">The module.</param>
        /// <returns>A Task.</returns>
        Task<SprocMessage> UpdateModuleAsync(IUDPartnerModule module);
        /// <summary>
        /// Updates the module display order async.
        /// </summary>
        /// <param name="moduleUpdate">The module update.</param>
        /// <returns>A Task.</returns>
        Task<SprocMessage> UpdateModuleDisplayOrderAsync(IUDPartnerModule moduleUpdate);
        /// <summary>
        /// Updates the module is active async.
        /// </summary>
        /// <param name="moduleUpdate">The module update.</param>
        /// <returns>A Task.</returns>
        Task<SprocMessage> UpdateModuleIsActiveAsync(IUDPartnerModule moduleUpdate);
    }

}
