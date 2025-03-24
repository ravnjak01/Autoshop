using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RS1_2024_25.API.Data;
using RS1_2024_25.API.Data.Models.Modul2_Basic;
using RS1_2024_25.API.Helper.Api;
using RS1_2024_25.API.Services;
using static RS1_2024_25.API.Endpoints.BlogsEndpoints.BlogAddForAdministration;

namespace RS1_2024_25.API.Endpoints.BlogsEndpoints
{

    [Route("blog-post")]
    public class BlogAddForAdministration(ApplicationDbContext db, MyAuthService myAuthService): MyEndpointBaseAsync
        .WithRequest<BlogPostUpdateOrInsertRequest>
        .WithoutResult
    {
        [HttpPost]
        public override async Task HandleAsync([FromForm] BlogPostUpdateOrInsertRequest request, CancellationToken cancellationToken = default)
        {

            var blog = await db.BlogPosts.SingleOrDefaultAsync(x => x.Id == request.ID, cancellationToken);

            byte[]? image = null;
            if (request.Image != null)
            {
                using var memoryStream = new MemoryStream();
                await request.Image.CopyToAsync(memoryStream);
                image = memoryStream.ToArray();

            }
            // Kreiranje ili ažuriranje blog posta
            if (blog == null)
            {
                blog = new BlogPost
                {
                    Title = request.Title,
                    Content = request.Content,
                    Author = request.Author,
                    IsPublished = request.IsPublished,
                    PublishedDate = request.IsPublished ? DateTime.Now : null,
                    Image = image,
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
                    blog.PublishedDate = DateTime.Now;
                }
                blog.Title = request.Title;
                blog.Content = request.Content;
                blog.Author = request.Author;
                blog.IsPublished = request.IsPublished;
                blog.Image = image ?? blog.Image; 
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
            //public string? Image { get; set;
            public IFormFile? Image { get; set; }
            public required string Author { get; set; }//promijeniti u id
            public required bool IsPublished { get; set; }
            public required bool Active { get; set; }
        }

        public class BlogPostUpdateOrInsertResponse
        {
            public required int ID { get; set; }
            public required string Title { get; set; }
            public required string Content { get; set; }
            public required string AuthorName { get; set; }
            public DateTime? PublishedTime { get; set; }
            public bool IsPublished { get; set; }
            public bool Active { get; set; }
        }
    }
}


