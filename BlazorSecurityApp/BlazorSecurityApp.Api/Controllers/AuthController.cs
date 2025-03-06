using BlazorSecurityApp.BusinessLayer.Services.Interfaces;
using BlazorSecurityApp.Core.Common;
using BlazorSecurityApp.Core.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlazorSecurityApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IConfiguration _configuration;

        public AuthController(IAuthService authService, IConfiguration configuration)
        {
            _authService = authService;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequestDTO loginDto)
        {
            Result<LoginResponseDTO>? result = _authService.Login(loginDto);

            if (!result.Success)
            {
                return StatusCode((int)result.StatusCode, result);
            }
            return Ok(result);
        }

        [HttpPost("refresh")]
        public IActionResult Refresh([FromBody] RefreshUserTokenRequestDTO requestDto)
        {
            Result<RefreshUserTokenResponseDTO>? result = _authService.RefreshToken(requestDto);

            if (!result.Success)
            {
                return StatusCode((int)result.StatusCode, result);
            }
            return Ok(result);
        }
    }
}
