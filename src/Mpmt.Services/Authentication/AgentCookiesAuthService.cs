using Google.Authenticator;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Configuration;
using Mpmt.Core;
using Mpmt.Core.Dtos.CashAgent;
using Mpmt.Data.Repositories.CashAgent;
using System.Security.Claims;

namespace Mpmt.Services.Authentication
{
    public class AgentCookiesAuthService : IAgentCookiesAuthService
    {
        private readonly ICashAgentEmployeeRepository _cashAgentEmployee;
        private readonly ICashAgentRepository _cashAgentRepository;
        private readonly IConfiguration _configuration;
        private readonly IActionContextAccessor _actionContextAccessor;
        private readonly IUrlHelperFactory _urlHelperFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private bool _result = false;

        public AgentCookiesAuthService(
            ICashAgentEmployeeRepository cashAgentEmployee,
            ICashAgentRepository cashAgentRepository,
            IConfiguration configuration,
            IActionContextAccessor actionContextAccessor,
            IUrlHelperFactory urlHelperFactory,
            IHttpContextAccessor httpContextAccessor)
        {
            _cashAgentEmployee = cashAgentEmployee;
            _cashAgentRepository = cashAgentRepository;
            _configuration = configuration;
            _actionContextAccessor = actionContextAccessor;
            _urlHelperFactory = urlHelperFactory;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> NormalAgentSignInAsync(AgentUser user, string RoleName)
        {
            ArgumentNullException.ThrowIfNull(nameof(user));

            //create claims for user's username and email
            var claims = new List<Claim>();

            if (!string.IsNullOrWhiteSpace(user.UserName))
                claims.Add(new Claim(ClaimTypes.Name, user.UserName, ClaimValueTypes.String, MpmtAuthDefaults.ClaimsIssuer));

            if (!string.IsNullOrWhiteSpace(user.Email))
                claims.Add(new Claim(ClaimTypes.Email, user.Email, ClaimValueTypes.Email, MpmtAuthDefaults.ClaimsIssuer));

            if (!string.IsNullOrWhiteSpace(user.ContactNumber))
                claims.Add(new Claim(ClaimTypes.MobilePhone, user.ContactNumber, ClaimValueTypes.String, MpmtAuthDefaults.ClaimsIssuer));

            claims.Add(new Claim("AgentCode", user.AgentCode.ToString()));
            claims.Add(new Claim(MpmtClaimTypes.UniqueId, user.UserGuid.ToString()));
            claims.Add(new Claim(MpmtClaimTypes.LastChanged, DateTime.UtcNow.ToString()));

            if (!string.IsNullOrWhiteSpace(user.UserType))
                claims.Add(new Claim("UserType", user.UserType));

            claims.Add(new Claim("Is2FAAuthenticated", Convert.ToString(user.Is2FAAuthenticated)));
            claims.Add(new Claim(ClaimTypes.Role, RoleName, ClaimValueTypes.String, MpmtAuthDefaults.ClaimsIssuer));

            //add Roles in clames if needed

            //create principal for the current authentication scheme
            var userIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var userPrincipal = new ClaimsPrincipal(userIdentity);

            //set value indicating whether session is persisted and the time at which the authentication was issued
            var authenticationProperties = new AuthenticationProperties
            {
                IsPersistent = false,
                IssuedUtc = DateTime.UtcNow
            };

            //sign in
            await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal, authenticationProperties);

            return true;
        }

