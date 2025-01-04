using AspNetCoreHero.ToastNotification.Abstractions;
using DocumentFormat.OpenXml.InkML;
using Google.Authenticator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Mpmt.Core.Domain;
using Mpmt.Core.Events;
using Mpmt.Core.ViewModel.ForgotPassword;
using Mpmt.Services.Authentication;
using Mpmt.Services.Partner;
using Mpmt.Services.Partner.IService;
using Mpmt.Web.Areas.Partner.Models.PartnerLogin;
using System.Net;
using System.Reflection;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Mpmt.Web.Areas.Partner.Controllers
{
    /// <summary>
    /// The login controller.
    /// </summary>
    public class LoginController : BasePartnerController
    {

        // private readonly IPartnerCookieAuthService _partnerCookieAuthService;
        private readonly IPartnerCookieAuthService _partnerCookieAuthService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPartnerRegistrationService _partnerRegistrationService;
        private readonly IPartnerService _partnerService;
        private readonly IEventPublisher _eventPublisher;
        private readonly INotyfService _notyfService;
        private readonly ClaimsPrincipal _Users;

        public LoginController(IPartnerCookieAuthService userCookieAuthService,
            IHttpContextAccessor httpContextAccessor,
            IPartnerRegistrationService partnerRegistrationService,
            INotyfService notyfService,
            IPartnerService partnerService,
            IEventPublisher eventPublisher)
        {
            _partnerCookieAuthService = userCookieAuthService;
            _httpContextAccessor = httpContextAccessor;
            _partnerRegistrationService = partnerRegistrationService;
            _notyfService = notyfService;
            _Users = _httpContextAccessor.HttpContext.User;
            _partnerService = partnerService;
            _eventPublisher = eventPublisher;
        }
        /// <summary>
        /// Indices the.
        /// </summary>
        /// <returns>An IActionResult.</returns>
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
                await _partnerCookieAuthService.SignOutPartnerAsync();

            }
            if (usertype == "Admin")
            {
                if (User.IsInRole("AdminAccess"))
                {
                    return RedirectToAction("Index", "adminDashboard", new { Area = "Admin" });
                }
                await _partnerCookieAuthService.SignOutPartnerAsync();
            }
            var test = _httpContextAccessor.HttpContext.Request.GetEncodedPathAndQuery();
            string redirecturl = "";
            if (!string.IsNullOrEmpty(test))
            {
                var url = test.Split('?');
                if (url.Count() >= 2)
                {
                    redirecturl = Uri.UnescapeDataString(url[1]);
                }
            }
            if (!string.IsNullOrEmpty(redirecturl))
            {
                string area = "";
                try
                {
                    area = redirecturl.Split('/')[1];
                }
                catch { }


                if (!string.IsNullOrEmpty(redirecturl) && area == "admin")
                {
                    await _partnerCookieAuthService.SignOutPartnerAsync();
                    return RedirectToAction("Index", "Login", new { Area = "Admin" });
                }
            }
            if (!string.IsNullOrEmpty(error))
            {
                ViewBag.Error = error;
            }
            return View();
        }



        [Route("[area]/logout")]
        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {

            await _partnerCookieAuthService.SignOutPartnerAsync();
            return RedirectToAction("Index");
        }
        /// <summary>
        /// Indices the.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="returnUrl">The return url.</param>
        /// <returns>A Task.</returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Index(PartnerLoginModel model, string returnUrl)
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
                var checkEmailConfirmed = await _partnerService.VerifyEmailConfirmed(userNameOrEmail);
                if (checkEmailConfirmed)
                {
                    ViewBag.Error = "Waiting for Admin approval.";
                    return View();
                }

                var parOremp = await _partnerRegistrationService.CheckPartnerOrEmployee(userNameOrEmail);
                if (parOremp == "Partner")
                {
                    var (loginResult, partneruser) = await _partnerRegistrationService.ValidatePartnerAsync(userNameOrEmail, model.Password);

                    switch (loginResult)
                    {
                        case LoginResults.Successful:
                            {
                                if (string.IsNullOrEmpty(model.Code) && (!partneruser.Is2FAAuthenticated))
                                {
                                    if (await _partnerCookieAuthService.NormalSignInPartnerAsync(partneruser, "2FAuth"))
                                    {
                                        return RedirectToAction("AccountSecretKey", "Login");
                                    }

                                }

                                if (string.IsNullOrEmpty(model.Code) && (partneruser.Is2FAAuthenticated))
                                {
                                    ViewBag.Error = "Code cannot be null";
                                    return View();
                                }

                                return await _partnerCookieAuthService.SignInPartnerAsync(partneruser, returnUrl, model.Code, isPersistent: model.RememberMe);
                            }


                        case LoginResults.InitiateFactorAuthentication:
                            {

                                if (await _partnerCookieAuthService.NormalSignInPartnerAsync(partneruser, "2FAuth"))
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
                    return View();
                }
                if (parOremp == "PartnerEmployee")
                {
                    var (loginResult, partnerEmployee) = await _partnerRegistrationService.ValidatePartnerEmployeeAsync(userNameOrEmail, model.Password);
                    switch (loginResult)
                    {
                        case LoginResults.Successful:
                            {
                                if (string.IsNullOrEmpty(model.Code) && (!partnerEmployee.Is2FAAuthenticated))
                                {
                                    if (await _partnerCookieAuthService.NormalSignInPartnerEmployeeAsync(partnerEmployee, "2FAuthE"))
                                    {
                                        return RedirectToAction("EmployeeAccountSecretKey", "Login");
                                    }


                                }

                                if (string.IsNullOrEmpty(model.Code) && (partnerEmployee.Is2FAAuthenticated))
                                {
                                    ViewBag.Error = "Code cannot be null";
                                    return View();
                                }

                                return await _partnerCookieAuthService.SignInPartnerEmployeeAsync(partnerEmployee, returnUrl, model.Code, isPersistent: model.RememberMe);
                            }


                        case LoginResults.InitiateFactorAuthentication:
                            {

                                if (await _partnerCookieAuthService.NormalSignInPartnerEmployeeAsync(partnerEmployee, "2FAuthE"))
                                {
                                    return RedirectToAction("EmployeeAccountSecretKey", "Login");
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
                    return View();
                }
                else
                {
                    ViewBag.Error = "Invalid login credentials";
                    return View();
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
                var (result, data) = await _partnerService.RequestTokenValidationAsync(token);
                if (result.StatusCode == 200)
                {
                    return View(data);
                }
            }
            _notyfService.Error("Password reset link has expired.");
            return RedirectToAction("Index");
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> PasswordReset(PasswordResetModel resetModel)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)(HttpStatusCode.BadRequest);
                return View(resetModel);
            }

            var (result, data) = await _partnerService.ResetTokenValidationAsync(resetModel);

            if (result.StatusCode == 200)
            {
                resetModel.UserId = data.UserId;
                resetModel.UserType = data.UserType;
                resetModel.UserName = data.UserName;
                resetModel.PartnerCode = data.PartnerCode;
                resetModel.UserGuid = data.UserGuid;

                var responseStatus = await _partnerService.ResetPasswordAsync(resetModel);

                if (responseStatus.StatusCode == 200)
                {
                    _notyfService.Success("Password has been updated succesfully.");
                    await _eventPublisher.PublishAsync(new EntityUpdatedEvent<SessionAuthExpiration>(
                            new SessionAuthExpiration { UserUniqueId = resetModel.UserGuid.ToString(), ExpireBefore = DateTime.UtcNow }));
                    //return RedirectToAction("Index","Login");
                    return RedirectToAction("Index", "Login", new { Area = "Partner" });
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

        [HttpPost]
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

                var parOremp = await _partnerRegistrationService.CheckPartnerOrEmployee(Email);
                if (parOremp == "Partner")
                {
                    var (loginResult, user) = await _partnerRegistrationService.ValidatePartnerEmailAsync(Email);
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
                                resetModel.UserType = "PARTNER";
                                var result = await _partnerService.ForgotPasswordAsync(resetModel);
                                if (result.StatusCode == 200)
                                {
                                    await _partnerService.SendPasswordResetList(resetModel.ResetRequestToken, resetModel.Email);
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
                else if (parOremp == "PartnerEmployee")
                {
                    var (loginResult, user) = await _partnerRegistrationService.ValidatePartnerEmployeeEmailAsync(Email);
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
                                resetModel.UserType = "EMPLOYEE";
                                var result = await _partnerService.ForgotPasswordAsync(resetModel);
                                if (result.StatusCode == 200)
                                {
                                    await _partnerService.SendPasswordResetList(resetModel.ResetRequestToken, resetModel.Email);
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
                else
                {
                    ViewBag.Error = "Email doesn't exist. Invalid credentials.";
                    return View();
                }
            }
            return View();
        }

        [Authorize(Roles = "2FAuthE")]
        public async Task<IActionResult> EmployeeAccountSecretKey()
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

            UpdateEmployeeAccountSecretKey(userEmail, accountSecretKey);
            await _partnerCookieAuthService.SignOutPartnerAsync();
            return PartialView("Secretkey", Model);


        }
        [Authorize(Roles = "2FAuthE")]
        public void UpdateEmployeeAccountSecretKey(string email, string Accountsecretkey)
        {
            _partnerCookieAuthService.UpdateEmployeeAccountSecretKey(email, Accountsecretkey);

        }

        [Authorize(Roles = "2FAuth")]
        public async Task<IActionResult> AccountSecretKey()
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
            await _partnerCookieAuthService.SignOutPartnerAsync();
            return PartialView("Secretkey", Model);


        }
        [Authorize(Roles = "2FAuth")]
        public void UpdateAccountSecretKey(string email, string Accountsecretkey)
        {
            _partnerRegistrationService.UpdateAccountSecretKey(email, Accountsecretkey);

        }
        [HttpGet]
        [Authorize(Roles = "EmployeeEmailValidateor")]
        public async Task<IActionResult> EmployeeTokenVerification()
        {
            var partnerCode = User.Claims.FirstOrDefault(x => x.Type == "PartnerCode").Value;
            var UserName = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name).Value;
            var data = await _partnerCookieAuthService.GetOtpBypartnerEmployeeCodeAsync(partnerCode, UserName, "Confirm-Email");
            ViewBag.countdown = data.ExpiredDate;
            ViewBag.email = data.Email;
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "EmployeeEmailValidateor")]
        public async Task<IActionResult> EmployeeTokenVerification(string OTP)
        {
            if (string.IsNullOrEmpty(OTP))
            {
                ViewBag.Error = "Please Enter OTP";
                ModelState.AddModelError("", "Please Enter OTP");
                return View();
            }

            var partnerCode = User.Claims.FirstOrDefault(x => x.Type == "PartnerCode").Value;
            var UserName = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name).Value;
            var data = await _partnerCookieAuthService.GetOtpBypartnerEmployeeCodeAsync(partnerCode, UserName, "Confirm-Email");
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
            _partnerCookieAuthService.UpdateEmployeeEmailConfirm(partnerCode, UserName);

            var result = await _partnerCookieAuthService.LoginLogout();
            if (result)
                return new RedirectToRouteResult("areas", new { controller = "Dashboard", action = "Index" });
            await _partnerCookieAuthService.SignOutPartnerAsync();
            return View("index", "Login");
        }

        [HttpGet]
        [Authorize(Roles = "EmailValidateor")]
        public async Task<IActionResult> TokenVerification()
        {
            var partnerCode = User.Claims.FirstOrDefault(x => x.Type == "PartnerCode").Value;
            var UserName = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name).Value;
            var data = await _partnerRegistrationService.GetOtpBypartnerCodeAsync(partnerCode, UserName, "Confirm-Email");
            ViewBag.countdown = data.ExpiredDate.ToString();
            ViewBag.email = data.Email;
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "EmailValidateor")]
        public async Task<IActionResult> TokenVerification(string OTP)
        {
            if (string.IsNullOrEmpty(OTP))
            {
                ViewBag.Error = "Please Enter OTP";
                ModelState.AddModelError("", "Please Enter OTP");
                return View();
            }

            var partnerCode = User.Claims.FirstOrDefault(x => x.Type == "PartnerCode").Value;
            var UserName = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name).Value;
            var data = await _partnerRegistrationService.GetOtpBypartnerCodeAsync(partnerCode, UserName, "Confirm-Email");
            ViewBag.countdown = data.ExpiredDate.ToString();
            if (data == null) return View();
            if (data.ExpiredDate < DateTime.UtcNow)
            {
                ViewBag.Error = "Otp Expired";
                ModelState.AddModelError("", "Otp Expired");
                return View();
            }

            if (data.OtpAttemptCount > 3)
            {
                ViewBag.countdown = data.ExpiredDate.AddDays(2).ToString();
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
            _partnerRegistrationService.UpdateEmailConfirm(partnerCode);

            var result = await _partnerCookieAuthService.partnerLoginLogout();
            if (result)
                return new RedirectToRouteResult("areas", new { controller = "Dashboard", action = "Index" });
            await _partnerCookieAuthService.SignOutPartnerAsync();
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
