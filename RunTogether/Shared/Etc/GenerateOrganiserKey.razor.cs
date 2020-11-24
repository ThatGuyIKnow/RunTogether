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
        IQueryable<ApplicationUser> organiserList;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                AuthenticationState authState = await authProvider.GetAuthenticationStateAsync();
                try
                {
                    user = await userManager.GetUserAsync(authState.User);
                    organiserKey = await dbContext.OrganiserCreationKeys.FirstAsync(k => k.GeneratedById == user.Id);
                }
                catch (InvalidOperationException e)
                {
                    await RefreshKey();
                }

                var ids = dbContext.UserRoles
                    .Where(r => r.RoleId == "organiser")
                    .Select(r => r.UserId);
                organiserList = dbContext.Users
                    .Where(u => ids.Contains(u.Id))
                    .Where(u => u.Id != user.Id);

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
            dbContext.Remove(selectedOrganiser);

            await dbContext.SaveChangesAsync();


            organiserGrid.Reload();
        }

        async Task LoadOrganisers()
        {
            await InvokeAsync(StateHasChanged);
        }
    }
}
