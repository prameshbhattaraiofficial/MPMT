using Mpmt.Core.Dtos.PartnerAction;
using Mpmt.Core.ViewModel.PartnerAction;
using Mpmts.Core.Dtos;

namespace Mpmt.Data.Repositories.PartnerAction
{
    public interface IPartnerActionRepository
    {
        /// <summary>
        /// Adds the action async.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <returns>A Task.</returns>
        Task<SprocMessage> AddActionAsync(IUDPartnerAction action);
        /// <summary>
        /// Gets the action async.
        /// </summary>
        /// <returns>A Task.</returns>
        Task<IEnumerable<PartnerActionModelView>> GetActionAsync();
        /// <summary>
        /// Gets the action by id async.
        /// </summary>
        /// <param name="ActionId">The action id.</param>
        /// <returns>A Task.</returns>
        Task<IUDPartnerAction> GetActionByIdAsync(int ActionId);
        /// <summary>
        /// Removes the action async.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <returns>A Task.</returns>
        Task<SprocMessage> RemoveActionAsync(IUDPartnerAction action);
        /// <summary>
        /// Updates the action async.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <returns>A Task.</returns>
        Task<SprocMessage> UpdateActionAsync(IUDPartnerAction action);
    }
}
