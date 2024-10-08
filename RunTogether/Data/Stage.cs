﻿using Radzen.Blazor;
using RunTogether.Areas.Identity;
using RunTogether.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using RunTogether.Areas.Identity;
using RunTogether.Pages;
using System.Linq;

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
        public RunningStatus Status { get; set; } = RunningStatus.NotStarted;

        public Sponsor? Sponsor { get; set; }

        public int? SponsorId { get; set; }

        public string? Message { get; set; }

        public Dictionary<string, object> ToJsonSerializableViewer()
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data["Status"] = Enum.GetName(typeof(RunningStatus), Status) ?? string.Empty;

            if (StartPoint != null)
                data["StartPoint"] = new List<float>(2) { StartPoint.X, StartPoint.Y };
            if (EndPoint != null)
                data["EndPoint"] = new List<float>(2) { EndPoint.X, EndPoint.Y };
            
            List<List<float>> throughPoints = new List<List<float>>();
            ThroughPoints.ForEach(point => 
                throughPoints.Add(new List<float>(2){point.X, point.Y})
                );
            data["ThroughPoints"] = throughPoints;

            List<Dictionary<string, object>> serializedRunners = new List<Dictionary<string, object>>();
            AssignedRunners.ForEach(runner =>
            {
                serializedRunners.Add(runner.ToJsonSerializableViewer());
            });
            data["Runners"] = serializedRunners;

            data["Sponsor"] = Sponsor?.ToJsonSerializableViewer();
            if(data["Sponsor"] != null)
            {
                var k = data;
                ((Dictionary<string, object>) data["Sponsor"]).Add("Message", Message ?? "");
            }
            data["StageId"] = StageId;
            
            return data;
        }

        public StageAssignment GetCurrentRunner()
        {
            List<StageAssignment> orderedRunners = AssignedRunners.OrderBy(a => a.Order).ToList();

            if (orderedRunners.Exists(o => o.Status == RunningStatus.Active))
            {
                return orderedRunners.Find(o => o.Status == RunningStatus.Active);
            }
            else
            {
                return orderedRunners.Find(o => o.Status == RunningStatus.NotStarted);
            }
        }

        public StageAssignment GetLastRunner()
        {
            return AssignedRunners.OrderBy(a => a.Order).ToList().Last();
        }

        public Stage GetPreviousStage()
        {
            int CurrentIndex = this.RunRoute.Stages.FindIndex(s => s.StageId == this.StageId);
            int PreviousIndex = CurrentIndex < 1 ? CurrentIndex : CurrentIndex - 1;
            return this.RunRoute.Stages[PreviousIndex];
        }

        public Stage GetNextStage()
        {
            int CurrentIndex = RunRoute.Stages.FindIndex(s => s.StageId == StageId);
            int LastIndex = RunRoute.Stages.Count - 1;
            int NextIndex = CurrentIndex == LastIndex ? CurrentIndex : CurrentIndex + 1;
            return RunRoute.Stages[NextIndex];
        }
    }

    public class StageAssignment
    {
        public int Id { get; set; }
        public int Order { get; set; } = 0;
        
        public ApplicationUser Runner { get; set; }
        public int RunnerId { get; set; }
        public TimeSpan? RunningTime { get; set; }
        public TimeSpan? StartTime { get; set; }

        public Stage Stage { get; set; }
        public int StageId { get; set; }

        public RunningStatus Status { get; set; } = RunningStatus.NotStarted;

        public Dictionary<string, object> ToJsonSerializableViewer()
        {
            return new Dictionary<string, object>()
            {
                {"Status", Enum.GetName(typeof(RunningStatus), Status) ?? string.Empty},
                {"Name", Runner.FirstName},
                {"Order", Order}
            };
        }
    }

    public enum RunningStatus
    {
        NotStarted,
        Active,
        Completed
    }

    
}