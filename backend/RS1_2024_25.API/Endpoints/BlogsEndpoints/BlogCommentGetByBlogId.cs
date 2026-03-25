using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RS1_2024_25.API.Data;
using RS1_2024_25.API.Helper.Api;
using static RS1_2024_25.API.Endpoints.BlogsEndpoints.BlogCommentGetByBlogId;

namespace RS1_2024_25.API.Endpoints.BlogsEndpoints
{
    [Route("blog-comment")]
    public class BlogCommentGetByBlogId(ApplicationDbContext db) : MyEndpointBaseAsync
        .WithRequest<BlogCommentsRequest>
        .WithResult<BlogCommentsByBlogIdResponse>
    {
        [HttpGet]
        public override async Task<BlogCommentsByBlogIdResponse> HandleAsync([FromQuery]BlogCommentsRequest request, CancellationToken cancellationToken = default)
        {
            var exists = await db.BlogPosts
                .AnyAsync(x => x.Id == request.BlogId, cancellationToken);

            if (!exists)
            {
                throw new KeyNotFoundException("Blog not found");
            }

            var query = db.BlogComments
                .Include(c => c.User)
                .Where(c => c.BlogPostId == request.BlogId);

            var totalCount = await query.CountAsync(cancellationToken);

            var commentsData = await query
                .OrderByDescending(c => c.CreatedAt)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(c => new
                {
                    c.Id,
                    c.BlogPostId,
                    Username = c.User != null ? c.User.UserName : string.Empty,
                    c.Content,
                    c.CreatedAt
                })
                .ToListAsync(cancellationToken);

            var comments = commentsData.Select(c => new BlogCommentDto
            {
                Id = c.Id,
                BlogPostId = c.BlogPostId,
                Username = c.Username,
                Content = c.Content,
                CreatedAtAgo = GetTimeAgo(c.CreatedAt)
            }).ToList();

            return new BlogCommentsByBlogIdResponse
            {
                Comments = comments,
                TotalCount = totalCount
            };
        }

        private string GetTimeAgo(DateTime createdAt)
        {
            var timeSpan = DateTime.UtcNow - createdAt;

            if (timeSpan.TotalSeconds < 60)
                return $"{timeSpan.TotalSeconds} s";
            if (timeSpan.TotalMinutes < 60)
                return $"{timeSpan.Minutes} min";
            if (timeSpan.TotalHours < 24)
                return $"{timeSpan.Hours} h";
            if (timeSpan.TotalDays < 30)
                return $"{timeSpan.Days} d";
            if (timeSpan.TotalDays < 365)
                return $"{timeSpan.Days / 30} m";
            return $"{timeSpan.Days / 365} g";
        }

        public class BlogCommentsByBlogIdResponse
        {
            public List<BlogCommentDto> Comments { get; set; } = new();
            public int TotalCount { get; set; }
        }

        public class BlogCommentDto
        {
            public int Id { get; set; }
            public int BlogPostId { get; set; }
            public string? Username { get; set; }
            public string Content { get; set; }
            public string CreatedAtAgo { get; set; }
        }

        public class BlogCommentsRequest
        {
            public int BlogId { get; set; }
            public int Page { get; set; } = 1;
            public int PageSize { get; set; } = 10;
        }
    }
}
