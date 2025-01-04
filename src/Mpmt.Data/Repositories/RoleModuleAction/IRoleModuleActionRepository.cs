using Mpmt.Core.Dtos.RoleModuleAction;
using Mpmt.Core.Models.Route;
using Mpmt.Core.ViewModel.AreaControllerAction;
using Mpmt.Core.ViewModel.RoleModuleAction;
using Mpmts.Core.Dtos;

namespace Mpmt.Data.Repositories.RoleModuleAction
{
    /// <summary>
    /// The role module action repository.
    /// </summary>
    public interface IRoleModuleActionRepository
    {
     
        Task<SprocMessage> AddRoleModuleActionAsync(IUDRoleModuleAction rolemoduleaction);
        Task<IEnumerable<RoleModuleActionModelView>> GetRoleModuleActionAsync();
        Task<IEnumerable<AreaControllerAction>> GetAreaControllerAction(); 
        Task<IUDRoleModuleAction> GetRoleModuleActionByIdAsync(int RoleModuleActionId);
        Task<SprocMessage> RemoveRoleModuleActionAsync(IUDRoleModuleAction rolemoduleaction);
        Task<SprocMessage> UpdateRoleModuleActionAsync(IUDRoleModuleAction rolemoduleaction);
        Task<SprocMessage> InsertControllerActionAsync(Controlleraction controlleraction);
        Task<SprocMessage> InsertControllerActionAgentAsync(Controlleraction controlleraction);
        Task<IEnumerable<AreaControllerAction>> GetAreaControllerActionAgent();

    }
}
