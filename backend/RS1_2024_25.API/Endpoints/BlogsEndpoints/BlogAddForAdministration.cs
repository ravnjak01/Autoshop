using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RS1_2024_25.API.Data;
using RS1_2024_25.API.Data.Models;
using RS1_2024_25.API.Data.Models.Modul2_Basic;
using RS1_2024_25.API.Helper.Api;
using static RS1_2024_25.API.Endpoints.BlogsEndpoints.BlogAddForAdministration;

namespace RS1_2024_25.API.Endpoints.BlogsEndpoints
{
    [Authorize(Roles = "Admin")]
    [Route("blog-post")]
    public class BlogAddForAdministration(ApplicationDbContext db, UserManager<User> userManager) : MyEndpointBaseAsync
        .WithRequest<BlogPostUpdateOrInsertRequest>
        .WithoutResult
    {
        [HttpPost]
        public override async Task HandleAsync([FromForm] BlogPostUpdateOrInsertRequest request, CancellationToken cancellationToken = default)
        {

            var blog = await db.BlogPosts.SingleOrDefaultAsync(x => x.Id == request.ID, cancellationToken);

            var userId = userManager.GetUserId(User);

            string? imagePath = blog?.ImagePath;

            if (request.Image != null)
            {
                const long maxFileSize = 5 * 1024 * 1024; // 5 MB

                if (request.Image.Length > maxFileSize)
                    throw new ArgumentException("The file is too large. Maximum allowed size is 5 MB.");

                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "blogs");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var fileName = $"{Guid.NewGuid()}_{request.Image.FileName}";
                var filePath = Path.Combine(uploadsFolder, fileName);

                await using var stream = new FileStream(filePath, FileMode.Create);
                await request.Image.CopyToAsync(stream);

                imagePath = $"/blogs/{fileName}";
            }

            if (string.IsNullOrWhiteSpace(request.Title) || string.IsNullOrWhiteSpace(request.Content))
            {
                throw new ArgumentException("Title or Content is null");
            }

            if (blog == null)
            {
                blog = new BlogPost
                {
                    Title = request.Title,
                    Content = request.Content,
                    AuthorId = userId,
                    IsPublished = request.IsPublished,
                    PublishedDate = request.IsPublished ? DateTime.UtcNow : null,
                    ImagePath = imagePath,
                    Active = request.Active
                };
                db.BlogPosts.Add(blog);
            }
            else
            {
                if(blog.IsPublished && !request.IsPublished)
                {
                    blog.PublishedDate = null;
                }
                else if(!blog.IsPublished && request.IsPublished)
                {
                    blog.PublishedDate = DateTime.UtcNow;
                }
                blog.Title = request.Title;
                blog.Content = request.Content;
                blog.AuthorId = userId;
                blog.IsPublished = request.IsPublished;
                blog.ImagePath = imagePath; 
                blog.Active = request.Active;
                db.BlogPosts.Update(blog); 
            }
            await db.SaveChangesAsync(cancellationToken);
        }

        public class BlogPostUpdateOrInsertRequest
        {
            public int? ID { get; set; } // Nullable to allow null for insert operations
            public required string Title { get; set; }
            public required string Content { get; set; }
            public IFormFile? Image { get; set; }
            public required bool IsPublished { get; set; }
            public required bool Active { get; set; }
        }

        public class BlogPostUpdateOrInsertResponse
        {
            public required int ID { get; set; }
            public required string Title { get; set; }
            public required string Content { get; set; }
            public DateTime? PublishedTime { get; set; }
            public bool IsPublished { get; set; }
            public bool Active { get; set; }
        }
    }
}


