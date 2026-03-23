using Microsoft.AspNetCore.Identity;
using RS1_2024_25.API.Data.Models;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
namespace RS1_2024_25.API.Services
{
    public class AuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(UserManager<User> userManager,IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> RegisterUser(string username, string email, string password, string firstname,string lastname)
        {

            if (!IsValidEmail(email))
            {
                return false; 
            }


            var existingUserByEmail = await _userManager.FindByEmailAsync(email);
            var existingUserByUsername = await _userManager.FindByNameAsync(username);


            if (existingUserByEmail != null || existingUserByUsername != null)
                return false;

            var user = new User
            {
                UserName = username,
                Email = email,
                FirstName=firstname,
                LastName = lastname,
                CreatedAt = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(user, password);
            foreach (var error in result.Errors)
            {
            }
   
            return result.Succeeded;
        }

        public int GetPasswordStrengthScore(string password)
        {
            if (string.IsNullOrWhiteSpace(password)) return 0;

            int score = 0;
            if (password.Length >= 8) score++;      
            if (password.Any(char.IsUpper)) score++; 
            if (password.Any(char.IsDigit)) score++; 
            if (password.Any(ch => !char.IsLetterOrDigit(ch))) score++; 

            return score; 
        }


        public bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;

            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public MyAuthInfo GetCurrentAuthInfo()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (user == null || !user.Identity?.IsAuthenticated == true)
                return new MyAuthInfo { IsLoggedIn = false };

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