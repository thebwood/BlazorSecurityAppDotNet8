using BlazorSecurityApp.BusinessLayer.Services.Interfaces;
using BlazorSecurityApp.Core.Common;
using BlazorSecurityApp.Core.DTOs;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace BlazorSecurityApp.BusinessLayer.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthService> _logger;

        public AuthService(IConfiguration configuration, ILogger<AuthService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public Result<LoginResponseDTO> Login(LoginRequestDTO login)
        {
            string? tokenString = GenerateJwtToken(1);
            string? refreshTokenString = CreateRefreshToken(1);
            return new Result<LoginResponseDTO>
            {
                StatusCode = HttpStatusCode.OK,
                Value = new LoginResponseDTO
                {
                    Token = tokenString,
                    RefreshToken = refreshTokenString
                }
            };
        }


        public Result<RefreshUserTokenResponseDTO> RefreshToken(RefreshUserTokenRequestDTO requestDTO)
        {
            string? tokenString = GenerateJwtToken(1);
            string? refreshTokenString = CreateRefreshToken(1);
            return new Result<RefreshUserTokenResponseDTO>
            {
                StatusCode = HttpStatusCode.OK,
                Value = new RefreshUserTokenResponseDTO
                {
                    Token = tokenString,
                    RefreshToken = refreshTokenString
                }
            };
        }
        private string? CreateRefreshToken(int v)
        {
            return GenerateRefreshToken();
        }

        private string? GenerateJwtToken(int v)
        {
            var tokenHandler = new JsonWebTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["JwtSettings:Secret"]);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "admin"),
                new Claim(ClaimTypes.NameIdentifier, "Admin User"),
                new Claim(ClaimTypes.Email, "admin@test.com"),
                new Claim(ClaimTypes.Role, "Admin")
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(15),
                Issuer = _configuration["JwtSettings:Issuer"],
                Audience = _configuration["JwtSettings:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return token;
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

    }
}
