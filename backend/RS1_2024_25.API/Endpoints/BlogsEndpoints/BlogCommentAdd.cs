using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    [Route("blog-comment")]
    public class BlogCommentAdd(ApplicationDbContext db, UserManager<User> userManager) : MyEndpointBaseAsync
        .WithRequest<BlogCommentRequest>
        .WithoutResult
    {
        [HttpPost]
        public override async Task HandleAsync([FromForm] BlogCommentRequest request, CancellationToken cancellationToken = default)
        {
            if (request.BlogPostId <= 0)
                throw new ArgumentException("Invalid blog post id.");

            if (string.IsNullOrWhiteSpace(request.Content))
                throw new ArgumentException("Comment content is required.");

            if (request.Content.Length > 1000)
                throw new ArgumentException("Comment is too long (max 1000 characters).");

            var blogExists = await db.BlogPosts
                .AnyAsync(x => x.Id == request.BlogPostId, cancellationToken);

            if (!blogExists)
                throw new KeyNotFoundException($"Blog post with ID {request.BlogPostId} was not found.");

            var userId = userManager.GetUserId(User);

            if (string.IsNullOrEmpty(userId))
                throw new UnauthorizedAccessException("You must be logged in to post a comment.");

            // 4. Sanitacija sadržaja
            var sanitizedContent = System.Net.WebUtility.HtmlEncode(request.Content.Trim());

            // 5. Kreiranje i spašavanje
            var comment = new BlogComment
            {
                BlogPostId = request.BlogPostId,
                Content = sanitizedContent,
                CreatedAt = DateTime.UtcNow,
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