namespace BlazorSecurityApp.Web.Services.Interfaces
{
    public interface IStorageService
    {
        Task SetAccessTokenAsync(string token);
        Task<string?> GetAccessTokenAsync();
        Task SetRefreshTokenAsync(string refreshToken);
        Task<string?> GetRefreshTokenAsync();
        Task ClearStorageAsync();
        void OnAfterRender(bool firstRender);

    }
}
