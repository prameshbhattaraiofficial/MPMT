using AutoMapper;
using Microsoft.Extensions.Configuration;
using Mpmt.Core;
using Mpmt.Core.Common;
using Mpmt.Core.Common.Helpers;
using Mpmt.Core.Domain;
using Mpmt.Core.Dtos.Partner;
using Mpmt.Core.Dtos.PartnerEmployee;
using Mpmt.Core.Dtos.Users;
using Mpmt.Data.Repositories.Employee;
using Mpmt.Services.Common;
using Mpmt.Services.Partner.IService;
using Mpmt.Services.Users;
using Mpmts.Core.Dtos;

namespace Mpmt.Services.Partner
{
    /// <summary>
    /// The partner registration service.
    /// </summary>
    public class PartnerRegistrationService : IPartnerRegistrationService
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IWebHelper _webHelper;
        private readonly IEmployeeRepo _employeeRepo;
        private readonly IPartnerService _partnerService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PartnerRegistrationService"/> class.
        /// </summary>
        /// <param name="mapper">The mapper.</param>
        /// <param name="webHelper">The web helper.</param>
        /// <param name="partnerService">The partner service.</param>
        public PartnerRegistrationService(IMapper mapper,
            IConfiguration configuration,
            IWebHelper webHelper,
            IEmployeeRepo employeeRepo,
            IPartnerService partnerService)
        {
            _mapper = mapper;
            _configuration = configuration;
            _webHelper = webHelper;
            _employeeRepo = employeeRepo;
            _partnerService = partnerService;
        }
        /// <summary>
        /// Changes the password async.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>A Task.</returns>
        public Task<ChangePasswordResult> ChangePasswordAsync(ChangePasswordRequest request)
        {
            throw new NotImplementedException();
        }
        public async Task<string> CheckPartnerOrEmployee(string usernameOrEmail)
        {
            var userType = CommonHelper.IsValidEmail(usernameOrEmail)
                ? await _employeeRepo.CheckPartnerOrEmployeeByEmail(usernameOrEmail)
                : await _employeeRepo.CheckPartnerOrEmployeeByUserName(usernameOrEmail);
            return userType.ToString();
        }

        public async Task<(LoginResults, AppPartner)> ValidatePartnerEmailAsync(string email)
        {
            var user = CommonHelper.IsValidEmail(email)
                ? await _partnerService.GetPartnerByEmailAsync(email)
                : await _partnerService.GetPartnerByUserNameAsync(email);

            if (user is null)
                return (LoginResults.NotExist, default);
            if (user.IsDeleted)
                return (LoginResults.Deleted, default);
            if (!user.IsActive)
                return (LoginResults.NotActive, default);

            // Case: check if user is in at least one user role (i.e. registered user)

            // check whether a user is locked out
            if (!string.IsNullOrEmpty(user.TemporaryLockedTillUtcDate))
            {
                if (DateTime.Parse(user.TemporaryLockedTillUtcDate) > DateTime.UtcNow)
                    return (LoginResults.LockedOut, default);
            }

            return (LoginResults.Successful, user);
        }

        public async Task<(LoginResults, AppPartner)> ValidatePartnerAsync(string usernameOrEmail, string password)
        {
            var user = CommonHelper.IsValidEmail(usernameOrEmail)
                ? await _partnerService.GetPartnerByEmailAsync(usernameOrEmail)
                : await _partnerService.GetPartnerByUserNameAsync(usernameOrEmail);


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

            // check whether a user is locked out
            if (!string.IsNullOrEmpty(user.TemporaryLockedTillUtcDate))
            {
                if (DateTime.Parse(user.TemporaryLockedTillUtcDate) > DateTime.UtcNow)
                    return (LoginResults.LockedOut, default);
            }


            var userLoginActivity = _mapper.Map<UserLoginActivity>(user);
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

                await _partnerService.UpdatePartnerLoginActivityAsync(userLoginActivity);

                return (LoginResults.WrongPassword, default);
            }

            userLoginActivity.FailedLoginAttempt = 0;
            userLoginActivity.TemporaryLockedTillUtcDate = null;
            userLoginActivity.LastLoginDateUtc = DateTime.UtcNow;
            userLoginActivity.LastIpAddress = _webHelper.GetCurrentIpAddress();


