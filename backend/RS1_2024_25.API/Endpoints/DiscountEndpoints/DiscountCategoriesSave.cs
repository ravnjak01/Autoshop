using Microsoft.AspNetCore.Mvc;
using RS1_2024_25.API.Data;
using RS1_2024_25.API.Data.Models.Modul2_Basic;
using RS1_2024_25.API.Helper.Api;
using static RS1_2024_25.API.Endpoints.DiscountEndpoints.DiscountCategoriesSave;

namespace RS1_2024_25.API.Endpoints.DiscountEndpoints
{
    [Route("discounts")]
    public class DiscountCategoriesSave(ApplicationDbContext db) : MyEndpointBaseAsync
    .WithRequest<DiscountCategoriesSaveRequest>
    .WithoutResult
    {
        [HttpPost("save-categories")]
        public override async Task HandleAsync([FromBody] DiscountCategoriesSaveRequest request, CancellationToken cancellationToken = default)
        {
            var existing = db.DiscountCategories.Where(x => x.DiscountId == request.DiscountId);
            db.DiscountCategories.RemoveRange(existing);

            var newItems = request.CategoryIds.Select(catId => new DiscountCategory
            {
                DiscountId = request.DiscountId,
                CategoryId = catId
            });

            await db.DiscountCategories.AddRangeAsync(newItems, cancellationToken);
            await db.SaveChangesAsync(cancellationToken);
        }

        public class DiscountCategoriesSaveRequest
        {
            public int DiscountId { get; set; }
            public List<int> CategoryIds { get; set; } = new();
        }
    }

}