        public async Task<IActionResult> SignInAsync(AgentUser user, string returnUrl, string Code, bool isPersistent = false)
        {
            bool IsValidSecretCode = false;
            ArgumentNullException.ThrowIfNull(nameof(user));

            var urlHelper = _urlHelperFactory.GetUrlHelper(_actionContextAccessor.ActionContext);

            if (!string.IsNullOrEmpty(Code.Trim()))
            {
                var twoFactorAuthenticator = new TwoFactorAuthenticator();
                if (!user.Is2FARequired)
                    _result = true;
                else
                {
                    IsValidSecretCode = twoFactorAuthenticator.ValidateTwoFactorPIN(user.AccountSecretKey, Code.Trim());
                    if (IsValidSecretCode)
                        await _cashAgentRepository.UpdateIs2FAAuthenticatedAsync(user.AgentCode, true);
                }

                if (_configuration["LoginSetting:2FAcheckRequired"] == "0" && Code == _configuration["LoginSetting:2FACode"])
                {
                    _result = true;
                }
                else
                {
                    _result = IsValidSecretCode;
                }
                if (!_result)
                {
                    return new RedirectResult("/login/index?error=Invalid Code");
                }
            }

            //if (!user.ContactNumberConfirmed)
            //{
            //    var login = await NormalAdminSignInAsync(user, "AdminEmailValidateor");
            //    if (login)
            //    {
            //        var otp = OtpGeneration.GenerateRandom6DigitCode();

            //        var mailRequest = new MailRequestModel
            //        {
            //            MailFor = "confirm-email",
            //            MailTo = user.Email,
            //            MailSubject = "Your One-Time Password (OTP) for Registration",
            //            RecipientName = "",
            //            Content = GenrateMailBodyforOtp(otp)

            //        };
            //        var mailServiceModel = await _mailService.EmailSettings(mailRequest);

            //        Thread email = new(delegate ()
            //        {
            //            _mailService.SendMail(mailServiceModel);
            //        });
            //        email.Start();

            //        var addtoken = new TokenVerification
            //        {
            //            UserId = user.Id,
            //            UserName = user.UserName,
            //            Email = user.Email,
            //            VerificationCode = otp,
            //            VerificationType = "E",
            //            SendToEmail = true,
            //            SendToMobile = false,
            //            IsConsumed = false,
            //            ExpiredDate = DateTime.Now.AddMinutes(2)
            //        };

            //        var response = await _usersRepository.AddLoginOtpAsync(addtoken);

            //        return new RedirectResult("admin/login/TokenVerification");
            //    }
            //}

            //check if ip have been changed

            //var authenticatedUser = await GetAuthenticatedUserAsync();
            //if (authenticatedUser is not null || !_result)
            //{
            //    await SignOutAsync();
            //    return new RedirectToRouteResult("areas", new { controller = "Login", action = "Index" });
            //}

            //create claims for user's username and email
            var claims = new List<Claim>();

            if (!string.IsNullOrWhiteSpace(user.UserName))
                claims.Add(new Claim(ClaimTypes.Name, user.UserName, ClaimValueTypes.String, MpmtAuthDefaults.ClaimsIssuer));

            if (!string.IsNullOrWhiteSpace(user.Email))
                claims.Add(new Claim(ClaimTypes.Email, user.Email, ClaimValueTypes.Email, MpmtAuthDefaults.ClaimsIssuer));

            if (!string.IsNullOrWhiteSpace(user.ContactNumber))
                claims.Add(new Claim(ClaimTypes.MobilePhone, user.ContactNumber, ClaimValueTypes.String, MpmtAuthDefaults.ClaimsIssuer));

            claims.Add(new Claim("AgentCode", user.AgentCode.ToString()));
            claims.Add(new Claim(MpmtClaimTypes.UniqueId, user.UserGuid.ToString()));
            claims.Add(new Claim(MpmtClaimTypes.LastChanged, DateTime.UtcNow.ToString()));

            if (!string.IsNullOrWhiteSpace(user.UserType))
                claims.Add(new Claim("UserType", user.UserType));

            claims.Add(new Claim("Is2FAAuthenticated", Convert.ToString(user.Is2FAAuthenticated)));

            //add Roles if needed here
            claims.Add(new Claim(ClaimTypes.Role, "AgentAccess", ClaimValueTypes.String, MpmtAuthDefaults.ClaimsIssuer));

            //create principal for the current authentication scheme
            var userIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var userPrincipal = new ClaimsPrincipal(userIdentity);

            //set value indicating whether session is persisted and the time at which the authentication was issued
            var authenticationProperties = new AuthenticationProperties
            {
                IsPersistent = isPersistent,
                IssuedUtc = DateTime.UtcNow
            };

            //sign in
            await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal, authenticationProperties);

            //redirect to the return URL if it's specified
            if (!string.IsNullOrWhiteSpace(returnUrl) && urlHelper.IsLocalUrl(returnUrl))
                return new RedirectResult(returnUrl);

            return new RedirectResult("Home/Index");
        }

