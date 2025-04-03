using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniProject_API.Models.DTO;
using MiniProject_API.Services.IServices;

namespace MiniProject_API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterationRequestDTO registerationRequestDTO)
        {
            var response = await _authService.Register(registerationRequestDTO);
            return StatusCode((int)response.StatusCode, response);
        }
        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string userId, [FromQuery] string code)
        {
            var response = await _authService.ConfirmEmail(userId, code);
            return StatusCode((int)response.StatusCode, response);
        }
    }
}
