using System.ComponentModel.DataAnnotations;

namespace Mpmt.Agent.Models
{
    public class AgentLoginModel
    {
        [Required(ErrorMessage = "Please enter your Username or PhoneNumber")]
        public string UsernameOrPhoneNumber { get; set; }

        [Required(ErrorMessage = "Please enter your password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string Code { get; set; } = "000000";

        public bool RememberMe { get; set; }
    }
}
