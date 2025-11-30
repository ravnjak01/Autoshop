namespace RS1_2024_25.API.Data.DTOs
{
    public class CheckoutResponseDTO
    {
        public int OrderId { get; set; }
        public decimal Total { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
    }
}
