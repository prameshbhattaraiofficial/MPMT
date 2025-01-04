using Mpmt.Core.Domain;
using System.Net;
using System.Text.Json;

namespace Mpmt.PublicApi.Middleware
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

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                var isClientError = context.Response.StatusCode is >= 400 and < 500;
                var statusCode = isClientError ? context.Response.StatusCode : (int)HttpStatusCode.InternalServerError;

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = statusCode;

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
                await context.Response.WriteAsync(jsonResponse);
            }
        }
    }
}
