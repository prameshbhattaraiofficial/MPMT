using AspNetCoreRateLimit;
using Microsoft.Extensions.Options;
using System.Net;

namespace Mpmt.Web.Features.RateLimiting
{
    public class CustomRateLimitingMiddleware : IpRateLimitMiddleware
    {
        public CustomRateLimitingMiddleware(
            RequestDelegate next,
            IProcessingStrategy processingStrategy,
            IOptions<IpRateLimitOptions> options,
            IIpPolicyStore policyStore,
            IRateLimitConfiguration config,
            ILogger<IpRateLimitMiddleware> logger) : base(next, processingStrategy, options, policyStore, config, logger)
        {
        }

        public override async Task ReturnQuotaExceededResponse(HttpContext context, RateLimitRule rule, string retryAfter)
        {
            context.Response.Headers["Retry-After"] = retryAfter;
            context.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
            context.Response.ContentType = "text/html";

            await context.Response.WriteAsync("429 Too Many Requests.");
        }
    }
}
