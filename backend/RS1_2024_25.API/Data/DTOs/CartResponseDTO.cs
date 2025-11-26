namespace RS1_2024_25.API.Data.DTOs
{
    public class CartResponseDTO
    {
        public List<CartItemDTO> Items { get; set; } = new();
        public decimal Total { get; set; }
        public int ItemCount { get; set; }
    }
}
