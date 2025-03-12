using Microsoft.AspNetCore.Mvc;
using RS1_2024_25.API.Data;
using RS1_2024_25.API.Data.Models.Modul2_Basic;
using RS1_2024_25.API.Helper.Api;
using RS1_2024_25.API.Services;
using static RS1_2024_25.API.Endpoints.BlogsEndpoints.BlogRatingAdd;

namespace RS1_2024_25.API.Endpoints.BlogsEndpoints
{
    [Route("blog-rating")]
    public class BlogRatingAdd(ApplicationDbContext db, MyAuthService myAuthService) : MyEndpointBaseAsync
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

            //var userId = myAuthService.GetUserId();
            //if (userId == null)
            //{
            //    Response.StatusCode = 401;
            //    await Response.WriteAsync("Unauthorized");
            //    return;
            //}

            var blogPost = await db.BlogPosts.FindAsync(request.BlogPostId);
            if (blogPost == null)
            {
                Response.StatusCode = 404;
                await Response.WriteAsync("Blog post not found.");
                return;
            }

            var rating = new BlogRating
            {
                BlogPostId = request.BlogPostId,
                UserId = null,
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
