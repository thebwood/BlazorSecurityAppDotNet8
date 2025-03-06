using BlazorSecurityApp.Web.Services.Interfaces;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.JSInterop;

namespace BlazorSecurityApp.Web.Services
{
    public class StorageService : IStorageService
    {
        private readonly ProtectedSessionStorage _sessionStorage;
        private readonly IJSRuntime _jsRuntime;

        public StorageService(ProtectedSessionStorage sessionStorage, IJSRuntime jsRuntime)
        {
            _sessionStorage = sessionStorage;
            _jsRuntime = jsRuntime;
        }

        public async Task SetAccessTokenAsync(string token)
        {
            await _sessionStorage.SetAsync("accessToken", token);
        }

        public async Task<string?> GetAccessTokenAsync()
        {
            var tokenResult = await _sessionStorage.GetAsync<string>("accessToken");
            return tokenResult.Success ? tokenResult.Value : null;
        }

        public async Task SetRefreshTokenAsync(string refreshToken)
        {
            await _sessionStorage.SetAsync("refreshToken", refreshToken);
        }

        public async Task<string?> GetRefreshTokenAsync()
        {
            var tokenResult = await _sessionStorage.GetAsync<string>("refreshToken");
            return tokenResult.Success ? tokenResult.Value : null;
        }

        public async Task ClearStorageAsync()
        {
            await _sessionStorage.DeleteAsync("accessToken");
            await _sessionStorage.DeleteAsync("refreshToken");
        }
    }
}

