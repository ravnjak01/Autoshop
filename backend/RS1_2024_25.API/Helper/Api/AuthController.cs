using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RS1_2024_25.API.Data.DTOs;
using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using RS1_2024_25.API.Services;
using RS1_2024_25.API.Data.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using RS1_2024_25.API.Controllers;

namespace RS1_2024_25.API.Helper.Api
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _config;
        private readonly IEmailService _emailService;
   private readonly ICartService _cartService;
        public AuthController(AuthService authService, UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration  config,IEmailService emailService,
            ICartService cartService)
        {
            _authService = authService;
            _userManager = userManager;
            _signInManager = signInManager;
            _config  = config;
            _emailService = emailService;
            _cartService = cartService;
        }

        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = new User
            {
                UserName = model.Username,
                Email = model.Email,
                FirstName = model.Firstname, 
                LastName = model.Lastname
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                
                int strengthScore = _authService.GetPasswordStrengthScore(model.Password);

                string feedbackMessage = strengthScore switch
                {
                    4 => "Great password! Your account is fully secured.",
                    3 => "Password is accepted, but you can upgrade with a special character.",
                    _ => "Password is accepted,but we recommend you to make a stronger one in the future"
                };

                
                return Ok(new
                {
                    Message = "Registration  successful!",
                    PasswordStrength = strengthScore,
                    Advice = feedbackMessage
                });
            }

            return BadRequest(result.Errors);
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

                var result = await _signInManager.PasswordSignInAsync(user, model.Password,
                    isPersistent: false, lockoutOnFailure: false);

                Console.WriteLine($"Login attempt for {model.Username}, success: {result.Succeeded}");

                if (!result.Succeeded)
                    return Unauthorized(new { message = "Invalid username or password" });

                var guestSessionId = Request.Cookies["guest_session"];
                Console.WriteLine($"=== LOGIN DEBUG ===");
                Console.WriteLine($"Guest session ID from cookie: {guestSessionId}");
                Console.WriteLine($"User ID: {user.Id}");
                if (!string.IsNullOrEmpty(guestSessionId))
                {
                    Console.WriteLine($"Attempting to merge cart...");
                    await _cartService.MergeGuestCartWithUser(user.Id, guestSessionId);
                    Console.WriteLine($"Merge completed");
                }

                var roles = await _userManager.GetRolesAsync(user);
                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.NameIdentifier, user.Id)
        };

                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    issuer: _config["Jwt:Issuer"],
                    audience: _config["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddHours(3),
                    signingCredentials: creds
                );
       

                var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
                var expiration = token.ValidTo; 
                var expUnix = new DateTimeOffset(expiration).ToUnixTimeSeconds();

                return Ok(new
                {
                    token = tokenString,
                    username = user.UserName,
                    roles = roles,
                    expires=expiration,
                    exp=expUnix
                });

         


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
     

        [AllowAnonymous]
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                
                return Ok(new { message = "if account exists,a link has been sent" });
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetLink = $"http://localhost:4200/reset-password?token={Uri.EscapeDataString(token)}&email={Uri.EscapeDataString(user.Email)}";

            try
            {

           await _emailService.SendResetPasswordEmail(user.Email, resetLink);
        return Ok(new
        {
            message = "Reset token generated and email sent.",
            resetToken = token,
            email = user.Email
        });
            }
            catch(Exception ex)
            {
                return StatusCode(500, $"Error during sending email-a: {ex.Message}");
            }

        }

        [AllowAnonymous]
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return BadRequest(new { message = "invalid request." });
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);
            if (!result.Succeeded)
            {
                return BadRequest(new { message = "Reset unsuccessfull", errors = result.Errors });
            }

            return Ok(new { message = "Reset of password successfull" });
        }

     
     
    }
}
