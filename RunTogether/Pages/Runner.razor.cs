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
        private string? codeCookie;
        private string? startRunCookie;
        private Run assignedRun = new Run();
        private Stage activeStage = new Stage();
        private StageAssignment? activeRunner;
        private ApplicationUser currentUser = new ApplicationUser();
        public CustomStopWatch timer = new CustomStopWatch();

        //Variables for hiding and displaying CSS.
        private const string HideCss = "display-none";
        private string cameraCSS = HideCss;
        private string startRunCSS = HideCss; 
        private string displayResultCSS = HideCss;
        private bool buttonVisible = false;
        private bool buttonDisabled = false;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await RetrieveUser();
            }
        }

        protected override void OnParametersSet()
        {
           
        }

        private async Task RetrieveUser()
        {
            Microsoft.AspNetCore.Components.Authorization.AuthenticationState? authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            System.Security.Claims.ClaimsPrincipal? user = authState.User;

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

                await CheckCookie();
            }

            StateHasChanged();
        }

        public async Task<bool> ValidateRunner()
        {
            //Finds the active stage and runner.
            SetActiveStageAndRunner();

            List<StageAssignment> notCompletedStageAssignments;
            try
            {
                notCompletedStageAssignments = currentUser.StageAssignments.FindAll(s => s.Status != RunningStatus.Completed).OrderBy(s => s.Stage.StageId).ToList();
            }
            catch (Exception)
            {
                notCompletedStageAssignments = new List<StageAssignment>();
            }

            //Checks if there is an active stage, and updates the stage status if necessary.
            if (activeStage == null)
            {
                try
                {
                    IEnumerable<StageAssignment> runnersStageAssignments = currentUser.StageAssignments.FindAll(s => s.Stage.Status != RunningStatus.Completed).OrderBy(s => s.StageId);
                    runnersStageAssignments.First().Stage.Status = RunningStatus.Active;
                    Stage previousStage = runnersStageAssignments.First().Stage.GetPreviousStage();
                    if (previousStage.StageId != runnersStageAssignments.First().StageId)
                    {
                        previousStage.Status = RunningStatus.Completed;
                    }
                    await dbContext.SaveChangesAsync();
                }
                catch (Exception)
                {
                    await JSRuntime.InvokeVoidAsync("alert", "Du er allerede færdig med dit løb.");
                }
                
                //if (runnersStageAssignments != null)
                //{
                //    runnersStageAssignments.First().Stage.Status = RunningStatus.Active;
                //    Stage previousStage = runnersStageAssignments.First().Stage.GetPreviousStage();
                //    if (previousStage.StageId != runnersStageAssignments.First().StageId)
                //    {
                //        previousStage.Status = RunningStatus.Completed;
                //    }
                //    await dbContext.SaveChangesAsync();
                //}
                //else
                //{
                //    await JSRuntime.InvokeVoidAsync("alert", "Du er allerede færdig med dit løb.");
                //}
            }
            else if (notCompletedStageAssignments.Count > 0)
            {
                notCompletedStageAssignments[0].Status = RunningStatus.Active;
                foreach (StageAssignment stageAssignment in notCompletedStageAssignments[0].Stage.AssignedRunners)
                {
                    if (stageAssignment.Order < notCompletedStageAssignments[0].Order)
                    {
                        stageAssignment.Status = RunningStatus.Completed;
                    }
                }

                foreach (Stage stage in assignedRun.Route.Stages)
                {
                    if (stage.StageId < notCompletedStageAssignments[0].StageId)
                    {
                        stage.Status = RunningStatus.Completed;
                        foreach (StageAssignment stageAssignment in stage.AssignedRunners)
                        {
                            stageAssignment.Status = RunningStatus.Completed;
                        }
                    }
                }
                await dbContext.SaveChangesAsync();
            }
            else
            {
                await JSRuntime.InvokeVoidAsync("alert", "Du er allerede færdig med dit løb.");
                return false;
            }
            //Sets the previous runner's status to Completed, if they still have a status of Active.
            //else if (!activeRunner.Runner.Id.Equals(currentUser.Id))
            //{
            //    await UpdateDatabase(RunningStatus.Completed);
            //}

            return true;
        }

        public async Task CheckCookie()
        {
            //Get the cookies, and display the relevant CSS elements.
            codeCookie = await JSRuntime.InvokeAsync<string>("Main.Common.ReadCookie", "CodeScanned");
            startRunCookie = await JSRuntime.InvokeAsync<string>("Main.Common.ReadCookie", "RunStarted");
            if (codeCookie == null || !codeCookie.Equals("Yes"))
            {
                await ValidateRunner();
                cameraCSS = "";
            }
            else if (startRunCookie == null || !startRunCookie.Equals("Yes"))
            {
                startRunCSS = "";
            }
            else
            {
                startRunCSS = "";
                StartRun("HasCookie");
            }
        }

        public async void CheckCode()
        {
            //Checks that the QR-code is correct.
            if (assignedRun.QRString.Equals(qrCode))
            {
                //Sets a cookie that remembers that the code has been scanned.
                await JSRuntime.InvokeAsync<string>("Main.Common.WriteCookie", "CodeScanned", "Yes", 2);

                await ValidateRunner();

                //Hides the camera CSS and displays the start run CSS.
                cameraCSS = HideCss;
                startRunCSS = "";

                StateHasChanged();
            }
            else
            {
                await JSRuntime.InvokeVoidAsync("alert", "QR-koden er ikke gyldig.");
            }
        }

        public async void StartRun(string? hasCookie = null)
        {
            SetActiveStageAndRunner();

            bool? confirmResult = false;

            if (hasCookie == null)
            {
                confirmResult = await dialogService.Confirm("Er du sikker?", "Start løb", new ConfirmOptions()
                                                           { OkButtonText = "Ja", CancelButtonText = "Nej" });
            }

            if ((confirmResult.HasValue && confirmResult.Value) || hasCookie != null)
            {
                await JSRuntime.InvokeAsync<string>("Main.Common.WriteCookie", "RunStarted", "Yes", 2);
                await StartTime();
                buttonVisible = true;
                buttonDisabled = true;
                await UpdateDatabase(RunningStatus.Active);
            }
            else
            {
                return;
            }

            StateHasChanged(); 
        }

        public async void StopRun()
        {
            StopTime();
            DisplayResult();
            activeRunner.RunningTime = timer.stopWatchValue;
            await UpdateDatabase(RunningStatus.Completed);
            if (activeStage.GetLastRunner().Id == activeRunner.Id)
            {
                activeStage.Status = RunningStatus.Completed;
                Stage nextStage = activeStage.GetNextStage();
                if (nextStage.StageId != activeStage.StageId)
                {
                    nextStage.Status = RunningStatus.Active;
                }
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task StartTime()
        {
            timer.StartStopWatch(() => { StateHasChanged(); }, activeRunner.StartTime);
            activeRunner.StartTime = timer.startTime;
            await dbContext.SaveChangesAsync();
        }

        public void StopTime()
        {
            timer.StopStopWatch();
        }

        public void DisplayResult()
        {
            startRunCSS = HideCss;
            displayResultCSS = "";
        }

        public async Task UpdateDatabase (RunningStatus runningStatus)
        {
            activeRunner.Status = runningStatus;
            await dbContext.SaveChangesAsync();
        }

        public void SetActiveStageAndRunner()
        {
            activeStage = assignedRun.GetCurrentStage();
            if (activeStage != null)
            {
                activeRunner = activeStage.GetCurrentRunner();
            }
        }
    }
}
