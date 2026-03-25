using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RS1_2024_25.API.Data;
using RS1_2024_25.API.Helper.Api;
using static RS1_2024_25.API.Endpoints.BlogsEndpoints.BlogGetById;

namespace RS1_2024_25.API.Endpoints.BlogsEndpoints
{
    [Route("/blogpost")]
    public class BlogGetById(ApplicationDbContext db, IHttpContextAccessor httpContextAccessor) : MyEndpointBaseAsync
        .WithRequest<int>
        .WithResult<BlogsGetByIdResponse>
    {
        [HttpGet("{id}")]
        public override async Task<BlogsGetByIdResponse> HandleAsync(int id, CancellationToken cancellationToken = default)
        {
            var blog = await db.BlogPosts.Include(b=> b.Author).SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (blog == null)
                throw new KeyNotFoundException("Blog not found");

            var httpContext = httpContextAccessor.HttpContext!;
            var baseUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}";

            return new BlogsGetByIdResponse()
            {
                ID = blog.Id,
                Title = blog.Title,
                Content = blog.Content,
                AuthorName = blog.Author != null ? blog.Author.LastName + " " + blog.Author.FirstName : string.Empty,
                PublishedTime = blog.PublishedDate,
                IsPublished = blog.IsPublished,
                Active = blog.Active,
                Image = blog.ImagePath != null
                        ? $"{baseUrl}/blogposts/{blog.Id}/image"
                        : null
            };
        }

        public class BlogsGetByIdResponse
        {
            public required int ID { get; set; }
            public required string Title { get; set; }
            public required string Content { get; set; }
            public string? AuthorName { get; set; }
            public DateTime? PublishedTime { get; set; }
            public bool IsPublished { get; set; }
            public bool Active { get; set; }
            public string? Image { get; set; }
        }
    }
}
