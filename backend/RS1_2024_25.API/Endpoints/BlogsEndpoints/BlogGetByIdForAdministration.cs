using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RS1_2024_25.API.Data;
using RS1_2024_25.API.Helper.Api;
using static RS1_2024_25.API.Endpoints.BlogsEndpoints.BlogGetByIdForAdministration;
using Microsoft.AspNetCore.Authorization;

namespace RS1_2024_25.API.Endpoints.BlogsEndpoints
{
    [Authorize(Roles = "Admin")]

    [Route("/administration/blogpost")]
    [Authorize(Roles = "Admin")]
    public class BlogGetByIdForAdministration(ApplicationDbContext db) : MyEndpointBaseAsync
        .WithRequest<int>
        .WithResult<BlogsGetByIdForAdministrationResponse>
    {
        [HttpGet("{id}")]
        public override async Task<BlogsGetByIdForAdministrationResponse> HandleAsync(int id, CancellationToken cancellationToken = default)
        {
            var blog = await db.BlogPosts.Include(b=> b.Author).SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (blog == null)
                throw new KeyNotFoundException("Blog not found");

            string image = null;
            if(blog.Image != null)
            {
                image = Convert.ToBase64String(blog.Image);

            }
            return new BlogsGetByIdForAdministrationResponse()
            {
                ID = blog.Id,
                Title = blog.Title,
                Content = blog.Content,
                AuthorName = blog.Author != null ? blog.Author.LastName + " " + blog.Author.FirstName : string.Empty,
                PublishedTime = blog.PublishedDate,
                IsPublished = blog.IsPublished,
                Active = blog.Active,
                Image = image
            };
        }

        public class BlogsGetByIdForAdministrationResponse
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
