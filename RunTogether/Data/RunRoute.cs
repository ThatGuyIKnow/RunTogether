//using Newtonsoft.Json;
using Radzen.Blazor;
using RunTogether.Data;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RunTogether
{
    public class RunRoute
    {
        public int RunRouteId { get; set; }
        public List<Stage> Stages { get; set; } = new List<Stage>();
        
        public int? RunId { get; set; }

        [JsonIgnore]
        public Run? Run { get; set; }

        public override string ToString()
        {
            return RunRouteId.ToString();
            //return $"Route with {Stages.Count} stages";
        }

        public void DeleteStage(ApplicationDbContext dbContext, Stage DeleteStage)
        {
            //Check if delete stage is part of route.... 
            Stage PreviousStage = DeleteStage.GetPreviousStage();

            PreviousStage.EndPoint.X = DeleteStage.EndPoint.X;
            PreviousStage.EndPoint.Y = DeleteStage.EndPoint.Y;

            dbContext.Remove(DeleteStage);
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

        public Dictionary<string, object> ToJsonSerializableViewer()
        {
            Dictionary<string, object> data = new Dictionary<string, object>();

            if (this.Run?.Name != null) data["Name"] = this.Run.Name;
            List<Dictionary<string, object>> serializedStages = new List<Dictionary<string, object>>();

            Stages.ForEach(stage =>
                serializedStages.Add(stage.ToJsonSerializableViewer())
            );
            data["Stages"] = serializedStages;
            return data;
        }

        //public RunRoute(List<Stage> Stages)
        //{
        //    this.Stages = Stages;
        //}


    }
}