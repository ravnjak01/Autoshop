using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RS1_2024_25.API.Data;
using RS1_2024_25.API.Helper.Api;
using Microsoft.AspNetCore.Identity;
using RS1_2024_25.API.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace RS1_2024_25.API.Endpoints.ProductEndpoints
{
    [Route("products")] 
    public class ProductGetAll(ApplicationDbContext db, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor) : MyEndpointBaseAsync
        .WithRequest<ProductGetAllRequest>
        .WithActionResult<ProductGetAllResponse>
    {
        [HttpGet]
        public override async Task<ActionResult<ProductGetAllResponse>> HandleAsync(
            [FromQuery] ProductGetAllRequest request,
            CancellationToken cancellationToken = default)
        {
            if(request.MinPrice.HasValue && request.MinPrice.Value >= 0 &&
                request.MaxPrice.HasValue && request.MaxPrice.Value >= 0 && 
                request.MaxPrice.Value < request.MinPrice.Value)
            {
                return BadRequest("Max price must be greater than min price");
            }

            var now = DateTime.UtcNow;

            var globalDiscount = await db.Discounts
                .Where(d => d.StartDate <= now && d.EndDate >= now)
                .OrderByDescending(d => d.DiscountPercentage)
                .FirstOrDefaultAsync(cancellationToken);

            var globalDiscountPercentage = globalDiscount?.DiscountPercentage ?? 0;

            var promoCode = await db.DiscountCodes
               .Where(d => d.ValidFrom <= now && d.ValidTo >= now)
               .Select(d => d.Code)
               .FirstOrDefaultAsync(cancellationToken);

            request.PageNumber = Math.Max(1, request.PageNumber);
            request.PageSize = Math.Clamp(request.PageSize, 1, 100);


            var isAdmin = User.IsInRole("Admin");
            var query = db.Products
                .Include(p => p.Category)
                .AsNoTracking()
                .AsQueryable();

            if (!isAdmin)
            {
                query = query.Where(p => p.Active && p.StockQuantity > 0);
            }


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
                query = query.Where(p => request.CategoryIds.Contains(p.CategoryId));
            }

            if (request.MinPrice.HasValue && request.MinPrice.Value >= 0)
                query = query.Where(p => p.Price >= request.MinPrice.Value);

            if (request.MaxPrice.HasValue && request.MaxPrice.Value >= 0)
                query = query.Where(p => p.Price <= request.MaxPrice.Value);

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

            var httpContext = httpContextAccessor.HttpContext!;
            var baseUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}";

            var productIds = await query.Select(p => p.Id).ToListAsync(cancellationToken);
            var categoryIds = await query.Select(p => p.CategoryId).Distinct().ToListAsync(cancellationToken);

            var categoryDiscounts = await db.DiscountCategories
                 .Where(dc => categoryIds.Contains(dc.CategoryId))
                    .ToDictionaryAsync(dc => dc.CategoryId, dc => dc.Discount.DiscountPercentage, cancellationToken);

            var productDiscounts = await db.DiscountProducts
                .Where(dp => productIds.Contains(dp.ProductId))
                .ToDictionaryAsync(dp => dp.ProductId, dp => dp.Discount.DiscountPercentage, cancellationToken);
            var rawProducts = await query
            .AsNoTracking()
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(p => new
            {
                p.Id,
                p.Name,
                p.Price,
                p.ImageUrl,
                p.Brend,
                p.CategoryId,
                CategoryName = p.Category != null ? p.Category.Name : null,
                IsFavorite = p.Favorites.Any(f => f.UserId == userId),
                p.StockQuantity,
                p.SKU
            })
            .ToListAsync(cancellationToken);

            var products = rawProducts.Select(x => new ProductDto
            {
                Id = x.Id,
                Name = x.Name,
                Price = x.Price,
                ImageUrl = x.ImageUrl.StartsWith("http")
                        ? x.ImageUrl
                        : $"{baseUrl.TrimEnd('/')}/{x.ImageUrl?.TrimStart('/')}",
                Brend = x.Brend,
                CategoryName = x.CategoryName,
                IsFavorite = x.IsFavorite,
                PriceAfterGlobalDiscount = x.Price - (x.Price * globalDiscountPercentage / 100),
                BadgeDiscountPercentage =
                      (categoryDiscounts.TryGetValue(x.CategoryId, out var cd) ? cd : 0) +
                 (productDiscounts.TryGetValue(x.Id, out var pd) ? pd : 0),
                SKU =x.SKU,
                StockQuantity = x.StockQuantity
            }).ToList();


            return new ProductGetAllResponse
            {
                Products = products,
                TotalCount = totalCount,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize),
                HasNextPage = request.PageNumber < (int)Math.Ceiling(totalCount / (double)request.PageSize),
                HasPreviousPage = request.PageNumber > 1,
                PromoCode = promoCode,
             
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
        public string? PromoCode { get; set; }
    }

    public class ProductDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
        public string? Brend { get; set; }
        public string? CategoryName { get; set; }
        public bool IsFavorite { get; set; }
        public decimal? PriceAfterGlobalDiscount { get; set; }
        public decimal? BadgeDiscountPercentage { get; set; }
        public string? SKU { get; set; }
        public int StockQuantity { get; set; }
    }
}