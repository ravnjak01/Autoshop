using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
        [HttpPost]
        public override async Task HandleAsync([FromForm] BlogRatingRequest request, CancellationToken cancellationToken = default)
        {
            if (request.Rating < 1 || request.Rating > 5)
            {
                throw new ArgumentException("Rating must be between 1 and 5.");
            }

            var blogPost = await db.BlogPosts.FindAsync(request.BlogPostId);
            if (blogPost == null)
            {
                throw new KeyNotFoundException("Blog post not found.");
            }

            var userId = userManager.GetUserId(User);
            var rating = new BlogRating
            {
                BlogPostId = request.BlogPostId,
                UserId = userId,
                Rating = request.Rating,
                CreatedAt = DateTime.UtcNow
            };

            db.BlogRatings.Add(rating);
            await db.SaveChangesAsync(cancellationToken);
        }

        public class BlogRatingRequest
        {
            public int BlogPostId { get; set; }
            public int Rating { get; set; }
        }
    }
}
