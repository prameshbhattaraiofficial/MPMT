namespace Mpmt.Core.Dtos.Partner
{
    /// <summary>
    /// The partner details.
    /// </summary>
    public class PartnerDetails
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }
        public string SN { get; set; }

        /// <summary>
        /// Gets or sets the date.
        /// </summary>

        public string CredentialId { get; set; }
        public Guid UserGuid { get; set; }

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        public string Date { get; set; }
        public string DateString { get; set; }
        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// Gets or sets the contact.
        /// </summary>
        public string Contact { get; set; }
        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Gets or sets the marchant id.
        /// </summary>
        public string MarchantId { get; set; }
        /// <summary>
        /// Gets or sets the organization name.
        /// </summary>
        public string OrganizationName { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether is active.
        /// </summary>
        public bool IsActive { get; set; }
        /// <summary>
        /// Gets or sets the created by.
        /// </summary>
        public string CreatedBy { get; set; }
        /// <summary>
        /// Gets or sets the updated by.
        /// </summary>
        public string UpdatedBy { get; set; }
        /// <summary>
        /// Gets or sets the i p address.
        /// </summary>
        public string IPAddress { get; set; }
        /// <summary>
        /// Gets or sets the a p i key.
        /// </summary>
        public string APIKey { get; set; }
        /// <summary>
        /// Gets or sets the a p i password.
        /// </summary>
        public string APIPassword { get; set; }
        /// <summary>
        /// Gets or sets the private key.
        /// </summary>
        public string PrivateKey { get; set; }
        /// <summary>
        /// Gets or sets the private password.
        /// </summary>
        public string PrivatePassword { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether status.
        /// </summary>
        public bool Status { get; set; }
        public string Shortname { get; set; }
            
    }
}
