using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Mpmt.Core.Dtos.ActivityLog;
using System.Security.Claims;
using System.Text;

namespace Mpmt.Core.Common.Helpers
{
    /// <summary>
    /// The log helper.
    /// </summary>
    public class LogHelper
    {
        /// <summary>
        /// Gets the user activity log async.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="hostEnv">The host env.</param>
        /// <param name="logRequestBody">If true, log request body.</param>
        /// <param name="userAction">The user action.</param>
        /// <returns>A Task.</returns>
        [Obsolete]
        public static async Task<UserActivityLogParam> GetUserActivityLogAsync(HttpContext context, IHostingEnvironment hostEnv = null, bool logRequestBody = true, string userAction = "")
        {
            var remoteIpAddress = GetIpAddress(context);
            var httpMethod = context.Request.Method;
            var requestUrl = GetRequestUrl(context);
            var queryString = GetQueryString(context);
            var userAgent = GetUserAgent(context);
            var headers = GetRequestHeaders(context);
            var controllerName = GetController(context);
            var actionName = GetAction(context);
            var environment = hostEnv?.EnvironmentName ?? null;

            var activityLogParam = new UserActivityLogParam
            {
                RequestUrl = requestUrl,
                QueryString = string.IsNullOrWhiteSpace(queryString) ? null : queryString,
                Environment = environment,
                RemoteIpAddress = remoteIpAddress,
                HttpMethod = httpMethod,
                ControllerName = controllerName,
                ActionName = actionName,
                UserAgent = userAgent,
                Headers = headers,
                MachineName = Environment.MachineName,
                UserAction = userAction
            };

            if (logRequestBody)
            {
                var (isForm, requestBody) = await GetRequestBodyAsString(context);
                activityLogParam.IsFormData = isForm;
                activityLogParam.RequestBody = requestBody;
            }

            if (context.User.Identity.IsAuthenticated)
            {
                var userName = GetUsername(context);
                var email = GetUserEmail(context);


                activityLogParam.UserName = userName;
                activityLogParam.Email = email;
                activityLogParam.IsCustomer = false;
            }

            return activityLogParam;
        }

        /// <summary>
        /// Gets the exception log async.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <param name="httpContext">The http context.</param>
        /// <param name="hostEnv">The host env.</param>
        /// <returns>A Task.</returns>
        [Obsolete]
        public static async Task<AppExceptionLogParam> GetExceptionLogAsync(Exception ex, HttpContext httpContext = null, IHostingEnvironment hostEnv = null)
        {
            var appExceptionParam = new AppExceptionLogParam
            {
                ExceptionMessage = ex.Message,
                ExceptionStackTrace = ex.StackTrace,
                ExceptionType = ex.GetType().FullName,
                InnerExceptionMessage = ex.InnerException?.Message,
                InnerExceptionStackTrace = ex.InnerException?.StackTrace,
                MachineName = Environment.MachineName,
                Environment = hostEnv?.EnvironmentName ?? null
            };

            if (httpContext is not null)
            {
                var remoteIPAddress = GetIpAddress(httpContext);
                //var requestUrl = $"{httpContext.Request.PathBase.Value ?? ""}{httpContext.Request.Path}";
                var requestUrl = GetRequestUrl(httpContext);
                var queryString = GetQueryString(httpContext);
                var httpMethod = httpContext.Request.Method;
                var userAgent = GetUserAgent(httpContext);
                var requestHeaders = GetRequestHeaders(httpContext);
                var userName = GetUsername(httpContext);
                var (isForm, requestBody) = await GetRequestBodyAsString(httpContext);

                appExceptionParam.RemoteIpAddress = remoteIPAddress;
                appExceptionParam.RequestUrl = requestUrl;
                appExceptionParam.QueryString = string.IsNullOrWhiteSpace(queryString) ? null : queryString;
                appExceptionParam.HttpMethod = httpMethod;
                appExceptionParam.UserAgent = userAgent;
                appExceptionParam.Headers = requestHeaders;
                appExceptionParam.UserName = userName;

                if (isForm) appExceptionParam.FormData = requestBody;
                else appExceptionParam.RequestBody = requestBody;
            }

            return appExceptionParam;
        }

