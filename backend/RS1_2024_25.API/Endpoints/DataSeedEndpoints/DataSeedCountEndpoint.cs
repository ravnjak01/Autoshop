namespace RS1_2024_25.API.Endpoints.DataSeed
{
    using Microsoft.AspNetCore.Mvc;
    using RS1_2024_25.API.Data;
    using RS1_2024_25.API.Helper.Api;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    namespace FIT_Api_Example.Endpoints
    {
        [Route("data-seed")]
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
                     { "BlogPosts", db.BlogPosts.Count() },
                     { "BlogComments", db.BlogComments.Count() },
                     { "BlogRating", db.BlogRatings.Count() },
                     { "Products", db.Products.Count() },
                     { "Categories", db.Categories.Count() },
                     { "Discounts", db.Discounts.Count() },
                     { "DiscountProducts", db.DiscountProducts.Count() },
                     { "DiscountCodes", db.DiscountCodes.Count() },
                     { "DiscountCategories", db.DiscountCategories.Count() },

                };

                return dataCounts;
            }
        }
    }

}
