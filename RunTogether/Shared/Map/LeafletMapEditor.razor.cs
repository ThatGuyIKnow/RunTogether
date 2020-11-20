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

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                

                //event called from JS
                Handler.AddHandler("AddStage", (evt) => {
                    //Deserialize and cast to a Stage object. 
                    Stage NewStage = JsonSerializer.Deserialize<Stage>(evt);

                    Run.Route.Stages.Add(NewStage);

                    dbContext.SaveChanges();

                    JsRunTime.InvokeVoidAsync("Main.MapEditor.loadRoute", Run.Route.ToJsonSerializableViewer());
                });


                //event called from JS
                Handler.AddHandler("EditStage", (evt) => {
                    //Deserialize and cast to a Stage object. 
                    Stage NewStage = JsonSerializer.Deserialize<Stage>(evt);

                    Stage EditStage = dbContext.Stages.Where(s => s.StageId == NewStage.StageId).FirstOrDefault();

                    EditStage.StartPoint = NewStage.StartPoint;
                    EditStage.EndPoint = NewStage.EndPoint;

                    dbContext.SaveChanges();

                    JsRunTime.InvokeVoidAsync("Main.MapEditor.loadRoute", Run.Route.ToJsonSerializableViewer());
                });

                //event called from JS
                Handler.AddHandler("SendStageId", (evt) => {
                    //Deserialize and cast to a Stage object. 
                    Stage NewStage = JsonSerializer.Deserialize<Stage>(evt);

                    Console.WriteLine(NewStage.StageId);
                    sendToParent.InvokeAsync(NewStage.StageId);
                });

                await JsRunTime.InvokeVoidAsync("Main.MapEditor.initializeMap", Handler.ObjRef);


                //if run has no route, add one
                if (Run.Route == null)
                {
                    Run.Route = new RunRoute() { Stages = new List<Stage>() };
                    //db save is needed for ToJsonSerializableViewer to find the new route's run
                    await dbContext.SaveChangesAsync();
                }

                await JsRunTime.InvokeVoidAsync("Main.MapEditor.loadRoute", Run.Route.ToJsonSerializableViewer());

                //StateHasChanged();
                
            }

        }

        protected override void OnParametersSet()
        {
            

        }
    }
}
