namespace RS1_2024_25.API.Data.Models.ShoppingCart
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
    
        public ICollection<Product> Products { get; set; }
        public string Code { get;  set; }
    }
}
