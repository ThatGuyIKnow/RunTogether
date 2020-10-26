using System.Collections.Generic;

namespace RunTogether
{
    public class RunRoute
    {
        public List<Stage> Stages { get; set; }
        
        public int RunId { get; set; }

        public Run Run { get; set; }

        public override string ToString()
        {
            return "This here sure is a route";
            //return $"Route with {Stages.Count} stages";
        }

        //public RunRoute(List<Stage> Stages)
        //{
        //    this.Stages = Stages;
        //}


    }
}