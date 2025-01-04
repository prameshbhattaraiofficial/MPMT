using Mpmt.Core.Dtos;
using Mpmt.Core.Dtos.CashAgent;
using Mpmt.Core.Dtos.Paging;
using Mpmt.Core.Dtos.Partner;
using Mpmt.Core.Dtos.SuperAgent;
using Mpmt.Core.ViewModel.CashAgent;
using Mpmts.Core.Dtos;

namespace Mpmt.Services.CashAgents
{
    public interface ICashAgentEmployee
    {
        Task<PagedList<AgentDetail>> getAgentEmployeeList(AgentFilter AgentFilter);
        Task<AgentUser> GetAgentEmployeeByUserNameAsync(string UserName);
        Task<AgentUser> GetAgentEmployeeUserByEmployeeIdAsync(string EmployeeId, string agentCode);
        Task<AgentUser> GetAgentEmployeeByPhoneNumberAsync(string PhoneNumber);
        Task UpdateEmployeeAccountSecretKeyAsync(string AgentCode, string accountsecretkey, int employeeId);
        Task UpdateAgentEmployeeLoginActivityAsync(AgentLoginActivity agentLoginActivity, int EmployeeId);
        Task<MpmtResult> AddAgentEmployeeUserAsync(CashAgentEmployeeVm cashAgentUserVm);
        Task<bool> VerifyUserName(string userName);
        Task<MpmtResult> UpdateAgentEmployeeUserAsync(CashAgentEmployeeVm cashAgentUserVm);
        //Task<MpmtResult> UpdateAgentEmployeeAsync(CashAgentUserVm cashAgentUserVm);
        Task<SprocMessage> ActivateAgentUserAsync(ActivateAgentEmployee activateAgentEmployee);
        Task<SprocMessage> DeleteUser(ActivateAgentEmployee activateAgentEmployee);
        //Task<bool> VerifyUserNameAgent(string userName);
        //Task<bool> VerifyEmailAgent(string Email);
        //Task<AgentUser> GetCashAgentListAsync(string agentCode);
        Task<string> CheckAgentOrEmployee(string usernameOrContactNumber);
        Task<VerificationToken> GetOtpByAgentCodeAsync(string agentCode, string UserName, string phoneNumber);
        Task<SprocMessage> ForgotPasswordAsync(ForgotPasswordToken reset);
        Task<SprocMessage> ResetTokenValidationAsync(ForgotPasswordToken reset);

    }
}
