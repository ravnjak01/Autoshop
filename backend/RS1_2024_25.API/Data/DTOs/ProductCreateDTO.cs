using System.ComponentModel.DataAnnotations;

namespace RS1_2024_25.API.Data.DTOs
{
    public class ProductCreateDTO
    {
     
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Product Code/SKU is required.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Code must be between 3 and 50 characters.")]
        public string SKU { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, (double)decimal.MaxValue, ErrorMessage = "Price must be a positive number.")]
        public decimal Price { get; set; }

       
        [Required(ErrorMessage = "Stock quantity is required.")]
        [Range(0, int.MaxValue, ErrorMessage = "Stock quantity cannot be negative.")]
        public int StockQuantity { get; set; }

       
        [StringLength(2000, ErrorMessage = "Description cannot exceed 2000 characters.")]
        public string? Description { get; set; }


        [Required(ErrorMessage = "Image is required.")]
        [Url(ErrorMessage = "ImageUrl has to be valid.")]
        public string ImageUrl { get; set; } //

        [Required(ErrorMessage = "Category is required.")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Brand is required.")]
        public string Brend { get; set; }

        [Required(ErrorMessage = "Active status is required.")]
        public bool Active { get; set; }
    }
}