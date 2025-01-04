using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.CookiePolicy;
using Mpmt.Agent.Features.Authentication;
using Mpmt.Core.Configuration;
using Mpmt.Core.Domain;

namespace Mpmt.Agent.Extensions
{
    public static class AgentAuthenticationExtension
    {
        public static IServiceCollection AddAgentAuthenticationService(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,options =>
                {
                    var cookieAuthSettings = configuration.GetSection(CookieAuthOptions.SectionName).Get<CookieAuthOptions>();

                    options.Cookie.Name = AgentAuthDefaults.CookieAuthenticationName;
                    options.Cookie.HttpOnly = true;
                    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                    //options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                    options.Cookie.SameSite = SameSiteMode.Strict;
                    options.LoginPath = AgentAuthDefaults.AgentLoginPath;
                    options.LogoutPath = AgentAuthDefaults.AgentLogoutPath;
                    options.AccessDeniedPath = AgentAuthDefaults.AccessDeniedPath;
                    options.ExpireTimeSpan = DateTime.Now
                        .AddDays(cookieAuthSettings.ExpirationDays)
                        .AddHours(cookieAuthSettings.ExpirationHours)
                        .AddMinutes(cookieAuthSettings.ExpirationMinutes)
                        .AddSeconds(cookieAuthSettings.ExpirationSeconds) - DateTime.Now;
                    options.SlidingExpiration = cookieAuthSettings.SlidingExpiration;
                    options.SessionStore = new InMemoryCacheTicketStore();
                    options.EventsType = typeof(CustomCookieAuthenticationEvents);
                });
            services.AddAuthorization(config =>
            {
                config.DefaultPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .AddAuthenticationSchemes(CookieAuthenticationDefaults.AuthenticationScheme)
                .Build();

                // Add application auth policies defined in Features/Auth/Policies directory
                //config.AddApplicationPolicies();
            });
            services.Configure<CookiePolicyOptions>(options =>
            {
                // prevent access from javascript 
                options.HttpOnly = HttpOnlyPolicy.Always;

                // If the URI that provides the cookie is HTTPS, 
                // cookie will be sent ONLY for HTTPS requests 
                // (refer mozilla docs for details) 
                options.Secure = CookieSecurePolicy.SameAsRequest;

                // refer "SameSite cookies" on mozilla website 
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromDays(2);
            });

            return services;

        }
    }
}
