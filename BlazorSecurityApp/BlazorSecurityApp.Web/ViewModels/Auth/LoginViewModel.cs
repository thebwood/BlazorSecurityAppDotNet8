using BlazorSecurityApp.Core.Common;
using BlazorSecurityApp.Core.Models;
using BlazorSecurityApp.Web.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace BlazorSecurityApp.Web.ViewModels.Auth
{
    public class LoginViewModel
    {
        private readonly IAuthClient _authClient;

        public LoginViewModel(IAuthClient authClient)
        {
            _authClient = authClient;
        }

        public async Task<Result> LoginAsync(LoginModel loginModel)
        {
            Result? result = await _authClient.LoginAsync(loginModel);

            return result;
        }


    }
}
