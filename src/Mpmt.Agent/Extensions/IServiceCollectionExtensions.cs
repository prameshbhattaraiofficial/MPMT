using EventManagement.Services.Cache;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Mpmt.Agent.Features.Authentication;
using Mpmt.Core.Events;
using Mpmt.Data.Common;
using Mpmt.Data.Repositories.AgentFundTransfer;
using Mpmt.Data.Repositories.Agents.AgentTxn;
using Mpmt.Data.Repositories.CashAgent;
using Mpmt.Data.Repositories.Common;
using Mpmt.Data.Repositories.Logging;
using Mpmt.Data.Repositories.Mailing;
using Mpmt.Data.Repositories.Notification;
using Mpmt.Data.Repositories.RoleModuleAction;
using Mpmt.Data.Repositories.Roles;
using Mpmt.Services.Authentication;
using Mpmt.Services.CashAgents;
using Mpmt.Services.Common;
using Mpmt.Services.Events;
using Mpmt.Services.Logging;
using Mpmt.Services.Services.AgentApplications.AgentFundTransfer;
using Mpmt.Services.Services.AgentTxn;
using Mpmt.Services.Services.Common;
using Mpmt.Services.Services.http.Sms;
using Mpmt.Services.Services.MailingService;
using Mpmt.Services.Services.Notification;
using Mpmt.Services.Services.Roles;
using System.Reflection;
using Mpmt.Services.Services.Sms;
using Mpmt.Data.Repositories.Partner;
using Mpmt.Services.Services.Receipt;
using Mpmt.Services.Services.RoleMenuPermission;
using Mpmt.Data.Repositories.RoleMenuPermissionRepository;
using Mpmt.Services.Services.AgentDashboardService;
using Mpmt.Data.Repositories.AgentDashboardRepo;

namespace Mpmt.Agent.Extensions;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        DbConnectionManager.DefaultConnectionString = configuration.GetConnectionString("DefaultConnection");

        // Event publisher
        services.AddScoped<IEventPublisher, EventPublisher>();

        // Event consumers registration
        // 1. Get all event consumers implementing generic interface 'IEventConsumer<>'
        // 2. Register each consumer implementing generic interface 'IEventConsumer<>' containing type parameter
        Assembly
            .GetAssembly(typeof(EntityEventConsumer))
            .GetTypes()
            .Where(type => type.GetInterfaces().Any(
                i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEventConsumer<>)))
            .ToList()
            .ForEach(consumerType =>
            {
                consumerType
                .GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEventConsumer<>))
                .ToList()
                .ForEach(iType => services.AddScoped(iType, consumerType));
            });

        services.AddScoped<IUserAuthSessionService, UserAuthSessionService>();
        services.AddScoped<CustomCookieAuthenticationEvents>();

        #region services

        // ActionContext accessor
        services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
        services.AddSingleton<IUrlHelperFactory, UrlHelperFactory>();

        services.TryAddScoped<IExceptionLogRepository, ExceptionLogRepository>();
        services.TryAddScoped<IExceptionLogger, ExceptionLogger>();
        services.AddScoped<IFileProviderService, FileProviderService>();
        services.AddScoped<ICashAgentEmployee, CashAgentEmployee>();
        services.AddScoped<IMailService, MailService>();

        // Sociair Sms
        services.AddScoped<ISmsService, SmsService>();
        services.AddScoped<ISmsHttpClient, SmsHttpClient>();

        services.AddScoped<IAgentRegistrationService, AgentRegistrationService>();
        services.AddScoped<IAgentMenuService, AgentMenuService>();
        services.AddScoped<ICommonddlServices, CommonddlServices>();
        services.AddScoped<IAgentRoleServices, AgentRoleServices>();

        services.AddScoped<ICashAgentUserService, CashAgentUserService>();
        services.AddScoped<IAgentCookiesAuthService, AgentCookiesAuthService>();
        services.AddScoped<IWebHelper, WebHelper>();
        services.AddScoped<ITransactionService, TransactionService>();
        services.AddScoped<IAgentFundTransfer, AgentFundTransfer>();
        services.AddScoped<IAgentReportService, AgentReportService>();
        services.AddScoped<INotificationService, NotificationService>();
        services.AddScoped<ICashAgentCommissionService, CashAgentCommissionService>();
        services.AddScoped<IReceiptGenerationService, ReceiptGenerationService>();
        services.AddScoped<IMailService, MailService>();
        services.AddScoped<IRMPService, RMPService>();
        services.AddScoped<IAgentDashboardService, AgentDashboardService>();
        services.AddScoped<ICashAgentUserService, CashAgentUserService>();

        #endregion services

        #region Repository

        services.AddScoped<ICashAgentRepository, CashAgentRepository>();
        services.AddScoped<ICommonddlRepo, CommonddlRepo>();
        services.AddScoped<ICashAgentEmployeeRepository, CashAgentEmployeeRepository>();
        services.AddScoped<IAgentTransactionRepository, AgentTransactionRepository>();
        services.AddScoped<IAgentFundTransferRepository, AgentFundTransferRepository>();
        services.AddScoped<IAgentRolesRepository, AgentRolesRepository>();
        services.AddScoped<IAgentReportRepository, AgentReportRepository>();
        services.AddScoped<IRoleModuleActionRepository, RoleModuleActionRepository>();
        services.AddScoped<IMailRepository, MailRepository>();
        services.AddScoped<INotificationRepo, NotificationRepo>();
        services.AddScoped<ICashAgentCommissionRepository, CashAgentCommissionRepository>();
        services.AddScoped<IRMPRepository, RMPRepository>();
        services.AddScoped<IAgentDashboardRepo, AgentDashboardRepo>();
        services.AddScoped<ICashAgentRepository, CashAgentRepository>();
        //Temporary
        services.AddScoped<IPartnerRepository, PartnerRepository>();

        #endregion Repository

        return services;
    }
}