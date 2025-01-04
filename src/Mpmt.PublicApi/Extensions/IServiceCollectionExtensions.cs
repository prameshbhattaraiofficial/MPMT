using Microsoft.AspNetCore.Mvc;
using Mpmt.Core.Domain;
using Mpmt.Data.Common;
using Mpmt.Data.Repositories.AgentList;
using Mpmt.Data.Repositories.Agents;
using Mpmt.Data.Repositories.Common;
using Mpmt.Data.Repositories.Partner;
using Mpmt.Data.Repositories.Public;
using Mpmt.Services.Partner;
using Mpmt.Services.Public.Feedbacks;
using Mpmt.Services.Services.AgentListApi;
using Mpmt.Services.Services.Common;

namespace Mpmt.PublicApi.Extensions
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
            // TODO: check common services and repositories and move them to service layer registration
            services.AddScoped<ICommonddlRepo, CommonddlRepo>();
            services.AddScoped<ICommonddlServices, CommonddlServices>();
            services.AddScoped<IPartnerApplicationRepository, PartnerApplicationRepository>();
            services.AddScoped<IPartnerApplicationService, PartnerApplicationService>();
            services.AddScoped<IFeedbackRepository, FeedbackRepository>();
            services.AddScoped<IFeedbackService, FeedbackService>();
            services.AddScoped<IAgentRegistrationService, AgentRegistrationService>();
            services.AddScoped<IAgentRegistrationRepository, AgentRegistrationRepository>();

            services.AddScoped<IAgentListService, AgentListService>();
            services.AddScoped<IAgentListRepository, AgentListRepository>();

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
            // Bind default database connection string
            DbConnectionManager.DefaultConnectionString = configuration.GetConnectionString("DefaultConnection");

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
                options.AddPolicy(name: MpmtPublicApiDefaults.DefaultCorsPolicyName, policy =>
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