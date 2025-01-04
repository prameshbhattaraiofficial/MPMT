using Microsoft.Extensions.Options;
using Mpmt.Core;
using Mpmt.Core.Common;
using Mpmt.Core.Dtos.BankLoad;
using Mpmt.Core.Dtos.PartnerApi;
using Mpmt.Services.Extensions;
using Mpmt.Services.Logging;
using Mpmts.Core.Dtos;
using Newtonsoft.Json;
using System.Net;
using System.Reflection;
using System.Text;

namespace Mpmt.Services.Services.BankLoadApi
{
    public class MyPayBankLoadApiService : IMyPayBankLoadApiService
    {
        private readonly IOptions<MyPayBankLoadApiConfig> _myPayBankLoadApiConfig;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IVendorApiLogger _vendorApiLogger;

        public MyPayBankLoadApiService(
            IOptions<MyPayBankLoadApiConfig> myPayBankLoadApiConfig,
            IHttpClientFactory httpClientFactory,
            IVendorApiLogger vendorApiLogger)
        {
            _myPayBankLoadApiConfig = myPayBankLoadApiConfig;
            _httpClientFactory = httpClientFactory;
            _vendorApiLogger = vendorApiLogger;
        }


        public async Task<(HttpStatusCode, MyPayBankPayoutApiResponse)> BankPayoutAsync(MyPayBankPayoutDto dto)
        {
            var requestObj = new MyPayBankPayoutApiRequest
            {
                Amount = dto.Amount,
                BankCode = dto.BankId,
                AccountNumber = dto.AccountNumber,
                AccountName = dto.AccountHolderName,
                Reference = dto.Reference,
                Description = string.IsNullOrEmpty(dto.Remarks) ? "Bank Transfer" : dto.Remarks,
                UserName = _myPayBankLoadApiConfig.Value.UserName,
                Password = _myPayBankLoadApiConfig.Value.Password,
                MerchantId = _myPayBankLoadApiConfig.Value.MerchantId
            };

            _vendorApiLogger.SetLogContext(transactionId: requestObj.Reference);

            try
            {
                requestObj.AuthTokenString = await GetAuthTokenStringAsync();
                if (string.IsNullOrWhiteSpace(requestObj.AuthTokenString))
                    throw new MpmtException("Failed to generate AuthTokenString.");

                var reqBodyStr = MyPayBankLoadApiHelper.ToJsonString(requestObj);
                var payloadSignature = await GenerateSignatureAsync(reqBodyStr);
                if (string.IsNullOrWhiteSpace(payloadSignature))
                    throw new MpmtException("Failed to generate payload signature.");

                using var httpClient = GetHttpClient();
                httpClient.Timeout = TimeSpan.FromMilliseconds(180000);       ////Maximum 3 min to wait for response
                httpClient.DefaultRequestHeaders.Add("Signature", payloadSignature);
                var reqBodyContent = MyPayBankLoadApiHelper.ToStringContent(reqBodyStr);

                // log request just before sending
                await _vendorApiLogger.LogVendorApiResponse2Async(
                    vendorType2: VendorTypes.BankLoad,
                    vendorId2: VendorsApis.MyPayBankLoad,
                    vendorTrackerId2: requestObj.Reference,
                    vendorRequestURL2: httpClient.GetRequestUrl(MyPayBankLoadApiUris.ValidateBankUserUri),
                    vendorRequestInput2: reqBodyStr,
                    vendorRequestHeaders2: httpClient.DefaultRequestHeaders.ToJsonDictionary());

                using var resMessage = await httpClient.PostAsync(MyPayBankLoadApiUris.BankPayoutUri, reqBodyContent);
                var resBodyStr = await resMessage.Content.ReadAsStringAsync();

                var responseObj = JsonConvert.DeserializeObject<MyPayBankPayoutApiResponse>(resBodyStr);

                // log response after getting response
                await _vendorApiLogger.LogVendorApiResponse2Async(
                    vendorType2: VendorTypes.BankLoad,
                    vendorId2: VendorsApis.MyPayBankLoad,
                    vendorTransactionId2: responseObj.TransactionUniqueId,
                    vendorTrackerId2: requestObj.Reference,
                    vendorRequestURL2: resMessage.RequestMessage?.RequestUri?.AbsoluteUri,
                    vendorRequestInput2: await resMessage.RequestMessage?.Content?.ReadAsStringAsync(),
                    vendorRequestHeaders2: resMessage.RequestMessage?.Headers.ToJsonDictionary(),
                    vendorResponseHttpStatus2: (int)resMessage.StatusCode,
                    vendorResponseStatus2: !string.IsNullOrWhiteSpace(resBodyStr),
                    vendorResponseMessage2: responseObj.responseMessage,
                    vendorResponseOutput2: resBodyStr);

                return (resMessage.StatusCode, responseObj);
            }
            //catch (Exception ex) when (ex is MpmtException or HttpRequestException or InvalidOperationException or TaskCanceledException)
            //{
            //    await _vendorApiLogger.LogVendorApiException2Async(ex.Message, ex.StackTrace);
            //    return (HttpStatusCode.GatewayTimeout, default);
            //}
            catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
            {
                // Specifically handle the timeout scenario
                await _vendorApiLogger.LogVendorApiException2Async(ex.Message, ex.StackTrace);
                return (HttpStatusCode.GatewayTimeout, default);
            }
            catch (TaskCanceledException ex)
            {
                // Handle other TaskCanceledException scenarios
                await _vendorApiLogger.LogVendorApiException2Async(ex.Message, ex.StackTrace);
                return (HttpStatusCode.RequestTimeout, default);
            }
            catch (MpmtException ex)
            {
                await _vendorApiLogger.LogVendorApiException2Async(ex.Message, ex.StackTrace);
                return (HttpStatusCode.GatewayTimeout, default);
            }
            catch (HttpRequestException ex)
            {
                await _vendorApiLogger.LogVendorApiException2Async(ex.Message, ex.StackTrace);
                return (HttpStatusCode.BadGateway, default);
            }
            catch (InvalidOperationException ex)
            {
                await _vendorApiLogger.LogVendorApiException2Async(ex.Message, ex.StackTrace);
                return (HttpStatusCode.InternalServerError, default);
            }
            catch (Exception ex)
            {
                // Handle any other exceptions
                await _vendorApiLogger.LogVendorApiException2Async(ex.Message, ex.StackTrace);
                return (HttpStatusCode.InternalServerError, default);
            }
        }

