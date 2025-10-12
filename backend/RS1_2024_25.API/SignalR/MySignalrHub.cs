using Microsoft.AspNetCore.SignalR;
using RS1_2024_25.API.Services;
using System.Security.Claims;

namespace RS1_2024_25.API.SignalRHubs
{
    public class MySignalrHub : Hub
    {
        private string? GetUserEmail()
        {
            return Context.User?.FindFirst(ClaimTypes.Email)?.Value;
        }
        private string? GetUserId()
        {
            return Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

        public override async Task OnConnectedAsync()
        {
            if (Context.User?.Identity?.IsAuthenticated != true)
            {
                throw new HubException("Unauthorized: User not authenticated.");
            }

            var email = GetUserEmail();
            if (string.IsNullOrEmpty(email))
            {
                throw new HubException("Unauthorized: Email claim missing.");
            }

            // Dodavanje korisnika u grupu na osnovu emaila
            await Groups.AddToGroupAsync(Context.ConnectionId, $"user_{email}");

            Console.WriteLine($"User {email} connected with ConnectionId {Context.ConnectionId}");

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var email = GetUserEmail();

            if (!string.IsNullOrEmpty(email))
            {
                // Uklanjanje korisnika iz grupe
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"user_{email}");
                Console.WriteLine($"User {email} disconnected from ConnectionId {Context.ConnectionId}");
            }

            await base.OnDisconnectedAsync(exception);
        }

        // Metoda za slanje poruke korisniku
        public async Task MyServerHubMethod1(string toUserEmail, string message)
        {
                // Korisnik je već autentifikovan kroz [Authorize] atribut
            var senderEmail = GetUserEmail();

            if (string.IsNullOrEmpty(senderEmail))
                throw new HubException("Unauthorized: Sender email missing.");

            // Slanje poruke korisniku toUserEmail
            await Clients.Group($"user_{toUserEmail}")
                         .SendAsync("myClientMethod1", new
                         {
                             from = senderEmail,
                             message = message,
                             timestamp = DateTime.UtcNow
                         });
        }
        // Opciono: Metoda za provjeru da li je korisnik u određenoj ulozi
        public async Task SendToAdmins(string message)
        {
            if (!Context.User.IsInRole("Admin"))
                throw new HubException("Forbidden: Only admins can use this method.");

            await Clients.Group("Admins").SendAsync("adminMessage", message);
        }
    }
}
