using Mpmt.Core.Dtos.Sms;
using Mpmt.Services.Services.Common;
using Mpmt.Services.Services.http.Sms;

namespace Mpmt.Services.Services.Sms;

public class SmsService : BaseService, ISmsService
{
    private readonly ISmsHttpClient _smsHttpClient;

    public SmsService(ISmsHttpClient smsHttpClient)
    {
        _smsHttpClient = smsHttpClient;
    }

    public async Task<(BalanceInquiryResponse, SociairErrorResponse)> BalanceInquiryAsync()
    {
        var (_, successResponse, errorResponse) = await _smsHttpClient.GetAsync<BalanceInquiryResponse, SociairErrorResponse>(SmsServiceUris.SociairBalanceInquiry);
        return (successResponse, errorResponse);
    }

    public async Task<bool> SendAsync(string message, string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
            throw new Exception($"{nameof(phoneNumber)} is empty");
        var smsRequest = new SmsSendRequest() { Message = message ?? string.Empty, Mobile = phoneNumber };
        var requestBodyContent = GetJsonStringContent(smsRequest);
        var (_, successResponse, _) = await _smsHttpClient.PostAsync<SmsSendSuccessResponse, SociairErrorResponse>(SmsServiceUris.SociairSms, requestBodyContent);
        if (successResponse is null)
            return false;
        return true;
    }

    public async Task<bool> SendAsync(string message, params string[] phoneNumber)
    {
        if (!phoneNumber.Any())
            throw new Exception($"{nameof(phoneNumber)} is empty");
        var phoneNumbers = string.Join(",", phoneNumber);
        var smsRequest = new SmsSendRequest() { Message = message ?? string.Empty, Mobile = phoneNumbers };
        var requestBodyContent = GetJsonStringContent(smsRequest);
        var (_, successResponse, _) = await _smsHttpClient.PostAsync<SmsSendSuccessResponse, SociairErrorResponse>(SmsServiceUris.SociairSms, requestBodyContent);
        if (successResponse is null)
            return false;
        return true;
    }
}