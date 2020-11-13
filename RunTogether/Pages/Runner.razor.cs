using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using Radzen;
using RunTogether.Areas.Identity;
using RunTogether.Areas.Identity.Data;
using RunTogether.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using System.Transactions;

namespace RunTogether.Pages
{
    public partial class Runner
    {
        //Variables for storing information about the current run and runner.
        private int currentCount = 0;
        private string qrCode = "";
        private Run assignedRun = new Run();
        private string runnerName = "";
        private int runnerID;
        private Stage activeStage = new Stage();
        private StageAssignment activeRunner = new StageAssignment();

        //Variables for hiding and displaying CSS.
        private const string HideCss = "display-none";
        private string cameraCSS = "";
        private string startRunCSS = HideCss; 
        private string displayResultCSS = HideCss;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
                var user = authState.User;

                if (user.Identity.IsAuthenticated)
                {
                    var currentUser = await UserManager.GetUserAsync(user);
                    assignedRun = await dbContext.Runs
                                        .Where(r => r.ID == currentUser.RunId)
                                        .Include(r => r.Route)
                                        .ThenInclude(rr => rr.Stages)
                                        .ThenInclude(s => s.AssignedRunners)
                                        .ThenInclude(a => a.Runner)
                                        .FirstOrDefaultAsync();

                    runnerName = currentUser.FirstName;
                    runnerID = currentUser.RunnerId;
                }
                StateHasChanged();
            }
        }

        public async void CheckCode()
        {
            //Finds the active stage and runner.
            SetActiveStageAndRunner();

            //Checks that the QR-code is correct.
            if (assignedRun.QRString.Equals(qrCode))
            {
                //Sets the previous runner's status to Completed, if they still have a status of Active.
                if (activeRunner.RunnerId != runnerID)
                {
                    UpdateDatabase(RunningStatus.Completed);
                }

                //Hides the camera CSS and displays the start run CSS.
                cameraCSS = HideCss;
                startRunCSS = "";
            }
            else
            {
                await JSRuntime.InvokeVoidAsync("alert", "QR-koden er ikke gyldig.");
            }
        }

        private bool buttonVisible = false;
        private bool buttonDisabled = false;
        public async void StartRun()
        {
            SetActiveStageAndRunner();

            var confirmResult = await dialogService.Confirm("Er du sikker?", "Start løb", new ConfirmOptions() 
                                                            { OkButtonText = "Ja", CancelButtonText = "Nej" });

            if (confirmResult.HasValue && confirmResult.Value)
            {
                StopWatch();
                buttonVisible = true;
                buttonDisabled = true;

                UpdateDatabase(RunningStatus.Active);
            }
            else
            {
                return;
            }
        }

        TimeSpan stopWatchValue = new TimeSpan();
        private bool stopWatchActive = false;

        public async Task StopWatch()
        {
            stopWatchActive = true;
            while (stopWatchActive)
            {
                await Task.Delay(1000);
 
                if (stopWatchActive)
                {
                    //Because of the Task.Delay, chances are that when the “Stop” button is clicked, the cycle has already started.
                    //This means that if we do not check for the Boolean value it will add another second to the already reset variable.
                    stopWatchValue = stopWatchValue.Add(new TimeSpan(0, 0, 1));
                    StateHasChanged();
                }
            }
        }

        public void DisplayResult()
        {
            startRunCSS = HideCss;
            displayResultCSS = "";
        }

        public async void UpdateDatabase (RunningStatus runningStatus)
        {
            activeRunner.Status = runningStatus;
            await dbContext.SaveChangesAsync();
        }

        public void SetActiveStageAndRunner()
        {
            activeStage = assignedRun.GetCurrentStage();
            activeRunner = activeStage.GetCurrentRunner();
        }
    }
}
