using BlazorSecurityApp.Core.Models;
using BlazorSecurityApp.Web.BaseClasses;
using BlazorSecurityApp.Web.ViewModels.Auth;
using Microsoft.AspNetCore.Components;

namespace BlazorSecurityApp.Web.Components.Pages
{
    public partial class Home : CommonBase
    {
        [Inject]
        private LoginViewModel _loginViewModel { get; set; }

        private async Task Login()
        {
            await _loginViewModel.LoginAsync(new LoginModel { Username = "test", Password = "test" });
            NavigationManager.NavigateTo("/dashboard");
        }
    }
}
