using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RS1_2024_25.API.Data;
using RS1_2024_25.API.Data.Models.ShoppingCart;
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
                switch (request.SortBy)
                {
                    case "priceAsc":
                        query = query.OrderBy(p => p.Price);
                        break;
                    case "priceDesc":
                        query = query.OrderByDescending(p => p.Price); 
                        break;
                    case "createdDateAsc":
                        query = query.OrderBy(p => p.CreatedAt); 
                        break;
                    case "createdDateDesc":
                        query = query.OrderByDescending(p => p.CreatedAt); 
                        break;
                    default:
                        query = query.OrderBy(p => p.CreatedAt); 
                        break;
                }
            }
            else
            {
                query = query.OrderBy(p => p.CreatedAt);
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
    }
    public class ProductGetAllResponse
    {
        public required List<Product> Products { get; set; }
    }
}
