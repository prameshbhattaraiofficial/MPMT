using System.ComponentModel.DataAnnotations;

namespace Mpmt.Core.ViewModel.ForgotPassword;

public class PasswordResetModel
{
    public int UserId { get; set; }
    public Guid UserGuid { get; set; }
    public string PartnerCode { get; set; }
    public string UserName { get; set; }
    public string ResetRequestToken { get; set; }
    public string ResetToken { get; set; }
    public string Email { get; set; }
    public string UserType { get; set; }
    public string PasswordHash { get; set; }
    public string PasswordSalt { get; set; }

    [Required]
    //[StringLength(int.MaxValue, ErrorMessage = "The password must be at least 12 characters long.", MinimumLength = 12)]
    //[DataType(DataType.Password)]
    //[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$", ErrorMessage = "The password must be 12 digit with one non-alphanumeric character, one digit (0-9), one uppercase (A-Z), one lowercase (a-z).")]
    [StringLength(int.MaxValue, ErrorMessage = "The password must be at least minimum 8 characters long.", MinimumLength = 8)]
    [DataType(DataType.Password)]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,51}$", ErrorMessage = "The password must be 8 digit with one non-alphanumeric character, one digit (0-9), one uppercase (A-Z), one lowercase (a-z).")]
    public string NewPassword { get; set; }

    [Compare(nameof(NewPassword), ErrorMessage = "The Password didn't match.")]
    public string ConfirmPassword { get; set; }
}