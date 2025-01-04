using AutoMapper;
using Microsoft.AspNetCore.Http;
using Mpmt.Core;
using Mpmt.Core.Common;
using Mpmt.Core.Common.Helpers;
using Mpmt.Core.Domain;
using Mpmt.Core.Dtos.Partner;
using Mpmt.Core.Dtos.Users;
using Mpmt.Core.ViewModel.ChangePassword;
using Mpmt.Core.ViewModel.ForgotPassword;
using Mpmt.Data.Common;
using Mpmt.Services.Common;
using Mpmts.Core.Dtos;
using System.Security.Claims;

namespace Mpmt.Services.Users
{
    /// <summary>
    /// The user registration service.
    /// </summary>
    public class UserRegistrationService : IUserRegistrationService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly IWebHelper _webHelper;
        private readonly IUsersService _usersService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRegistrationService"/> class.
        /// </summary>
        /// <param name="mapper">The mapper.</param>
        /// <param name="webHelper">The web helper.</param>
        /// <param name="usersService">The users service.</param>
        public UserRegistrationService(IHttpContextAccessor httpContextAccessor,
            IMapper mapper,
            IWebHelper webHelper,
            IUsersService usersService)
        {
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _webHelper = webHelper;
            _usersService = usersService;
        }

        /// <summary>
        /// Changes the password async.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>A Task.</returns>
        public async Task<SprocMessage> ChangePasswordAsync(PartnerChangePasswordVM request)
        {
            var response = new SprocMessage()
            {
                StatusCode = 400,
                MsgText = "Something Went wrong",
            };

            var userType = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "UserType")?.Value;
            var UserName = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
            bool PasswordCheck = false;

            if (userType == "Admin")
            {

                var User = await _usersService.GetUserByUserNameAsync(UserName);
                if (User != null)
                {
                    PasswordCheck = HashUtils.CheckEqualBase64HashHmacSha512(request.OldPassword, User.PasswordHash, User.PasswordSalt);
                }
                var newpasswordsameasold = HashUtils.CheckEqualBase64HashHmacSha512(request.NewPassword, User.PasswordHash, User.PasswordSalt);

                if(newpasswordsameasold){
                    response.StatusCode = 400;
                    response.MsgText = "New Password cant be same as old one";
                    return response;
                }

                if (PasswordCheck)
                {
                    var passwordSalt = CryptoUtils.GenerateKeySalt();
                    var passwordHash = CryptoUtils.HashHmacSha512Base64(request.NewPassword, passwordSalt);

                    var changeRequest = new AppUser()
                    {

                        PasswordHash = passwordHash,
                        PasswordSalt = passwordSalt,
                        UserName = UserName,
                        UpdatedBy = UserName

                    };
                    response = await _usersService.ChangePasswordAsync(changeRequest);

                }
                else
                {
                    response.StatusCode = 400;
                    response.MsgText = "Wrong Password";
                }



                return response;



            }
            return response;
        }

        /// <summary>
        /// Registers the user async.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>A Task.</returns>
        public Task<UserRegistrationResult> RegisterUserAsync(UserRegistrationRequest request)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Validates the user async.
        /// </summary>
        /// <param name="usernameOrEmail">The username or email.</param>
        /// <param name="password">The password.</param>
        /// <returns>A Task.</returns>
        public async Task<(LoginResults, AppUser)> ValidateUserAsync(string usernameOrEmail, string password)
        {
            var user = CommonHelper.IsValidEmail(usernameOrEmail)
                ? await _usersService.GetUserByEmailAsync(usernameOrEmail)
                : await _usersService.GetUserByUserNameAsync(usernameOrEmail);

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

                await _usersService.UpdateUserLoginActivityAsync(userLoginActivity);

                return (LoginResults.WrongPassword, default);
            }

            userLoginActivity.FailedLoginAttempt = 0;
            userLoginActivity.TemporaryLockedTillUtcDate = null;
            userLoginActivity.LastLoginDateUtc = DateTime.UtcNow;
            userLoginActivity.LastIpAddress = _webHelper.GetCurrentIpAddress();

            await _usersService.UpdateUserLoginActivityAsync(userLoginActivity);
            if (string.IsNullOrEmpty(user.AccountSecretKey))
            {
                return (LoginResults.InitiateFactorAuthentication, user);
            }

            return (LoginResults.Successful, user);
        }
        /// <summary>
        /// Updates the account secret key.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="accountsecretkey">The accountsecretkey.</param>
        /// <returns>A string.</returns>
        public void UpdateAccountSecretKey(string email, string accountsecretkey)
        {
            _usersService.UpdateAccountSecretKey(email, accountsecretkey);
        }
        public async Task<SprocMessage> AddLoginOtpAsync(TokenVerification tokenVerification)
        {
            var response = await _usersService.AddLoginOtpAsync(tokenVerification);
            return response;
        }

        public async Task<TokenVerification> GetOtpByUsernameAsync(string UserName)
        {
            var data = await _usersService.GetOtpByUserNameAsync(UserName);
            return data;
        }
        public async void UpdateEmailConfirm(string UserName)
        {
            _usersService.UpdateEmailConfirmAsync(UserName);

        }
        public async Task<AppUser> GetUserByEmail(string Email)
        {
            return await _usersService.GetUserByEmailAsync(Email);
        }

        public async Task<(LoginResults, AppUser)> ValidateUserEmailAsync(string email)
        {
            var user = CommonHelper.IsValidEmail(email)
                ? await _usersService.GetUserByEmailAsync(email)
                : await _usersService.GetUserByUserNameAsync(email);

            if (user is null)
                return (LoginResults.NotExist, default);
            if (user.IsDeleted)
                return (LoginResults.Deleted, default);
            if (!user.IsActive)
                return (LoginResults.NotActive, default);

            //check whether a user is locked out
            if (user.TemporaryLockedTillUtcDate.HasValue && user.TemporaryLockedTillUtcDate.Value > DateTime.UtcNow)
                return (LoginResults.LockedOut, default);

            return (LoginResults.Successful, user);
        }

        public async Task<SprocMessage> ForgotPasswordAsync(PasswordResetModel reset)
        {
            var data = await _usersService.ForgotPasswordAsync(reset);
            return data;
        }

        public async Task<SprocMessage> ResetPasswordAsync(PasswordResetModel request)
        {
            var User = await _usersService.GetUserByUserNameAsync(request.UserName);

            var passwordSalt = CryptoUtils.GenerateKeySalt();
            var passwordHash = CryptoUtils.HashHmacSha512Base64(request.NewPassword, passwordSalt);
                
            var changeRequest = new AppUser()
            {
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                UserName = User.UserName,
                UpdatedBy = User.UserName
            };
            SprocMessage response = await _usersService.ChangePasswordAsync(changeRequest);
            return response;
        }
    }
}