        public async Task<(HttpStatusCode, MyPayValidateBankUserApiResponse)> ValidateBankUserAsync(MyPayValidateBankUserDto dto)
        {
            var requestObj = new MyPayValidateBankUserApiRequest
            {
                AccountNumber = dto.AccountNumber,
                Amount = dto.Amount,
                BankCode = dto.BankCode,
                AccountName = dto.AccountName,
                Reference = dto.Reference,
                UserName = _myPayBankLoadApiConfig.Value.UserName,
                Password = _myPayBankLoadApiConfig.Value.Password,
                Description = "Bank Load",
                MerchantId = _myPayBankLoadApiConfig.Value.MerchantId
            };

            _vendorApiLogger.SetLogContext(vendorTrackerId: requestObj.Reference);

            try
            {
                requestObj.AuthTokenString = await GetAuthTokenStringAsync();
                if (string.IsNullOrWhiteSpace(requestObj.AuthTokenString))
                    throw new MpmtException("Failed to generate AuthTokenString.");

                var reqBodyStr = MyPayBankLoadApiHelper.ToJsonString(requestObj);
                var payloadSignature = await GenerateSignatureAsync(reqBodyStr);
                if (string.IsNullOrWhiteSpace(payloadSignature))
                    throw new MpmtException("Failed to generate payload signature.");

                using var httpClient = GetHttpClient();
                httpClient.DefaultRequestHeaders.Add("Signature", payloadSignature);
                var reqBodyContent = MyPayBankLoadApiHelper.ToStringContent(reqBodyStr);

                // log request just before sending
                await _vendorApiLogger.LogVendorApiResponseAsync(
                    vendorType: VendorTypes.BankLoad,
                    vendorId: VendorsApis.MyPayBankLoad,
                    vendorTrackerId: requestObj.Reference,
                    vendorRequestURL: httpClient.GetRequestUrl(MyPayBankLoadApiUris.ValidateBankUserUri),
                    vendorRequestInput: reqBodyStr,
                    vendorRequestHeaders: httpClient.DefaultRequestHeaders.ToJsonDictionary());

                using var resMessage = await httpClient.PostAsync(MyPayBankLoadApiUris.ValidateBankUserUri, reqBodyContent);
                var resBodyStr = await resMessage.Content.ReadAsStringAsync();

                var responseObj = JsonConvert.DeserializeObject<MyPayValidateBankUserApiResponse>(resBodyStr);

                // log response after getting response
                await _vendorApiLogger.LogVendorApiResponseAsync(
                    vendorType: VendorTypes.BankLoad,
                    vendorId: VendorsApis.MyPayBankLoad,
                    vendorTrackerId: requestObj.Reference,
                    vendorRequestURL: resMessage.RequestMessage?.RequestUri?.AbsoluteUri,
                    vendorRequestInput: await resMessage.RequestMessage?.Content?.ReadAsStringAsync(),
                    vendorRequestHeaders: resMessage.RequestMessage?.Headers.ToJsonDictionary(),
                    vendorResponseHttpStatus: (int)resMessage.StatusCode,
                    vendorResponseStatus: !string.IsNullOrWhiteSpace(resBodyStr),
                    vendorResponseMessage: responseObj.responseMessage,
                    vendorResponseOutput: resBodyStr);

                return (resMessage.StatusCode, responseObj);
            }
            catch (Exception ex) when (ex is MpmtException or HttpRequestException or InvalidOperationException or TaskCanceledException)
            {
                await _vendorApiLogger.LogVendorApiExceptionAsync(ex.Message, ex.StackTrace);
                return (HttpStatusCode.GatewayTimeout, default);
            }
        }

