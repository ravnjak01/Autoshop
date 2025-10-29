namespace RS1_2024_25.API.Data.Models.ShoppingCart
{
    public class Cart
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public User User { get; set; }
        public string? GuestSessionId { get; set; }
        public ICollection<CartItem> Items { get; set; } = new List<CartItem>();



    }
}
