using Mpmt.Core.Dtos.Menu;
using Mpmt.Core.ViewModel.Menu;
using Mpmts.Core.Dtos;

namespace Mpmt.Data.Repositories.Menu
{
    /// <summary>
    /// The menu repository.
    /// </summary>
    public interface IMenuRepository
    {
        /// <summary>
        /// Adds the menu async.
        /// </summary>
        /// <param name="menu">The menu.</param>
        /// <returns>A Task.</returns>
        Task<SprocMessage> AddMenuAsync(IUDMenu menu);
        Task<SprocMessage> AddPartnerMenuAsync(IUDPartnerMenu menu);
        /// <summary>
        /// Gets the menu async.
        /// </summary>
        /// <returns>A Task.</returns>
        Task<IEnumerable<MenuModelView>> GetMenuAsync();
        Task<IEnumerable<PartnerMenu>> GetPartnerMenuAsync();
        Task<IEnumerable<PartnerMenuWithPermission>> GetPartnerMenuByUserNameAsync(string UserName);
        /// <summary>
        /// Gets the menu by id async.
        /// </summary>
        /// <param name="MenuId">The menu id.</param>
        /// <returns>A Task.</returns>
        Task<IUDMenu> GetMenuByIdAsync(int MenuId);
        Task<IUDPartnerMenu> GetPartnerMenuByIdAsync(int MenuId);
        /// <summary>
        /// Removes the menu async.
        /// </summary>
        /// <param name="menu">The menu.</param>
        /// <returns>A Task.</returns>
        Task<SprocMessage> RemoveMenuAsync(IUDMenu menu);
        Task<SprocMessage> RemovePartnerMenuAsync(IUDPartnerMenu menu);
        /// <summary>
        /// Updates the menu async.
        /// </summary>
        /// <param name="menu">The menu.</param>
        /// <returns>A Task.</returns>
        Task<SprocMessage> UpdateMenuAsync(IUDMenu menu);
        Task<SprocMessage> UpdatePartnerMenuAsync(IUDPartnerMenu menu);
        /// <summary>
        /// Updates the menu display order async.
        /// </summary>
        /// <param name="menuUpdate">The menu update.</param>
        /// <returns>A Task.</returns>
        Task<SprocMessage> UpdateMenuDisplayOrderAsync(IUDMenu menuUpdate);
        Task<SprocMessage> UpdateMenuIsActiveAsync(IUDMenu menuUpdate);
        Task<SprocMessage> UpdatePartnerMenuDisplayOrderAsync(IUDMenu menuUpdate);
        Task<SprocMessage> UpdatePartnerMenuIsActiveAsync(IUDMenu menuUpdate);
    }
}
