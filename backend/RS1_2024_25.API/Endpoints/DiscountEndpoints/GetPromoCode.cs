using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RS1_2024_25.API.Data;
using RS1_2024_25.API.Data.Models;
using RS1_2024_25.API.Data.Models.Modul2_Basic;
using RS1_2024_25.API.Helper.Api;
using static RS1_2024_25.API.Endpoints.DiscountEndpoints.GetPromoCode;

namespace RS1_2024_25.API.Endpoints.DiscountEndpoints
{
    [Route("promo-code")]
    public class GetPromoCode(ApplicationDbContext db, UserManager<User> userManager)
        : MyEndpointBaseAsync
        .WithoutRequest
        .WithResult<PromoCodeResponse>
    {
        [HttpGet]
        public override async Task<PromoCodeResponse> HandleAsync(CancellationToken cancellationToken = default)
        {
            var now = DateTime.UtcNow;
            var code = await db.DiscountCodes
                        .Include(dc => dc.Discount)
                        .Where(dc => dc.ValidFrom <= now
                                && dc.ValidTo >= now
                                && dc.Discount != null
                                && dc.Discount.StartDate <= now
                                && dc.Discount.EndDate >= now)
                        .OrderByDescending(dc => dc.Discount.DiscountPercentage)
                        .FirstOrDefaultAsync(cancellationToken);

            if (code == null)
                return new PromoCodeResponse { IsValid = false, Message = "Promo code does not exist." };

            return new PromoCodeResponse
            {
                IsValid = true,
                PromoCode = code.Code,
                DiscountPercentage = code.Discount.DiscountPercentage
            };

        }

        public class PromoCodeResponse
        {
            public bool IsValid { get; set; }
            public string? PromoCode { get; set; } 
            public decimal? DiscountPercentage { get; set; }
            public string? Message { get; set; } = string.Empty;
        }
    }
}