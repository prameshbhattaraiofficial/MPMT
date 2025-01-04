using Mpmt.Core.Dtos.DropDown;
using Mpmt.Core.Dtos.PartnerEmployee;
using Mpmts.Core.Dtos;

namespace Mpmt.Data.Repositories.PartnerEmployee;

public interface IPartnerEmployeeRepo
{
    Task<IEnumerable<PartnerEmployeeList>> GetPartnerEmployeeAsync(string code);
    Task<PartnerEmployeeList> GetPartnerEmployeeByIdAsync(int id, string code);
    Task<IEnumerable<Commonddl>> GetPartnerEmployeeRolesByIdAsync(int id, string code);
    Task<SprocMessage> AssignUserRoleAsync(string PartnerId, int user_id, int[] roleids);
    Task<SprocMessage> IUDPartnerEmployeeAsync(IUDPartnerEmployee partnerEmployee);
    Task<SprocMessage> PartnerEmployeeChangePasswordAsync(IUDPartnerEmployee partnerEmployee);
}
