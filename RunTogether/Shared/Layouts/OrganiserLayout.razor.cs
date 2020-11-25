using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace RunTogether.Shared.Layouts
{
    public partial class OrganiserLayout
    {
        public string runnerStatus = "You are the active runner";

        /* Keeps track of whether sidebar is collapsed or not. By default not collapsed */
        private bool collapsed = false;

        private readonly string sidebarId = "navBarOrgId";

        private async Task ToggleSidebar()
        {
            collapsed = !collapsed;

            if (collapsed == false) /* Open */
            {
                await JSRuntime.InvokeVoidAsync("Main.Sidebar.sidebarToggle",
                    new { id = sidebarId, attribute = "width", value = "15rem" });
            }
            else /* Close */
            {
                await JSRuntime.InvokeVoidAsync("Main.Sidebar.sidebarToggle",
                    new { id = sidebarId, attribute = "width", value = "0" });
            }
        }

        public string ID { get; set; }

        protected override void OnParametersSet()
        {
            // pull out the "ID" parameter from the route data
            object id = null;
            if ((this.Body.Target as RouteView)?.RouteData.RouteValues?.TryGetValue("id", out id) == true)
            {
                ID = id?.ToString();
            }
        }
    }
}