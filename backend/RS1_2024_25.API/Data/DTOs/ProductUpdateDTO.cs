using System.ComponentModel.DataAnnotations;

namespace RS1_2024_25.API.Data.DTOs
{
    public class ProductUpdateDTO
    {
        [Required(ErrorMessage = "Product ID is required for update.")]
        [Range(1, int.MaxValue, ErrorMessage = "Product ID must be a positive number.")]
        public int Id { get; set; }

   
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters.")]
        public string? Name { get; set; }

        [Range(0.01, (double)decimal.MaxValue, ErrorMessage = "Price must be a positive number.")]
        public decimal? Price { get; set; } 


        public string? ImageUrl { get; set; } 


        public string? Brend { get; set; } 

        

        [Range(0, int.MaxValue, ErrorMessage = "Stock quantity cannot be negative.")]
        public int? StockQuantity { get; set; } 

        [StringLength(2000, ErrorMessage = "Description cannot exceed 2000 characters.")]
        public string? Description { get; set; }

        public bool? Active { get; set; }

        public string? SKU { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Category ID must be a positive number.")]

        public int? CategoryId { get; set; } 

       
        public List<string>? AdditionalImagesUrl { get; set; } = new List<string>();
    }
}