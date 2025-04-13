namespace RS1_2024_25.API.Data.Models.Modul2_Basic
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public int? CategoryId { get; set; }
        public Category Category { get; set; } 
    }
}
