using Mpmt.Core.Domain;
using Mpmt.Services.Logging;
using System.Net;
using System.Text.Json;

namespace Mpmt.Api.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHostEnvironment _env;

        public ExceptionMiddleware(RequestDelegate next, IHostEnvironment env)
        {
            _next = next;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context, IExceptionLogger exceptionLogger, IVendorApiLogger vendorApiLogger)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                var isClientError = context.Response.StatusCode is >= 400 and < 500;
                var statusCode = isClientError ? context.Response.StatusCode : (int)HttpStatusCode.InternalServerError;

                var response = _env.IsDevelopment()
                    ? isClientError
                        ? new ApiResponse { ResponseCode = statusCode.ToString(), ResponseStatus = ResponseStatuses.Error }
                        : new ApiResponse
                        {
                            ResponseCode = statusCode.ToString(),
                            ResponseStatus = ResponseStatuses.Error,
                            ResponseMessage = ex.Message,
                            ResponseDetailMessage = ex.StackTrace
                        }
                    : isClientError
                        ? new ApiResponse { ResponseCode = statusCode.ToString(), ResponseStatus = ResponseStatuses.Error }
                        : new ApiResponse { ResponseCode = statusCode.ToString(), ResponseStatus = ResponseStatuses.Error };

                var jsonSerializerOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

                var jsonResponse = JsonSerializer.Serialize(response, response.GetType(), jsonSerializerOptions);

                // Ignore Swagger exceptions, Not found exceptions
                if (!context.Request.Path.StartsWithSegments("/swagger") && statusCode != 404)
                {
                    await Task.WhenAll(
                        exceptionLogger.LogAsync(ex),
                        // When exception occurs, 'response log' filter cannot update the log, that's why we log here
                        vendorApiLogger.LogUpdateResponseAsync(responseHttpStatus: statusCode, responseOutput: jsonResponse)
                        );
                }

                if (!context.Response.HasStarted)
                {
                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = statusCode;
                    await context.Response.WriteAsync(jsonResponse);
                }
            }
        }
    }
}
