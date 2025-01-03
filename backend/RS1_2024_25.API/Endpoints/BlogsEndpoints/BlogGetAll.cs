using RS1_2024_25.API.Data;
using RS1_2024_25.API.Helper.Api;
using RS1_2024_25.API.Helper;
using static RS1_2024_25.API.Endpoints.BlogsEndpoints.BlogGetAll;
using Microsoft.AspNetCore.Mvc;
using RS1_2024_25.API.Data.Models.Modul2_Basic;
using Microsoft.EntityFrameworkCore;

namespace RS1_2024_25.API.Endpoints.BlogsEndpoints
{
    [Route("blogposts")]
    public class BlogGetAll(ApplicationDbContext db) : MyEndpointBaseAsync
        .WithRequest<BlogGetAllRequest>
        .WithResult<BlogGetAllResponse>
    {

        [HttpGet("filter")]
        public override async Task<BlogGetAllResponse> HandleAsync([FromQuery] BlogGetAllRequest request, CancellationToken cancellationToken = default)
        {
            var blogs = await db.BlogPosts
                .Where(b => b.Active && b.IsPublished)
                .OrderBy(b => b.PublishedDate) // Možete menjati kriterijum sortiranja
                .Skip((request.PageNumber - 1) * request.PageSize) // Skip za paginaciju
                .Take(request.PageSize) // Limitiramo broj rezultata
                .Select(b => new BlogPost
                {
                    Id = b.Id,
                    Title = b.Title,
                    Author = b.Author,
                    PublishedDate = b.PublishedDate,
                    Image = b.Image
                })
                .ToListAsync();
            var totalCount = db.BlogPosts.Where(b => b.Active && b.IsPublished).ToArray().Length;


            return new BlogGetAllResponse()
            {
                TotalCount = totalCount,
                Blogs = blogs
            };
        }
        public class BlogGetAllRequest
        {
            public required int PageNumber { get; set; }
            public int PageSize { get; set; }
        }
        public class BlogGetAllResponse
        {
            public required int TotalCount { get; set; }
            public required List<BlogPost> Blogs { get; set; }
        }
    }
}

    
