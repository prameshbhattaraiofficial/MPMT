using Mpmt.Core.Dtos.PartnerModuleAction;
using Mpmt.Core.ViewModel.PartnerModuleAction;
using Mpmts.Core.Dtos;

namespace Mpmt.Data.Repositories.PartnerModuleAction
{
    public interface IPartnerModuleActionRepository
    {
        Task<SprocMessage> AddModuleActionAsync(IUDPartnerModuleAction moduleaction);
        Task<IEnumerable<PartnerModuleActionModelView>> GetModuleActionAsync();
        Task<IUDPartnerModuleAction> GetModuleActionByIdAsync(int ModuleActionId);
        Task<SprocMessage> RemoveModuleActionAsync(IUDPartnerModuleAction moduleaction);
        Task<SprocMessage> UpdateModuleActionAsync(IUDPartnerModuleAction moduleaction);
    }
}
