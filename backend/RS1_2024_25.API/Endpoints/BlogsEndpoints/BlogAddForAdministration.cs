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
        .WithActionResult<BlogPostUpdateOrInsertResponse>
    {
        [HttpPost]
        public override async Task<ActionResult<BlogPostUpdateOrInsertResponse>> HandleAsync([FromForm] BlogPostUpdateOrInsertRequest request, CancellationToken cancellationToken = default)
        {
            byte[]? image = null;
            if (request.Image != null)
            {
                using var memoryStream = new MemoryStream();
                await request.Image.CopyToAsync(memoryStream);
                image = memoryStream.ToArray();

                // Sačuvaj sliku u bazu ili na server
            }
            // Kreiranje ili ažuriranje blog posta
            var blogPost = new BlogPost
            {
                Title = request.Title,
                Content = request.Content,
                Author = request.Author,
                IsPublished = request.IsPublished,
                Image = image,
                Active = request.Active
            };

            // Dodavanje ili ažuriranje u bazi podataka
            db.BlogPosts.Add(blogPost);
            await db.SaveChangesAsync(cancellationToken);

            // Kreiranje odgovora
            var response = new BlogPostUpdateOrInsertResponse
            {
                ID = blogPost.Id,
                Title = blogPost.Title,
                Content = blogPost.Content,
                AuthorName = blogPost.Author,
                PublishedTime = blogPost.PublishedDate,
                IsPublished = blogPost.IsPublished, 
                Active = blogPost.Active
            };

            return response;
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


