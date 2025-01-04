using Mpmt.Core.Dtos.CashAgent;
using Mpmt.Core.Dtos.Paging;
using Mpmt.Core.Dtos.SuperAgent;
using Mpmts.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mpmt.Data.Repositories.CashAgent
{
    public interface ICashAgentEmployeeRepository
    {
        Task<AgentUser> GetAgentEmployeeUserByUserName(string UserName);
        Task<AgentUser> GetAgentEmployeeUserByPhonenumberAsync(string PhoneNUmber);
        Task<AgentUser> GetAgentEmployeeUserByEmployeeIdAsync(string EmployeeId, string agentCode);
        Task<SprocMessage> AgentEmployeeChangePasswordAsync(CashAgentUser user);    
        Task<SprocMessage> IUDAgentEmployeeUserAsync(CashAgentUser agentUser);
        Task<PagedList<AgentDetail>> GetAgentEmployeeAsync(AgentFilter AgentFilter);
        Task UpdateAgentEmployeeLoginActivityAsync(AgentLoginActivity agentLoginActivity, int EmployeeId);
        Task UpdateEmployeeAccountSecretKeyAsync(string AgentCode, string accountsecretkey, int employeeId);
        Task UpdateIs2FAAuthenticatedAgentEmployeeAsync(string Agentcode, bool Is2FAAuthenticated, int EmployeeId);
        Task<string> CheckAgentOrEmployeeByUserName(string UserName);
        Task<string> CheckAgentOrEmployeeByContactNumber(string ContactNumber);
        Task<bool> VerifyUserNameAsync(string userName);
    }
}
