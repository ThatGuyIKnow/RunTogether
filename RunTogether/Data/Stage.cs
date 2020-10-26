using Radzen.Blazor;
using RunTogether.Data;
using System;
using System.Collections.Generic;
using System.Data;

namespace RunTogether
{
    public class Stage
    {
        public int StageId { get; set; }

        public DateTime Date { get; set; }

        public StartPoint StartPoint { get; set; }

        public EndPoint EndPoint { get; set; }

        public List<ThroughPoint> ThroughPoints { get; set; } 

        public int RunRouteId { get; set; }

        public RunRoute RunRoute { get; set; }

    }
}