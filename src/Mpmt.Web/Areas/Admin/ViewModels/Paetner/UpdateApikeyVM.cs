namespace Mpmt.Web.Areas.Admin.ViewModels.Paetner
{
    /// <summary>
    /// The update apikey v m.
    /// </summary>
    public class UpdateApikeyVM
    {
        /// <summary>
        /// Gets or sets the apikey.
        /// </summary>
        public string Apikey { get; set; }
        /// <summary>
        /// Gets or sets the credential id.
        /// </summary>
        public string CredentialId { get; set; }
        /// <summary>
        /// Gets or sets the partner code.
        /// </summary>
        public string PartnerCode { get; set; }
    }

    public class UpdateApikeyAgentVM
    {
        /// <summary>
        /// Gets or sets the apikey.
        /// </summary>
        public string Apikey { get; set; }
        /// <summary>
        /// Gets or sets the credential id.
        /// </summary>
        public string CredentialId { get; set; }
        /// <summary>
        /// Gets or sets the partner code.
        /// </summary>
        public string AgentCode { get; set; }
    }
}
