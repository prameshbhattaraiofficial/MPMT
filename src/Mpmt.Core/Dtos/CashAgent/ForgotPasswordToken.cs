namespace Mpmt.Core.Dtos.CashAgent;

public class ForgotPasswordToken
{
    public string PhoneNumber { get; set; }
    public string OTP { get; set; }
    public string AgentCode { get; set; }
    public string ResetToken { get; set; }
    public bool IsConsumed { get; set; }
}
