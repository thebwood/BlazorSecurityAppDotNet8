using BlazorSecurityApp.Core.Common;
using BlazorSecurityApp.Core.DTOs;
using BlazorSecurityApp.Core.Models;

namespace BlazorSecurityApp.Web.Services.Interfaces
{
    public interface IAuthClient
    {
        Task<Result<LoginResponseDTO>> LoginAsync(LoginModel loginModel);
        Task<Result<RefreshUserTokenResponseDTO>> RefreshTokenAsync(string refreshToken);
    }
}
