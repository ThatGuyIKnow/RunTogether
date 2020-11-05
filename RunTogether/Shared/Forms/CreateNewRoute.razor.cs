using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen.Blazor;
using RunTogether.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RunTogether.Shared.Forms
{
    public partial class CreateNewRoute
    {
        ElementReference mapid;
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await JSRuntime.InvokeVoidAsync("Main.leaflet_start");
                firstRender = false;
            }
        }

        Stage Selected = new Stage(new StartPoint(0F,0F), new EndPoint(0F,0F));
        IQueryable<Stage> Selecteds;

        //Stage stage = new Stage();
        RadzenGrid<Stage> table;

        RunRoute runRoute = new RunRoute();
        Run run = new Run();

        List<Stage> newStages = new List<Stage>();

        DateTime date;
        float xCoordinate;
        float yCoordinate;



        //IEnumerable<RunRoute> runRoutes;
        //IEnumerable<Stage> stages;
        //protected override void OnInitialized()
        //{
        //    runRoutes = dbContext.RunRoutes;
        //    stages = dbContext.Stages;
        //}


    public void OnSubmit(DateTime Start, float xCoord, float yCoord)
        {
            Stage StageObj = new Stage(new StartPoint(0F, 0F), new EndPoint(0F, 0F)) { Date = Start, RunRoute = runRoute };
            //StageObj.StartPoint.StageId = StageObj.StageId;
            StartPoint startPoint = new StartPoint(xCoord, yCoord);
            StageObj.StartPoint = startPoint;
            runRoute.Stages.Add(StageObj);
            newStages.Add(StageObj);
            Console.WriteLine("I am in onsubmit");
            Console.WriteLine(StageObj.StartPoint.X + "," + StageObj.StartPoint.Y);
            this.dialogService.Close(true);
            table.Reload();

        }

        void OnInvalidSubmit()
        {
            Console.WriteLine("AW");
        }

        public void SaveRouteChanges()
        {
            runRoute.Run = run; //burde laves om til at automatisk tage det run man har valgt at edit route på
            dialogService.Close(true);

            dbContext.RunRoutes.Add(runRoute);

            foreach (Stage stage in newStages)
            {
                dbContext.Stages.Add(stage);
            }

            dbContext.SaveChanges();
        }

        public void CancelRouteChanges()
        {
            dialogService.Close(true);
        }
    }
}
