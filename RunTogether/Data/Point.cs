using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RunTogether.Data
{
    public abstract class Point
    {

        ValueTuple<float, float> Coordinates { get; set; }

        public int StageId { get; set; }

        public Stage Stage { get; set; }

    }

    public class StartPoint : Point
    {
        public int StartPointId { get; set; }
    }

    public class EndPoint : Point
    {
        public int EndPointId { get; set; }
    }

    public class ThroughPoint : Point
    {
        public int ThroughPointId { get; set; }
    }

}
 