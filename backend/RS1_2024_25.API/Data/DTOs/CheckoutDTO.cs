using RS1_2024_25.API.Data.Models;

namespace RS1_2024_25.API.Data.DTOs
{
    public class CheckoutDTO
    {
        public int UserId { get; set; }
        public Address Adresa { get; set; } 
        public string PaymentMethod { get; set; } 
        public decimal TotalAmount { get; set; }
    }
}
