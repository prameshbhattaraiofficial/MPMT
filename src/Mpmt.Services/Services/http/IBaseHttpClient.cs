using System.Net;

namespace Mpmt.Services.Services.http
{
    public interface IBaseHttpClient
    {

        HttpClient CreateHttpClient();
        Task<(HttpStatusCode statusCode, T responseType)> GetAsync<T>(
            string requestUri, IEnumerable<KeyValuePair<string, string>> headers = null);
        Task<(HttpStatusCode statusCode, TResponseType responseType, TErrorResponseType errorResponse)>
            GetAsync<TResponseType, TErrorResponseType>(string requestUri, IEnumerable<KeyValuePair<string, string>> headers = null);
        Task<(HttpStatusCode statusCode, T responseType)> PostAsync<T>(
            string requestUri, HttpContent content, IEnumerable<KeyValuePair<string, string>> headers = null);
        Task<(HttpStatusCode statusCode, TResponseType responseType, TErrorResponseType errorResponse)> PostAsync<TResponseType, TErrorResponseType>(
            string requestUri, HttpContent content, IEnumerable<KeyValuePair<string, string>> headers = null);
        Task<(HttpStatusCode statusCode, TResponse responseType)> PostStreamAsync<TSource, TResponse>(
            string requestUri, TSource sourceObj, IEnumerable<KeyValuePair<string, string>> headers = null);
        Task<(HttpStatusCode statusCode, T responseType)> PutAsync<T>(
            string requestUri, HttpContent content, IEnumerable<KeyValuePair<string, string>> headers = null);
        Task<(HttpStatusCode statusCode, T responseType)> DeleteAsync<T>(
            string requestUri, IEnumerable<KeyValuePair<string, string>> headers = null);
        Task<(HttpStatusCode statusCode, T responseType)> DeleteAsync<T>(
            string requestUri, HttpContent content, IEnumerable<KeyValuePair<string, string>> headers = null);
    }
}
