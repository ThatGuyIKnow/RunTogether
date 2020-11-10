using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using RunTogether.Data;



namespace RunTogether.Shared.Map
{
    public partial class LeafletMapEditor
    {
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await JsRunTime.InvokeVoidAsync("Main.MapEditor.initializeMap");
                System.Diagnostics.Debug.WriteLine("got here!!");
                StateHasChanged();
            }
        }

        public async void ClickEvent()
        {
            //await JsRunTime.InvokeVoidAsync("Main.tester");
            //await JsRunTime.InvokeVoidAsync("Main.MapEditor.onMapClick");

        }

    }

}
