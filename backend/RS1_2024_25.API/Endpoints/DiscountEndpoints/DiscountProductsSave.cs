using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RS1_2024_25.API.Data;
using RS1_2024_25.API.Data.Models;
using RS1_2024_25.API.Data.Models.Modul2_Basic;
using RS1_2024_25.API.Helper.Api;
using static RS1_2024_25.API.Endpoints.DiscountEndpoints.DiscountProductsSave;

namespace RS1_2024_25.API.Endpoints.DiscountEndpoints
{
    [Route("discounts")]
    public class DiscountProductsSave(ApplicationDbContext db, UserManager<User> userManager) : MyEndpointBaseAsync
    .WithRequest<DiscountProductsSaveRequest>
    .WithoutResult
    {
        [HttpPost("save-products")]
        public override async Task HandleAsync([FromBody] DiscountProductsSaveRequest request, CancellationToken cancellationToken = default)
        {
            var existing = db.DiscountProducts.Where(x => x.DiscountId == request.DiscountId);
            db.DiscountProducts.RemoveRange(existing);

            var userId = userManager.GetUserId(User);

            var newItems = request.ProductIds.Select(prod => new DiscountProduct
            {
                DiscountId = request.DiscountId,
                ProductId = prod,
                LastModifiedUserId = userId
            });

            await db.DiscountProducts.AddRangeAsync(newItems, cancellationToken);
            await db.SaveChangesAsync(cancellationToken);
        }

        public class DiscountProductsSaveRequest
        {
            public int DiscountId { get; set; }
            public List<int> ProductIds { get; set; } = new();
        }
    }
}
