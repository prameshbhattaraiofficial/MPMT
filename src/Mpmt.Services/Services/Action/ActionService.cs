using Mpmt.Core.Dtos.Action;
using Mpmt.Core.ViewModel.Action;
using Mpmt.Data.Repositories.Action;
using Mpmts.Core.Dtos;

namespace Mpmt.Services.Services.Action
{
    /// <summary>
    /// The action service.
    /// </summary>
    public class ActionService : IActionService
    {
        private readonly IActionRepository _actionRepo;
        /// <summary>
        /// Initializes a new instance of the <see cref="ActionService"/> class.
        /// </summary>
        /// <param name="actionRepo">The action repo.</param>
        public ActionService(IActionRepository actionRepo)
        {
            _actionRepo = actionRepo;
        }

        /// <summary>
        /// Adds the action async.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> AddActionAsync(IUDAction action)
        {
            ;
            var response = await _actionRepo.AddActionAsync(action);
            return response;
        }


        /// <summary>
        /// Gets the action async.
        /// </summary>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<ActionModelView>> GetActionAsync()
        {
            var response = await _actionRepo.GetActionAsync();
            return response;
        }


        /// <summary>
        /// Gets the action by id async.
        /// </summary>
        /// <param name="ActionId">The action id.</param>
        /// <returns>A Task.</returns>
        public async Task<IUDAction> GetActionByIdAsync(int ActionId)
        {
            var response = await _actionRepo.GetActionByIdAsync(ActionId);
            return response;
        }



        /// <summary>
        /// Removes the action async.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> RemoveActionAsync(IUDAction action)
        {

            var response = await _actionRepo.RemoveActionAsync(action);
            return response;
        }


        /// <summary>
        /// Updates the action async.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> UpdateActionAsync(IUDAction action)
        {

            var response = await _actionRepo.UpdateActionAsync(action);
            return response;
        }
    }
}
