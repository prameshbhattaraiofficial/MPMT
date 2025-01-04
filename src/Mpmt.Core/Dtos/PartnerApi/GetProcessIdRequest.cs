using System.ComponentModel.DataAnnotations;

namespace Mpmt.Core.Dtos.PartnerApi
{
    public class GetProcessIdRequest
    {
        [Required(ErrorMessage = "ApiUserName is required")]
        public string ApiUserName { get; set; }

        [Required(ErrorMessage = "ReferenceId is required")]
        public string ReferenceId { get; set; }

        [Required(ErrorMessage = "Signature is required")]
        public string Signature { get; set; }
    }
}
