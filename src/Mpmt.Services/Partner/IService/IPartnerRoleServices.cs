using Mpmt.Core.Dtos.Partner;
using Mpmt.Core.Dtos.Roles;
using Mpmt.Core.ViewModel.Role;
using Mpmts.Core.Dtos;
using System.Security.Claims;

namespace Mpmt.Services.Partner.IService
{
    public interface IPartnerRoleServices
    {
        Task<IEnumerable<PartnerRoleDetail>> GetRoleAsync(ClaimsPrincipal user);
        Task<IEnumerable<PartnerRoleList>> GetPartnerRoleAsync();
        Task<PartnerRoleList> GetAdminPartnerRoleById(int Id);

        Task<PartnerRoleDetail> GetPartnerRoleById(int Id);

        Task<PartnerRole> GetPartnerRoleByName(string roleName);

        Task<SprocMessage> AddRoleAsync(AddPartnerRoleVm addRole, ClaimsPrincipal user);
        Task<SprocMessage> AddPartnerRoleAsync(AddAdminPartnerRoleVm addRole, ClaimsPrincipal user);
        Task<SprocMessage> UpdatePartnerRoleAsync(UpdateAdminPartnerRoleVm updateRole, ClaimsPrincipal user);   
        Task<SprocMessage> DeletePartnerRoleAsync(UpdateAdminPartnerRoleVm updateRole, ClaimsPrincipal user);   

        Task<SprocMessage> UpdateRoleAsync(UpdatePartnerRoleVm updateRole, ClaimsPrincipal user);

        Task<SprocMessage> RemoveRoleAsync(int roleid, ClaimsPrincipal user);

        Task<IEnumerable<AppPartner>> GetPartnerRole();
    }
}
