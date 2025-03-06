using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace BlazorSecurityApp.Web.Components.Shared
{
    public partial class MainLayout : LayoutComponentBase
    {
        [Inject]
        public AuthenticationStateProvider authStateProvider { get; set; }

        [Inject]
        public NavigationManager navManager { get; set; }
        

    }
}
