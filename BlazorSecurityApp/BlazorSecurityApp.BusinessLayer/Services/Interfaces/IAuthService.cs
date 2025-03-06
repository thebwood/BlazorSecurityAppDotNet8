using BlazorSecurityApp.Core.Common;
using BlazorSecurityApp.Core.DTOs;

namespace BlazorSecurityApp.BusinessLayer.Services.Interfaces
{
    public interface IAuthService
    {
        Result<LoginResponseDTO> Login(LoginRequestDTO login);
        Result<RefreshUserTokenResponseDTO> RefreshToken(RefreshUserTokenRequestDTO requestDTO);
    }
}
