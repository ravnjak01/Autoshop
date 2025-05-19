using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RS1_2024_25.API.Data.Models;
using RS1_2024_25.API.Data.Models.Modul1_Auth.Services;
using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace RS1_2024_25.API.Helper.Api
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
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

        [AllowAnonymous]
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                foreach (var modelState in ModelState)
                {
                    Console.WriteLine($"Key: {modelState.Key}");
                    foreach (var error in modelState.Value.Errors)
                    {
                        Console.WriteLine($"  Error: {error.ErrorMessage}");
                    }
                }
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var success = await _authService.RegisterUser(model.Username, model.Email, model.Password, model.Fullname);
            if (!success) return BadRequest( new { message= "Email or Username already in use." });

            return Ok(new { message = "Registration successful." });
        }


        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            try
            {

                var user = await _userManager.FindByNameAsync(model.Username);
                if (user == null)
                {
                    Console.WriteLine("User not found");
                    return Unauthorized(new { message = "Invalid username or password" });
                }

                var result = await _signInManager.PasswordSignInAsync(user, model.Password, isPersistent: false, lockoutOnFailure: false);
                Console.WriteLine($"Login attempt for {model.Username}, success: {result.Succeeded}");
                if (!result.Succeeded) return Unauthorized(new { message = "Invalid username or password" });

                await _signInManager.SignInAsync(user, isPersistent: false);
                return Ok(new { message = "Login successful" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during login: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok(new { message = "Logged out" });
        }
        [Authorize]
        [HttpGet("check-auth")]
        public IActionResult CheckAuth()
        {
           // if (User.Identity != null && User.Identity.IsAuthenticated)
                return Ok(new { user = User.Identity.Name });

            //return Unauthorized();
        }

        [AllowAnonymous]
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // Ne otkrivaj da korisnik ne postoji (security reason)
                return Ok(new { message = "If an account with that email exists, a reset link has been sent." });
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            // Ovdje pošalji email sa linkom za reset (ili samo pošalji token ako testiraš)
            // Za sada ćemo samo vratiti token u odgovoru (testni scenarij)
            return Ok(new
            {
                message = "Reset token generated.",
                resetToken = token,
                email = user.Email
            });
        }

        [AllowAnonymous]
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return BadRequest(new { message = "Invalid request." });
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);
            if (!result.Succeeded)
            {
                return BadRequest(new { message = "Reset failed", errors = result.Errors });
            }

            return Ok(new { message = "Password reset successful." });
        }


        public class RegisterModel
        {
            [Required]
            [JsonPropertyName("username")]
            public string Username { get; set; }

            [Required]
            [EmailAddress]
            [JsonPropertyName("email")]
            public string Email { get; set; }

            [Required]
            [MinLength(6)]
            [JsonPropertyName("password")]
            public string Password { get; set; }

            [Required]
            [JsonPropertyName("fullname")]
            public string Fullname { get; set; }
            

        }
    }
}
