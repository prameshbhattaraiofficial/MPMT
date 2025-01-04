using Mpmt.Core.Dtos.PartnerModuleAction;
using Mpmt.Core.ViewModel.PartnerModuleAction;
using Mpmt.Data.Repositories.PartnerModuleAction;
using Mpmts.Core.Dtos;

namespace Mpmt.Services.Services.PartnerModuleAction
{
    public class PartnerModuleActionService : IPartnerModuleActionService
    {
        private readonly IPartnerModuleActionRepository _partnermoduleactionRepository;
        /// <summary>
        /// Initializes a new instance of the <see cref="PartnerModuleActionService"/> class.
        /// </summary>
        /// <param name="PartnermoduleActionRepository">The module action repository.</param>
        public PartnerModuleActionService(IPartnerModuleActionRepository partnermoduleactionRepository)
        {
            _partnermoduleactionRepository = partnermoduleactionRepository;
        }
        /// <summary>
        /// Adds the module action async.
        /// </summary>
        /// <param name="moduleaction">The moduleaction.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> AddModuleActionAsync(IUDPartnerModuleAction moduleaction)
        {

            var response = await _partnermoduleactionRepository.AddModuleActionAsync(moduleaction);
            return response;
        }




        /// <summary>
        /// Gets the module action async.
        /// </summary>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<PartnerModuleActionModelView>> GetModuleActionAsync()
        {
            var response = await _partnermoduleactionRepository.GetModuleActionAsync();
            return response;
        }




        /// <summary>
        /// Gets the module by id async.
        /// </summary>
        /// <param name="ModuleActionId">The module action id.</param>
        /// <returns>A Task.</returns>
        public async Task<IUDPartnerModuleAction> GetModuleByIdAsync(int ModuleActionId)
        {
            var response = await _partnermoduleactionRepository.GetModuleActionByIdAsync(ModuleActionId);
            return response;
        }




        /// <summary>
        /// Removes the module action async.
        /// </summary>
        /// <param name="moduleaction">The moduleaction.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> RemoveModuleActionAsync(IUDPartnerModuleAction moduleaction)
        {

            var response = await _partnermoduleactionRepository.RemoveModuleActionAsync(moduleaction);
            return response;
        }




        /// <summary>
        /// Updates the module action async.
        /// </summary>
        /// <param name="moduleaction">The moduleaction.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> UpdateModuleActionAsync(IUDPartnerModuleAction moduleaction)
        {

            var response = await _partnermoduleactionRepository.UpdateModuleActionAsync(moduleaction);
            return response;
        }
    }
}
