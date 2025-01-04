using Microsoft.AspNetCore.Mvc;
using Mpmt.Core.Dtos.CashAgent;
using System.Security.Claims;

namespace Mpmt.Services.Authentication
{
    public interface IAgentCookiesAuthService
    {
        Task<IActionResult> SignInAsync(AgentUser user, string returnUrl, string Code, bool isPersistent = false);
        Task<bool> NormalAgentSignInAsync(AgentUser user, string RoleName);
        Task<ClaimsPrincipal> ChangePasswordRole(AgentUser user, string RoleName);
        Task<bool> NormalAgentEmployeeSignInAsync(AgentUser user, string RoleName);
        Task<IActionResult> SignInAgentEmployeeAsync(AgentUser user, string returnUrl, string Code, bool isPersistent = false);
        Task SignOutAsync();
    }
}
