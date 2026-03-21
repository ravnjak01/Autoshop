using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RS1_2024_25.API.Data;
using RS1_2024_25.API.Data.Models;
using RS1_2024_25.API.Helper.Api;
using static RS1_2024_25.API.Endpoints.DiscountEndpoints.PromoCodeValidateEndpoint;

namespace RS1_2024_25.API.Endpoints.DiscountEndpoints
{
    [Route("promo-code")]
    [Authorize] 
    public class PromoCodeValidateEndpoint(ApplicationDbContext db, UserManager<User> userManager)
        : MyEndpointBaseAsync
        .WithoutRequest
        .WithResult<PromoCodeValidateResponse>
    {
        [HttpGet]
        public override async Task<PromoCodeValidateResponse> HandleAsync(CancellationToken cancellationToken = default)
        {
            var userId = userManager.GetUserId(User);

            var now = DateTime.Now;
            var codes = await db.DiscountCodes
                        .Include(dc => dc.Discount)
                        .ToListAsync(cancellationToken); 

            var code = codes
                .Where(dc => dc.IsActive
                                && dc.ValidFrom <= now
                                && dc.ValidTo >= now
                                && dc.Discount != null
                                && dc.Discount.StartDate <= now
                                && dc.Discount.EndDate >= now)
                .OrderByDescending(dc => dc.Discount.DiscountPercentage)
                .FirstOrDefault();

            if (code == null)
                return new PromoCodeValidateResponse { IsValid = false, Message = "Promo code does not exist." };

            return new PromoCodeValidateResponse
            {
                IsValid = true,
                PromoCode = code.Code,
                DiscountPercentage = code.Discount.DiscountPercentage
            };

        }

        public class PromoCodeValidateResponse
        {
            public bool IsValid { get; set; }
            public string? PromoCode { get; set; } 
            public decimal? DiscountPercentage { get; set; }
            public string? Message { get; set; } = string.Empty;
        }
    }
}