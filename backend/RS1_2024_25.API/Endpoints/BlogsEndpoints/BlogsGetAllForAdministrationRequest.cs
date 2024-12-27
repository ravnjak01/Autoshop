using Microsoft.AspNetCore.Mvc;
using RS1_2024_25.API.Data;
using RS1_2024_25.API.Helper.Api;
using RS1_2024_25.API.Helper;
using static RS1_2024_25.API.Endpoints.BlogsEndpoints.GetAllForAdministration;

namespace RS1_2024_25.API.Endpoints.BlogsEndpoints
{
    //sa paging i sa filterom
    [Route("/administration/blogposts")]
    public class GetAllForAdministration(ApplicationDbContext db) : MyEndpointBaseAsync
        .WithRequest<BlogsGetAllForAdministrationRequest>
        .WithResult<MyPagedList<BlogsGetAllForAdministrationResponse>>
    {
        [HttpGet("filter")]
        public override async Task<MyPagedList<BlogsGetAllForAdministrationResponse>> HandleAsync([FromQuery] BlogsGetAllForAdministrationRequest request, CancellationToken cancellationToken = default)
        {
            // Kreiranje osnovnog query-a
            var query = db.BlogPosts
                .AsQueryable();

            // Primjena filtera na osnovu naziva grada
            if (!string.IsNullOrWhiteSpace(request.Q))
            {
                query = query.Where(blog => (!string.IsNullOrEmpty(blog.Author) && blog.Author.Contains(request.Q)) ||
                    blog.Title.Contains(request.Q)
                );
            }

            // Projektovanje u rezultatni tip
            var projectedQuery = query.Select(blog => new BlogsGetAllForAdministrationResponse
            {
                ID = blog.Id,
                Title = blog.Title,
                AuthorName = blog.Author,
                PublishedTime = blog.PublishedDate,
                IsPublished = blog.IsPublished,
            });

            // Kreiranje paginiranog odgovora sa filterom
            var result = await MyPagedList<BlogsGetAllForAdministrationResponse>.CreateAsync(projectedQuery, request, cancellationToken);


            return result;
        }
        public class BlogsGetAllForAdministrationRequest : MyPagedRequest //naslijeđujemo
        {
            public string? Q { get; set; } = string.Empty;
        }

        public class BlogsGetAllForAdministrationResponse
        {
            public required int ID { get; set; }
            public required string Title { get; set; }
            public required string AuthorName { get; set; }
            public DateTime? PublishedTime { get; set; }
            public bool IsPublished { get; set; }
        }
    }
}
