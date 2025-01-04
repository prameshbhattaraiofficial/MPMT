using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Mpmt.Services.Extensions;
using Mpmt.Services.Logging;

namespace Mpmt.Services.Mvc.Filters
{
    public sealed class LogRequestAttribute : TypeFilterAttribute
    {
        public LogRequestAttribute() : base(typeof(LogRequestFilter)) { }

        public class LogRequestFilter : IAsyncResourceFilter
        {
            private readonly IVendorApiLogger _vendorApiLogger;

            public LogRequestFilter(IVendorApiLogger vendorApiLogger)
            {
                _vendorApiLogger = vendorApiLogger;
            }

            public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
            {
                await LogRequestAsync(context);
                await next();
            }

            private async Task LogRequestAsync(ResourceExecutingContext context)
            {
                var requestUrl = context.HttpContext.GetRequestUrl();
                var (_, requestBody) = await context.HttpContext.GetRequestBodyAsStringAsync();
                var reqHeaders = context.HttpContext.GetRequestHeadersAsDictionaryJson();

                await _vendorApiLogger.LogInsertAsync(requestUrl: requestUrl, requestInput: requestBody, requestHeaders: reqHeaders);
            }
        }
    }
}
