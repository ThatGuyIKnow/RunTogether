using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RunTogether.Areas.Identity;
using RunTogether.Areas.Identity.Data;
using RunTogether.Data;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using System.Transactions;

namespace RunTogether.Pages
{
    public partial class Runner
    {
        private int currentCount = 0;
        private string qrCode = "";
        private Run assignedRun = new Run();
        private List<Stage> assignedStages = new List<Stage>();

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
                assignedRun = currentUser.Run;
                assignedStages = currentUser.Stages;
            }
        }

        public void CheckCode()
        { 
            if (assignedRun.QRString.Equals(qrCode))
            {
                //cameraCSS = HideCss;
                //startRunCSS = "";
            }
        }

        public void StartRun()
        {
            Stage activeStage = new Stage();
            activeStage = assignedRun.GetCurrentStage();

            foreach (Stage stage in assignedStages)
            {
                if (stage.StageId == activeStage.StageId)
                {
                    //pop-up med confirm at løbet skal starte
                    //start timer
                    //skift til at vise stop løb og current time(?)
                }
            }
        }
    }
}
