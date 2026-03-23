using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RS1_2024_25.API.Data;
using RS1_2024_25.API.Data.Models.Modul2_Basic;
using RS1_2024_25.API.Helper.Api;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace RS1_2024_25.API.Endpoints.BlogsEndpoints
{
    [Route("publish-blog")]
    [Authorize(Roles = "Admin")]
    public class BlogPublishForAdministration(ApplicationDbContext db) : MyEndpointBaseAsync
    .WithRequest<int>
    .WithoutResult
    {

        [HttpPost("{id}")]
        public override async Task HandleAsync(int id, CancellationToken cancellationToken = default)
        {
            var blog = await db.BlogPosts.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (blog == null)
                throw new KeyNotFoundException("Blog not found");
            blog.IsPublished = !blog.IsPublished;

            if (blog.IsPublished)
            {
                blog.PublishedDate = DateTime.UtcNow;
            }
            else
            {
                blog.PublishedDate = null;
            }

            db.Set<BlogPost>().Attach(blog);
            db.Entry(blog).Property("IsPublished").IsModified = true;
            db.Entry(blog).Property("PublishedDate").IsModified = true;

            await db.SaveChangesAsync(cancellationToken);
        }
    }
}
