using System.ComponentModel.DataAnnotations;

namespace Mpmt.Core.Dtos.PartnerSignUp
{
    public class SignUpPartner : IValidatableObject
    {
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is Required")]
        public string LastName { get; set; }    

        //[Required(ErrorMessage = "Shortname is Required")]
        //public string ShortName { get; set; }   
        public bool Withoutfirstname { get; set; }

        [Required(ErrorMessage = "Email is Required")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Please Input Valid Email")]
        public string Email { get; set; }
        [Required(ErrorMessage ="Enter the Phone Number*")]
        public string PhoneNumber { get; set; }

        public string Callingcode { get; set; }
        //[StringLength(int.MaxValue, ErrorMessage = "The password must be at least 12 characters long.", MinimumLength = 12)]
        //[DataType(DataType.Password)]
        //[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$", ErrorMessage = "The password must be 12 digit with one non-alphanumeric character, one digit (0-9), one uppercase (A-Z), one lowercase (a-z).")]
        [StringLength(int.MaxValue, ErrorMessage = "The password must be at least minimum 8 characters long.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,51}$", ErrorMessage = "The password must be 8 digit with one non-alphanumeric character, one digit (0-9), one uppercase (A-Z), one lowercase (a-z).")]
        [Required(ErrorMessage = "Password Field is Required*")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Enter the Position*")]
        public string Position { get; set; }

        [Compare(nameof(Password), ErrorMessage = "The Password didn't match.")]
        [Required(ErrorMessage = "Confirm Password Field is Required*")]
        public string ConfirmPassword { get; set; }

        public string otp { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!this.Withoutfirstname && string.IsNullOrEmpty(this.FirstName))
            {
                yield return new ValidationResult("FirstName is Required", new string[] { "FirstName" });
            }
        }
    }
    public class Otpvalidatiom
    {
        public string Otp { get; set; }

        [Required(ErrorMessage = "Email is Required")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Please Input Valid Email")]
        public string Email { get; set; }

        
    

        //[Required(ErrorMessage = "Password is required")]
        //[StringLength(255, ErrorMessage = "Password must be greater than 8", MinimumLength = 8)]
        //[DataType(DataType.Password)]
        //public string Password { get; set; }

        //[Required(ErrorMessage = "Confirm Password is required")]
        //[StringLength(255, ErrorMessage = "confirm Password must be greater than 8", MinimumLength = 8)]
        //[DataType(DataType.Password)]
        //[Compare(nameof(Password), ErrorMessage = "The Password didn't match.")]
        //public string ConfirmPassword { get; set; }

        //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //    if (string.Compare(Password, ConfirmPassword) == 1)
        //    {
        //        yield return new ValidationResult("password and Confirm password must be same", new string[] { "ConfirmPassword" });
        //    }

        //    if ( string.IsNullOrEmpty(this.ConfirmPassword))
        //    {
        //        yield return new ValidationResult("Confirm Password is required*", new string[] { "ConfirmPassword" });
        //    }

        //    if (string.IsNullOrEmpty(this.Password))
        //    {
        //        yield return new ValidationResult("password is required*", new string[] { "ConfirmPassword" });
        //    }
        // }
    }
}
