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

        [Parameter] public EventCallback<int> sendToParent { get; set; }

        public string json;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                

                //event called from JS
                Handler.AddHandler("AddSegment", (evt) => {
                    //Deserialize and cast to a Stage object. 
                    Stage NewStage = JsonSerializer.Deserialize<Stage>(evt);

                    Run.Route.Stages.Add(NewStage);

                    dbContext.SaveChanges();
                    //StateHasChanged();
                    //JsRunTime.InvokeVoidAsync("Main.MapEditor.loadRoute", json);
                });

                //event called from JS
                Handler.AddHandler("SendStageId", (evt) => {
                    //Deserialize and cast to a Stage object. 
                    Stage NewStage = JsonSerializer.Deserialize<Stage>(evt);

                    Console.WriteLine(NewStage.StageId);
                    sendToParent.InvokeAsync(NewStage.StageId);
                });

                await JsRunTime.InvokeVoidAsync("Main.MapEditor.initializeMap", Handler.ObjRef);
                
                StateHasChanged();
                
            }

            await JsRunTime.InvokeVoidAsync("Main.MapEditor.loadRoute", json);
        }

        protected override void OnParametersSet()
        {
            //if run has no route, add one
            if (Run.Route == null)
            {
                Run.Route = new RunRoute();
            }

            json = JsonSerializer.Serialize(Run.Route);
        }
    }
}
