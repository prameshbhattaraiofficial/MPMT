namespace Mpmt.Core.Dtos.Partner
{
    /// <summary>
    /// The partner credential insert request.
    /// </summary>
    public class PartnerCredentialInsertRequest
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

        // public bool IsActive { get; set; }

    }
}
