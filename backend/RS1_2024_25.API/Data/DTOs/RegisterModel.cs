using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RS1_2024_25.API.Data.DTOs
{
    public class RegisterModel
    {
        [Required]
        [JsonPropertyName("username")]
        public string Username { get; set; }

        [Required]
        [JsonPropertyName("firstname")]
        public string Firstname { get; set; }

        [Required]
        [JsonPropertyName("lastname")]
        public string Lastname { get; set; }

        [Required(ErrorMessage ="Email address is required")]
        [EmailAddress(ErrorMessage ="Invalid format of email address")]
        [JsonPropertyName("email")]
        public string Email { get; set; }

        [Required]
        [JsonPropertyName("password")]
        public string Password { get; set; }

   


    }
}
