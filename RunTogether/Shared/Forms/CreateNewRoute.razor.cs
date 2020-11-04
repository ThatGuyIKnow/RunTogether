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
        //Stage Selected = new Stage(new StartPoint(0F,0F), new EndPoint(0F,0F));
        //IQueryable<Stage> Selecteds;

        //Stage stage = new Stage();
        RadzenGrid<Stage> table;

        RunRoute runRoute = new RunRoute();
        RunRoute initialMapRoute = new RunRoute() { Stages = new List<Stage> { new Stage(new StartPoint(0F, 0F), new EndPoint(0F, 0F)) } };
        Run run = new Run();

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


        void OnSubmit()
        {
            StartPoint startPoint = new StartPoint(xCoordinateStart, yCoordinateStart);
            EndPoint endPoint = new EndPoint(xCoordinateEnd, yCoordinateEnd);
            Stage StageObj = new Stage(startPoint, endPoint) { Date = date, RunRoute = runRoute };
            
            runRoute.Stages.Add(StageObj);
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

            dbContext.SaveChanges();
        }

        void CancelRouteChanges()
        {
            dialogService.Close(true);
        }
    }
}
