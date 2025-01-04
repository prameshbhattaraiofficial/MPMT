using EventManagement.Services.Cache;
using Mpmt.Core.Configuration;
using Mpmt.Core.Events;
using Mpmt.Data.Repositories.Action;
using Mpmt.Data.Repositories.AddressProofType;
using Mpmt.Data.Repositories.AdminDashBoard;
using Mpmt.Data.Repositories.Agents;
using Mpmt.Data.Repositories.Agents.AgentTxn;
using Mpmt.Data.Repositories.Bank;
using Mpmt.Data.Repositories.CashAgent;
using Mpmt.Data.Repositories.Common;
using Mpmt.Data.Repositories.ComplianceRule;
using Mpmt.Data.Repositories.ConversionRate;
using Mpmt.Data.Repositories.Currency;
using Mpmt.Data.Repositories.DocumentType;
using Mpmt.Data.Repositories.Employee;
using Mpmt.Data.Repositories.ExchangeRateRepo;
using Mpmt.Data.Repositories.FeeFundRequest;
using Mpmt.Data.Repositories.FundType;
using Mpmt.Data.Repositories.IncomeSource;
using Mpmt.Data.Repositories.KYCRemark;
using Mpmt.Data.Repositories.Mailing;
using Mpmt.Data.Repositories.Menu;
using Mpmt.Data.Repositories.Module;
using Mpmt.Data.Repositories.ModuleAction;
using Mpmt.Data.Repositories.Notification;
using Mpmt.Data.Repositories.Occupation;
using Mpmt.Data.Repositories.Partner;
using Mpmt.Data.Repositories.Partner.IRepository;
using Mpmt.Data.Repositories.Partner.Repository;
using Mpmt.Data.Repositories.PartnerAction;
using Mpmt.Data.Repositories.PartnerApplications;
using Mpmt.Data.Repositories.PartnerBank;
using Mpmt.Data.Repositories.PartnerEmployee;
using Mpmt.Data.Repositories.PartnerEODBalance;
using Mpmt.Data.Repositories.PartnerRegioster;
using Mpmt.Data.Repositories.PartnerRoles;
using Mpmt.Data.Repositories.PaymentType;
using Mpmt.Data.Repositories.PreFund;
using Mpmt.Data.Repositories.Relation;
using Mpmt.Data.Repositories.RoleMenuPermissionRepository;
using Mpmt.Data.Repositories.RoleModuleAction;
using Mpmt.Data.Repositories.Roles;
using Mpmt.Data.Repositories.ServiceCharge;
using Mpmt.Data.Repositories.ServiceChargeCategory;
using Mpmt.Data.Repositories.TransferPurpose;
using Mpmt.Data.Repositories.UserActivityLog;
using Mpmt.Data.Repositories.Users.AdminUser;
using Mpmt.Services.Authentication;
using Mpmt.Services.CashAgents;
using Mpmt.Services.Common;
using Mpmt.Services.Events;
using Mpmt.Services.Partner;
using Mpmt.Services.Partner.IService;
using Mpmt.Services.Partner.Service;
using Mpmt.Services.Services.Action;
using Mpmt.Services.Services.AddressProofType;
using Mpmt.Services.Services.AdminDashBoard;
using Mpmt.Services.Services.AdminUser;
using Mpmt.Services.Services.AgentApplications;
using Mpmt.Services.Services.Bank;
using Mpmt.Services.Services.Common;
using Mpmt.Services.Services.ComplianceRule;
using Mpmt.Services.Services.ConversionRate;
using Mpmt.Services.Services.Currency;
using Mpmt.Services.Services.DocumentType;
using Mpmt.Services.Services.Employee;
using Mpmt.Services.Services.ExchangeRateService;
using Mpmt.Services.Services.FedanRateService;
using Mpmt.Services.Services.FeeFundRequest;
using Mpmt.Services.Services.FundType;
using Mpmt.Services.Services.http.Sms;
using Mpmt.Services.Services.http.Testhttp;
using Mpmt.Services.Services.IncomeSource;
using Mpmt.Services.Services.KYCRemark;
using Mpmt.Services.Services.MailingService;
using Mpmt.Services.Services.Menu;
using Mpmt.Services.Services.Module;
using Mpmt.Services.Services.ModuleAction;
using Mpmt.Services.Services.Notification;
using Mpmt.Services.Services.Occupation;
using Mpmt.Services.Services.PartnerAction;
using Mpmt.Services.Services.PartnerApplications;
using Mpmt.Services.Services.PartnerBank;
using Mpmt.Services.Services.PartnerEODBalance;
using Mpmt.Services.Services.PartnerModule;
using Mpmt.Services.Services.PartnerRegister;
using Mpmt.Services.Services.PaymentType;
using Mpmt.Services.Services.PreFund;
using Mpmt.Services.Services.Receipt;
using Mpmt.Services.Services.Relation;
using Mpmt.Services.Services.RoleMenuPermission;
using Mpmt.Services.Services.RoleModuleAction;
using Mpmt.Services.Services.Roles;
using Mpmt.Services.Services.ServiceCharge;
using Mpmt.Services.Services.ServiceChargeCategory;
using Mpmt.Services.Services.Sms;
using Mpmt.Services.Services.TransferPurpose;
using Mpmt.Services.Services.UserActivityLog;
using Mpmt.Web.Features.Authentication;
using Mpmt.Web.Filter;
using System.Reflection;

