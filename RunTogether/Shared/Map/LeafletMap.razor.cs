using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using RunTogether.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;

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

                try
                {
                    Run run = dbContext.Runs
                        .Where(r => r.Active)
                        .Include(r => r.Route)
                        .ThenInclude(rr => rr.Stages)
                        .ThenInclude(s => s.AssignedRunners)
                        .ThenInclude(a => a.Runner)
                        .Include(r => r.Route)
                        .ThenInclude(rr => rr.Stages)
                        .ThenInclude(s => s.StartPoint)
                        .Include(r => r.Route)
                        .ThenInclude(rr => rr.Stages)
                        .ThenInclude(s => s.EndPoint)
                        .Include(r => r.Route)
                        .ThenInclude(rr => rr.Stages)
                        .ThenInclude(s => s.ThroughPoints)
                        .Include(r => r.Route)
                        .ThenInclude(rr => rr.Stages)
                        .ThenInclude(s => s.Sponsor)
                        .First();

                    //var data = run;
                    json = JsonSerializer.Serialize(run.Route.ToJsonSerializableViewer(), typeof(Dictionary<string, object>));
                    await JsRunTime.InvokeVoidAsync("Main.Map.addMarkersAndLines", json);
                }
                catch (Exception e) { }

                StateHasChanged();
            }

            /*            await JsRunTime.InvokeVoidAsync("Main.Map.addMarkersAndLines");
            */

        }


/*        private async void button_add()
        {
            //json = JsonConvert.SerializeObject(new { Coordinates = Route.ToPointList() });

            //await JsRunTime.InvokeVoidAsync("Main.Map.addMarkersAndLines", json);
        }


        private async void button_remove()
        {
            await JsRunTime.InvokeVoidAsync("Main.Map.removeMarkersAndLines");
        }
*/


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
