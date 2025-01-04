namespace Mpmt.Core.Dtos.Partner
{
    /// <summary>
    /// The partner credential.
    /// </summary>
    public class PartnerCredential
    {
        /// <summary>
        /// Gets or sets the credential id.
        /// </summary>
        public string CredentialId { get; set; }
        /// <summary>
        /// Gets or sets the partner code.
        /// </summary>
        public string PartnerCode { get; set; }
        /// <summary>
        /// Gets or sets the api user name.
        /// </summary>
        public string ApiUserName { get; set; }
        /// <summary>
        /// Gets or sets the api password.
        /// </summary>
        public string ApiPassword { get; set; }
        /// <summary>
        /// Gets or sets the api key.
        /// </summary>
        public string ApiKey { get; set; }
        /// <summary>
        /// Gets or sets the system private key.
        /// </summary>
        public string SystemPrivateKey { get; set; }
        /// <summary>
        /// Gets or sets the system public key.
        /// </summary>
        public string SystemPublicKey { get; set; }
        /// <summary>
        /// Gets or sets the user private key.
        /// </summary>
        public string UserPrivateKey { get; set; }
        /// <summary>
        /// Gets or sets the user public key.
        /// </summary>
        public string UserPublicKey { get; set; }
        /// <summary>
        /// Gets or sets the i p address.
        /// </summary>
        public string IPAddress { get; set; }

        /// <summary>
        /// Gets or sets the is active.
        /// </summary>
        public string IsActive { get; set; }
        /// <summary>
        /// Gets or sets the created by id.
        /// </summary>

        // public bool IsActive { get; set; }

        public string CreatedById { get; set; }
        /// <summary>
        /// Gets or sets the created by name.
        /// </summary>
        public string CreatedByName { get; set; }
        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        public string CreatedDate { get; set; }
        /// <summary>
        /// Gets or sets the updated by id.
        /// </summary>
        public string UpdatedById { get; set; }
        /// <summary>
        /// Gets or sets the updated by name.
        /// </summary>
        public string UpdatedByName { get; set; }
        /// <summary>
        /// Gets or sets the updated date.
        /// </summary>
        public string UpdatedDate { get; set; }
    }
}
