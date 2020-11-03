using Radzen.Blazor;
using RunTogether.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

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

        public RunRoute RunRoute { get; set; } = new RunRoute();

        public Stage(StartPoint startPoint, EndPoint endPoint)
        {
            this.StartPoint = startPoint;
            this.EndPoint = endPoint;
        }

        private Stage()
        {

        }

    }
}