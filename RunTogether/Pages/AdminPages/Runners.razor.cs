using Microsoft.AspNetCore.Identity;
using RunTogether.Shared.Forms;
using RunTogether.Areas.Identity;
using System.Collections.Generic;
using RunTogether.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Components;
using Radzen.Blazor;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Radzen;
using System;
using System.Linq;

namespace RunTogether.Pages.AdminPages
{
    public partial class Runners
    {
        [Parameter] public int id { get; set; }

        RadzenGrid<ApplicationUser> runnerTable;

        Run run = new Run();
        IQueryable<ApplicationUser> runnerList;

        protected override async Task OnInitializedAsync()
        {

            run = dbContext.Runs
                .Where(r => r.ID == id)
                .Include(r => r.Runners).FirstOrDefault();

            runnerList = dbContext.Runs
                .Where(r => r.ID == id)
                .Include(r => r.Runners)
                .SelectMany(r => r.Runners);





            dialogService.OnOpen += Open;
            dialogService.OnClose += Close;

        }

        //retunere Email hvis den blev fundet ellers default
        public string FindRunner(string Email)
        {
            if (runnerList.Any(r => r.Email == Email) == true)
            {
                return Email;
            }
            return default;
        }


        void Open(string title, Type type, Dictionary<string, object> parameters, DialogOptions options)
        {
            StateHasChanged();
        }

        void Close(dynamic result)
        {
            runnerTable.Reload();
            StateHasChanged();
        }



        async Task OnUpdateRow(ApplicationUser runner)
        {
            dbContext.Update(runner);
            await dbContext.SaveChangesAsync();

        }
        void EditRow(ApplicationUser runner)
        {
            runnerTable.EditRow(runner);
        }

        void SaveRow(ApplicationUser runner)
        {
            runnerTable.UpdateRow(runner);
        }

        void CancelEdit(ApplicationUser runner)
        {
            runnerTable.CancelEditRow(runner);

            var runnerEntry = dbContext.Entry(runner);
            if (runnerEntry.State == EntityState.Modified)
            {
                runnerEntry.CurrentValues.SetValues(runnerEntry.OriginalValues);
                runnerEntry.State = EntityState.Unchanged;
            }
        }

        async Task DeleteRow(ApplicationUser runner)
        {
            if (run.Runners.Contains(runner))
            {
                dbContext.Remove<ApplicationUser>(runner);

                await dbContext.SaveChangesAsync();


                runnerTable.Reload();
            }
            else
            {
                runnerTable.CancelEditRow(runner);
            }
        }

        void CopyLogin(string key)
        {
            string url = $"{Navigator.BaseUri}runner/login?key={key}";
            jsRuntime.InvokeVoidAsync("Main.Common.CopyToClipboard", url);
        }
    }
}
