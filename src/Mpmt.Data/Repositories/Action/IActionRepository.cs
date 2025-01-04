using Mpmt.Core.Dtos.Action;
using Mpmt.Core.ViewModel.Action;
using Mpmts.Core.Dtos;

namespace Mpmt.Data.Repositories.Action
{
    /// <summary>
    /// The action repository.
    /// </summary>
    public interface IActionRepository
    {
        /// <summary>
        /// Adds the action async.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <returns>A Task.</returns>
        Task<SprocMessage> AddActionAsync(IUDAction action);
        /// <summary>
        /// Gets the action async.
        /// </summary>
        /// <returns>A Task.</returns>
        Task<IEnumerable<ActionModelView>> GetActionAsync();
        /// <summary>
        /// Gets the action by id async.
        /// </summary>
        /// <param name="ActionId">The action id.</param>
        /// <returns>A Task.</returns>
        Task<IUDAction> GetActionByIdAsync(int ActionId);
        /// <summary>
        /// Removes the action async.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <returns>A Task.</returns>
        Task<SprocMessage> RemoveActionAsync(IUDAction action);
        /// <summary>
        /// Updates the action async.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <returns>A Task.</returns>
        Task<SprocMessage> UpdateActionAsync(IUDAction action);
    }
}
