using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Mpmt.Services.Extensions
{
    public static class HeaderDictionaryExtensions
    {
        public static Dictionary<string, string> ToDictionary(this IHeaderDictionary headers)
        {
            var headersDict = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);

            foreach (var headerKvp in headers)
                headersDict.TryAdd(headerKvp.Key, string.Join(';', headerKvp.Value));

            return headersDict;
        }

        public static string ToJsonDictionary(this IHeaderDictionary headers)
        {
            return JsonConvert.SerializeObject(headers.ToDictionary());
        }
    }
}
