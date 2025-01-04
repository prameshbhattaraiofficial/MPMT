using Microsoft.AspNetCore.Authentication;

namespace Mpmt.Api.Features.AuthenticationSchemes.PartnerApi
{
    public class PartnerApiAuthenticationOptions : AuthenticationSchemeOptions
    {
        public const string DefaultScheme = "PartnerApiAuthentication";
        public string Scheme => DefaultScheme;
        public string AuthenticationType = DefaultScheme;
    }
}
