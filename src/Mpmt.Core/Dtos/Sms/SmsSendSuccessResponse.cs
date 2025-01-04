namespace Mpmt.Core.Dtos.Sms;

public class SmsSendSuccessResponse
{
    public string Message { get; set; }
    public int Status { get; set; }
    public int Ntc { get; set; }
    public int Ncell { get; set; }
    public int Smartcell { get; set; }  
}