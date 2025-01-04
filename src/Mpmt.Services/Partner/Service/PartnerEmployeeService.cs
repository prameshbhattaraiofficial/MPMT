using Mpmt.Core.Dtos.DropDown;
using Mpmt.Core.Dtos.PartnerEmployee;
using Mpmt.Core.Dtos.Roles;
using Mpmt.Data.Common;
using Mpmt.Data.Repositories.Common;
using Mpmt.Data.Repositories.PartnerEmployee;
using Mpmt.Services.Partner.IService;
using Mpmt.Services.Services.Common;
using Mpmts.Core.Dtos;

namespace Mpmt.Services.Partner.Service;

public class PartnerEmployeeService : BaseService, IPartnerEmployeeService
{
    private readonly IPartnerEmployeeRepo _partnerEmployeeRepo;
    private readonly ICommonddlRepo _commonddlRepo;

    public PartnerEmployeeService(IPartnerEmployeeRepo partnerEmployeeRepo , ICommonddlRepo commonddlRepo)
    {
        _partnerEmployeeRepo = partnerEmployeeRepo;
        _commonddlRepo = commonddlRepo;
    }

    public async Task<SprocMessage> AddPartnerEmployeeAsync(IUDPartnerEmployee partnerEmployee)
    {
        var passwordSalt = CryptoUtils.GenerateKeySalt();
        var passwordHash = CryptoUtils.HashHmacSha512Base64(partnerEmployee.Password, passwordSalt);
        partnerEmployee.Event = "I";
        partnerEmployee.PasswordHash = passwordHash;
        partnerEmployee.PasswordSalt = passwordSalt;
        var response = await _partnerEmployeeRepo.IUDPartnerEmployeeAsync(partnerEmployee);
        return response;
    }

    public async Task<SprocMessage> DeletePartnerEmployeeAsync(IUDPartnerEmployee partnerEmployee)
    {
        partnerEmployee.Event = "D";
        var response = await _partnerEmployeeRepo.IUDPartnerEmployeeAsync(partnerEmployee);
        return response;
    }

    public async Task<IEnumerable<PartnerEmployeeList>> GetPartnerEmployeeAsync(string code)
    {
        var response = await _partnerEmployeeRepo.GetPartnerEmployeeAsync(code);
        return response;
    }

    public async Task<PartnerEmployeeList> GetPartnerEmployeeByIdAsync(int id, string code)
    {
        var response = await _partnerEmployeeRepo.GetPartnerEmployeeByIdAsync(id, code);
        return response;
    }
    public async Task<IEnumerable<Commonddl>> GetPartnerEmployeeRolesByIdAsync(int id, string code)
    {
        var response = await _partnerEmployeeRepo.GetPartnerEmployeeRolesByIdAsync(id, code);
        return response;
    }
    public async Task<SprocMessage> AssignUserRole(string PartnerId, int user_id, int[] roleids)
    {
        var response = await _partnerEmployeeRepo.AssignUserRoleAsync(PartnerId,user_id, roleids);
        return response;
    }
    public async Task<IEnumerable<Commonddl>> GetPartnerEmployeeRolesAsync(string code = "")
    {
        var response = await _commonddlRepo.GetPartnerRoleddl(code);
        return response;
    }
   
    public async Task<SprocMessage> UpdatePartnerEmployeeAsync(IUDPartnerEmployee partnerEmployee)
    {
        partnerEmployee.Event = "U";
        var response = await _partnerEmployeeRepo.IUDPartnerEmployeeAsync(partnerEmployee);
        return response;
    }
}
