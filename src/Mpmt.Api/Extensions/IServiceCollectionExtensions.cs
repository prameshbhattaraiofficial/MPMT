using Microsoft.AspNetCore.Mvc;
using Mpmt.Core.Domain;
using Mpmt.Data.Repositories.AgentModule;
using Mpmt.Data.Repositories.Mailing;
using Mpmt.Data.Repositories.Partner;
using Mpmt.Data.Repositories.PartnerApi;
using Mpmt.Data.Repositories.Payout;
using Mpmt.Data.Repositories.UserActivityLog;
using Mpmt.Services.Authentication;
using Mpmt.Services.Common;
using Mpmt.Services.Services.AgentApi;
using Mpmt.Services.Services.BankLoadApi;
using Mpmt.Services.Services.MailingService;
using Mpmt.Services.Services.PartnerApi;
using Mpmt.Services.Services.UserActivityLog;
using Mpmt.Services.Services.WalletLoadApi.MyPay;

namespace Mpmt.Api.Extensions
{
    /// <summary>
    /// The i service collection extensions.
    /// </summary>
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the application services.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <returns>An IServiceCollection.</returns>
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IFileProviderService, FileProviderService>();
            // TODO: check common services and repositories and move them to service layer registration
            services.AddScoped<IPartnerRepository, PartnerRepository>();
            //services.AddScoped<IUserCookieAuthService, UserCookieAuthService>();
            services.AddScoped<IPartnerApiRepository, PartnerApiRepository>();
            services.AddScoped<IPartnerApiService, PartnerApiService>();
            services.AddScoped<IAgentRepository, AgentRepository>();
            services.AddScoped<IAgentApiService, AgentApiService>();
            services.AddScoped<IAgentApiRepository, AgentApiRepository>();

            //services.AddScoped<IUserActivityLogRepo, UserActivityLogRepo>();
            //services.AddScoped<IUserActivityLog, UserActivityLog>();

            return services;
        }

        /// <summary>
        /// Adds the hosted services.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <returns>An IServiceCollection.</returns>
        public static IServiceCollection AddHostedServices(this IServiceCollection services)
        {
            return services;
        }

        /// <summary>
        /// Configures the application services.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns>An IServiceCollection.</returns>
        public static IServiceCollection ConfigureApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

            // ApiBehavior configuration for invalid API model state
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var errors = actionContext.ModelState
                        .Where(entry => entry.Value.Errors.Count > 0)
                        .SelectMany(entry => entry.Value.Errors, (entry, error) => new FieldError { Field = entry.Key, Message = error.ErrorMessage })
                        .ToList();

                    var errorResponse = new ApiResponse
                    {
                        ResponseCode = ResponseCodes.Code400_BadRequest,
                        ResponseMessage = ResponseMessages.Msg400_BadRequest,
                        ResponseStatus = ResponseStatuses.Error,
                        FieldErrors = errors
                    };

                    return new BadRequestObjectResult(errorResponse);
                };
            });

            services.Configure<MyPayWalletLoadApiConfig>(configuration.GetSection(MyPayWalletLoadApiConfig.SectionName));

            return services;
        }

        /// <summary>
        /// Adds the application cors policies.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns>An IServiceCollection.</returns>
        public static IServiceCollection AddApplicationCorsPolicies(this IServiceCollection services, IConfiguration configuration)
        {
            var originsStr = configuration.GetValue<string>("AllowedOrigins")?.Trim();

            services.AddCors(options =>
            {
                options.AddPolicy(name: MpmtApiDefaults.DefaultCorsPolicyName, policy =>
                {
                    if (originsStr.Equals("*"))
                    {
                        policy
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();

                        return;
                    }

                    var origins = originsStr
                        .Split(";")
                        .Where(o => string.IsNullOrWhiteSpace(o))
                        .Select(o => o.Trim())
                        .ToArray();

                    policy
                    //.SetIsOriginAllowed(origin => true)
                    //.AllowCredentials()
                    .WithOrigins(origins)
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            });

            return services;
        }


    }
}
