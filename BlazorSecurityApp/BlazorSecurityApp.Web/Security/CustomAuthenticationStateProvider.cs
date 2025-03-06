using BlazorSecurityApp.Web.Services.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Headers;
using System.Security.Claims;
using Microsoft.IdentityModel.JsonWebTokens;

namespace BlazorSecurityApp.Web.Security
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly IStorageService _storageService;
        private readonly NavigationManager _navigationManager;
        private ClaimsPrincipal _anonymous = new ClaimsPrincipal(new ClaimsIdentity());

        public CustomAuthenticationStateProvider(IStorageService storageService, NavigationManager navigationManager)
        {
            _storageService = storageService;
            _navigationManager = navigationManager;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await _storageService.GetAccessTokenAsync();

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

        public async Task<string?> GetTokenAsync()
        {
            return await _storageService.GetAccessTokenAsync();
        }


        public async Task MarkUserAsAuthenticated(string token, string refreshToken)
        {
            await _storageService.SetAccessTokenAsync(token);
            await _storageService.SetRefreshTokenAsync(refreshToken);
            var user = new ClaimsPrincipal(new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt"));

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }

        public async Task MarkUserAsLoggedOut()
        {
            await _storageService.ClearStorageAsync();
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_anonymous)));
        }

        public async Task<bool> SetAuthorizationHeaderAsync(HttpClient httpClient)
        {
            var token = await _storageService.GetAccessTokenAsync();
            if (token != null)
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                return true;
            }
            return false;
        }
    }
}