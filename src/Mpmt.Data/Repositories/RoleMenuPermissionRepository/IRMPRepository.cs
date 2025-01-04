using Mpmt.Core.Dtos.Partner;
using Mpmt.Core.Models.RMP;
using Mpmt.Core.Models.Route;
using Mpmts.Core.Dtos;

namespace Mpmt.Data.Repositories.RoleMenuPermissionRepository;

public interface IRMPRepository
{
    Task<IEnumerable<RMPermissionModel>> GetListWithSubMenusAsync(string userName);

    Task<SprocMessage> AddPartnermenuPermissionAsync(AddcontrollerAction test);
    Task<SprocMessage> PartnerMenuPermissionAsync(AddcontrollerAction test);

   
    Task<IEnumerable<GetcontrollerAction>> GetListcontrollerActionAsync(int roleId, string area = "", string controller = "", string action = "");

    Task<IEnumerable<GetcontrollerAction>> GetPartnerListcontrollerActionAsync(int roleId, string area = "", string controller = "", string action = "");
    Task<IEnumerable<GetcontrollerAction>> GetPartnerMenuListControllerAction(int roleId, string area = "", string controller = "", string action = "");

    Task<bool> CheckPermission(string area, string controller, string action, string UserName);

    Task<bool> CheckPartnerPermission(string area, string controller, string action, string UserName);
    Task<bool> CheckPartnerMenuPermission(string area, string controller, string action, string UserName);  

    Task<IEnumerable<ActionPermission>> GetActionPermissionList(string area, string controller, string UserName);

    Task<IEnumerable<ActionPermission>> GetPartnerActionPermissionList(string area, string controller, string UserName);

    Task<IEnumerable<ActionPermission>> GetAgentActionPermissionList(string controller, string UserName);

    Task<SprocMessage> AddmenuPermission(AddcontrollerAction test);
}