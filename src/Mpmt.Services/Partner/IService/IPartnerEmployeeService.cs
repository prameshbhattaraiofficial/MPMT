using Mpmt.Core.Dtos.DropDown;
using Mpmt.Core.Dtos.PartnerEmployee;
using Mpmts.Core.Dtos;

namespace Mpmt.Services.Partner.IService;

public interface IPartnerEmployeeService
{
    Task<IEnumerable<PartnerEmployeeList>> GetPartnerEmployeeAsync(string code);
    Task<PartnerEmployeeList> GetPartnerEmployeeByIdAsync(int id, string code);
    Task<IEnumerable<Commonddl>> GetPartnerEmployeeRolesAsync(string code = "");
  
    Task<IEnumerable<Commonddl>> GetPartnerEmployeeRolesByIdAsync(int id, string code);
    Task<SprocMessage> AssignUserRole( string PartnerId, int user_id, int[] roleids);
    Task<SprocMessage> AddPartnerEmployeeAsync(IUDPartnerEmployee partnerEmployee);
    Task<SprocMessage> UpdatePartnerEmployeeAsync(IUDPartnerEmployee partnerEmployee);
    Task<SprocMessage> DeletePartnerEmployeeAsync(IUDPartnerEmployee partnerEmployee);
}
