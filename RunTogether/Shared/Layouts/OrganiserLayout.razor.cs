using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace RunTogether.Shared.Layouts
{
    public partial class OrganiserLayout
    {
        public string runnerStatus = "You are the active runner";

        /* Keeps track of whether sidebar is collapsed or not. By default not collapsed */
        bool collapsed = false;

        readonly string sidebarId = "navBarOrgId";

        async Task ToggleSidebar()
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
    }
}
