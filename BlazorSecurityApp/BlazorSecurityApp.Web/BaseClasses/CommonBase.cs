using Microsoft.AspNetCore.Components;

namespace BlazorSecurityApp.Web.BaseClasses
{
    public class CommonBase : ComponentBase
    {
        [Inject]
        public NavigationManager NavigationManager { get; set; } = default!;

    }
}
