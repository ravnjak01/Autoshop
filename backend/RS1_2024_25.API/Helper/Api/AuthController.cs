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
        private readonly ILogger<AuthController> _logger;


        public AuthController(AuthService authService, UserManager<User> userManager, SignInManager<User> signInManager,IEmailService emailService,
            ICartService cartService,TokenBlacklistService blacklistService,MyTokenGenerator tokenGenerator, IConfiguration configuration,
              ILogger<AuthController> logger)
        {
            _authService = authService;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _cartService = cartService;
            _tokenGenerator = tokenGenerator;
            _blacklistService = blacklistService;
            _configuration = configuration;
            _logger = logger;
        }


        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingUserByEmail = await _userManager.FindByEmailAsync(model.Email);
            var existingUserByUsername = await _userManager.FindByNameAsync(model.Username);


            if (existingUserByEmail != null || existingUserByUsername != null)
                if (existingUserByEmail != null)
                {
                    return BadRequest(new { message = "Email is already in use." });
                }
            if (existingUserByUsername != null)
            {
                return BadRequest(new { message = "Username is already taken." });
            }

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
                await _userManager.AddToRoleAsync(user, UserRoles.Customer.ToString());
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
                var (token, expiration) = _tokenGenerator.GenerateToken(user, roles);

  

                return Ok(new
                {
                    token = token,
                    username = user.UserName,
                    expiration = expiration,
                });
        }
        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var authHeader = Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
                return Unauthorized(new { message = "Missing or invalid Authorization header" });

            var token = authHeader.Replace("Bearer ", "");
            var handler = new JwtSecurityTokenHandler();
            if (!handler.CanReadToken(token))
                return BadRequest(new { message = "Token is malformed and cannot be read" });

            var jwtToken = handler.ReadJwtToken(token);
            await _blacklistService.BlacklistToken(token, jwtToken.ValidTo);
            return Ok(new { message = "Logged out successfully" });
        }


        [AllowAnonymous]
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordModel model, CancellationToken cancellationToken)
        {
            var genericResponse = new { message = "If an account exists for this email, a reset link has been sent." };

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return Ok(genericResponse);

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var baseUrl = _configuration["Frontend:BaseUrl"];
            var resetLink = $"{baseUrl}/reset-password?token={Uri.EscapeDataString(token)}&email={Uri.EscapeDataString(user.Email)}";

            await _emailService.SendResetPasswordEmail(user.Email, resetLink, cancellationToken);
            return Ok(genericResponse);
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
