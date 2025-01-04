using Mpmt.Core.Dtos.PartnerModule;
using Mpmt.Core.ViewModel.PartnerModule;
using Mpmt.Data.Repositories.Module;
using Mpmts.Core.Dtos;

namespace Mpmt.Services.Services.PartnerModule
{
    public class PartnerModuleService : IPartnerModuleService
    {
        private readonly IPartnerModuleRepository _partnermoduleRepository;
        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleService"/> class.
        /// </summary>
        /// <param name="menuRepository">The menu repository.</param>
        public PartnerModuleService(IPartnerModuleRepository partnermoduleRepository)
        {
            _partnermoduleRepository = partnermoduleRepository;
        }

        /// <summary>
        /// Adds the module async.
        /// </summary>
        /// <param name="module">The module.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> AddModuleAsync(IUDPartnerModule module)
        {

            var response = await _partnermoduleRepository.AddModuleAsync(module);
            return response;
        }



        /// <summary>
        /// Gets the module async.
        /// </summary>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<PartnerModuleModelView>> GetModuleAsync()
        {
            var response = await _partnermoduleRepository.GetModuleAsync();
            return response;
        }



        /// <summary>
        /// Gets the module by id async.
        /// </summary>
        /// <param name="ModuleId">The module id.</param>
        /// <returns>A Task.</returns>
        public async Task<IUDPartnerModule> GetModuleByIdAsync(int ModuleId)
        {
            var response = await _partnermoduleRepository.GetModuleByIdAsync(ModuleId);
            return response;
        }



        /// <summary>
        /// Removes the module async.
        /// </summary>
        /// <param name="module">The module.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> RemoveModuleAsync(IUDPartnerModule module)
        {

            var response = await _partnermoduleRepository.RemoveModuleAsync(module);
            return response;
        }



        /// <summary>
        /// Updates the module async.
        /// </summary>
        /// <param name="module">The module.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> UpdateModuleAsync(IUDPartnerModule module)
        {

            var response = await _partnermoduleRepository.UpdateModuleAsync(module);
            return response;
        }
        /// <summary>
        /// Updates the module display order async.
        /// </summary>
        /// <param name="module">The module.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> UpdateModuleDisplayOrderAsync(IUDPartnerModule module)
        {

            var response = await _partnermoduleRepository.UpdateModuleDisplayOrderAsync(module);
            return response;
        }
        /// <summary>
        /// Updates the module is active async.
        /// </summary>
        /// <param name="module">The module.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> UpdateModuleIsActiveAsync(IUDPartnerModule module)
        {

            var response = await _partnermoduleRepository.UpdateModuleIsActiveAsync(module);
            return response;
        }
    }
}
