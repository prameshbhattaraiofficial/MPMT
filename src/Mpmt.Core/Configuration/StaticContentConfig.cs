using System.ComponentModel.DataAnnotations;

namespace Mpmt.Core.Configuration
{
    public class StaticContentConfig
    {
        [Required]
        public string UserDataDirectory { get; set; }
    }
}
