using Microsoft.AspNetCore.Mvc;
using RS1_2024_25.API.Data;
using RS1_2024_25.API.Data.Models.Modul2_Basic;
using RS1_2024_25.API.Helper.Api;
using RS1_2024_25.API.Services;
using static RS1_2024_25.API.Endpoints.BlogsEndpoints.BlogCommentAdd;

namespace RS1_2024_25.API.Endpoints.BlogsEndpoints
{
    [Route("blog-comment")]
    public class BlogCommentAdd(ApplicationDbContext db, MyAuthService myAuthService) : MyEndpointBaseAsync
        .WithRequest<BlogCommentRequest>
        .WithoutResult
    {
        [HttpPost]
        public override async Task HandleAsync([FromForm] BlogCommentRequest request, CancellationToken cancellationToken = default)
        {
            var comment = new BlogComment
            {
                BlogPostId = request.BlogPostId,
                UserId = request.UserId,
                Content = request.Content,
                CreatedAt = DateTime.Now,
            };

            db.BlogComments.Add(comment);
            await db.SaveChangesAsync(cancellationToken);
        }

        public class BlogCommentRequest
        {
            public int BlogPostId { get; set; }
            public int? UserId { get; set; } 
            public string Content { get; set; }
        }
    }
}
