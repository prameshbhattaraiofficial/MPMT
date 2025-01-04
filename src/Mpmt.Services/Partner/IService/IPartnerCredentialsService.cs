using Mpmt.Core.Dtos;
using Mpmt.Core.Dtos.Partner;
using Mpmt.Core.ViewModel.ChangePassword;
using Mpmts.Core.Dtos;

namespace Mpmt.Services.Partner.IService
{
    /// <summary>
    /// The partner credentials service.
    /// </summary>
    public interface IPartnerCredentialsService
    {
        /// <summary>
        /// Gets the partner credentials by id async.
        /// </summary>
        /// <param name="credentialId">The credential id.</param>
        /// <returns>A Task.</returns>
        Task<PartnerCredential> GetPartnerCredentialsByIdAsync(string credentialId);

        /// <summary>
        /// Adds the credentials async.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>A Task.</returns>

        Task<PartnerCredential> GetCredentialsByPartnerCodeAsync(string PartnerCode);

       

        /// <summary>
        /// Adds the credentials async.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>A Task.</returns>
        Task<MpmtResult> AddCredentialsAsync(PartnerCredentialInsertRequest request);
        Task<SprocMessage> ChangePartnerPassword(PartnerChangePasswordVM changepassword);
        /// <summary>
        /// Updates the credentials async.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>A Task.</returns>
        Task<MpmtResult> UpdateCredentialsAsync(PartnerCredentialUpdateRequest request);
        /// <summary>
        /// Regenerates the system rsa key pair async.
        /// </summary>
        /// <param name="partnerCode">The partner code.</param>
        /// <param name="credentialId">The credential id.</param>
        /// <returns>A Task.</returns>
        Task<(SprocMessage, string privateKey, string publicKey)> RegenerateSystemRsaKeyPairAsync(string partnerCode, string credentialId);
        /// <summary>
        /// Regenerates the user rsa key pair async.
        /// </summary>
        /// <param name="partnerCode">The partner code.</param>
        /// <param name="credentialId">The credential id.</param>
        /// <returns>A Task.</returns>
        Task<(SprocMessage, string privateKey, string publicKey)> RegenerateUserRsaKeyPairAsync(string partnerCode, string credentialId);
        /// <summary>
        /// Regenerates the api key async.
        /// </summary>
        /// <param name="partnerCode">The partner code.</param>
        /// <param name="credentialId">The credential id.</param>
        /// <returns>A Task.</returns>
        Task<(SprocMessage, string apiKey)> RegenerateApiKeyAsync(string partnerCode, string credentialId);
        /// <summary>
        /// Regenerates the api password async.
        /// </summary>
        /// <param name="partnerCode">The partner code.</param>
        /// <param name="credentialId">The credential id.</param>
        /// <returns>A Task.</returns>
        Task<(SprocMessage, string apiPassword)> RegenerateApiPasswordAsync(string partnerCode, string credentialId);
    }
}
