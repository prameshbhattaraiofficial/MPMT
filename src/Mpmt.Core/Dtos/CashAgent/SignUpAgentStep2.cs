using Microsoft.AspNetCore.Http;
using Mpmt.Core.Common.Attribites;
using System.ComponentModel.DataAnnotations;

namespace Mpmt.Core.Dtos.CashAgent;

public class SignUpAgentStep2
{
    public string Email { get; set; }
    public string Token { get; set; }
    public string PhoneNumber { get; set; }
    public string CompanyLogoImagePath { get; set; }
    [Required]
    public string OrganizationName { get; set; }
    [Required]
    public string RegistrationNumber { get; set; }
    [Required]
    public string District { get; set; }
    [Required]
    public string Address { get; set; }
    [MaxFileSize]
    [AllowedExtensions]
    public IFormFile DocumentImage { get; set; }
    public List<string> DocumentImagePaths { get; set; }
    [MaxFileSize]
    [AllowedExtensions]
    public List<IFormFile> LicenseDocument { get; set; } = new List<IFormFile>();
    public List<string> LicensedocImgPath { get; set; } = new List<string>();
}
