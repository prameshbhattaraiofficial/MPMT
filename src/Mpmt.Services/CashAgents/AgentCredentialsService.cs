using Mpmt.Core.Common.Helpers;
using Mpmt.Core.Common;
using Mpmt.Core.Dtos;
using Mpmt.Core.Extensions;
using Mpmt.Core.Dtos.Agent;
using Mpmt.Core.Dtos.CashAgent;
using Mpmt.Data.Repositories.CashAgent;
using Mpmt.Core.Dtos.Partner;
using Mpmts.Core.Dtos;
using Mpmt.Core.Dtos.Roles;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Net;
using Mpmt.Core.ViewModel.CashAgent;

namespace Mpmt.Services.CashAgents
{
    public class AgentCredentialsService : IAgentCredentialsService
    {
        private readonly IAgentCredentialsRepository _agentCredentialsRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ClaimsPrincipal _loggedInUser;

        public AgentCredentialsService(IAgentCredentialsRepository agentCredentialsRepository, IHttpContextAccessor httpContextAccessor)
        {
            _agentCredentialsRepository = agentCredentialsRepository;
            _httpContextAccessor = httpContextAccessor;
            _loggedInUser = _httpContextAccessor.HttpContext?.User;
        }

        public async Task<MpmtResult> AddCredentialsAsync(AgentCredentialInsertRequest request)
        {
            var result = new MpmtResult();
            if (string.IsNullOrWhiteSpace(request.AgentCode))
            {
                result.AddError(400, "Agent is required");
                return result;
            }

            if (string.IsNullOrWhiteSpace(request.ApiUserName))
            {
                result.AddError(400, "ApiUserName is required");
                return result;
            }

            //if (!CommonHelper.IsValidIpAddress(request.IPAddress))
            if (request.IPAddress == null || request.IPAddress.Any(ip => !CommonHelper.IsValidIpAddress(ip.ToString())))
            {
                result.AddError(400, "Invalid IPAddress");
                return result;
            }

            var multpleipaddress = "";
            foreach (var item in request.IPAddress)
            {
                multpleipaddress = multpleipaddress==""?item.ToString():(multpleipaddress+","+ item.ToString());

            }

            var creds = new AgentCredential
            {
                AgentCode = request.AgentCode,
                ApiUserName = request.ApiUserName,  
                IPAddress = multpleipaddress,
                IsActive = request.IsActive,
                ApiPassword = PasswordUtils.GeneratePassword(16),
                ApiKey = PasswordUtils.GeneratePassword(64)
            };

            (creds.SystemPublicKey, creds.SystemPrivateKey) = RsaCryptoUtils.GenerateRSAKeyPairPem(2048);
            (creds.UserPublicKey, creds.UserPrivateKey) = RsaCryptoUtils.GenerateRSAKeyPairPem(2048);

            creds.CreatedByName = "testyash"; // to be changed later

            creds.OperationMode = "I";

            var addResult = await _agentCredentialsRepository.InsertCredentialsAsync(creds);
            result = addResult.MapToMpmtResult();

            return result;
        }



        public async Task<AgentCredential> GetAgentCredentialsByIdAsync(string credentialId)
        {
            ArgumentNullException.ThrowIfNull(credentialId);

            return await _agentCredentialsRepository.GetCredentialsByIdAsync(credentialId);
        }

        public async Task<AgentCredential> GetCredentialsByAgentCodeAsync(string AgentCode)
        {
            ArgumentNullException.ThrowIfNull(AgentCode);

            return await _agentCredentialsRepository.GetCredentialsByAgentCodeAsync(AgentCode);
        }


        public async Task<(SprocMessage, string apiKey)> RegenerateApiKeyAsync(string Agentcode, string credentialId)
        {
            ArgumentNullException.ThrowIfNull(Agentcode);
            ArgumentNullException.ThrowIfNull(credentialId);

            var apiKey = PasswordUtils.GeneratePassword(64);
            var loggedInUserName = _loggedInUser.FindFirstValue(ClaimTypes.Name); 
            var sprocMessage = await _agentCredentialsRepository.UpdateApiKeyAsync(Agentcode, credentialId, apiKey, loggedInUserName: loggedInUserName);

            if (sprocMessage.StatusCode != 200)
                return (sprocMessage, default);

            return (sprocMessage, apiKey);
        }

