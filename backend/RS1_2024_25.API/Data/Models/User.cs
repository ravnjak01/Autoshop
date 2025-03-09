using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace RS1_2024_25.API.Data.Models
{
    public class User:IdentityUser
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string FullName { get; set; }
       // [Required]
        //public string Email { get; set; }
        //[Required]
        //public string PasswordHash { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
