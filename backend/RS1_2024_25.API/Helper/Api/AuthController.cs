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
        private readonly IEmailService _emailService;
   private readonly ICartService _cartService;
        private readonly TokenBlacklistService _blacklistService;
        private readonly MyTokenGenerator _tokenGenerator;
        private readonly IConfiguration _configuration;

        public AuthController(AuthService authService, UserManager<User> userManager, SignInManager<User> signInManager,IEmailService emailService,
            ICartService cartService,TokenBlacklistService blacklistService,MyTokenGenerator tokenGenerator, IConfiguration configuration)
        {
            _authService = authService;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _cartService = cartService;
            _tokenGenerator = tokenGenerator;
            _blacklistService = blacklistService;
            _configuration = configuration;
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
        public async Task<IActionResult> Login([FromBody] LoginModel model,CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(model.Username);
                if (user == null)
                {
                    return Unauthorized(new { message = "Invalid username or password" });
                }

                var result = await _signInManager.PasswordSignInAsync(user, model.Password,
                    isPersistent: false, lockoutOnFailure: false);


                if (!result.Succeeded)
                    return Unauthorized(new { message = "Invalid username or password" });

                var guestSessionId = Request.Cookies["guest_session"];
                if (!string.IsNullOrEmpty(guestSessionId))
                {
                    await _cartService.MergeGuestCartWithUser(user.Id, guestSessionId,cancellationToken);
                }

                var roles = await _userManager.GetRolesAsync(user);
                
                var tokenString = _tokenGenerator.GenerateToken(user, roles);
                var expUnix = DateTimeOffset.UtcNow.AddHours(3).ToUnixTimeSeconds();

                return Ok(new
                {
                    token = tokenString,
                    username = user.UserName,
                    roles = roles,
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
            var token = Request.Headers["Authorization"]
                     .ToString()
                        .Replace("Bearer ", "");

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            _blacklistService.BlacklistToken(token, jwtToken.ValidTo);

            return Ok(new { message = "Logged out successfully" });
        }
     

        [AllowAnonymous]
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordModel model,CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return Ok(new { message = "if account exists,a link has been sent" });
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var baseUrl = _configuration["Frontend:BaseUrl"];

            var resetLink = $"{baseUrl}/reset-password?token={Uri.EscapeDataString(token)}&email={Uri.EscapeDataString(user.Email)}";

            try
            {

                await _emailService.SendResetPasswordEmail(user.Email, resetLink,cancellationToken);
                return Ok(new
                {
                    message = "Reset token generated and email sent."
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
                return Ok(new { message = "If account exists, password has been reset." });

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);
            if (!result.Succeeded)
            {
                return BadRequest(new { message = "Reset unsuccessfull", errors = result.Errors });
            }

            return Ok(new { message = "Reset of password successfull" });
        }

     
     
    }
}
