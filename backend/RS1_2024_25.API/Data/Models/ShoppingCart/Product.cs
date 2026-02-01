using RS1_2024_25.API.Data.Models.Modul2_Basic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RS1_2024_25.API.Data.Models.ShoppingCart
{
    public class Product
    {
        
        public int Id { get; set; }

        [Required] 
        [StringLength(255)] 
        public string Name { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")] 
        public decimal Price { get; set; }

        public int? StockQuantity { get; set; } = 0; 

        [StringLength(4000)] 
        public string Description { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        public bool Active { get; set; } = true;

        [StringLength(50)]
        public string? SKU { get; set; } 

        [StringLength(100)]
        public string Brend { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Category ID must be a positive number.")]
        public int CategoryId { get; set; } 


        public Category Category { get; set; }

        // Opciono: Lista URL-ova za dodatne slike
        public List<string>? AdditionalImagesUrl { get; set; } // Može se mapirati kao JSON u DB ili kao zasebna tabela


        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();


        public string? Code { get; set; }
        public DateTime? CreatedAt { get; internal set; }

        public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();
    }
}
