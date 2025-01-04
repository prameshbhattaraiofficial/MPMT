using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mpmt.Core.Common.Attribites;
using System.ComponentModel.DataAnnotations;

namespace Mpmt.Core.Dtos.Partner
{


    public class AddPatnerRequest : IValidatableObject

    {

        public string FirstName { get; set; }
        public bool ContinueWithoutFirstName { get; set; }
        [Required(ErrorMessage = "Last Name Is Required")]
        public string LastName { get; set; }
        [Required]
        [Remote(action: "VerifyShortname", controller: "Partners")]
        public string Shortname { get; set; }

        //public bool ContinueWithoutFirstName { get; set; }
        [Required(ErrorMessage = "Phone Number is required.")]
        [RegularExpression("^[\\+]?[(]?[0-9]{3}[)]?[-\\s\\.]?[0-9]{3}[-\\s\\.]?[0-9]{4,6}$", ErrorMessage = "Please input valid phone number")]
        public string Number { get; set; }
        public string CallingCode { get; set; }

        [Required(ErrorMessage = "Enter the Business Number*")]
        public string BusinessNumber { get; set; }

        [Required(ErrorMessage = "Enter the Financial Transaction Reg No*")]
        public string FinancialTransactionRegNo { get; set; }

        [Required(ErrorMessage ="Enter Remittance Reg Number*")]
        public string RemittancRegNumber { get; set; }
        [Required(ErrorMessage = "Enter the License Number*")]
        public string LicenseNumber { get; set; }

        [Required(ErrorMessage ="Enter the Zip Code*")]
        public string ZipCode { get; set; }
        [Required(ErrorMessage = "State Name Is Required")]
        public string OrgState { get; set; }
        public bool IsFirstNamePresent { get; set; }

        [Required(ErrorMessage = "Email is Required")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Please Input Valid Email")]
        [Remote(action: "VerifyEmail", controller: "Partners")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Position is required.")]
        public string Post { get; set; }


        [Required]
        [Remote(action: "VerifyUserName", controller: "Partners")]
        public string Username { get; set; }

        [Required]

        //[StringLength(int.MaxValue, ErrorMessage = "The password must be at least 12 characters long.", MinimumLength = 12)]
        //[DataType(DataType.Password)]
        //[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$", ErrorMessage = "The password must be 12 digit with one non-alphanumeric character, one digit (0-9), one uppercase (A-Z), one lowercase (a-z).")]
        [StringLength(int.MaxValue, ErrorMessage = "The password must be at least minimum 8 characters long.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,51}$", ErrorMessage = "The password must be 8 digit with one non-alphanumeric character, one digit (0-9), one uppercase (A-Z), one lowercase (a-z).")]
        public string Password { get; set; }

        [Compare(nameof(Password), ErrorMessage = "The Password didn't match.")]
        public string ConfirmPassword { get; set; }

        public int Service { get; set; }
        public int FundTypeId { get; set; }

        public string BalanceType { get; set; }

        //[Required(ErrorMessage = "Invalid Transaction Limit")]
        //public decimal CreditSendTxnLimitFlat { get; set; } = 0;

        [Required(ErrorMessage = "Invalid Transaction Limit")]
        public decimal CreditSendTxnLimitPercent { get; set; } = 0;

        [Required(ErrorMessage = "Invalid Transaction Limit")]
        public decimal CreditSendTxnLimit { get; set; } = 0;

        [Required(ErrorMessage = "Invalid Transaction Limit")]
        public decimal SendTxnLimitCashPayout { get; set; } = 0;

        [Required(ErrorMessage = "Invalid Transaction Limit")]
        public decimal SendTxnLimitWallet { get; set; } = 0;

        [Required(ErrorMessage = "Invalid Transaction Limit")]
        public decimal SendTxnLimitBank { get; set; } = 0;

        [Required(ErrorMessage = "Invalid Transaction Limit")]
        public bool IsTxnApproveActive { get; set; }

        [Required(ErrorMessage = "Organization Name is Required")]
        public string OrganizationName { get; set; }

        [Required(ErrorMessage = "Registration Number is Required")]
        public string CompanyRegistrationNumber { get; set; }

        [Required(ErrorMessage = "Organization Email is Required")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Please Input Valid Email")]
        public string CompanyEmail { get; set; }

        public string Country { get; set; }
        [Required(ErrorMessage = "City  is Required")]
        public string City { get; set; }

        [Required(ErrorMessage = "Address  is Required")]
        public string FullAddress { get; set; }

        [Required(ErrorMessage = "GMT Time Zone  is Required")]
        public string GMTTimeZone { get; set; }
        public string GMTTimeZoneId { get; set; }   

        [Required(ErrorMessage = "Local Source Currency  is Required")]
        public string LocalSourceCurrency { get; set; }

        public string IPAddress { get; set; }

        [Required(ErrorMessage = "Company Logo  is Required")]
        [MaxFileSize]
        [AllowedExtensions]
        public IFormFile CompanyLogo { get; set; }

        [Required(ErrorMessage = "License Document   is Required")]

        [MaxFileSize]
        [AllowedExtensions]
        public List<IFormFile> LicenseDocument { get; set; }
        [MaxFileSize]
        [AllowedExtensions]
        public IFormFile LicenseDocumentdemo { get; set; }

        public List<Director> Directors { get; set; }
        [Required(ErrorMessage = "Invalid Document Type")]
        public int DocumentType { get; set; }

        [Required(ErrorMessage = "Document Number is required")]
        public string DocumentNumber { get; set; }

        public DateTime? ExpiryDate { get; set; }

        public DateTime? IssueDate { get; set; }

        [Required(ErrorMessage = "IDFront is required")]
        [MaxFileSize]
        [AllowedExtensions]
        public IFormFile IDFront { get; set; }
        [MaxFileSize]
        [AllowedExtensions]
        public IFormFile IDBack { get; set; }
        [Required(ErrorMessage = "Invalid Address Type")]
        public int AddressProfType { get; set; }

        [Required(ErrorMessage = "AddressProfImage is required")]
        [MaxFileSize]
        [AllowedExtensions]
        public IFormFile AddressProfImage { get; set; }

        public bool IsActive { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> results = new List<ValidationResult>();
            if (!ContinueWithoutFirstName && string.IsNullOrWhiteSpace(LastName))
            {
                results.Add(new ValidationResult("FirstName", new[] { "FirstName" }));

            }
            foreach (var result in results)
            {
                yield return result; // Return each ValidationResult using yield return
            }
        }
    }

    public class Director
    {
        public string FirstName { get; set; }

        public string ContactNumber { get; set; }

        [Required(ErrorMessage = "Email is Required")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Please Input Valid Email")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Please enter a valid email address.")]
        
        public string Email { get; set; }
    }
    public class License
    {
        [MaxFileSize]
        [AllowedExtensions]
        [Required(ErrorMessage = "Enter the License*")]
        public IFormFile Image { get; set; }
        public string Imagename { get; set; }

    }










}
