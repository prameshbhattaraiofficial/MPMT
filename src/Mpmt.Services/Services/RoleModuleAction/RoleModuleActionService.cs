using Mpmt.Core.Dtos.RoleModuleAction;
using Mpmt.Core.ViewModel.RoleModuleAction;
using Mpmt.Data.Repositories.RoleModuleAction;
using Mpmts.Core.Dtos;

namespace Mpmt.Services.Services.RoleModuleAction
{
    /// <summary>
    /// The role module action service.
    /// </summary>
    public class RoleModuleActionService : IRoleModuleActionService
    {
        private readonly IRoleModuleActionRepository _rolemoduleactionRepository;
        /// <summary>
        /// Initializes a new instance of the <see cref="RoleModuleActionService"/> class.
        /// </summary>
        /// <param name="roleModuleActionRepository">The role module action repository.</param>
        public RoleModuleActionService(IRoleModuleActionRepository roleModuleActionRepository)
        {
            _rolemoduleactionRepository = roleModuleActionRepository;
        }


        /// <summary>
        /// Adds the role module action async.
        /// </summary>
        /// <param name="rolemoduleaction">The rolemoduleaction.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> AddRoleModuleActionAsync(IUDRoleModuleAction rolemoduleaction)
        {

            var response = await _rolemoduleactionRepository.AddRoleModuleActionAsync(rolemoduleaction);
            return response;
        }





        /// <summary>
        /// Gets the role module action async.
        /// </summary>
        /// <returns>A Task.</returns>
        public async Task<IEnumerable<RoleModuleActionModelView>> GetRoleModuleActionAsync()
        {
            var response = await _rolemoduleactionRepository.GetRoleModuleActionAsync();
            return response;
        }





        /// <summary>
        /// Gets the role module by id async.
        /// </summary>
        /// <param name="RoleModuleActionId">The role module action id.</param>
        /// <returns>A Task.</returns>
        public async Task<IUDRoleModuleAction> GetRoleModuleByIdAsync(int RoleModuleActionId)
        {
            var response = await _rolemoduleactionRepository.GetRoleModuleActionByIdAsync(RoleModuleActionId);
            return response;
        }





        /// <summary>
        /// Removes the role module action async.
        /// </summary>
        /// <param name="rolemoduleaction">The rolemoduleaction.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> RemoveRoleModuleActionAsync(IUDRoleModuleAction rolemoduleaction)
        {

            var response = await _rolemoduleactionRepository.RemoveRoleModuleActionAsync(rolemoduleaction);
            return response;
        }





        /// <summary>
        /// Updates the role module action async.
        /// </summary>
        /// <param name="rolemoduleaction">The rolemoduleaction.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> UpdateRoleModuleActionAsync(IUDRoleModuleAction rolemoduleaction)
        {

            var response = await _rolemoduleactionRepository.UpdateRoleModuleActionAsync(rolemoduleaction);
            return response;
        }
    }
}
