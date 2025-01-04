using Mpmt.Core.Common.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Mpmt.Core.Models.Partners.Applications
{
    public class PartnerApplicationRequest
    {
        [Required]
        [MaxLength(128)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(128)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(128)]
        public string OrganizationName { get; set; }

        [Required]
        [MaxLength(320)]
        [MpmtEmail(ErrorMessage = "Please enter a valid organization email.")]
        public string OrganizationEmail { get; set; }

        [Required]
        [MaxLength(20)]
        public string OrganizationContactNo { get; set; }

        [MaxLength(50)]
        public string Designation { get; set; }

        [Required]
        [MaxLength(10, ErrorMessage = "Country is required.")]
        public string CountryCode { get; set; }

        public string CountryName { get; set; }

        [Required]
        [MaxLength(320)]
        public string Address { get; set; }

        [MaxLength(2048)]
        public string Message { get; set; }
    }
}
