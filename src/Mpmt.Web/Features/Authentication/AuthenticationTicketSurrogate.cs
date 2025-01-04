using System.Text.Json.Serialization;

namespace Mpmt.Web.Features.Authentication
{
    
    public class AuthenticationTicketSurrogate
    {
        public string Principal { get; set; }
        public string Properties { get; set; }
        public string AuthenticationScheme { get; set; }
        // Add other properties as needed

        // Parameterless constructor or constructor annotated with JsonConstructorAttribute
        [JsonConstructor]
        public AuthenticationTicketSurrogate()
        {
        }
    }
}
