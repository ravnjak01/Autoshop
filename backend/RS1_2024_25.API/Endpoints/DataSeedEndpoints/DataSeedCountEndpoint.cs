namespace RS1_2024_25.API.Endpoints.DataSeed
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using RS1_2024_25.API.Data;
    using RS1_2024_25.API.Helper.Api;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    namespace FIT_Api_Example.Endpoints
    {
        [Route("data-seed")]
        [Authorize(Roles = "Admin")]
        public class DataSeedCountEndpoint(ApplicationDbContext db)
            : MyEndpointBaseAsync
            .WithoutRequest
            .WithResult<Dictionary<string, int>>
        {
            [HttpGet]
            public override async Task<Dictionary<string, int>> HandleAsync(CancellationToken cancellationToken = default)
            {
                Dictionary<string, int> dataCounts = new ()
                {
                    { "BlogPosts", await db.BlogPosts.CountAsync(cancellationToken) },
                    { "BlogComments", await db.BlogComments.CountAsync(cancellationToken) },
                    { "BlogRatings", await db.BlogRatings.CountAsync(cancellationToken) },
                    { "Products", await db.Products.CountAsync(cancellationToken) },
                    { "Categories", await db.Categories.CountAsync(cancellationToken) },
                    { "Discounts", await db.Discounts.CountAsync(cancellationToken) },
                    { "DiscountProducts", await db.DiscountProducts.CountAsync(cancellationToken) },
                    { "DiscountCodes", await db.DiscountCodes.CountAsync(cancellationToken) },
                    { "DiscountCategories", await db.DiscountCategories.CountAsync(cancellationToken) }
                };

                return dataCounts;
            }
        }
    }

}
