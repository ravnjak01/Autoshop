namespace RS1_2024_25.API.Data.Models.Modul2_Basic
{
    public class Discount
    {
        public int Id { get; set; }
        public string Name { get; set; } 
        public decimal DiscountPercentage { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public ICollection<DiscountProduct> DiscountProducts { get; set; }
        public ICollection<DiscountCategory> DiscountCategories { get; set; }
        public ICollection<DiscountCode> DiscountCodes { get; set; }
    }
}
