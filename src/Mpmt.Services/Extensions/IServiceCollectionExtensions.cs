using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mpmt.Data.Common;
using Mpmt.Data.Repositories.Logging;
using Mpmt.Data.Repositories.Mailing;
using Mpmt.Data.Repositories.Payout;
using Mpmt.Data.Repositories.Roles;
using Mpmt.Data.Repositories.Users;
using Mpmt.Data.Repositories.Users.AdminUser;
using Mpmt.Services.Common;
using Mpmt.Services.Infrastructure;
using Mpmt.Services.Logging;
using Mpmt.Services.Partner;
using Mpmt.Services.Services.AdminUser;
using Mpmt.Services.Services.BankLoadApi;
using Mpmt.Services.Services.MailingService;
using Mpmt.Services.Services.WalletLoadApi.MyPay;
using Mpmt.Services.Users;

namespace Mpmt.Services.Extensions
{
    /// <summary>
    /// The i service collection extensions.
    /// </summary>
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the common application services.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <returns>An IServiceCollection.</returns>
        public static IServiceCollection AddCommonApplicationServices(this IServiceCollection services)
        {
            // ActionContext accessor
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            services.AddScoped<IWebHelper, WebHelper>();
            services.AddScoped<IMailRepository, MailRepository>();
            services.AddScoped<IMailService, MailService>();

            #region Logging
            services.AddScoped<IExceptionLogRepository, ExceptionLogRepository>();
            services.AddScoped<IExceptionLogger, ExceptionLogger>();
            services.AddScoped<IVendorApiLogRepository, VendorApiLogRepository>();
            services.AddScoped<IVendorApiLogger, VendorApiLogger>();
            services.AddScoped<IAgentApiLogRepository, AgentApiLogRepository>();
            services.AddScoped<IAgentApiLogger, AgentApiLogger>();
            #endregion

            #region Repository
            services.AddScoped<IUsersRepository, UsersRepository>();
            services.AddScoped<IRolesRepository, RolesRepository>();
            services.AddScoped<IUserRolesRepository, UserRolesRepository>();
            services.AddScoped<IAdminUserRepo, AdminUserRepo>();


            #endregion

            #region Services
            services.AddScoped<IUsersService, UsersService>();
            services.AddScoped<IUserRegistrationService, UserRegistrationService>();
            services.AddScoped<IAdminUserServices, AdminuserServices>();


            #endregion

            // Vendor API services
            services.AddScoped<IPayoutRepository, PayoutRepository>();
            services.AddScoped<IMyPayWalletLoadRepository, MyPayWalletLoadRepository>();
            services.AddScoped<IMyPayWalletLoadApiService, MyPayWalletLoadApiService>();
            services.AddScoped<IMyPayBankLoadApiService, MyPayBankLoadApiService>();
            services.AddScoped<IPartnerPayoutHandlerService, PartnerPayoutHandlerService>();

            return services;
        }

        /// <summary>
        /// Adds the common package libraries.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <returns>An IServiceCollection.</returns>
        public static IServiceCollection AddCommonPackageLibraries(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingProfiles).Assembly);
            services.AddHttpContextAccessor();
            services.AddLazyCache();

            return services;
        }

        /// <summary>
        /// Configures the common application services.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns>An IServiceCollection.</returns>
        public static IServiceCollection ConfigureCommonApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Bind default database connection string
            DbConnectionManager.DefaultConnectionString = configuration.GetConnectionString("DefaultConnection");

            // MyPayWalletLoad API Configuration
            services.Configure<MyPayWalletLoadApiConfig>(configuration.GetSection(MyPayWalletLoadApiConfig.SectionName));
            services.Configure<MyPayBankLoadApiConfig>(configuration.GetSection(MyPayBankLoadApiConfig.SectionName));

            return services;
        }
    }
}
