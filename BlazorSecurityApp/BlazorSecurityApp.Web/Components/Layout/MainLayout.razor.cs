using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;

namespace BlazorSecurityApp.Web.Components.Layout
{
    public partial class MainLayout : LayoutComponentBase
    {
        [Inject]
        public AuthenticationStateProvider authStateProvider { get; set; } = default!;

        [Inject]
        public NavigationManager navManager { get; set; } = default!;


    }
}
