using Mpmt.Core.Dtos.Partner;
using Mpmt.Core.Dtos;
using Mpmt.Core.ViewModel.ChangePassword;
using Mpmts.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mpmt.Core.Dtos.Agent;
using Mpmt.Core.Dtos.CashAgent;
using Mpmt.Core.ViewModel.CashAgent;

namespace Mpmt.Services.CashAgents
{
    public interface IAgentCredentialsService
    {
        Task<AgentCredential> GetAgentCredentialsByIdAsync(string credentialId);

        Task<AgentCredential> GetCredentialsByAgentCodeAsync(string AgentCode);

        Task<RemitCashAgentsDetails> GetDetailsByAgentCode (string AgentCode);



        Task<MpmtResult> AddCredentialsAsync(AgentCredentialInsertRequest request);
        Task<MpmtResult> UpdateCredentialsAsync(AgentCredentialUpdateRequest request);
       
        Task<(SprocMessage, string privateKey, string publicKey)> RegenerateSystemRsaKeyPairAsync(string AgentCode, string credentialId);
       
        Task<(SprocMessage, string privateKey, string publicKey)> RegenerateUserRsaKeyPairAsync(string AgentCode, string credentialId);
       
        Task<(SprocMessage, string apiKey)> RegenerateApiKeyAsync(string partnerCode, string credentialId);
      
        Task<(SprocMessage, string apiPassword)> RegenerateApiPasswordAsync(string partnerCode, string credentialId);
    }
}
