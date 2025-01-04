using AspNetCoreHero.ToastNotification.Abstractions;
using Google.Authenticator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Mpmt.Agent.Models;
using Mpmt.Core.Domain;
using Mpmt.Core.Dtos.CashAgent;
using Mpmt.Services.Authentication;
using Mpmt.Services.CashAgents;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Mpmt.Agent.Controllers;

/// <summary>
/// The login controller.
/// </summary>
public class LoginController : Controller
{
    private readonly ICashAgentEmployee _cashAgentEmployee;
    private readonly IAgentCookiesAuthService _agentCookiesAuthService;
    private readonly IAgentRegistrationService _agentRegistrationService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly INotyfService _notyfService;
    private readonly ClaimsPrincipal _Users;

    public LoginController(ICashAgentEmployee cashAgentEmployee, IAgentCookiesAuthService agentCookiesAuthService,
        IAgentRegistrationService agentRegistrationService,
        IHttpContextAccessor httpContextAccessor,
        INotyfService notyfService)
    {
        _cashAgentEmployee = cashAgentEmployee;
        _agentCookiesAuthService = agentCookiesAuthService;
        _agentRegistrationService = agentRegistrationService;
        _httpContextAccessor = httpContextAccessor;
        _Users = _httpContextAccessor.HttpContext.User;
        _notyfService = notyfService;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Index(string error = "")
    {
        if (_Users.Identity.IsAuthenticated)
        {
            if (User.IsInRole("AgentAccess"))
            {
                return RedirectToAction("Index", "Home");
            }
            await _agentCookiesAuthService.SignOutAsync();
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

            if (!string.IsNullOrEmpty(redirecturl) && area == "Agent")
            {
                await _agentCookiesAuthService.SignOutAsync();
                return RedirectToAction("Index", "Login", new { Area = "Agent" });
            }
        }
        if (!string.IsNullOrEmpty(error))
        {
            ViewBag.Error = error;
        }

        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Index(AgentLoginModel model, string returnUrl)
    {
        if (string.IsNullOrEmpty(model.UsernameOrPhoneNumber))
        {
            ViewBag.Error = "UserName or PhoneNumber cannot be null";
            return View();
        }

        if (string.IsNullOrEmpty(model.Password))
        {
            ViewBag.Error = "Password cannot be null";
            return View();
        }

        if (ModelState.IsValid)
        {
            var userNameOrPhoneNumber = model.UsernameOrPhoneNumber?.Trim();
            var agentOremp = await _cashAgentEmployee.CheckAgentOrEmployee(userNameOrPhoneNumber);
            if (agentOremp == "Agent")
            {
                var (loginResult, user) = await _agentRegistrationService.ValidateAgentAsync(userNameOrPhoneNumber, model.Password);

                switch (loginResult)
                {
                    case LoginResults.Successful:
                        if (string.IsNullOrEmpty(model.Code) && !user.Is2FAAuthenticated)
                        {
                            if (await _agentCookiesAuthService.NormalAgentSignInAsync(user, "2FAgentAuth"))
                                return RedirectToAction("AccountSecretKey", "Login");
                        }

                        if (string.IsNullOrEmpty(model.Code) && user.Is2FAAuthenticated)
                        {
                            ViewBag.Error = "Code cannot be null";
                            return View();
                        }

                        return await _agentCookiesAuthService.SignInAsync(user, returnUrl, model.Code, isPersistent: model.RememberMe);

                    case LoginResults.InitiateFactorAuthentication:
                        if (await _agentCookiesAuthService.NormalAgentSignInAsync(user, "2FAgentAuth"))
                            return RedirectToAction("AccountSecretKey", "Login");
                        break;

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
            else if (agentOremp == "Employee")
            {
                var (loginResult, user) = await _agentRegistrationService.ValidateAgentEmployeeAsync(userNameOrPhoneNumber, model.Password);

                switch (loginResult)
                {
                    case LoginResults.Successful:
                        if (string.IsNullOrEmpty(model.Code) && !user.Is2FAAuthenticated)
                        {
                            if (await _agentCookiesAuthService.NormalAgentEmployeeSignInAsync(user, "2FAgentAuthEmployee"))
                                return RedirectToAction("AccountSecretKeyEmployee", "Login");
                        }

                        if (string.IsNullOrEmpty(model.Code) && user.Is2FAAuthenticated)
                        {
                            ViewBag.Error = "Code cannot be null";
                            return View();
                        }

                        return await _agentCookiesAuthService.SignInAgentEmployeeAsync(user, returnUrl, model.Code, isPersistent: model.RememberMe);

                    case LoginResults.InitiateFactorAuthentication:
                        if (await _agentCookiesAuthService.NormalAgentEmployeeSignInAsync(user, "2FAgentAuthEmployee"))
                            return RedirectToAction("AccountSecretKeyEmployee", "Login");

                        break;

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
            else
            {
                ViewBag.Error = "Invalid login credentials";
                return View();
            }
        }

        return View("Index", model);
    }

    [Authorize(Roles = "2FAgentAuth")]
    public async Task<IActionResult> AccountSecretKey()
    {
        var Agentcode = _Users.Claims?.FirstOrDefault(x => x.Type == "AgentCode").Value;

        EnableAuthenticatorModel Model = new EnableAuthenticatorModel();
        var userAgentcode = Agentcode;
        var twoFactorAuthenticator = new TwoFactorAuthenticator();
        var secretCode = Guid.NewGuid().ToString().Replace("-", "")[0..10];
        var accountSecretKey = $"{secretCode}-{userAgentcode}";
        var setupCode = twoFactorAuthenticator.GenerateSetupCode("2FA", userAgentcode,
            Encoding.ASCII.GetBytes(accountSecretKey));
        Model.SharedKey = setupCode.ManualEntryKey;
        string AuthenticatorUriFormat = string.Format("otpauth://totp/twofact:{0}?secret={1}&issuer=twofact&digits=6", userAgentcode, setupCode.ManualEntryKey);
        Model.AuthenticatorUri = AuthenticatorUriFormat;

        await UpdateAccountSecretKey(userAgentcode, accountSecretKey);
        await _agentCookiesAuthService.SignOutAsync();
        return PartialView("Secretkey", Model);
    }

    [Authorize(Roles = "2FAgentAuth")]
    public async Task UpdateAccountSecretKey(string Agentcode, string Accountsecretkey)
    {
        await _agentRegistrationService.UpdateAccountSecretKeyAsync(Agentcode, Accountsecretkey);
    }

    [Authorize(Roles = "2FAgentAuthEmployee")]
    public async Task<IActionResult> AccountSecretKeyEmployee()
    {
        var Agentcode = _Users.Claims?.FirstOrDefault(x => x.Type == "AgentCode").Value;
        var EmployeeId = _Users.Claims?.FirstOrDefault(x => x.Type == "EmployeeId").Value;
        int.TryParse(EmployeeId, out var Employee);

        EnableAuthenticatorModel Model = new EnableAuthenticatorModel();
        var userAgentcode = Agentcode;
        var twoFactorAuthenticator = new TwoFactorAuthenticator();
        var secretCode = Guid.NewGuid().ToString().Replace("-", "")[0..10];
        var accountSecretKey = $"{secretCode}-{userAgentcode}";
        var setupCode = twoFactorAuthenticator.GenerateSetupCode("2FA", userAgentcode,
            Encoding.ASCII.GetBytes(accountSecretKey));
        Model.SharedKey = setupCode.ManualEntryKey;
        string AuthenticatorUriFormat = string.Format("otpauth://totp/twofact:{0}?secret={1}&issuer=twofact&digits=6", userAgentcode, setupCode.ManualEntryKey);
        Model.AuthenticatorUri = AuthenticatorUriFormat;

        await SecretKeyEmployee(userAgentcode, accountSecretKey, Employee);
        await _agentCookiesAuthService.SignOutAsync();
        return PartialView("Secretkey", Model);
    }

    [Authorize(Roles = "2FAgentAuthEmployee")]
    public async Task SecretKeyEmployee(string Agentcode, string Accountsecretkey, int EmployeeId)

    {
        await _cashAgentEmployee.UpdateEmployeeAccountSecretKeyAsync(Agentcode, Accountsecretkey, EmployeeId);
    }

    [Authorize(Roles = "ResetPassword")]
    public async Task<IActionResult> PasswordReset(ForgotPassword forgotPassword)
    {
        return View(forgotPassword);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "ResetPassword")]
    public async Task<IActionResult> PasswordReset(ForgotPassword forgotPassword, string pass)
    {
        if (string.IsNullOrEmpty(forgotPassword.OTP))
        {
            ViewBag.Error = "Please Enter OTP";
            ModelState.AddModelError("", "Please Enter OTP");
            Response.StatusCode = (int)(HttpStatusCode.BadRequest);
            return PartialView(forgotPassword);
        }
        var agentCode = User.Claims.FirstOrDefault(x => x.Type == "AgentCode").Value;
        var userName = User.FindFirstValue(ClaimTypes.Name);
        var userType = User.FindFirstValue("UserType");
        var employeeId = User.FindFirstValue("EmployeeId");
        var data = await _agentRegistrationService.GetOtpByAgentCodeAsync(agentCode, userName, forgotPassword.PhoneNumber);
        if (data == null) return RedirectToAction("Index");
        if (data.ExpiredDate < DateTime.UtcNow)
        {
            ViewBag.Error = "Otp Expired";
            ModelState.AddModelError("", "Otp Expired");
            Response.StatusCode = (int)(HttpStatusCode.BadRequest);
            return PartialView(forgotPassword);
        }
        if (data.OtpAttemptCount > 3)
        {
            ViewBag.Error = "Otp Expired";
            ModelState.AddModelError("", "Otp Expired");
            Response.StatusCode = (int)(HttpStatusCode.BadRequest);
            return PartialView(forgotPassword);
        }
        if (data.VerificationCode != forgotPassword.OTP)
        {
            ViewBag.Error = "Invalid OTP";
            ModelState.AddModelError("", "Invalid OTP");
            Response.StatusCode = (int)(HttpStatusCode.BadRequest);
            return PartialView(forgotPassword);
        }
        if (data.VerificationCode == forgotPassword.OTP)
        {
            var guid = GenerateTokenKey();
            forgotPassword.ResetToken = guid;
            var resetPassword = new ForgotPasswordToken { ResetToken = guid, OTP = forgotPassword.OTP, AgentCode = agentCode, PhoneNumber = forgotPassword.PhoneNumber };

            var result = await _agentRegistrationService.ForgotPasswordAsync(resetPassword);

            if (result.StatusCode == 200)
            {
                var user = new AgentUser() { AgentCode = agentCode, UserName = userName, ContactNumber = forgotPassword.PhoneNumber, UserType = userType, EmployeeId = int.Parse(employeeId) };
                await _agentCookiesAuthService.SignOutAsync();
                var userPrincipal = await _agentCookiesAuthService.ChangePasswordRole(user, "PasswordChange");
                return RedirectToAction("ChangePassword", "Login", forgotPassword);
            }
        }

        return PartialView(forgotPassword);
    }

    [HttpGet]
    [Authorize(Roles = "PasswordChange")]
    public async Task<IActionResult> ChangePassword(ForgotPassword forgotPassword)
    {
        var resetModel = new ForgotPasswordToken()
        {
            AgentCode = User.Claims.FirstOrDefault(x => x.Type == "AgentCode").Value,
            ResetToken = forgotPassword.ResetToken,
            PhoneNumber = forgotPassword.PhoneNumber,
            OTP = forgotPassword.OTP
        };
        var result = await _agentRegistrationService.ResetTokenValidationAsync(resetModel);
        if (result.StatusCode == 200)
        {
            return View(forgotPassword);
        }
        else
        {
            await _agentCookiesAuthService.SignOutAsync();
            _notyfService.Error(result.MsgText);
            return RedirectToAction("Index", "Login");
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "PasswordChange")]
    public async Task<IActionResult> ChangePassword(ForgotPassword forgotPassword, string pass)
    {
        if (!ModelState.IsValid)
        {
            Response.StatusCode = (int)(HttpStatusCode.BadRequest);
            return PartialView(forgotPassword);
        }
        var resetModel = new ForgotPasswordToken()
        {
            AgentCode = User.Claims.FirstOrDefault(x => x.Type == "AgentCode").Value,
            ResetToken = forgotPassword.ResetToken,
            PhoneNumber = forgotPassword.PhoneNumber,
            OTP = forgotPassword.OTP
        };
        var result = await _agentRegistrationService.ResetTokenValidationAsync(resetModel);
        if (result.StatusCode == 200)
        {
            var responseStatus = await _agentRegistrationService.ChangeAgentPassword(forgotPassword);
            if (responseStatus.StatusCode == 200)
            {
                resetModel.IsConsumed = true;
                var results = await _agentRegistrationService.ResetTokenValidationAsync(resetModel);
                if (responseStatus.StatusCode == 200)
                {
                    await _agentCookiesAuthService.SignOutAsync();
                    _notyfService.Success(responseStatus.MsgText);
                    return RedirectToAction("Index", "Login");
                }
            }
            else
            {
                Response.StatusCode = (int)(HttpStatusCode.BadRequest);
                ViewBag.Error = responseStatus.MsgText;
                return PartialView(forgotPassword);
            }
        }
        else
        {
            Response.StatusCode = (int)(HttpStatusCode.BadRequest);
            ViewBag.Error = result.MsgText;
            return PartialView(forgotPassword);
        }
        Response.StatusCode = (int)(HttpStatusCode.BadRequest);
        ViewBag.Error = result.MsgText;
        return PartialView(forgotPassword);
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> ForgotPassword()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [AllowAnonymous]
    public async Task<IActionResult> ForgotPassword(string PhoneNumber)
    {
        if (string.IsNullOrEmpty(PhoneNumber))
        {
            ViewBag.Error = "Phone Number can't be empty.";
            return View();
        }
        if (ModelState.IsValid)
        {
            PhoneNumber = PhoneNumber?.Trim();
            var agentOrEmployee = await _cashAgentEmployee.CheckAgentOrEmployee(PhoneNumber);
            if (agentOrEmployee == "Agent")
            {
                var (loginResult, user) = await _agentRegistrationService.ValidateAgentPhoneAsync(PhoneNumber);
                switch (loginResult)
                {
                    case LoginResults.Successful:
                        {
                            var result = await _agentRegistrationService.SendOtpVerification(user);
                            if (result)
                            {
                                var userPrincipal = await _agentCookiesAuthService.ChangePasswordRole(user, "ResetPassword");
                                if (userPrincipal != null)
                                {
                                    return RedirectToAction("PasswordReset", "Login", new ForgotPassword { PhoneNumber = PhoneNumber });
                                }
                            }
                            break;
                        }
                    case LoginResults.NotExist:
                        ViewBag.Error = "Phone Number doesn't exist. Invalid credentials.";
                        ModelState.AddModelError("", "Phone Number doesn't exist. Invalid credentials.");
                        break;

                    case LoginResults.Deleted:
                        ViewBag.Error = "Account is deleted.";
                        ModelState.AddModelError("", "Account is deleted.");
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
            else if (agentOrEmployee == "Employee")
            {
                var (loginResult, user) = await _agentRegistrationService.ValidateEmployeePhoneAsync(PhoneNumber);
                switch (loginResult)
                {
                    case LoginResults.Successful:
                        {
                            var result = await _agentRegistrationService.SendOtpVerification(user);
                            if (result)
                            {
                                var userPrincipal = await _agentCookiesAuthService.ChangePasswordRole(user, "ResetPassword");
                                if (userPrincipal != null)
                                {
                                    return RedirectToAction("PasswordReset", "Login", new ForgotPassword { PhoneNumber = PhoneNumber });
                                }
                            }
                            break;
                        }
                    case LoginResults.NotExist:
                        ViewBag.Error = "Phone Number doesn't exist. Invalid credentials.";
                        ModelState.AddModelError("", "Phone Number doesn't exist. Invalid credentials.");
                        break;

                    case LoginResults.Deleted:
                        ViewBag.Error = "Account is deleted.";
                        ModelState.AddModelError("", "Account is deleted.");
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
                ViewBag.Error = "Invalid login credentials.";
                ModelState.AddModelError("", "Invalid login credentials.");
            }
        }
        return View();
    }

    [AllowAnonymous]
    public async Task<IActionResult> Logout()
    {
        await _agentCookiesAuthService.SignOutAsync();

        return RedirectToAction("Index");
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