using Microsoft.AspNetCore.Mvc;

namespace RS1_2024_25.API.Data.Middleware
{
    public class GlobalExceptionHandlerMiddleware(RequestDelegate next,ILogger<GlobalExceptionHandlerMiddleware>logger)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (KeyNotFoundException ex)
            {
                logger.LogWarning(ex, "Resource not found");
                await WriteProblemDetails(context, StatusCodes.Status404NotFound, "Not Found", ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                logger.LogWarning(ex, "Unauthorized access");
                await WriteProblemDetails(context, StatusCodes.Status401Unauthorized, "Unauthorized", ex.Message);
            }
            catch (ArgumentException ex)
            {
                logger.LogWarning(ex, "Invalid argument");
                await WriteProblemDetails(context, StatusCodes.Status400BadRequest, "Bad Request", ex.Message);
            }

            catch (Exception ex)
            {
                logger.LogError(ex, "Unhandled exception");
                await WriteProblemDetails(context, StatusCodes.Status500InternalServerError, "Server Error", "An unexpected error occurred.");
            }

           
        }

        private static async Task WriteProblemDetails(HttpContext context, int statusCode, string title, string detail)
        {
            var problem = new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Detail = detail,
                Instance = context.Request.Path
            };

            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/problem+json";

            await context.Response.WriteAsJsonAsync(problem);
        }
    }
}
