using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Mpmt.Api.Features.AuthenticationSchemes.PartnerApi;
using Mpmt.Core.Common.Helpers;
using Mpmt.Core.Domain;
using Mpmt.Core.Domain.Agent;
using Mpmt.Core.Dtos.Agent;
using Mpmt.Data.Repositories.AgentModule;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace Mpmt.Api.Features.AuthenticationSchemes.AgentApi
{
    public class AgentApiAuthenticationHandler : AuthenticationHandler<AgentApiAuthenticationOptions>
    {
        private readonly IConfiguration _config;
        private readonly IAgentRepository _agentRepository;
        private string _responseCode = ResponseCodes.Code401_Unauthorized;
        private string _errorMessage = string.Empty;

        public AgentApiAuthenticationHandler(
           IOptionsMonitor<AgentApiAuthenticationOptions> options,
           ILoggerFactory logger,
           UrlEncoder encoder,
           ISystemClock clock,
           IConfiguration config,
           IAgentRepository agentRepository) : base(options, logger, encoder, clock)
        {
            _config = config;
            _agentRepository = agentRepository;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            const string MessageInvalidApiCreds = "Invalid API credentials.";
            const string MessageReqFromUnauthorizedIp = "Request from an unauthorized IP address.";

            _errorMessage = string.Empty;

            Request.Headers.TryGetValue(AgentApiAuthenticationDefaults.AuthorizationHeader, out var authorizationHeaderValue);
            var basicAuth = authorizationHeaderValue.FirstOrDefault();

            if (string.IsNullOrWhiteSpace(basicAuth))
            {
                _errorMessage = $"API credentials are required.";
                return AuthenticateResult.NoResult();
            }

            if (!basicAuth.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase))
            {
                _errorMessage = MessageInvalidApiCreds;
                return AuthenticateResult.NoResult();
            }

            // to apiusername:password baseS64 string
            basicAuth = basicAuth[6..]?.Trim();
            if (string.IsNullOrWhiteSpace(basicAuth))
            {
                _errorMessage = MessageInvalidApiCreds;
                return AuthenticateResult.NoResult();
            }

            // to apiusername:password
            basicAuth = Encoding.UTF8.GetString(Convert.FromBase64String(basicAuth));
            var basicAuthCredentials = basicAuth.Split(":");
            if (basicAuthCredentials.Length < 2)
            {
                _errorMessage = MessageInvalidApiCreds;
                return AuthenticateResult.NoResult();
            }

            var apiUserName = basicAuthCredentials[0];
            var apiPassword = basicAuthCredentials[1];

            var apiClient = await _agentRepository.GetAgentWithCredentialsByApiUserNameAsync(apiUserName);
            if (apiClient is null)
            {
                _errorMessage = MessageInvalidApiCreds;
                return AuthenticateResult.NoResult();
            }

            if (apiClient.IsDeleted)
            {
                _errorMessage = MessageInvalidApiCreds;
                return AuthenticateResult.NoResult();
            }
            if (apiClient.IsBlocked)
            {
                _errorMessage = "Account blocked, please contact your API vendor.";
                return AuthenticateResult.NoResult();
            }

            if (!apiClient.IsActive)
            {
                _errorMessage = "Account inactive, please contact your API vendor.";
                return AuthenticateResult.NoResult();
            }

            // ApiPassword validation
            if (string.IsNullOrWhiteSpace(apiClient.ApiPassword) || !apiClient.ApiPassword.Equals(apiPassword))
            {
                _errorMessage = MessageInvalidApiCreds;
                return AuthenticateResult.NoResult();
            }

            // IP validation
            if (!IsRequestFromValidIpAddress(apiClient))
            {
                _errorMessage = MessageReqFromUnauthorizedIp;
                return AuthenticateResult.NoResult();
            }

            return AuthenticateResult.Success(GetAuthenticationTicket(apiClient));
        }

        protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            if (Response.HasStarted)
                return;

            Response.ContentType = "application/json";
            Response.StatusCode = 401;

            var response = new AgentApiErrorResponse
            {
                ResponseCode = _responseCode,
                ResponseStatus = ResponseStatuses.Error,
                ResponseMessage = _errorMessage
            };

            await Response.WriteAsync(JsonSerializer.Serialize(response, DefaultJsonSerializerOptions.Options));
        }

        protected override async Task HandleForbiddenAsync(AuthenticationProperties properties)
        {
            if (Response.HasStarted)
                return;

            Response.ContentType = "application/json";
            Response.StatusCode = 403;

            var response = new AgentApiErrorResponse
            {
                ResponseCode = ResponseCodes.Code403_Forbidden,
                ResponseStatus = ResponseStatuses.Error,
                ResponseMessage = ResponseMessages.Msg403_Forbidden
            };

            await Response.WriteAsync(JsonSerializer.Serialize(response, DefaultJsonSerializerOptions.Options));
        }

        private bool IsRequestFromValidIpAddress(AgentWithCredentials agentCredential)
        {
            _ = bool.TryParse(_config["AgentApi:IpFilterEnabled"] ?? "True", out var isIpFilterEnabled);
            _ = bool.TryParse(_config["AgentApi:AllowLoopbackInterNetworkIps"] ?? "False", out var allowLoopbackInterNetworkIps);

            if (!isIpFilterEnabled)
                return true;

            var clientIp = Request.HttpContext.Connection.RemoteIpAddress;
            if (clientIp is null)
                return false;

            if (allowLoopbackInterNetworkIps && CommonHelper.IsLoopbackInterNetworkIp(clientIp))
                return true;

            var isValidIp = (agentCredential.IPAddress ?? string.Empty)
                    .Split(",")
                    .Select(ip => ip.Trim())
                    .Where(ip => IPAddress.TryParse(ip, out _))
                    .Select(ip => IPAddress.Parse(ip).MapToIPv4())
                    .Any(ip => ip.GetAddressBytes().SequenceEqual(clientIp.MapToIPv4().GetAddressBytes()));

            return isValidIp;
        }

        private AuthenticationTicket GetAuthenticationTicket(AgentWithCredentials apiClient)
        {
            var claims = new List<Claim>
            {
                new Claim(AgentClaimTypes.Id, apiClient.Id.ToString()),
                new Claim(AgentClaimTypes.ApiUserName, apiClient.ApiUserName),
                new Claim(AgentClaimTypes.AgentCode, apiClient.AgentCode),
                new Claim(AgentClaimTypes.UserType, "Agent")
            };

            if (!string.IsNullOrWhiteSpace(apiClient.UserName))
                claims.Add(new Claim(AgentClaimTypes.UserName, apiClient.UserName));

            if (!string.IsNullOrWhiteSpace(apiClient.ApiKey))
                claims.Add(new Claim(AgentClaimTypes.ApiKey, apiClient.ApiKey));

            if (!string.IsNullOrWhiteSpace(apiClient.IPAddress))
                claims.Add(new Claim(AgentClaimTypes.Ips, apiClient.IPAddress));

            if (!string.IsNullOrWhiteSpace(apiClient.SystemPrivateKey))
                claims.Add(new Claim(AgentClaimTypes.SystemPrivateKey, apiClient.SystemPrivateKey));

            if (!string.IsNullOrWhiteSpace(apiClient.SystemPublicKey))
                claims.Add(new Claim(AgentClaimTypes.SystemPublicKey, apiClient.SystemPublicKey));

            if (!string.IsNullOrWhiteSpace(apiClient.UserPublicKey))
                claims.Add(new Claim(AgentClaimTypes.UserPublicKey, apiClient.UserPublicKey));

            if (!string.IsNullOrWhiteSpace(apiClient.UserPrivateKey))
                claims.Add(new Claim(AgentClaimTypes.UserPrivateKey, apiClient.UserPrivateKey));

            var identity = new ClaimsIdentity(claims, Options.AuthenticationType);
            var identities = new List<ClaimsIdentity> { identity };
            var principal = new ClaimsPrincipal(identities);

            return new AuthenticationTicket(principal, Options.Scheme);
        }
    }
}
