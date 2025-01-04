using Microsoft.AspNetCore.Http;
using Mpmt.Core.Common.Attribites;
using System.ComponentModel.DataAnnotations;

namespace Mpmt.Core.Dtos.PartnerSignUp
{
    public class SignUpStep3 : IValidatableObject
    {
        [Required(ErrorMessage = "Email is Required")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Please Input Valid Email")]
        public string Email { get; set; }
        public string Token { get; set; }
        public string IdFrontImgPath { get; set; }
        public string AddressProofImgPath { get; set; }
        public string IdBackImgPath { get; set; }
        public int DocumentTypeId { get; set; }
        [Required(ErrorMessage = "Document Number is Required")]
        public string DocumentNumber { get; set; }
        [MaxFileSize]
        [AllowedExtensions]
        [Required(ErrorMessage = "Image is Required")]
        public IFormFile IdFrontImg { get; set; }
        [MaxFileSize]
        [AllowedExtensions]
        [Required(ErrorMessage = "Back Iamge is Required*")]
        public IFormFile IdBackImg { get; set; }
        [Required(ErrorMessage = "Document IssueDate is Required")]
        public DateTime? IssueDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public int AddressProofTypeId { get; set; }
        [MaxFileSize]
        [AllowedExtensions]
        [Required(ErrorMessage ="Address Image is Required*")]
        public IFormFile AddressProofImg { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(this.IdBackImgPath) && this.IdBackImg == null)
            {
                yield return new ValidationResult("IdBackImg is Required", new string[] { "IdBackImg" });
            }
            if (string.IsNullOrEmpty(this.IdFrontImgPath) && this.IdFrontImg == null)
            {
                yield return new ValidationResult("IdFrontImg is Required", new string[] { "IdFrontImg" });
            }
            if (string.IsNullOrEmpty(this.AddressProofImgPath) && this.AddressProofImg == null)
            {
                yield return new ValidationResult("AddressImg is Required", new string[] { "AddressProofImg" });
            }
        }
    }
}
