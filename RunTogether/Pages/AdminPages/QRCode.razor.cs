using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Radzen;
using Microsoft.JSInterop;

namespace RunTogether.Pages.AdminPages
{
    public partial class QRCode
    {
        //variable til at holde id fra url
        [Parameter] public int id { get; set; }

        //variabler til at holde qr kode parametere
        List<Tuple<string, string>> ColorList = new List<Tuple<string, string>>();
        List<Tuple<string, int>> SizeList = new List<Tuple<string, int>>();
        string color = "#000000";
        int size = 30;
        string code;
        
        //variable til at holde løb med id fra url
        Run run = new Run();

        protected override async Task OnInitializedAsync()
        {
            run = dbContext.Runs
                .Find(id);

            dialogService.OnOpen += Open;
            dialogService.OnClose += Close;

            ColorList.Add(new Tuple<string, string>("RT rød", "#cc4545"));
            ColorList.Add(new Tuple<string, string>("Sort", "#000000"));
            ColorList.Add(new Tuple<string, string>("Navy blå", "#000080"));

            SizeList.Add(new Tuple<string, int>("Stor (A4)", 30));
            SizeList.Add(new Tuple<string, int>("Mellem", 20));
            SizeList.Add(new Tuple<string, int>("Lille", 10));
            SizeList.Add(new Tuple<string, int>("Meget lille", 1));
        }

        void PrintImage()
        {
            jsRuntime.InvokeVoidAsync("Main.Common.PrintImage", "QRCodeImg", run.QRString);
        }

        void Open(string title, Type type, Dictionary<string, object> parameters, DialogOptions options)
        {
            StateHasChanged();
        }

        void Close(dynamic result)
        {

            if (result == true)
            {
                run.QRString = code;
                dbContext.Update(run);
                dbContext.SaveChanges();
            }
            else if (result == false)
            {
                code = run.QRString;
            }


            StateHasChanged();
        }

    }
}
