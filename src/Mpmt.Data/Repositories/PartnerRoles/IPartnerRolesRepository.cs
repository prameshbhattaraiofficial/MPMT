using Mpmt.Core.Dtos.Partner;
using Mpmt.Core.Dtos.Roles;
using Mpmts.Core.Dtos;

namespace Mpmt.Data.Repositories.PartnerRoles
{
    public interface IPartnerRolesRepository
    {
        Task<SprocMessage> AddRoleAsync(PartnerRole role);
        Task<SprocMessage> AddPartnerRoleAsync(PartnerAdminRole role);  
        Task<SprocMessage> IUDPartnerRoleAsync(IUDPartnerRole role);

        Task<PartnerRole> GetRoleByNameAsync(string roleName);

        Task<PartnerRoleDetail> GetRoleByIdAsync(int roleId);

        Task<SprocMessage> UpdateRoleAsync(PartnerRole role);

        Task<SprocMessage> RemoveRoleAsync(int roleid, string loggedInUser, string PartnerCode);

        Task<IEnumerable<PartnerRoleDetail>> GetRoleAsync(string PartnerCode);
        Task<IEnumerable<PartnerRoleList>> GetPartnerRoleAsync();
        Task<PartnerRoleList> GetPartnerRoleByIdAsync(int id);  
        Task<PartnerRoleList> GetPartnerRoleByNameAsync(string roleName);   

        Task<IEnumerable<AppPartner>> GetPartnerRole();
    }
}
