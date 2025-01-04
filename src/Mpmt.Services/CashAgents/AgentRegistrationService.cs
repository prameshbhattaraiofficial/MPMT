using AutoMapper;
using Mpmt.Core;
using Mpmt.Core.Common;
using Mpmt.Core.Common.Helpers;
using Mpmt.Core.Domain;
using Mpmt.Core.Dtos.CashAgent;
using Mpmt.Services.Common;
using Mpmts.Core.Dtos;
using Mpmt.Core.Dtos.Partner;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Mpmt.Data.Repositories.Partner;
using Mpmt.Services.Services.Sms;
using Mpmt.Data.Common;
using Mpmt.Data.Repositories.CashAgent;
using Mpmt.Core.Domain.Agents;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using Mpmt.Core.ViewModel.CashAgent;
using Microsoft.AspNetCore.Mvc;

namespace Mpmt.Services.CashAgents;

public class AgentRegistrationService : IAgentRegistrationService
{
    private readonly ICashAgentEmployee _cashAgentEmployee;
    private readonly IMapper _mapper;
    private readonly ICashAgentRepository _cashAgentRepository;
    private readonly IPartnerRepository _partnerRepository;
    private readonly ICashAgentEmployeeRepository _employeeRepository;
    private readonly IWebHelper _webHelper;
    private readonly IConfiguration _config;
    private readonly IFileProviderService _fileProviderService;
    private readonly ISmsService _smsService;
    private readonly ICashAgentUserService _cashAgentUserService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ClaimsPrincipal _loggedInUser;

    public AgentRegistrationService(ICashAgentEmployee cashAgentEmployee, IMapper mapper,
         IWebHelper webHelper,
        ICashAgentUserService cashAgentUserService, IHttpContextAccessor httpContextAccessor, IPartnerRepository partnerRepository, ISmsService smsService, ICashAgentRepository cashAgentRepository, ICashAgentEmployeeRepository employeeRepository, IFileProviderService fileProviderService, IConfiguration config)
    {
        _cashAgentEmployee = cashAgentEmployee;
        _mapper = mapper;
        _webHelper = webHelper;
        _cashAgentUserService = cashAgentUserService;
        _httpContextAccessor = httpContextAccessor;
        _loggedInUser = _httpContextAccessor.HttpContext?.User;
        _partnerRepository = partnerRepository;
        _smsService = smsService;
        _cashAgentRepository = cashAgentRepository;
        _employeeRepository = employeeRepository;
        _fileProviderService = fileProviderService;
        _config = config;
    }

