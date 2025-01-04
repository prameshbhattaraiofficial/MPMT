using Mpmt.Core.Dtos.PartnerModuleAction;
using Mpmt.Core.ViewModel.PartnerModuleAction;
using Mpmts.Core.Dtos;

namespace Mpmt.Services.Services.PartnerModuleAction
{
    public interface IPartnerModuleActionService
    {
        Task<SprocMessage> AddModuleActionAsync(IUDPartnerModuleAction moduleaction);
         Task<IEnumerable<PartnerModuleActionModelView>> GetModuleActionAsync();
         Task<IUDPartnerModuleAction> GetModuleByIdAsync(int ModuleActionId);
        Task<SprocMessage> RemoveModuleActionAsync(IUDPartnerModuleAction moduleaction);
        Task<SprocMessage> UpdateModuleActionAsync(IUDPartnerModuleAction moduleaction);
    }
}
