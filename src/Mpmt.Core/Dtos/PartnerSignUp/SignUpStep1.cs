using Mpmt.Core.Dtos.Partner;
using System.ComponentModel.DataAnnotations;

namespace Mpmt.Core.Dtos.PartnerSignUp
{
    public class SignUpStep1
    {
        public string Token { get; set; }

        [Required(ErrorMessage = "Email is Required")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Please Input Valid Email")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; }


        public List<Director> Directors { get; set; }


    }

    //public class Director
    //{
    //    [Required(ErrorMessage ="FirstName is Required*")]
    //    public string FirstName { get; set; }

    //    [Required(ErrorMessage ="Contact Number is Required*")]
    //    public string ContactNumber { get; set; }

    //    [Required(ErrorMessage = "Email is Required")]
    //    [DataType(DataType.EmailAddress, ErrorMessage = "Please Input Valid Email")]
    //    [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Please enter a valid email address.")]
    //    public string Email { get; set; }
    //}
}
