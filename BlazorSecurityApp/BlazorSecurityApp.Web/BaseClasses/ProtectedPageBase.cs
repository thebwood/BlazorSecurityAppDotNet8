using BlazorSecurityApp.Core.Common;
using BlazorSecurityApp.Core.DTOs;
using BlazorSecurityApp.Web.Security;
using BlazorSecurityApp.Web.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using System.Security.Claims;

namespace BlazorSecurityApp.Web.BaseClasses
{
    public abstract class ProtectedPageBase : CommonBase
    {
        [Inject]
        protected CustomAuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;
        [Inject]
        protected IAuthClient AuthClient { get; set; } = default!;
        [Inject]
        protected IStorageService StorageService { get; set; } = default!;
        [Inject]
        protected ILogger<ProtectedPageBase> Logger { get; set; } = default!;

        protected ClaimsPrincipal? User { get; private set; }
        private bool _isRendered;

        protected override async Task OnInitializedAsync()
        {
            Logger.LogInformation("OnInitializedAsync called");

            if (AuthenticationStateProvider == null || AuthClient == null)
            {
                Logger.LogError("AuthenticationStateProvider or AuthClient is null");
                HandleAuthenticationFailure();
                return;
            }

            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            if (authState == null)
            {
                Logger.LogError("AuthenticationState is null");
                HandleAuthenticationFailure();
                return;
            }


            if (_isRendered)
            {

                User = authState.User;
                await RefreshTokenAsync();
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            _isRendered = true;
            await RefreshTokenAsync();
        }

        private async Task RefreshTokenAsync()
        {
            var refreshToken = await StorageService.GetRefreshTokenAsync();
            if (!string.IsNullOrEmpty(refreshToken))
            {
                Result<RefreshUserTokenResponseDTO>? result = await AuthClient.RefreshTokenAsync(refreshToken);
                if (result.Success)
                {
                    await AuthenticationStateProvider.MarkUserAsAuthenticated(result.Value.Token, result.Value.RefreshToken);
                }
                else
                {
                    Logger.LogWarning("Failed to refresh token");
                    HandleAuthenticationFailure();
                }
            }
            else
            {
                Logger.LogWarning("Refresh token is null or empty");
                HandleAuthenticationFailure();
            }
        }

        private void HandleAuthenticationFailure()
        {
            Logger.LogInformation("Handling authentication failure");
            // Log the user out and redirect to the login page
            AuthenticationStateProvider?.MarkUserAsLoggedOut();
            NavigationManager.NavigateTo("/", forceLoad: true);
        }

        protected virtual bool IsUserAuthorized() => true;
    }
}