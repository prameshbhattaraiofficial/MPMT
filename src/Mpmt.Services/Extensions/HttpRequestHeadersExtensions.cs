using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace Mpmt.Services.Extensions
{
    public static class HttpRequestHeadersExtensions
    {
        public static Dictionary<string, string> ToDictionary(this HttpRequestHeaders headers)
        {
            var headersDict = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);

            foreach (var headerKvp in headers)
                headersDict.TryAdd(headerKvp.Key, string.Join(';', headerKvp.Value));

            return headersDict;
        }

        public static string ToJsonDictionary(this HttpRequestHeaders headers)
        {
            return JsonConvert.SerializeObject(ToDictionary(headers));
        }
    }
}