        public async  Task<(SprocMessage, string apiPassword)> RegenerateApiPasswordAsync(string partnerCode, string credentialId)
        {
            ArgumentNullException.ThrowIfNull(partnerCode);
            ArgumentNullException.ThrowIfNull(credentialId);

            var password = PasswordUtils.GeneratePassword(16);
            var loggedInUserName = _loggedInUser.FindFirstValue(ClaimTypes.Name);
            var sprocMessage = await _agentCredentialsRepository.UpdateApiPasswordAsync(partnerCode, credentialId, password, loggedInUserName: loggedInUserName);

            if (sprocMessage.StatusCode != 200)
                return (sprocMessage, default);

            return (sprocMessage, password);
        }

        public async Task<(SprocMessage, string privateKey, string publicKey)> RegenerateSystemRsaKeyPairAsync(string AgentCode, string credentialId)
        {
            ArgumentNullException.ThrowIfNull(AgentCode);
            ArgumentNullException.ThrowIfNull(credentialId);

            var loggedInUserName = _loggedInUser.FindFirstValue(ClaimTypes.Name);

            var (systemPublicKey, systemPrivateKey) = RsaCryptoUtils.GenerateRSAKeyPairPem(2048);
            var sprocMessage = await _agentCredentialsRepository
                .UpdateSystemRsaKeyPairAsync(AgentCode, credentialId, systemPrivateKey, systemPublicKey, loggedInUserName: loggedInUserName);

            if (sprocMessage.StatusCode != 200)
                return (sprocMessage, default, default);

            return (sprocMessage, systemPrivateKey, systemPublicKey);
        }

        public async Task<(SprocMessage, string privateKey, string publicKey)> RegenerateUserRsaKeyPairAsync(string AgentCode, string credentialId)
        {
            ArgumentNullException.ThrowIfNull(AgentCode);
            ArgumentNullException.ThrowIfNull(credentialId);

            var loggedInUserName = _loggedInUser.FindFirstValue(ClaimTypes.Name);

            var (userPublicKey, userPrivateKey) = RsaCryptoUtils.GenerateRSAKeyPairPem(2048);
            var sprocMessage = await _agentCredentialsRepository
                .UpdateUserRsaKeyPairAsync(AgentCode, credentialId, userPrivateKey, userPublicKey, loggedInUserName: loggedInUserName);

            if (sprocMessage.StatusCode != 200)
                return (sprocMessage, default, default);

            return (sprocMessage, userPrivateKey, userPublicKey);
        }

        public async Task<MpmtResult> UpdateCredentialsAsync(AgentCredentialUpdateRequest request)
        {
            var result = new MpmtResult();

            if (string.IsNullOrWhiteSpace(request.AgentCode))
            {
                result.AddError(400, "AgentCode is required.");
                return result;
            }

            if (string.IsNullOrWhiteSpace(request.ApiUserName))
            {
                result.AddError(400, "ApiUserName is required.");
                return result;
            }
            //if (string.IsNullOrWhiteSpace(request.CredentialId))
            //{
            //    result.AddError(400, "CredentialId is required.");
            //    return result;
            //}
            var multpleipaddress = "";
            foreach (var item in request.IPAddress)       
            {
                if (!CommonHelper.IsValidIpAddress(item.ToString()))
                {
                    result.AddError(400, "Invalid IPAddress");
                    return result;
                }
                multpleipaddress = multpleipaddress == "" ? item.ToString() : (multpleipaddress + "," + item.ToString());
            }

       
            var creds = new AgentCredential
            {
                AgentCode = request.AgentCode,
                ApiUserName = request.ApiUserName,
                IPAddress = multpleipaddress,
                IsActive = request.IsActive,
                CredentialId = request.CredentialId,
                
            };
            creds.UpdatedByName = _loggedInUser.FindFirstValue(ClaimTypes.Name);
            creds.OperationMode = "U";
            var updateResult = await _agentCredentialsRepository.InsertCredentialsAsync(creds);
            result = updateResult.MapToMpmtResult();

            return result;
        }

       public  async Task<RemitCashAgentsDetails> GetDetailsByAgentCode(string AgentCode)
        {
            ArgumentNullException.ThrowIfNull(AgentCode);

            return await _agentCredentialsRepository.GetDetailsByAgentCode(AgentCode);
        }
    }
}
