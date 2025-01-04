using Mpmt.Core.Dtos.Partner;
using Mpmts.Core.Dtos;

namespace Mpmt.Data.Repositories.Partner.IRepository
{
    /// <summary>
    /// The partner credentials repository.
    /// </summary>
    public interface IPartnerCredentialsRepository
    {
        /// <summary>
        /// Gets the credentials by id async.
        /// </summary>
        /// <param name="credentialId">The credential id.</param>
        /// <returns>A Task.</returns>
        Task<PartnerCredential> GetCredentialsByIdAsync(string credentialId);

        /// <summary>
        /// Inserts the credentials async.
        /// </summary>
        /// <param name="cred">The cred.</param>
        /// <returns>A Task.</returns>

        Task<PartnerCredential> GetCredentialsByPartnerCodeAsync(string PartnerCode);

        /// <summary>
        /// Inserts the credentials async.
        /// </summary>
        /// <param name="cred">The cred.</param>
        /// <returns>A Task.</returns>
        Task<SprocMessage> InsertCredentialsAsync(PartnerCredential cred);
        /// <summary>
        /// Updates the credentials async.
        /// </summary>
        /// <param name="cred">The cred.</param>
        /// <returns>A Task.</returns>
        Task<SprocMessage> UpdateCredentialsAsync(PartnerCredential cred);
        /// <summary>
        /// Updates the api key async.
        /// </summary>
        /// <param name="partnerCode">The partner code.</param>
        /// <param name="credentialId">The credential id.</param>
        /// <param name="apiKey">The api key.</param>
        /// <param name="loggedInUserId">The logged in user id.</param>
        /// <param name="loggedInUserName">The logged in user name.</param>
        /// <returns>A Task.</returns>
        Task<SprocMessage> UpdateApiKeyAsync(string partnerCode, string credentialId, string apiKey, string loggedInUserId = null, string loggedInUserName = null);
        /// <summary>
        /// Updates the system rsa key pair async.
        /// </summary>
        /// <param name="partnerCode">The partner code.</param>
        /// <param name="credentialId">The credential id.</param>
        /// <param name="privateKey">The private key.</param>
        /// <param name="publicKey">The public key.</param>
        /// <param name="loggedInUserId">The logged in user id.</param>
        /// <param name="loggedInUserName">The logged in user name.</param>
        /// <returns>A Task.</returns>
        Task<SprocMessage> UpdateSystemRsaKeyPairAsync(string partnerCode, string credentialId, string privateKey, string publicKey, string loggedInUserId = null, string loggedInUserName = null);
        /// <summary>
        /// Updates the user rsa key pair async.
        /// </summary>
        /// <param name="partnerCode">The partner code.</param>
        /// <param name="credentialId">The credential id.</param>
        /// <param name="privateKey">The private key.</param>
        /// <param name="publicKey">The public key.</param>
        /// <param name="loggedInUserId">The logged in user id.</param>
        /// <param name="loggedInUserName">The logged in user name.</param>
        /// <returns>A Task.</returns>
        Task<SprocMessage> UpdateUserRsaKeyPairAsync(string partnerCode, string credentialId, string privateKey, string publicKey, string loggedInUserId = null, string loggedInUserName = null);
        /// <summary>
        /// Updates the api password async.
        /// </summary>
        /// <param name="partnerCode">The partner code.</param>
        /// <param name="credentialId">The credential id.</param>
        /// <param name="apiPassword">The api password.</param>
        /// <param name="loggedInUserId">The logged in user id.</param>
        /// <param name="loggedInUserName">The logged in user name.</param>
        /// <returns>A Task.</returns>
        Task<SprocMessage> UpdateApiPasswordAsync(string partnerCode, string credentialId, string apiPassword, string loggedInUserId = null, string loggedInUserName = null);
    }
}
