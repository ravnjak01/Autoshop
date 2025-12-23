namespace RS1_2024_25.API.Data.DTOs
{
    public class CartItemDTO
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal Total { get; set; }
        public string? imageUrl { get; set; }
        public int StockQuantity { get; set; }
    }
}
