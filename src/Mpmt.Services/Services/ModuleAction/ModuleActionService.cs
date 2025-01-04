using Mpmt.Core.Dtos.ModuleAction;
using Mpmt.Core.ViewModel.ModuleAction;
using Mpmt.Data.Repositories.ModuleAction;
using Mpmts.Core.Dtos;

namespace Mpmt.Services.Services.ModuleAction
{
    /// <summary>
    /// The module action service.
    /// </summary>
    public class ModuleActionService : IModuleActionService
    {
        private readonly IModuleActionRepository _moduleactionRepository;
        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleActionService"/> class.
        /// </summary>
        /// <param name="moduleActionRepository">The module action repository.</param>
        public ModuleActionService(IModuleActionRepository moduleActionRepository)
        {
            _moduleactionRepository = moduleActionRepository;
        }
        /// <summary>
        /// Adds the module action async.
        /// </summary>
        /// <param name="moduleaction">The moduleaction.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> AddModuleActionAsync(IUDModuleAction moduleaction)
        {

            var response = await _moduleactionRepository.AddModuleActionAsync(moduleaction);
            return response;
        }




        /// <summary>
        /// Gets the module action async.
        /// </summary>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<ModuleActionModelView>> GetModuleActionAsync()
        {
            var response = await _moduleactionRepository.GetModuleActionAsync();
            return response;
        }




        /// <summary>
        /// Gets the module by id async.
        /// </summary>
        /// <param name="ModuleActionId">The module action id.</param>
        /// <returns>A Task.</returns>
        public async Task<IUDModuleAction> GetModuleByIdAsync(int ModuleActionId)
        {
            var response = await _moduleactionRepository.GetModuleActionByIdAsync(ModuleActionId);
            return response;
        }




        /// <summary>
        /// Removes the module action async.
        /// </summary>
        /// <param name="moduleaction">The moduleaction.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> RemoveModuleActionAsync(IUDModuleAction moduleaction)
        {

            var response = await _moduleactionRepository.RemoveModuleActionAsync(moduleaction);
            return response;
        }




        /// <summary>
        /// Updates the module action async.
        /// </summary>
        /// <param name="moduleaction">The moduleaction.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> UpdateModuleActionAsync(IUDModuleAction moduleaction)
        {

            var response = await _moduleactionRepository.UpdateModuleActionAsync(moduleaction);
            return response;
        }
    }
}
