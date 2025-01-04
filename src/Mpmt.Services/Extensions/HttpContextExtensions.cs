using Microsoft.AspNetCore.Http;
using Mpmt.Core.Domain.Partners;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text;
using UAParser;

namespace Mpmt.Services.Extensions
{
    public static class HttpContextExtensions
    {
        // Get request body in string format; skips form files
        public static async Task<(bool isForm, string body)> GetRequestBodyAsStringAsync(this HttpContext context)
        {
            if (context is null)
                throw new ArgumentNullException(nameof(context));

            if (!context.Request.Body.CanRead)
                return (false, null);

            string requestBody;
            if (context.Request.HasFormContentType)
            {
                requestBody = context.GetRequestBodyFormDataAsString();
                //requestBody = context.GetRequestBodyFormDataAsJsonDictionaryString();
                return (true, requestBody);
            }

            var buffer = await context.GetRequestBodyAsByteArrayAsync();
            requestBody = Encoding.UTF8.GetString(buffer);

            return (false, requestBody);
        }

        public static async Task<string> GetResponseBodyAsStringAsync(this HttpContext context)
        {
            using var memoryStream = new MemoryStream();
            await context.Response.Body.CopyToAsync(memoryStream);
            var responseBodyAsString = Encoding.UTF8.GetString(memoryStream.ToArray());

            return responseBodyAsString;
        }

        public static string GetAuthorizationToken(this HttpContext context)
        {
            if (context.Request.Headers.TryGetValue("Authorization", out var token))
                return token;

            return default;
        }

        public static string GetUserName(this HttpContext context)
        {
            return context.User?.FindFirst(c => c.Type == ClaimTypes.Name)?.Value
                ?? context.User?.FindFirst(c => c.Type == PartnerClaimTypes.UserName)?.Value
                ?? context.User?.Identity?.Name;
        }

        public static string GetUserEmail(this HttpContext context)
        {
            return context.User?.FindFirst(c => c.Type == ClaimTypes.Email)?.Value;
        }

        public static string GetUserType(this HttpContext context)
        {
            return context.User?.FindFirst(c => c.Type == "UserType")?.Value
                ?? context.User?.FindFirst(c => c.Type == PartnerClaimTypes.UserType)?.Value;
        }

        public static string GetPartnerCode(this HttpContext context)
        {
            return context.User?.FindFirst(c => c.Type == "PartnerCode")?.Value
                ?? context.User?.FindFirst(c => c.Type == PartnerClaimTypes.PartnerCode)?.Value;
        }

        public static async Task<byte[]> GetRequestBodyAsByteArrayAsync(this HttpContext context)
        {
            context.Request.EnableBuffering();

            var buffer = new byte[Convert.ToInt32(context.Request.ContentLength)];
            _ = await context.Request.Body.ReadAsync(buffer.AsMemory(0, buffer.Length));

            context.Request.Body.Position = 0;

            return buffer;
        }

        public static string GetParsedUserAgent(this HttpContext context, string userAgent = null)
        {
            userAgent ??= context.GetUserAgent();

            if (userAgent is null)
                return null;

            try
            {
                var uaParser = Parser.GetDefault();
                ClientInfo c = uaParser.Parse(userAgent);

                return $"{c.UA.Family} on {c.OS.Family}";
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static string GetUserAgentDevicePlatform(this HttpContext context, string userAgent = null)
        {
            userAgent ??= context.GetUserAgent();

            if (userAgent is null)
                return null;

            try
            {
                var uaParser = Parser.GetDefault();
                ClientInfo c = uaParser.Parse(userAgent);

                return $"{c.OS.Family} {c.OS.Major ?? string.Empty}";
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static string GetRequestBodyFormDataAsString(this HttpContext context)
        {
            context.Request.EnableBuffering();

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

            context.Request.Body.Position = 0;

            return string.Join("&", formData);
        }

        /// <summary>
        /// Convert request form data to dictionary. NOTE: It skips form files.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static Dictionary<string, List<string>> GetRequestBodyFormDataAsDictionary(this HttpContext context)
        {
            context.Request.EnableBuffering();

            var formDictionary = new Dictionary<string, List<string>>();

            var form = context.Request.Form;
            if (form is null)
                return formDictionary;

            foreach (var key in form.Keys)
            {
                var values = new List<string>();
                if (form[key].Count > 1)
                {
                    // If there are multiple values for the same key, add them all to the list.
                    foreach (var value in form[key])
                    {
                        if (IsFile(value))
                            values.Add(string.Empty);
                        else
                            values.Add(value);
                    }
                }
                else
                {
                    // If there is only one value for the key, add it to the list.
                    var value = form[key];
                    if (IsFile(value))
                        values.Add(string.Empty);
                    else
                        values.Add(value);
                }

                formDictionary.Add(key, values);
            }

            context.Request.Body.Position = 0;

            return formDictionary;
        }

        /// <summary>
        /// Convert request form data to JSON dictionary string. NOTE: It skips form files.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string GetRequestBodyFormDataAsJsonDictionaryString(this HttpContext context)
        {
            var formDict = context.GetRequestBodyFormDataAsDictionary();
            return JsonConvert.SerializeObject(formDict, new JsonSerializerSettings { ContractResolver = null, Formatting = Formatting.None });
        }

        public static string GetRequestHeaders(this HttpContext context)
        {
            var headers = new List<string>();

            foreach (var header in context.Request.Headers)
                headers.Add($"{header.Key}: {header.Value}");

            return string.Join("; ", headers);
        }

        public static string GetRequestHeadersAsDictionaryJson(this HttpContext context)
        {
            var headersDict = new Dictionary<string, string>();

            foreach (var header in context.Request.Headers)
                headersDict.Add(header.Key, header.Value);

            return JsonConvert.SerializeObject(headersDict);
        }

        public static string GetIpAddress(this HttpContext context)
        {
            return context.Connection.RemoteIpAddress?.ToString();
        }

        public static string GetController(this HttpContext context)
        {
            return context.Request.RouteValues["controller"]?.ToString();
        }

        public static string GetAction(this HttpContext context)
        {
            return context.Request.RouteValues["action"]?.ToString();
        }

        public static string GetQueryString(this HttpContext context)
        {
            return context.Request.QueryString.HasValue ? context.Request.QueryString.Value : string.Empty;
        }

        public static string GetUserAgent(this HttpContext context)
        {
            context.Request.Headers.TryGetValue("User-Agent", out var userAgent);
            return userAgent;
        }

        public static string GetRequestUrl(this HttpContext context)
        {
            return $"{context.Request.Scheme}://{context.Request.Host}{context.Request.Path}{context.Request.QueryString}";
        }

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
