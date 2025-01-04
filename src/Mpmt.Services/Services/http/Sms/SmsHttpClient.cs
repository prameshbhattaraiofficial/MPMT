using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;

namespace Mpmt.Services.Services.http.Sms;

public class SmsHttpClient : BaseHttpClient, ISmsHttpClient
{
    private readonly IConfiguration _configuration;
    private readonly IHttpClientFactory _httpClientFactory;

    public SmsHttpClient(IConfiguration configuration, IHttpClientFactory httpClientFactory)
    {
        _configuration = configuration;
        _httpClientFactory = httpClientFactory;
    }

    public override HttpClient CreateHttpClient()
    {
        var merchantApiBaseUrl = _configuration["Sms:Sociair:BaseUrl"];
        if (string.IsNullOrWhiteSpace(merchantApiBaseUrl))
            throw new Exception("Base URL not found");
        var merchantAuthToken = _configuration["Sms:Sociair:AuthToken"];
        if (string.IsNullOrWhiteSpace(merchantAuthToken))
            throw new Exception("Auth token not found");
        var httpClient = _httpClientFactory.CreateClient();
        httpClient.BaseAddress = new Uri(merchantApiBaseUrl);
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", merchantAuthToken);
        return httpClient;
    }
}