using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace RunTogether.Shared.Layouts
{
    public partial class MainLayout
    {
        /* Keeps track of whether sidebar is collapsed or not. By default not collapsed */
        bool collapsed = false;

        readonly string sidebarId = "navBarId";

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