using System.Text.Json.Serialization;

namespace SWDCoreAPI.Models
{
    public class Login
    {
        [JsonPropertyName("username")]

        public string? UserName { get; set; }
        [JsonPropertyName("password")]
        public string? Password { get; set; }

    }
}
