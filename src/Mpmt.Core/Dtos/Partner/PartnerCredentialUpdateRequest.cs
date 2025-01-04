namespace Mpmt.Core.Dtos.Partner
{
    /// <summary>
    /// The partner credential update request.
    /// </summary>
    public class PartnerCredentialUpdateRequest
    {
        /// <summary>
        /// Gets or sets the partner code.
        /// </summary>
        public string PartnerCode { get; set; }
        /// <summary>
        /// Gets or sets the api user name.
        /// </summary>
        public string ApiUserName { get; set; }
        /// <summary>
        /// Gets or sets the i p address.
        /// </summary>
        public string[] IPAddress { get; set; }

        /// <summary>
        /// Gets or sets the is active.
        /// </summary>
        public string IsActive { get; set; }

        /// <summary>
        /// Gets or sets the credential id.
        /// </summary>
        public string CredentialId { get; set; }
        //  public bool IsActive { get; set; }

    }
}
