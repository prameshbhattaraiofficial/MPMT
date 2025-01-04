using Mpmt.Core.Domain;
using Mpmt.Core.Dtos.CashAgent;
using Mpmts.Core.Dtos;

namespace Mpmt.Services.CashAgents;

public interface IAgentRegistrationService
{
    Task<(LoginResults, AgentUser)> ValidateAgentAsync(string usernameOrPhoneNumber, string password);
    Task<(LoginResults, AgentUser)> ValidateAgentEmployeeAsync(string usernameOrPhoneNumber, string password);
    Task<(LoginResults, AgentUser)> ValidateEmployeePhoneAsync(string phoneNumber);
    Task<(LoginResults, AgentUser)> ValidateAgentPhoneAsync(string phoneNumber);
    Task<bool> SendOtpVerification(AgentUser user);
    Task<VerificationToken> GetOtpByAgentCodeAsync(string agentCode, string UserName, string phoneNumber);
    Task<SprocMessage> ChangeAgentPassword(ForgotPassword changepassword);
    Task<SprocMessage> ForgotPasswordAsync(ForgotPasswordToken reset);
    Task<SprocMessage> ResetTokenValidationAsync(ForgotPasswordToken reset);
    Task UpdateAccountSecretKeyAsync(string AgentCode, string accountsecretkey);
    Task<SprocMessage> RegisterAgent(SignUpAgent data);
    Task<SprocMessage> ValidateOtp(OtpValidationAgent validate);
    Task<AgentDetailSignUp> GetRegisterAgent(OtpValidationAgent validate);
    Task<AgentDetailSignUp> GetRegisterAgentByID(string Id);
    Task<SprocMessage> RegisterAgentStep2(SignUpAgentStep2 data);
}
