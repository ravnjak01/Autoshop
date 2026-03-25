
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RS1_2024_25.API.Data;
using RS1_2024_25.API.Helper.Api;
using static RS1_2024_25.API.Endpoints.BlogsEndpoints.BlogRatingGetByBlogId;

namespace RS1_2024_25.API.Endpoints.BlogsEndpoints
{
    [Route("blog-rating")]
    public class BlogRatingGetByBlogId(ApplicationDbContext db) : MyEndpointBaseAsync
        .WithRequest<int>
        .WithResult<BlogRatingByBlogIdResponse>
    {
        [HttpGet("{id}")]
        public override async Task<BlogRatingByBlogIdResponse> HandleAsync(int id, CancellationToken cancellationToken = default)
        {
            var exists = await db.BlogPosts.AnyAsync(x => x.Id == id, cancellationToken);
            if (!exists)
            {
                throw new KeyNotFoundException("Blog not found");
            }

            var averageRating = await db.BlogRatings
                .Where(r => r.BlogPostId == id)
                .Select(r => (double?)r.Rating)
                .AverageAsync(cancellationToken);

            return new BlogRatingByBlogIdResponse
            {
                AverageRating = averageRating ?? 0
            };
        }

        public class BlogRatingByBlogIdResponse 
        { 
            public double AverageRating { get; set; }
        }
    }
}
