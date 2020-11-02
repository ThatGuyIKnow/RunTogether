using Radzen.Blazor;
using RunTogether.Data;
using System;
using System.Collections.Generic;

namespace RunTogether
{
    public class RunRoute
    {
        public int RunRouteId { get; set; }
        public List<Stage> Stages { get; set; }
        
        public int RunId { get; set; }

        public Run Run { get; set; }

        public override string ToString()
        {
            return "This here sure is a route";
            //return $"Route with {Stages.Count} stages";
        }

        public List<List<float>> ToPointList()
        {
            List<List<float>> PointList = new List<List<float>>(); 
            foreach (Stage stage in this.Stages)
            {
                PointList.Add(new List<float> { stage.StartPoint.Coordinates.X, stage.StartPoint.Coordinates.Y});

                if (stage.ThroughPoints != null)
                {
                    foreach (ThroughPoint point in stage.ThroughPoints)
                    {
                        PointList.Add(new List<float> { point.Coordinates.X, point.Coordinates.Y });
                    }
                }

                PointList.Add(new List<float> { stage.EndPoint.Coordinates.X, stage.EndPoint.Coordinates.Y });

            }
            return PointList;
        }

        //public RunRoute(List<Stage> Stages)
        //{
        //    this.Stages = Stages;
        //}


    }
}