using Microsoft.Extensions.Options;
using Mpmt.Core.Configuration;

namespace Mpmt.Services.Services.http.Testhttp
{
    public class MypayClient : BaseHttpClient, IMypayClient
    {
        private readonly MypayConfig _testHttpConfig;
        private readonly IHttpClientFactory _httpClientFactory;


        public MypayClient(IHttpClientFactory httpClientFactory, IOptions<MypayConfig> testHttpConfig)
        {
            _httpClientFactory = httpClientFactory;
            _testHttpConfig = testHttpConfig.Value;
        }

        public override HttpClient CreateHttpClient()
        {
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.BaseAddress = new Uri(_testHttpConfig.BaseUrl);
            httpClient.DefaultRequestHeaders.Add("API_KEY", _testHttpConfig.ApiKey);

            return httpClient;
        }
    }
}
