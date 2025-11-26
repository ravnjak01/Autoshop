using System.ComponentModel.DataAnnotations;

namespace RS1_2024_25.API.Data.Models
{
    public class LoginModel
    {

            [Required]
            public string Username { get; set; }
        [Required]
            public string Password { get; set; }
        
    }
}
