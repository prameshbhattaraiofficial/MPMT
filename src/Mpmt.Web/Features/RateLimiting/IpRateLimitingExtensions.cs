using AspNetCoreRateLimit;
using Newtonsoft.Json;
using System.Reflection;

namespace Mpmt.Web.Features.RateLimiting
{
    public static class IpRateLimitingExtensions
    {
        public static IServiceCollection AddAppRateLimiting(this IServiceCollection services, IConfiguration config)
        {
            // Add rate limiting services
            //services.AddMemoryCache();
            //services.Configure<IpRateLimitOptions>(config.GetSection("IpRateLimiting"));

            var rulesPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Config\\RateLimit\\ipratelimit.generalrules.json");
            var rulesByteContent = File.ReadAllText(rulesPath);
            var rules = JsonConvert.DeserializeObject<List<RateLimitRule>>(rulesByteContent);

            services.Configure<IpRateLimitOptions>(opts =>
            {
                opts.EnableEndpointRateLimiting = true;
                opts.StackBlockedRequests = true;
                opts.RealIpHeader = "X-Real-IP";
                opts.ClientIdHeader = "X-ClientId";
                opts.HttpStatusCode = 429;
                opts.GeneralRules = rules;
            });

            //services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            //services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
            services.AddSingleton<IIpPolicyStore, LazyCacheCachePolicyStore>(_ => new LazyCacheCachePolicyStore());
            services.AddSingleton<IRateLimitCounterStore, LazyCacheRateLimitCounterStore>(_ => new LazyCacheRateLimitCounterStore());

            return services;
        }
    }
}
