using Microsoft.Extensions.Options;
using Mpmt.Core;
using Mpmt.Core.Common;
using Mpmt.Core.Dtos.WalletLoad.MyPay;
using Mpmt.Services.Extensions;
using Mpmt.Services.Logging;
using Newtonsoft.Json;
using System.Net;
using System.Reflection;
using System.Text;

namespace Mpmt.Services.Services.WalletLoadApi.MyPay
{
    public class MyPayWalletLoadApiService : IMyPayWalletLoadApiService
    {
        private readonly IOptions<MyPayWalletLoadApiConfig> _myPayWalletLoadApiConfig;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IVendorApiLogger _vendorApiLogger;

        public MyPayWalletLoadApiService(
            IOptions<MyPayWalletLoadApiConfig> myPayWalletLoadApiConfig,
            IHttpClientFactory httpClientFactory,
            IVendorApiLogger vendorApiLogger)
        {
            _myPayWalletLoadApiConfig = myPayWalletLoadApiConfig;
            _httpClientFactory = httpClientFactory;
            _vendorApiLogger = vendorApiLogger;
        }

        public async Task<(HttpStatusCode, MyPayWalletPayoutCheckTransactionStatusApiResponse)> CheckTransactionStatusAsync(
            MyPayWalletPayoutCheckTransactionStatusDto dto)
        {
            var requestObj = new MyPayWalletPayoutCheckTransactionStatusApiRequest
            {
                Reference = dto.Reference,
                TransactionId = dto.TransactionId,
                TransactionReference = dto.TransactionReference,
                UserName = _myPayWalletLoadApiConfig.Value.UserName,
                Password = _myPayWalletLoadApiConfig.Value.Password,
                MerchantId = _myPayWalletLoadApiConfig.Value.MerchantId
            };

            _vendorApiLogger.SetLogContext(transactionId: requestObj.TransactionReference);

            try
            {
                requestObj.AuthTokenString = await GetAuthTokenStringAsync();
                if (string.IsNullOrWhiteSpace(requestObj.AuthTokenString))
                    throw new MpmtException("Failed to generate AuthTokenString.");

                var reqBodyStr = MyPayWalletLoadApiHelper.ToJsonString(requestObj);
                var payloadSignature = await GenerateSignatureAsync(reqBodyStr);
                if (string.IsNullOrWhiteSpace(payloadSignature))
                    throw new MpmtException("Failed to generate payload signature.");

                using var httpClient = GetHttpClient();
                httpClient.DefaultRequestHeaders.Add("Signature", payloadSignature);
                var reqBodyContent = MyPayWalletLoadApiHelper.ToStringContent(reqBodyStr);

                // log request just before sending
                await _vendorApiLogger.LogVendorApiResponse3Async(
                    vendorType3: VendorTypes.WalletLoad,
                    vendorId3: VendorsApis.MyPayWalletLoad,
                    vendorTransactionId3: requestObj.TransactionId,
                    vendorTrackerId3: requestObj.Reference,
                    vendorRequestURL3: httpClient.GetRequestUrl(MyPayWalletLoadApiUris.ValidateWalletUserUri),
                    vendorRequestInput3: reqBodyStr,
                    vendorRequestHeaders3: httpClient.DefaultRequestHeaders.ToJsonDictionary());

                using var resMessage = await httpClient.PostAsync(MyPayWalletLoadApiUris.CheckTransactionStatusUri, reqBodyContent);
                var resBodyStr = await resMessage.Content.ReadAsStringAsync();

                var responseObj = JsonConvert.DeserializeObject<MyPayWalletPayoutCheckTransactionStatusApiResponse>(resBodyStr);

                // log response after getting response
                await _vendorApiLogger.LogVendorApiResponse3Async(
                    vendorType3: VendorTypes.WalletLoad,
                    vendorId3: VendorsApis.MyPayWalletLoad,
                    vendorTransactionId3: requestObj.TransactionId,
                    vendorTrackerId3: requestObj.Reference,
                    vendorRequestURL3: resMessage.RequestMessage?.RequestUri?.AbsoluteUri,
                    vendorRequestInput3: await resMessage.RequestMessage?.Content?.ReadAsStringAsync(),
                    vendorRequestHeaders3: resMessage.RequestMessage?.Headers.ToJsonDictionary(),
                    vendorResponseHttpStatus3: (int)resMessage.StatusCode,
                    vendorResponseStatus3: !string.IsNullOrWhiteSpace(resBodyStr),
                    vendorResponseMessage3: responseObj.ResponseMessage,
                    vendorResponseOutput3: resBodyStr);

                return (resMessage.StatusCode, responseObj);
            }
            catch (Exception ex) when (ex is MpmtException or HttpRequestException or InvalidOperationException or TaskCanceledException)
            {
                await _vendorApiLogger.LogVendorApiException3Async(ex.Message, ex.StackTrace);
                return (HttpStatusCode.InternalServerError, default);
            }
        }

