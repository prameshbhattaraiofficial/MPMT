using System.Security.Claims;

namespace Mpmt.Services.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        //public static bool IsSuperAdmin(this ClaimsPrincipal user)
        //{
        //    return user.HasClaim(UserClaimTypes.IsSuperAdmin, true.ToString());
        //}

        //public static bool IsMerchant(this ClaimsPrincipal user)
        //{
        //    return user.HasClaim(ClaimTypes.Role, UserRoles.Merchant)
        //        && user.HasClaim(UserClaimTypes.IsMerchant, true.ToString());
        //}

        //public static string GetApiClientCode(this ClaimsPrincipal user)
        //{
        //    return user.FindFirstValue(UserClaimTypes.ApiClientClientCodeClaim);
        //}

        //public static string GetPartnerApiClientCode(this ClaimsPrincipal user)
        //{
        //    return user.FindFirstValue(PartnerApiClaimTypes.ClientCodeClaim);
        //}
    }
}
