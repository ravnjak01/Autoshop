using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RS1_2024_25.API.Data.Models;

namespace RS1_2024_25.API.Helper.Api
{

    [ApiController]
    [Route("api/user")]

    public class UserInfoController : ControllerBase
    {
        private readonly UserManager<User> _userManager;


        public UserInfoController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }
        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentUser(CancellationToken cancellationToken)
        {
            
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized(new { message = "User not found in database." });

          
            var roles = await _userManager.GetRolesAsync(user);

            
            return Ok(new UserInfoDto
            {
                Id = user.Id,
                Username = user.UserName,
                Email = user.Email,
                Roles = roles.ToList()
            });
        }
    }

    public class UserInfoDto
    {
        public string Id { get; set; } = string.Empty;
        public string? Username { get; set; }
        public string? Email { get; set; }
        public List<string> Roles { get; set; } = new();
    }
}
