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
    [Route("promo-validation")]
    [Authorize]
    public class PromoCodeValidateEndpoint(ApplicationDbContext db, UserManager<User> userManager)
        : MyEndpointBaseAsync
        .WithRequest<PromoCodeValidateRequest>
        .WithResult<PromoCodeValidateResponse>
    {
        [HttpPost]
        public override async Task<PromoCodeValidateResponse> HandleAsync(PromoCodeValidateRequest request, CancellationToken cancellationToken = default)
        {
            var userId = userManager.GetUserId(User);

            var now = DateTime.UtcNow;

            var code = await db.DiscountCodes
                .Include(dc => dc.Discount)
                .FirstOrDefaultAsync(dc => dc.Code == request.Code, cancellationToken);

            if (code == null)
                return new PromoCodeValidateResponse { IsValid = false, Message = "Promo code does not exist." };

            if (code.ValidFrom > now || code.ValidTo < now)
                return new PromoCodeValidateResponse { IsValid = false, Message = "Promo code is not active." };

            return new PromoCodeValidateResponse
            {
                IsValid = true,
                DiscountPercentage = code.Discount.DiscountPercentage,
                Message = "Promo code applied successfully!"
            };
        }

        public class PromoCodeValidateRequest
        {
            public string Code { get; set; }
        }

        public class PromoCodeValidateResponse
        {
            public bool IsValid { get; set; }
            public decimal? DiscountPercentage { get; set; }
            public string Message { get; set; } = string.Empty;
        }
    }
}