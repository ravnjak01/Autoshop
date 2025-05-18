namespace RS1_2024_25.API.Data.Models.Modul2_Basic
{
    public class DiscountCategory
    {
        public int Id { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public int DiscountId { get; set; }
        public Discount Discount { get; set; }
    }
}
