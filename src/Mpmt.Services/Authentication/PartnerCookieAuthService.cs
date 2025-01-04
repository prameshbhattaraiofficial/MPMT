using Google.Authenticator;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Configuration;
using Mpmt.Core;
using Mpmt.Core.Dtos.Partner;
using Mpmt.Core.Dtos.PartnerEmployee;
using Mpmt.Core.Models.Mail;
using Mpmt.Data.Repositories.Employee;
using Mpmt.Data.Repositories.Partner;
using Mpmt.Data.Repositories.PartnerEmployee;
using Mpmt.Services.Common;
using Mpmt.Services.Services.MailingService;
using System.Data;
using System.Security.Claims;

namespace Mpmt.Services.Authentication
{
    /// <summary>
    /// The user cookie auth service.
    /// </summary>
    public class PartnerCookieAuthService : IPartnerCookieAuthService
    {
        private readonly IUrlHelperFactory _urlHelperFactory;
        private readonly IConfiguration _configuration;
        private readonly IActionContextAccessor _actionContextAccessor;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmployeeRepo _employeeRepo;
        private readonly IPartnerRepository _partnerRepository;
        private readonly IPartnerEmployeeRepo _partnerEmployeeRepo;
        public readonly ClaimsPrincipal _Users;
        private AppPartner _cachedPartner;
        private bool _result = false;
        private readonly IMailService _mailService;
        private readonly IConfiguration _config;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserCookieAuthService"/> class.
        /// </summary>
        /// <param name="urlHelperFactory">The url helper factory.</param>
        /// <param name="actionContextAccessor">The action context accessor.</param>
        /// <param name="httpContextAccessor">The http context accessor.</param>
        /// <param name="usersRepository">The users repository.</param>
        public PartnerCookieAuthService(
            IUrlHelperFactory urlHelperFactory,
            IConfiguration configuration,
            IActionContextAccessor actionContextAccessor,
            IHttpContextAccessor httpContextAccessor,
            IEmployeeRepo employeeRepo,
            IPartnerRepository partnerRepository,
            IPartnerEmployeeRepo partnerEmployeeRepo,
            IMailService mailService, IConfiguration config)

        {
            _urlHelperFactory = urlHelperFactory;
            _configuration = configuration;
            _actionContextAccessor = actionContextAccessor;
            _httpContextAccessor = httpContextAccessor;
            _employeeRepo = employeeRepo;
            _Users = _httpContextAccessor.HttpContext.User;
            _partnerRepository = partnerRepository;
            _partnerEmployeeRepo = partnerEmployeeRepo;
            _mailService = mailService;
            _config = config;

        }

        /// <summary>
        /// Gets the authenticated partner async.
        /// </summary>
        /// <returns>A Task.</returns>
        public async Task<AppPartner> GetAuthenticatedPartnerAsync()
        {
            if (_cachedPartner is not null)
                return _cachedPartner;

            //try to get authenticated user identity
            var authenticateResult = await _httpContextAccessor.HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (!authenticateResult.Succeeded)
                return null;

            AppPartner user = null;

            var usernameClaim = authenticateResult.Principal.FindFirst(claim => claim.Type == ClaimTypes.Name
                && claim.Issuer.Equals(MpmtAuthDefaults.ClaimsIssuer, StringComparison.InvariantCultureIgnoreCase));

            if (usernameClaim is not null)
                user = await _partnerRepository.GetPartnerByUserNameAsync(usernameClaim.Value);
            else
            {
                var emailClaim = authenticateResult.Principal.FindFirst(claim => claim.Type == ClaimTypes.Email
                    && claim.Issuer.Equals(MpmtAuthDefaults.ClaimsIssuer, StringComparison.InvariantCultureIgnoreCase));

                user = await _partnerRepository.GetPartnerByEmailAsync(emailClaim.Value);
            }

            //whether the found user is available
            if (user is null || !user.IsActive || user.IsDeleted || user.IsBlocked)
                return null;

            //cache authenticated user
            _cachedPartner = user;

            return _cachedPartner;
        }
        /// <summary>
        /// Signs the in partner async.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="returnUrl">The return url.</param>
        /// <param name="isPersistent">If true, is persistent.</param>
        /// <returns>A Task.</returns>
        public async Task<IActionResult> SignInPartnerAsync(AppPartner user, string returnUrl, string Code, bool isPersistent = false)
        {
            bool IsValidSecretCode = false;
            if (user is null)
                throw new ArgumentNullException(nameof(user));

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
                        _partnerRepository.UpdateIs2FAAuthenticated(user.Email, true);
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

                    return new RedirectResult("/partner/login/index?error=Invalid Code");
                }
            }
            if (!user.EmailConfirmed)
            {
                var login = await NormalSignInPartnerAsync(user, "EmailValidateor");
                if (login)
                {
                    var otp = OtpGeneration.GenerateRandom6DigitCode();

                    var mailRequest = new MailRequestModel
                    {
                        MailFor = "confirm-email",
                        MailTo = user.Email,
                        MailSubject = "Your One-Time Password (OTP) for Registration",
                        RecipientName = "",
                        Content = GenrateMailBodyforOtp(otp)

                    };
                    var mailServiceModel = await _mailService.EmailSettings(mailRequest);

                    Thread email = new(delegate ()
                    {
                        _mailService.SendMail(mailServiceModel);
                    });
                    email.Start();

                    var addtoken = new TokenVerification
                    {
                        UserId = user.Id,
                        PartnerCode = user.PartnerCode,
                        UserName = user.UserName,
                        Email = user.Email,
                        VerificationCode = otp,
                        VerificationType = "Email",
                        OtpVerificationFor = "Confirm-Email",
                        SendToEmail = true,
                        SendToMobile = false,
                        IsConsumed = false,
                        ExpiredDate = DateTime.Now.AddMinutes(2)
                    };

                    var response = await _partnerRepository.AddLoginOtpAsync(addtoken);

                    return new RedirectResult("Partner/login/TokenVerification");
                }
            }
            //if(user.IpAddress != "current ip address")
            //{

