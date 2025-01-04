namespace Mpmt.Core.Domain
{
    /// <summary>
    /// The auth defaults.
    /// </summary>
    public static class AuthDefaults
    {

        public const string CookieAuthenticationName = "mpmtauth";

        public const string AdminLoginPath = "/Admin/Login";

        public const string PartnerLoginPath = "/Partner/Login";

        public const string PartnerLogoutPath = "/Partner/Login";

        
        public const string LogoutPath = "/Account/Logout";

       
        public const string AccessDeniedPath = "/errors/forbidden";
    }
}
