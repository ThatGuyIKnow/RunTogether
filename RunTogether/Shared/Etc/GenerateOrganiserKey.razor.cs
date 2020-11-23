using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using Radzen.Blazor;
using RunTogether.Areas.Identity;
using RunTogether.Areas.Identity.Data;

namespace RunTogether.Shared.Etc
{
    public partial class GenerateOrganiserKey
    {
        private OrganiserCreationKey organiserKey;
        private ApplicationUser user;

        RadzenGrid<ApplicationUser> organiserGrid;
        IEnumerable<ApplicationUser> organiserList;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                AuthenticationState authState = await authProvider.GetAuthenticationStateAsync();
                user = await userManager.GetUserAsync(authState.User);

                try
                {
                    organiserKey = await dbContext.OrganiserCreationKeys.FirstAsync(k => k.GeneratedById == user.Id);
                }
                catch (InvalidOperationException e)
                {
                    await RefreshKey();
                }
                StateHasChanged();
            }
        }

        void CopyKey(string key)
        {
            string url = $"{Navigator.BaseUri}organiser/register?organiserKey={key}";
            jsRuntime.InvokeVoidAsync("Main.Common.CopyToClipboard", url);
        }

        async Task RefreshKey()
        {
            IQueryable<OrganiserCreationKey> keys = dbContext.OrganiserCreationKeys.Where(k => k.GeneratedById == user.Id);
            dbContext.OrganiserCreationKeys.RemoveRange(keys);
            await dbContext.SaveChangesAsync();
            OrganiserCreationKey newKey = new OrganiserCreationKey(user.Id);
            await dbContext.OrganiserCreationKeys.AddAsync(newKey);
            await dbContext.SaveChangesAsync();
            organiserKey = newKey;
            await InvokeAsync(StateHasChanged);
        }

        async Task DeleteRow(ApplicationUser selectedOrganiser)
        {
            await userManager.DeleteAsync(selectedOrganiser);
        }

        async Task LoadOrganisers()
        {
            organiserList = await userManager.GetUsersInRoleAsync(IdentityRoleTypes.Organiser);
            await InvokeAsync(StateHasChanged);
        }
    }
}
