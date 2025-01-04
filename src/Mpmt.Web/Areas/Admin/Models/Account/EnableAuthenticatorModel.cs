using Microsoft.AspNetCore.Mvc;

namespace Mpmt.Web
{
    /// <summary>
    /// The enable authenticator model.
    /// </summary>
    public class EnableAuthenticatorModel
    {

        /// <summary>
        /// Gets or sets the shared key.
        /// </summary>
        public string SharedKey { get; set; }

        /// <summary>
        /// Gets or sets the authenticator uri.
        /// </summary>
        public string AuthenticatorUri { get; set; }

        /// <summary>
        /// Gets or sets the recovery codes.
        /// </summary>
        [TempData]
        public string[] RecoveryCodes { get; set; }

        /// <summary>
        /// Gets or sets the status message.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        //[BindProperty]
        //public InputModel Input { get; set; }

        //public class InputModel
        //{
        //    [Required]
        //    [StringLength(7, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        //    [DataType(DataType.Text)]
        //    [Display(Name = "Verification Code")]
        //    public string Code { get; set; }
        //}
    }
}