        public async Task<(HttpStatusCode, MyPayValidateWalletUserApiResponse)> ValidateWalletUserAsync(MyPayValidateWalletUserDto dto)
        {
            var requestObj = new MyPayValidateWalletUserApiRequest
            {
                ContactNumber = dto.WalletNumber,
                Reference = dto.Reference,
                UserName = _myPayWalletLoadApiConfig.Value.UserName,
                Password = _myPayWalletLoadApiConfig.Value.Password,
                MerchantId = _myPayWalletLoadApiConfig.Value.MerchantId
            };

            _vendorApiLogger.SetLogContext(vendorTrackerId: requestObj.Reference);

            try
            {
                requestObj.AuthTokenString = await GetAuthTokenStringAsync();
                if (string.IsNullOrWhiteSpace(requestObj.AuthTokenString))
                    throw new MpmtException("Failed to generate AuthTokenString.");

                var reqBodyStr = MyPayWalletLoadApiHelper.ToJsonString(requestObj);
                var payloadSignature = await GenerateSignatureAsync(reqBodyStr);
                if (string.IsNullOrWhiteSpace(payloadSignature))
                    throw new MpmtException("Failed to generate payload signature.");

                using var httpClient = GetHttpClient();
                httpClient.DefaultRequestHeaders.Add("Signature", payloadSignature);
                var reqBodyContent = MyPayWalletLoadApiHelper.ToStringContent(reqBodyStr);

                // log request just before sending
                await _vendorApiLogger.LogVendorApiResponseAsync(
                    vendorType: VendorTypes.WalletLoad,
                    vendorId: VendorsApis.MyPayWalletLoad,
                    vendorTrackerId: requestObj.Reference,
                    vendorRequestURL: httpClient.GetRequestUrl(MyPayWalletLoadApiUris.ValidateWalletUserUri),
                    vendorRequestInput: reqBodyStr,
                    vendorRequestHeaders: httpClient.DefaultRequestHeaders.ToJsonDictionary());

                using var resMessage = await httpClient.PostAsync(MyPayWalletLoadApiUris.ValidateWalletUserUri, reqBodyContent);
                var resBodyStr = await resMessage.Content.ReadAsStringAsync();

                var responseObj = JsonConvert.DeserializeObject<MyPayValidateWalletUserApiResponse>(resBodyStr);

                // log response after getting response
                await _vendorApiLogger.LogVendorApiResponseAsync(
                    vendorType: VendorTypes.WalletLoad,
                    vendorId: VendorsApis.MyPayWalletLoad,
                    vendorTrackerId: requestObj.Reference,
                    vendorRequestURL: resMessage.RequestMessage?.RequestUri?.AbsoluteUri,
                    vendorRequestInput: await resMessage.RequestMessage?.Content?.ReadAsStringAsync(),
                    vendorRequestHeaders: resMessage.RequestMessage?.Headers.ToJsonDictionary(),
                    vendorResponseHttpStatus: (int)resMessage.StatusCode,
                    vendorResponseStatus: !string.IsNullOrWhiteSpace(resBodyStr),
                    vendorResponseMessage: responseObj.ResponseMessage,
                    vendorResponseOutput: resBodyStr);

                return (resMessage.StatusCode, responseObj);
            }
            catch (Exception ex) when (ex is MpmtException or HttpRequestException or InvalidOperationException or TaskCanceledException)
            {
                await _vendorApiLogger.LogVendorApiExceptionAsync(ex.Message, ex.StackTrace);
                return (HttpStatusCode.GatewayTimeout, default);
            }
        }

