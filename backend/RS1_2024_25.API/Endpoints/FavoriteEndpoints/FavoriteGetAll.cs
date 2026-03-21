using Duende.IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RS1_2024_25.API.Data;
using RS1_2024_25.API.Data.Models;
using RS1_2024_25.API.Helper.Api;

namespace RS1_2024_25.API.Endpoints.FavoriteEndpoints
{
    [Route("favorite")]
    [Authorize]
    public class FavoriteGetAll(ApplicationDbContext db, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor) : MyEndpointBaseAsync
        .WithRequest<FavoriteGetAllRequest>
        .WithResult<FavoriteGetAllResponse>
    {
        [HttpGet]
        public override async Task<FavoriteGetAllResponse> HandleAsync(
            [FromQuery] FavoriteGetAllRequest request,
            CancellationToken cancellationToken = default)
        {

            request.PageNumber = Math.Max(1, request.PageNumber);
            request.PageSize = Math.Clamp(request.PageSize, 1, 100);

            var userId = userManager.GetUserId(User);

            var query = db.Favorites
                .Include(f => f.Product)
                .Where(f => f.UserId == userId)
                .AsQueryable();

            query = (request.SortBy?.ToLower()) switch
            {
                "newest" => query.OrderByDescending(f => f.Id),
                "oldest" => query.OrderBy(f => f.Id),
                "nameasc" => query.OrderBy(f => f.Product.Name),
                "namedesc" => query.OrderByDescending(f => f.Product.Name),
                _ => query.OrderByDescending(f => f.Id)
            };

            var totalCount = await query.CountAsync(cancellationToken);

            var httpContext = httpContextAccessor.HttpContext!;
            var baseUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}";

            var favorites = await query
                .AsNoTracking()
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(f => new FavoriteDto
                {
                    Id = f.Id,
                    ProductId = f.ProductId,
                    Name = f.Product.Name,
                    Price = f.Product.Price,
                    Description = f.Product.Description,
                    ImageUrl = f.Product.ImageUrl != null ? $"{baseUrl}{f.Product.ImageUrl}" : string.Empty,
                    Active = f.Product.Active && f.Product.StockQuantity > 0,
                    Brend = f.Product.Brend
                })
                .ToListAsync(cancellationToken);

            return new FavoriteGetAllResponse
            {
                Favorites = favorites,
                TotalCount = totalCount,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize),
                HasNextPage = request.PageNumber < (int)Math.Ceiling(totalCount / (double)request.PageSize),
                HasPreviousPage = request.PageNumber > 1
            };
        }
    }


    public class FavoriteGetAllRequest
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public string? SortBy { get; set; }

    }


    public class FavoriteGetAllResponse
    {
        public required List<FavoriteDto> Favorites { get; set; }
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public bool HasNextPage { get; set; }
        public bool HasPreviousPage { get; set; }
    }


    public class FavoriteDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public required string Name { get; set; }
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public string ImageUrl { get; set; }
        public bool Active { get; set; }
        public string? Brend { get; set; }

    }
}
