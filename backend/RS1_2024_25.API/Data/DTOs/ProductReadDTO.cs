namespace RS1_2024_25.API.Data.DTOs
{
    public class ProductReadDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public string? SKU { get; set; }
        public string? Brend { get; set; }
        public string? Code { get; set; }
        public int StockQuantity { get; set; }
        public bool Active { get; set; }

        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }

        public decimal? PriceAfterGlobalDiscount { get; set; }
        public decimal? BadgeDiscountPercentage { get; set; }

        public bool IsFavorite { get; set; }

        public DateTime? CreatedAt { get; set; }
    }
}