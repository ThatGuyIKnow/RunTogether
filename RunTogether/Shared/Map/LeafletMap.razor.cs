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

        [Parameter]
        public RunRoute Route { get; set; }

        public bool Markers = false; 

        public string json; 

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {


            //await JsRunTime.InvokeVoidAsync("Main.tester", json);

            if (firstRender)
            {
                await JsRunTime.InvokeVoidAsync("Main.Map.initializeMap");
                StateHasChanged();
            }

            await JsRunTime.InvokeVoidAsync("Main.Map.addMarkersAndLines", json);

        }

        protected override void OnParametersSet()
        {

            json = JsonConvert.SerializeObject(new { Coordinates = Route.ToPointList() });

        }


        private async void button_add()
        {
            //json = JsonConvert.SerializeObject(new { Coordinates = Route.ToPointList() });

            //await JsRunTime.InvokeVoidAsync("Main.Map.addMarkersAndLines", json);
        }


        private async void button_remove()
        {
            //await JsRunTime.InvokeVoidAsync("Main.Map.removeMarkersAndLines");
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