            //    var login = await NormalSignInPartnerAsync(user, "EmailValidateor");
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
            //            PartnerCode = user.PartnerCode,
            //            UserName = user.UserName,
            //            Email = user.Email,
            //            VerificationCode = otp,
            //            VerificationType = "E",
            //            SendToEmail = true,
            //            SendToMobile = false,
            //            IsConsumed = false,
            //            ExpiredDate = DateTime.Now.AddMinutes(2)
            //        };

            //        var response = await _partnerRepository.AddLoginOtpAsync(addtoken);

            //        return new RedirectToRouteResult("Partner", new { controller = "Login", action = "TokenVerification" });
            //    }
            //}

            //create claims for user's username and email
            var claims = new List<Claim>();

            if (!string.IsNullOrWhiteSpace(user.UserName))
                claims.Add(new Claim(ClaimTypes.Name, user.UserName, ClaimValueTypes.String, MpmtAuthDefaults.ClaimsIssuer));

            if (!string.IsNullOrWhiteSpace(user.Email))
                claims.Add(new Claim(ClaimTypes.Email, user.Email, ClaimValueTypes.Email, MpmtAuthDefaults.ClaimsIssuer));


            if (!string.IsNullOrWhiteSpace(user.PartnerCode))
                claims.Add(new Claim("PartnerCode", user.PartnerCode, "PartnerCode", MpmtAuthDefaults.ClaimsIssuer));

            if (!string.IsNullOrWhiteSpace(user.Id.ToString()))
                claims.Add(new Claim("Id", user.Id.ToString(), "Id", MpmtAuthDefaults.ClaimsIssuer));

            claims.Add(new Claim(ClaimTypes.Role, "Access", ClaimValueTypes.String, MpmtAuthDefaults.ClaimsIssuer));
            claims.Add(new Claim(ClaimTypes.Role, "PARTNERADMIN", ClaimValueTypes.String, MpmtAuthDefaults.ClaimsIssuer));

            claims.Add(new Claim("UserType", "Partner"));
            claims.Add(new Claim("Is2FAAuthenticated", Convert.ToString(user.Is2FAAuthenticated)));

            claims.Add(new Claim(MpmtClaimTypes.UniqueId, user.UserGuid.ToString()));
            claims.Add(new Claim(MpmtClaimTypes.LastChanged, DateTime.UtcNow.ToString()));

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

            //cache authenticated user
            _cachedPartner = user;

            //if (user.Is2FAAuthenticated && !string.IsNullOrEmpty(Code.Trim()))
            //{
            //    var twoFactorAuthenticator = new TwoFactorAuthenticator();
            //    _result = twoFactorAuthenticator.ValidateTwoFactorPIN(user.AccountSecretKey, Code.Trim());
            //}

