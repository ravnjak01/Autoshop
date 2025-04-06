using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RS1_2024_25.API.Data;
using RS1_2024_25.API.Data.Models.Modul2_Basic;
using RS1_2024_25.API.Helper.Api;

namespace RS1_2024_25.API.Endpoints.ProductEndpoints
{
    [Route("product")]
    public class ProductGetAll(ApplicationDbContext db): MyEndpointBaseAsync
        .WithRequest<ProductGetAllRequest>
        .WithResult<ProductGetAllResponse>
    {
        [HttpGet("filter")]
        public override async Task<ProductGetAllResponse> HandleAsync([FromQuery] ProductGetAllRequest request, CancellationToken cancellationToken = default)
        {
            var query = db.Products.AsQueryable();
            if (!string.IsNullOrEmpty(request.SearchQuery))
            {
                query = query.Where(p => p.Name.Contains(request.SearchQuery) || p.Code.Contains(request.SearchQuery));
            }
            var products = await query
                .OrderBy(p => p.Name)
                .ToListAsync(cancellationToken);
            return new ProductGetAllResponse()
            {
                Products = products
            };
        }
    }
    public class ProductGetAllRequest
    {
        public string? SearchQuery { get; set; }
    }
    public class ProductGetAllResponse
    {
        public required List<Product> Products { get; set; }
    }
}
