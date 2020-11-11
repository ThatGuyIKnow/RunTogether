using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
//using Newtonsoft.Json;
using RunTogether.Data;
using RunTogether.Shared.Etc.Helpers;
using Microsoft.EntityFrameworkCore;

namespace RunTogether.Shared.Map
{

    public partial class LeafletMapEditor
    {
        private EventHandlerHelper Handler = new EventHandlerHelper();

        [Parameter]
        public Run Run { get; set; }

        public RunRoute Route { get; set; }

        public string json;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                


                Handler.AddHandler("AddSegment", (evt) => {
                   
                    Stage NewStage = JsonSerializer.Deserialize<Stage>(evt.ToString());

                    Route.Stages.Add(NewStage);

                    dbContext.SaveChanges(); 
                    
                });
                
                await JsRunTime.InvokeVoidAsync("Main.MapEditor.initializeMap", Handler.ObjRef);
                
                StateHasChanged();
                
            }

            await JsRunTime.InvokeVoidAsync("Main.MapEditor.loadRoute", json);
        }

        protected override void OnParametersSet()
        {
            if (Run.Route == null)
            {
                Run.Route = new RunRoute();
            }

            Route = Run.Route;

            json = JsonSerializer.Serialize(Route);
        }

        //public async void ClickEvent()
        //{
        //    //await JsRunTime.InvokeVoidAsync("Main.tester");
        //    //await JsRunTime.InvokeVoidAsync("Main.MapEditor.onMapClick");

        //}

    }

}
