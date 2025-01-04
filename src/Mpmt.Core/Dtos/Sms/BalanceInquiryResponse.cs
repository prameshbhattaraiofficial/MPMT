namespace Mpmt.Core.Dtos.Sms;

public class BalanceInquiryResponse
{
    public string Balance { get; set; }
    public string NtcRate { get; set; } 
    public string NcellRate { get; set; }
    public string SmartcellRate { get; set; }   
}