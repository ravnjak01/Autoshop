using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RS1_2024_25.API.Helper.Api
{
    
        [ApiController]
        [Route("api/user")]
        [Authorize]
        public class UserInfoController : ControllerBase
        {
            [HttpGet("me")]
            public IActionResult GetCurrentUser()
            {
                var username = User.Identity?.Name;
                return Ok(new { Username = username });
            }
        }
    
}
