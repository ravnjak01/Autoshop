using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RS1_2024_25.API.Data;
using RS1_2024_25.API.Data.Models;
using RS1_2024_25.API.Data.Models.Modul2_Basic;
using RS1_2024_25.API.Helper.Api;
using static RS1_2024_25.API.Endpoints.DiscountEndpoints.DiscountCodeAddOrUpdateForAdministration;

namespace RS1_2024_25.API.Endpoints.DiscountEndpoints
{
    [Route("discount-code-post")]
    [Authorize(Roles = "Admin")]
    public class DiscountCodeAddOrUpdateForAdministration(ApplicationDbContext db, UserManager<User> userManager) : MyEndpointBaseAsync
        .WithRequest<DiscountCodePostRequest>
        .WithoutResult
    {
        [HttpPost]
        public override async Task HandleAsync([FromForm] DiscountCodePostRequest request, CancellationToken cancellationToken = default)
        {
            var discount = await db.Discounts
                    .SingleOrDefaultAsync(d => d.Id == request.DiscountId, cancellationToken);

            if (discount == null)
            {
                throw new Exception("Discount not found.");
            }

            var now = DateTime.Now;
            if (discount.StartDate > now || discount.EndDate < now)
            {
                throw new Exception("Discount is not currently active.");
            }

            var discountCode = await db.DiscountCodes
                                       .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            var userId = userManager.GetUserId(User);

            if(string.IsNullOrEmpty(request.Code) || 
                request.DiscountId == null || request.DiscountId == 0 ||
                request.ValidFrom == null || request.ValidTo == null)
            {
                throw new ArgumentNullException("Filled the required fields");
            }

            if(request.ValidTo < request.ValidFrom)
            {
                throw new Exception("End date must be grater than start date");
            }
            if (discountCode == null)
            {
                discountCode = new DiscountCode
                {
                    Code = request.Code,
                    DiscountId = request.DiscountId,
                    ValidFrom = request.ValidFrom,
                    ValidTo = request.ValidTo,
                    LastModifiedUserId = userId
                };
                db.DiscountCodes.Add(discountCode);
            }
            else
            {
                discountCode.Code = request.Code;
                discountCode.DiscountId = request.DiscountId;
                discountCode.ValidFrom = request.ValidFrom;
                discountCode.ValidTo = request.ValidTo;
                discountCode.LastModifiedUserId = userId;
                db.DiscountCodes.Update(discountCode);
            }

            await db.SaveChangesAsync(cancellationToken);
        }

        public class DiscountCodePostRequest
        {
            public int? Id { get; set; }  // nullable, za insert
            public required string Code { get; set; }
            public required int DiscountId { get; set; }
            public required DateTime ValidFrom { get; set; }
            public required DateTime ValidTo { get; set; }
        }
    }
}
