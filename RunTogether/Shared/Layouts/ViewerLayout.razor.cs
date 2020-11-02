using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace RunTogether.Shared.Layouts
{
    public partial class ViewerLayout
    {
        /* Keeps track of whether sidebar is collapsed or not. By default not collapsed */
        bool collapsed = false;

        readonly string sidebarId = "sponsorbarId";

        async Task ToggleSidebar()
        {
            collapsed = !collapsed;

            if (collapsed == false) /* Open */
            {
                await JSRuntime.InvokeVoidAsync("Main.sidebar.sidebarToggle",
                    new { id = sidebarId, attribute = "width", value = "15rem" });
            }
            else /* Close */
            {
                await JSRuntime.InvokeVoidAsync("Main.sidebar.sidebarToggle",
                    new { id = sidebarId, attribute = "width", value = "0" });
            }
        }
    }
}