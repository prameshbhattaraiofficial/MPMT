using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Mpmt.Core.Common.Helpers;
using Mpmt.Core.Domain;
using Mpmt.Core.Domain.Partners;
using Mpmt.Core.Dtos.Partner;
using Mpmt.Data.Repositories.Partner;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace Mpmt.Api.Features.AuthenticationSchemes.PartnerApi
{
    public class PartnerApiAuthenticationHandler : AuthenticationHandler<PartnerApiAuthenticationOptions>
    {
        private string _responseCode = ResponseCodes.Code401_Unauthorized;
        private string _errorMessage = string.Empty;
        private readonly IConfiguration _config;
        private readonly IPartnerRepository _partnerRepository;

        public PartnerApiAuthenticationHandler(
            IOptionsMonitor<PartnerApiAuthenticationOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IConfiguration config,
            IPartnerRepository partnerRepository) : base(options, logger, encoder, clock)
        {
            _config = config;
            _partnerRepository = partnerRepository;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            const string MessageInvalidApiCreds = "Invalid API credentials.";
            const string MessageReqFromUnauthorizedIp = "Request from an unauthorized IP address.";

            _errorMessage = string.Empty;

            Request.Headers.TryGetValue(PartnerApiAuthenticationDefaults.AuthorizationHeader, out var authorizationHeaderValue);
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

            var apiClient = await _partnerRepository.GetPartnerWithCredentialsByApiUserNameAsync(apiUserName);
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

            var response = new PartnerApiErrorResponse
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
            Response.StatusCode = StatusCodes.Status403Forbidden;

            var response = new PartnerApiErrorResponse
            {
                ResponseCode = ResponseCodes.Code403_Forbidden,
                ResponseStatus = ResponseStatuses.Error,
                ResponseMessage = ResponseMessages.Msg403_Forbidden
            };

            await Response.WriteAsync(JsonSerializer.Serialize(response, DefaultJsonSerializerOptions.Options));
        }

        private bool IsRequestFromValidIpAddress(PartnerWithCredentials partner)
        {
            _ = bool.TryParse(_config["PartnerApi:IpFilterEnabled"] ?? "True", out var isIpFilterEnabled);
            _ = bool.TryParse(_config["PartnerApi:AllowLoopbackInterNetworkIps"] ?? "False", out var allowLoopbackInterNetworkIps);

            if (!isIpFilterEnabled)
                return true;

            var clientIp = Request.HttpContext.Connection.RemoteIpAddress;
            if (clientIp is null)
                return false;

            if (allowLoopbackInterNetworkIps && CommonHelper.IsLoopbackInterNetworkIp(clientIp))
                return true;

            var isValidIp = (partner.IPAddress ?? string.Empty)
                    .Split(",")
                    .Select(ip => ip.Trim())
                    .Where(ip => IPAddress.TryParse(ip, out _))
                    .Select(ip => IPAddress.Parse(ip).MapToIPv4())
                    .Any(ip => ip.GetAddressBytes().SequenceEqual(clientIp.MapToIPv4().GetAddressBytes()));

            return isValidIp;
        }

        private AuthenticationTicket GetAuthenticationTicket(PartnerWithCredentials apiClient)
        {
            var claims = new List<Claim>
            {
                new Claim(PartnerClaimTypes.Id, apiClient.Id.ToString()),
                new Claim(PartnerClaimTypes.ApiUserName, apiClient.ApiUserName),
                new Claim(PartnerClaimTypes.PartnerCode, apiClient.PartnerCode),
                new Claim(PartnerClaimTypes.UserType, "Partner")
            };

            if (!string.IsNullOrWhiteSpace(apiClient.UserName))
                claims.Add(new Claim(PartnerClaimTypes.UserName, apiClient.UserName));

            if (!string.IsNullOrWhiteSpace(apiClient.ApiKey))
                claims.Add(new Claim(PartnerClaimTypes.ApiKey, apiClient.ApiKey));

            if (!string.IsNullOrWhiteSpace(apiClient.IPAddress))
                claims.Add(new Claim(PartnerClaimTypes.Ips, apiClient.IPAddress));

            //if (!string.IsNullOrWhiteSpace(apiClient.SystemPrivateKey))
            //    claims.Add(new Claim(PartnerClaimTypes.SystemPrivateKey, apiClient.SystemPrivateKey));

            if (!string.IsNullOrWhiteSpace(apiClient.SystemPublicKey))
                claims.Add(new Claim(PartnerClaimTypes.SystemPublicKey, apiClient.SystemPublicKey));

            if (!string.IsNullOrWhiteSpace(apiClient.UserPublicKey))
                claims.Add(new Claim(PartnerClaimTypes.UserPublicKey, apiClient.UserPublicKey));

            var identity = new ClaimsIdentity(claims, Options.AuthenticationType);
            var identities = new List<ClaimsIdentity> { identity };
            var principal = new ClaimsPrincipal(identities);

            return new AuthenticationTicket(principal, Options.Scheme);
        }
    }
}
