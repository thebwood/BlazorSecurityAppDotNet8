using BlazorSecurityApp.Core.Common;
using BlazorSecurityApp.Core.DTOs;
using BlazorSecurityApp.Web.Services.Interfaces;
using System.Text.Json;
using System.Text;
using BlazorSecurityApp.Core.Models;
using BlazorSecurityApp.Web.Security;

namespace BlazorSecurityApp.Web.Services
{
    public class AuthClient : IAuthClient
    {
        private readonly HttpClient _httpClient;
        private readonly CustomAuthenticationStateProvider _authStateProvider;
        private readonly IConfiguration _configuration;

        public AuthClient(HttpClient httpClient, CustomAuthenticationStateProvider authStateProvider, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _authStateProvider = authStateProvider;
            _configuration = configuration;
        }

        public async Task<Result<LoginResponseDTO>> LoginAsync(LoginModel loginModel)
        {
            Result<LoginResponseDTO> result = new();
            LoginRequestDTO loginRequest = new LoginRequestDTO
            {
                UserName = loginModel.Username,
                Password = loginModel.Password
            };
            string jsonPayload = JsonSerializer.Serialize(loginRequest);
            StringContent? requestContent = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            using HttpResponseMessage response = await _httpClient.PostAsync("api/auth/login", requestContent);
            string? content = await response.Content.ReadAsStringAsync();
            result = JsonSerializer.Deserialize<Result<LoginResponseDTO>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();

            if (result.Success)
            {
                await _authStateProvider.MarkUserAsAuthenticated(result.Value.Token, result.Value.RefreshToken);
            }
            return result;
        }

        public async Task<Result<RefreshUserTokenResponseDTO>> RefreshTokenAsync(string refreshToken)
        {
            RefreshUserTokenRequestDTO refreshRequest = new RefreshUserTokenRequestDTO
            {
                UserId = 1,
                RefreshToken = refreshToken
            };

            string jsonPayload = JsonSerializer.Serialize(refreshRequest);
            StringContent? requestContent = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            using HttpResponseMessage response = await _httpClient.PostAsync("api/auth/refresh", requestContent);
            string? content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<Result<RefreshUserTokenResponseDTO>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();

            if (result.Success)
            {
                await _authStateProvider.MarkUserAsAuthenticated(result.Value.Token, result.Value.RefreshToken);
            }
            else
            {
                await _authStateProvider.MarkUserAsLoggedOut();
            }

            return result;
        }
    }
}
