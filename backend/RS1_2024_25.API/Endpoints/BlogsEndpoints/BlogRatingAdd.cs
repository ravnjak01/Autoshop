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
    [Route("blog-rating")]
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

            var blogPost = await db.BlogPosts.FindAsync(request.BlogPostId);
            if (blogPost == null)
            {
                Response.StatusCode = 404;
                await Response.WriteAsync("Blog post not found.");
                return;
            }

            var userId = userManager.GetUserId(User);

            var rating = new BlogRating
            {
                BlogPostId = request.BlogPostId,
                UserId = userId,
                Rating = request.Rating,
                CreatedAt = DateTime.Now
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
