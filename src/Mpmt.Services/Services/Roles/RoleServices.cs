using AutoMapper;
using Mpmt.Core.Dtos.Roles;
using Mpmt.Core.ViewModel.Role;
using Mpmt.Data.Repositories.Roles;
using Mpmt.Services.Services.Common;
using Mpmts.Core.Dtos;

namespace Mpmt.Services.Services.Roles
{
    /// <summary>
    /// The role services.
    /// </summary>
    public class RoleServices : BaseService, IRoleServices
    {
        private readonly IRolesRepository _roles;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleServices"/> class.
        /// </summary>
        /// <param name="roles">The roles.</param>
        /// <param name="mapper">The mapper.</param>
        public RoleServices(IRolesRepository roles, IMapper mapper)
        {
            _roles = roles;
            _mapper = mapper;
        }

        /// <summary>
        /// Adds the role async.
        /// </summary>
        /// <param name="addRole">The add role.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> AddRoleAsync(AddRoleVm addRole)
        {
            var mappeddata = _mapper.Map<AppRole>(addRole);
            var response = await _roles.AddRoleAsync(mappeddata);
            //if (response.StatusCode != 200) return GetErrorResponseFromSprocMessage(response);
            //return (HttpStatusCode.OK, new ResponseDto { Success = true, Message = response.MsgText });
            return response;
        }

        /// <summary>
        /// Gets the app role by id.
        /// </summary>
        /// <param name="Id">The id.</param>
        /// <returns>A Task.</returns>
        public async Task<AppRole> GetAppRoleById(int Id)
        {
            var Result = await _roles.GetRoleByIdAsync(Id);
            return Result;
        }

        /// <summary>
        /// Gets the role async.
        /// </summary>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<AppRole>> GetRoleAsync()
        {
            var Result = await _roles.GetRoleAsync();
            return Result;
        }

        /// <summary>
        /// Removes the role async.
        /// </summary>
        /// <param name="roleid">The roleid.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> RemoveRoleAsync(int roleid)
        {
            var Result = await _roles.RemoveRoleAsync(roleid);
            return Result;
        }

        /// <summary>
        /// Updates the role async.
        /// </summary>
        /// <param name="updateRole">The update role.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> UpdateRoleAsync(UpdateRoleVm updateRole)
        {
            var mappeddata = _mapper.Map<AppRole>(updateRole);
            var response = await _roles.UpdateRoleAsync(mappeddata);
            //if (response.StatusCode != 200) return GetErrorResponseFromSprocMessage(response);
            //return (HttpStatusCode.OK, new ResponseDto { Success = true, Message = response.MsgText });
            return response;
        }
        /// <summary>
        /// Gets the admin role.
        /// </summary>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<ModuleActionClass>> GetAdminRole()
        {

            var response = await _roles.GetAdminRole();

            return response;
        }
        /// <summary>
        /// Gets the menu by role id.
        /// </summary>
        /// <param name="roleid">The roleid.</param>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<MenuByRole>> GetMenuByRoleId(int roleid)
        {

            var response = await _roles.GetMenuByRoleId(roleid);
            return response;
        }

        /// <summary>
        /// Updates the menu to role.
        /// </summary>
        /// <param name="menuByRoles">The menu by roles.</param>
        /// <param name="roleId">The role id.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> UpdateMenuToRole(List<MenuByRole> menuByRoles, int roleId)
        {

            var response = await _roles.UpdateMenuToRole(menuByRoles, roleId);
            return response;
        }
    }
}
