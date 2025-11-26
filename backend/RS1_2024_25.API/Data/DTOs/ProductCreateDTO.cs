namespace RS1_2024_25.API.Data.DTOs
{
    public class ProductCreateDTO
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
    }
}
