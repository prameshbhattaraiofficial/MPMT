using Microsoft.AspNetCore.Authentication;

namespace Mpmt.Api.Features.AuthenticationSchemes.AgentApi
{
    public class AgentApiAuthenticationOptions:AuthenticationSchemeOptions
    {
        public const string DefaultScheme = "AgentApiAuthentication";
        public string Scheme => DefaultScheme;
        public string AuthenticationType = DefaultScheme;
    }
}
