using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

public class SwaggerDevelopmentFilter(IWebHostEnvironment env) : IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        if (!env.IsDevelopment())
        {
            swaggerDoc.Paths.Remove("/data-seed");
        }
    }
}