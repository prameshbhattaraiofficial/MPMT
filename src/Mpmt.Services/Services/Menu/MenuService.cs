using AutoMapper;
using Mpmt.Core.Dtos.Menu;
using Mpmt.Core.ViewModel.Menu;
using Mpmt.Data.Repositories.Menu;
using Mpmts.Core.Dtos;

namespace Mpmt.Services.Services.Menu
{
    /// <summary>
    /// The menu service.
    /// </summary>
    public class MenuService : IMenuService
    {
        private readonly IMenuRepository _menuRepository;
        private readonly IMapper _mapper;
        /// <summary>
        /// Initializes a new instance of the <see cref="MenuService"/> class.
        /// </summary>
        /// <param name="menuRepository">The menu repository.</param>
        /// <param name="mapper">The mapper.</param>
        public MenuService(IMenuRepository menuRepository, IMapper mapper)
        {
            _menuRepository = menuRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Adds the menu async.
        /// </summary>
        /// <param name="menu">The menu.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> AddMenuAsync(IUDMenu menu)
        {
            //var mappedData = _mapper.Map<IUDOccupation>(addOccupation);
            var response = await _menuRepository.AddMenuAsync(menu);
            return response;
        }
        public async Task<SprocMessage> AddPartnerMenuAsync(IUDPartnerMenu menu)
        {
            //var mappedData = _mapper.Map<IUDOccupation>(addOccupation);
            var response = await _menuRepository.AddPartnerMenuAsync(menu);
            return response;
        }


        /// <summary>
        /// Gets the menu async.
        /// </summary>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<MenuModelView>> GetMenuAsync()
        {
            var response = await _menuRepository.GetMenuAsync();
            return response;
        }

        public async Task<IEnumerable<PartnerMenu>> GetPartnerMenuAsync()
        {
            var response = await _menuRepository.GetPartnerMenuAsync();
            return response;
        }
        public async Task<IEnumerable<PartnerMenuWithPermission>> GetPartnerMenuByUserNameAsync(string UserName)
        {
            var response = await _menuRepository.GetPartnerMenuByUserNameAsync(UserName);
            return response;
        }


        /// <summary>
        /// Gets the occupation by id async.
        /// </summary>
        /// <param name="MenuId">The menu id.</param>
        /// <returns>A Task.</returns>
        public async Task<IUDMenu> GetMenuByIdAsync(int MenuId)
        {
            var response = await _menuRepository.GetMenuByIdAsync(MenuId);
            return response;
        }
         public async Task<IUDPartnerMenu> GetPartnerMenuByIdAsync(int MenuId)
        {
            var response = await _menuRepository.GetPartnerMenuByIdAsync(MenuId);
            return response;
        }


        /// <summary>
        /// Removes the menu async.
        /// </summary>
        /// <param name="menu">The menu.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> RemoveMenuAsync(IUDMenu menu)
        {

            var response = await _menuRepository.RemoveMenuAsync(menu);
            return response;
        }
        public async Task<SprocMessage> RemovePartnerMenuAsync(IUDPartnerMenu menu)
        {

            var response = await _menuRepository.RemovePartnerMenuAsync(menu);
            return response;
        }


        /// <summary>
        /// Updates the menu async.
        /// </summary>
        /// <param name="menu">The menu.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> UpdateMenuAsync(IUDMenu menu)
        {

            var response = await _menuRepository.UpdateMenuAsync(menu);
            return response;
        }
        public async Task<SprocMessage> UpdatePartnerMenuAsync(IUDPartnerMenu menu)
        {

            var response = await _menuRepository.UpdatePartnerMenuAsync(menu);
            return response;
        }
        /// <summary>
        /// Updates the menu display order async.
        /// </summary>
        /// <param name="menu">The menu.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> UpdateMenuDisplayOrderAsync(IUDMenu menu)
        {

            var response = await _menuRepository.UpdateMenuDisplayOrderAsync(menu);
            return response;
        }
      
        public async Task<SprocMessage> UpdateMenuIsActiveAsync(IUDMenu menu)
        {

            var response = await _menuRepository.UpdateMenuIsActiveAsync(menu);
            return response;
        }
        public async Task<SprocMessage> UpdatePartnerMenuDisplayOrderAsync(IUDMenu menu)
        {

            var response = await _menuRepository.UpdatePartnerMenuDisplayOrderAsync(menu);
            return response;
        }
      
        public async Task<SprocMessage> UpdatePartnerMenuIsActiveAsync(IUDMenu menu)
        {

            var response = await _menuRepository.UpdatePartnerMenuIsActiveAsync(menu);
            return response;
        }
    }
}
