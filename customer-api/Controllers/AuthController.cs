using customer_api.Models;
using customer_api.Services;
using Microsoft.AspNetCore.Mvc;

namespace customer_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserInsertDto user)
        {
            try
            {
                var registeredUser = await _userService.Register(user);
                return Ok(registeredUser);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserInsertDto user)
        {
            try
            {
                var token = await _userService.Login(user);
                return Ok(new { Token = token });
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
