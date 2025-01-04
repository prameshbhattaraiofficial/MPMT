using Microsoft.AspNetCore.Authentication;
using System;

namespace Mpmt.Api.Features.AuthenticationSchemes.PartnerApi
{
    public static class PartnerApiAuthenticationBuilderExtensions
    {
        public static AuthenticationBuilder AddPartnerApiAuthentication(
            this AuthenticationBuilder authenticationBuilder,
            Action<PartnerApiAuthenticationOptions> options)
        {
            return authenticationBuilder
                .AddScheme<PartnerApiAuthenticationOptions, PartnerApiAuthenticationHandler>(PartnerApiAuthenticationOptions.DefaultScheme, options);
        }

        public static AuthenticationBuilder AddPartnerApiAuthentication(
            this AuthenticationBuilder authenticationBuilder,
            string authenticationScheme,
            Action<PartnerApiAuthenticationOptions> options)
        {
            if (authenticationScheme is null)
                throw new ArgumentNullException(nameof(authenticationScheme));

            return authenticationBuilder
                .AddScheme<PartnerApiAuthenticationOptions, PartnerApiAuthenticationHandler>(authenticationScheme, options);
        }
    }
}
