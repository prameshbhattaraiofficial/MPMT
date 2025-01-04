using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Mpmt.Core.ViewModel.CashAgent
{
    public class AgentChangePasswordVM : IValidatableObject
    {
        [Required(ErrorMessage = "Password is Required")]
        public string OldPassword { get; set; }
        [Required]
        [StringLength(int.MaxValue, ErrorMessage = "The password must be at least 8 characters long.", MinimumLength = 12)]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$", ErrorMessage = "The password must be 12 digit with one non-alphanumeric character, one digit (0-9), one uppercase (A-Z), one lowercase (a-z).")]
        public string NewPassword { get; set; }
        [Compare(nameof(NewPassword), ErrorMessage = "The Password didn't match.")]
        public string ConfirmPassword { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Check the missing requirements for the NewPassword
            var missingRequirements = GetMissingRequirementMessage(NewPassword);
            if (!string.IsNullOrEmpty(missingRequirements))
            {
                yield return new ValidationResult(missingRequirements, new string[] { "NewPassword" });
            }
            if (OldPassword == NewPassword)
            {
                yield return new ValidationResult("Password and New Password Cannot be same !!!", new string[] { "NewPassword" });
            }
        }

        public string GetMissingRequirementMessage(string password)
        {
            var missingRequirements = new List<string>();

            if (!Regex.IsMatch(password, @"[a-z]"))
            {
                missingRequirements.Add("at least one lowercase letter");
            }

            if (!Regex.IsMatch(password, @"[A-Z]"))
            {
                missingRequirements.Add("at least one uppercase letter");
            }

            if (!Regex.IsMatch(password, @"\d"))
            {
                missingRequirements.Add("at least one digit");
            }

            if (missingRequirements.Count > 0)
            {
                return "Password must contain " + string.Join(", ", missingRequirements) + ".";
            }
            return null;
        }
    }
}
