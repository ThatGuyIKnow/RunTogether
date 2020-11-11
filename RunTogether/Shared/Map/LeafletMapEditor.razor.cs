using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using RunTogether.Data;



namespace RunTogether.Shared.Map
{
    public partial class LeafletMapEditor
    {

        [Parameter]
        public RunRoute Route { get; set; }

        public string json;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                Route.Stages.Add(new Stage() { StartPoint = new StartPoint(57.0117789F, 9.9907118F), EndPoint = new EndPoint(56.7499F, 9.9921F) }); 
                await JsRunTime.InvokeVoidAsync("Main.MapEditor.initializeMap");
                System.Diagnostics.Debug.WriteLine("got here!!");
                StateHasChanged();
            }

            await JsRunTime.InvokeVoidAsync("Main.MapEditor.loadRoute", json);
        }

        protected override void OnParametersSet()
        {

            json = JsonConvert.SerializeObject(Route);

        }

        //public async void ClickEvent()
        //{
        //    //await JsRunTime.InvokeVoidAsync("Main.tester");
        //    //await JsRunTime.InvokeVoidAsync("Main.MapEditor.onMapClick");

        //}

    }

}
