using Microsoft.AspNetCore.Identity;
using RS1_2024_25.API.Data.Models;
using System.Threading.Tasks;

namespace RS1_2024_25.API.Services
{
    public class AuthService
    {
        private readonly UserManager<User> _userManager;

        public AuthService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<bool> RegisterUser(string username, string email, string password, string firstname,string lastname)
        {
            Console.WriteLine($"Pokusaj registracije: username={username}, email={email}");

            var existingUserByEmail = await _userManager.FindByEmailAsync(email);
            var existingUserByUsername = await _userManager.FindByNameAsync(username);

            Console.WriteLine($"Postojeci email: {(existingUserByEmail != null)}");
            Console.WriteLine($"Postojeci username: {(existingUserByUsername != null)}");

            if (existingUserByEmail != null || existingUserByUsername != null)
                return false;

            var user = new User
            {
                UserName = username,
                Email = email,
                FirstName=firstname,
                LastName = lastname,
                CreatedAt = DateTime.Now
            };

            var result = await _userManager.CreateAsync(user, password);
            foreach (var error in result.Errors)
            {
                Console.WriteLine($"Greška: {error.Code} - {error.Description}");
            }
            Console.WriteLine($"Kreiranje uspjelo: {result.Succeeded}");

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

    }
}