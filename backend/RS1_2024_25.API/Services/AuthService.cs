using Microsoft.AspNetCore.Identity;

namespace RS1_2024_25.API.Data.Models.Modul1_Auth.Services
{
    public class AuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;

        public AuthService(ApplicationDbContext context, IPasswordHasher<User> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        public async Task<bool> RegisterUser(string username, string email, string password)
        {
            if (_context.Users.Any(u => u.Email == email)) return false;

            var user = new User { Username = username, Email = email };
            user.PasswordHash = _passwordHasher.HashPassword(user, password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
