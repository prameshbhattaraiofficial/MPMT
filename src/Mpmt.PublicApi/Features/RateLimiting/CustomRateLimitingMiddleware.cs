using AspNetCoreRateLimit;
using Microsoft.Extensions.Options;
using Mpmt.Core.Domain;
using Newtonsoft.Json;
using System.Net;

namespace Mpmt.PublicApi.Features.RateLimiting
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
            context.Response.ContentType = "application/json";

            var responseMessage = new ApiResponse
            {
                ResponseCode = context.Response.StatusCode.ToString(),
                ResponseStatus = ResponseStatuses.Error,
                ResponseMessage = ResponseMessages.Msg429_TooManyRequests,
                ResponseDetailMessage = "You have exceeded the rate limit for this endpoint. Please try again later."
            };

            await context.Response.WriteAsync(JsonConvert.SerializeObject(responseMessage));
        }
    }
}
