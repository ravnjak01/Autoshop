using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RS1_2024_25.API.Data;
using RS1_2024_25.API.Helper.Api;

namespace RS1_2024_25.API.Endpoints.DiscountEndpoints
{
    [Route("discounts")]
    public class DiscountProductsGet(ApplicationDbContext db) : MyEndpointBaseAsync
    .WithRequest<int> // DiscountId
    .WithResult<List<DiscountProductResponse>>
    {
        [HttpGet("{discountId}/products")]
        public override async Task<List<DiscountProductResponse>> HandleAsync(int discountId, CancellationToken cancellationToken = default)
        {
            var allProducts = await db.Products
                                       .Select(p => new DiscountProductResponse
                                       {
                                           ProductId = p.Id,
                                           ProductName = p.Name,
                                           IsSelected = db.DiscountProducts.Any(dp => dp.DiscountId == discountId && dp.ProductId == p.Id)
                                       })
                                       .ToListAsync(cancellationToken);

            return allProducts;
        }
    }

    public class DiscountProductResponse
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public bool IsSelected { get; set; }
    }
}
