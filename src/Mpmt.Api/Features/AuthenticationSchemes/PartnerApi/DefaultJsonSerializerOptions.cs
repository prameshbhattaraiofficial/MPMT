using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mpmt.Api.Features.AuthenticationSchemes.PartnerApi
{
    public static class DefaultJsonSerializerOptions
    {
        public static JsonSerializerOptions Options => new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };
    }
}
