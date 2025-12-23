using RS1_2024_25.API.Data.Models.ShoppingCart;
using System.ComponentModel.DataAnnotations;

namespace RS1_2024_25.API.Data.Models.Modul2_Basic
{
    public class Favorite
    {
        [Key]
        public int Id { get; set; }

        // FK → User
        [Required]
        public string UserId { get; set; }

        // FK → Product
        [Required]
        public int ProductId { get; set; }

        // Navigation properties
        public User User { get; set; }
        public Product Product { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
