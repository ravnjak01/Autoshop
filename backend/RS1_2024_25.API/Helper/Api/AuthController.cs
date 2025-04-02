using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RS1_2024_25.API.Data.Models;
using RS1_2024_25.API.Data.Models.Modul1_Auth.Services;


namespace RS1_2024_25.API.Helper.Api
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController:ControllerBase
    {
        private readonly AuthService _authService;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AuthController(AuthService authService, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _authService = authService;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        //public AuthController(AuthService authService,UserManager<User>userManager)
        //{
        //    _authService = authService;
        //    _userManager = userManager;
        //}

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var success = await _authService.RegisterUser(model.Username, model.Email, model.Password, model.Fullname);
            if (!success) return BadRequest("Email already in use.");

            return Ok("Registration successful");
        }
        //[HttpPost("login")]
        //public async Task<IActionResult> Login([FromBody] LoginModel model)
        //{
        //    var user = await _userManager.FindByNameAsync(model.Username);

        //    if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
        //    {
        //        return Unauthorized(new { message = "Invalid username or password" });
        //    }

        //    // Ovdje možeš dodati token ili koristiti Cookie-based autentifikaciju
        //    return Ok(new { message = "Login successful" });
        //}
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null) return Unauthorized(new { message = "Invalid username or password" });

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
            if (!result.Succeeded) return Unauthorized(new { message = "Invalid username or password" });

            return Ok(new { message = "Login successful" });
        }
    }
    public class RegisterModel
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Fullname { get; set; } // Added Fullname property
    }
}
