using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace RunTogether.Shared.Layouts
{
    public partial class RunnerLayout
    {
        public string runnerStatus = "You are the active runner";

        /* Keeps track of whether sidebar is collapsed or not. By default not collapsed */
        private bool collapsed = false;

        private readonly string sidebarId = "navBarId";

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
    }
}