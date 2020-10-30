using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RunTogether.Shared.Map
{
    public partial class Map
    {
        ElementReference mapid;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
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
