using BlazorSecurityApp.Web.Services.Interfaces;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.JSInterop;

namespace BlazorSecurityApp.Web.Services
{
    public class StorageService : IStorageService
    {
        private readonly ProtectedLocalStorage _localStorage;
        private readonly IJSRuntime _jsRuntime;

        public StorageService(ProtectedLocalStorage localStorage, IJSRuntime jsRuntime)
        {
            _localStorage = localStorage;
            _jsRuntime = jsRuntime;
        }

        public async Task SetAccessTokenAsync(string token)
        {
            await _localStorage.SetAsync("accessToken", token);
        }

        public async Task<string?> GetAccessTokenAsync()
        {
            if (_jsRuntime is IJSInProcessRuntime)
            {
                // JavaScript interop calls are allowed
                var tokenResult = await _localStorage.GetAsync<string>("accessToken");
                return tokenResult.Success ? tokenResult.Value : null;
            }
            else
            {
                // JavaScript interop calls are not allowed during prerendering
                return null;
            }
        }

        public async Task SetRefreshTokenAsync(string refreshToken)
        {
            await _localStorage.SetAsync("refreshToken", refreshToken);
        }

        public async Task<string?> GetRefreshTokenAsync()
        {
            if (_jsRuntime is IJSInProcessRuntime)
            {
                // JavaScript interop calls are allowed
                var tokenResult = await _localStorage.GetAsync<string>("refreshToken");
                return tokenResult.Success ? tokenResult.Value : null;
            }
            else
            {
                // JavaScript interop calls are not allowed during prerendering
                return null;
            }
        }

        public async Task ClearStorageAsync()
        {
            await _localStorage.DeleteAsync("accessToken");
            await _localStorage.DeleteAsync("refreshToken");
        }
    }
}
