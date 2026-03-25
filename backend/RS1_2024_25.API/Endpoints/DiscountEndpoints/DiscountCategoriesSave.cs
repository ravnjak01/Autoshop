using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RS1_2024_25.API.Data;
using RS1_2024_25.API.Data.Models;
using RS1_2024_25.API.Data.Models.Modul2_Basic;
using RS1_2024_25.API.Helper.Api;
using static RS1_2024_25.API.Endpoints.DiscountEndpoints.DiscountCategoriesSave;

namespace RS1_2024_25.API.Endpoints.DiscountEndpoints
{
    [Route("discounts")]
    [Authorize(Roles = "Admin")]
    public class DiscountCategoriesSave(ApplicationDbContext db, UserManager<User> userManager) : MyEndpointBaseAsync
    .WithRequest<DiscountCategoriesSaveRequest>
    .WithoutResult
    {
        [HttpPost("save-categories")]
        public override async Task HandleAsync([FromBody] DiscountCategoriesSaveRequest request, CancellationToken cancellationToken = default)
        {
            var discount = await db.Discounts
                   .SingleOrDefaultAsync(d => d.Id == request.DiscountId, cancellationToken);

            if (discount == null)
            {
                throw new Exception("Discount not found.");
            }

            var userId = userManager.GetUserId(User);

            var strategy = db.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                await using var transaction = await db.Database.BeginTransactionAsync(cancellationToken);

                var existingIds = await db.DiscountCategories
                    .Where(x => x.DiscountId == request.DiscountId)
                    .Select(x => x.CategoryId)
                    .ToListAsync(cancellationToken);

                var toAdd = request.CategoryIds.Except(existingIds)
                    .Select(catId => new DiscountCategory
                    {
                        DiscountId = request.DiscountId,
                        CategoryId = catId,
                        LastModifiedUserId = userId
                    });

                var toRemove = db.DiscountCategories
                    .Where(x => x.DiscountId == request.DiscountId && !request.CategoryIds.Contains(x.CategoryId));

                db.DiscountCategories.RemoveRange(toRemove);
                await db.DiscountCategories.AddRangeAsync(toAdd, cancellationToken);

                await db.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
            });
        }

        public class DiscountCategoriesSaveRequest
        {
            public int DiscountId { get; set; }
            public List<int> CategoryIds { get; set; } = new();
        }
    }

}
