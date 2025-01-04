using Mpmt.Core.Common.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Mpmt.Core.Models.Public.Feedbacks
{
    public class PublicFeedbackRequest
    {
        [Required]
        [MaxLength(128)]
        public string FullName { get; set; }
        
        [Required]
        [MaxLength(320)]
        [MpmtEmail(ErrorMessage = "Please enter a valid email.")]
        public string Email { get; set; }

        [Required]
        [MaxLength(20)]
        public string ContactNo { get; set; }

        [Required]
        [MaxLength(320)]
        public string Subject { get; set; }

        [Required]
        [MaxLength(2048)]
        public string Message { get; set; }
    }
}
