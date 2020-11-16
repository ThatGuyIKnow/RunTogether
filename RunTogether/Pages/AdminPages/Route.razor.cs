using Microsoft.AspNetCore.Identity;
using RunTogether.Shared.Forms;
using RunTogether.Areas.Identity;
using System.Collections.Generic;
using RunTogether.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Components;
using Radzen.Blazor;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Radzen;
using System;
using System.Linq;

namespace RunTogether.Pages.AdminPages
{
    public partial class Route
    {
        //Id for the selected run
        [Parameter] public int id { get; set; }
        //Id for det valgte stage på ruten
        [Parameter] public int selectedStageId { get; set; } = -1;

        //variable til overførte stage
        Stage selectedStage = new Stage();

        //Variable som holder mængden af løbere på stage(bruges til at tegne korekt mængde dropdown)
        public int rows { get; set; }

        Run run = new Run();
        StageAssignment AssignmentPlaceholder = new StageAssignment();

        protected override async Task OnInitializedAsync()
        {
            run = dbContext.Runs
                .Where(r => r.ID == id)
                .Include(r => r.Runners)
                .Include(r => r.Route)
                    .ThenInclude(r => r.Stages)
                        .ThenInclude(s => s.StartPoint)
                .Include(r => r.Route)
                    .ThenInclude(r => r.Stages)
                        .ThenInclude(s => s.EndPoint)
                .Include(r => r.Route)
                    .ThenInclude(r => r.Stages)
                        .ThenInclude(s => s.ThroughPoints)
                .Include(r => r.Route)
                    .ThenInclude(r => r.Stages)
                        .ThenInclude(s => s.AssignedRunners)
                .FirstOrDefault();

            //indlæser listen af løbere til det valgte løb
            loadRunnerList();

        }

        //Bliver kørt ved ændring af værdi i dropdown box
        //Ændre stageAssignment på index til vlagte værdi
        public async Task Change(ApplicationUser value, int index)
        {

            selectedStage.AssignedRunners[index].Order = index;
            selectedStage.AssignedRunners[index].Runner = value;
            selectedStage.AssignedRunners[index].RunnerId = value.RunnerId;
            selectedStage.AssignedRunners[index].Stage = selectedStage;
            selectedStage.AssignedRunners[index].StageId = selectedStage.StageId;
        }

        //gemmer ændringer i databasen ved klik på gem knap
        public async void Save() 
        {
            await dbContext.SaveChangesAsync();
        }

        //Sletter valgte løber fra stage og ændre Order nummer til at matche index
        public void Delete(int index)
        {
            StageAssignment runnerToDel = selectedStage.AssignedRunners.Find(x => x.Order == index);
            selectedStage.AssignedRunners.Remove(runnerToDel);
           
            for (int i = 0; i < selectedStage.AssignedRunners.Count ; i++)
            {
                selectedStage.AssignedRunners[i].Order = i;
            }

            StateHasChanged();
            
            rows--;
        }

        //tilføjer nu stageAssignment tom til AssignedRunners 
        public void Add()
        {
            selectedStage.AssignedRunners.Add(new StageAssignment());
            rows++;
        }

        void reciveFromChild(int stageId)
        {
            selectedStageId = stageId;
            loadRunnerList();
            StateHasChanged();
        }

        void loadRunnerList()
        {
            if (selectedStageId != -1)
            {

                selectedStage = run.Route.Stages
                    .Where(s => s.StageId == selectedStageId)
                    .FirstOrDefault();

                //sortere listen af assignedRunners på Order nr. lille til stor
                selectedStage.AssignedRunners.OrderBy(o => o.Order).ToList();

                //tæler mængden af løbere på vlagte stage
                rows = selectedStage.AssignedRunners.Count;

                //Hvis nul løbere på valgte stage lav ny tom løber og tæl rows op
                if (rows == 0)
                {
                    selectedStage.AssignedRunners.Add(new StageAssignment());
                    rows++;
                }
            }
        }

    }
}
