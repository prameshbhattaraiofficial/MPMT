using System.ComponentModel.DataAnnotations;

namespace Mpmt.Web.Areas.Partner.Models.PartnerLogin
{
    /// <summary>
    /// The partner login model.
    /// </summary>
    public class PartnerLoginModel
    {
        /// <summary>
        /// Gets or sets the username or email.
        /// </summary>
        [Required(ErrorMessage = "Please enter your Username or Email")]

        public string UsernameOrEmail { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        [Required(ErrorMessage = "Please enter your password")]
        public string Password { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether remember me.
        /// </summary>
        public bool RememberMe { get; set; }
        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        public string Code { get; set; }
    }
}
