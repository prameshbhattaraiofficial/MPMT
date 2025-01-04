using Mpmt.Core.Dtos.Module;
using Mpmt.Core.ViewModel.Module;
using Mpmt.Data.Repositories.Module;
using Mpmts.Core.Dtos;

namespace Mpmt.Services.Services.Module
{
    /// <summary>
    /// The module service.
    /// </summary>
    public class ModuleService : IModuleService
    {
        private readonly IModuleRepository _moduleRepository;
        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleService"/> class.
        /// </summary>
        /// <param name="menuRepository">The menu repository.</param>
        public ModuleService(IModuleRepository menuRepository)
        {
            _moduleRepository = menuRepository;
        }

        /// <summary>
        /// Adds the module async.
        /// </summary>
        /// <param name="module">The module.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> AddModuleAsync(IUDModule module)
        {

            var response = await _moduleRepository.AddModuleAsync(module);
            return response;
        }



        /// <summary>
        /// Gets the module async.
        /// </summary>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<ModuleViewModel>> GetModuleAsync()
        {
            var response = await _moduleRepository.GetModuleAsync();
            return response;
        }



        /// <summary>
        /// Gets the module by id async.
        /// </summary>
        /// <param name="ModuleId">The module id.</param>
        /// <returns>A Task.</returns>
        public async Task<IUDModule> GetModuleByIdAsync(int ModuleId)
        {
            var response = await _moduleRepository.GetModuleByIdAsync(ModuleId);
            return response;
        }



        /// <summary>
        /// Removes the module async.
        /// </summary>
        /// <param name="module">The module.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> RemoveModuleAsync(IUDModule module)
        {

            var response = await _moduleRepository.RemoveModuleAsync(module);
            return response;
        }



        /// <summary>
        /// Updates the module async.
        /// </summary>
        /// <param name="module">The module.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> UpdateModuleAsync(IUDModule module)
        {

            var response = await _moduleRepository.UpdateModuleAsync(module);
            return response;
        }
        /// <summary>
        /// Updates the module display order async.
        /// </summary>
        /// <param name="module">The module.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> UpdateModuleDisplayOrderAsync(IUDModule module)
        {

            var response = await _moduleRepository.UpdateModuleDisplayOrderAsync(module);
            return response;
        }
        /// <summary>
        /// Updates the module is active async.
        /// </summary>
        /// <param name="module">The module.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> UpdateModuleIsActiveAsync(IUDModule module)
        {

            var response = await _moduleRepository.UpdateModuleIsActiveAsync(module);
            return response;
        }
    }
}
