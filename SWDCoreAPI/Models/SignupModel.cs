using System.Text.Json.Serialization;

namespace SWDCoreAPI.Models
{
    public class SignupModel
    {
        [JsonPropertyName("firstName")]
        public string FirstName { get; set; }

        [JsonPropertyName("lastName")]
        public string LastName { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("password")]
        public string Password { get; set; }

        [JsonPropertyName("confirmPassword")]
        public string ConfirmPassword { get; set; }
    }
}
