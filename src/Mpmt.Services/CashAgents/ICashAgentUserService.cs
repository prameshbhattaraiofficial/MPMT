using Mpmt.Core.Dtos;
using Mpmt.Core.Dtos.CashAgent;
using Mpmt.Core.Dtos.Paging;
using Mpmt.Core.Dtos.SuperAgent;
using Mpmt.Core.ViewModel.CashAgent;
using Mpmts.Core.Dtos;
using System.Security.Claims;

namespace Mpmt.Services.CashAgents
{
    public interface ICashAgentUserService
    {
        Task<PagedList<AgentDetail>> GetAgentUserAsync(AgentFilter AgentFilter);
        Task<PagedList<AgentLedger>> GetAgentLedgerAsync(AgentLedgerFilter AgentLedgerFilter);
        Task<MpmtResult> AddFundRequestAsync(AddAgentFundRequest addUpdateFundRequest, ClaimsPrincipal user);
        Task<PagedList<AgentUserModel>> GetAgentAsync(AgentFilterModel agentFilter);
        Task<AgentUser> GetAgentByPhoneNumberAsync(string PhoneNumber);
        Task<AgentPrefundDetail> GetAgentPrefundByAgentCode(string AgentCode);
        Task UpdateAccountSecretKeyAsync(string AgentCode, string accountsecretkey);
        Task<AgentUser> GetAgentByUserNameAsync(string UserName);
        Task<SprocMessage> ChangeAgentPassword(AgentChangePasswordVM changepassword);
        Task UpdateAgentLoginActivityAsync(AgentLoginActivity agentLoginActivity);
        Task<MpmtResult> AddAgentUserAsync(CashAgentUserVm cashAgentUserVm);
        Task<PagedList<AgentAccountStatement>> GetAgentAccountSettlementReport(AgentStatementFilter filter);

        //for agent side
        Task<MpmtResult> AddAgentAsync(CashAgentUserVm cashAgentUserVm);
        Task<MpmtResult> UpdateAgentUserAsync(CashAgentUpdateVm cashAgentUserVm);

        //for agent side
        Task<MpmtResult> UpdateAgentAsync(CashAgentUserVm cashAgentUserVm);
        Task<SprocMessage> ActivateAgentUserAsync(ActivateAgent activateAgent);
        Task<SprocMessage> MarkasSuperAgent(ActivateAgent activateAgent);
        Task<SprocMessage> DeleteUser(ActivateAgent activateAgent);
        Task<SprocMessage> Withdraw(Withdraw withdraw, ClaimsPrincipal user);

        Task<AgentUser> GetCashAgentByAgentCodeAsync(string agentCode);
        Task<bool> VerifyUserName(string userName);
        Task<bool> VerifyContactNumber(string contactNumber);
        Task<bool> VerifyRegistrationNumber(string registrationNumber);
        Task<PagedList<AgentDetail>> GetAgentBySuperAgentAsync(AgentFilter agentFilter);
        Task<PagedList<CashAgentRegister>> GetRemitAgentRegisterAsync(AgentRegisterFilter request);
        Task<SprocMessage> ApprovedAgentRequest(CashAgentRequest request, ClaimsPrincipal claimsPrincipal);
        Task<SprocMessage> RejectAgentRequest(CashAgentRequest request, ClaimsPrincipal claimsPrincipal);
        Task<AgentDetailSignUp> GetAgentDetail(string Email, string phoneNumber);
    }
}
