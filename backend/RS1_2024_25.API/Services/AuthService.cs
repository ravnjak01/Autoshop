using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace RS1_2024_25.API.Data.Models.Modul1_Auth.Services
{
    public class AuthService
    {
        private readonly UserManager<User> _userManager;

        public AuthService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        //public async Task<bool> RegisterUser(string username, string email, string password, string fullname)
        //{
        //    var existingUserByEmail = await _userManager.FindByEmailAsync(email);
        //    if (existingUserByEmail != null) return false;

        //    var existingUserByUsername = await _userManager.FindByNameAsync(username);
        //    if (existingUserByUsername != null) return false; // Dodaj provjeru i za korisničko ime

        //    var user = new User
        //    {
        //        UserName = username,
        //        Email = email,
        //        FullName = fullname,
        //        CreatedAt = DateTime.Now
        //    };

        //    var result = await _userManager.CreateAsync(user, password);
        //    return result.Succeeded;
        //}
        public async Task<bool> RegisterUser(string username, string email, string password, string fullname)
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
                FullName = fullname,
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

    }
}