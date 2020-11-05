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


            //await JsRunTime.InvokeVoidAsync("Main.tester", json);

            if (firstRender)
            {
                await JsRunTime.InvokeVoidAsync("Main.Map.initializeMap");
                StateHasChanged();
            }
        }

    }

}
