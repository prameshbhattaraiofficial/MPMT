using Mpmt.Core.Dtos.CashAgent;
using Mpmt.Core.Models.Route;
using Mpmt.Core.ViewModel.CashAgent;
using Mpmts.Core.Dtos;

namespace Mpmt.Services.CashAgents;

public interface IAgentMenuService
{
    Task<SprocMessage> AddAgentMenuAsync(IUDAgentMenu menu);
    Task<IEnumerable<AgentMenuModel>> GetAgentMenuAsync();
    Task<IEnumerable<AgentMenuChild>> GetMenusSubmenusForCurrentUserByUserTypeAsync(string UserType);
    Task<SprocMessage> UpdateMenuDisplayOrderAsync(IUDAgentMenu menu);
    Task<SprocMessage> UpdateMenuIsActiveAsync(IUDAgentMenu menu);
    Task<IUDAgentMenu> GetAgentMenuByIdAsync(int MenuId);
    Task<SprocMessage> UpdateAgentMenuAsync(IUDAgentMenu menu);
    Task<SprocMessage> RemoveAgentMenuAsync(IUDAgentMenu menu);
    Task<IEnumerable<AgentMenuChild>> GetMenusSubmenusForCurrentUserByUser();
    Task<IEnumerable<GetcontrollerAction>> GetListcontrollerActionAsync(string UserType);
    Task<SprocMessage> AddmenuPermission(AddControllerActionUserType test);
}