            await _partnerService.UpdatePartnerLoginActivityAsync(userLoginActivity);

            //check if securitykey is null
            if (string.IsNullOrEmpty(user.AccountSecretKey))
            {
                return (LoginResults.InitiateFactorAuthentication, user);
            }
            //check if firstlogin attempt success

            return (LoginResults.Successful, user);
        }

        public async Task<(LoginResults, AppPartnerEmployee)> ValidatePartnerEmployeeEmailAsync(string email)
        {
            var user = CommonHelper.IsValidEmail(email)
                ? await _employeeRepo.GetPartnerEmployeeByEmailAsync(email)
                : await _employeeRepo.GetPartnerEmployeeByUserNameAsync(email);

            if (user is null)
                return (LoginResults.NotExist, default);
            if (user.IsDeleted)
                return (LoginResults.Deleted, default);
            if (!user.IsActive)
                return (LoginResults.NotActive, default);

            // Case: check if user is in at least one user role (i.e. registered user)

            // check whether a user is locked out
            if (!string.IsNullOrEmpty(user.TemporaryLockedTillUtcDate))
            {
                if (DateTime.Parse(user.TemporaryLockedTillUtcDate) > DateTime.UtcNow)
                    return (LoginResults.LockedOut, default);
            }

            return (LoginResults.Successful, user);
        }

        public async Task<(LoginResults, AppPartnerEmployee)> ValidatePartnerEmployeeAsync(string usernameOrEmail, string password)
        {
            var user = CommonHelper.IsValidEmail(usernameOrEmail)
                ? await _employeeRepo.GetPartnerEmployeeByEmailAsync(usernameOrEmail)
                : await _employeeRepo.GetPartnerEmployeeByUserNameAsync(usernameOrEmail);



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

            // check whether a user is locked out
            if (!string.IsNullOrEmpty(user.TemporaryLockedTillUtcDate))
            {
                if (DateTime.Parse(user.TemporaryLockedTillUtcDate) > DateTime.UtcNow)
                    return (LoginResults.LockedOut, default);
            }


            var userLoginActivity = _mapper.Map<UserLoginActivity>(user);
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

                await _employeeRepo.UpdatePartnerEmployeeLoginActivityAsync(userLoginActivity);

                return (LoginResults.WrongPassword, default);
            }

            userLoginActivity.FailedLoginAttempt = 0;
            userLoginActivity.TemporaryLockedTillUtcDate = null;
            userLoginActivity.LastLoginDateUtc = DateTime.UtcNow;
            userLoginActivity.LastIpAddress = _webHelper.GetCurrentIpAddress();


            await _employeeRepo.UpdatePartnerEmployeeLoginActivityAsync(userLoginActivity);

            var test = _configuration["LoginSetting:2FAcheckRequired"];
            //check if securitykey is null
            if (string.IsNullOrEmpty(user.AccountSecretKey))
            {
                return (LoginResults.InitiateFactorAuthentication, user);
            }
            //check if firstlogin attempt success

            return (LoginResults.Successful, user);
        }
        /// <summary>
        /// Updates the account secret key.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="Accountsecretkey">The accountsecretkey.</param>
        public void UpdateAccountSecretKey(string email, string Accountsecretkey)
        {
            _partnerService.UpdateAccountSecretKey(email, Accountsecretkey);

        }


        public async Task<SprocMessage> AddLoginOtpAsync(TokenVerification tokenVerification)
        {
            var response = await _partnerService.AddLoginOtpAsync(tokenVerification);
            return response;
        }

        public async Task<TokenVerification> GetOtpBypartnerCodeAsync(string partnercode, string UserName, string OtpVerificationFor)
        {
            var data = await _partnerService.GetOtpBypartnerCodeAsync(partnercode, UserName, OtpVerificationFor);
            return data;
        }
        public async void UpdateEmailConfirm(string partnercode)
        {
            _partnerService.UpdateEmailConfirmAsync(partnercode);

        }

        public Task<bool> VerifyEmailConfirmed(string Email)
        {
            //var response = await _partnerRepo.VerifyShortnameAsync(Shortname);
            //return response;
            throw new NotImplementedException();
        }
    }
}
