using Mpmt.Core.Dtos.Partner;
using Mpmt.Core.Models.RMP;
using Mpmt.Core.Models.Route;
using Mpmts.Core.Dtos;

namespace Mpmt.Services.Services.RoleMenuPermission;

public interface IRMPService
{
    Task<IEnumerable<GetcontrollerAction>> GetcontrollerActionAsync(int roleId);

    Task<IEnumerable<GetcontrollerAction>> GetPartnerListcontrollerAction(int roleId);
    Task<IEnumerable<GetcontrollerAction>> GetPartnerMenuListControllerAction(int roleId);  

    Task<SprocMessage> AddPartnermenuPermission(AddcontrollerAction test);
    Task<SprocMessage> PartnerMenuPermission(AddcontrollerAction test);

   

    Task<IEnumerable<ActionPermission>> GetActionPermissionListAsync(string controller);

    Task<IEnumerable<ActionPermission>> GetPartnerActionPermissionList(string controller);

    Task<IEnumerable<ActionPermission>> GetAgentActionPermissionList(string controller);

    Task<IEnumerable<MenuSubMenu>> GetMenusSubmenusForCurrentUserAsync(string UserName);
}