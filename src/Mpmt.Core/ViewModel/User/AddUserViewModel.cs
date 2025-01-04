using Microsoft.AspNetCore.Http;
using Mpmt.Core.Common.Attribites;
using System.ComponentModel.DataAnnotations;

namespace Mpmt.Core.ViewModel.User
{
    public class AddUserViewModel
    {

        public string FirstName { get; set; }
        [Required]
        public string SurName { get; set; }
        public bool IsSurNamePresent { get; set; }
        [Required(ErrorMessage = "Phone Number is required.")]
        [RegularExpression("^[\\+]?[(]?[0-9]{3}[)]?[-\\s\\.]?[0-9]{3}[-\\s\\.]?[0-9]{4,6}$", ErrorMessage = "Please input valid phone number")]
        public string MobileNumber { get; set; }
        [Required]
        public string Email { get; set; }
        public int GenderId { get; set; }
        [Required]
        public IFormFile ProfileImage { get; set; }
        [Required]
        public DateTime? DateOfBirth { get; set; }
        [Required]
        public string CountryCode { get; set; }
        [Required]
        public string Province { get; set; }
        [Required]
        public string City { get; set; }
        public string CallingCode { get; set; }
        public string Zipcode { get; set; }
        public string Address { get; set; }
        public int OccupationId { get; set; }
        public int DocumentTypeId { get; set; }
        [Required]
        public string DocumentNumber { get; set; }
        [Required]
        public DateTime? IssuedDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        [Required]
        [MaxFileSize]
        [AllowedExtensions]
        public IFormFile IdFrontImg { get; set; }
        [Required]
        [MaxFileSize]
        [AllowedExtensions]
        public IFormFile IdBackImg { get; set; }
        [Required]
        public string BankName { get; set; }
        [Required]
        public string BankCode { get; set; }
        public string Branch { get; set; }
        [Required]
        public string AccountHolderName { get; set; }
        [Required]
        public string AccountNumber { get; set; }
        public int IncomeSourceId { get; set; }
        public string GMTTimeZone { get; set; }
        public bool IsActive { get; set; }
        public string LoggedInUser { get; set; }
        public string UserType { get; set; }
    }
}
