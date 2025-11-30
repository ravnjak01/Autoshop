using Duende.IdentityServer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RS1_2024_25.API.Data;
using RS1_2024_25.API.Data.Models.ShoppingCart;
using RS1_2024_25.API.Helper.Api;
using static Duende.IdentityServer.Models.IdentityResources;
using System.Globalization;

namespace RS1_2024_25.API.Endpoints.ProductEndpoints
{
    [Route("product")]
    public class ProductGetAll(ApplicationDbContext db) : MyEndpointBaseAsync
        .WithRequest<ProductGetAllRequest>
        .WithResult<ProductGetAllResponse>
    {
        [HttpGet("filter")]
        public override async Task<ProductGetAllResponse> HandleAsync(
            [FromQuery] ProductGetAllRequest request,
            CancellationToken cancellationToken = default)
        {
       
            request.PageNumber = Math.Max(1, request.PageNumber);
            request.PageSize = Math.Clamp(request.PageSize, 1, 100);

            var query = db.Products
                .Include(p => p.Category)
                .AsQueryable();

   
            if (!string.IsNullOrWhiteSpace(request.SearchQuery))
            {
                var searchLower = request.SearchQuery.Trim().ToLower();
                query = query.Where(p =>
                    p.Name.ToLower().Contains(searchLower) ||
                    (p.Code != null && p.Code.ToLower().Contains(searchLower)) ||
                    (p.SKU != null && p.SKU.ToLower().Contains(searchLower)));
            }

      
            if (request.CategoryIds?.Any() == true)
            {
                query = query.Where(p =>
                    request.CategoryIds.Contains(p.CategoryId));
                   
            }

        
            if (!string.IsNullOrWhiteSpace(request.Brand))
            {
                var brandLower = request.Brand.ToLower();
                query = query.Where(p => p.Brend.ToLower().Contains(brandLower));
            }

      
            if (request.MinPrice.HasValue && request.MinPrice.Value >= 0)
            {
                query = query.Where(p => p.Price >= request.MinPrice.Value);
            }

            if (request.MaxPrice.HasValue && request.MaxPrice.Value >= 0)
            {
                query = query.Where(p => p.Price <= request.MaxPrice.Value);
            }

           
            if (request.IsActive.HasValue)
            {
                query = query.Where(p => p.Active == request.IsActive.Value);
            }

        
            if (request.InStock == true)
            {
                query = query.Where(p => p.StockQuantity > 0);
            }

          

          
            if (request.CreatedAfter.HasValue)
            {
                query = query.Where(p => p.CreatedAt >= request.CreatedAfter.Value);
            }

            if (request.CreatedBefore.HasValue)
            {
                query = query.Where(p => p.CreatedAt <= request.CreatedBefore.Value);
            }

          
            query = (request.SortBy?.ToLower()) switch
            {
                "priceasc" => query.OrderBy(p => p.Price).ThenBy(p => p.Name),
                "pricedesc" => query.OrderByDescending(p => p.Price).ThenBy(p => p.Name),
                "nameasc" => query.OrderBy(p => p.Name),
                "namedesc" => query.OrderByDescending(p => p.Name),
                "ratingdesc" => query.OrderByDescending(p => p.AvgGrade).ThenBy(p => p.Name),
                "ratingasc" => query.OrderBy(p => p.AvgGrade).ThenBy(p => p.Name),
                "dateasc" => query.OrderBy(p => p.CreatedAt).ThenBy(p => p.Name),
                "datedesc" => query.OrderByDescending(p => p.CreatedAt).ThenBy(p => p.Name),
                "popularitydesc" => query.OrderByDescending(p => p.NumberOfReviews).ThenBy(p => p.Name),
                _ => query.OrderByDescending(p => p.CreatedAt).ThenBy(p => p.Name)
            };

     
            var totalCount = await query.CountAsync(cancellationToken);

         
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
                    AvgGrade = p.AvgGrade,
                    NumberOfReviews = p.NumberOfReviews,
                    StockQuantity = p.StockQuantity,
                    Code = p.Code,
                    CreatedAt = p.CreatedAt
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
        public string? Brand { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public bool? IsActive { get; set; }
        public bool? InStock { get; set; }
        public decimal? MinRating { get; set; }
        public DateTime? CreatedAfter { get; set; }
        public DateTime? CreatedBefore { get; set; }

        
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
        public decimal? AvgGrade { get; set; }
        public int? NumberOfReviews { get; set; }
        public int? StockQuantity { get; set; }
        public string? Code { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
