using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RS1_2024_25.API.Data;
using RS1_2024_25.API.Helper.Api;

namespace RS1_2024_25.API.Endpoints.DiscountEndpoints
{
    [Route("discounts")]
    public class DiscountCategoriesGet(ApplicationDbContext db) : MyEndpointBaseAsync
    .WithRequest<int> // DiscountId
    .WithResult<List<DiscountCategoryResponse>> // List of selected CategoryIds
    {
        [HttpGet("{discountId}/categories")]
        public override async Task<List<DiscountCategoryResponse>> HandleAsync(int discountId, CancellationToken cancellationToken = default)
        {
            var allCategories = await db.Categories
                                       .Select(c => new DiscountCategoryResponse
                                       {
                                           CategoryId = c.Id,
                                           CategoryName = c.Name,
                                           IsSelected = db.DiscountCategories.Any(dc => dc.DiscountId == discountId && dc.CategoryId == c.Id)
                                       })
                                       .ToListAsync(cancellationToken);

            return allCategories;
        }
    }

    public class DiscountCategoryResponse
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public bool IsSelected { get; set; }
    }
}
