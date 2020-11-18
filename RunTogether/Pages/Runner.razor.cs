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
        private string qrCode = "";
        private string? cookie;
        private Run assignedRun = new Run();
        private Stage activeStage = new Stage();
        private StageAssignment activeRunner = new StageAssignment();
        private ApplicationUser currentUser = new ApplicationUser();

        //Variables for hiding and displaying CSS.
        private const string HideCss = "display-none";
        private string cameraCSS = HideCss;
        private string startRunCSS = HideCss; 
        private string displayResultCSS = HideCss;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await RetrieveUser();
                StateHasChanged();
            }
        }

        private async Task RetrieveUser()
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user.Identity.IsAuthenticated)
            {
                currentUser = await UserManager.GetUserAsync(user);
                assignedRun = await dbContext.Runs
                                    .Where(r => r.ID == currentUser.RunId)
                                    .Include(r => r.Route)
                                    .ThenInclude(rr => rr.Stages)
                                    .ThenInclude(s => s.AssignedRunners)
                                    .ThenInclude(a => a.Runner)
                                    .FirstOrDefaultAsync();

                //Get the codeScanned cookie, and display the relevant CSS elements.
                cookie = await JSRuntime.InvokeAsync<string>("Main.Common.ReadCookie", "CodeScanned");
                if (cookie == null || !cookie.Equals("Yes"))
                {
                    cameraCSS = "";
                }
                else
                {
                    startRunCSS = "";
                }
            }
        }

        public async void CheckCode()
        {
            //Finds the active stage and runner.
            SetActiveStageAndRunner();

            //Checks that the QR-code is correct.
            if (assignedRun.QRString.Equals(qrCode))
            {
                //Sets a cookie that remembers that the code has been scanned.
                await JSRuntime.InvokeAsync<string>("Main.Common.WriteCookie", "CodeScanned", "Yes", 2);

                //Checks if there is an active runner, or if the current user's status is Completed.
                if (activeRunner == null || activeStage.AssignedRunners.Find(a => a.Runner.Id.Equals(currentUser.Id)).Status == RunningStatus.Completed)
                {
                    await JSRuntime.InvokeVoidAsync("alert", "Du er allerede færdig med dit løb.");
                }
                //Sets the previous runner's status to Completed, if they still have a status of Active.
                else if (!activeRunner.Runner.Id.Equals(currentUser.Id))
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
        //public TimeSpan stopWatchValue = new TimeSpan();
        //public bool stopWatchActive = false;
        public CustomStopWatch timer = new CustomStopWatch();
        public async void StartRun()
        {
            SetActiveStageAndRunner();

            var confirmResult = await dialogService.Confirm("Er du sikker?", "Start løb", new ConfirmOptions() 
                                                            { OkButtonText = "Ja", CancelButtonText = "Nej" });

            if (confirmResult.HasValue && confirmResult.Value)
            {
                StartTime();
                buttonVisible = true;
                buttonDisabled = true;

                UpdateDatabase(RunningStatus.Active);
            }
            else
            {
                return;
            }
        }

        public void StopRun()
        {
            //stopWatchActive = false; 
            PauseTime();
            DisplayResult();
            //activeRunner.RunningTime = stopWatchValue;
            activeRunner.RunningTime = timer.stopWatchValue;
            UpdateDatabase(RunningStatus.Completed);
        }

        public async Task StartTime()
        {
            await timer.StartStopWatch();
            //myStopWatch.StartStopWatch(stopWatchActive, stopWatchValue);
        }

        public void PauseTime()
        {
            timer.StopStopWatch();
            //myStopWatch.StopStopWatch(stopWatchActive);
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
