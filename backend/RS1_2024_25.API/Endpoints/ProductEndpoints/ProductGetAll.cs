using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RS1_2024_25.API.Data;
using RS1_2024_25.API.Helper.Api;
using Microsoft.AspNetCore.Identity;
using RS1_2024_25.API.Data.Models;

namespace RS1_2024_25.API.Endpoints.ProductEndpoints
{
    [Route("products")]
    public class ProductGetAll(ApplicationDbContext db, UserManager<User> userManager) : MyEndpointBaseAsync
        .WithRequest<ProductGetAllRequest>
        .WithResult<ProductGetAllResponse>
    {
        [HttpGet()]
        public override async Task<ProductGetAllResponse> HandleAsync(
            [FromQuery] ProductGetAllRequest request,
            CancellationToken cancellationToken = default)
        {
       
            request.PageNumber = Math.Max(1, request.PageNumber);
            request.PageSize = Math.Clamp(request.PageSize, 1, 100);

            var query = db.Products
                .Include(p => p.Category)
                .Where(p => p.Active && p.StockQuantity > 0)
                .AsQueryable();

   
            if (!string.IsNullOrWhiteSpace(request.SearchQuery))
            {
                var searchLower = request.SearchQuery.Trim().ToLower();
                query = query.Where(p =>
                    p.Name.ToLower().Contains(searchLower) ||
                    (p.Code != null && p.Code.ToLower().Contains(searchLower)) ||
                    (p.SKU != null && p.SKU.ToLower().Contains(searchLower)) ||
                    (p.Brend != null && p.Brend.ToLower().Contains(searchLower)));
            }

      
            if (request.CategoryIds?.Any() == true)
            {
                query = query.Where(p =>
                    request.CategoryIds.Contains(p.CategoryId));
                   
            }

      
            if (request.MinPrice.HasValue && request.MinPrice.Value >= 0)
            {
                query = query.Where(p => p.Price >= request.MinPrice.Value);
            }

            if (request.MaxPrice.HasValue && request.MaxPrice.Value >= 0)
            {
                query = query.Where(p => p.Price <= request.MaxPrice.Value);
            }
          
            query = (request.SortBy?.ToLower()) switch
            {
                "priceasc" => query.OrderBy(p => p.Price).ThenBy(p => p.Name),
                "pricedesc" => query.OrderByDescending(p => p.Price).ThenBy(p => p.Name),
                "nameasc" => query.OrderBy(p => p.Name),
                "namedesc" => query.OrderByDescending(p => p.Name),
                "dateasc" => query.OrderBy(p => p.CreatedAt).ThenBy(p => p.Name),
                "datedesc" => query.OrderByDescending(p => p.CreatedAt).ThenBy(p => p.Name),
                _ => query.OrderByDescending(p => p.CreatedAt).ThenBy(p => p.Name)
            };

     
            var totalCount = await query.CountAsync(cancellationToken);

            var userId = userManager.GetUserId(User);

            var products = await query
                .AsNoTracking()
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Description = p.Description,
                    ImageUrl = p.ImageUrl,
                    Active = p.Active,
                    SKU = p.SKU,
                    Brend = p.Brend,
                    CategoryId = p.CategoryId,
                    CategoryName = p.Category != null ? p.Category.Name : null,
                    StockQuantity = p.StockQuantity,
                    Code = p.Code,
                    CreatedAt = p.CreatedAt,
                    IsFavorite = p.Favorites.FirstOrDefault(f => f.UserId == userId) != null ? true : false
                })
                .ToListAsync(cancellationToken);

            return new ProductGetAllResponse
            {
                Products = products,
                TotalCount = totalCount,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize),
                HasNextPage = request.PageNumber < (int)Math.Ceiling(totalCount / (double)request.PageSize),
                HasPreviousPage = request.PageNumber > 1
            };
        }
    }

  
    public class ProductGetAllRequest
    {
     
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
   

        public string? SearchQuery { get; set; }

       
        public List<int>? CategoryIds { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public decimal? MinRating { get; set; }

        
        public string? SortBy { get; set; }
      
    }


    public class ProductGetAllResponse
    {
        public required List<ProductDto> Products { get; set; }


    
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public bool HasNextPage { get; set; }
        public bool HasPreviousPage { get; set; }
    }

 
    public class ProductDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public bool Active { get; set; }
        public string? SKU { get; set; }
        public string? Brend { get; set; }
        public int? CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public int? StockQuantity { get; set; }
        public string? Code { get; set; }
        public DateTime? CreatedAt { get; set; }
        public bool IsFavorite { get; set; }
    }
}
