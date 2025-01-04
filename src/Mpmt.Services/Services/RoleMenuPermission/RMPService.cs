using AutoMapper;
using DocumentFormat.OpenXml.Drawing.Spreadsheet;
using Microsoft.AspNetCore.Http;
using Mpmt.Core.Dtos.Partner;
using Mpmt.Core.Models.RMP;
using Mpmt.Core.Models.Route;
using Mpmt.Data.Repositories.RoleMenuPermissionRepository;
using Mpmts.Core.Dtos;
using System.Security.Claims;

namespace Mpmt.Services.Services.RoleMenuPermission;

public class RMPService : IRMPService
{
    private readonly IRMPRepository _rMPRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;
    private readonly ClaimsPrincipal _loggedInUser;

    public RMPService(IRMPRepository rMPRepository,
        IHttpContextAccessor httpContextAccessor,
        IMapper mapper)
    {
        _rMPRepository = rMPRepository;
        _httpContextAccessor = httpContextAccessor;
        _mapper = mapper;
        _loggedInUser = _httpContextAccessor.HttpContext?.User;
    }

    public async Task<IEnumerable<ActionPermission>> GetActionPermissionListAsync(string controller)
    {
        var UserName = _loggedInUser.FindFirstValue(ClaimTypes.Name);
        var data = await _rMPRepository.GetActionPermissionList("admin", controller, UserName);
        return data;
    }

    public async Task<IEnumerable<ActionPermission>> GetPartnerActionPermissionList(string controller)
    {
        var User = _loggedInUser.FindFirstValue(ClaimTypes.Name);
        var data = await _rMPRepository.GetPartnerActionPermissionList("Partner", controller, User);
        return data;
    }

    public async Task<IEnumerable<GetcontrollerAction>> GetListcontrollerAction(int roleId)
    {
        var data = await _rMPRepository.GetListcontrollerActionAsync(roleId);
        return data;
    }

    public async Task<IEnumerable<GetcontrollerAction>> GetcontrollerActionAsync(int roleId)
    {
        var data = await _rMPRepository.GetListcontrollerActionAsync(roleId);
        return data;
    }

    public async Task<IEnumerable<GetcontrollerAction>> GetPartnerListcontrollerAction(int roleId)
    {
        var data = await _rMPRepository.GetPartnerListcontrollerActionAsync(roleId);
        return data;
    }

    public async Task<SprocMessage> AddPartnermenuPermission(AddcontrollerAction test)
    {
        var data = await _rMPRepository.AddPartnermenuPermissionAsync(test);
        return data;
    }

    public async Task<IEnumerable<MenuSubMenu>> GetMenusSubmenusForCurrentUserAsync(string UserName)
    {
        var User = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(claims => claims.Type == ClaimTypes.Name)?.Value;
        //keep the user name here from httpcontext accessor
        var menusWithSubmenus = await _rMPRepository.GetListWithSubMenusAsync(User);
        var list = new List<MenuSubMenu>();
        foreach (var menu in menusWithSubmenus)
        {
            if (menu.ParentId == 0)
            {
                var temMenu = new MenuSubMenu();
                temMenu = _mapper.Map<MenuSubMenu>(menu);
                var seperation = temMenu.MenuUrl.Split('/');
                if (seperation.Length == 3)
                {
                    temMenu.Area = seperation[1];
                    temMenu.Controller = seperation[2];
                }
                if (seperation.Length == 4)
                {
                    temMenu.Area = seperation[1];
                    temMenu.Controller = seperation[2];
                    temMenu.Action = seperation[3];
                }
                temMenu.submenus = new List<RMPermissionModel>();
                foreach (var sub in menusWithSubmenus)
                {
                    var submenuseprate = sub.MenuUrl.Split('/');
                    if (sub.ParentId == menu.MenuId)
                    {
                        if (submenuseprate.Length == 3)
                        {
                            sub.Area = submenuseprate[1];
                            sub.Controller = submenuseprate[2];
                        }
                        if (submenuseprate.Length == 4)
                        {
                            sub.Area = submenuseprate[1];
                            sub.Controller = submenuseprate[2];
                            sub.Action = submenuseprate[3];
                        }
                        temMenu.submenus.Add(sub);
                    }
                }
                list.Add(temMenu);
            }
        }
        return list;
    }

    public async Task<IEnumerable<ActionPermission>> GetAgentActionPermissionList(string controller)
    {
        var User = _loggedInUser.FindFirstValue(ClaimTypes.Name);
        var data = await _rMPRepository.GetAgentActionPermissionList(controller, User);
        return data;
    }

    public async Task<SprocMessage> PartnerMenuPermission(AddcontrollerAction test)
    {
        var data = await _rMPRepository.PartnerMenuPermissionAsync(test);
        return data;
    }

    public async Task<IEnumerable<GetcontrollerAction>> GetPartnerMenuListControllerAction(int roleId)
    {
        var data = await _rMPRepository.GetPartnerMenuListControllerAction(roleId);
        return data;
    }

    
}   