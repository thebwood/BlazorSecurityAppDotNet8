using BlazorSecurityApp.BusinessLayer.Services;
using BlazorSecurityApp.BusinessLayer.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorSecurityApp.BusinessLayer.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBusinessLayer(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            return services;
        }
    }
}
