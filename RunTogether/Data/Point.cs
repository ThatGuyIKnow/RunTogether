using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace RunTogether.Data
{
    public abstract class Point
    {

        public virtual ValueTuple<float, float> Coordinates { get; set; }

        public virtual int StageId { get; set; }

        public virtual Stage Stage { get; set; }

    }

    public class StartPoint : Point
    {
        public int StartPointId { get; set; }
        //public Stage Stage { get; set; }

        public override ValueTuple<float, float> Coordinates { get; set; }

        public override int StageId { get; set; }

        public override Stage Stage { get; set; }
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
 