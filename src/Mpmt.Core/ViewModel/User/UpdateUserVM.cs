using Microsoft.AspNetCore.Http;
using Mpmt.Core.Common.Attribites;
using System.ComponentModel.DataAnnotations;

namespace Mpmt.Core.ViewModel.User
{
    public class UpdateUserVM
    {
        public int Id { get; set; }

        public string FirstName { get; set; }
        [Required]
        public string SurName { get; set; }
        public bool IsSurNamePresent { get; set; }
        [Required]
        public string MobileNumber { get; set; }
        [Required]
        public string Email { get; set; }
        public int GenderId { get; set; }

        public IFormFile ProfileImage { get; set; }
        public string ProfileImagePath { get; set; }
        [Required]
        public DateTime? DateOfBirth { get; set; }
        [Required]
        public string CountryCode { get; set; }
        [Required]
        public string Province { get; set; }
        [Required]
        public string City { get; set; }

        public string Zipcode { get; set; }
        public string Address { get; set; }
        public int OccupationId { get; set; }
        public int DocumentTypeId { get; set; }
        [Required]
        public string DocumentNumber { get; set; }
        [Required]
        public DateTime? IssuedDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        [MaxFileSize]
        [AllowedExtensions]
        public IFormFile IdFrontImg { get; set; }
        public string IdFrontImgPath { get; set; }
        [MaxFileSize]
        [AllowedExtensions]
        public IFormFile IdBackImg { get; set; }
        public string IdBackImgPath { get; set; }
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
