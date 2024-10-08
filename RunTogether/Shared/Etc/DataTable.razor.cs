﻿using Microsoft.JSInterop;
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

        //Variable for referencing radzen table (@ref="table") as RadzenGrid of type Run 
        RadzenGrid<Run> runTable;
        public bool loading = true;

        //alt querying bliver lavet i DB og kun det relevante data sendes til client.
        IQueryable<Run> runs;

        protected override async Task OnInitializedAsync()
        {

            //variabler til dropdown box for farve til print af qrkode
            ColorList.Add(new Tuple<string, string>("RT rød", "#cc4545"));
            ColorList.Add(new Tuple<string, string>("Sort", "#000000"));
            ColorList.Add(new Tuple<string, string>("Navy blå", "#000080"));

            //variabler til dropdown box for størelse til print af qrkode
            SizeList.Add(new Tuple<string, int>("Stor (A4)", 30));
            SizeList.Add(new Tuple<string, int>("Mellem", 20));
            SizeList.Add(new Tuple<string, int>("Lille", 10));
            SizeList.Add(new Tuple<string, int>("Meget lille", 5));

            //Bestemmer hvilken function der skal køres når en bestemt dialog service åbnes eller lukkes
            dialogService.OnOpen += Open;
            dialogService.OnClose += Close;

            StateHasChanged();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
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
                loading = false;
                StateHasChanged();
            }
        }

        //Sætter run til at være det valgte run fra tabelen
        public async Task QueryForRunners(Run QueryRun)
        {
            run = QueryRun;
            StateHasChanged();
        }

        //Går til url med "path"
        private void NavigateToPage(string path)
        {
            NavigationManager.NavigateTo(path);
        }

        //Skifter activ status for løb
        async Task ChangeActiveStatus(bool value, Run passedRun)
        {
            bool? dialogReturnValue = await dialogService.Confirm("Hvis du skifter status på dette løb, vil det aktive løb blive deaktiveret", "Skift status på " + run.Name + "?", new ConfirmOptions() { OkButtonText = "Ja", CancelButtonText = "Nej" });
            if (dialogReturnValue == true)
            {
                foreach (Run r in runs)
                {
                    r.Active = false;
                }
                passedRun.Active = value;
                dbContext.SaveChanges();
            }

        }

        //Dialogbox for at oprette et løb
        void Open(string title, Type type, Dictionary<string, object> parameters, DialogOptions options)
        {
            StateHasChanged();
        }
        void Close(dynamic result)
        {
            runTable.Reload();
            StateHasChanged();
        }

         async Task DeleteRun(Run run)
        {
            bool? dialogReturnValue = await dialogService.Confirm("Er du sikker på at du vil slette løb med navnet: " + run.Name + "?", "Slet " + run.Name + "?", new ConfirmOptions() { OkButtonText = "Ja", CancelButtonText = "Nej" });
            if(dialogReturnValue == true)
            {
                Console.WriteLine("Sletter: " + run.Name);
                dbContext.Remove(run);
                dbContext.SaveChanges();
            }
            runTable.Reload();
        }

    }
}
