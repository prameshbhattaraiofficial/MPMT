using System.ComponentModel.DataAnnotations;

namespace Mpmt.Web.Areas.Admin.ViewModels.Agent
{
    public class AddApiKeysAgent
    {
        /// <summary>
        /// Gets or sets the partner code.
        /// </summary>
        [Required]
        public string AgentCode { get; set; }
        /// <summary>
        /// Gets or sets the api user name.
        /// </summary>
        [Required]
        public string ApiUserName { get; set; }
        /// <summary>
        /// Gets or sets the i p address.
        /// </summary>
        [Required]
        public string[] IPAddress { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether is active.
        /// </summary>
        public bool IsActive { get; set; }
        /// <summary>
        /// Gets or sets the credential id.
        /// </summary>
        public string CredentialId { get; set; }
    }
    /// <summary>
    /// The update api keys.
    /// </summary>
    public class UpdateApiKeysAgent
    {
        /// <summary>
        /// Gets or sets the partner code.
        /// </summary>
        [Required]
        public string AgentCode { get; set; }

        /// <summary>
        /// Gets or sets the api user name.
        /// </summary>
        public string ApiUserName { get; set; }
        /// <summary>
        /// Gets or sets the i p address.
        /// </summary>
        [Required]
        public string IPAddress { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether is active.
        /// </summary>
        public bool IsActive { get; set; }
        /// <summary>
        /// Gets or sets the credential id.
        /// </summary>
        public string CredentialId { get; set; }
    }
}
