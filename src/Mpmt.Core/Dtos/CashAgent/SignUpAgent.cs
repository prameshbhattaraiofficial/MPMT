using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Mpmt.Core.Dtos.CashAgent;

public class SignUpAgent
{
    public string FirstName { get; set; }

    [Required(ErrorMessage = "Last name is required")]
    public string LastName { get; set; }

    public bool WithoutFirstName { get; set; }

    [DataType(DataType.EmailAddress, ErrorMessage = "Please input valid Email")]
    public string Email { get; set; }

    [Required]
    [RegularExpression("^[\\+]?[(]?[0-9]{3}[)]?[-\\s\\.]?[0-9]{3}[-\\s\\.]?[0-9]{4,6}$", ErrorMessage = "Please input valid phone number")]
    [Remote(action: "VerifyContactNumber", controller: "AgentRegistration")]
    public string PhoneNumber { get; set; }

    [Required(ErrorMessage = "Username is required.")]
    [Remote(action: "VerifyUserName", controller: "AgentRegistration")]
    public string UserName { get; set; }

    public string CallingCode { get; set; }

    [Required]
    //[StringLength(int.MaxValue, ErrorMessage = "The password must be at least 12 characters long", MinimumLength = 12)]
    //[DataType(DataType.Password)]
    //[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$", ErrorMessage = "The password must be 12 digit with one non-alphanumeric character, one digit (0-9), one uppercase (A-Z), one lowercase (a-z)")]
    [StringLength(int.MaxValue, ErrorMessage = "The password must be at least minimum 8 characters long.", MinimumLength = 8)]
    [DataType(DataType.Password)]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,51}$", ErrorMessage = "The password must be 8 digit with one non-alphanumeric character, one digit (0-9), one uppercase (A-Z), one lowercase (a-z).")]
    public string Password { get; set; }

    [Compare(nameof(Password), ErrorMessage = "Password didn't match")]
    public string ConfirmPassword { get; set; }

    public string Otp { get; set; }
}

public class OtpValidationAgent
{
    public string Otp { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
}