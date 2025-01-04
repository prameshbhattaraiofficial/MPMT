namespace Mpmt.Web.Areas.Admin.ViewModels.Paetner
{
    /// <summary>
    /// The system rsakey pair v m.
    /// </summary>
    public class SystemRsakeyPairVM
    {
        /// <summary>
        /// Gets or sets the privet key.
        /// </summary>
        public string PrivetKey { get; set; }
        /// <summary>
        /// Gets or sets the public key.
        /// </summary>
        public string PublicKey { get; set; }
        /// <summary>
        /// Gets or sets the credential id.
        /// </summary>
        public string CredentialId { get; set; }
        /// <summary>
        /// Gets or sets the partner code.
        /// </summary>
        public string PartnerCode { get; set; }
    }
    public class SystemRsakeyPairAgentVM
    {
        /// <summary>
        /// Gets or sets the privet key.
        /// </summary>
        public string PrivetKey { get; set; }
        /// <summary>
        /// Gets or sets the public key.
        /// </summary>
        public string PublicKey { get; set; }
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
