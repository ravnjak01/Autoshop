using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RS1_2024_25.API.Data;
using RS1_2024_25.API.Helper.Api;
using static RS1_2024_25.API.Endpoints.DiscountEndpoints.DiscountGetByIdForAdministration;
using Microsoft.AspNetCore.Authorization;

namespace RS1_2024_25.API.Endpoints.DiscountEndpoints
{
    [Authorize(Roles = "Admin")]

    [Route("/administration/discount")]
    [Authorize(Roles = "Admin")]
    public class DiscountGetByIdForAdministration(ApplicationDbContext db) : MyEndpointBaseAsync
        .WithRequest<int>
        .WithResult<DiscountGetByIdForAdministrationResponse>
    {
        [HttpGet("{id}")]
        public override async Task<DiscountGetByIdForAdministrationResponse> HandleAsync(int id, CancellationToken cancellationToken = default)
        {
            var discount = await db.Discounts.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (discount == null)
                throw new KeyNotFoundException("Discount not found");

            return new DiscountGetByIdForAdministrationResponse()
            {
                ID = discount.Id,
                Name = discount.Name,
                DiscountPercentage = discount.DiscountPercentage,
                StartDate = discount.StartDate,
                EndDate = discount.EndDate
            };
        }

        public class DiscountGetByIdForAdministrationResponse
        {
            public required int ID { get; set; }
            public required string Name { get; set; }
            public required decimal DiscountPercentage { get; set; }
            public required DateTime StartDate { get; set; }
            public required DateTime EndDate { get; set; }
        }
    }
}
