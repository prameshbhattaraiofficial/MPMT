using System.ComponentModel.DataAnnotations;

namespace Mpmt.Web.Areas.Admin.Models.Users
{
    /// <summary>
    /// The user login model.
    /// </summary>
    public class UserLoginModel
    {
       
        [Required(ErrorMessage = "Please enter your Username or Email")]
        public string UsernameOrEmail { get; set; }

        [Required(ErrorMessage = "Please enter your password")]
        [DataType(DataType.Password)]
       public string Password { get; set; }

        public string Code { get; set; } = "000000";
      
        public bool RememberMe { get; set; }


    }
}
