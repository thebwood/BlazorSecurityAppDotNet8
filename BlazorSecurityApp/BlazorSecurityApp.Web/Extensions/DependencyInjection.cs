using BlazorSecurityApp.Web.Security;
using BlazorSecurityApp.Web.Services;
using BlazorSecurityApp.Web.Services.Interfaces;
using BlazorSecurityApp.Web.ViewModels.Auth;
using Polly;
using Polly.Extensions.Http;
using Polly.Retry;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.Extensions.Options;
using BlazorSecurityApp.Core.Common;

namespace BlazorSecurityApp.Web.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPresentation(this IServiceCollection services, string baseAddress)
        {
            services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(baseAddress) });

            AsyncRetryPolicy<HttpResponseMessage>? retryPolicy = HttpPolicyExtensions
                .HandleTransientHttpError() // Handles 5xx and 408 errors
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(retryAttempt));

            // Add HttpClient with retry policy and exception handling
            services.AddHttpClient<IAuthClient, AuthClient>((serviceProvider, client) => {
                ApiSettings? apiSettings = serviceProvider.GetRequiredService<IOptions<ApiSettings>>().Value;
                client.BaseAddress = new Uri(baseAddress);

            })
                .AddPolicyHandler(retryPolicy); // Attach the retry policy

            services.AddScoped<IStorageService, StorageService>();
            services.AddScoped<CustomAuthenticationStateProvider>();
            services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
            services.AddTransient<ProtectedLocalStorage>();
            services.AddTransient<LoginViewModel>();
            services.AddTransient<LogoutViewModel>();

            services.AddCascadingAuthenticationState();

            return services;
        }
    }
}
