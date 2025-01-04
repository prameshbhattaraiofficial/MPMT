using System.ComponentModel.DataAnnotations;

namespace Mpmt.Core.Dtos.PartnerEmployee;

public class IUDPartnerEmployee
{
    public string Event { get; set; }
    public int Id { get; set; }
    public string PartnerCode { get; set; }
    public string FirstName { get; set; }
    public string SurName { get; set; }
    public bool IsSurNamePresent { get; set; }
    [Required(ErrorMessage = "Phone Number is required.")]
    [RegularExpression("^[\\+]?[(]?[0-9]{3}[)]?[-\\s\\.]?[0-9]{3}[-\\s\\.]?[0-9]{4,6}$", ErrorMessage = "Please input valid phone number")]
    public string MobileNumber { get; set; }
    public bool MobileConfirmed { get; set; }
    [Required(ErrorMessage = "Email is Required")]
    public string Email { get; set; }
    public bool EmailConfirmed { get; set; }
    [Required(ErrorMessage = "Position is Required")]
    public string Post { get; set; }
    public int GenderId { get; set; }
    public string UserName { get; set; } = string.Empty;
    [StringLength(int.MaxValue, ErrorMessage = "The password must be at least minimum 8 characters long.", MinimumLength = 8)]
    [DataType(DataType.Password)]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,51}$", ErrorMessage = "The password must be 8 digit with one non-alphanumeric character, one digit (0-9), one uppercase (A-Z), one lowercase (a-z).")]
    public string Password { get; set; }
    [Compare(nameof(Password), ErrorMessage = "The Password didn't match.")]
    public string ConfirmPassword { get; set; }
    public string PasswordHash { get; set; }
    public string PasswordSalt { get; set; }
    public string AccessCodeHash { get; set; }
    public string AccessCodeSalt { get; set; }
    public string MPINHash { get; set; }
    public string MPINSalt { get; set; }
    public string IpAddress { get; set; }
    public string LastIpAddress { get; set; }
    public string DeviceId { get; set; }
    public bool IsActive { get; set; }
    public bool IsDeleted { get; set; }
    public bool IsBlocked { get; set; }
    public int FailedLoginAttempt { get; set; }
    public DateTime? TemporaryLockedTillUtcDate { get; set; } 
    public DateTime? LastLoginDateUtc { get; set; } 
    public DateTime? LastActivityDateUtc { get; set; } 
    public bool Is2FAAuthenticated { get; set; }
    public string AccountSecretKey { get; set; }
    public int CreatedById { get; set; }
    public string CreatedByName { get; set; }
    public DateTime? CreatedDate { get; set; } = DateTime.Now;
    public int UpdatedById { get; set; }
    public string UpdatedByName { get; set; }
    public DateTime? UpdatedDate { get; set; } = DateTime.Now;
    public string Remarks { get; set; } 
}
