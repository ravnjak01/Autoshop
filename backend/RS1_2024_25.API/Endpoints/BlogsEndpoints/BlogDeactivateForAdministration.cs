using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RS1_2024_25.API.Data;
using RS1_2024_25.API.Data.Models.Modul2_Basic;
using RS1_2024_25.API.Helper.Api;

namespace RS1_2024_25.API.Endpoints.BlogsEndpoints
{
    [Route("deactivate-blog")]
    public class BlogDeactivateForAdministration(ApplicationDbContext db) : MyEndpointBaseAsync
    .WithRequest<int>
    .WithoutResult
    {

        [HttpPost("{id}")]
        public override async Task HandleAsync(int id, CancellationToken cancellationToken = default)
        {
            var blog = await db.BlogPosts.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (blog == null)
                throw new KeyNotFoundException("Blog not found");
            blog.Active = !blog.Active;

            db.Set<BlogPost>().Attach(blog);
            db.Entry(blog).Property("Active").IsModified = true;
            await db.SaveChangesAsync(cancellationToken);
        }
    }
}
