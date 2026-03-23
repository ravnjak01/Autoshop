using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RS1_2024_25.API.Data;
using RS1_2024_25.API.Data.Models;
using RS1_2024_25.API.Data.Models.Modul2_Basic;
using RS1_2024_25.API.Helper.Api;
using RS1_2024_25.API.Services;
using static RS1_2024_25.API.Endpoints.BlogsEndpoints.BlogRatingAdd;

namespace RS1_2024_25.API.Endpoints.BlogsEndpoints
{
    [Authorize]
    [Route("blog-rating")]
    public class BlogRatingAdd(ApplicationDbContext db, UserManager<User> userManager) : MyEndpointBaseAsync
        .WithRequest<BlogRatingRequest>
        .WithoutResult
    {
        [HttpPost]
        public override async Task HandleAsync([FromForm] BlogRatingRequest request, CancellationToken cancellationToken = default)
        {
            // 1. Validacija - Middleware vraća 400 Bad Request
            if (request.Rating < 1 || request.Rating > 5)
            {
                throw new ArgumentException("Rating must be between 1 and 5.");
            }

            // 2. Provjera postojanja bloga - Middleware vraća 404 Not Found
            var blogPostExists = await db.BlogPosts
                .AnyAsync(x => x.Id == request.BlogPostId, cancellationToken);

            if (!blogPostExists)
            {
                throw new KeyNotFoundException($"Blog post with ID {request.BlogPostId} was not found.");
            }

            // 3. Provjera autentifikacije - Middleware vraća 401 Unauthorized
            var userId = userManager.GetUserId(User);
            if (string.IsNullOrEmpty(userId))
            {
                throw new UnauthorizedAccessException("You must be logged in to rate a blog post.");
            }

            // 4. Provjera da li je korisnik već ocijenio ovaj blog
            var existingRating = await db.BlogRatings
                .SingleOrDefaultAsync(x =>
                    x.BlogPostId == request.BlogPostId &&
                    x.UserId == userId,
                    cancellationToken);

            if (existingRating == null)
            {
                // Kreiranje nove ocjene
                db.BlogRatings.Add(new BlogRating
                {
                    BlogPostId = request.BlogPostId,
                    UserId = userId,
                    Rating = request.Rating,
                    CreatedAt = DateTime.UtcNow
                });
            }
            else
            {
                // Ažuriranje postojeće ocjene
                existingRating.Rating = request.Rating;
                existingRating.CreatedAt = DateTime.UtcNow; // Preporuka koristiti UtcNow radi konzistentnosti
            }

            await db.SaveChangesAsync(cancellationToken);
        }

        public class BlogRatingRequest
        {
            public int BlogPostId { get; set; }
            public int Rating { get; set; }
        }
    }
}