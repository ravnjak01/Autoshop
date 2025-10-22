using static Duende.IdentityServer.Models.IdentityResources;

namespace RS1_2024_25.API.Data.Models.ShoppingCart
{
    public class Order
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public int AdresaId { get; set; }
        public Address Adresa { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public decimal TotalAmount { get; set; }

        public List<OrderItem> Items { get; set; } = new List<OrderItem>();

        
        public string Status { get; set; } = "Pending";
    }
}
