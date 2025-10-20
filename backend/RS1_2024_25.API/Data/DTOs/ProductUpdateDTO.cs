namespace RS1_2024_25.API.Data.DTOs
{
    public class ProductUpdateDTO
    {
        public string Name { get; set; }
        public decimal? Price { get; set; }
        public int? StockQuantity { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public bool? Active { get; set; }
        public string? SKU { get; set; }
        public string? Brend { get; set; }
        public int? CategoryId { get; set; }
        public List<string> AdditionalImagesUrl { get; set; }
    }
}
