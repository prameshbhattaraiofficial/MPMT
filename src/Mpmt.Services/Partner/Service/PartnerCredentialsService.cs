using Microsoft.AspNetCore.Http;
using Mpmt.Core.Common;
using Mpmt.Core.Common.Helpers;
using Mpmt.Core.Dtos;
using Mpmt.Core.Dtos.Partner;
using Mpmt.Data.Repositories.Partner.IRepository;
using Mpmt.Services.Partner.IService;
using Mpmts.Core.Dtos;
using Mpmt.Core.Extensions;
using System.Security.Claims;
using Mpmt.Core.ViewModel.ChangePassword;
using Mpmt.Data.Repositories.Partner;
using Mpmt.Data.Common;
using Mpmt.Data.Repositories.PartnerEmployee;
using Mpmt.Core.Dtos.PartnerEmployee;
using Mpmt.Core.Events;
using Mpmt.Services.Authentication;
using DocumentFormat.OpenXml.Spreadsheet;
using Org.BouncyCastle.Asn1.Ocsp;
using DocumentFormat.OpenXml.Office2016.Excel;

namespace Mpmt.Services.Partner.Service
{
    /// <summary>
    /// The partner credentials service.
    /// </summary>
    public class PartnerCredentialsService : IPartnerCredentialsService
    {
        private readonly IEventPublisher _eventPublisher;
        private readonly IPartnerEmployeeRepo _partnerEmployeeRepo;
        private readonly IPartnerRepository _partnerRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPartnerCredentialsRepository _partnerCredentialsRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="PartnerCredentialsService"/> class.
        /// </summary>
        /// <param name="httpContextAccessor">The http context accessor.</param>
        /// <param name="partnerCredentialsRepository">The partner credentials repository.</param>
        public PartnerCredentialsService(
            IEventPublisher eventPublisher,
            IPartnerEmployeeRepo partnerEmployeeRepo,
            IPartnerRepository partnerRepository,
            IHttpContextAccessor httpContextAccessor,
            IPartnerCredentialsRepository partnerCredentialsRepository)
        {
            _eventPublisher = eventPublisher;
            _partnerEmployeeRepo = partnerEmployeeRepo;
            _partnerRepository = partnerRepository;
            _httpContextAccessor = httpContextAccessor;
            _partnerCredentialsRepository = partnerCredentialsRepository;
        }

        /// <summary>
        /// Adds the credentials async.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>A Task.</returns>
        public async Task<MpmtResult> AddCredentialsAsync(PartnerCredentialInsertRequest request)
        {
            var result = new MpmtResult();
            if (string.IsNullOrWhiteSpace(request.PartnerCode))
            {
                result.AddError(400, "PartnerCode is required");
                return result;
            }

            if (string.IsNullOrWhiteSpace(request.ApiUserName))
            {
                result.AddError(400, "ApiUserName is required");
                return result;
            }

            if (request.IPAddress == null || request.IPAddress.Any(ip => !CommonHelper.IsValidIpAddress(ip.ToString())))
            {
                result.AddError(400, "Invalid IPAddress");
                return result;
            }
            var multpleipaddress = "";
            foreach (var item in request.IPAddress)
            {
                multpleipaddress = multpleipaddress == "" ? item.ToString() : (multpleipaddress + "," + item.ToString());

            }

            var creds = new PartnerCredential
            {
                PartnerCode = request.PartnerCode,
                ApiUserName = request.ApiUserName,
                IPAddress = multpleipaddress,
                IsActive = request.IsActive,
                ApiPassword = PasswordUtils.GeneratePassword(16),
                ApiKey = PasswordUtils.GeneratePassword(64)
            };

            (creds.SystemPublicKey, creds.SystemPrivateKey) = RsaCryptoUtils.GenerateRSAKeyPairPem(2048);
            (creds.UserPublicKey, creds.UserPrivateKey) = RsaCryptoUtils.GenerateRSAKeyPairPem(2048);

            creds.CreatedByName = GetLoggedInUserName();

            var addResult = await _partnerCredentialsRepository.InsertCredentialsAsync(creds);

            result = addResult.MapToMpmtResult();

            return result;
        }

        /// <summary>
        /// Gets the partner credentials by id async.
        /// </summary>
        /// <param name="credentialId">The credential id.</param>
        /// <returns>A Task.</returns>
        public async Task<PartnerCredential> GetPartnerCredentialsByIdAsync(string credentialId)
        {
            ArgumentNullException.ThrowIfNull(credentialId);

            return await _partnerCredentialsRepository.GetCredentialsByIdAsync(credentialId);
        }
        /// <summary>
        /// Gets the credentials by partner code async.
        /// </summary>
        /// <param name="PartnerCode">The partner code.</param>
        /// <returns>A Task.</returns>
        public async Task<PartnerCredential> GetCredentialsByPartnerCodeAsync(string PartnerCode)
        {
            ArgumentNullException.ThrowIfNull(PartnerCode);

            return await _partnerCredentialsRepository.GetCredentialsByPartnerCodeAsync(PartnerCode);
        }