        public Task validateReferenceNumber(ValidateAccountRequest request)
        {
            throw new NotImplementedException();
        }

       

        private async Task<string> GenerateSignatureAsync(string data)
        {
            try
            {
                var privateCertPath = Path.Combine(
                    Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Certificates\\MyPay\\MYPAY_WALLETLOAD.pem");
                var pemString = await File.ReadAllTextAsync(privateCertPath);
                var privateKey = RsaCryptoUtils.ImportPrivateKeyPem(pemString);
                var signatureBytes = RsaCryptoUtils.GenerateSignature(Encoding.UTF8.GetBytes(data), privateKey);

                return Convert.ToBase64String(signatureBytes);
            }
            catch (Exception)
            {
                // TODO: Log exceptions
                return default;
            }
        }

        private Task<string> GetAuthTokenStringAsync()
        {
            var authTokenString = $"KeyPair:{_myPayBankLoadApiConfig.Value.UserName}:{_myPayBankLoadApiConfig.Value.Password}";
            return GenerateSignatureAsync(authTokenString);
        }

        private HttpClient GetHttpClient()
        {
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.BaseAddress = new Uri(_myPayBankLoadApiConfig.Value.BaseUrl);
            httpClient.DefaultRequestHeaders.Add("User-Agent", "HttpClient");
            httpClient.DefaultRequestHeaders.Add("API_KEY", _myPayBankLoadApiConfig.Value.ApiKey);
            httpClient.Timeout = TimeSpan.FromMilliseconds(60000);
            return httpClient;
        }
    }
}
