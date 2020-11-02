using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace RunTogether.Data
{
    public abstract class Point
    {

        public Vector2 Coordinates;

        public virtual int StageId { get; set; }

        public virtual Stage Stage { get; set; }

        public Point(float x, float y)
        {
            this.Coordinates.X = x;
            this.Coordinates.Y = y; 

        }

    }

    public class StartPoint : Point
    {
        public int StartPointId { get; set; }

        public StartPoint(float x, float y) : base(x, y)
        {

        }
    }

    public class EndPoint : Point
    {
        public int EndPointId { get; set; }

        public EndPoint(float x, float y) : base(x, y)
        {

        }
    }

    public class ThroughPoint : Point
    {
        public int ThroughPointId { get; set; }

        public ThroughPoint(float x, float y) : base(x, y)
        {

        }
    }

}
 