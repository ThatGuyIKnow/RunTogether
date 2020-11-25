using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using RunTogether.Areas.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RunTogether.Shared.Etc
{
    public partial class NavMenuOrganiserHome
    {
        [Parameter]
        public string ID { get; set; }

        bool isSuperAdmin = false;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            AuthenticationState authState = await authProvider.GetAuthenticationStateAsync();
            try
            {
                ApplicationUser user = await userManager.GetUserAsync(authState.User);

                isSuperAdmin = await userManager.IsInRoleAsync(user, IdentityRoleTypes.SuperOrganiser);
            }
            catch (Exception e)
            {
                isSuperAdmin = false;
            }
            StateHasChanged();
        }
    }
}
