using DocumentFormat.OpenXml.InkML;
using Google.Authenticator;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Configuration;
using Mpmt.Core;
using Mpmt.Core.Common.Helpers;
using Mpmt.Core.Dtos.Partner;
using Mpmt.Core.Dtos.Users;
using Mpmt.Core.Models.Mail;
using Mpmt.Data.Repositories.Roles;
using Mpmt.Data.Repositories.Users;
using Mpmt.Services.Common;
using Mpmt.Services.Services.MailingService;
using Mpmt.Services.Services.UserActivityLog;
using System.Security.Claims;

namespace Mpmt.Services.Authentication
{
    /// <summary>
    /// The user cookie auth service.
    /// </summary>
    public class UserCookieAuthService : IUserCookieAuthService
    {
        private readonly IUserRolesRepository _userRolesRepository;
        private readonly IConfiguration _configuration;
        private readonly IUrlHelperFactory _urlHelperFactory;
        private readonly IActionContextAccessor _actionContextAccessor;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMailService _mailService;
        private readonly IUsersRepository _usersRepository;
        private readonly ClaimsPrincipal _Users;
        private readonly IUserActivityLog _activityLogService;
        //  private readonly IPartnerRepository _partnerRepository;
        private readonly IConfiguration _config;
        private AppUser _cachedUser;
        private bool _result = false;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostEnv;
        /// private AppPartner _cachedPartner;



        /// <summary>
        /// Initializes a new instance of the <see cref="UserCookieAuthService"/> class.
        /// </summary>
        /// <param name="urlHelperFactory">The url helper factory.</param>
        /// <param name="actionContextAccessor">The action context accessor.</param>
        /// <param name="httpContextAccessor">The http context accessor.</param>
        /// <param name="usersRepository">The users repository.</param>
        /// <param name="config">The config.</param>
        /// <summary>


#pragma warning disable ConstructorDocumentationHeader // The constructor must have a documentation header.
        public UserCookieAuthService(

#pragma warning restore ConstructorDocumentationHeader // The constructor must have a documentation header.

            IUserRolesRepository userRolesRepository,
            IConfiguration configuration,
            IUrlHelperFactory urlHelperFactory,
            IActionContextAccessor actionContextAccessor,
            IHttpContextAccessor httpContextAccessor,
            IMailService mailService,
            IUsersRepository usersRepository, IConfiguration config, IUserActivityLog activityLogService, Microsoft.AspNetCore.Hosting.IHostingEnvironment hostEnv)
        {
            _userRolesRepository = userRolesRepository;
            _configuration = configuration;
            _urlHelperFactory = urlHelperFactory;
            _actionContextAccessor = actionContextAccessor;
            _httpContextAccessor = httpContextAccessor;
            _mailService = mailService;
            _usersRepository = usersRepository;
            _config = config;
            _Users = _httpContextAccessor.HttpContext.User;
            _activityLogService = activityLogService;
            _hostEnv = hostEnv;
            ///  _partnerRepository = partnerRepository;
        }

        /// <summary>
        /// Gets the authenticated user async.
        /// </summary>
        /// <returns>A Task.</returns>
        public async Task<AppUser> GetAuthenticatedUserAsync()
        {
            if (_cachedUser is not null)
                return _cachedUser;

            //try to get authenticated user identity
            var authenticateResult = await _httpContextAccessor.HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (!authenticateResult.Succeeded)
                return null;

            AppUser user = null;

            var usernameClaim = authenticateResult.Principal.FindFirst(claim => claim.Type == ClaimTypes.Name
                && claim.Issuer.Equals(MpmtAuthDefaults.ClaimsIssuer, StringComparison.InvariantCultureIgnoreCase));

            if (usernameClaim is not null)
                user = await _usersRepository.GetUserByUserNameAsync(usernameClaim.Value);
            else
            {
                var emailClaim = authenticateResult.Principal.FindFirst(claim => claim.Type == ClaimTypes.Email
                    && claim.Issuer.Equals(MpmtAuthDefaults.ClaimsIssuer, StringComparison.InvariantCultureIgnoreCase));

                user = await _usersRepository.GetUserByEmailAsync(emailClaim.Value);
            }

            //whether the found user is available
            if (user is null || !user.IsActive || user.IsDeleted || user.IsBlocked)
                return null;

            //cache authenticated user
            _cachedUser = user;

            return _cachedUser;
        }


