using Microsoft.AspNetCore.Http;
using Mpmt.Core.Dtos;
using Mpmt.Core.Dtos.Partner;
using Mpmt.Core.Dtos.Users;
using Mpmt.Core.Extensions;
using Mpmt.Core.Models.Mail;
using Mpmt.Core.ViewModel.ForgotPassword;
using Mpmt.Data.Repositories.Users;
using Mpmt.Services.Common;
using Mpmt.Services.Services.MailingService;
using Mpmts.Core.Dtos;
using System.Security.Claims;
using static System.Net.WebRequestMethods;

namespace Mpmt.Services.Users
{
    /// <summary>
    /// The users service.
    /// </summary>
    public class UsersService : IUsersService
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IWebHelper _webHelper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMailService _mailService;
        /// <summary>
        /// Initializes a new instance of the <see cref="UsersService"/> class.
        /// </summary>
        /// <param name="usersRepository">The users repository.</param>
        public UsersService(IUsersRepository usersRepository,
             IWebHelper webHelper,
            IHttpContextAccessor httpContextAccessor, IMailService mailService)
        {
            _usersRepository = usersRepository;
            _webHelper = webHelper;
            _httpContextAccessor = httpContextAccessor;
            _mailService = mailService;
        }

        /// <summary>
        /// Adds the user async.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>A Task.</returns>
        public async Task<MpmtResult> AddUserAsync(AppUser user)
        {
            var addUsersStatus = await _usersRepository.AddUserAsync(user);

            return addUsersStatus.MapToMpmtResult();
        }
        public async Task<SprocMessage> ChangePasswordAsync(AppUser user)
        {
            var UsersStatus = await _usersRepository.ChangePasswordAsync(user);
            return UsersStatus;
        }

        /// <summary>
        /// Checks the user exists by email async.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns>A Task.</returns>
        public async Task<bool> CheckUserExistsByEmailAsync(string email)
        {
            ArgumentNullException.ThrowIfNull(email, nameof(email));

            return await _usersRepository.CheckUserExistsByEmailAsync(email);
        }

        /// <summary>
        /// Checks the user exists by user name async.
        /// </summary>
        /// <param name="userName">The user name.</param>
        /// <returns>A Task.</returns>
        public async Task<bool> CheckUserExistsByUserNameAsync(string userName)
        {
            ArgumentNullException.ThrowIfNull(userName, nameof(userName));

            return await _usersRepository.CheckUserExistsByUserNameAsync(userName);
        }

        /// <summary>
        /// Gets the user by email async.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns>A Task.</returns>
        public async Task<AppUser> GetUserByEmailAsync(string email)
        {
            ArgumentNullException.ThrowIfNull(email, nameof(email));

            return await _usersRepository.GetUserByEmailAsync(email);
        }

        /// <summary>
        /// Gets the user by id async.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>A Task.</returns>
        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            if (id < 0) return null;

            return await _usersRepository.GetUserByIdAsync(id);
        }

        /// <summary>
        /// Gets the user by user name async.
        /// </summary>
        /// <param name="userName">The user name.</param>
        /// <returns>A Task.</returns>
        public Task<AppUser> GetUserByUserNameAsync(string userName)
        {
            ArgumentNullException.ThrowIfNull(userName, nameof(userName));

            return _usersRepository.GetUserByUserNameAsync(userName);
        }

        /// <summary>
        /// Updates the user login activity async.
        /// </summary>
        /// <param name="loginActivity">The login activity.</param>
        /// <returns>A Task.</returns>
        public Task UpdateUserLoginActivityAsync(UserLoginActivity loginActivity)
        {
            ArgumentNullException.ThrowIfNull(loginActivity, nameof(loginActivity));

            return _usersRepository.UpdateUserLoginActivityAsync(loginActivity);
        }
        /// <summary>
        /// Updates the account secret key.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="accountsecretkey">The accountsecretkey.</param>
        public void UpdateAccountSecretKey(string email, string accountsecretkey)
        {
            _usersRepository.UpdateAccountSecretKey(email, accountsecretkey);
        }
        public async Task<SprocMessage> AddLoginOtpAsync(TokenVerification tokenVerification)
        {
            var response = await _usersRepository.AddLoginOtpAsync(tokenVerification);
            return response;
        }

        public async Task<TokenVerification> GetOtpByUserNameAsync(string partnercode)
        {
            var data = await _usersRepository.GetOtpByUsernameAsync(partnercode);
            return data;
        }
        public void UpdateEmailConfirmAsync(string partnercode)
        {
            _usersRepository.UpdateEmailConfirmAsync(partnercode);

        }

        public async Task<SprocMessage> ForgotPasswordAsync(PasswordResetModel reset)
        {
            var UsersStatus = await _usersRepository.ForgotPasswordAsync(reset);
            return UsersStatus;
        }

        public string GenrateMailBodyForPasswordReset(string link)
        {
            var companyName = "MyPay Money Transfer Pvt. Ltd.";
            var companyEmail = "info@MPMT.com";

            string mailBody =
                $@"
                <p>We got a request to reset your Password.</p>
                
                <P>Tap to redirect to Password reset link: <a href=""{link}"">{link}</a><p>
                            
                <p>If you ignore this message, your password will not be changed. If you didn't request a password reset, let us know.</p>
                <p>ATTN : Please do not reply to this email. This mailbox is not monitored and you will not receive a response.</p>
                <p>{companyName},<br>
                Radhe Radhe Bhaktapur, Nepal.<br>
                {companyEmail}</p>

                <p>Warm Regards,<br>
                {companyName}</p>

                <p>CONFIDENTIALITY NOTICE: This transmittal is a confidential communication or may otherwise be privileged. If you are not the intended recipient, you are hereby notified that you have received this transmittal in error and that any review, dissemination, distribution or copying of this transmittal is strictly prohibited. If you have received this communication in error, please notify this office, and immediately delete this message and all its attachments, if any.</p>";

            return mailBody;
        }

        public async Task SendPasswordResetList(string link, string emails)
        {
            var baseurl = _webHelper.GetbaseUrl();
            var mailRequest = new MailRequestModel
            {
                MailFor = "password-reset",
                MailTo = emails,
                MailSubject = "Password Reset",
                RecipientName = "",
                Content = GenrateMailBodyForPasswordReset($"{baseurl}/admin/login/passwordreset?token=" + link)
            };
            var mailServiceModel = await _mailService.EmailSettings(mailRequest);
            Thread email = new(delegate ()
            {
                _mailService.SendMail(mailServiceModel);
            });
            email.Start();
        }

        public async Task<(SprocMessage, PasswordResetModel)> RequestTokenValidationAsync(string token)
        {
            var UsersStatus = await _usersRepository.RequestTokenValidationAsync(token);
            return UsersStatus;
        }
        public async Task<(SprocMessage, PasswordResetModel)> ResetTokenValidationAsync(PasswordResetModel resetModel)
        {
            var UsersStatus = await _usersRepository.ResetTokenValidationAsync(resetModel);
            return UsersStatus;
        }
    }
}