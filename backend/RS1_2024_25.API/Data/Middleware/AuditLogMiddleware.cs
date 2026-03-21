using Microsoft.AspNetCore.Identity;
using RS1_2024_25.API.Data.Models;
using RS1_2024_25.API.Data.Models.Modul3_Audit;
using System.Text;
using System.Text.Json;

namespace RS1_2024_25.API.Data.Middleware
{
    public class AuditLogMiddleware
    {
        private const string ControllerKey = "controller";
        private const string IdKey = "id";

        private readonly RequestDelegate _next;

        public AuditLogMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ApplicationDbContext dbContext)
        {
            context.Request.EnableBuffering();

            await _next(context);

            var request = context.Request;

            request.RouteValues.TryGetValue(ControllerKey, out var controllerValue);
            var controllerName = (string)(controllerValue ?? string.Empty);

            string changedValue;

            if (request.Path.StartsWithSegments("/api/auth/login") || request.Path.StartsWithSegments("/api/auth/register") ||
                request.Path.StartsWithSegments("/api/auth/forgot-password") || request.Path.StartsWithSegments("/api/auth/reset-password"))
            {
                changedValue = "[CREDENTIALS REDACTED]";
            }
            else
            {
                changedValue = await GetChangedValues(request).ConfigureAwait(false);
            }

            var userManager = context.RequestServices.GetRequiredService<UserManager<User>>();
            var user = await userManager.GetUserAsync(context.User);
            var email = user?.Email;

            var auditLog = new AuditLog
            {
                UserEmail = email ?? "Guest", 
                EntityName = controllerName,
                Action = request.Method,
                Timestamp = DateTime.UtcNow,
                Changes = changedValue
            };

            dbContext.AuditLogs.Add(auditLog);
            await dbContext.SaveChangesAsync();

        }

        private static async Task<string> GetChangedValues(HttpRequest request)
        {
            var changedValue = string.Empty;

            switch (request.Method)
            {
                case "POST":
                case "PUT":
                    changedValue = await ReadRequestBody(request, Encoding.UTF8).ConfigureAwait(false);
                    break;

                case "DELETE":
                    request.RouteValues.TryGetValue(IdKey, out var idValueObj);
                    changedValue = (string?)idValueObj ?? string.Empty;
                    break;

                default:
                    break;
            }

            return changedValue;
        }

        private static async Task<string> ReadRequestBody(HttpRequest request, Encoding? encoding = null)
        {
            var originalPosition = request.Body.Position;

            // Enable buffering so that the body can be read multiple times
            request.Body.Position = 0;

            var reader = new StreamReader(request.Body, encoding ?? Encoding.UTF8);
            var requestBody = await reader.ReadToEndAsync().ConfigureAwait(false);

            // Reset the body stream position back to the original value
            request.Body.Position = originalPosition;
            if (string.IsNullOrEmpty(requestBody))
            {
                request.RouteValues.TryGetValue("id", out var id);
                requestBody = (string)(id ?? string.Empty);
            }
            return requestBody;
        }

        public class Item
        {
            public int Id { get; set; }
        }
    }
}

