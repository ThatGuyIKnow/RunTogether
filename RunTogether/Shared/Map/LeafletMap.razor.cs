using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using RunTogether.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace RunTogether.Shared.Map
{
    public partial class LeafletMap
    {
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            Stage stage1 = new Stage(new StartPoint(1.1F, 6.7F),  new EndPoint(2.2F, 7.6F));
            Stage stage2 = new Stage(new StartPoint(3.3F, 6.7F), new EndPoint(4.4F, 7.6F));
            Stage stage3 = new Stage(new StartPoint(5.5F, 6.7F), new EndPoint(6.6F, 7.6F));

            RunRoute test = new RunRoute();

            test.Stages = new List<Stage> { stage1, stage2, stage3 }; 

            var json = JsonConvert.SerializeObject(new { Coordinates = test.ToPointList() });

            Console.WriteLine(json);

            await JsRunTime.InvokeVoidAsync("Main.tester", json);

            if (firstRender)
            {
                await JsRunTime.InvokeVoidAsync("Main.Map.initializeMap");
                StateHasChanged();
            }

        }

        private async void button_add()
        {
            await JsRunTime.InvokeVoidAsync("Main.Map.addMarkersAndLines");
        }


        private async void button_remove()
        {
            await JsRunTime.InvokeVoidAsync("Main.Map.removeMarkersAndLines");
        }



        /*
            protected string Coordinates { get; set; }

            protected void Mouse_Move(MouseEventArgs e)
            {
                Coordinates = $"X = {e.ClientX } Y = {e.ClientY}";
            }

            private async void CreateMarker()
            {
                await JsRunTime.InvokeVoidAsync("Main.onMapClick");
            }

        */
    }
}