        public async Task<bool> NormalAgentEmployeeSignInAsync(AgentUser user, string RoleName)
        {
            ArgumentNullException.ThrowIfNull(nameof(user));

            //create claims for user's username and email
            var claims = new List<Claim>();

            if (!string.IsNullOrWhiteSpace(user.UserName))
                claims.Add(new Claim(ClaimTypes.Name, user.UserName, ClaimValueTypes.String, MpmtAuthDefaults.ClaimsIssuer));

            if (user.EmployeeId > 0)
                claims.Add(new Claim("EmployeeId", user.EmployeeId.ToString()));

            if (!string.IsNullOrWhiteSpace(user.Email))
                claims.Add(new Claim(ClaimTypes.Email, user.Email, ClaimValueTypes.Email, MpmtAuthDefaults.ClaimsIssuer));

            if (!string.IsNullOrWhiteSpace(user.ContactNumber))
                claims.Add(new Claim(ClaimTypes.MobilePhone, user.ContactNumber, ClaimValueTypes.String, MpmtAuthDefaults.ClaimsIssuer));

            if (!string.IsNullOrWhiteSpace(user.AgentCode.ToString()))
                claims.Add(new Claim("AgentCode", user.AgentCode.ToString()));

            claims.Add(new Claim(MpmtClaimTypes.UniqueId, user.UserGuid.ToString()));
            claims.Add(new Claim(MpmtClaimTypes.LastChanged, DateTime.UtcNow.ToString()));

            if (!string.IsNullOrWhiteSpace(user.UserType))
                claims.Add(new Claim("UserType", user.UserType));

            claims.Add(new Claim("Is2FAAuthenticated", Convert.ToString(user.Is2FAAuthenticated)));
            claims.Add(new Claim(ClaimTypes.Role, RoleName, ClaimValueTypes.String, MpmtAuthDefaults.ClaimsIssuer));

            //add Roles in clames if needed

            //create principal for the current authentication scheme
            var userIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var userPrincipal = new ClaimsPrincipal(userIdentity);

            //set value indicating whether session is persisted and the time at which the authentication was issued
            var authenticationProperties = new AuthenticationProperties
            {
                IsPersistent = false,
                IssuedUtc = DateTime.UtcNow
            };

            //sign in
            await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal, authenticationProperties);