        // Get request body in string format; skips form files
        /// <summary>
        /// Gets the request body as string.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>A Task.</returns>
        private static async Task<(bool isForm, string body)> GetRequestBodyAsString(HttpContext context)
        {
            if (context is null)
                throw new ArgumentNullException(nameof(context));

            if (!context.Request.Body.CanRead)
                return (false, null);

            context.Request.EnableBuffering();

            string requestBody;
            if (context.Request.HasFormContentType)
            {
                requestBody = GetRequestBodyFormData(context);
                return (true, requestBody);
            }

            var buffer = new byte[Convert.ToInt32(context.Request.ContentLength)];
            _ = await context.Request.Body.ReadAsync(buffer.AsMemory(0, buffer.Length));
            requestBody = Encoding.UTF8.GetString(buffer);
            context.Request.Body.Position = 0;

            return (false, requestBody);
        }

        /// <summary>
        /// Gets the username.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>A string.</returns>
        private static string GetUsername(HttpContext context)
        {
            return context.User?.FindFirst(
                c => c.Type == ClaimTypes.Name)?.Value ?? context.User?.Identity?.Name;
        }

        /// <summary>
        /// Gets the user email.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>A string.</returns>
        private static string GetUserEmail(HttpContext context)
        {
            return context.User?.FindFirst(c => c.Type == ClaimTypes.Email)?.Value;
        }

        /// <summary>
        /// Roles the name.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>A string.</returns>
        private static string RoleName(HttpContext context)
        {
            return context.User?.FindFirst(c => c.Type == ClaimTypes.Role)?.Value;
        }

        /// <summary>
        /// Gets the request body form data.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>A string.</returns>
        private static string GetRequestBodyFormData(HttpContext context)
        {
            var form = context.Request.Form;
            var formData = new List<string>();
            foreach (var key in form.Keys)
            {
                if (form[key].Count > 1)
                {
                    // If there are multiple values for the same key, add them all to the list.
                    foreach (var value in form[key])
                    {
                        if (!string.IsNullOrEmpty(value) && !IsFile(value))
                            formData.Add($"{key}={value}");
                    }
                }
                else
                {
                    // If there is only one value for the key, add it to the list.
                    var value = form[key];
                    if (!string.IsNullOrEmpty(value) && !IsFile(value))
                        formData.Add($"{key}={value}");
                }
            }

            return string.Join("&", formData);
        }

        /// <summary>
        /// Gets the request headers.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>A string.</returns>
        private static string GetRequestHeaders(HttpContext context)
        {
            var headers = new List<string>();

            foreach (var header in context.Request.Headers)
                headers.Add($"{header.Key}: {header.Value}");

            return string.Join("; ", headers);
        }

        /// <summary>
        /// Gets the ip address.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>A string.</returns>
        private static string GetIpAddress(HttpContext context)
        {
            return context.Connection.RemoteIpAddress?.ToString();
        }

        /// <summary>
        /// Gets the controller.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>A string.</returns>
        private static string GetController(HttpContext context)
        {
            //return context.Request.RouteValues["controller"]?.ToString();

            var routeData = context.GetRouteData();
            var controllerName = routeData.Values["controller"]?.ToString();
            return controllerName;

        }

        /// <summary>
        /// Gets the action.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>A string.</returns>
        private static string GetAction(HttpContext context)
        {
            // return context.Request.RouteValues["action"]?.ToString();
            var routeData = context.GetRouteData();
            var ActionName = routeData.Values["action"]?.ToString();
            return ActionName;
        }

        /// <summary>
        /// Gets the query string.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>A string.</returns>
        private static string GetQueryString(HttpContext context)
        {
            return context.Request.QueryString.HasValue ? context.Request.QueryString.Value : string.Empty;
        }

        /// <summary>
        /// Gets the user agent.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>A string.</returns>
        private static string GetUserAgent(HttpContext context)
        {
            context.Request.Headers.TryGetValue("User-Agent", out var userAgent);
            return userAgent;
        }

        /// <summary>
        /// Gets the request url.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>A string.</returns>
        private static string GetRequestUrl(HttpContext context)
        {
            return $"{context.Request.Scheme}://{context.Request.Host}{context.Request.Path}{context.Request.QueryString}";
        }

        /// <summary>
        /// Are the file.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>A bool.</returns>
        private static bool IsFile(string value)
        {
            if (string.IsNullOrEmpty(value))
                return false;

            if (value.StartsWith("filename=", StringComparison.OrdinalIgnoreCase) || value.StartsWith("content-type=", StringComparison.OrdinalIgnoreCase))
                return true;

            return false;
        }
    }
}
