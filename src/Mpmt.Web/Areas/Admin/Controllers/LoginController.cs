using AspNetCoreHero.ToastNotification.Abstractions;
using Google.Authenticator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mpmt.Core.Domain;
using Mpmt.Core.Events;
using Mpmt.Core.ViewModel.ForgotPassword;
using Mpmt.Data.Common;
using Mpmt.Services.Authentication;
using Mpmt.Services.Users;
using Mpmt.Web.Areas.Admin.Models.Users;
using Mpmt.Web.Filter;
using System.Net;
using System.Reflection;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Mpmt.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// The login controller.
    /// </summary>
    public class LoginController : BaseAdminController
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserCookieAuthService _userCookieAuthService;
        private readonly INotyfService _notyfService;
        private readonly IEventPublisher _eventPublisher;
        private readonly IUserRegistrationService _userRegistrationService;
        private readonly IUsersService _usersService;
        private readonly ClaimsPrincipal _Users;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginController"/> class.
        /// </summary>
        /// <param name="userCookieAuthService">The user cookie auth service.</param>
        /// <param name="userRegistrationService">The user registration service.</param>
        public LoginController(
            IHttpContextAccessor httpContextAccessor,
            IUserCookieAuthService userCookieAuthService,
            IUserRegistrationService userRegistrationService,
            IUsersService usersService,
            INotyfService notyfService,
            IEventPublisher eventPublisher)
        {
            _httpContextAccessor = httpContextAccessor;
            _userCookieAuthService = userCookieAuthService;
            _userRegistrationService = userRegistrationService;
            _Users = _httpContextAccessor.HttpContext.User;
            _usersService = usersService;
            _notyfService = notyfService;
            _eventPublisher = eventPublisher;
        }

        /// <summary>
        /// Indices the.
        /// </summary>
        /// <returns>An IActionResult.</returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Index(string error = "")
        {
            var usertype = _Users.Claims.Where(c => c.Type == "UserType").Select(c => c.Value).FirstOrDefault();
            if (usertype == "Partner")
            {
                if (User.IsInRole("Access"))
                {
                    return RedirectToAction("Index", "Dashboard", new { Area = "Partner" });
                }
                await _userCookieAuthService.SignOutAsync();

            }
            if (usertype == "Admin")
            {
                if (User.IsInRole("AdminAccess"))
                {
                    return RedirectToAction("Index", "adminDashboard", new { Area = "Admin" });
                }
                await _userCookieAuthService.SignOutAsync(); ;
            }
            if (!string.IsNullOrEmpty(error))
            {
                ViewBag.Error = error;
            }
            return View();
        }

        /// <summary>
        /// Indices the.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="returnUrl">The return url.</param>
        /// <returns>A Task.</returns>
        
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Index(UserLoginModel model, string returnUrl)
        {

            if (string.IsNullOrEmpty(model.UsernameOrEmail))
            {
                ViewBag.Error = "UserName or Email cannot be null";
                return View();
            }
            if (string.IsNullOrEmpty(model.Password))
            {
                ViewBag.Error = "Password cannot be null";
                return View();
            }



            if (ModelState.IsValid)
            {


                var userNameOrEmail = model.UsernameOrEmail?.Trim();
                var (loginResult, user) = await _userRegistrationService.ValidateUserAsync(userNameOrEmail, model.Password);

                switch (loginResult)
                {
                    case LoginResults.Successful:
                        {

                            if (string.IsNullOrEmpty(model.Code) && (!user.Is2FAAuthenticated))
                            {
                                if (await _userCookieAuthService.NormalAdminSignInAsync(user, "2FAdminAuth"))
                                {
                                    return RedirectToAction("AccountSecretKey", "Login");
                                }
                            }
                            if (string.IsNullOrEmpty(model.Code) && (user.Is2FAAuthenticated))
                            {
                                ViewBag.Error = "Code cannot be null";
                                return View();
                            }

                            return await _userCookieAuthService.SignInAsync(user, returnUrl, model.Code, isPersistent: model.RememberMe);

                        }

                    case LoginResults.InitiateFactorAuthentication:
                        {

                            if (await _userCookieAuthService.NormalAdminSignInAsync(user, "2FAdminAuth"))
                            {
                                return RedirectToAction("AccountSecretKey", "Login");
                            }
                            break;

                        }


                    case LoginResults.NotExist:
                        ViewBag.Error = "Invalid login credentials.";
                        ModelState.AddModelError("", "Invalid login credentials.");
                        break;
                    case LoginResults.WrongPassword:
                        ViewBag.Error = "Invalid login credentials.";
                        ModelState.AddModelError("", "Invalid login credentials.");
                        break;
                    case LoginResults.Deleted:
                        ViewBag.Error = "Invalid login credentials.";
                        ModelState.AddModelError("", "Invalid login credentials.");
                        break;
                    case LoginResults.NotActive:
                        ViewBag.Error = "Account not activated.";
                        ModelState.AddModelError("", "Account not activated.");
                        break;
                    case LoginResults.PasswordExpired:
                        ViewBag.Error = "Password Expired. Please change your Password.";
                        ModelState.AddModelError("", "Password Expired");
                        break;
                    case LoginResults.Blocked:
                        ViewBag.Error = "Account blocked! Please contact our support team.";
                        ModelState.AddModelError("", "Account blocked! Please contact our support team.");
                        break;
                    case LoginResults.LockedOut:
                        ViewBag.Error = "Account temporarily locked.";
                        ModelState.AddModelError("", "Account temporarily locked.");
                        break;
                    case LoginResults.MultiFactorAuthenticationRequired:
                        ViewBag.Error = "Account blocked! Please contact our support team.";
                        ModelState.AddModelError("", "Account blocked! Please contact our support team.");
                        break;
                    default:
                        ViewBag.Error = "Invalid login credentials.";
                        ModelState.AddModelError("", "Invalid login credentials.");
                        break;
                }

            }

            return View("Index", model);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> PasswordReset([FromQuery] string token)
        {
            if (token != null && token != "")
            {
                var (result, data) = await _usersService.RequestTokenValidationAsync(token);
                if (result.StatusCode == 200)
                {
                    return View(data);
                }
            }
            _notyfService.Error("Password reset link has been expired.");
            return RedirectToAction("Index");
        }

        [HttpPost]
        [LogUserActivity("reset password")]
        [AllowAnonymous]
        public async Task<IActionResult> PasswordReset(PasswordResetModel resetModel)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)(HttpStatusCode.BadRequest);
                return View(resetModel);
            }
            var (result, data) = await _usersService.ResetTokenValidationAsync(resetModel);
            if (result.StatusCode == 200)
            {
                resetModel.UserName = data.UserName;
                resetModel.UserGuid = data.UserGuid;
                var responseStatus = await _userRegistrationService.ResetPasswordAsync(resetModel);

                if (responseStatus.StatusCode == 200)
                {
                    _notyfService.Success("Password has been updated succesfully.");
                    await _eventPublisher.PublishAsync(new EntityUpdatedEvent<SessionAuthExpiration>(
                            new SessionAuthExpiration { UserUniqueId = resetModel.UserGuid.ToString(), ExpireBefore = DateTime.UtcNow }));
                    return RedirectToAction("Index");
                }
                else
                {
                    Response.StatusCode = (int)(HttpStatusCode.BadRequest);
                    ViewBag.Error = responseStatus.MsgText;
                    return View(resetModel);
                }
            }
            return View(resetModel);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword()
        {
            return View();
        }

        //[HttpPost]
        //[AllowAnonymous]
        //public async Task<IActionResult> ForgotPassword(PasswordResetModel resetModel)
        //{
        //    if (string.IsNullOrEmpty(resetModel.Email))
        //    {
        //        ViewBag.Error = "Email cannot be null";
        //        return View();
        //    }
        //    if (ModelState.IsValid)
        //    {
        //        var Email = resetModel.Email?.Trim();
        //        var (loginResult, user) = await _userRegistrationService.ValidateUserEmailAsync(Email);

        //        switch (loginResult)
        //        {
        //            case LoginResults.Successful:
        //                {
        //                    resetModel.UserId = user.Id;
        //                    Guid guid = Guid.NewGuid();
        //                    resetModel.ResetRequestToken = guid.ToString();
        //                    var result = await _userRegistrationService.ForgotPasswordAsync(resetModel);
        //                    if (result.StatusCode == 200)
        //                    {
        //                        // send mail service and show toast message
        //                        await _usersService.SendPasswordResetList(resetModel.ResetRequestToken, resetModel.Email);
        //                        return RedirectToAction("Index");
        //                    }
        //                    break;
        //                }
        //            case LoginResults.NotExist:
        //                ViewBag.Error = "Invalid login credentials.";
        //                ModelState.AddModelError("", "Invalid login credentials.");
        //                break;
        //            case LoginResults.Deleted:
        //                ViewBag.Error = "Invalid login credentials.";
        //                ModelState.AddModelError("", "Invalid login credentials.");
        //                break;
        //            case LoginResults.NotActive:
        //                ViewBag.Error = "Account not activated.";
        //                ModelState.AddModelError("", "Account not activated.");
        //                break;
        //            case LoginResults.Blocked:
        //                ViewBag.Error = "Account blocked! Please contact our support team.";
        //                ModelState.AddModelError("", "Account blocked! Please contact our support team.");
        //                break;
        //            case LoginResults.LockedOut:
        //                ViewBag.Error = "Account temporarily locked.";
        //                ModelState.AddModelError("", "Account temporarily locked.");
        //                break;
        //            default:
        //                ViewBag.Error = "Invalid login credentials.";
        //                ModelState.AddModelError("", "Invalid login credentials.");
        //                break;
        //        }
        //    }
        //    return View();
        //}

        [HttpPost]
        [LogUserActivity("forgot password")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(string Email)
        {
            if (string.IsNullOrEmpty(Email))
            {
                ViewBag.Error = "Email can't be empty.";
                return View();
            }
            if (ModelState.IsValid)
            {
                Email = Email?.Trim();
                var (loginResult, user) = await _userRegistrationService.ValidateUserEmailAsync(Email);
                PasswordResetModel resetModel = new PasswordResetModel() { Email = Email };
                switch (loginResult)
                {
                    case LoginResults.Successful:
                        {
                            resetModel.UserId = user.Id;

                            //Guid guid = Guid.NewGuid();
                            //resetModel.ResetRequestToken = guid.ToString();

                            var guid = GenerateTokenKey();
                            resetModel.ResetRequestToken = guid;

                            resetModel.UserType = "ADMIN";
                            var result = await _userRegistrationService.ForgotPasswordAsync(resetModel);
                            if (result.StatusCode == 200)
                            {
                                await _usersService.SendPasswordResetList(resetModel.ResetRequestToken, resetModel.Email);
                                _notyfService.Success("Password reset link has been sent to your email.");
                                return RedirectToAction("Index");
                            }
                            break;
                        }
                    case LoginResults.NotExist:
                        ViewBag.Error = "Email doesn't exist. Invalid credentials.";
                        ModelState.AddModelError("", "Email doesn't exist. Invalid credentials.");
                        break;
                    case LoginResults.Deleted:
                        ViewBag.Error = "Email is deleted.";
                        ModelState.AddModelError("", "Email is deleted.");
                        break;
                    case LoginResults.NotActive:
                        ViewBag.Error = "Account not activated.";
                        ModelState.AddModelError("", "Account not activated.");
                        break;
                    case LoginResults.Blocked:
                        ViewBag.Error = "Account blocked! Please contact our support team.";
                        ModelState.AddModelError("", "Account blocked! Please contact our support team.");
                        break;
                    case LoginResults.LockedOut:
                        ViewBag.Error = "Account temporarily locked.";
                        ModelState.AddModelError("", "Account temporarily locked.");
                        break;
                    default:
                        ViewBag.Error = "Invalid login credentials.";
                        ModelState.AddModelError("", "Invalid login credentials.");
                        break;
                }
            }
            return View();
        }

        /// <summary>
        /// Logouts the.
        /// </summary>
        /// <returns>A Task.</returns>
        [Route("[area]/logout")]
        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {

            await _userCookieAuthService.SignOutAsync();

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "2FAdminAuth")]
        public IActionResult AccountSecretKey()
        {
            var email = _Users.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email).Value;

            EnableAuthenticatorModel Model = new EnableAuthenticatorModel();
            var userEmail = email;
            var twoFactorAuthenticator = new TwoFactorAuthenticator();
            var secretCode = Guid.NewGuid().ToString().Replace("-", "")[0..10];
            var accountSecretKey = $"{secretCode}-{userEmail}";
            var setupCode = twoFactorAuthenticator.GenerateSetupCode("2FA", userEmail,
                Encoding.ASCII.GetBytes(accountSecretKey));
            Model.SharedKey = setupCode.ManualEntryKey;
            string AuthenticatorUriFormat = string.Format("otpauth://totp/twofact:{0}?secret={1}&issuer=twofact&digits=6", userEmail, setupCode.ManualEntryKey);
            Model.AuthenticatorUri = AuthenticatorUriFormat;

            UpdateAccountSecretKey(userEmail, accountSecretKey);
            _userCookieAuthService.SignOutAsync();
            return PartialView("Secretkey", Model);


        }
        [Authorize(Roles = "2FAdminAuth")]
        public void UpdateAccountSecretKey(string email, string Accountsecretkey)
        {
            _userRegistrationService.UpdateAccountSecretKey(email, Accountsecretkey);

        }
        [HttpGet]
        [Authorize(Roles = "AdminEmailValidateor")]
        public async Task<IActionResult> TokenVerification()
        {
            var UserName = _Users.Claims.Where(x => x.Type == ClaimTypes.Name).FirstOrDefault()?.Value;
            var Email = _Users.Claims.Where(x => x.Type == ClaimTypes.Email).FirstOrDefault()?.Value;
            var data = await _userRegistrationService.GetOtpByUsernameAsync(UserName);
            ViewBag.countdown = data.ExpiredDate.ToString();
            ViewBag.email = data.Email;
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "AdminEmailValidateor")]
        public async Task<IActionResult> TokenVerification(string OTP)
        {
            if (string.IsNullOrEmpty(OTP))
            {
                ViewBag.Error = "Please Enter OTP";
                ModelState.AddModelError("", "Please Enter OTP");
                return View();
            }

            var UserName = _Users.Claims.Where(x => x.Type == ClaimTypes.Name).FirstOrDefault()?.Value;
            var Email = _Users.Claims.Where(x => x.Type == ClaimTypes.Email).FirstOrDefault()?.Value;
            var data = await _userRegistrationService.GetOtpByUsernameAsync(UserName);
            ViewBag.countdown = data.ExpiredDate;
            if (data == null) return View();
            if (data.ExpiredUtcDate < DateTime.UtcNow)
            {
                ViewBag.Error = "Otp Expired";
                ModelState.AddModelError("", "Otp Expired");
                return View();
            }
            if (data.OtpAttemptCount > 3)
            {
                ViewBag.countdown = data.ExpiredDate.AddDays(1);
                ViewBag.Error = "Otp Expired";
                ModelState.AddModelError("", "Otp Expired");
                return View();
            }
            if (data.VerificationCode != OTP)
            {
                ViewBag.Error = "Invalid OTP";
                ModelState.AddModelError("", "Invalid OTP");
                return View();
            }
            _userRegistrationService.UpdateEmailConfirm(UserName);

            var user = await _userRegistrationService.GetUserByEmail(Email);
            if (user != null)
            {
                var result = await _userCookieAuthService.LoginLogout(user);
                if (result)
                    return new RedirectResult("/admin/AdminDashboard/Index");
            }

            await _userCookieAuthService.SignOutAsync();
            return View("index", "Login");
        }

        public static string GenerateTokenKey(int tokenLength = 128)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var bytesBuffer = new byte[tokenLength];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(bytesBuffer);
            }
            var charsBuffer = new char[tokenLength];
            for (int i = 0; i < tokenLength; i++)
            {
                charsBuffer[i] = chars[bytesBuffer[i] % chars.Length];
            }
            return new string(charsBuffer);
        }
    }
}
