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
            jsRuntime.InvokeVoidAsync("Main.Common.PrintImage", "QRCodeImg", run.QRString);
        }

        // variable til at holde det valgte løb
        Run run = new Run();

        //Variabler for QRCode gen
        List<Tuple<string, string>> ColorList = new List<Tuple<string, string>>();
        List<Tuple<string, int>> SizeList = new List<Tuple<string, int>>();
        string color = "#000000";
        int size = 30;

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

            SizeList.Add(new Tuple<string, int>("Stor (A4)", 30));
            SizeList.Add(new Tuple<string, int>("Mellem", 20));
            SizeList.Add(new Tuple<string, int>("Lille", 10));
            SizeList.Add(new Tuple<string, int>("Meget lille", 1));

            dialogService.OnOpen += Open;
            dialogService.OnClose += Close;

            await GenerateTestData();
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
                        
                    },

                    Route = new RunRoute
                    {
                        Stages = new List<Stage>
                        {
                            new Stage() {StartPoint = new StartPoint(57.0117789F, 9.9907118F), EndPoint = new EndPoint(56.7499F, 9.9921F)}
                        }
                    }
                };

                await userCreation.CreateRunner("Karin", "Wallsten", "asd@asd.dk", testRun);

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
