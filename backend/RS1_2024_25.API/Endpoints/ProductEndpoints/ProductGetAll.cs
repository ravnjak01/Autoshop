using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RS1_2024_25.API.Data;
using RS1_2024_25.API.Data.Models.Modul2_Basic;
using RS1_2024_25.API.Helper.Api;
using System.Linq;

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

            // Filter by search query (name/code)
            if (!string.IsNullOrEmpty(request.SearchQuery))
            {
                query = query.Where(p => p.Name.Contains(request.SearchQuery) || p.Code.Contains(request.SearchQuery));
            }

            if (request.CategoryIds != null && request.CategoryIds.Any())
            {
                query = query.Where(p => p.CategoryId != null && request.CategoryIds.Contains((int)p.CategoryId));
            }

            // Filter by price range
            if (request.MinPrice.HasValue)
            {
                query = query.Where(p => p.Price >= request.MinPrice.Value);
            }
            if (request.MaxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= request.MaxPrice.Value);
            }

            // Sorting
            if (!string.IsNullOrEmpty(request.SortBy))
            {
                var isDescending = request.SortDescending;

                query = request.SortBy.ToLower() switch
                {
                    "price" => isDescending ? query.OrderByDescending(p => p.Price) : query.OrderBy(p => p.Price),
                    "createddate" => isDescending ? query.OrderByDescending(p => p.CreatedAt) : query.OrderBy(p => p.CreatedAt),
                    _ => query.OrderBy(p => p.Name) // default sort
                };
            }
            else
            {
                query = query.OrderBy(p => p.Name); // default
            }

            var products = await query.ToListAsync(cancellationToken);

            return new ProductGetAllResponse()
            {
                Products = products
            };
        }
    }
    public class ProductGetAllRequest
    {
        public string? SearchQuery { get; set; }
        public List<int>? CategoryIds { get; set; } 
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public string? SortBy { get; set; } // "price", "createdDate"
        public bool SortDescending { get; set; } = false;
    }
    public class ProductGetAllResponse
    {
        public required List<Product> Products { get; set; }
    }
}
