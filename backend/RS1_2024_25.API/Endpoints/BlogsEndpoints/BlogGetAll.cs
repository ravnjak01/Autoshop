using RS1_2024_25.API.Data;
using RS1_2024_25.API.Helper.Api;
using static RS1_2024_25.API.Endpoints.BlogsEndpoints.BlogGetAll;
using Microsoft.AspNetCore.Mvc;
using RS1_2024_25.API.Data.Models.Modul2_Basic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

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
            var query = db.BlogPosts
                .Where(b => b.Active && b.IsPublished); // Base filter for active and published blogs

            // If a search query is provided, filter the blogs by title and content (adjust as necessary)
            if (!string.IsNullOrEmpty(request.SearchQuery))
            {
                query = query.Where(b =>
                                    (b.Title != null && b.Title.Contains(request.SearchQuery)) ||
                                    (b.Content != null && b.Content.Contains(request.SearchQuery)) ||
                                    (b.Author != null && b.Author.UserName != null &&
                                     b.Author.UserName.Contains(request.SearchQuery))
                                );

            }

            // Apply pagination
            var blogs = await query
                .OrderBy(b => b.PublishedDate) // Sorting by published date, adjust as needed
                .Skip((request.PageNumber - 1) * request.PageSize) // Pagination
                .Take(request.PageSize) // Limit the number of results
                .Select(b => new BlogPostDTO
                {
                    Id = b.Id,
                    Title = b.Title,
                    AuthorName = b.Author != null ?  b.Author.LastName + " " +  b.Author.FirstName : string.Empty,
                    PublishedDate = b.PublishedDate,
                    Image = b.Image
                })
                .ToListAsync(cancellationToken);

            // Calculate the total count (including the filter for search query)
            var totalCount = await query.CountAsync(cancellationToken);

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
            public string? SearchQuery { get; set; }
        }
        public class BlogGetAllResponse
        {
            public required int TotalCount { get; set; }
            public required List<BlogPostDTO> Blogs { get; set; }
        }
        public class BlogPostDTO
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string AuthorName { get; set; }
            public DateTime? PublishedDate { get; set; }
            public byte[]? Image { get; set; }
        }
    }
}

    
