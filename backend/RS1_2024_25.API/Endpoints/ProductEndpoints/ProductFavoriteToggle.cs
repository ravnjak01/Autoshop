using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RS1_2024_25.API.Data;
using RS1_2024_25.API.Data.Models;
using RS1_2024_25.API.Data.Models.Modul2_Basic;
using RS1_2024_25.API.Helper.Api;

namespace RS1_2024_25.API.Endpoints.ProductEndpoints
{
    [Route("product/favorite/toggle")]
    [Authorize]
    public class ProductFavoriteToggle(ApplicationDbContext db, UserManager<User> userManager) : MyEndpointBaseAsync
        .WithRequest<int>
        .WithResult<bool>
    {
        [HttpPost("{id}")]
        public override async Task<bool> HandleAsync(int id, CancellationToken cancellationToken = default)
        {
            var userId = userManager.GetUserId(User);

            if (string.IsNullOrEmpty(userId))
            {
                throw new UnauthorizedAccessException("User is not authenticated.");
            }

            var productExists = await db.Products.AnyAsync(x => x.Id == id, cancellationToken);

            if (!productExists)
            {
                throw new Exception("Product not found.");
            }

            var favorite = await db.Favorites.SingleOrDefaultAsync(x => x.ProductId == id && x.UserId == userId, cancellationToken);

            var isFavorite = true;

            if (favorite == null)
            {
                db.Add(new Favorite
                {
                    ProductId = id,
                    UserId = userId,
                    CreatedAt = DateTime.UtcNow
                });
            }
            else
            {
                db.Remove(favorite);
                isFavorite = false;
            }

            await db.SaveChangesAsync(cancellationToken);

            return isFavorite;
        }
    }
}
