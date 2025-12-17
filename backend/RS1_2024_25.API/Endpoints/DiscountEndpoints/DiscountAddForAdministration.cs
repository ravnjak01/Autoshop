using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RS1_2024_25.API.Data;
using RS1_2024_25.API.Data.Models;
using RS1_2024_25.API.Data.Models.Modul2_Basic;
using RS1_2024_25.API.Helper.Api;
using RS1_2024_25.API.Services;
using static RS1_2024_25.API.Endpoints.DiscountEndpoints.DiscountAddForAdministration;


namespace RS1_2024_25.API.Endpoints.DiscountEndpoints
{

    [Route("discount-post")]
    public class DiscountAddForAdministration(ApplicationDbContext db, UserManager<User> userManager) : MyEndpointBaseAsync
        .WithRequest<DiscountPostUpdateOrInsertRequest>
        .WithoutResult
    {
        [HttpPost]
        public override async Task HandleAsync([FromForm] DiscountPostUpdateOrInsertRequest request, CancellationToken cancellationToken = default)
        {

            var discount = await db.Discounts.SingleOrDefaultAsync(x => x.Id == request.ID, cancellationToken);

            var userId = userManager.GetUserId(User);

            // Kreiranje ili ažuriranje blog posta
            if (discount == null)
            {
                discount = new Discount
                {
                    Name = request.Name,
                    DiscountPercentage = request.DiscountPercentage, 
                    StartDate = request.StartDate,
                    EndDate = request.EndDate,
                    CreateUserId = userId
                };
                db.Discounts.Add(discount);
            }
            else
            {
                discount.Name = request.Name;
                discount.DiscountPercentage = request.DiscountPercentage;
                discount.StartDate = request.StartDate;
                discount.EndDate = request.EndDate;
                db.Discounts.Update(discount); 
            }
            await db.SaveChangesAsync(cancellationToken);
        }

        public class DiscountPostUpdateOrInsertRequest
        {
            public int? ID { get; set; } // Nullable to allow null for insert operations
            public required string Name { get; set; }
            public required decimal DiscountPercentage { get; set; }
            public required DateTime StartDate { get; set; }
            public required DateTime EndDate { get; set; }
        }

        public class DiscountPostUpdateOrInsertResponse
        {
            public required int ID { get; set; }
            public required string Name { get; set; }
            public required decimal DiscountPercentage { get; set; }
            public required DateTime StartDate { get; set; }
            public required DateTime EndDate { get; set; }
        }
    }
}


