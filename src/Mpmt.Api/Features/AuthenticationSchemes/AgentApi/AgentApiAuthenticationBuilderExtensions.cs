using Microsoft.AspNetCore.Authentication;
using Mpmt.Api.Features.AuthenticationSchemes.AgentApi;

namespace Mpmt.Api.Features.AuthenticationSchemes.AgentApi
{
    public static class AgentApiAuthenticationBuilderExtensions
    {
        public static AuthenticationBuilder AddAgentApiAuthentication(
            this AuthenticationBuilder authenticationBuilder,
            Action<AgentApiAuthenticationOptions> options)
        {
            return authenticationBuilder
                .AddScheme<AgentApiAuthenticationOptions, AgentApiAuthenticationHandler>(AgentApiAuthenticationOptions.DefaultScheme, options);
        }
        public static AuthenticationBuilder AddAgentApiAuthentication(
           this AuthenticationBuilder authenticationBuilder,
           string authenticationScheme,
           Action<AgentApiAuthenticationOptions> options)
        {
            if (authenticationScheme is null)
                throw new ArgumentNullException(nameof(authenticationScheme));

            return authenticationBuilder
                .AddScheme<AgentApiAuthenticationOptions, AgentApiAuthenticationHandler>(authenticationScheme, options);
        }
    }
}
