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
    [Route("blog-rating")]
    [Authorize]
    public class BlogRatingAdd(ApplicationDbContext db, UserManager<User> userManager) : MyEndpointBaseAsync
        .WithRequest<BlogRatingRequest>
        .WithoutResult
    {
        [HttpPost]
        public override async Task HandleAsync([FromForm] BlogRatingRequest request, CancellationToken cancellationToken = default)
        {
            if (request.Rating < 1 || request.Rating > 5)
            {
                Response.StatusCode = 400;
                await Response.WriteAsync("Rating must be between 1 and 5.");
                return;
            }

            var blogPostExists = await db.BlogPosts
            .AnyAsync(x => x.Id == request.BlogPostId, cancellationToken);

            if (!blogPostExists)
                throw new Exception("Blog post not found.");

            var userId = userManager.GetUserId(User);

            if (string.IsNullOrEmpty(userId))
                throw new Exception("User not authenticated.");

            var existingRating = await db.BlogRatings
            .SingleOrDefaultAsync(x =>
                x.BlogPostId == request.BlogPostId &&
                x.UserId == userId,
                cancellationToken);

            if (existingRating == null)
            {
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
                existingRating.Rating = request.Rating;
                existingRating.CreatedAt = DateTime.Now;
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
