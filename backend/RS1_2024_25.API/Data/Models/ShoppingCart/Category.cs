using System.ComponentModel.DataAnnotations;

namespace RS1_2024_25.API.Data.Models.ShoppingCart
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        [StringLength(255)]
        public string Name { get; set; }
    
        public ICollection<Product> Products { get; set; }=new List<Product>();

        public string? Description { get; set; }

        [Required]
        [StringLength(50)]
        public string Code { get;  set; }
    }
}
