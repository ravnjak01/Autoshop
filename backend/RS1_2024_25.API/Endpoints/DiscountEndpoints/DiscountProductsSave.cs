using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RS1_2024_25.API.Data;
using RS1_2024_25.API.Data.Models;
using RS1_2024_25.API.Data.Models.Modul2_Basic;
using RS1_2024_25.API.Helper.Api;
using static RS1_2024_25.API.Endpoints.DiscountEndpoints.DiscountProductsSave;

namespace RS1_2024_25.API.Endpoints.DiscountEndpoints
{
    [Authorize(Roles = "Admin")]

    [Route("discounts")]
    [Authorize(Roles = "Admin")]
    public class DiscountProductsSave(ApplicationDbContext db, UserManager<User> userManager) : MyEndpointBaseAsync
    .WithRequest<DiscountProductsSaveRequest>
    .WithoutResult
    {
        [HttpPost("save-products")]
        public override async Task HandleAsync([FromBody] DiscountProductsSaveRequest request, CancellationToken cancellationToken = default)
        {
            var discount = await db.Discounts
           .SingleOrDefaultAsync(d => d.Id == request.DiscountId, cancellationToken);

            if (discount == null)
                throw new Exception("Discount not found.");

            var userId = userManager.GetUserId(User);

            await using var transaction = await db.Database.BeginTransactionAsync(cancellationToken);

            var existingIds = await db.DiscountProducts
                .Where(x => x.DiscountId == request.DiscountId)
                .Select(x => x.ProductId)
                .ToListAsync(cancellationToken);

            var toAdd = request.ProductIds.Except(existingIds)
                .Select(prodId => new DiscountProduct
                {
                    DiscountId = request.DiscountId,
                    ProductId = prodId,
                    LastModifiedUserId = userId
                });

            var toRemove = db.DiscountProducts
                .Where(x => x.DiscountId == request.DiscountId && !request.ProductIds.Contains(x.ProductId));

            db.DiscountProducts.RemoveRange(toRemove);
            await db.DiscountProducts.AddRangeAsync(toAdd, cancellationToken);

            await db.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        }

        public class DiscountProductsSaveRequest
        {
            public int DiscountId { get; set; }
            public List<int> ProductIds { get; set; } = new();
        }
    }
}
