using Mpmt.Core.Dtos.Roles;
using Mpmt.Core.Models.Route;
using Mpmt.Core.ViewModel.Role;
using Mpmts.Core.Dtos;

namespace Mpmt.Services.Services.Roles;

public interface IAgentRoleServices
{
    Task<IEnumerable<AppRole>> GetRoleAsync(string AgentCode);
    Task<SprocMessage> AddRoleAsync(AddRoleVm addRole);
    Task<AppRole> GetAppRoleById(int Id,string AgentCode);
    Task<SprocMessage> UpdateRoleAsync(UpdateRoleVm updateRole);
    Task<SprocMessage> RemoveRoleAsync(int roleid,string AgentCode);
    Task<SprocMessage> AssignUserRole(int user_id, int[] roleids);
    Task<IEnumerable<GetcontrollerAction>> GetListcontrollerActionAsync(int roleId, string area = "", string controller = "", string action = "");
    Task<SprocMessage> AddmenuPermission(AddcontrollerAction test);
    Task<bool> CheckPermission(string area, string controller, string action, string UserName);
}
