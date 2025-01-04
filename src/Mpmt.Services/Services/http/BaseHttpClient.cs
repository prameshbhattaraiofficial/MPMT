using Mpmt.Services.Common.Extensions;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;



namespace Mpmt.Services.Services.http
{
    public abstract class BaseHttpClient : IBaseHttpClient
    {
        public abstract HttpClient CreateHttpClient();

        protected virtual HttpClient SetHeaders(HttpClient httpClient, IEnumerable<KeyValuePair<string, string>> headers)
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

        public virtual async Task<(HttpStatusCode statusCode, T responseType)> DeleteAsync<T>(
            string requestUri, IEnumerable<KeyValuePair<string, string>> headers = null)
        {
            if (string.IsNullOrWhiteSpace(requestUri))
                requestUri = string.Empty;

            var httpClient = CreateHttpClient();

            if (headers is not null)
                httpClient = SetHeaders(httpClient, headers);

            using var response = await httpClient.DeleteAsync(requestUri);

            var responseString = await response.Content.ReadAsStringAsync();
            var responseObj = JsonConvert.DeserializeObject<T>(responseString);

            return (response.StatusCode, responseObj);
        }

        public virtual async Task<(HttpStatusCode statusCode, T responseType)> DeleteAsync<T>(
            string requestUri, HttpContent content, IEnumerable<KeyValuePair<string, string>> headers = null)
        {
            if (string.IsNullOrWhiteSpace(requestUri))
                requestUri = string.Empty;

            using var request = new HttpRequestMessage(HttpMethod.Delete, requestUri);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Content = content;

            var httpClient = CreateHttpClient();
            if (headers is not null)
                httpClient = SetHeaders(httpClient, headers);

            using var response = await httpClient.SendAsync(request);
            var responseString = await response.Content.ReadAsStringAsync();
            var responseObj = JsonConvert.DeserializeObject<T>(responseString);

            return (response.StatusCode, responseObj);
        }

        public virtual async Task<(HttpStatusCode statusCode, T responseType)> GetAsync<T>(
            string requestUri, IEnumerable<KeyValuePair<string, string>> headers = null)
        {
            if (string.IsNullOrWhiteSpace(requestUri))
                requestUri = string.Empty;

            var httpClient = CreateHttpClient();
            if (headers is not null)
                httpClient = SetHeaders(httpClient, headers);

            using var response = await httpClient.GetAsync(requestUri);

            var responseString = await response.Content.ReadAsStringAsync();
            var responseObj = JsonConvert.DeserializeObject<T>(responseString);

            return (response.StatusCode, responseObj);
        }

        public virtual async Task<(HttpStatusCode statusCode, TResponseType responseType, TErrorResponseType errorResponse)> GetAsync<TResponseType, TErrorResponseType>(
            string requestUri, IEnumerable<KeyValuePair<string, string>> headers = null)
        {
            if (string.IsNullOrWhiteSpace(requestUri))
                requestUri = string.Empty;

            var httpClient = CreateHttpClient();
            if (headers is not null)
                httpClient = SetHeaders(httpClient, headers);

            using var response = await httpClient.GetAsync(requestUri);

            var responseString = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                var errResponseObj = JsonConvert.DeserializeObject<TErrorResponseType>(responseString);
                return (response.StatusCode, default, errResponseObj);
            }

            var responseObj = JsonConvert.DeserializeObject<TResponseType>(responseString);
            return (response.StatusCode, responseObj, default);
        }

        public virtual async Task<(HttpStatusCode statusCode, T responseType)> PostAsync<T>(
            string requestUri, HttpContent content, IEnumerable<KeyValuePair<string, string>> headers = null)
        {
            if (string.IsNullOrWhiteSpace(requestUri))
                requestUri = string.Empty;

            var httpClient = CreateHttpClient();
            if (headers is not null)
                httpClient = SetHeaders(httpClient, headers);

            using var response = await httpClient.PostAsync(requestUri, content);

            var responseString = await response.Content.ReadAsStringAsync();
            var responseObj = JsonConvert.DeserializeObject<T>(responseString);

            return (response.StatusCode, responseObj);
        }

        public virtual async Task<(HttpStatusCode statusCode, TResponseType responseType, TErrorResponseType errorResponse)> PostAsync<TResponseType, TErrorResponseType>(
            string requestUri, HttpContent content, IEnumerable<KeyValuePair<string, string>> headers = null)
        {
            if (string.IsNullOrWhiteSpace(requestUri))
                requestUri = string.Empty;

            var httpClient = CreateHttpClient();
            if (headers is not null)
                httpClient = SetHeaders(httpClient, headers);

            using var response = await httpClient.PostAsync(requestUri, content);

            var responseString = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                var errResponseObj = JsonConvert.DeserializeObject<TErrorResponseType>(responseString);
                return (response.StatusCode, default, errResponseObj);
            }

            var responseObj = JsonConvert.DeserializeObject<TResponseType>(responseString);
            return (response.StatusCode, responseObj, default);
        }

        public virtual async Task<(HttpStatusCode statusCode, TResponse responseType)> PostStreamAsync<TSource, TResponse>(
            string requestUri, TSource sourceObj, IEnumerable<KeyValuePair<string, string>> headers = null)
        {
            if (string.IsNullOrWhiteSpace(requestUri))
                requestUri = string.Empty;

            var memoryContentStream = new MemoryStream();
            memoryContentStream.SerializeToJsonAndWrite(sourceObj);
            memoryContentStream.Seek(0, SeekOrigin.Begin);

            using var request = new HttpRequestMessage(HttpMethod.Post, requestUri);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            using var streamContent = new StreamContent(memoryContentStream);
            request.Content = streamContent;

            var httpClient = CreateHttpClient();
            if (headers is not null)
                httpClient = SetHeaders(httpClient, headers);

            using var response = await httpClient.SendAsync(request);
            var responseString = await response.Content.ReadAsStringAsync();
            var responseObj = JsonConvert.DeserializeObject<TResponse>(responseString);

            return (response.StatusCode, responseObj);
        }

        public virtual async Task<(HttpStatusCode statusCode, T responseType)> PutAsync<T>(
            string requestUri, HttpContent content, IEnumerable<KeyValuePair<string, string>> headers = null)
        {
            if (string.IsNullOrWhiteSpace(requestUri))
                requestUri = string.Empty;

            var httpClient = CreateHttpClient();
            if (headers is not null)
                httpClient = SetHeaders(httpClient, headers);

            using var response = await httpClient.PutAsync(requestUri, content);

            var responseString = await response.Content.ReadAsStringAsync();
            var responseObj = JsonConvert.DeserializeObject<T>(responseString);

            return (response.StatusCode, responseObj);
        }

    }
}
