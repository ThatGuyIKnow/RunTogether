﻿using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
using RunTogether.Areas.Identity;
using RunTogether.Areas.Identity.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        RadzenGrid<Run> table;

        //henter hele data table ned og filtere client-side, men der laves ingen filtering her, så det er fint.
        IEnumerable<Run> runs;

        //alt querying bliver lavet i DB og kun det relevante data sendes til client.
        IQueryable<ApplicationUser> runners;

        protected override async Task OnInitializedAsync()
        {
            runs = dbContext.Runs;

            colorList.Add(new Farver() { Name = "RT rød", code = "#cc4545" });
            colorList.Add(new Farver() { Name = "Sort", code = "#000000" });

            dialogService.OnOpen += Open;
            dialogService.OnClose += Close;

            //dbContext.Runs.Add(new Run { Name = "Løb 1", StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(1), QRString = "test code" });
            //dbContext.Runs.Add(new Run { Name = "Løb 2", StartDate = DateTime.Now.AddDays(2), EndDate = DateTime.Now.AddDays(3), QRString = "ajuf_££$dafdf" });
            //dbContext.Runs.Add(new Run { Name = "Løb 3", StartDate = DateTime.Now.AddDays(4), EndDate = DateTime.Now.AddDays(5), QRString = "asdafgds" });
            //dbContext.SaveChanges();
            //await test.CreateRunner("Frederik", "Deiborg", "HejMedDig@gmail.com", runs.ElementAt(0));
            //await test.CreateRunner("Oliver", "Hansen", "Coolguy@gmail.com", runs.ElementAt(0));
            //await test.CreateRunner("Kurt", "C.Kode", "CisGod@gmail.com", runs.ElementAt(1));
            //await test.CreateRunner("Mads", "Madsen", "SejtNavnGod@gmail.com", runs.ElementAt(1));
            //await test.CreateRunner("Ran", "D.Om", "RandomMaild@gmail.com", runs.ElementAt(1));
            //await test.CreateRunner("All", "Alone", "Lonely@gmail.com", runs.ElementAt(2));
        }

        Run run = new Run();
        public void QueryForRunners(Run QueryRun)
        {

            run = QueryRun;

            runners = dbContext.Users
                .Where(u => u.RunId == QueryRun.ID);
        }

        void Open(string title, Type type, Dictionary<string, object> parameters, DialogOptions options)
        {
            StateHasChanged();
        }

        void Close(dynamic result)
        {
            table.Reload();
            StateHasChanged();
        }


    }
}
