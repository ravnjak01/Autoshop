using Microsoft.AspNetCore.Mvc;
using RS1_2024_25.API.Data;
using RS1_2024_25.API.Helper.Api;
using static RS1_2024_25.API.Endpoints.BlogsEndpoints.BlogCommentGetByBlogId;

namespace RS1_2024_25.API.Endpoints.BlogsEndpoints
{
    [Route("blog-comment")]
    public class BlogCommentGetByBlogId(ApplicationDbContext db) : MyEndpointBaseAsync
        .WithRequest<int>
        .WithResult<BlogCommentGetByBlogId.BlogCommentsByBlogIdResponse>
    {
        [HttpGet("{id}")]
        public override async Task<BlogCommentsByBlogIdResponse> HandleAsync(int id, CancellationToken cancellationToken = default)
        {
            var comments = db.BlogComments
                .Where(c => c.BlogPostId == id)
                .ToList();  

            List<BlogCommentDto> response = new List<BlogCommentDto>();
            foreach (var c in comments)
            {
                response.Add(new BlogCommentDto
                {
                    Id = c.Id,
                    BlogPostId = c.BlogPostId,
                    UserId = c.UserId,
                    Content = c.Content,
                    CreatedAtAgo = GetTimeAgo(c.CreatedAt) 
                });
            }

            return new BlogCommentsByBlogIdResponse { Comments = response };
        }

        private string GetTimeAgo(DateTime createdAt)
        {
            var timeSpan = DateTime.Now - createdAt;

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
        }

        public class BlogCommentDto
        {
            public int Id { get; set; }
            public int BlogPostId { get; set; }
            public int? UserId { get; set; }
            public string Content { get; set; }
            public string CreatedAtAgo { get; set; }
        }
    }
}
