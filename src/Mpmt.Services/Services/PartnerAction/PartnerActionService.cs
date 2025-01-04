using Mpmt.Core.Dtos.PartnerAction;
using Mpmt.Core.ViewModel.PartnerAction;
using Mpmt.Data.Repositories.PartnerAction;
using Mpmts.Core.Dtos;

namespace Mpmt.Services.Services.PartnerAction
{
    public class PartnerActionService : IPartnerActionService
    {
        private readonly IPartnerActionRepository _partneractionRepo;
        /// <summary>
        /// Initializes a new instance of the <see cref="ActionService"/> class.
        /// </summary>
        /// <param name="actionRepo">The action repo.</param>
        public PartnerActionService(IPartnerActionRepository partneractionRepo)
        {
            _partneractionRepo = partneractionRepo;
        }

        /// <summary>
        /// Adds the action async.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> AddActionAsync(IUDPartnerAction action)
        {
            ;
            var response = await _partneractionRepo.AddActionAsync(action);
            return response;
        }


        /// <summary>
        /// Gets the action async.
        /// </summary>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<PartnerActionModelView>> GetActionAsync()
        {
            var response = await _partneractionRepo.GetActionAsync();
            return response;
        }


        /// <summary>
        /// Gets the action by id async.
        /// </summary>
        /// <param name="ActionId">The action id.</param>
        /// <returns>A Task.</returns>
        public async Task<IUDPartnerAction> GetActionByIdAsync(int ActionId)
        {
            var response = await _partneractionRepo.GetActionByIdAsync(ActionId);
            return response;
        }



        /// <summary>
        /// Removes the action async.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> RemoveActionAsync(IUDPartnerAction action)
        {

            var response = await _partneractionRepo.RemoveActionAsync(action);
            return response;
        }


        /// <summary>
        /// Updates the action async.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> UpdateActionAsync(IUDPartnerAction action)
        {

            var response = await _partneractionRepo.UpdateActionAsync(action);
            return response;
        }
    }
}
