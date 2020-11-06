using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
using RunTogether.Areas.Identity;
using RunTogether.Areas.Identity.Helpers;
using RunTogether.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RunTogether.Shared.Forms;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Components;

namespace RunTogether.Shared.Etc
{
    public partial class DataTable
    {

        // Kalder JS function til at printe QR koden
        void PrintImage()
        {
            jsRuntime.InvokeVoidAsync("Main.Common.PrintImage", "QRCodeImg");
        }

        // variable til at holde det valgte løb
        Run run = new Run();

        //Variabler for QRCode gen
        List<Tuple<string, string>> ColorList = new List<Tuple<string, string>>();
        string color = "#000000";
        int slider = 50;

        //Delecare variable for referencing radzen table (@ref="table") as RadzenGrid of type Run 
        RadzenGrid<Run> runTable;
        RadzenGrid<ApplicationUser> runnerTable;

        //alt querying bliver lavet i DB og kun det relevante data sendes til client.
        IQueryable<Run> runs;

        protected override async Task OnInitializedAsync()
        {
            runs = dbContext.Runs
                .Include(r => r.Route)
                    .ThenInclude(rr => rr.Stages)
                        .ThenInclude(s => s.StartPoint)
                .Include(r => r.Route)
                    .ThenInclude(rr => rr.Stages)
                        .ThenInclude(s => s.EndPoint)
                .Include(r => r.Route)
                    .ThenInclude(rr => rr.Stages)
                        .ThenInclude(s => s.ThroughPoints)
                .Include(r => r.Runners);

            ColorList.Add(new Tuple<string, string>("RT rød", "#cc4545"));
            ColorList.Add(new Tuple<string, string>("Sort", "#000000"));
            ColorList.Add(new Tuple<string, string>("Navy blå", "#000080"));

            dialogService.OnOpen += Open;
            dialogService.OnClose += Close;

            //await GenerateTestData();
        }

        public async Task GenerateTestData()
        {
                var testRun = new Run
                {
                    Name = "Løb 1",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddDays(1),
                    QRString = "test code",

                    Runners = new List<ApplicationUser>
                    {
                        new ApplicationUser { FirstName = "Oliver", LastName = "Hansen", Email = "Coolguy@gmail.com" },
                        new ApplicationUser { FirstName = "Kurt", LastName = "C.Kode", Email = "CisGod@gmail.com" },
                        new ApplicationUser { FirstName = "Mads", LastName = "Madsen", Email = "SejtNavnGod@gmail.com" }
                    },

                    Route = new RunRoute
                    {
                        Stages = new List<Stage>
                        {
                            new Stage() {StartPoint = new StartPoint(57.0117789F, 9.9907118F), EndPoint = new EndPoint(56.7499F, 9.9921F)}
                        }
                    }
                };

                dbContext.Runs.Add(testRun);
                await dbContext.SaveChangesAsync();
        }

        public async Task QueryForRunners(Run QueryRun)
        {
            run = QueryRun;
        }

        private void NavigateToPage(string path)
        {
            NavigationManager.NavigateTo(path);
        }

        void Open(string title, Type type, Dictionary<string, object> parameters, DialogOptions options)
        {
            StateHasChanged();
        }

        void Close(dynamic result)
        {
            runTable.Reload();
            if(run.ID != default)
                runnerTable.Reload();
            StateHasChanged();
        }

    }
}
