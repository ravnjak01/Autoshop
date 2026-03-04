using RS1_2024_25.API.Endpoints.ProductEndpoints;

namespace RS1_2024_25.API.Data.DTOs
{
    public class CategoryDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string? Description { get; set; }

        // Lista proizvoda unutar kategorije
    }
}
