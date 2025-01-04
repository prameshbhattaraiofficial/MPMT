using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Mpmt.Core.Configuration;
using Mpmt.Core.Domain;
using Mpmt.Web.Features.Authentication;
using Microsoft.Extensions.Caching.SqlServer;
using System.Configuration;

namespace Mpmt.Web.Extensions
{
    /// <summary>
    /// The authentication service extensions.
    /// </summary>
    public static class AuthenticationServiceExtensions
    {
        /// <summary>
        /// Adds the authentication services.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="config">The config.</param>
        /// <returns>An IServiceCollection.</returns>
        public static IServiceCollection AddAuthenticationServices(this IServiceCollection services, IConfiguration config)
        {
            //services.AddDistributedSqlServerCache(options =>
            //{
            //    options.ConnectionString = config.GetConnectionString("DefaultConnection");
            //    options.SchemaName = "dbo";
            //    options.TableName = "SessionState";
            //});
            //services.AddScoped<ITicketStore, SqlServerCacheTicketStore>();

            services.AddDistributedMemoryCache();


            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
                {
                    var cookieAuthOptions = config.GetSection(CookieAuthOptions.SectionName).Get<CookieAuthOptions>();

                    options.Cookie.Name = AuthDefaults.CookieAuthenticationName;
                    options.Cookie.HttpOnly = true;
                    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                    //options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                    options.Cookie.IsEssential = true;
                    options.Cookie.SameSite = SameSiteMode.Strict;
                    options.LoginPath = AuthDefaults.PartnerLoginPath;
                    options.LogoutPath = AuthDefaults.PartnerLogoutPath;
                    options.AccessDeniedPath = AuthDefaults.AccessDeniedPath;
                    options.ExpireTimeSpan = DateTime.Now
                        .AddDays(cookieAuthOptions.ExpirationDays)
                        .AddHours(cookieAuthOptions.ExpirationHours)
                        .AddMinutes(cookieAuthOptions.ExpirationMinutes)
                        .AddSeconds(cookieAuthOptions.ExpirationSeconds) - DateTime.Now;
                    options.SlidingExpiration = cookieAuthOptions.SlidingExpiration;
                    options.SessionStore = new InMemoryCacheTicketStore();
                    //options.SessionStore = new CustomDistributedCacheTicketStore(services.BuildServiceProvider().GetService<IDistributedCache>());

                    //// Use SqlServerCacheTicketStore for session storage
                    //var cacheTicketStore = new SqlServerCacheTicketStore(services.BuildServiceProvider().GetService<IDistributedCache>());
                    //options.SessionStore = cacheTicketStore;

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

            //services.Configure<CookieAuthOptions>(config.GetSection(CookieAuthOptions.SectionName));

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
