using Microsoft.AspNetCore.Mvc;
using RS1_2024_25.API.Data;
using RS1_2024_25.API.Helper.Api;
using RS1_2024_25.API.Helper;
using static RS1_2024_25.API.Endpoints.DiscountEndpoints.DiscountGetAll;

namespace RS1_2024_25.API.Endpoints.DiscountEndpoints
{
    [Route("discounts")]
    public class DiscountGetAll(ApplicationDbContext db) : MyEndpointBaseAsync
        .WithRequest<DiscountGetAllRequest>
        .WithResult<MyPagedList<DiscountGetAllResponse>>
    {
        [HttpGet("filter")]
        public override async Task<MyPagedList<DiscountGetAllResponse>> HandleAsync([FromQuery] DiscountGetAllRequest request, CancellationToken cancellationToken = default)
        {
            var query = db.Discounts.AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.Q))
            {
                query = query.Where(d => d.Name.Contains(request.Q));
            }

            var projectedQuery = query
                .OrderByDescending(x => x.StartDate)
                .Select(d => new DiscountGetAllResponse
                {
                    Id = d.Id,
                    Name = d.Name,
                    DiscountPercentage = d.DiscountPercentage,
                    StartDate = d.StartDate,
                    EndDate = d.EndDate,
                    RequiresPromoCode = d.RequiresPromoCode
                });

            var result = await MyPagedList<DiscountGetAllResponse>.CreateAsync(projectedQuery, request, cancellationToken);
            return result;
        }

        public class DiscountGetAllRequest : MyPagedRequest
        {
            public string? Q { get; set; } = string.Empty;
        }

        public class DiscountGetAllResponse
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public decimal DiscountPercentage { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public bool RequiresPromoCode { get; set; }
        }
    }
}
