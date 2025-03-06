using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Net.Http.Headers;
using System.Security.Claims;
using Microsoft.IdentityModel.JsonWebTokens;

namespace BlazorSecurityApp.Web.Security
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider, IComponent
    {
        private readonly ProtectedSessionStorage _sessionStorage;
        private readonly NavigationManager _navigationManager;
        private ClaimsPrincipal _anonymous = new ClaimsPrincipal(new ClaimsIdentity());
        private bool _isRendered;

        public CustomAuthenticationStateProvider(ProtectedSessionStorage sessionStorage, NavigationManager navigationManager)
        {
            _sessionStorage = sessionStorage;
            _navigationManager = navigationManager;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await GetAccessTokenAsync();

            if (string.IsNullOrEmpty(token))
            {
                return new AuthenticationState(_anonymous);
            }

            if (IsTokenExpired(token))
            {
                _navigationManager.NavigateTo("/");
                return new AuthenticationState(_anonymous);
            }

            var user = new ClaimsPrincipal(new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt"));
            return new AuthenticationState(user);
        }

        public static bool IsTokenExpired(string token)
        {
            var jwt = new JsonWebTokenHandler().ReadJsonWebToken(token);
            return jwt.ValidTo < DateTime.UtcNow;
        }

        public static IEnumerable<Claim> ParseClaimsFromJwt(string token)
        {
            var jwtHandler = new JsonWebTokenHandler();
            var jwtToken = jwtHandler.ReadJsonWebToken(token);

            return jwtToken.Claims;
        }

        public async Task<string?> GetAccessTokenAsync()
        {
            if (_isRendered)
            {
                var tokenResult = await _sessionStorage.GetAsync<string>("accessToken");
                return tokenResult.Success ? tokenResult.Value : null;
            }
            return null;
        }

        public async Task SetAccessTokenAsync(string token)
        {
            await _sessionStorage.SetAsync("accessToken", token);
        }

        public async Task<string?> GetRefreshTokenAsync()
        {
            if (_isRendered)
            {
                var tokenResult = await _sessionStorage.GetAsync<string>("refreshToken");
                return tokenResult.Success ? tokenResult.Value : null;
            }
            return null;
        }

        public async Task SetRefreshTokenAsync(string refreshToken)
        {
            await _sessionStorage.SetAsync("refreshToken", refreshToken);
        }

        public async Task ClearStorageAsync()
        {
            await _sessionStorage.DeleteAsync("accessToken");
            await _sessionStorage.DeleteAsync("refreshToken");
        }

        public async Task MarkUserAsAuthenticated(string token, string refreshToken)
        {
            await SetAccessTokenAsync(token);
            await SetRefreshTokenAsync(refreshToken);
            var user = new ClaimsPrincipal(new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt"));

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }

        public async Task MarkUserAsLoggedOut()
        {
            await ClearStorageAsync();
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_anonymous)));
        }

        public async Task<bool> SetAuthorizationHeaderAsync(HttpClient httpClient)
        {
            var token = await GetAccessTokenAsync();
            if (token != null)
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                return true;
            }
            return false;
        }

        public void Attach(RenderHandle renderHandle)
        {
            // Implementation for IComponent.Attach
        }

        public Task SetParametersAsync(ParameterView parameters)
        {
            // Implementation for IComponent.SetParametersAsync
            return Task.CompletedTask;
        }

        public void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                _isRendered = true;
            }
        }
    }
}

