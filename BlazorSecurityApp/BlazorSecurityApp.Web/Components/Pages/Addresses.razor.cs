using BlazorSecurityApp.Web.BaseClasses;

namespace BlazorSecurityApp.Web.Components.Pages
{
    public partial class Addresses : ProtectedPageBase
    {
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            // Add any additional logic specific to the Dashboard component here
        }

    }
}
