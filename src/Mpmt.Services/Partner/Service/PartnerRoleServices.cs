using AutoMapper;
using Mpmt.Core.Dtos.Partner;
using Mpmt.Core.Dtos.Roles;
using Mpmt.Core.ViewModel.Role;
using Mpmt.Data.Repositories.PartnerRoles;
using Mpmt.Services.Partner.IService;
using Mpmt.Services.Services.Common;
using Mpmts.Core.Dtos;
using System.Security.Claims;

namespace Mpmt.Services.Partner.Service
{
    public class PartnerRoleServices : BaseService, IPartnerRoleServices
    {
        private readonly IPartnerRolesRepository _roles;
        private readonly IMapper _mapper;

        public PartnerRoleServices(IPartnerRolesRepository roles, IMapper mapper)
        {
            _roles = roles;
            _mapper = mapper;
        }

        public async Task<SprocMessage> AddRoleAsync(AddPartnerRoleVm addRole, ClaimsPrincipal user)
        {
            addRole.Event = 'I';
            addRole.LoggedInUser = user?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
            addRole.UserType = user?.Claims.FirstOrDefault(x => x.Type == "UserType")?.Value;
            var partnerCode = user?.Claims.FirstOrDefault(x => x.Type == "PartnerCode")?.Value;
            var mPartnereddata = _mapper.Map<PartnerRole>(addRole);
            mPartnereddata.PartnerCode = partnerCode;
            var response = await _roles.AddRoleAsync(mPartnereddata);
            return response;
        }

        public async Task<PartnerRoleDetail> GetPartnerRoleById(int Id)
        {
            var Result = await _roles.GetRoleByIdAsync(Id);
            return Result;
        }

        public async Task<PartnerRole> GetPartnerRoleByName(string roleName)
        {
            var Result = await _roles.GetRoleByNameAsync(roleName);
            return Result;
        }

        public async Task<IEnumerable<PartnerRoleDetail>> GetRoleAsync(ClaimsPrincipal user)
        {
            var partnerCode = user?.Claims.FirstOrDefault(x => x.Type == "PartnerCode")?.Value;
            var Result = await _roles.GetRoleAsync(partnerCode);
            return Result;
        }

        public async Task<SprocMessage> RemoveRoleAsync(int roleid, ClaimsPrincipal user)
        {
            var loggedInUser = user?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
            var partnerCode = user?.Claims.FirstOrDefault(x => x.Type == "PartnerCode")?.Value;
            var Result = await _roles.RemoveRoleAsync(roleid, loggedInUser, partnerCode);
            return Result;
        }

        public async Task<SprocMessage> UpdateRoleAsync(UpdatePartnerRoleVm updateRole, ClaimsPrincipal user)
        {
            updateRole.Event = 'U';
            updateRole.LoggedInUser = user?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
            updateRole.UserType = user?.Claims.FirstOrDefault(x => x.Type == "UserType")?.Value;
            var partnerCode = user?.Claims.FirstOrDefault(x => x.Type == "PartnerCode")?.Value;
            var mPartnereddata = _mapper.Map<PartnerRole>(updateRole);
            mPartnereddata.PartnerCode = partnerCode;
            var response = await _roles.UpdateRoleAsync(mPartnereddata);
            return response;
        }

        public async Task<IEnumerable<AppPartner>> GetPartnerRole()
        {
            return await _roles.GetPartnerRole();
        }

        public async Task<SprocMessage> AddPartnerRoleAsync(AddAdminPartnerRoleVm addRole, ClaimsPrincipal user)
        {
            var adminPartnerRole = _mapper.Map<IUDPartnerRole>(addRole);
            adminPartnerRole.Event = 'I';
            adminPartnerRole.LoggedInUser = user?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
            adminPartnerRole.UserType = user?.Claims.FirstOrDefault(x => x.Type == "UserType")?.Value;
            var response = await _roles.IUDPartnerRoleAsync(adminPartnerRole);
            return response;
        }

        public async Task<IEnumerable<PartnerRoleList>> GetPartnerRoleAsync()
        {
            var Result = await _roles.GetPartnerRoleAsync();
            return Result;
        }

        public async Task<SprocMessage> UpdatePartnerRoleAsync(UpdateAdminPartnerRoleVm updateRole, ClaimsPrincipal user)
        {
            var adminPartnerRole = _mapper.Map<IUDPartnerRole>(updateRole);
            adminPartnerRole.Event = 'U';
            adminPartnerRole.LoggedInUser = user?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
            adminPartnerRole.UserType = user?.Claims.FirstOrDefault(x => x.Type == "UserType")?.Value;
            var response = await _roles.IUDPartnerRoleAsync(adminPartnerRole);
            return response;
        }

        public async Task<PartnerRoleList> GetAdminPartnerRoleById(int Id)
        {
            var result = await _roles.GetPartnerRoleByIdAsync(Id);
            return result;
        }

        public async Task<SprocMessage> DeletePartnerRoleAsync(UpdateAdminPartnerRoleVm updateRole, ClaimsPrincipal user)
        {
            var adminPartnerRole = _mapper.Map<IUDPartnerRole>(updateRole);
            adminPartnerRole.Event = 'D';
            adminPartnerRole.LoggedInUser = user?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
            adminPartnerRole.UserType = user?.Claims.FirstOrDefault(x => x.Type == "UserType")?.Value;
            var response = await _roles.IUDPartnerRoleAsync(adminPartnerRole);
            return response;
        }
    }
}