namespace Mpmt.Web.Extensions
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

            services.AddScoped<IUserCookieAuthService, UserCookieAuthService>();
            services.AddScoped<IPartnerRoleServices, PartnerRoleServices>();

            #region Repository
            //Setting
            services.AddScoped<ICurrencyRepo, CurrencyRepo>();
            services.AddScoped<IBankRepo, BankRepo>();
            services.AddScoped<IOccupationRepo, OccupationRepo>();
            services.AddScoped<IRelationRepo, RelationRepo>();
            services.AddScoped<ITransferPurposeRepo, TransferPurposeRepo>();
            services.AddScoped<IKycRemarkRepo, KycRemarkRepo>();
            services.AddScoped<IDocumentTypeRepo, DocumentTypeRepo>();
            services.AddScoped<IFundTypeRepo, FundTypeRepo>();
            services.AddScoped<IServiceCategoryRepo, ServiceCategoryRepo>();
            services.AddScoped<IPartnerBankRepo, PartnerBankRepo>();
            services.AddScoped<IConversionRateRepo, ConversionRateRepo>();
            services.AddScoped<IPaymentTypeRepo, PaymentTypeRepo>();
            services.AddScoped<IServiceChargeRepo, ServiceChargeRepo>();
            services.AddScoped<ICommonddlRepo, CommonddlRepo>();
            services.AddScoped<IPartnerRepository, PartnerRepository>();
            services.AddScoped<IUserActivityLogRepo, UserActivityLogRepo>();
            services.AddScoped<IPartnerWalletCurrencyRepo, PartnerWalletCurrencyRepo>();
            services.AddScoped<IPreFundRequestRepo, PreFundRequestRepo>();
            services.AddScoped<IRolesRepository, RolesRepository>();
            services.AddScoped<IAddressProofTypeRepo, AddressProofTypeRepo>();
            services.AddScoped<ICashAgentRepository, CashAgentRepository>();
            services.AddScoped<IIncomeSourceRepo, IncomeSourceRepo>();
            services.AddScoped<IPartnerCredentialsRepository, PartnerCredentialsRepository>();
            services.AddScoped<IPartnerRolesRepository, PartnerRolesRepository>();
            services.AddScoped<IPartnerCredentialsRepository, PartnerCredentialsRepository>();
            services.AddScoped<IPartnerSenderRepository, PartnerSenderRepository>();
            services.AddScoped<IAgentReportRepository, AgentReportRepository>();
            services.AddScoped<ICashAgentRepository, CashAgentRepository>();
            services.AddScoped<ICashAgentEmployeeRepository, CashAgentEmployeeRepository>();

            services.AddScoped<IReceiptGenerationService, ReceiptGenerationService>();
            services.AddScoped<IRegisterRepository, RegisterRepository>();
            services.AddScoped<IComplianceRuleRepo, ComplianceRuleRepo>();
            services.AddScoped<IRMPRepository, RMPRepository>();

            services.AddScoped<IEmployeeRepo, EmployeeRepo>();
            services.AddScoped<IMenuRepository, MenuRepository>();

            services.AddScoped<IPartnerDashBoardRepo, PartnerDashBoardRepo>();
            services.AddScoped<IAdminDashBoardRepo, AdminDashBoardRepo>();
            services.AddScoped<IFeeFundRequestRepo, FeeFundRequestRepo>();
            services.AddScoped<IAgentCredentialsRepository, AgentCredentialsRepository>();

            //remove this after test
            services.AddScoped<IMypayClient, MypayClient>();

            //Partner Module
            services.AddScoped<IPartnerRecipentRepo, PartnerRecipentRepo>();
            services.AddScoped<IPartnerConversionRateRepo, PartnerConversionRateRepo>();
            services.AddScoped<IPartnerSendTxnsRepository, PartnerSendTxnsRepository>();
            services.AddScoped<IRemitPartnerRegisterRepo, RemitPartnerRegisterRepo>();
            services.AddScoped<IAdminUserRepo, AdminUserRepo>();
            services.AddScoped<IRemitPartnerRegisterRepo, RemitPartnerRegisterRepo>();
            services.AddScoped<ITransactionRepo, TransactionRepo>();
            services.AddScoped<IPartnerReportRepo, PartnerReportRepo>();
            services.AddScoped<IModuleRepository, ModuleRepository>();
            services.AddScoped<IActionRepository, ActionRepository>();
            services.AddScoped<IModuleActionRepository, ModuleActionRepository>();
            services.AddScoped<IRoleModuleActionRepository, RoleModuleActionRepository>();

            services.AddScoped<IPartnerModuleRepository, PartnerModuleRepository>();
            services.AddScoped<IPartnerActionRepository, PartnerActionRepository>();
            services.AddScoped<IPartnerEmployeeRepo, PartnerEmployeeRepo>();

            services.AddScoped<IPartnerApplicationsRepo, PartnerApplicationsRepo>();
            services.AddScoped<IAgentRegistrationRepository, AgentRegistrationRepository>();
            services.AddScoped<INotificationRepo, NotificationRepo>();

            services.AddSingleton<IEODBalanceRepository, EODBalanceRepository>();
            services.AddSingleton<IMailRepository, MailRepository>();
            services.AddSingleton<IExchangeRateRepository, ExchangeRateRepository>();

            //Agent
            services.AddScoped<ICashAgentRepository, CashAgentRepository>();
            services.AddScoped<ICashAgentCommissionRepository, CashAgentCommissionRepository>();

            #endregion

            #region Services

            services.AddScoped<IFileProviderService, FileProviderService>();

            //Setting
            services.AddScoped<ICurrencyServices, CurrencyServices>();
            services.AddScoped<IBankServices, BankServices>();
            services.AddScoped<IOccupationServices, OccupationServices>();
            services.AddScoped<IRelationServices, RelationServices>();
            services.AddScoped<ITransferPurposeServices, TransferPurposeServices>();
            services.AddScoped<IKycRemarkServices, KycRemarkServices>();
            services.AddScoped<IDocumentTypeServices, DocumentTypeServices>();
            services.AddScoped<IAgentMenuService, AgentMenuService>();
            services.AddScoped<IFundTypeServices, FundTypeServices>();
            services.AddScoped<IServiceCategoryServices, ServiceCategoryServices>();
            services.AddScoped<IPartnerBankServices, PartnerBankServices>();
            services.AddScoped<IConversionRateServices, ConversionRateServices>();
            services.AddScoped<IPaymentTypeServices, PaymentTypeServices>();
            services.AddScoped<IServiceChargeServices, ServiceChargeServices>();
            services.AddScoped<ICommonddlServices, CommonddlServices>();
            services.AddScoped<IPartnerRegistrationService, PartnerRegistrationService>();
            services.AddScoped<IPartnerService, PartnerService>();
            services.AddScoped<IPartnerCookieAuthService, PartnerCookieAuthService>();
            services.AddScoped<IUserActivityLog, UserActivityLog>();
            services.AddScoped<IPartnerWalletCurrencyServices, PartnerWalletCurrencyServices>();
            services.AddScoped<IPartnerCredentialsService, PartnerCredentialsService>();
            services.AddScoped<IPartnerWalletCurrencyServices, PartnerWalletCurrencyServices>();
            services.AddScoped<IPreFundRequestServices, PreFundRequestServices>();
            services.AddScoped<IAddressProofTypeService, AddressProofTypeService>();
            services.AddScoped<IIncomeSourceService, IncomeSourceService>();
            services.AddScoped<IPartnerCredentialsService, PartnerCredentialsService>();
            services.AddScoped<IRoleServices, RoleServices>();
            services.AddScoped<IPartnerRoleServices, PartnerRoleServices>();
            services.AddScoped<IPartnerSenderService, PartnerSenderService>();
            services.AddScoped<IAgentReportService, AgentReportService>();

            services.AddScoped<IRegisterService, RegisterService>();
            //PartnerModule
            services.AddScoped<IPartnerRecipentServices, PartnerRecipentServices>();
            services.AddScoped<IPartnerConversionRateServices, PartnerConversionRateServices>();

            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<RoleGroupFilterAttribute>();
            services.AddScoped<IPartnerSendTxnsService, PartnerSendTxnsService>();
            services.AddScoped<IMenuService, MenuService>();
            services.AddScoped<IFeeFundRequestService, FeeFundRequestService>();

            services.AddScoped<IDashBoardServices, DashBoardServices>();
            services.AddScoped<IAdminDashBoardServices, AdminDashBoardServices>();
            services.AddScoped<IAdminUserServices, AdminuserServices>();

            services.AddScoped<ITransactionServices, TransactionServices>();
            services.AddScoped<IRemitPartnerRegisterServices, RemitPartnerRegisterServices>();
            services.AddScoped<IPartnerReportServices, PartnerReportServices>();
            services.AddScoped<IModuleService, ModuleService>();
            services.AddScoped<IActionService, ActionService>();
            services.AddScoped<IModuleActionService, ModuleActionService>();
            services.AddScoped<IRoleModuleActionService, RoleModuleActionService>();
            services.AddScoped<IRemitPartnerRegisterServices, RemitPartnerRegisterServices>();
            services.AddScoped<IComplianceRuleService, ComplianceRuleService>();
            services.AddScoped<IRMPService, RMPService>();

            services.AddScoped<IPartnerModuleService, PartnerModuleService>();
            services.AddScoped<IPartnerActionService, PartnerActionService>();
            services.AddScoped<IPartnerEmployeeService, PartnerEmployeeService>();

            services.AddScoped<IPartnerApplicationsService, PartnerApplicationsService>();
            services.AddScoped<IAgentApplicationsService, AgentApplicationsService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<ICashAgentUserService, CashAgentUserService>();

            services.AddSingleton<IEODBalanceService, EODBalanceService>();
            services.AddSingleton<IMailService, MailService>();
            services.AddSingleton<IExchangeRateService, ExchangeRateService>();

            services.AddScoped<PartnerAuthorizationFilter>();

            #endregion

            services.AddScoped<IUserAuthSessionService, UserAuthSessionService>();
            services.AddScoped<CustomCookieAuthenticationEvents>();

            // Sociair Sms
            services.AddScoped<ISmsService, SmsService>();
            services.AddScoped<ISmsHttpClient, SmsHttpClient>();

            //agent
            services.AddScoped<ICashAgentUserService, CashAgentUserService>();
            services.AddScoped<ICashAgentCommissionService, CashAgentCommissionService>();
            services.AddScoped<IAgentCredentialsService, AgentCredentialsService>();
            services.AddScoped<IAgentReportService, AgentReportService>();

            services.AddScoped<IAgentTransactionRepository, AgentTransactionRepository>();

            return services;
        }

        /// <summary>
        /// Adds the hosted services.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <returns>An IServiceCollection.</returns>
        public static IServiceCollection AddHostedServices(this IServiceCollection services)
        {
            services.AddHostedService<MpmtBackgroundService>();
            services.AddHostedService<FedanRateBackgroundService>();
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
            services.Configure<RouteOptions>(opts => opts.LowercaseUrls = true);
            // methods options configuration
            services.Configure<MypayConfig>(configuration.GetSection(MypayConfig.SectionName));

            return services;
        }
    }
}
