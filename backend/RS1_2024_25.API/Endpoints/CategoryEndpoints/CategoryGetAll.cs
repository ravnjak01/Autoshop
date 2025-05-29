using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RS1_2024_25.API.Data;
using RS1_2024_25.API.Data.Models.Modul2_Basic;
using RS1_2024_25.API.Helper.Api;

namespace RS1_2024_25.API.Endpoints.CategoryEndpoints
{
    [Route("category")]
    public class CategoryGetAll(ApplicationDbContext db) : MyEndpointBaseAsync
        .WithoutRequest
        .WithResult<CategoryGetAllResponse>
    {
        [HttpGet]
        public override async Task<CategoryGetAllResponse> HandleAsync(CancellationToken cancellationToken = default)
        {
            var query = db.Categories.AsQueryable();
            var categories = await query.ToListAsync(cancellationToken);
            return new CategoryGetAllResponse()
            {
                Categories = categories ?? new List<Category>()
            };
        }
    }

    public class CategoryGetAllResponse
    {
        public List<Category> Categories { get; set; } 
    }
}
