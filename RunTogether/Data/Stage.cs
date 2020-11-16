﻿using Radzen.Blazor;
using RunTogether.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using RunTogether.Areas.Identity;
using RunTogether.Pages;
//using Newtonsoft.Json;
using System.Text.Json.Serialization;

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

        [JsonIgnore]
        public RunRoute RunRoute { get; set; }

        public List<StageAssignment> AssignedRunners { get; set; } = new List<StageAssignment>();
        public RunningStatus Status { get; set; } = RunningStatus.NotStarted;



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

            return data;
        }
    }


    public class StageAssignment
    {
        public int Id { get; set; }
        public int Order { get; set; } = 0;
        
        public ApplicationUser Runner { get; set; }
        public int RunnerId { get; set; }
        [JsonIgnore]
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