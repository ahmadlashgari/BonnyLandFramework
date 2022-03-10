using System.Text.Json;

namespace BL.Framework.Core.Constants
{
    public static class HttpClientTestConstants
    {
        public static JsonSerializerOptions DefaultJsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }
}
