using AutoMapper;
using DocumentFormat.OpenXml.Vml.Office;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mpmt.Core.Dtos.CashAgent;
using Mpmt.Core.Models.RMP;
using Mpmt.Core.Models.Route;
using Mpmt.Core.ViewModel.CashAgent;
using Mpmt.Core.ViewModel.Menu;
using Mpmt.Data.Repositories.CashAgent;
using Mpmts.Core.Dtos;
using System.Security.Claims;

namespace Mpmt.Services.CashAgents;

public class AgentMenuService : IAgentMenuService
{
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ClaimsPrincipal _loggedInUser;
    private readonly ICashAgentRepository _cashAgentRepository;

    public AgentMenuService(IMapper mapper,
        IHttpContextAccessor httpContextAccessor,
        ICashAgentRepository cashAgentRepository)
    {
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
        _cashAgentRepository = cashAgentRepository;
        _loggedInUser = _httpContextAccessor.HttpContext?.User; ;
    }

    public async Task<SprocMessage> AddAgentMenuAsync(IUDAgentMenu menu)
    {
        menu.Event = 'I';
        var response = await _cashAgentRepository.AddAgentMenuAsync(menu);
        return response;
    }

    public async Task<IEnumerable<AgentMenuModel>> GetAgentMenuAsync()
    {
        var response = await _cashAgentRepository.GetAgentMenuAsync();
        return response;
    }

    public async Task<IUDAgentMenu> GetAgentMenuByIdAsync(int MenuId)
    {
        var response = await _cashAgentRepository.GetAgentMenuByIdAsync(MenuId);
        return response;
    }

    public async Task<IEnumerable<AgentMenuChild>> GetMenusSubmenusForCurrentUserByUserTypeAsync(string UserType)
    {
        UserType = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(claims => claims.Type == "UserType")?.Value;
        //keep the user name here from httpcontext accessor
        var result = await _cashAgentRepository.GetAgentMenuAsync();
       
        var menuWithChild = new List<AgentMenuChild>();
        foreach (var menu in result)
        {
            if (menu.ParentId == 0)
            {
                var ParentMenu = new AgentMenuChild
                {
                    Child = new List<AgentMenuChild>(),
                    Id = menu.Id,
                    ImagePath = menu.ImagePath,
                    ParentId = menu.ParentId,
                    Title = menu.Title,
                    Controller = menu.Controller,
                    Action = menu.Action,
                    Status = menu.IsActive,
                    DisplayOrder = menu.DisplayOrder
                };

                foreach (var child in result)
                {
                    if (child.ParentId == menu.Id && child.Id != menu.Id)
                    {
                        var childmenu = new AgentMenuChild();
                        childmenu.Id = child.Id;
                        childmenu.ParentId = child.ParentId;
                        childmenu.ImagePath = child.ImagePath;
                        childmenu.Title = child.Title;
                        childmenu.Controller = child.Controller;
                        childmenu.Action = child.Action;
                        childmenu.MenuUrl = child.MenuUrl;
                        childmenu.Status = child.IsActive;
                        childmenu.DisplayOrder = child.DisplayOrder;
                        ParentMenu.Child.Add(childmenu);
                    }
                }
                menuWithChild.Add(ParentMenu);
            }
        }
        return menuWithChild;
    }

    public async Task<IEnumerable<AgentMenuChild>> GetMenusSubmenusForCurrentUserByUser()
    {
        //UserType = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(claims => claims.Type == "UserType")?.Value;
        var UserName = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
        //keep the user name here from httpcontext accessor
        var result = await _cashAgentRepository.GetMenusSubmenusForCurrentUserByUser(UserName);
        // var menuToDisplay = result.Where(x => x.IsActive == true);
        var menuWithChild = new List<AgentMenuChild>();
        foreach (var menu in result)
        {
            if (menu.ParentId == 0)
            {
                var ParentMenu = new AgentMenuChild
                {
                    Id = menu.Id,
                    ImagePath = menu.ImagePath,
                    ParentId = menu.ParentId,
                    Title = menu.Title,
                    Controller = menu.Controller,
                    Action = menu.Action,
                    Permission = menu.Permission,
                    Status = menu.Status,
                    DisplayOrder = menu.DisplayOrder,
                    Child = new List<AgentMenuChild>()
                };

                foreach (var child in result)
                {
                    if (child.ParentId == menu.Id && child.Id != menu.Id)
                    {

                        var ChildMenu = new AgentMenuChild();
                        ChildMenu.Id = child.Id;
                        ChildMenu.ParentId = child.ParentId;
                        ChildMenu.ImagePath = child.ImagePath;
                        ChildMenu.Title = child.Title;
                        ChildMenu.Action = child.Action;
                        ChildMenu.Controller = child.Controller;
                        ChildMenu.Area = child.Area;
                        ChildMenu.Status = child.Status;
                        ChildMenu.DisplayOrder = child.DisplayOrder;
                        ChildMenu.Permission = child.Permission;
                        ParentMenu.Child.Add(ChildMenu);
                    }
                }
                menuWithChild.Add(ParentMenu);
            }
        }
        return menuWithChild;
    }

    public async Task<SprocMessage> RemoveAgentMenuAsync(IUDAgentMenu menu)
    {
        menu.Event = 'D';
        var response = await _cashAgentRepository.AddAgentMenuAsync(menu);
        return response;
    }

    public async Task<SprocMessage> UpdateAgentMenuAsync(IUDAgentMenu menu)
    {
        menu.Event = 'U';
        var response = await _cashAgentRepository.AddAgentMenuAsync(menu);
        return response;
    }

    public async Task<SprocMessage> UpdateMenuDisplayOrderAsync(IUDAgentMenu menu)
    {
        var response = await _cashAgentRepository.UpdateMenuDisplayOrderAsync(menu);
        return response;
    }

    public async Task<SprocMessage> UpdateMenuIsActiveAsync(IUDAgentMenu menu)
    {
        var response = await _cashAgentRepository.UpdateMenuIsActiveAsync(menu);
        return response;
    }

    public async Task<IEnumerable<GetcontrollerAction>> GetListcontrollerActionAsync(string UserType)
    {
        var response = await _cashAgentRepository.GetListcontrollerActionAsync(UserType);
        return response;
    }

    public async Task<SprocMessage> AddmenuPermission(AddControllerActionUserType test)
    {
        test.CreatedBy = _loggedInUser.FindFirstValue(ClaimTypes.Name);
        var response = await _cashAgentRepository.AddmenuPermission(test);
        return response;
    }
}
