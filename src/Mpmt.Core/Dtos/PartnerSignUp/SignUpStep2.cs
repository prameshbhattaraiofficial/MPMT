using Microsoft.AspNetCore.Http;
using Mpmt.Core.Common.Attribites;
using System.ComponentModel.DataAnnotations;

namespace Mpmt.Core.Dtos.PartnerSignUp
{
    public class SignUpStep2 : IValidatableObject
    {
    //   [Required(ErrorMessage = "Email is Required")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Please Input Valid Email")]
        public string Email { get; set; }
        public string Token { get; set; }


      
        public string BusinessNumber { get; set; }
        
        public string FinancialTransactionRegNo { get; set; }
       
        public string RemittancRegNumber { get; set; }

        [MaxFileSize]
        [AllowedExtensions]
        //[Required(ErrorMessage ="License is Required*")]
        public List<IFormFile> LicensedocImg { get; set; }
        public List<string> LicensedocImgPath { get; set; }
        public List<string> DeletedLicensedocImgPath { get; set; }
        public string CompanyLogoImgPath { get; set; }
        
        public string LicenseNumber { get; set; }
        [Required(ErrorMessage ="Zip code is Required*")]
        public string ZipCode { get; set; }
        [Required]
        public string OrgState { get; set; }
        [Required(ErrorMessage ="Address is Required*")]
        public string Address { get; set; }
        [Required(ErrorMessage ="Organization is Required*")]
        public string OrganizationName { get; set; }

        [Required(ErrorMessage ="Email is Required*")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Please Input Valid Email")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Please enter a valid email address.")]
        public string OrgEmail { get; set; }

        [Required(ErrorMessage ="Select the Country*")]
        public string CountryCode { get; set; }

        public string Callingcode { get; set; }
        [Required(ErrorMessage ="Enter the City*")]
        public string City { get; set; }

        public string FullAddress { get; set; }
        [Required(ErrorMessage = "Time is Required")]
        public string GMTTimeZone { get; set; }
        [Required(ErrorMessage ="Registration Number is Required*")]
        public string RegistrationNumber { get; set; }

        [Required(ErrorMessage ="Currency is required*")]
        public string SourceCurrency { get; set; }

        [MaxFileSize]
        [AllowedExtensions]
        [Required (ErrorMessage = "Please Enter the Image")]
        public IFormFile CompanyLogoImg { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(this.CompanyLogoImgPath) && this.CompanyLogoImg == null)
            {
                yield return new ValidationResult("CompanyLogoImg is Required", new string[] { "CompanyLogoImg" });
            }
            if (string.IsNullOrEmpty(this.Email))
            {
                yield return new ValidationResult("Email is Required", new string[] { "Email" });
            }

        }
    }

}
