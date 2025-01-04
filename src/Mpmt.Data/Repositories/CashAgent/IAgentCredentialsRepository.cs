using Mpmt.Core.Dtos.CashAgent;
using Mpmt.Core.Dtos.Partner;
using Mpmt.Core.ViewModel.CashAgent;
using Mpmts.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mpmt.Data.Repositories.CashAgent
{
    public interface IAgentCredentialsRepository
    {
        Task<SprocMessage> InsertCredentialsAsync(AgentCredential cred);
        
        Task<AgentCredential> GetCredentialsByIdAsync(string credentialId);

        Task<AgentCredential> GetCredentialsByAgentCodeAsync(string AgentCode);

        Task<RemitCashAgentsDetails> GetDetailsByAgentCode(string AgentCode);


        Task<SprocMessage> UpdateCredentialsAsync(AgentCredential cred);
      
        Task<SprocMessage> UpdateApiKeyAsync(string AgentCode, string credentialId, string apiKey, string loggedInUserId = null, string loggedInUserName = null);
      
        Task<SprocMessage> UpdateSystemRsaKeyPairAsync(string AgentCode, string credentialId, string privateKey, string publicKey, string loggedInUserId = null, string loggedInUserName = null);
       
        Task<SprocMessage> UpdateUserRsaKeyPairAsync(string AgentCode, string credentialId, string privateKey, string publicKey, string loggedInUserId = null, string loggedInUserName = null);
       
        Task<SprocMessage> UpdateApiPasswordAsync(string AgentCode, string credentialId, string apiPassword, string loggedInUserId = null, string loggedInUserName = null);
    }
}
