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

namespace RunTogether.Shared.Etc
{
    public partial class DataTable
    {


        void PrintImage()
        {
            jsRuntime.InvokeVoidAsync("Main.Common.PrintImage", "QRCodeImg");
        }


        public class Farver
        {
            public string Name { get; set; }
            public string code { get; set; }
        }

        List<Farver> colorList = new List<Farver>();

        //Variabler for QRCode gen
        string color = "#000000";
        int slider = 50;

        //Delecare variable for referencing radzen table (@ref="table") as RadzenGrid of type Run 
        RadzenGrid<Run> runTable;
        RadzenGrid<ApplicationUser> runnerTable;

        //henter hele data table ned og filtere client-side, men der laves ingen filtering her, så det er fint.
        IEnumerable<Run> runs;

        //alt querying bliver lavet i DB og kun det relevante data sendes til client.
        IQueryable<ApplicationUser> runners;

        protected override async Task OnInitializedAsync()
        {
            runs = dbContext.Runs.Include(r => r.Route);

            colorList.Add(new Farver() { Name = "RT rød", code = "#cc4545" });
            colorList.Add(new Farver() { Name = "Sort", code = "#000000" });

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

        Run run = new Run();
        RunRoute Route = new RunRoute();
        public async Task QueryForRunners(Run QueryRun)
        {
            run = QueryRun;

            Route = await dbContext.RunRoutes
                .Where(r => r.RunId == QueryRun.ID)
                .Include(r => r.Stages).ThenInclude(s => s.EndPoint)
                .FirstOrDefaultAsync();

            //await dbContext.Entry(Route).Collection(r => r.Stages).LoadAsync();
            //dbContext.Entry(Route.Stages[0]).Collection(s => s.EndPoint).Load();

            //Stage stage = await dbContext.Stages
            //    .Where(s => s.RunRouteId == Route.RunRouteId)
            //    .Include(s => s.StartPoint)
            //    .Include(s => s.EndPoint)
            //    .FirstOrDefaultAsync();

            Console.WriteLine(Route.Stages.Count);

            runners = dbContext.Users
                .Where(u => u.RunId == QueryRun.ID);

        }

        void EditRunner(ApplicationUser selectedRunner)
        {
                dialogService.Open<EditRunner>(($"Rediger: {selectedRunner.FirstName} {selectedRunner.LastName}"),
                    new Dictionary<string, object>() { {"selectedRunner", selectedRunner } },
                    new DialogOptions() { Width = "700px", Height = "530px", Left = "calc(50% - 350px)", Top = "calc(50% - 265px)" });
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



        void OnUpdateRow(ApplicationUser runner)
        {
            dbContext.Update(runner);
            dbContext.SaveChanges();
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

            // For production
            var runnerEntry = dbContext.Entry(runner);
            if (runnerEntry.State == EntityState.Modified)
            {
                runnerEntry.CurrentValues.SetValues(runnerEntry.OriginalValues);
                runnerEntry.State = EntityState.Unchanged;
            }
        }

        void DeleteRow(ApplicationUser runner)
        {
            if (runners.Contains(runner))
            {
                dbContext.Remove<ApplicationUser>(runner);

                // For production
                dbContext.SaveChanges();

                runnerTable.Reload();
            }
            else
            {
                runnerTable.CancelEditRow(runner);
            }
        }

    }
}
