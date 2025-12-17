namespace RS1_2024_25.API.Data.Models.Modul2_Basic
{
    public class DiscountCode
    {
        public int Id { get; set; }
        public string Code { get; set; } 

        public int DiscountId { get; set; }
        public Discount Discount { get; set; }

        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }

        public string? LastModifiedUserId { get; set; }
        public User? LastModifiedUser { get; set; }

        public bool IsActive =>
            (!ValidFrom.HasValue || ValidFrom.Value <= DateTime.UtcNow) &&
            (!ValidTo.HasValue || ValidTo.Value >= DateTime.UtcNow);
    }
}