        /// <summary>
        /// Regenerates the api key async.
        /// </summary>
        /// <param name="partnerCode">The partner code.</param>
        /// <param name="credentialId">The credential id.</param>
        /// <returns>A Task.</returns>
        public async Task<(SprocMessage, string apiKey)> RegenerateApiKeyAsync(string partnerCode, string credentialId)
        {
            ArgumentNullException.ThrowIfNull(partnerCode);
            ArgumentNullException.ThrowIfNull(credentialId);

            var apiKey = PasswordUtils.GeneratePassword(64);
            var loggedInUserName = GetLoggedInUserName();
            var sprocMessage = await _partnerCredentialsRepository.UpdateApiKeyAsync(partnerCode, credentialId, apiKey, loggedInUserName: loggedInUserName);

            if (sprocMessage.StatusCode != 200)
                return (sprocMessage, default);

            return (sprocMessage, apiKey);
        }

        /// <summary>
        /// Regenerates the api password async.
        /// </summary>
        /// <param name="partnerCode">The partner code.</param>
        /// <param name="credentialId">The credential id.</param>
        /// <returns>A Task.</returns>
        public async Task<(SprocMessage, string apiPassword)> RegenerateApiPasswordAsync(string partnerCode, string credentialId)
        {
            ArgumentNullException.ThrowIfNull(partnerCode);
            ArgumentNullException.ThrowIfNull(credentialId);

            var password = PasswordUtils.GeneratePassword(16);
            var loggedInUserName = GetLoggedInUserName();
            var sprocMessage = await _partnerCredentialsRepository.UpdateApiPasswordAsync(partnerCode, credentialId, password, loggedInUserName: loggedInUserName);

            if (sprocMessage.StatusCode != 200)
                return (sprocMessage, default);

            return (sprocMessage, password);
        }

        /// <summary>
        /// Regenerates the system rsa key pair async.
        /// </summary>
        /// <param name="partnerCode">The partner code.</param>
        /// <param name="credentialId">The credential id.</param>
        /// <returns>A Task.</returns>
        public async Task<(SprocMessage, string privateKey, string publicKey)> RegenerateSystemRsaKeyPairAsync(string partnerCode, string credentialId)
        {
            ArgumentNullException.ThrowIfNull(partnerCode);
            ArgumentNullException.ThrowIfNull(credentialId);

            var loggedInUserName = GetLoggedInUserName();

            var (systemPublicKey, systemPrivateKey) = RsaCryptoUtils.GenerateRSAKeyPairPem(2048);
            var sprocMessage = await _partnerCredentialsRepository
                .UpdateSystemRsaKeyPairAsync(partnerCode, credentialId, systemPrivateKey, systemPublicKey, loggedInUserName: loggedInUserName);

            if (sprocMessage.StatusCode != 200)
                return (sprocMessage, default, default);

            return (sprocMessage, systemPrivateKey, systemPublicKey);
        }

        /// <summary>
        /// Regenerates the user rsa key pair async.
        /// </summary>
        /// <param name="partnerCode">The partner code.</param>
        /// <param name="credentialId">The credential id.</param>
        /// <returns>A Task.</returns>
        public async Task<(SprocMessage, string privateKey, string publicKey)> RegenerateUserRsaKeyPairAsync(string partnerCode, string credentialId)
        {
            ArgumentNullException.ThrowIfNull(partnerCode);
            ArgumentNullException.ThrowIfNull(credentialId);

            var loggedInUserName = GetLoggedInUserName();

            var (userPublicKey, userPrivateKey) = RsaCryptoUtils.GenerateRSAKeyPairPem(2048);
            var sprocMessage = await _partnerCredentialsRepository
                .UpdateUserRsaKeyPairAsync(partnerCode, credentialId, userPrivateKey, userPublicKey, loggedInUserName: loggedInUserName);

            if (sprocMessage.StatusCode != 200)
                return (sprocMessage, default, default);

            return (sprocMessage, userPrivateKey, userPublicKey);
        }

        /// <summary>
        /// Updates the credentials async.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>A Task.</returns>
        public async Task<MpmtResult> UpdateCredentialsAsync(PartnerCredentialUpdateRequest request)
        {
            var result = new MpmtResult();

            if (string.IsNullOrWhiteSpace(request.PartnerCode))
            {
                result.AddError(400, "PartnerCode is required.");
                return result;
            }

            if (string.IsNullOrWhiteSpace(request.ApiUserName))
            {
                result.AddError(400, "ApiUserName is required.");
                return result;
            }

            var multpleipaddress = "";
            foreach (var item in request.IPAddress)
            {
                if (!CommonHelper.IsValidIpAddress(item.ToString()))
                {
                    result.AddError(400, "Invalid IPAddress");
                    return result;
                }
                multpleipaddress = multpleipaddress == "" ? item.ToString() : (multpleipaddress + "," + item.ToString());
            }

            var creds = new PartnerCredential
            {
                PartnerCode = request.PartnerCode,
                ApiUserName = request.ApiUserName,
                IPAddress = multpleipaddress,
                IsActive = request.IsActive,
                CredentialId = request.CredentialId,
                UpdatedByName = GetLoggedInUserName()
            };

            var updateResult = await _partnerCredentialsRepository.UpdateCredentialsAsync(creds);
            result = updateResult.MapToMpmtResult();

            return result;
        }

