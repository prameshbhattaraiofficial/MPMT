using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Mpmt.Services.Logging;

namespace Mpmt.Services.Mvc.Filters
{
    public sealed class LogResponseAgentAttribute : TypeFilterAttribute
    {
        public LogResponseAgentAttribute() : base(typeof(LogResponseFilter)) { }

        public class LogResponseFilter : IAsyncResourceFilter
        {
            private readonly IAgentApiLogger _agentApiLogger;


            public LogResponseFilter(IAgentApiLogger agentApiLogger)
            {
                _agentApiLogger = agentApiLogger;
            }

            public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
            {
                // Before the action method is executed
                var originalResponseStream = context.HttpContext.Response.Body;
                var memoryStream = new MemoryStream();
                context.HttpContext.Response.Body = memoryStream;

                var executedContext = await next();

                memoryStream.Seek(0, SeekOrigin.Begin);
                var responseBody = new StreamReader(memoryStream).ReadToEnd();
                memoryStream.Seek(0, SeekOrigin.Begin);

                context.HttpContext.Response.Body = originalResponseStream;
                await memoryStream.CopyToAsync(originalResponseStream);

                await _agentApiLogger.LogUpdateResponseAsync(responseHttpStatus: executedContext.HttpContext.Response.StatusCode, responseOutput: responseBody);
            }

            private string[] ResponseContentTypes => new string[] { "text/plain", "text/html", "application/xml", "text/xml", "application/json", "text/json" };
        }
    }
}
