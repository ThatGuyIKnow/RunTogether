using Radzen.Blazor;
using RunTogether.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using RunTogether.Areas.Identity;
using RunTogether.Pages;

namespace RunTogether
{
    public class Stage
    {
        public int StageId { get; set; }

        public DateTime Date { get; set; }

        public StartPoint? StartPoint { get; set; }

        public EndPoint? EndPoint { get; set; }

        public List<ThroughPoint> ThroughPoints { get; set; } = new List<ThroughPoint>();

        public int RunRouteId { get; set; } 

        public RunRoute RunRoute { get; set; }

        public List<StageAssignment> AssignedRunners { get; set; } = new List<StageAssignment>();
        public RunningStatus status { get; set; } = RunningStatus.NotStarted;

    }


    public class StageAssignment
    {
        public int Id { get; set; }
        public int Order { get; set; } = 0;
        
        public ApplicationUser Runner { get; set; }
        public int RunnerId { get; set; }

        public Stage Stage { get; set; }
        public int StageId { get; set; }

        public RunningStatus Status { get; set; } = RunningStatus.NotStarted;
    }

    public enum RunningStatus
    {
        NotStarted,
        Active,
        Completed
    }
}