    public async Task<SprocMessage> ChangeAgentPassword(ForgotPassword changepassword)
    {
        var response = new SprocMessage()
        {
            StatusCode = 400,
            MsgText = "Something Went wrong",
        };

        var userType = _loggedInUser.FindFirstValue("UserType");
        var agentCode = _loggedInUser.FindFirstValue("AgentCode");
        var UserName = _loggedInUser.FindFirstValue(ClaimTypes.Name);

        if (userType == "Agent" || userType == "SuperAgent")
        {
            var agent = await _cashAgentRepository.GetCashAgentByAgentCodeAsync(agentCode);
            if (agent == null)
            {
                response.StatusCode = 400;
                response.MsgText = "Invalid Credentials";
                return response;
            }

            var AgentCode = agent.AgentCode;

            var newpasswordsameasold = HashUtils.CheckEqualBase64HashHmacSha512(changepassword.NewPassword, agent.PasswordHash, agent.PasswordSalt);

            if (newpasswordsameasold)
            {
                response.StatusCode = 400;
                response.MsgText = "New Password cant be same as old one";
                return response;
            }

            var passwordSalt = CryptoUtils.GenerateKeySalt();
            var passwordHash = CryptoUtils.HashHmacSha512Base64(changepassword.NewPassword, passwordSalt);

            var request = new CashAgentUser()
            {
                Event = "CP",
                UserName = UserName,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                UserType = userType,
                AgentCode = AgentCode,
                LoginUserId = "1",
                LoginUserName = UserName
            };
            response = await _cashAgentRepository.AgentChangePasswordAsync(request);

            return response;
        }
        else if (userType == "Employee")
        {
            var employeeId = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "EmployeeId")?.Value;
            var employee = await _employeeRepository.GetAgentEmployeeUserByEmployeeIdAsync(employeeId, agentCode);
            if (employee == null)
            {
                response.StatusCode = 400;
                response.MsgText = "Invalid Credentials";
                return response;
            }
            var AgentCode = employee.AgentCode;

            var newpasswordsameasold = HashUtils.CheckEqualBase64HashHmacSha512(changepassword.NewPassword, employee.PasswordHash, employee.PasswordSalt);

            if (newpasswordsameasold)
            {
                response.StatusCode = 400;
                response.MsgText = "New Password cant be same as old one";
                return response;
            }

            var passwordSalt = CryptoUtils.GenerateKeySalt();
            var passwordHash = CryptoUtils.HashHmacSha512Base64(changepassword.NewPassword, passwordSalt);

            var request = new CashAgentUser()
            {
                Event = "CP",
                EmployeeId = employeeId,
                UserName = employee.UserName,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                UserType = userType,
                AgentCode = AgentCode,
                LoginUserId = employeeId,
                LoginUserName = UserName
            };
            response = await _employeeRepository.AgentEmployeeChangePasswordAsync(request);

            return response;
        }
        response.StatusCode = 400;
        response.MsgText = "Permission Denied";
        return response;
    }

    public async Task<SprocMessage> ForgotPasswordAsync(ForgotPasswordToken reset)
    {
        var UsersStatus = await _cashAgentEmployee.ForgotPasswordAsync(reset);
        return UsersStatus;
    }

    public async Task<VerificationToken> GetOtpByAgentCodeAsync(string agentCode, string UserName, string phoneNumber)
    {
        var data = await _cashAgentEmployee.GetOtpByAgentCodeAsync(agentCode, UserName, phoneNumber);
        return data;
    }

    public async Task<AgentDetailSignUp> GetRegisterAgent(OtpValidationAgent validate)
    {
        var response = await _cashAgentRepository.GetRegisterAgent(validate);
        return response;
    }

    public async Task<AgentDetailSignUp> GetRegisterAgentByID(string Id)
    {
        var response = await _cashAgentRepository.GetAgentDetailById(Id);
        return response;
    }

    public async Task<SprocMessage> RegisterAgent(SignUpAgent data)
    {
        var passwordSalt = CryptoUtils.GenerateKeySalt();
        var passwordHash = CryptoUtils.HashHmacSha512Base64(data.Password, passwordSalt);

        var registerDetail = _mapper.Map<RegisterAgent>(data);
        registerDetail.Event = "I";
        registerDetail.OtpExipiryDate = DateTime.UtcNow.AddMinutes(2);
        registerDetail.FormNumber = 1;
        registerDetail.UserName = data.UserName;
        registerDetail.PasswordSalt = passwordSalt;
        registerDetail.PasswordHash = passwordHash;

        var response = await _cashAgentRepository.RegisterAgent(registerDetail);
        return response;
    }

    public async Task<SprocMessage> RegisterAgentStep2(SignUpAgentStep2 data)
    {
        var registerDetail = _mapper.Map<RegisterAgent>(data);
        registerDetail.Event = "U";
        registerDetail.FormNumber = 2;
        var CompanyLogoImgPath = data.CompanyLogoImagePath;
        var LicenseDocumentImagePath = string.Empty;

        if (data.LicenseDocument != null & data.LicenseDocument.Count > 0)
        {
            foreach (var image in data.LicenseDocument)
            {
                var (isvalidlicenseImage, _) = await FileValidatorUtils.IsValidImageAsync(image, FileTypes.ImageFiles);
                if (!isvalidlicenseImage)
                {
                    return new SprocMessage() { StatusCode = 400, MsgType = "Error", MsgText = "Invalid ID License image." };
                }
                var folderPath = _config["Folder:AgentLicenseDocument"];
                _fileProviderService.TryUploadFile(image, folderPath, out LicenseDocumentImagePath);
                registerDetail.LicensedocImgPath.Add(LicenseDocumentImagePath);
            }
        }

        if (data.DocumentImage is not null)
        {
            if (data.DocumentImage.Length > 0)
            {
                var folderPaths = _config["Folder:CashAgentCompanyLogo"];
                _fileProviderService.TryUploadFile(data.DocumentImage, folderPaths, out CompanyLogoImgPath);
            }
        }

        if (data.DocumentImagePaths is not null && data.DocumentImagePaths.Count > 0)
        {
            foreach (var image in data.DocumentImagePaths)
            {
                registerDetail.LicensedocImgPath.Add(image);
            }
        }

        registerDetail.CompanyLogoImgPath = CompanyLogoImgPath;
        var response = await _cashAgentRepository.RegisterAgent(registerDetail);
        if (response.StatusCode != 200)
        {
            try
            {
                if (!string.IsNullOrEmpty(LicenseDocumentImagePath))
                {
                    string imgPath = LicenseDocumentImagePath.Substring(1);
                    string pths = _config["Static:UserDataDirectory"];
                    string existingImage = Path.Combine(pths + imgPath);
                    File.Delete(existingImage);
                }
                if (!string.IsNullOrEmpty(CompanyLogoImgPath))
                {
                    string imgPath = CompanyLogoImgPath.Substring(1);
                    string pths = _config["Static:UserDataDirectory"];
                    string existingImage = Path.Combine(pths + imgPath);
                    File.Delete(existingImage);
                }
                if (registerDetail.LicensedocImgPath != null & registerDetail.LicensedocImgPath.Count > 0)
                {
                    foreach (var img in registerDetail.LicensedocImgPath)
                    {
                        string imgPath = img.Substring(1);
                        string pths = _config["Static:UserDataDirectory"];
                        string existingImage = Path.Combine(pths + imgPath);
                        File.Delete(existingImage);
                    }
                }
                return response;
            }
            catch (Exception ex)
            {
            }
        }
        return response;
    }

    public async Task<SprocMessage> ResetTokenValidationAsync(ForgotPasswordToken reset)
    {
        var UsersStatus = await _cashAgentEmployee.ResetTokenValidationAsync(reset);
        return UsersStatus;
    }

    public async Task<bool> SendOtpVerification(AgentUser user)
    {
        var otp = OtpGeneration.GenerateRandom6DigitCode();
        var message = $"MyPay Money Transfer password reset verification OTP code for your Mobile is: {otp}. DO NOT SHARE your Password/OTP with anyone.";
        var addToken = new TokenVerification
        {
            UserId = user.EmployeeId != 0 ? user.EmployeeId : 1,
            PartnerCode = user.AgentCode,
            UserName = user.UserName,
            Email = user.Email,
            Mobile = user.ContactNumber,
            CountryCallingCode = "+977",
            VerificationCode = otp,
            VerificationType = "SMS",
            OtpVerificationFor = "Reset-Password",
            SendToEmail = false,
            SendToMobile = true,
            IsConsumed = false,
            ExpiredDate = DateTime.Now.AddMinutes(2),
        };
        var response = await _partnerRepository.AddLoginOtpAsync(addToken);
        if (response.StatusCode == 200)
        {
            var data = await _smsService.SendAsync(message, user.ContactNumber);
            return data;
            //return true;
        }
        return false;
    }

    public async Task UpdateAccountSecretKeyAsync(string AgentCode, string accountsecretkey)
    {
        await _cashAgentUserService.UpdateAccountSecretKeyAsync(AgentCode, accountsecretkey);
    }

    public async Task<(LoginResults, AgentUser)> ValidateAgentAsync(string usernameOrPhoneNumber, string password)
    {
        var user = CommonHelper.IsValidPin(usernameOrPhoneNumber, 10)
            ? await _cashAgentUserService.GetAgentByPhoneNumberAsync(usernameOrPhoneNumber)
            : await _cashAgentUserService.GetAgentByUserNameAsync(usernameOrPhoneNumber);

        if (user is null)
            return (LoginResults.NotExist, default);
        if (user.IsDeleted)
            return (LoginResults.Deleted, default);
        if (!user.IsActive)
            return (LoginResults.NotActive, default);

        if (user.LastPasswordChangedUtcDate.Value.AddDays(MpmtUserDefaults.PasswordChangeExpiryLimit + MpmtUserDefaults.PasswordChangeExpiryDayLength) < DateTime.UtcNow)
        {
            return (LoginResults.PasswordExpired, default);
        }

        // Case: check if user is in at least one user role (i.e. registered user)

        //check whether a user is locked out
        if (user.TemporaryLockedTillUtcDate.HasValue && user.TemporaryLockedTillUtcDate.Value > DateTime.UtcNow)
            return (LoginResults.LockedOut, default);

        var userLoginActivity = _mapper.Map<AgentLoginActivity>(user);
        if (!HashUtils.CheckEqualBase64HashHmacSha512(password, user.PasswordHash, user.PasswordSalt))
        {
            // wrong password
            userLoginActivity.FailedLoginAttempt++;
            if (user.FailedLoginAttempt > 0 && user.FailedLoginAttempt >= MpmtUserDefaults.UserMaxFailedAllowedLoginAttempts)
            {
                // lockout
                userLoginActivity.TemporaryLockedTillUtcDate = DateTime.UtcNow.AddMinutes(MpmtUserDefaults.FailedPasswordLockoutMinutes);

                //reset the counter
                userLoginActivity.FailedLoginAttempt = 0;
            }

            await _cashAgentUserService.UpdateAgentLoginActivityAsync(userLoginActivity);

            return (LoginResults.WrongPassword, default);
        }

        userLoginActivity.FailedLoginAttempt = 0;
        userLoginActivity.TemporaryLockedTillUtcDate = null;
        userLoginActivity.LastLoginDateUtc = DateTime.UtcNow;
        userLoginActivity.IpAddress = _webHelper.GetCurrentIpAddress();

        await _cashAgentUserService.UpdateAgentLoginActivityAsync(userLoginActivity);
        if (string.IsNullOrEmpty(user.AccountSecretKey))
        {
            return (LoginResults.InitiateFactorAuthentication, user);
        }

        return (LoginResults.Successful, user);
    }

    public async Task<(LoginResults, AgentUser)> ValidateAgentEmployeeAsync(string usernameOrPhoneNumber, string password)
    {
        var user = CommonHelper.IsValidPin(usernameOrPhoneNumber, 10)
           ? await _cashAgentEmployee.GetAgentEmployeeByPhoneNumberAsync(usernameOrPhoneNumber)
           : await _cashAgentEmployee.GetAgentEmployeeByUserNameAsync(usernameOrPhoneNumber);

        if (user is null)
            return (LoginResults.NotExist, default);
        if (user.IsDeleted)
            return (LoginResults.Deleted, default);
        if (!user.IsActive)
            return (LoginResults.NotActive, default);

        if (user.LastPasswordChangedUtcDate.Value.AddDays(MpmtUserDefaults.PasswordChangeExpiryLimit + MpmtUserDefaults.PasswordChangeExpiryDayLength) < DateTime.UtcNow)
        {
            return (LoginResults.PasswordExpired, default);
        }

        // Case: check if user is in at least one user role (i.e. registered user)

        //check whether a user is locked out
        if (user.TemporaryLockedTillUtcDate.HasValue && user.TemporaryLockedTillUtcDate.Value > DateTime.UtcNow)
            return (LoginResults.LockedOut, default);

        var userLoginActivity = _mapper.Map<AgentLoginActivity>(user);
        if (!HashUtils.CheckEqualBase64HashHmacSha512(password, user.PasswordHash, user.PasswordSalt))
        {
            // wrong password
            userLoginActivity.FailedLoginAttempt++;
            if (user.FailedLoginAttempt > 0 && user.FailedLoginAttempt >= MpmtUserDefaults.UserMaxFailedAllowedLoginAttempts)
            {
                // lockout
                userLoginActivity.TemporaryLockedTillUtcDate = DateTime.UtcNow.AddMinutes(MpmtUserDefaults.FailedPasswordLockoutMinutes);

                //reset the counter
                userLoginActivity.FailedLoginAttempt = 0;
            }

            await _cashAgentEmployee.UpdateAgentEmployeeLoginActivityAsync(userLoginActivity, user.EmployeeId);

            return (LoginResults.WrongPassword, default);
        }

        userLoginActivity.FailedLoginAttempt = 0;
        userLoginActivity.TemporaryLockedTillUtcDate = null;
        userLoginActivity.LastLoginDateUtc = DateTime.UtcNow;
        userLoginActivity.IpAddress = _webHelper.GetCurrentIpAddress();

        await _cashAgentEmployee.UpdateAgentEmployeeLoginActivityAsync(userLoginActivity, user.EmployeeId);
        if (string.IsNullOrEmpty(user.AccountSecretKey))
        {
            return (LoginResults.InitiateFactorAuthentication, user);
        }

        return (LoginResults.Successful, user);
    }

    public async Task<(LoginResults, AgentUser)> ValidateAgentPhoneAsync(string phoneNumber)
    {
        var user = await _cashAgentUserService.GetAgentByPhoneNumberAsync(phoneNumber);
        if (user is null)
            return (LoginResults.NotExist, default);
        if (user.IsDeleted)
            return (LoginResults.Deleted, default);
        if (!user.IsActive)
            return (LoginResults.NotActive, default);
        if (user.IsBlocked)
            return (LoginResults.Blocked, default);
        if (user.TemporaryLockedTillUtcDate > DateTime.UtcNow)
            return (LoginResults.LockedOut, default);
        return (LoginResults.Successful, user);
    }

    public async Task<(LoginResults, AgentUser)> ValidateEmployeePhoneAsync(string phoneNumber)
    {
        var user = await _cashAgentEmployee.GetAgentEmployeeByPhoneNumberAsync(phoneNumber);
        if (user is null)
            return (LoginResults.NotExist, default);
        if (user.IsDeleted)
            return (LoginResults.Deleted, default);
        if (!user.IsActive)
            return (LoginResults.NotActive, default);
        if (user.IsBlocked)
            return (LoginResults.Blocked, default);
        if (user.TemporaryLockedTillUtcDate > DateTime.UtcNow)
            return (LoginResults.LockedOut, default);
        return (LoginResults.Successful, user);
    }

    public async Task<SprocMessage> ValidateOtp(OtpValidationAgent validate)
    {
        var response = await _cashAgentRepository.ValidateOtpAsync(validate);
        return response;
    }
}
