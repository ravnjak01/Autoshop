using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RS1_2024_25.API.Data;
using RS1_2024_25.API.Data.Models;
using RS1_2024_25.API.Data.Models.Modul2_Basic;
using RS1_2024_25.API.Helper.Api;
using RS1_2024_25.API.Services;
using static RS1_2024_25.API.Endpoints.BlogsEndpoints.BlogCommentAdd;

namespace RS1_2024_25.API.Endpoints.BlogsEndpoints
{
    [Route("blog-comment")]
    public class BlogCommentAdd(ApplicationDbContext db, UserManager<User> userManager) : MyEndpointBaseAsync
        .WithRequest<BlogCommentRequest>
        .WithoutResult
    {
        [HttpPost]
        public override async Task HandleAsync([FromForm] BlogCommentRequest request, CancellationToken cancellationToken = default)
        {
            var blog = await db.BlogPosts.SingleOrDefaultAsync(x => x.Id == request.BlogPostId, cancellationToken);

            var userId = userManager.GetUserId(User);

            if (blog == null)
            {
                Response.StatusCode = 400;
                await Response.WriteAsync("There is no blog with this id.");
                return;
            }

            var comment = new BlogComment
            {
                BlogPostId = request.BlogPostId,
                Content = request.Content,
                CreatedAt = DateTime.Now,
                UserId = userId
            };

            db.BlogComments.Add(comment);
            await db.SaveChangesAsync(cancellationToken);
        }

        public class BlogCommentRequest
        {
            public int BlogPostId { get; set; }
            public string Content { get; set; }
        }
    }
}
