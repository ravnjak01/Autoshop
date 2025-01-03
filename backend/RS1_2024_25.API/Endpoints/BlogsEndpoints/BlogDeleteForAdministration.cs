using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RS1_2024_25.API.Data;
using RS1_2024_25.API.Data.Models.Modul2_Basic;
using RS1_2024_25.API.Helper.Api;

namespace RS1_2024_25.API.Endpoints.BlogsEndpoints
{
    [Route("delete-blog")]
    public class BlogDeleteForAdministration(ApplicationDbContext db) : MyEndpointBaseAsync
    .WithRequest<int>
    .WithoutResult
    {

        [HttpDelete("{id}")]
        public override async Task HandleAsync(int id, CancellationToken cancellationToken = default)
        {
            var blog = await db.BlogPosts.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (blog == null)
                throw new KeyNotFoundException("Blog not found");

            db.Remove(blog);
            await db.SaveChangesAsync(cancellationToken);
        }
    }
}
