using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace RS1_2024_25.API.Data.Models
{
    public class User:IdentityUser
    {
        [Required]
        public string FullName { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now; // Podrazumevana vrednost

    }
}
