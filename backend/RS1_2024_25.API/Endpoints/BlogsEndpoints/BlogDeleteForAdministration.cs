using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RS1_2024_25.API.Data;
using RS1_2024_25.API.Helper.Api;

namespace RS1_2024_25.API.Endpoints.BlogsEndpoints
{
    [Route("delete-blog")]
    [Authorize(Roles = "Admin")]
    public class BlogDeleteForAdministration(ApplicationDbContext db) : MyEndpointBaseAsync
    .WithRequest<int>
    .WithoutResult
    {

        [HttpDelete("{id}")]
        public override async Task HandleAsync(int id, CancellationToken cancellationToken = default)
        {
            var blog = await db.BlogPosts
                .Include(b => b.Comments)
                .Include(b => b.Ratings)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (blog == null)
                throw new KeyNotFoundException("Blog not found");

            db.Remove(blog);
            await db.SaveChangesAsync();
        }
    }
}