        /// <summary>
        /// Gets the logged in user name.
        /// </summary>
        /// <returns>A string.</returns>
        private string GetLoggedInUserName()
        {
            return _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Name)?.Value;
        }
        public async Task<SprocMessage> ChangePartnerPassword(PartnerChangePasswordVM changepassword)
        {
            var response = new SprocMessage()
            {
                StatusCode = 400,
                MsgText = "Something Went wrong",
            };
            var partnerId = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Id")?.Value;
            var userType = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "UserType")?.Value;
            var Partnercod = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "PartnerCode")?.Value;
            var UserName = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
            int.TryParse(partnerId, out int Id);
            bool PasswordCheck = false;
            string PartnerCode = "";

            if (Id < 0)
            {
                response.StatusCode = 400;
                response.MsgText = "Permission Denied";
                return response;
            }

            if (userType == "Partner")
            {
                var Partner = await _partnerRepository.GetPartnerByIdAsync(Id);
                if (Partner == null)
                {
                    response.StatusCode = 400;
                    response.MsgText = "Invalid Credentials";
                    return response;

                }
                PartnerCode = Partner.PartnerCode;
                PasswordCheck = HashUtils.CheckEqualBase64HashHmacSha512(changepassword.OldPassword, Partner.PasswordHash, Partner.PasswordSalt);



                var newpasswordsameasold = HashUtils.CheckEqualBase64HashHmacSha512(changepassword.NewPassword, Partner.PasswordHash, Partner.PasswordSalt);

                if (newpasswordsameasold)
                {
                    response.StatusCode = 400;
                    response.MsgText = "New Password cant be same as old one";
                    return response;
                }

                if (PasswordCheck)
                {
                    var passwordSalt = CryptoUtils.GenerateKeySalt();
                    var passwordHash = CryptoUtils.HashHmacSha512Base64(changepassword.NewPassword, passwordSalt);

                    var request = new AppPartner()
                    {
                        Event = "CP",
                        PasswordHash = passwordHash,
                        PasswordSalt = passwordSalt,
                        Id = Id,
                        PartnerCode = PartnerCode,
                        UpdatedById = Id,
                        UpdatedByName = UserName

                    };

                    response = await _partnerRepository.PartnerChangePasswordAsync(request);

                    // expire session on change password successful
                    if (response.StatusCode == 200)
                        await _eventPublisher.PublishAsync(new EntityUpdatedEvent<SessionAuthExpiration>(
                            new SessionAuthExpiration { UserUniqueId = Partner.UserGuid.ToString(), ExpireBefore = DateTime.UtcNow }));
                }
                else
                {
                    response.StatusCode = 400;
                    response.MsgText = "Wrong Password";
                }

                return response;
            }

            if (userType == "PartnerEmployee")
            {
                var PartnerEmployee = await _partnerEmployeeRepo.GetPartnerEmployeeByIdAsync(Id, Partnercod);
                if (PartnerEmployee == null)
                {
                    response.StatusCode = 400;
                    response.MsgText = "Invalid Credentials";
                    return response;

                }

                PasswordCheck = HashUtils.CheckEqualBase64HashHmacSha512(changepassword.OldPassword, PartnerEmployee.PasswordHash, PartnerEmployee.PasswordSalt);

                var newpasswordsameasold = HashUtils.CheckEqualBase64HashHmacSha512(changepassword.NewPassword, PartnerEmployee.PasswordHash, PartnerEmployee.PasswordSalt);

                if (newpasswordsameasold)
                {
                    response.StatusCode = 400;
                    response.MsgText = "New Password cant be same as old one";
                    return response;
                }

                if (PasswordCheck)
                {
                    var passwordSalt = CryptoUtils.GenerateKeySalt();
                    var passwordHash = CryptoUtils.HashHmacSha512Base64(changepassword.NewPassword, passwordSalt);

                    var request = new IUDPartnerEmployee()
                    {
                        Event = "CP",
                        PasswordHash = passwordHash,
                        PasswordSalt = passwordSalt,
                        Id = Id,
                        PartnerCode = Partnercod,
                        UpdatedById = Id,
                        UpdatedByName = UserName
                    };

                    response = await _partnerEmployeeRepo.PartnerEmployeeChangePasswordAsync(request);
                    // expire session on change password successful
                    if (response.StatusCode == 200)
                        await _eventPublisher.PublishAsync(new EntityUpdatedEvent<SessionAuthExpiration>(
                            new SessionAuthExpiration { UserUniqueId = PartnerEmployee.UserGuid.ToString(), ExpireBefore = DateTime.UtcNow }));
                }
                else
                {
                    response.StatusCode = 400;
                    response.MsgText = "Wrong Password";
                }

                return response;
            }

            response.StatusCode = 400;
            response.MsgText = "Permission Denied";
            return response;
        }

       
    }
}
