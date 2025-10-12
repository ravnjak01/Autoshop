using Microsoft.AspNetCore.Identity;
using RS1_2024_25.API.Data.Models;
using RS1_2024_25.API.Data.Models.Modul1_Auth;
using System.Security.Claims;

namespace RS1_2024_25.API.Services
{
    public class MyAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MyAuthService(
            UserManager<User> userManager,
            IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

       
        public MyAuthInfo GetCurrentAuthInfo()
        {
            var user = _httpContextAccessor.HttpContext?.User;

            if (user == null || !user.Identity?.IsAuthenticated == true)
            {
                return new MyAuthInfo { IsLoggedIn = false };
            }

            return new MyAuthInfo
            {
                UserId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                Email = user.FindFirst(ClaimTypes.Email)?.Value,
                FirstName = user.FindFirst(ClaimTypes.GivenName)?.Value,
                LastName = user.FindFirst(ClaimTypes.Surname)?.Value,
                IsAdmin = user.IsInRole("Admin"),
                IsCustomer = user.IsInRole("Customer"),
                IsManager = user.IsInRole("Manager"),
                IsLoggedIn = true,
                
            };
        }

        public async Task<User?> GetCurrentUser()
        {
            var userId = _httpContextAccessor.HttpContext?.User
                .FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return null;

            return await _userManager.FindByIdAsync(userId);
        }

        public bool IsInRole(string role)
        {
            return _httpContextAccessor.HttpContext?.User.IsInRole(role) ?? false;
        }

    
        public bool IsAuthenticated()
        {
            return _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
        }
    }

   
}