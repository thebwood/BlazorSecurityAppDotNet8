﻿using BlazorSecurityApp.Web.BaseClasses;

namespace BlazorSecurityApp.Web.Components.Pages
{
    public partial class Dashboard : ProtectedPageBase
    {
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }
    }
}
