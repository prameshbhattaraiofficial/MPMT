namespace Mpmt.Core.Domain.Agents;

public class RegisterAgent
{
    public int Id { get; set; }
    public string Event { get; set; }
    public int FormNumber { get; set; }
    public string AgentCode { get; set; }   
    public string FirstName { get; set; }
    public string UserName { get; set; }    
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public bool PhoneNumberConfirmed { get; set; }
    public string CallingCode { get; set; }
    public string Otp { get; set; }
    public DateTime? OtpExipiryDate { get; set; }
    public bool WithoutFirstName { get; set; }
    public string Email { get; set; }
    public bool EmailConfirmed { get; set; }
    public string PasswordHash { get; set; }
    public string PasswordSalt { get; set; }
    public string OrganizationName { get; set; }
    public string RegistrationNumber { get; set; }
    public string District { get; set; }
    public string City { get; set; }
    public string Address { get; set; }
    public List<string> DocumentImagePaths { get; set; }
    public int DocumentTypeId { get; set; }
    public string DocumentNumber { get; set; }
    public DateTime? IssueDate { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public string IdFrontImagePath { get; set; }
    public string IdBackImagePath { get; set; }
    public string CompanyLogoImgPath { get; set; }
    public bool IsActive { get; set; }
    public bool Maker { get; set; }
    public bool Checker { get; set; }
    public int CreatedById { get; set; }
    public string CreatedByName { get; set; }
    public DateTime? CreatedDate { get; set; }
    public int UpdatedById { get; set; }
    public string UpdatedByName { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public List<string> LicensedocImgPath { get; set; } = new List<string>();
}
