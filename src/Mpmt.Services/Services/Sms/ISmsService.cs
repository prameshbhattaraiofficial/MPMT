using Mpmt.Core.Dtos.Sms;

namespace Mpmt.Services.Services.Sms;

public interface ISmsService
{
    Task<(BalanceInquiryResponse, SociairErrorResponse)> BalanceInquiryAsync();
    Task<bool> SendAsync(string message, string phoneNumber);
    Task<bool> SendAsync(string message, params string[] phoneNumber);
}