namespace Mpmt.Web.Areas.Admin.ViewModels.Paetner
{
    /// <summary>
    /// The update api password v m.
    /// </summary>
    public class UpdateApiPasswordVM
    {
        /// <summary>
        /// Gets or sets the api password.
        /// </summary>
        public string ApiPassword { get; set; }
        /// <summary>
        /// Gets or sets the credential id.
        /// </summary>
        public string CredentialId { get; set; }
        /// <summary>
        /// Gets or sets the partner code.
        /// </summary>
        public string PartnerCode { get; set; }
        public string ApiUserName { get; set; }
    }
    public class UpdateApiPasswordAgentVM
    {
        /// <summary>
        /// Gets or sets the api password.
        /// </summary>
        public string ApiPassword { get; set; }
        /// <summary>
        /// Gets or sets the credential id.
        /// </summary>
        public string CredentialId { get; set; }
        /// <summary>
        /// Gets or sets the partner code.
        /// </summary>
        public string AgentCode { get; set; }
        public string ApiUserName { get; set; }
    }
}
