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
                await JSRuntime.InvokeVoidAsync("Main.Map.initializeMap");
                StateHasChanged();
            }

        }

        Stage Selected = new Stage();
        IQueryable<Stage> Selecteds;

        //Stage stage = new Stage();
        RadzenGrid<Stage> table;

        RunRoute runRoute = new RunRoute();
        Run run = new Run();

        //List<Stage> newStages = new List<Stage>();

        DateTime date;
        float xCoordinateStart;
        float yCoordinateStart;
        float xCoordinateEnd;
        float yCoordinateEnd;



        //IEnumerable<RunRoute> runRoutes;
        //IEnumerable<Stage> stages;
        //protected override void OnInitialized()
        //{
        //    runRoutes = dbContext.RunRoutes;
        //    stages = dbContext.Stages;
        //}


        void OnSubmit(DateTime Start, float xCoordStart, float yCoordStart, float xxCoordEnd, float yCoordEnd)
        {
            Stage StageObj = new Stage() { Date = Start, RunRoute = runRoute };
            StartPoint startPoint = new StartPoint() { Coordinates = (xCoordStart, yCoordStart) };
            EndPoint endPoint = new EndPoint() { Coordinates = (xCoordinateEnd, yCoordinateEnd) };
            StageObj.StartPoint = startPoint;
            StageObj.EndPoint = endPoint;
            runRoute.Stages.Add(StageObj);
            //newStages.Add(StageObj);
            this.dialogService.Close(true);
            table.Reload();

        }

        void OnInvalidSubmit()
        {
            Console.WriteLine("AW");
        }

        void SaveRouteChanges()
        {
            runRoute.Run = run; //burde laves om til at automatisk tage det run man har valgt at edit route på
            dialogService.Close(true);

            dbContext.RunRoutes.Add(runRoute);

            //foreach (Stage stage in newStages)
            //{
            //    dbContext.Stages.Add(stage);
            //}

            dbContext.SaveChanges();
        }

        void CancelRouteChanges()
        {
            dialogService.Close(true);
        }
    }
}
