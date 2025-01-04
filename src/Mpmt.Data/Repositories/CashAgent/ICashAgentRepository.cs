using Mpmt.Core.Domain.Agents;
using Mpmt.Core.Dtos.CashAgent;
using Mpmt.Core.Dtos.Paging;
using Mpmt.Core.Dtos.SuperAgent;
using Mpmt.Core.Models.Route;
using Mpmt.Core.ViewModel.CashAgent;
using Mpmts.Core.Dtos;

namespace Mpmt.Data.Repositories.CashAgent
{
    public interface ICashAgentRepository
    {
        Task<PagedList<AgentDetail>> GetAgentUserAsync(AgentFilter userFilter);
        //agentside
        Task<PagedList<AgentUserModel>> GetAgentAsync(AgentFilterModel agentFilter);
        Task<SprocMessage> IUDAgentUserAsync(CashAgentUser agentUser);
        Task<SprocMessage> WithdrawPrefundAsync(Withdraw withdraw); 
        //Task<SprocMessage> AssignRoletoUser(int user_id, int[] roleids);
        Task UpdateIs2FAAuthenticatedAsync(string AgentCode, bool Is2FAAuthenticated);
        Task<AgentUser> GetAgentUserByUserName(string UserName);
        Task<AgentUser> GetAgentUserByPhonenumber(string PhoneNUmber);
        Task<AgentPrefundDetail> GetAgentPrefundByAgentCode(string AgentCode);
        Task<PagedList<AgentAccountStatement>> GetAgentAccountSettlementReport(AgentStatementFilter filter);
        Task UpdateAgentLoginActivityAsync(AgentLoginActivity agentLoginActivity);
        Task UpdateAccountSecretKeyAsync(string AgentCode, string accountsecretkey);
        Task<SprocMessage> AddAgentMenuAsync(IUDAgentMenu menu);
        Task<IUDAgentMenu> GetAgentMenuByIdAsync(int MenuId);
        Task<IEnumerable<AgentMenuModel>> GetAgentMenuAsync();
        Task<IEnumerable<AgentMenuChild>> GetMenusSubmenusForCurrentUserByUser(string Username);
        Task<SprocMessage> UpdateMenuDisplayOrderAsync(IUDAgentMenu menuUpdate);
        Task<SprocMessage> UpdateMenuIsActiveAsync(IUDAgentMenu menuUpdate);
        Task<AgentUser> GetCashAgentByAgentCodeAsync(string agentCode);
        Task<SprocMessage> AgentChangePasswordAsync(CashAgentUser user);
        Task<SprocMessage> AgentEmployeeChangePasswordAsync(CashAgentUser user);
        Task<IEnumerable<GetcontrollerAction>> GetListcontrollerActionAsync(string UserType);
        Task<SprocMessage> AddmenuPermission(AddControllerActionUserType test);
        Task<bool> VerifyUserNameAsync(string userName);
        Task<bool> VerifyContactNumber(string contactNumber);
        Task<bool> VerifyRegistrationNumber(string registrationNumber);
        Task<PagedList<AgentLedger>> GetAgentLedgerAsync(AgentLedgerFilter AgentLedgerFilter);
        Task<PagedList<AgentDetail>> GetAgentBySuperAgentAsync(AgentFilter agentFilter);
        Task<PagedList<CashAgentRegister>> GetRemitAgentRegisterAsync(AgentRegisterFilter request);
        Task<SprocMessage> RegisterAgent(RegisterAgent agentRegister);
        Task<AgentDetailSignUp> GetRegisterAgent(OtpValidationAgent validate);
        Task<SprocMessage> ValidateOtpAsync(OtpValidationAgent validate);
        Task<AgentDetailSignUp> GetAgentDetailById(string Id);
        Task<AgentDetailSignUp> GetAgentDetail(string Email, string phoneNumber);
        Task<SprocMessage> ApprovedRejectAgentRequest(CashAgentRequest request);
        Task<SprocMessage> AddUpdateFundRequestAsync(AddAgentFundRequest addUpdateFundRequest);
    }
}