        public async Task<(HttpStatusCode, MyPayWalletPayoutApiResponse)> WalletPayoutAsync(MyPayWalletPayoutDto dto)
        {
            var requestObj = new MyPayWalletPayoutApiRequest
            {
                Amount = dto.Amount,
                ContactNumber = dto.ContactNumber,
                Reference = dto.Reference,
                Remarks = dto.Remarks,
                UserName = _myPayWalletLoadApiConfig.Value.UserName,
                Password = _myPayWalletLoadApiConfig.Value.Password,
                MerchantId = _myPayWalletLoadApiConfig.Value.MerchantId
            };

            _vendorApiLogger.SetLogContext(transactionId: requestObj.Reference);

            try
            {
                requestObj.AuthTokenString = await GetAuthTokenStringAsync();
                if (string.IsNullOrWhiteSpace(requestObj.AuthTokenString))
                    throw new MpmtException("Failed to generate AuthTokenString.");

                var reqBodyStr = MyPayWalletLoadApiHelper.ToJsonString(requestObj);
                var payloadSignature = await GenerateSignatureAsync(reqBodyStr);
                if (string.IsNullOrWhiteSpace(payloadSignature))
                    throw new MpmtException("Failed to generate payload signature.");

                using var httpClient = GetHttpClient();
                httpClient.DefaultRequestHeaders.Add("Signature", payloadSignature);
                var reqBodyContent = MyPayWalletLoadApiHelper.ToStringContent(reqBodyStr);

                // log request just before sending
                await _vendorApiLogger.LogVendorApiResponse2Async(
                    vendorType2: VendorTypes.WalletLoad,
                    vendorId2: VendorsApis.MyPayWalletLoad,
                    vendorTrackerId2: requestObj.Reference,
                    vendorRequestURL2: httpClient.GetRequestUrl(MyPayWalletLoadApiUris.ValidateWalletUserUri),
                    vendorRequestInput2: reqBodyStr,
                    vendorRequestHeaders2: httpClient.DefaultRequestHeaders.ToJsonDictionary());

                using var resMessage = await httpClient.PostAsync(MyPayWalletLoadApiUris.WalletPayoutUri, reqBodyContent);
                var resBodyStr = await resMessage.Content.ReadAsStringAsync();

                var responseObj = JsonConvert.DeserializeObject<MyPayWalletPayoutApiResponse>(resBodyStr);

                // log response after getting response
                await _vendorApiLogger.LogVendorApiResponse2Async(
                    vendorType2: VendorTypes.WalletLoad,
                    vendorId2: VendorsApis.MyPayWalletLoad,
                    vendorTransactionId2 : responseObj.MerchantWallet_TransactionId,
                    vendorTrackerId2: requestObj.Reference,
                    vendorRequestURL2: resMessage.RequestMessage?.RequestUri?.AbsoluteUri,
                    vendorRequestInput2: await resMessage.RequestMessage?.Content?.ReadAsStringAsync(),
                    vendorRequestHeaders2: resMessage.RequestMessage?.Headers.ToJsonDictionary(),
                    vendorResponseHttpStatus2: (int)resMessage.StatusCode,
                    vendorResponseStatus2: !string.IsNullOrWhiteSpace(resBodyStr),
                    vendorResponseMessage2: responseObj.ResponseMessage,
                    vendorResponseOutput2: resBodyStr);

                return (resMessage.StatusCode, responseObj);
            }
            catch (Exception ex) when (ex is MpmtException or HttpRequestException or InvalidOperationException or TaskCanceledException)
            {
                await _vendorApiLogger.LogVendorApiException2Async(ex.Message, ex.StackTrace);
                return (HttpStatusCode.GatewayTimeout, default);
            }
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
            var authTokenString = $"KeyPair:{_myPayWalletLoadApiConfig.Value.UserName}:{_myPayWalletLoadApiConfig.Value.Password}";
            return GenerateSignatureAsync(authTokenString);
        }

        private HttpClient GetHttpClient()
        {
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.BaseAddress = new Uri(_myPayWalletLoadApiConfig.Value.BaseUrl);
            httpClient.DefaultRequestHeaders.Add("User-Agent", "HttpClient");
            httpClient.DefaultRequestHeaders.Add("API_KEY", _myPayWalletLoadApiConfig.Value.ApiKey);

            return httpClient;
        }
    }
}