            //redirect to the return URL if it's specified
            if (!string.IsNullOrWhiteSpace(returnUrl) && urlHelper.IsLocalUrl(returnUrl))
                return new RedirectResult(returnUrl);


            return new RedirectResult("Partner/Dashboard/Index");
        }

        public async Task<bool> NormalSignInPartnerAsync(AppPartner user, string RoleName)
        {

            if (user is null)
                throw new ArgumentNullException(nameof(user));

            //create claims for user's username and email
            var claims = new List<Claim>();

            if (!string.IsNullOrWhiteSpace(user.UserName))
                claims.Add(new Claim(ClaimTypes.Name, user.UserName, ClaimValueTypes.String, MpmtAuthDefaults.ClaimsIssuer));

            if (!string.IsNullOrWhiteSpace(user.Email))
                claims.Add(new Claim(ClaimTypes.Email, user.Email, ClaimValueTypes.Email, MpmtAuthDefaults.ClaimsIssuer));
            if (!string.IsNullOrWhiteSpace(user.PartnerCode))
                claims.Add(new Claim("PartnerCode", user.PartnerCode, "PartnerCode", MpmtAuthDefaults.ClaimsIssuer));
            if (!string.IsNullOrWhiteSpace(user.Id.ToString()))
                claims.Add(new Claim("Id", user.Id.ToString(), "Id", MpmtAuthDefaults.ClaimsIssuer));

            claims.Add(new Claim(MpmtClaimTypes.UniqueId, user.UserGuid.ToString()));
            claims.Add(new Claim(MpmtClaimTypes.LastChanged, DateTime.UtcNow.ToString()));

            claims.Add(new Claim("UserType", "Partner"));
            claims.Add(new Claim("Is2FAAuthenticated", Convert.ToString(user.Is2FAAuthenticated)));
            claims.Add(new Claim(ClaimTypes.Role, RoleName, ClaimValueTypes.String, MpmtAuthDefaults.ClaimsIssuer));

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

            //redirect to the return URL if it's specified
            return true;
        }
        public async Task<bool> NormalSignInPartnerEmployeeAsync(AppPartnerEmployee user, string RoleName)
        {

            if (user is null)
                throw new ArgumentNullException(nameof(user));

            //create claims for user's username and email
            var claims = new List<Claim>();

            if (!string.IsNullOrWhiteSpace(user.UserName))
                claims.Add(new Claim(ClaimTypes.Name, user.UserName, ClaimValueTypes.String, MpmtAuthDefaults.ClaimsIssuer));

            if (!string.IsNullOrWhiteSpace(user.Email))
                claims.Add(new Claim(ClaimTypes.Email, user.Email, ClaimValueTypes.Email, MpmtAuthDefaults.ClaimsIssuer));
            if (!string.IsNullOrWhiteSpace(user.PartnerCode))
                claims.Add(new Claim("PartnerCode", user.PartnerCode, "PartnerCode", MpmtAuthDefaults.ClaimsIssuer));
            if (!string.IsNullOrWhiteSpace(user.Id.ToString()))
                claims.Add(new Claim("Id", user.Id.ToString(), "Id", MpmtAuthDefaults.ClaimsIssuer));

            claims.Add(new Claim(MpmtClaimTypes.UniqueId, user.UserGuid.ToString()));
            claims.Add(new Claim(MpmtClaimTypes.LastChanged, DateTime.UtcNow.ToString()));

            claims.Add(new Claim("UserType", "Partner"));
            claims.Add(new Claim("Is2FAAuthenticated", Convert.ToString(user.Is2FAAuthenticated)));
            claims.Add(new Claim(ClaimTypes.Role, RoleName, ClaimValueTypes.String, MpmtAuthDefaults.ClaimsIssuer));

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

            //redirect to the return URL if it's specified
            return true;
        }
        public async Task<IActionResult> SignInPartnerEmployeeAsync(AppPartnerEmployee user, string returnUrl, string Code, bool isPersistent = false)
        {
            bool IsValidSecretCode = false;
            if (user is null)
                throw new ArgumentNullException(nameof(user));

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
                        _employeeRepo.UpdateIs2FAAuthenticated(user.Email, true);
                }

                if (_configuration["LoginSetting:2FAcheckRequired"] == "0" && Code == _configuration["LoginSetting:2FACode"])
                {
                    _result = true;
                } else
                {
                    _result = IsValidSecretCode;
                }

                if (!_result)
                {

                    return new RedirectResult("/partner/login/index?error=Invalid Code");
                }
            }
            if (!user.EmailConfirmed)
            {
                var login = await NormalSignInPartnerEmployeeAsync(user, "EmployeeEmailValidateor");
                if (login)
                {
                    var otp = OtpGeneration.GenerateRandom6DigitCode();

                    var mailRequest = new MailRequestModel
                    {
                        MailFor = "confirm-email",
                        MailTo = user.Email,
                        MailSubject = "Your One-Time Password (OTP) for Registration",
                        RecipientName = "",
                        Content = GenrateMailBodyforOtp(otp)

                    };
                    var mailServiceModel = await _mailService.EmailSettings(mailRequest);

                    Thread email = new(delegate ()
                    {
                        _mailService.SendMail(mailServiceModel);
                    });
                    email.Start();

                    var addtoken = new TokenVerification
                    {
                        UserId = user.Id,
                        PartnerCode = user.PartnerCode,
                        UserName = user.UserName,
                        Email = user.Email,
                        VerificationCode = otp,
                        VerificationType = "Email",
                        OtpVerificationFor = "Confirm-Email",
                        SendToEmail = true,
                        SendToMobile = false,
                        IsConsumed = false,
                        ExpiredDate = DateTime.Now.AddMinutes(2)
                    };

                    var response = await _partnerRepository.AddLoginOtpAsync(addtoken);

                    return new RedirectResult("Partner/login/EmployeeTokenVerification");
                }
            }
            //if(user.IpAddress != "current ip address")
            //{

            //    var login = await NormalSignInPartnerAsync(user, "EmailValidateor");
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
            //            PartnerCode = user.PartnerCode,
            //            UserName = user.UserName,
            //            Email = user.Email,
            //            VerificationCode = otp,
            //            VerificationType = "E",
            //            SendToEmail = true,
            //            SendToMobile = false,
            //            IsConsumed = false,
            //            ExpiredDate = DateTime.Now.AddMinutes(2)
            //        };

            //        var response = await _partnerRepository.AddLoginOtpAsync(addtoken);

            //        return new RedirectToRouteResult("Partner", new { controller = "Login", action = "TokenVerification" });
            //    }
            //}

            //create claims for user's username and email


            var claims = new List<Claim>();

            if (!string.IsNullOrWhiteSpace(user.UserName))
                claims.Add(new Claim(ClaimTypes.Name, user.UserName, ClaimValueTypes.String, MpmtAuthDefaults.ClaimsIssuer));

            if (!string.IsNullOrWhiteSpace(user.Email))
                claims.Add(new Claim(ClaimTypes.Email, user.Email, ClaimValueTypes.Email, MpmtAuthDefaults.ClaimsIssuer));


            if (!string.IsNullOrWhiteSpace(user.PartnerCode))
                claims.Add(new Claim("PartnerCode", user.PartnerCode, "PartnerCode", MpmtAuthDefaults.ClaimsIssuer));

            if (!string.IsNullOrWhiteSpace(user.Id.ToString()))
                claims.Add(new Claim("Id", user.Id.ToString(), "Id", MpmtAuthDefaults.ClaimsIssuer));

            claims.Add(new Claim(ClaimTypes.Role, "Access", ClaimValueTypes.String, MpmtAuthDefaults.ClaimsIssuer));

            var roles = await _partnerEmployeeRepo.GetPartnerEmployeeRolesByIdAsync(user.Id, user.PartnerCode);

            //addpartnerEmployee role
            if (roles.Count() != 0)
            {
                foreach (var userRole in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, userRole.Text, ClaimValueTypes.String, MpmtAuthDefaults.ClaimsIssuer));
                }
            }

            claims.Add(new Claim(MpmtClaimTypes.UniqueId, user.UserGuid.ToString()));
            claims.Add(new Claim(MpmtClaimTypes.LastChanged, DateTime.UtcNow.ToString()));

            claims.Add(new Claim("UserType", "PartnerEmployee"));
            claims.Add(new Claim("Is2FAAuthenticated", Convert.ToString(user.Is2FAAuthenticated)));
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

            //cache authenticated user


            //if (user.Is2FAAuthenticated && !string.IsNullOrEmpty(Code.Trim()))
            //{
            //    var twoFactorAuthenticator = new TwoFactorAuthenticator();
            //    _result = twoFactorAuthenticator.ValidateTwoFactorPIN(user.AccountSecretKey, Code.Trim());
            //}

            //redirect to the return URL if it's specified
            if (!string.IsNullOrWhiteSpace(returnUrl) && urlHelper.IsLocalUrl(returnUrl))
                return new RedirectResult(returnUrl);


            return new RedirectResult("Partner/Dashboard/Index");
        }
        /// <summary>
        /// Signs the out partner async.
        /// </summary>
        /// <returns>A Task.</returns>
        public async Task SignOutPartnerAsync()
        {
            // reset cached user
            _cachedPartner = null;
            //and sign out from the current authentication scheme
            await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }


        public string GenrateMailBodyforOtp(string Otp)
        {
            var companyName = "MyPay Money Transfer Pvt Ltd";
            var companyEmail = "info@MPMT.com";

            string mailBody =
                $@"
                <br>
              
                <p>We are pleased to provide you with the One-Time Password (OTP) necessary to Complete Login!!</p>
                
                <P>Your OTP is: {Otp} <p>
                  
                <p style='color=red;'>Important! Do not share your File</p>
                <p>Please remember that this OTP is valid for a single use and has a limited timeframe for use. For your security, do not share this OTP with anyone, including our support team. </P>
                <br>             
                <p>If you have any queries, Please contact us at,</p>
                <p>ATTN : Please do not reply to this email. This mailbox is not monitored and you will not receive a response.</p>
                <p>{companyName},<br>
                Radhe Radhe Bhaktapur, Nepal.<br>
                {companyEmail}</p>

                <p>Warm Regards,<br>
                {companyName}</p>


                <p>CONFIDENTIALITY NOTICE: This transmittal is a confidential communication or may otherwise be privileged. If you are not the intended recipient, you are hereby notified that you have received this transmittal in error and that any review, dissemination, distribution or copying of this transmittal is strictly prohibited. If you have received this communication in error, please notify this office, and immediately delete this message and all its attachments, if any.</p>";

            return mailBody;
        }

        public async Task<bool> LoginLogout()
        {
            var claims = new List<Claim>();

            if (!string.IsNullOrWhiteSpace(_Users.Claims.Where(x => x.Type == ClaimTypes.Name).FirstOrDefault()?.Value))
                claims.Add(new Claim(ClaimTypes.Name, _Users.Claims.Where(x => x.Type == ClaimTypes.Name).FirstOrDefault()?.Value, ClaimValueTypes.String, MpmtAuthDefaults.ClaimsIssuer));

            if (!string.IsNullOrWhiteSpace(_Users.Claims.Where(x => x.Type == "Email").FirstOrDefault()?.Value))
                claims.Add(new Claim(ClaimTypes.Email, _Users.Claims.Where(x => x.Type == "Email").FirstOrDefault()?.Value, ClaimValueTypes.Email, MpmtAuthDefaults.ClaimsIssuer));


            if (!string.IsNullOrWhiteSpace(_Users.Claims.Where(x => x.Type == "PartnerCode").FirstOrDefault()?.Value))
                claims.Add(new Claim("PartnerCode", _Users.Claims.Where(x => x.Type == "PartnerCode").FirstOrDefault()?.Value, "PartnerCode", MpmtAuthDefaults.ClaimsIssuer));

            if (!string.IsNullOrWhiteSpace(_Users.Claims.Where(x => x.Type == "Id").FirstOrDefault()?.Value))
                claims.Add(new Claim("Id", _Users.Claims.Where(x => x.Type == "Id").FirstOrDefault()?.Value, "Id", MpmtAuthDefaults.ClaimsIssuer));

            if (_Users.HasClaim(c => c.Type == MpmtClaimTypes.UniqueId))
                claims.Add(_Users.Claims.First(c => c.Type == MpmtClaimTypes.UniqueId));

            claims.Add(new Claim(MpmtClaimTypes.LastChanged, DateTime.UtcNow.ToString()));

            var roles = await _partnerEmployeeRepo.GetPartnerEmployeeRolesByIdAsync(int.Parse(_Users.Claims.Where(x => x.Type == "Id").FirstOrDefault()?.Value), _Users.Claims.Where(x => x.Type == "PartnerCode").FirstOrDefault()?.Value);

            //addpartnerEmployee role
            foreach (var userRole in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole.Text, ClaimValueTypes.String, MpmtAuthDefaults.ClaimsIssuer));
            }

            claims.Add(new Claim(ClaimTypes.Role, "Access", ClaimValueTypes.String, MpmtAuthDefaults.ClaimsIssuer));
            claims.Add(new Claim(ClaimTypes.Role, "PARTNERADMIN", ClaimValueTypes.String, MpmtAuthDefaults.ClaimsIssuer));


            claims.Add(new Claim("UserType", "PartnerEmployee"));

            claims.Add(new Claim("Is2FAAuthenticated", Convert.ToString(_Users.Claims.Where(x => x.Type == "Is2FAAuthenticated").FirstOrDefault()?.Value)));
            //create principal for the current authentication scheme
            var userIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var userPrincipal = new ClaimsPrincipal(userIdentity);

            //set value indicating whether session is persisted and the time at which the authentication was issued
            var authenticationProperties = new AuthenticationProperties
            {
                IsPersistent = false,
                IssuedUtc = DateTime.UtcNow
            };
            await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            try
            {
                await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal, authenticationProperties);
                return true;
            }
            catch
            {
                return false;
            }


        }
        public async Task<bool> partnerLoginLogout()
        {
            var claims = new List<Claim>();

            if (!string.IsNullOrWhiteSpace(_Users.Claims.Where(x => x.Type == ClaimTypes.Name).FirstOrDefault()?.Value))
                claims.Add(new Claim(ClaimTypes.Name, _Users.Claims.Where(x => x.Type == ClaimTypes.Name).FirstOrDefault()?.Value, ClaimValueTypes.String, MpmtAuthDefaults.ClaimsIssuer));

            if (!string.IsNullOrWhiteSpace(_Users.Claims.Where(x => x.Type == "Email").FirstOrDefault()?.Value))
                claims.Add(new Claim(ClaimTypes.Email, _Users.Claims.Where(x => x.Type == "Email").FirstOrDefault()?.Value, ClaimValueTypes.Email, MpmtAuthDefaults.ClaimsIssuer));


            if (!string.IsNullOrWhiteSpace(_Users.Claims.Where(x => x.Type == "PartnerCode").FirstOrDefault()?.Value))
                claims.Add(new Claim("PartnerCode", _Users.Claims.Where(x => x.Type == "PartnerCode").FirstOrDefault()?.Value, "PartnerCode", MpmtAuthDefaults.ClaimsIssuer));

            if (!string.IsNullOrWhiteSpace(_Users.Claims.Where(x => x.Type == "Id").FirstOrDefault()?.Value))
                claims.Add(new Claim("Id", _Users.Claims.Where(x => x.Type == "Id").FirstOrDefault()?.Value, "Id", MpmtAuthDefaults.ClaimsIssuer));

            claims.Add(new Claim(ClaimTypes.Role, "Access", ClaimValueTypes.String, MpmtAuthDefaults.ClaimsIssuer));
            claims.Add(new Claim(ClaimTypes.Role, "PARTNERADMIN", ClaimValueTypes.String, MpmtAuthDefaults.ClaimsIssuer));

            if (_Users.HasClaim(c => c.Type == MpmtClaimTypes.UniqueId))
                claims.Add(_Users.Claims.First(c => c.Type == MpmtClaimTypes.UniqueId));

            claims.Add(new Claim(MpmtClaimTypes.LastChanged, DateTime.UtcNow.ToString()));

            claims.Add(new Claim("UserType", "Partner"));

            claims.Add(new Claim("Is2FAAuthenticated", Convert.ToString(_Users.Claims.Where(x => x.Type == "Is2FAAuthenticated").FirstOrDefault()?.Value)));
            //create principal for the current authentication scheme
            var userIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var userPrincipal = new ClaimsPrincipal(userIdentity);

            //set value indicating whether session is persisted and the time at which the authentication was issued
            var authenticationProperties = new AuthenticationProperties
            {
                IsPersistent = false,
                IssuedUtc = DateTime.UtcNow
            };
            await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            try
            {
                await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal, authenticationProperties);
                return true;
            }
            catch
            {
                return false;
            }


        }

        public void UpdateEmployeeAccountSecretKey(string email, string Accountsecretkey)
        {
            _employeeRepo.UpdateAccountSecretKey(email, Accountsecretkey);

        }
        public async Task<TokenVerification> GetOtpBypartnerEmployeeCodeAsync(string partnercode, string UserName, string OtpVerificationFor)
        {
            var data = await _employeeRepo.GetOtpBypartnerEmployeeCodeAsync(partnercode, UserName, OtpVerificationFor);
            return data;
        }
        public async void UpdateEmployeeEmailConfirm(string partnercode,string UserName)
        {
            _employeeRepo.UpdateEmailConfirmAsync(partnercode, UserName);

        }

    }
}