        public async Task<bool> NormalAdminSignInAsync(AppUser user, string RoleName)
        {

            if (user is null)
                throw new ArgumentNullException(nameof(user));

            //create claims for user's username and email
            var claims = new List<Claim>();

            if (!string.IsNullOrWhiteSpace(user.UserName))
                claims.Add(new Claim(ClaimTypes.Name, user.UserName, ClaimValueTypes.String, MpmtAuthDefaults.ClaimsIssuer));

            if (!string.IsNullOrWhiteSpace(user.Email))
                claims.Add(new Claim(ClaimTypes.Email, user.Email, ClaimValueTypes.Email, MpmtAuthDefaults.ClaimsIssuer));

            claims.Add(new Claim(MpmtClaimTypes.UniqueId, user.UserGuid.ToString()));
            claims.Add(new Claim("Id", user.Id.ToString()));
            claims.Add(new Claim(MpmtClaimTypes.LastChanged, DateTime.UtcNow.ToString()));

            claims.Add(new Claim("UserType", "Admin"));
            claims.Add(new Claim("Is2FAAuthenticated", Convert.ToString(user.Is2FAAuthenticated)));

            var roles = await _userRolesRepository.GetRolesByUserIdAsync(user.Id);
            if (roles.Any())
            {
                foreach (var userRole in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, userRole.RoleName, ClaimValueTypes.String, MpmtAuthDefaults.ClaimsIssuer));
                }
            }
            //added this role to access particular endpoint
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

            return true;
        }

        /// <summary>
        /// Signs the in async.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="returnUrl">The return url.</param>
        /// <param name="isPersistent">If true, is persistent.</param>
        /// <returns>A Task.</returns>
        public async Task<IActionResult> SignInAsync(AppUser user, string returnUrl, string Code, bool isPersistent = false)
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
                        _usersRepository.UpdateIs2FAAuthenticated(user.Email, true);
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
                    return new RedirectResult("/admin/login/index?error=Invalid Code");
                }


            }
            if (!user.EmailConfirmed)
            {
                var login = await NormalAdminSignInAsync(user, "AdminEmailValidateor");
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
                        PartnerCode = "Admin",
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

                    var response = await _usersRepository.AddLoginOtpAsync(addtoken);

                    return new RedirectResult("admin/login/TokenVerification");
                }
            }

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

            claims.Add(new Claim(MpmtClaimTypes.UniqueId, user.UserGuid.ToString()));
            claims.Add(new Claim("Id", user.Id.ToString()));
            claims.Add(new Claim(MpmtClaimTypes.LastChanged, DateTime.UtcNow.ToString()));

            claims.Add(new Claim("UserType", "Admin"));
            claims.Add(new Claim("Is2FAAuthenticated", Convert.ToString(user.Is2FAAuthenticated)));


            var roles = await _userRolesRepository.GetRolesByUserIdAsync(user.Id);
            if (roles.Any())
            {
                foreach (var userRole in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, userRole.RoleName, ClaimValueTypes.String, MpmtAuthDefaults.ClaimsIssuer));
                }
            }
            claims.Add(new Claim(ClaimTypes.Role, "AdminAccess", ClaimValueTypes.String, MpmtAuthDefaults.ClaimsIssuer));

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
            _cachedUser = user;

            //redirect to the return URL if it's specified
            if (!string.IsNullOrWhiteSpace(returnUrl) && urlHelper.IsLocalUrl(returnUrl))
                return new RedirectResult(returnUrl);

            var userActivityLogParams = await LogHelper.GetUserActivityLogAsync(_httpContextAccessor.HttpContext, _hostEnv, logRequestBody: true, userAction: "logged in to admin portal");
            userActivityLogParams.UserName = userPrincipal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name).Value;
            userActivityLogParams.Email = userPrincipal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email).Value;
            await _activityLogService.AddAsync(userActivityLogParams);

            return new RedirectResult("admin/AdminDashboard/Index");
        }

        public async Task<bool> LoginLogout(AppUser user)
        {

            var claims = new List<Claim>();

            if (!string.IsNullOrWhiteSpace(user.UserName))
                claims.Add(new Claim(ClaimTypes.Name, user.UserName, ClaimValueTypes.String, MpmtAuthDefaults.ClaimsIssuer));

            if (!string.IsNullOrWhiteSpace(user.Email))
                claims.Add(new Claim(ClaimTypes.Email, user.Email, ClaimValueTypes.Email, MpmtAuthDefaults.ClaimsIssuer));

            claims.Add(new Claim(MpmtClaimTypes.UniqueId, user.UserGuid.ToString()));
            claims.Add(new Claim(MpmtClaimTypes.LastChanged, DateTime.UtcNow.ToString()));

            claims.Add(new Claim("UserType", "Admin"));
            claims.Add(new Claim("Is2FAAuthenticated", Convert.ToString(user.Is2FAAuthenticated)));


            var roles = await _userRolesRepository.GetRolesByUserIdAsync(user.Id);
            if (roles.Any())
            {
                foreach (var userRole in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, userRole.RoleName, ClaimValueTypes.String, MpmtAuthDefaults.ClaimsIssuer));
                }
            }
            claims.Add(new Claim(ClaimTypes.Role, "AdminAccess", ClaimValueTypes.String, MpmtAuthDefaults.ClaimsIssuer));

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
        public async Task SignOutAsync()
        {
            // reset cached user
            _cachedUser = null;
            _httpContextAccessor.HttpContext.Session.Clear();

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

    }
}