            return true;
        }

        public async Task<IActionResult> SignInAgentEmployeeAsync(AgentUser user, string returnUrl, string Code, bool isPersistent = false)
        {
            bool IsValidSecretCode = false;
            ArgumentNullException.ThrowIfNull(nameof(user));

            var urlHelper = _urlHelperFactory.GetUrlHelper(_actionContextAccessor.ActionContext);

            if (!string.IsNullOrEmpty(Code.Trim()))
            {
                var twoFactorAuthenticator = new TwoFactorAuthenticator();
                if (!user.Is2FARequired)
                {
                    _result = true;
                }
                else
                {
                    IsValidSecretCode = twoFactorAuthenticator.ValidateTwoFactorPIN(user.AccountSecretKey, Code.Trim());
                    if (IsValidSecretCode)
                        await _cashAgentEmployee.UpdateIs2FAAuthenticatedAgentEmployeeAsync(user.AgentCode, true, user.EmployeeId);
                }

                if (_configuration["LoginSetting:2FAcheckRequired"] == "0" && Code == _configuration["LoginSetting:2FACode"])
                {
                    _result = true;

                }
                else
                {
                    _result = IsValidSecretCode;
                }

                if (!_result)
                {
                    return new RedirectResult("/login/index?error=Invalid Code");
                }
            }
            //if (!user.ContactNumberConfirmed)
            //{
            //    var login = await NormalAdminSignInAsync(user, "AdminEmailValidateor");
            //    if (login)
            //    {
            //        var otp = OtpGeneration.GenerateRandom6DigitCode();

            //        var mailRequest = new MailRequestModel
            //        {
            //            MailFor = "confirm-email",
            //            MailTo = user.Email,
            //            MailSubject = "Your One-Time Password (OTP) for Registration",
            //            RecipientName = "",
            //            Content = GenrateMailBodyforOtp(otp)

            //        };
            //        var mailServiceModel = await _mailService.EmailSettings(mailRequest);

            //        Thread email = new(delegate ()
            //        {
            //            _mailService.SendMail(mailServiceModel);
            //        });
            //        email.Start();

            //        var addtoken = new TokenVerification
            //        {
            //            UserId = user.Id,
            //            UserName = user.UserName,
            //            Email = user.Email,
            //            VerificationCode = otp,
            //            VerificationType = "E",
            //            SendToEmail = true,
            //            SendToMobile = false,
            //            IsConsumed = false,
            //            ExpiredDate = DateTime.Now.AddMinutes(2)
            //        };

            //        var response = await _usersRepository.AddLoginOtpAsync(addtoken);

            //        return new RedirectResult("admin/login/TokenVerification");
            //    }
            //}

            //check if ip have been changed

            //var authenticatedUser = await GetAuthenticatedUserAsync();
            //if (authenticatedUser is not null || !_result)
            //{
            //    await SignOutAsync();
            //    return new RedirectToRouteResult("areas", new { controller = "Login", action = "Index" });
            //}

            //create claims for user's username and email
            var claims = new List<Claim>();

            if (!string.IsNullOrWhiteSpace(user.UserName))
                claims.Add(new Claim(ClaimTypes.Name, user.UserName, ClaimValueTypes.String, MpmtAuthDefaults.ClaimsIssuer));

            if (!string.IsNullOrWhiteSpace(user.Email))
                claims.Add(new Claim(ClaimTypes.Email, user.Email, ClaimValueTypes.Email, MpmtAuthDefaults.ClaimsIssuer));

            if (!string.IsNullOrWhiteSpace(user.ContactNumber))
                claims.Add(new Claim(ClaimTypes.MobilePhone, user.ContactNumber, ClaimValueTypes.String, MpmtAuthDefaults.ClaimsIssuer));

            if (user.EmployeeId > 0)
                claims.Add(new Claim("EmployeeId", user.EmployeeId.ToString()));

            claims.Add(new Claim("AgentCode", user.AgentCode.ToString()));
            claims.Add(new Claim(MpmtClaimTypes.UniqueId, user.UserGuid.ToString()));
            claims.Add(new Claim(MpmtClaimTypes.LastChanged, DateTime.UtcNow.ToString()));

            if (!string.IsNullOrWhiteSpace(user.UserType))
                claims.Add(new Claim("UserType", user.UserType));

            claims.Add(new Claim("Is2FAAuthenticated", Convert.ToString(user.Is2FAAuthenticated)));

            //TODO: add Roles if needed here

            claims.Add(new Claim(ClaimTypes.Role, "AgentAccess", ClaimValueTypes.String, MpmtAuthDefaults.ClaimsIssuer));

            //create principal for the current authentication scheme
            var userIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var userPrincipal = new ClaimsPrincipal(userIdentity);

            //set value indicating whether session is persisted and the time at which the authentication was issued
            var authenticationProperties = new AuthenticationProperties
            {
                IsPersistent = isPersistent,
                IssuedUtc = DateTime.UtcNow
            };

            //sign in
            await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal, authenticationProperties);

            //redirect to the return URL if it's specified
            if (!string.IsNullOrWhiteSpace(returnUrl) && urlHelper.IsLocalUrl(returnUrl))
                return new RedirectResult(returnUrl);

            return new RedirectResult("Home/Index");
        }

        public async Task SignOutAsync()
        {
            _httpContextAccessor.HttpContext.Session.Clear();

            //and sign out from the current authentication scheme
            await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        public async Task<ClaimsPrincipal> ChangePasswordRole(AgentUser user, string RoleName)
        {
            ArgumentNullException.ThrowIfNull(nameof(user));

            var claims = new List<Claim>();

            if (!string.IsNullOrWhiteSpace(user.UserName))
                claims.Add(new Claim(ClaimTypes.Name, user.UserName, ClaimValueTypes.String, MpmtAuthDefaults.ClaimsIssuer));

            if (!string.IsNullOrWhiteSpace(user.ContactNumber))
                claims.Add(new Claim(ClaimTypes.MobilePhone, user.ContactNumber, ClaimValueTypes.String, MpmtAuthDefaults.ClaimsIssuer));

            claims.Add(new Claim("AgentCode", user.AgentCode.ToString()));

            claims.Add(new Claim("EmployeeId", user.EmployeeId.ToString()));

            if (!string.IsNullOrWhiteSpace(user.UserType))
                claims.Add(new Claim("UserType", user.UserType));

            claims.Add(new Claim(MpmtClaimTypes.UniqueId, user.UserGuid.ToString()));
            claims.Add(new Claim(MpmtClaimTypes.LastChanged, DateTime.UtcNow.ToString()));
            claims.Add(new Claim(ClaimTypes.Role, RoleName, ClaimValueTypes.String, MpmtAuthDefaults.ClaimsIssuer));

            var userIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var userPrincipal = new ClaimsPrincipal(userIdentity);

            var authenticationProperties = new AuthenticationProperties
            {
                IsPersistent = false,
                IssuedUtc = DateTime.UtcNow
            };

            await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal, authenticationProperties);
            return userPrincipal;
        }
    }
}
