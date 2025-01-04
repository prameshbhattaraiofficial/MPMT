using Mpmt.Core.Dtos.Roles;
using Mpmt.Core.Models.Route;
using Mpmts.Core.Dtos;

namespace Mpmt.Data.Repositories.Roles;

public interface IAgentRolesRepository
{
    Task<SprocMessage> AddRoleAsync(AppRole role);
    Task<IEnumerable<AppRole>> GetRoleAsync(string AgentCode);
    Task<AppRole> GetRoleByIdAsync(int roleId,string AgentCode);
    Task<SprocMessage> AssignRoletoUser(int user_id, int[] roleids);
    Task<SprocMessage> UpdateRoleAsync(AppRole role);
    Task<SprocMessage> RemoveRoleAsync(int roleid,string AgentCode);
    Task<IEnumerable<GetcontrollerAction>> GetListcontrollerActionAsync(int roleId, string area = "", string controller = "", string action = "");
    Task<SprocMessage> AddmenuPermission(AddcontrollerAction test);
    Task<bool> CheckPermission(string area, string controller, string action, string UserName);
}
