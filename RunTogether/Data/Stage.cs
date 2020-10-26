using Radzen.Blazor;
using System;
using System.Collections.Generic;
using System.Data;

namespace RunTogether
{
    public class Stage
    {
        public int StageId { get; set; }

        public DateTime Date { get; set; }

        public Point StartPoint { get; set; }

        public Point EndPoint { get; set; }

        public List<Point> ThroughPoints { get; set; } 

        public int RunRouteId { get; set; }

        public RunRoute RunRoute { get; set; }

    }
}