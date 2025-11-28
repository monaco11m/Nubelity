using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nubelity.Application.Interfaces;

namespace Nubelity.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromQuery] string username, [FromQuery] string password)
        {
            var token = await _authService.LoginAsync(username, password);
            return Ok(new { token });
        }
    }
}
