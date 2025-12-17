using RS1_2024_25.API.Data.Models.ShoppingCart;

namespace RS1_2024_25.API.Data.Models.Modul2_Basic
{
    public class DiscountProduct
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int DiscountId { get; set; }
        public Discount Discount { get; set; }

        public string? LastModifiedUserId { get; set; }
        public User? LastModifiedUser { get; set; }
    }
}
