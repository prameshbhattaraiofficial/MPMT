using System.ComponentModel.DataAnnotations;

namespace Mpmt.Web.Areas.Admin.ViewModels.Paetner
{
    public class ExportCredentialsModel
    {
        //[Required, Range(minimum:1, maximum: int.MaxValue, ErrorMessage = "Invalid PartnerId")]
        //public int PartnerId { get; set; }

        [Required(ErrorMessage = "PartnerCode is required")]
        public string PartnerCode { get; set; }

        [Required(ErrorMessage = "CredentialType is required")]
        public string CredentialType { get; set; }
    }
    public class ExportCredentialsAgentModel
    {
        //[Required, Range(minimum:1, maximum: int.MaxValue, ErrorMessage = "Invalid PartnerId")]
        //public int PartnerId { get; set; }

        [Required(ErrorMessage = "AgentCode is required")]
        public string AgentCode { get; set; }

        [Required(ErrorMessage = "CredentialType is required")]
        public string CredentialType { get; set; }
    }
}
