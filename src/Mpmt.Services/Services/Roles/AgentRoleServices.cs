using AutoMapper;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Http;
using Mpmt.Core.Dtos.CashAgent.Commission;
using Mpmt.Core.Dtos.Roles;
using Mpmt.Core.Models.Route;
using Mpmt.Core.ViewModel.Role;
using Mpmt.Data.Repositories.Roles;
using Mpmts.Core.Dtos;

namespace Mpmt.Services.Services.Roles;

public class AgentRoleServices : IAgentRoleServices
{
    private readonly IAgentRolesRepository _roles;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AgentRoleServices(IMapper mapper, IAgentRolesRepository roles, IHttpContextAccessor httpContextAccessor)
    {
        _mapper = mapper;
        _roles = roles;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<SprocMessage> AddmenuPermission(AddcontrollerAction test)
    {
        var response = await _roles.AddmenuPermission(test);
        return response;
    }

    public async Task<SprocMessage> AddRoleAsync(AddRoleVm addRole)
    {
        addRole.AgentCode = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(claims => claims.Type == "AgentCode")?.Value;
        var mappeddata = _mapper.Map<AppRole>(addRole);

        var response = await _roles.AddRoleAsync(mappeddata);
        return response;
    }

    public async Task<SprocMessage> AssignUserRole(int user_id, int[] roleids)
    {
        var response = await _roles.AssignRoletoUser(user_id, roleids);
        return response;
    }

    public async Task<bool> CheckPermission(string area, string controller, string action, string UserName)
    {
        var response = await _roles.CheckPermission(area, controller, action, UserName);
        return response;
    }

    public async Task<AppRole> GetAppRoleById(int Id, string AgentCode)
    {
        AgentCode = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(claims => claims.Type == "AgentCode")?.Value;
        var Result = await _roles.GetRoleByIdAsync(Id, AgentCode);
        return Result;
    }

    public async Task<IEnumerable<GetcontrollerAction>> GetListcontrollerActionAsync(int roleId, string area = "", string controller = "", string action = "")
    {
        var Result = await _roles.GetListcontrollerActionAsync(roleId, area, controller, action);
        return Result;
    }

    public async Task<IEnumerable<AppRole>> GetRoleAsync(string AgentCode)
    {
        var Result = await _roles.GetRoleAsync(AgentCode);
        return Result;
    }

    public async Task<SprocMessage> RemoveRoleAsync(int roleid, string AgentCode)
    {
        AgentCode = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(claims => claims.Type == "AgentCode")?.Value;
        var Result = await _roles.RemoveRoleAsync(roleid, AgentCode);
        return Result;
    }

    public async Task<SprocMessage> UpdateRoleAsync(UpdateRoleVm updateRole)
    {
        var mappeddata = _mapper.Map<AppRole>(updateRole);
        var response = await _roles.UpdateRoleAsync(mappeddata);
        return response;
    }
}
