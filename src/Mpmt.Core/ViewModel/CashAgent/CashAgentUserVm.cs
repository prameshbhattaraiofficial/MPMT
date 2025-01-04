using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mpmt.Core.Common.Attribites;
using System.ComponentModel.DataAnnotations;

namespace Mpmt.Core.ViewModel.CashAgent;

public class CashAgentUserVm
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Username is required.")]
    //[Remote(action: "VerifyUserName", controller: "AdminUser")]
    [Remote(action: "VerifyUserName", controller: "SuperAgent")]
    public string UserName { get; set; }
    [Required(ErrorMessage = "First Name is required.")]
    public string FirstName { get; set; }

    public string SuperAgentCode { get; set; }

    [Required(ErrorMessage = "last Name is required.")]
    public string LastName { get; set; }
    //[Remote(action: "VerifyEmail", controller: "AdminUser")]
    public string Email { get; set; }
    [RegularExpression("^[\\+]?[(]?[0-9]{3}[)]?[-\\s\\.]?[0-9]{3}[-\\s\\.]?[0-9]{4,6}$", ErrorMessage = "Please input valid phone number")]
    [Remote(action: "VerifyContactNumber", controller: "SuperAgent")]
    public string ContactNumber { get; set; }
    
    [Required(ErrorMessage = "Address is required.")]
    public string FullAddress { get; set; } 
    [Required(ErrorMessage = "District is required.")]
    public string DistrictCode { get; set; }
    //[StringLength(int.MaxValue, ErrorMessage = "The password must be at least 12 characters long.", MinimumLength = 12)]
    //[DataType(DataType.Password)]
    //[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$", ErrorMessage = "The password must be 12 digit with one non-alphanumeric character, one digit (0-9), one uppercase (A-Z), one lowercase (a-z).")]
    [StringLength(int.MaxValue, ErrorMessage = "The password must be at least minimum 8 characters long.", MinimumLength = 8)]
    [DataType(DataType.Password)]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,51}$", ErrorMessage = "The password must be 8 digit with one non-alphanumeric character, one digit (0-9), one uppercase (A-Z), one lowercase (a-z).")]
    public string Password { get; set; }
    [Compare(nameof(Password), ErrorMessage = "The Password didn't match.")]
    public string ConfirmPassword { get; set; }
    [MaxFileSize]
    [AllowedExtensions]
    public IFormFile DocumentImage { get; set; }
    [Required(ErrorMessage = "Organization Name is required.")]
    public string OrganizationName { get; set; }
    [Required(ErrorMessage = "Registration Number Name is required.")]
    [Remote(action: "VerifyRegistrationNumber", controller: "SuperAgent")]
    public string RegistrationNumber { get; set; }
    public bool IsActive { get; set; }
    public bool IsPrefunding { get; set; }  
    public string DocumentImagepath { get; set; }
    public string AgentCode { get; set; }
    public string UserType { get; set; }
    public string AgentCategoryId { get; set; }
    [MaxFileSize]
    [AllowedExtensions]
    public List<IFormFile> LicenseDocument { get; set; } = new List<IFormFile>();
    public List<string> LicensedocImgPath { get; set; } = new List<string>();
    public List<string> DeletedLicensedocImgPath { get; set; } = new List<string>();
    public string Event { get; set; }
}

public class CashAgentUpdateVm
{
    public int Id { get; set; }
    public string UserName { get; set; }
    [Required(ErrorMessage = "First Name is required.")]
    public string FirstName { get; set; }

    public string SuperAgentCode { get; set; }

    [Required(ErrorMessage = "last Name is required.")]
    public string LastName { get; set; }
    //[Remote(action: "VerifyEmail", controller: "AdminUser")]
    public string Email { get; set; }
    [RegularExpression("^[\\+]?[(]?[0-9]{3}[)]?[-\\s\\.]?[0-9]{3}[-\\s\\.]?[0-9]{4,6}$", ErrorMessage = "Please input valid phone number")]
    public string ContactNumber { get; set; }

    [Required(ErrorMessage = "Address is required.")]
    public string FullAddress { get; set; }
    [Required(ErrorMessage = "District is required.")]
    public string DistrictCode { get; set; }

    [StringLength(int.MaxValue, ErrorMessage = "The password must be at least minimum 8 characters long.", MinimumLength = 8)]
    [DataType(DataType.Password)]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,51}$", ErrorMessage = "The password must be 8 digit with one non-alphanumeric character, one digit (0-9), one uppercase (A-Z), one lowercase (a-z).")]
    public string Password { get; set; }
    [Compare(nameof(Password), ErrorMessage = "The Password didn't match.")]
    public string ConfirmPassword { get; set; }
    [MaxFileSize]
    [AllowedExtensions]
    public IFormFile DocumentImage { get; set; }
    [Required(ErrorMessage = "Organization Name is required.")]
    public string OrganizationName { get; set; }
    [Required(ErrorMessage = "Registration Number Name is required.")]
    public string RegistrationNumber { get; set; }
    public bool IsActive { get; set; }
    public bool IsPrefunding { get; set; }
    public string DocumentImagepath { get; set; }   
    public string AgentCode { get; set; }
    public string UserType { get; set; }
    public string AgentCategoryId { get; set; }
    [MaxFileSize]
    [AllowedExtensions]
    public List<IFormFile> LicenseDocument { get; set; } = new List<IFormFile>();
    public List<string> LicensedocImgPath { get; set; } = new List<string>();
    public List<string> DeletedLicensedocImgPath { get; set; } = new List<string>();
    public string Event { get; set; }
}