using Radzen.Blazor;
using RunTogether.Data;
using System;
using System.Collections.Generic;

namespace RunTogether
{
    public class RunRoute
    {
        public int RunRouteId { get; set; }
        public List<Stage> Stages { get; set; } //= new List<Stage>();
        
        public int? RunId { get; set; }

        public Run? Run { get; set; }

        //public RunRoute()
        //{
        //    Stages = new List<Stage>();
        //}

        public override string ToString()
        {
            return RunRouteId.ToString();
            //return $"Route with {Stages.Count} stages";
        }

        public List<List<float>> ToPointList()
        {
            List<List<float>> PointList = new List<List<float>>(); 
            foreach (Stage stage in this.Stages)
            {
                PointList.Add(new List<float> { stage.StartPoint.X, stage.StartPoint.Y});

                if (stage.ThroughPoints != null)
                {
                    foreach (ThroughPoint point in stage.ThroughPoints)
                    {
                        PointList.Add(new List<float> { point.X, point.Y });
                    }
                }

                PointList.Add(new List<float> { stage.EndPoint.X, stage.EndPoint.Y });

            }
            return PointList;
        }

        //public RunRoute(List<Stage> Stages)
        //{
        //    this.Stages = Stages;
        //}


    }
}