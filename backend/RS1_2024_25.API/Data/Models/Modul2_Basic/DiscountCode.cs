namespace RS1_2024_25.API.Data.Models.Modul2_Basic
{
    public class DiscountCode
    {
        public int Id { get; set; }
        public string Code { get; set; } 

        public int DiscountId { get; set; }
        public Discount Discount { get; set; }
    }
}
