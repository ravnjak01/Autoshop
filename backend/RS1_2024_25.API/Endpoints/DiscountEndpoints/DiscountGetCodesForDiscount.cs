using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RS1_2024_25.API.Data;
using RS1_2024_25.API.Helper.Api;
using static RS1_2024_25.API.Endpoints.DiscountEndpoints.DiscountGetCodesForDiscount;

namespace RS1_2024_25.API.Endpoints.DiscountEndpoints
{
    [Route("discount-codes")]
    public class DiscountGetCodesForDiscount(ApplicationDbContext db) : MyEndpointBaseAsync
        .WithRequest<int>
        .WithResult<List<DiscountCodeResponse>>
    {
        [HttpGet("{discountId}")]
        public override async Task<List<DiscountCodeResponse>> HandleAsync(int discountId, CancellationToken cancellationToken = default)
        {
            var codes = await db.DiscountCodes
                .Where(x => x.DiscountId == discountId)
                .Select(x => new DiscountCodeResponse
                {
                    Id = x.Id,
                    Code = x.Code,
                    ValidFrom = x.ValidFrom,
                    ValidTo = x.ValidTo
                }).ToListAsync(cancellationToken);

            return codes;
        }

        public class DiscountCodeResponse
        {
            public int Id { get; set; }
            public string Code { get; set; } = string.Empty;
            public DateTime? ValidFrom { get; set; }
            public DateTime? ValidTo { get; set; }
        }
    }
}
