namespace Mpmt.Services.Extensions
{
    public static class HttpClientExtensions
    {
        public static HttpClient SetHeaders(this HttpClient httpClient, IEnumerable<KeyValuePair<string, string>> headers)
        {
            if (headers is null)
                throw new ArgumentNullException(nameof(headers));

            foreach (var headerKv in headers)
            {
                if (httpClient.DefaultRequestHeaders.TryGetValues(headerKv.Key, out var _))
                    httpClient.DefaultRequestHeaders.Remove(headerKv.Key);

                httpClient.DefaultRequestHeaders.Add(headerKv.Key, headerKv.Value);
            }

            return httpClient;
        }

        public static HttpClient SetHeaders(this HttpClient httpClient, Dictionary<string, string> headers)
        {
            if (headers is null)
                throw new ArgumentNullException(nameof(headers));

            foreach (var headerKv in headers)
            {
                if (httpClient.DefaultRequestHeaders.TryGetValues(headerKv.Key, out var _))
                    httpClient.DefaultRequestHeaders.Remove(headerKv.Key);

                httpClient.DefaultRequestHeaders.Add(headerKv.Key, headerKv.Value);
            }

            return httpClient;
        }

        public static string GetRequestUrl(this HttpClient httpClient, string relativePath = "")
        {
            if (httpClient?.BaseAddress is null)
                return default;

            //return new Uri(httpClient.BaseAddress, relativePath).OriginalString;
            return new Uri(httpClient.BaseAddress, relativePath).AbsoluteUri;
        }
    }
}
