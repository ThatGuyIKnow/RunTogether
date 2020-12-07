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
        public RunRoute? Route { get; set; }

        public bool Markers = false; 

        public string json; 

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
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

                        json = JsonSerializer.Serialize(run.Route.ToJsonSerializableViewer(), typeof(Dictionary<string, object>));

                await JsRunTime.InvokeVoidAsync("Main.Map.addMarkersAndLines", json);
                }
                catch (Exception e) { }

            }
            else
            {
                try
                {
                    if (Route != null)
                    {
                        json = JsonSerializer.Serialize(Route.ToJsonSerializableViewer(), typeof(Dictionary<string, object>));
                    }

                    await JsRunTime.InvokeVoidAsync("Main.Map.addMarkersAndLines", json);
                }
                catch (Exception e) { }

              
            }
        }
    }
}
