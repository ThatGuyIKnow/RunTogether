using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RunTogether.Areas.Identity;
using RunTogether.Areas.Identity.Data;
using RunTogether.Data;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using System.Transactions;

namespace RunTogether.Pages
{
    public partial class Runner
    {
        private int currentCount = 0;
        private string qrCode = "";
        private int? assignedRunId;
        private string? runQrCode = null;

        private const string HideCss = "display-none";
        private string cameraCSS = "";
        private string startRunCSS = HideCss;

        private async Task GetRunQrCode()
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user.Identity.IsAuthenticated)
            {
                var currentUser = await UserManager.GetUserAsync(user);
                assignedRunId = currentUser.RunId;

                Run run = new Run();
                run = dbContext.Runs.Find(assignedRunId);
                runQrCode = run.QRString;
            }
        }

        public void CheckCode()
        {
            if (runQrCode != null && runQrCode.Equals(qrCode))
            {
                cameraCSS = HideCss;
                startRunCSS = "";
            }
        }

        public void StartRun()
        {

        }
    }
}
